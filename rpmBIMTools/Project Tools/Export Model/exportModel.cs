using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.Exceptions;

using rpmBIMTools.Create;

namespace rpmBIMTools
{
    public partial class exportModel : System.Windows.Forms.Form
    {
        // Global Variables
        Autodesk.Revit.DB.Document doc = rpmBIMTools.Load.liveDoc;
        FamilySymbol landingSymbol;
        ViewSheet landingSheet;
        View3D threeDView;
        Autodesk.Revit.DB.View renderedView;
        double ftMM = 1 / 304.8; // Feet to Millimetres
        int purgedViews = 0, purgedSheets = 0, purgedLinks = 0, purgedItems = 0;
        string centralPath = null;

        public exportModel()
        {
            InitializeComponent();
        }

        private void helpRequest(object sender, HelpEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Model-Export");
        }

        private void helpButtonClick(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Model-Export");
            e.Cancel = true;
        }

        private void exportModel_Load(object sender, EventArgs e)
        {
            // Form Load
            if (!string.IsNullOrWhiteSpace(doc.PathName))
            {
                exportTo.Text = Path.GetDirectoryName(doc.PathName);
                folderBrowserDialog.SelectedPath = Path.GetDirectoryName(doc.PathName);
            }
            // Add Default NGB Landing Page Data and select
            exportOptions_LandingPage.DisplayMember = "Name";
            exportOptions_LandingPage.ValueMember = "Id";
            exportOptions_LandingPage.Items.Add (new ElementItem { Name = "NGB Landing Page (Default)", Id = ElementId.InvalidElementId });
            exportOptions_LandingPage.SelectedIndex = 0;
            // Add All Other Sheets to Landing Page Data
            foreach (ViewSheet vs in new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).Cast<ViewSheet>().OrderBy(vs => vs.ViewName, Comparer<string>.Default))
            {
                exportOptions_LandingPage.Items.Add(new ElementItem { Name = vs.ViewName, Id = vs.Id });
            }
            exportOptions_Group_Worksharing.Enabled = doc.IsWorkshared;
            if ((Properties.Settings.Default.exportModel_exportOptions_IncludeViews == 2)) {
                exportOptions_IncludeViewsSelected.Checked = true;
            } else if ((Properties.Settings.Default.exportModel_exportOptions_IncludeViews == 1)) {
                exportOptions_IncludeViewsOnSheets.Checked = true;
            } else {
                exportOptions_IncludeViewsAll.Checked = true;
            }
            addRemoveViewTab(sender, e);
        }

        private void exportToButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                exportTo.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void exportOptions_IncludeSheets_CheckedChanged(object sender, EventArgs e)
        {
            exportOptions_IncludeViewsAll.Enabled = exportOptions_IncludeSheets.Checked;
            exportOptions_IncludeViewsOnSheets.Enabled = exportOptions_IncludeSheets.Checked;
            exportOptions_IncludeViewsSelected.Enabled = exportOptions_IncludeSheets.Checked;
            addRemoveViewTab(sender, e);
        }

        private void exportOptions_IncludeViews_Checked(object sender, EventArgs e)
        {
            Properties.Settings.Default.exportModel_exportOptions_IncludeViews =
                exportOptions_IncludeViewsSelected.Checked ? 2 :
                exportOptions_IncludeViewsOnSheets.Checked ? 1 : 0;
            addRemoveViewTab(sender, e);
        }

        private void addRemoveViewTab(object sener, EventArgs e) {
            if (!exportOptions_IncludeSheets.Checked) {
                purgeViews.Text = "Include Views";
                if (!tabControl.TabPages.Contains(purgeViews)) tabControl.TabPages.Add(purgeViews);
            } else {
                if (exportOptions_IncludeViewsSelected.Checked)
                {
                    purgeViews.Text = "Selected Views";
                    if (!tabControl.TabPages.Contains(purgeViews)) tabControl.TabPages.Add(purgeViews);
                }
                else
                {
                    tabControl.TabPages.Remove(purgeViews);
                }
            }
        }
        
        private void purgeViews_SelectAll_Click(object sender, EventArgs e)
        {
            includeView_3DViews.Checked = true;
            includeView_AreaPlans.Checked = true;
            includeView_CeilingPlans.Checked = true;
            includeView_ColumnSchedules.Checked = true;
            includeView_Details.Checked = true;
            includeView_DraftingViews.Checked = true;
            includeView_Elevations.Checked = true;
            includeView_FloorPlans.Checked = true;
            includeView_Legends.Checked = true;
            includeView_PanelSchedules.Checked = true;
            includeView_Renderings.Checked = true;
            includeView_Schedules.Checked = true;
            includeView_Sections.Checked = true;
            includeView_StructuredPlans.Checked = true;
            includeView_Walkthroughs.Checked = true;
        }

        private void purgeViews_ClearAll_Click(object sender, EventArgs e)
        {
            includeView_3DViews.Checked = false;
            includeView_AreaPlans.Checked = false;
            includeView_CeilingPlans.Checked = false;
            includeView_ColumnSchedules.Checked = false;
            includeView_Details.Checked = false;
            includeView_DraftingViews.Checked = false;
            includeView_Elevations.Checked = false;
            includeView_FloorPlans.Checked = false;
            includeView_Legends.Checked = false;
            includeView_PanelSchedules.Checked = false;
            includeView_Renderings.Checked = false;
            includeView_Schedules.Checked = false;
            includeView_Sections.Checked = false;
            includeView_StructuredPlans.Checked = false;
            includeView_Walkthroughs.Checked = false;
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            // Form checking
            if (string.IsNullOrWhiteSpace(doc.PathName))
            {
                TaskDialog.Show("Export Model", "Project has not been saved, please save before exporting.");
                return;
            }
            if (string.IsNullOrWhiteSpace(doc.ProjectInformation.Number) | string.IsNullOrWhiteSpace(doc.ProjectInformation.Name))
            {
                TaskDialog.Show("Export Model", "Export process requires project name and number to be set.");
                return;
            }
            if (!Directory.Exists(exportTo.Text))
            {
                TaskDialog.Show("Export Model", "Please input a valid export path.");
                return;
            }
            if (doc.IsWorkshared) {
                BasicFileInfo fileInfo = BasicFileInfo.Extract(doc.PathName);
                centralPath = ModelPathUtils.ConvertModelPathToUserVisiblePath(doc.GetWorksharingCentralModelPath());
                if (fileInfo.IsCentral) {
                    TaskDialog.Show("Export Model", "Export process cannot be used on a central model, please create a local copy.");
                    return;
                }
                if (fileInfo.Username != doc.Application.Username)
                {
                    TaskDialog.Show("Export Model", "Local file owner does not match your current user name. Please change your user name or create a new local.");
                    return;
                }
                try
                {
                    if (doc.HasAllChangesFromCentral() == false)
                    {
                        TaskDialog taskDialog = new TaskDialog("Snyc with central before exporting?");
                        taskDialog.MainInstruction = "Local appeared to be out of sync with central.";
                        taskDialog.TitleAutoPrefix = false;
                        taskDialog.MainContent = "Do you want to sync with the central model before doing the export process?";
                        taskDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Sync with central model.");
                        taskDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Skip and continue.");
                        TaskDialogResult tResult = taskDialog.Show();
                        if (tResult == TaskDialogResult.CommandLink1)
                        {
                            ProcessingWindow cWindow = new ProcessingWindow("Exporting Model", "Syncing with Central Model, Please wait...", false);
                            cWindow.Location = new System.Drawing.Point(this.Location.X + (this.Width - cWindow.Width) / 2, this.Location.Y + (this.Height - cWindow.Height) / 2);
                            cWindow.Show();
                            System.Windows.Forms.Application.DoEvents();
                            TransactWithCentralOptions transactOptions = new TransactWithCentralOptions { };
                            SynchronizeWithCentralOptions syncOptions = new SynchronizeWithCentralOptions { };
                            doc.SynchronizeWithCentral(transactOptions, syncOptions);
                            cWindow.Close();
                        }
                    }
                }
                catch (CentralModelException)
                {
                    TaskDialog taskDialog = new TaskDialog("Local File Incompatible with Central Model");
                    taskDialog.TitleAutoPrefix = false;
                    taskDialog.CommonButtons = TaskDialogCommonButtons.Ok;
                    taskDialog.MainInstruction = "Your local file is not compatible with the central model.";
                    taskDialog.MainContent = "This could be due to an upgrade to a newer version of Revit or " +
                        "because the central model was substituted with an unrelated model using the same name or " +
                        "because the central model was restored from a previous version. " +
                        "You must create a new local file and copy/paste any changes made to your original local file into the new model.";
                    taskDialog.Show();
                    return;
                }
            }
            if (doc.IsModified)
            {
                TaskDialog taskDialog =  new TaskDialog("Save project before exporting?");
                taskDialog.MainInstruction = "Project has been modified since last save.";
                taskDialog.TitleAutoPrefix = false;
                taskDialog.MainContent = "Do you want to save the project before doing the export process?";
                taskDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Save changes and continue.");
                taskDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Discard changes and continue.");
                TaskDialogResult tResult = taskDialog.Show();
                if (tResult == TaskDialogResult.CommandLink1)
                {
                    doc.Save();
                }
            }

            // Create processing window
            this.Hide();
            string messageText = "Performing model audit";
            messageText += exportOptions_Audit.Checked ? " and disabling worksharing." : ".";
            ProcessingWindow pWindow = new ProcessingWindow("Exporting Model", messageText, true);
            pWindow.Location = new System.Drawing.Point(this.Location.X + (this.Width - pWindow.Width) / 2, this.Location.Y + (this.Height - pWindow.Height) / 2);
            pWindow.Show();
            System.Windows.Forms.Application.DoEvents();

            // Opened document with or without worksets
            OpenOptions openOptions = new OpenOptions {
                AllowOpeningLocalByWrongUser = true,
                Audit = exportOptions_Audit.Checked ? true : false,
                DetachFromCentralOption = exportOptions_DisableWorksharing.Checked ?
                DetachFromCentralOption.DetachAndDiscardWorksets :
                DetachFromCentralOption.DetachAndPreserveWorksets
            };
            doc = rpmBIMTools.Load.uiApp.Application.OpenDocumentFile(ModelPathUtils.ConvertUserVisiblePathToModelPath(doc.PathName), openOptions);
            string originalFileName = doc.PathName;

            // Update processing window
            pWindow.Update(10, "Prepairing model for exporting.");

            // Search for NGB Landing Page Family
            FamilyInstance landingInstance = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilyInstance))
                        .Cast<FamilyInstance>()
                        .FirstOrDefault(f => f.Name.StartsWith("NGB Landing Page"));

            // Select Landing Page or use default
            if ((exportOptions_LandingPage.SelectedItem as ElementItem).Id == ElementId.InvalidElementId)
            {
                // Find or create landing page and set to active view
                if (landingInstance == null)
                {
                    try
                    {
                        landingSymbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "NGB Landing Page", @"C:\rpmBIM\families\ngbsheets\NGB Landing Page.rfa");
                    }
                    catch (Autodesk.Revit.Exceptions.FileNotFoundException ex)
                    {
                        TaskDialog.Show("Export Model", "Unable to load NGB Landing Page located at:\n\n" + ex.Message);
                        pWindow.Dispose();
                        this.Close();
                        return;
                    }
                    using (Transaction t = new Transaction(doc, "Creating Landing Page"))
                    {
                        t.Start();
                        landingSheet = ViewSheet.Create(doc, landingSymbol.Id);
                        t.Commit();
                    }
                }
                else
                {
                    landingSheet = doc.GetElement(landingInstance.OwnerViewId) as ViewSheet;
                }
            }
            else
            {
                landingSheet = doc.GetElement((exportOptions_LandingPage.SelectedItem as ElementItem).Id) as ViewSheet;
            }

            // Make landing page the active view
            rpmBIMTools.Load.uiApp.ActiveUIDocument.ActiveView = landingSheet;

            // Updating default landing page (if selected)
            if ((exportOptions_LandingPage.SelectedItem as ElementItem).Id == ElementId.InvalidElementId)
            {
                using (Transaction t = new Transaction(doc, "Setting Landing Family Parameters"))
                {
                    t.Start();
                    landingInstance = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilyInstance))
                        .Cast<FamilyInstance>()
                        .FirstOrDefault(f => f.Name.StartsWith("NGB Landing Page"));
                    Parameter customImage = landingInstance.Symbol.LookupParameter("Custom Image");
                    if (customImage != null) customImage.Set(1);
                    t.Commit();
                }
            }
            
            // Create export directory
            string folderName = null;
            folderName += doc.ProjectInformation.Number + "_";
            folderName += doc.ProjectInformation.Name + "_";
            folderName += "NGB_Model_WIP_";
            string rev = !string.IsNullOrWhiteSpace(landingSheet.get_Parameter(BuiltInParameter.SHEET_CURRENT_REVISION).AsString()) ?
                landingSheet.get_Parameter(BuiltInParameter.SHEET_CURRENT_REVISION).AsString() : "-";
            folderName += "Rev_" + rev + "_";
            folderName += DateTime.Now.ToString("yyyy-MM-yy_HH.mm.ss");
            folderName = rpmBIMUtils.GetSafeFilename(folderName);
            DirectoryInfo exportFolder = Directory.CreateDirectory(folderBrowserDialog.SelectedPath + @"\" + folderName);

            // Close all views, but landing sheet
            doc.CloseViews();

            // Update processing window
            pWindow.Update(20, "Prepairing linked model files.");

            // Remove Links
            ICollection<ElementId> ids = ExternalFileUtils.GetAllExternalFileReferences(doc);
            List<ElementId> linksToDelete = new List<ElementId>();
            if (exportOptions_IncludeCADLinks.Checked == false)
            {
                ICollection<ElementId> CADinstances = new FilteredElementCollector(doc)
                    .OfClass(typeof(ImportInstance))
                    .Cast<ImportInstance>()
                    .Where<ImportInstance>(r => (r.IsLinked))
                    .Select<ImportInstance, ElementId>(r => r.Id)
                    .ToList<ElementId>();
                linksToDelete.AddRange(CADinstances);
            }
            foreach (ElementId id in ids)
            {
                ExternalFileReference xr = ExternalFileUtils.GetExternalFileReference(doc, id);
                ModelPath xrPath = xr.GetPath();
                string path = ModelPathUtils.ConvertModelPathToUserVisiblePath(xrPath).ToLower();
                if (xr.ExternalFileReferenceType == ExternalFileReferenceType.RevitLink &
                    exportOptions_IncludeRevitLinks.Checked == false &
                    path.EndsWith(".rvt"))
                {
                    linksToDelete.Add(id); continue;
                }
                if (xr.ExternalFileReferenceType == ExternalFileReferenceType.Decal &
                    exportOptions_IncludeDecals.Checked == false &
                    (path.EndsWith(".bmp") | path.EndsWith(".jpg") | path.EndsWith(".jpeg")
                    |path.EndsWith(".png") | path.EndsWith(".tif")))
                {
                    linksToDelete.Add(id); continue;
                }
                if (xr.ExternalFileReferenceType == ExternalFileReferenceType.DWFMarkup &
                    exportOptions_IncludeDWFLinks.Checked == false)
                {
                    linksToDelete.Add(id); continue;
                }
            }
            using (Transaction t = new Transaction(doc, "Removing External Links"))
            {
                t.Start();
                purgedLinks = linksToDelete.Count();
                doc.Delete(linksToDelete);
                t.Commit();
            }

            // Purge Sheets (all but landing sheet)
            if (!exportOptions_IncludeSheets.Checked)
            {
                // Update processing window
                pWindow.Update(30, "Purging sheets from model.");
                ICollection<ElementId> sheets = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet))
                    .OfCategory(BuiltInCategory.OST_Sheets)
                    .Cast<ViewSheet>()
                    .Where<ViewSheet>(v => v.ViewType == ViewType.DrawingSheet && v.Id != landingSheet.Id)
                    .Select<ViewSheet, ElementId>(v => v.Id)
                    .ToList<ElementId>();
                using (Transaction t = new Transaction(doc, "Purge Sheets"))
                {
                    t.Start();
                    purgedSheets = sheets.Count();
                    doc.Delete(sheets);
                    t.Commit();
                }
            }

            // Purge Views
            if (!exportOptions_IncludeSheets.Checked | (exportOptions_IncludeSheets.Checked & !exportOptions_IncludeViewsAll.Checked))
            {
                // Update processing window
                pWindow.Update(40, "Purging views from model.");
                // Data Collection
                ICollection<Element> views = new FilteredElementCollector(doc)
                    .OfClass(typeof(Autodesk.Revit.DB.View))
                    .Cast<Autodesk.Revit.DB.View>()
                    .Where<Autodesk.Revit.DB.View>(
                    v => v.ViewType != ViewType.ProjectBrowser &
                        v.ViewType != ViewType.SystemBrowser &
                        v.ViewType != ViewType.DrawingSheet &
                        v.OwnerViewId == ElementId.InvalidElementId)
                    .Select<Autodesk.Revit.DB.View, Element>(v => v)
                    .ToList<Element>();
                ICollection<ElementId> sheets = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet))
                    .OfCategory(BuiltInCategory.OST_Sheets)
                    .Cast<ViewSheet>()
                    .Where<ViewSheet>(v => v.ViewType == ViewType.DrawingSheet && v.Id != landingSheet.Id)
                    .Select<ViewSheet, ElementId>(v => v.Id)
                    .ToList<ElementId>();
                ICollection<ElementId> schedules = new FilteredElementCollector(doc)
                    .OfClass(typeof(ScheduleSheetInstance))
                    .ToElementIds();
                List<ElementId> viewsOnSheets = new List<ElementId>();
                List<ElementId> viewsToDelete = new List<ElementId>();

                // Gets all views and schedules that are on sheets for later
                foreach (ElementId sheet in sheets)
                {
                    ViewSheet vs = doc.GetElement(sheet) as ViewSheet;
                    viewsOnSheets.AddRange(vs.GetAllPlacedViews());
                }
                foreach (ElementId schedule in schedules)
                {
                    ScheduleSheetInstance ssi = doc.GetElement(schedule) as ScheduleSheetInstance;
                    if (!viewsOnSheets.Contains(ssi.ScheduleId)) viewsOnSheets.Add(ssi.ScheduleId);
                }

                // Start process
                foreach (Element element in views)
                {
                    Autodesk.Revit.DB.View view = element as Autodesk.Revit.DB.View;

                    // View is on sheet, don't delete (skip to next)
                    if (viewsOnSheets.Contains(view.Id)) continue;

                    // View type selection is off, delete view.
                    if (exportOptions_IncludeSheets.Checked & exportOptions_IncludeViewsOnSheets.Checked)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    
                    // View type selection is on, delete views that are not included
                    if (includeView_FloorPlans.Checked == false & view.ViewType == ViewType.FloorPlan)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_CeilingPlans.Checked == false & view.ViewType == ViewType.CeilingPlan)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_StructuredPlans.Checked == false & view.ViewType == ViewType.EngineeringPlan)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_AreaPlans.Checked == false & view.ViewType == ViewType.AreaPlan)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_Sections.Checked == false & view.ViewType == ViewType.Section)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_Elevations.Checked == false & view.ViewType == ViewType.Elevation)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_Details.Checked == false & view.ViewType == ViewType.Detail)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_DraftingViews.Checked == false & view.ViewType == ViewType.DraftingView)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_3DViews.Checked == false & view.ViewType == ViewType.ThreeD)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_Renderings.Checked == false & view.ViewType == ViewType.Rendering)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_Walkthroughs.Checked == false & view.ViewType == ViewType.Walkthrough)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_Schedules.Checked == false & view.ViewType == ViewType.Schedule)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_PanelSchedules.Checked == false & view.ViewType == ViewType.PanelSchedule)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_ColumnSchedules.Checked == false & view.ViewType == ViewType.ColumnSchedule)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                    if (includeView_Legends.Checked == false & view.ViewType == ViewType.Legend)
                    {
                        viewsToDelete.Add(view.Id); continue;
                    }
                }
                using (Transaction t = new Transaction(doc, "Purge Views"))
                {
                    t.Start();
                    purgedViews = viewsToDelete.Count();
                    doc.Delete(viewsToDelete);
                    t.Commit();
                }
            }

            // Update processing window
            pWindow.Update(50, "Purging unused elements from model.");

            // Purge Elements (WiP)
            purgedItems = doc.PurgeUnused().Count();

            // Update processing window
            pWindow.Update(70, "Updating landing page on model.");

            // Only run if NGB landing page is selected
            if ((exportOptions_LandingPage.SelectedItem as ElementItem).Id == ElementId.InvalidElementId)
            {
                // Create 3D View (deletes old if exists)
                using (Transaction t = new Transaction(doc, "Create 3D View"))
                {
                    t.Start();
                    Element oldExport3DView = rpmBIMUtils.FindElementByName(doc, typeof(View3D), "Landing Page");
                    if (oldExport3DView != null) { doc.Delete(oldExport3DView.Id); }
                    threeDView = View3D.CreateIsometric(doc, doc.GetViewFamilyType(ViewFamily.ThreeDimensional).Id);
                    threeDView.DetailLevel = ViewDetailLevel.Fine;
                    threeDView.DisplayStyle = DisplayStyle.Realistic;
                    threeDView.ViewName = "Landing Page";
                    Parameter subDiscipline = threeDView.LookupParameter("Sub-Discipline");
                    if (subDiscipline != null ? subDiscipline.StorageType == StorageType.String ? true : false : false)
                        subDiscipline.Set("Exported Model");
                    t.Commit();
                }

                // Make 3D View the active view
                rpmBIMTools.Load.uiApp.ActiveUIDocument.ActiveView = threeDView;

                // Create Rendered View of 3D View
                using (Transaction t = new Transaction(doc, "Create Rendered View"))
                {
                    t.Start();
                    Element oldExportRender = rpmBIMUtils.FindElementByName(doc, typeof(ImageView), "Landing Page");
                    if (oldExportRender != null) { doc.Delete(oldExportRender.Id); }
                    ImageExportOptions ExportOptions = new ImageExportOptions
                    {
                        ZoomType = ZoomFitType.FitToPage,
                        ViewName = "Landing Page",
                        PixelSize = 1400,
                        FitDirection = FitDirectionType.Vertical,
                        ImageResolution = ImageResolution.DPI_600
                    };
                    renderedView = doc.GetElement(doc.SaveToProjectAsImage(ExportOptions)) as Autodesk.Revit.DB.View;
                    Parameter subDiscipline = renderedView.LookupParameter("Sub-Discipline");
                    if (subDiscipline != null ? subDiscipline.StorageType == StorageType.String ? true : false : false)
                        subDiscipline.Set("Exported Model");
                    t.Commit();
                }

                // Set Landing Sheet Parameters
                using (Transaction t = new Transaction(doc, "Setting Landing Sheet"))
                {
                    t.Start();
                    XYZ p = (landingInstance.Location as LocationPoint).Point * 304.8;
                    Viewport.Create(doc, landingSheet.Id, renderedView.Id, new XYZ(p.X + 55, p.Y + 30.1, 0) * ftMM);
                    landingSheet.SheetNumber = "###";
                    landingSheet.ViewName = "Landing Page";
                    Parameter subDiscipline = landingSheet.LookupParameter("Sub-Discipline");
                    if (subDiscipline != null ? subDiscipline.StorageType == StorageType.String ? true : false : false)
                        subDiscipline.Set("Exported Model");
                    t.Commit();
                }
            }

            // Close all views, but landing sheet
            doc.CloseViews(landingSheet.Id);
            rpmBIMTools.Load.uiApp.ActiveUIDocument.ActiveView = landingSheet;

            // Update processing window
            pWindow.Update(80, "Exporting files from model.");

            // Copy external files
            ICollection<ElementId> links = ExternalFileUtils.GetAllExternalFileReferences(doc);
            foreach (ElementId link in links)
            {
                DirectoryInfo exportLinkFolder = exportFolder.CreateSubdirectory("Linked Files");
                ExternalFileReference fileRef = ExternalFileUtils.GetExternalFileReference(doc, link);
                string path = ModelPathUtils.ConvertModelPathToUserVisiblePath(fileRef.GetPath());
                string fullpath = ModelPathUtils.ConvertModelPathToUserVisiblePath(fileRef.GetAbsolutePath());
                string file = Path.GetFileName(fullpath);
                string newFileLocation = exportLinkFolder.FullName + @"\" + file;
                if (File.Exists(fullpath))
                {
                    File.Copy(fullpath, newFileLocation);
                    File.SetAttributes(newFileLocation, FileAttributes.Normal);
                }
                else
                {
                    if (centralPath != null & File.Exists(Path.GetDirectoryName(centralPath) + @"\" + path))
                    {
                        File.Copy(Path.GetDirectoryName(centralPath) + @"\" + path, newFileLocation);
                        File.SetAttributes(newFileLocation, FileAttributes.Normal);
                    }
                }
            }

            // Create export report file
            if (exportOptions_IncludeExportReport.Checked)
            {
                if (!File.Exists(exportFolder.FullName + @"\report.txt"))
                {
                    using (StreamWriter report = File.CreateText(exportFolder.FullName + @"\report.txt"))
                    {
                        report.WriteLine("Files exported using Model Exporter Utility by NG Bailey & Autodesk Revit " + rpmBIMTools.Load.uiApp.Application.VersionNumber);
                        report.WriteLine("------------------------------------------------------------------------------");
                        report.WriteLine();
                        report.WriteLine("Export created:");
                        report.WriteLine("- " + DateTime.Now.ToString("dddd d MMMM yyyy h:mm tt"));
                        report.WriteLine();
                        report.WriteLine("Original file:");
                        report.WriteLine("- " + Path.GetFileName(originalFileName));
                        report.WriteLine();
                        report.WriteLine("Included files:");
                        foreach (ElementId link in links)
                        {
                            ExternalFileReference fileRef = ExternalFileUtils.GetExternalFileReference(doc, link);
                            report.WriteLine("- " + Path.GetFileName(ModelPathUtils.ConvertModelPathToUserVisiblePath(fileRef.GetPath())));
                        }
                        report.WriteLine();
                        report.WriteLine("Purged Items:");
                        report.WriteLine("- " + purgedSheets + " Sheets");
                        report.WriteLine("- " + purgedViews + " Views");
                        report.WriteLine("- " + purgedLinks + " Linked Items");
                        report.WriteLine("- " + purgedItems + " Unused Elements");
                        report.WriteLine();
                        report.WriteLine("------------------------------------------------------------------------------");
                        report.WriteLine("Copyright files in this export are the property of NG Bailey Limited.");
                    }
                }
            }

            // Create exported revit file
            SaveAsOptions saveOptions = new SaveAsOptions()
            {
                Compact = true,
                OverwriteExistingFile = true,
                MaximumBackups = 1,
                PreviewViewId = landingSheet.Id
            };
            doc.SaveAs(exportFolder.FullName + @"\" + Path.GetFileName(doc.PathName), saveOptions);
            ModelPath exportedModel = ModelPathUtils.ConvertUserVisiblePathToModelPath(doc.PathName);

            // Update processing window
            pWindow.Update(90, "Opening original model.");

            // Reopen original document
            rpmBIMTools.Load.uiApp.OpenAndActivateDocument(originalFileName);

            // Close Exported Model
            doc.Close(false);

            // Repath links in created revit file
            TransmissionData transData = TransmissionData.ReadTransmissionData(exportedModel);
            if (transData != null)
            {
                ICollection<ElementId> externalReferences = transData.GetAllExternalFileReferenceIds();
                foreach (ElementId refId in externalReferences)
                {
                    ExternalFileReference extRef = transData.GetLastSavedReferenceData(refId);
                    string extFileName = Path.GetFileName(ModelPathUtils.ConvertModelPathToUserVisiblePath(extRef.GetPath()));
                    transData.SetDesiredReferenceData(refId, new FilePath(@"Linked Files\" + extFileName), PathType.Relative, true);
                }
                transData.IsTransmitted = true;
                TransmissionData.WriteTransmissionData(exportedModel, transData);
            }

            // Compressing files, if selected
            if (exportOptions_Compression.Checked)
            {
                pWindow.Update(95, "Compressing files.");
                ZipFile.CreateFromDirectory(exportFolder.FullName, exportFolder.Parent.FullName + @"\" + folderName + ".zip", CompressionLevel.Optimal, false);
                exportFolder.Delete(true);
            }

            // Save Form Settings
            if (saveSettings.Checked)
            {
                Properties.Settings.Default.Save();
            }

            // Close Process Window
            pWindow.Close();

            // Export Model Completed
            TaskDialog finishDialog = new TaskDialog("Export Model Completed");
            finishDialog.MainInstruction = "Exporting Model has been completed!";
            finishDialog.TitleAutoPrefix = false;
            finishDialog.MainContent = "Do you want to view the exported files?";
            finishDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Yes, Show Files");
            finishDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "No, Close");
            TaskDialogResult fResult = finishDialog.Show();
            if (fResult == TaskDialogResult.CommandLink1)
            {
                Process.Start(exportFolder.Exists ? exportFolder.FullName : exportFolder.Parent.FullName);
            }

            this.Close();
        }
    }
}