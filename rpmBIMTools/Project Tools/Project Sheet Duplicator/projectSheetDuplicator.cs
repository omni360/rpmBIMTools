using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.IO;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.Exceptions;

using rpmBIMTools.Create;

namespace rpmBIMTools
{
    [TransactionAttribute(TransactionMode.Manual)]
    /// <summary>
    /// NGB Project sheet duplicator
    /// </summary>
    class projectSheetDuplicator : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;
            string disciplineParameter = "Sub-Discipline";
            string disciplineDefault = "master";
            string serviceRegEx = @"^\w\d{2}";
            string username = new String(Environment.UserName.Where(Char.IsLetter).ToArray());
            username = username.ToUpper().Substring(username.Length - 1, 1) + " " + username.Substring(0, 1) + username.Substring(1, username.Length - 2);

            // Check if the document is a valid NGB Template
            if (doc.isNGBTemplate()) {
                TaskDialog.Show("Project Sheet Duplicator", "Utililty can only be used on a NGB Template");
                return Result.Failed;
            }

            // Collect and any new sheets to calculate the sheet number for sheets being added
            ICollection<ViewSheet> allSheets = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .ToList();

            // Collect all sheets with Sub-Discipline parameter containing the word 'master' and order by sheet number
            ICollection<ViewSheet> sheets = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .Where(v => v.LookupParameter(disciplineParameter).AsString().ToLower().Contains(disciplineDefault))
                .OrderBy(v => v.SheetNumber, Comparer<string>.Default)
                .ToList();

            // Collect all titleBlocks to make sure one in on each sheet or show warnings
            ICollection<FamilyInstance> titleBlocks = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .Cast<FamilyInstance>()
                .Where(fi => fi.Symbol.FamilyName.Contains("NGB Drawing Sheet"))
                .ToList();

            // Collect all legends
            ICollection<Autodesk.Revit.DB.View> legendViews = new FilteredElementCollector(doc)
                .OfClass(typeof(Autodesk.Revit.DB.View))
                .OfCategory(BuiltInCategory.OST_Views)
                .Cast<Autodesk.Revit.DB.View>()
                .Where(v => v.ViewType == ViewType.Legend)
                .ToList();
            
            // Collect all view templates
            ICollection<Autodesk.Revit.DB.View> templateViews = new FilteredElementCollector(doc)
                .OfClass(typeof(Autodesk.Revit.DB.View))
                .Cast<Autodesk.Revit.DB.View>()
                .Where(v => v.IsTemplate && 
                        (v.ViewType == ViewType.FloorPlan || v.ViewType == ViewType.EngineeringPlan ||
                        v.ViewType == ViewType.AreaPlan || v.ViewType == ViewType.CeilingPlan))
                .ToList();

            // Settings Form creation
            projectSheetSettings duplicator = new projectSheetSettings();

            // Populate sheets into TreeView
            TreeNode topNode = duplicator.sheetTree.Nodes.Add("Sheets (to be duplicated)");
            topNode.ImageIndex = 0;
            topNode.SelectedImageIndex = 0;
            topNode.Expand();
            foreach (ViewSheet sheet in sheets) {
                TreeNode sheetNode = topNode.Nodes.Add(sheet.Id.IntegerValue.ToString(), sheet.SheetNumber + " - " + sheet.Name, 1,1);
                if (sheet.GetAllPlacedViews().Count() == 0)
                    sheetNode.Nodes.Add("noViews", "No views on this sheet", 3, 3);
                if (!titleBlocks.Any(fi => fi.OwnerViewId == sheet.Id))
                    sheetNode.Nodes.Add("noTitleblock", "No NGB titleblock on this sheet", 3, 3);
                if (!sheet.IsValidNGBDwgNum())
                    sheetNode.Nodes.Add("noNumber", "Sheet number is not valid", 3, 3);
                if (sheet.GetAllPlacedViews().Count() == 0 ||
                    !titleBlocks.Any(fi => fi.OwnerViewId == sheet.Id) ||
                    !sheet.IsValidNGBDwgNum())
                {
                    sheetNode.ImageIndex = 3;
                    sheetNode.SelectedImageIndex = 3;
                    duplicator.sheetWarnings++;
                }
                foreach (ElementId viewId in sheet.GetAllPlacedViews()) {
                    Autodesk.Revit.DB.View view  = doc.GetElement(viewId) as Autodesk.Revit.DB.View;
                    if (view != null && (view.ViewType == ViewType.FloorPlan || view.ViewType == ViewType.CeilingPlan))
                    {
                        TreeNode viewNode = sheetNode.Nodes.Add(view.Title);
                        viewNode.ImageIndex = 2;
                        viewNode.SelectedImageIndex = 2;
                    }
                }
            }
            if (sheets.Count() == 0)
                topNode.Nodes.Add("noSheets", "No sheets found with 'Master' in the Sub-Discipline", 3, 3);

            // Populate view templates into checkedListBox
            duplicator.templateList.DisplayMember = "Name";
            duplicator.templateList.ValueMember = "Id";
            foreach (Autodesk.Revit.DB.View view in templateViews)
            {
                if (Regex.IsMatch(view.LookupParameter(disciplineParameter).AsString(), serviceRegEx))
                    duplicator.templateList.Items.Add(new ElementItem { Name = view.ViewName, Id = view.Id }, true);
            }

            // Start Duplicator Function
            if (duplicator.ShowDialog() == DialogResult.OK)
            {
                // Create and open waiting window
                ProcessingWindow process = new ProcessingWindow("Project Sheet Duplicator", "Starting Duplication Process", true);
                double step = 100.00 / Convert.ToDouble(duplicator.templateList.CheckedItems.Count * sheets.Count);
                double prc = 0;
                int count = 0;
                int totalCount = duplicator.templateList.CheckedItems.Count * sheets.Count;
                Rectangle rW = uiApp.MainWindowExtents;
                process.Location = new System.Drawing.Point(
                    rW.Left + ((rW.Right - rW.Left) - process.Width) / 2,
                    rW.Top + ((rW.Bottom - rW.Top) - process.Height) / 2
                    );
                process.Show();
                Application.DoEvents();

                // Loop through the view templates to copy to
                foreach (object itemChecked in duplicator.templateList.CheckedItems)
                {
                    // Template Checks
                    ElementItem templateItem = itemChecked as ElementItem;
                    if (templateItem == null) continue;
                    Autodesk.Revit.DB.View templateView = doc.GetElement(templateItem.Id) as Autodesk.Revit.DB.View;
                    if (templateView == null) continue;
                    string templateDiscipline = templateView.LookupParameter(disciplineParameter).AsString();
                    string templateDisciplineCode = templateDiscipline[0] + "-" + templateDiscipline.Substring(1, 2);

                        using (Transaction t = new Transaction(doc, "Creating Sheets for " + templateDiscipline))
                        {
                            t.Start();

                            // Loop through the sheets required to be created
                            foreach (ViewSheet sheet in sheets)
                            {
                                // ViewSheet Checks
                                if (sheet.GetAllPlacedViews().Count() == 0 ||
                                    !titleBlocks.Any(fi => fi.OwnerViewId == sheet.Id) ||
                                    !sheet.IsValidNGBDwgNum())
                                {
                                    count += 1;
                                    prc += step;
                                    continue;
                                }

                                // Get titleBlock used on sheet
                                FamilyInstance titleBlock = titleBlocks.FirstOrDefault(fi => fi.OwnerViewId == sheet.Id);

                                // Get sheet parameters for future use
                                string newSheetCode = sheet.SheetNumber.Substring(0, 9) + templateDisciplineCode;
                                int newSheetCount = 0;
                                for (int i = 1; i < 100; i++)
                                {
                                    if (allSheets.Any(v => v.SheetNumber == newSheetCode + i.ToString("00"))) continue;
                                    newSheetCount = i;
                                    break;
                                }
                                string newSheetNumber = newSheetCode + newSheetCount.ToString("00");
                                string service = Get_ServiceName(templateDiscipline);
                                string level = Get_Level(newSheetCode.ToUpper());
                                string zone = Get_Zone(newSheetCode.ToUpper());

                                // Updating process window
                                process.Update(Convert.ToInt32(prc += step), "Duplicating Sheet " + (count += 1) + " of " + totalCount + " (" + Convert.ToInt32(prc) + "%)\n\n" + sheet.SheetNumber + " => " + newSheetNumber);

                                // Create new sheet and set parameters
                                ViewSheet newSheet = ViewSheet.Create(doc, titleBlock.GetTypeId());
                                newSheet.LookupParameter(disciplineParameter).Set(templateDiscipline);
                                newSheet.get_Parameter(BuiltInParameter.SHEET_DRAWN_BY).Set(username);
                                newSheet.get_Parameter(BuiltInParameter.SHEET_ISSUE_DATE).Set(DateTime.Today.ToString("dd/MM/yy"));
                                newSheet.get_Parameter(BuiltInParameter.SHEET_NUMBER).Set(newSheetNumber);
                                newSheet.get_Parameter(BuiltInParameter.SHEET_NAME).Set(Get_DwgNum(newSheetCode.ToUpper(), templateDiscipline, newSheetCount.ToString()));
                                if (newSheet.LookupParameter("STATUS") != null)
                                    newSheet.LookupParameter("STATUS").Set("S3 - For Review & Comment");
                                allSheets.Add(newSheet);

                                // Skip legend creation if no legends exist, revit limitation, cannot create your own legend from api
                                if (duplicator.includeLegends.Checked && legendViews.Count != 0)
                                {
                                    // Select or create legend by discipline
                                    Autodesk.Revit.DB.View legend = legendViews.Where(v => v.ViewName == templateDiscipline).FirstOrDefault();
                                    if (legend == null)
                                    {
                                        legend = doc.GetElement(legendViews.First().Duplicate(ViewDuplicateOption.WithDetailing)) as Autodesk.Revit.DB.View;
                                        legend.ViewName = templateDiscipline;
                                        TextNote noteText = new FilteredElementCollector(doc, legend.Id)
                                            .OfClass(typeof(TextNote))
                                            .Cast<TextNote>()
                                            .FirstOrDefault(lt => lt.TextNoteType.Name == "2mm");
                                        if (noteText != null)
                                            noteText.Text = "Notes:\n\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse laoreet id urna quis maximus. Integer interdum sollicitudin urna, vel gravida magna vestibulum et. Duis aliquam auctor magna, eu lacinia libero pretium id. Morbi nec augue ut urna maximus eleifend vel sit amet erat. Integer id lobortis sapien, sed mollis magna. Mauris nulla leo, consectetur ac sagittis vel, faucibus nec ex. Mauris nec pulvinar lorem. Quisque quam magna, pharetra quis ipsum ut, vestibulum vehicula urna. Aenean neque sem, eleifend eu porttitor quis, commodo quis diam. Aenean egestas id leo ac ultrices. Phasellus sodales augue quis metus vulputate pellentesque.";
                                        doc.Regenerate();
                                        legendViews.Add(legend);
                                    }
                                    Viewport legendViewPort = Viewport.Create(doc, newSheet.Id, legend.Id, XYZ.Zero);
                                    double legendWidth = legendViewPort.GetBoxOutline().MaximumPoint.X - legendViewPort.GetBoxOutline().MinimumPoint.X;
                                    double legendHeight = legendViewPort.GetBoxOutline().MaximumPoint.Y - legendViewPort.GetBoxOutline().MinimumPoint.Y;
                                    // Offset by NGB sheet size
                                    double offsetWidth = titleBlock.Name.Contains("A4") ? -0.052 : titleBlock.Name.Contains("A3") ? -0.052 :
                                        titleBlock.Name.Contains("A2") ? -0.41 : titleBlock.Name.Contains("A1") ? -0.052 : -0.052;
                                    double offsetHeight = titleBlock.Name.Contains("A4") ? 0.39 : titleBlock.Name.Contains("A3") ? 0.59 :
                                        titleBlock.Name.Contains("A2") ? 1.10 : titleBlock.Name.Contains("A1") ? 1.22 : 2.10;
                                    legendViewPort.Location.Move(new XYZ(offsetWidth - (legendWidth / 2), offsetHeight - (legendHeight / 2), 0));
                                }

                                // Get Viewsports / views on sheet
                                foreach (ElementId viewPortId in sheet.GetAllViewports())
                                {
                                    // View Checks, only viewSheet allowed at this time
                                    Viewport viewPort = doc.GetElement(viewPortId) as Viewport;
                                    if (viewPort == null) continue;
                                    Autodesk.Revit.DB.View view = doc.GetElement(viewPort.ViewId) as Autodesk.Revit.DB.View;
                                    if (view == null) continue;
                                    if (view.ViewType != ViewType.FloorPlan) continue;

                                    // Create the host service level / view 
                                    Autodesk.Revit.DB.View serviceLevelView = new FilteredElementCollector(doc)
                                    .OfClass(typeof(Autodesk.Revit.DB.View))
                                    .Cast<Autodesk.Revit.DB.View>()
                                    .FirstOrDefault(v => v.ViewName == service + " - " + level);
                                    if (serviceLevelView == null)
                                    {
                                        if (view.GetPrimaryViewId() != ElementId.InvalidElementId)
                                        {
                                            Autodesk.Revit.DB.View primeView = doc.GetElement(view.GetPrimaryViewId()) as Autodesk.Revit.DB.View;
                                            serviceLevelView = doc.GetElement(primeView.Duplicate(ViewDuplicateOption.WithDetailing)) as Autodesk.Revit.DB.View;
                                        }
                                        else
                                        {
                                            serviceLevelView = ViewPlan.Create(doc, view.GetTypeId(), view.GenLevel.Id);
                                        }
                                        serviceLevelView.ViewName = service + " - " + level;
                                        serviceLevelView.ApplyViewTemplateParameters(templateView);
                                        serviceLevelView.ViewTemplateId = templateView.Id;
                                    }

                                    // Create the AsDependent views per zone / sheet
                                    Autodesk.Revit.DB.View newView = doc.GetElement(serviceLevelView.Duplicate(ViewDuplicateOption.AsDependent)) as Autodesk.Revit.DB.View;
                                    if (view.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP).AsElementId() != null)
                                        newView.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP).Set(view.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP).AsElementId());
                                    string viewName = service + " - " + level + " - " + zone + " - Sheet " + newSheetCount + " - View " + (newSheet.GetAllViewports().Count);
                                    newView.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION).Set(viewName);
                                    newView.CropBox = view.CropBox;
                                    newView.CropBoxActive = view.CropBoxActive;
                                    newView.CropBoxVisible = view.CropBoxVisible;
                                    Viewport newViewPort = Viewport.Create(doc, newSheet.Id, newView.Id, viewPort.GetBoxCenter());
                                    newViewPort.get_Parameter(BuiltInParameter.VIEWER_ANNOTATION_CROP_ACTIVE)
                                        .Set(viewPort.get_Parameter(BuiltInParameter.VIEWER_ANNOTATION_CROP_ACTIVE).AsInteger());
                                    doc.Regenerate();
                                    newViewPort.SetBoxCenter(viewPort.GetBoxCenter());
                                    newView.ViewName = viewName;
                                }
                            }
                            t.Commit();
                        }
                }
                process.Close();
                process.Dispose();
                return Result.Succeeded;
            }
            else
            {
                return Result.Cancelled;
            }
        }

        public string Get_DwgNum(string zoneLevel, string service, string sheetNum)
        {
            string s = "";
            // Service
            string[] serviceNames = service.Split(new string[] { " - " }, StringSplitOptions.None);
            s += serviceNames.Count() == 2 ? serviceNames[1].Trim() : service;
            // Level
            string level = zoneLevel.Substring(3, 2);
            s += level == "XX" ? string.Empty :
                 level == "ZZ" ? ", Multiple Levels" :
                 level == "EX" ? ", External" :
                 level[0] == 'M' && char.IsDigit(level[1]) ? ", Mezzanine " + level[1] :
                 ", Level " + level;
            // Zone
            string zone = zoneLevel.Substring(0, 2);
            s += zone == "ZZ" ? string.Empty :
                 zone[0] == 'Z' && char.IsDigit(zone[1]) ? ", Zone " + zone[1] :
                 zone[0] == 'R' && char.IsDigit(zone[1]) ? ", Riser " + zone[1] :
                 ", Zone " + zone;
            // Sheet Number
            s += ", Sheet " + sheetNum;
            return s;
        }

        public string Get_ServiceName(string service)
        {
            string[] serviceNames = service.Split(new string[] { " - " }, StringSplitOptions.None);
            return serviceNames.Count() == 2 ? serviceNames[1].Trim() : service;
        }

        public string Get_Level(string zoneLevel)
        {
            string level = zoneLevel.Substring(3, 2);
            return level == "XX" ? "All Levels" :
                 level == "ZZ" ? "Multiple Levels" :
                 level == "EX" ? "External" :
                 level[0] == 'M' && char.IsDigit(level[1]) ? "Mezzanine" + level[1] :
                 "Level " + level;
        }

        public string Get_Zone(string zoneLevel)
        {
            string zone = zoneLevel.Substring(0, 2);
            return zone == "ZZ" ? "All Zones" :
                 zone[0] == 'Z' && char.IsDigit(zone[1]) ? "Zone " + zone[1] :
                 zone[0] == 'R' && char.IsDigit(zone[1]) ? "Riser " + zone[1] :
                 "Zone " + zone;
        }
    }
}