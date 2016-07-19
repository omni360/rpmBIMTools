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

namespace rpmBIMTools
{
    [TransactionAttribute(TransactionMode.Manual)]
    /// <summary>
    /// NGB Project Setup, for cleaning and setuping up of project using architect model.
    /// </summary>
    class projectSetup : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            // Holding any command values
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;
            Document docA;
            RevitLinkInstance linkA;
            ViewPlan newPlan = null;
            ICollection<ViewPlan> newPlans = new List<ViewPlan>();
            projectSetupElements projectSetupElements = null;
            bool isNGBFolder = true;
            string modelType = string.Empty;
            string correctPath = string.Empty;

            // Linked file and output project selection (UI)
            projectSetupFiles projectSetupFiles = new projectSetupFiles();
            if (projectSetupFiles.ShowDialog() != DialogResult.OK)
            {
                return Result.Cancelled;
            }

            // Version detection for architect model
            try
            {
                BasicFileInfo.Extract(projectSetupFiles.filePathArchitect.Text);
            }
            catch (Autodesk.Revit.Exceptions.FileArgumentNotFoundException)
            {
                TaskDialog.Show("Project Setup", "Architect Model file not found.");
                return Result.Failed;
            }
            catch (Autodesk.Revit.Exceptions.InvalidOperationException)
            {
                TaskDialog.Show("Project Setup", "Architect Model is not a Revit Project or is saved in a later version of Revit and cannot be used.");
                return Result.Failed;
            }

            // Concat all additional files together for looping
            IEnumerable<string> additionalFiles = projectSetupFiles.openFileAdditional1.FileNames
                .Concat(projectSetupFiles.openFileAdditional2.FileNames)
                .Concat(projectSetupFiles.openFileAdditional3.FileNames)
                .Concat(projectSetupFiles.openFileAdditional4.FileNames)
                .Concat(projectSetupFiles.openFileAdditional5.FileNames);

            // Version detection for additional models (if selected)
            foreach (string filePath in additionalFiles)
            {
                try
                {
                    BasicFileInfo.Extract(projectSetupFiles.filePathArchitect.Text);
                }
                catch (Autodesk.Revit.Exceptions.FileArgumentNotFoundException)
                {
                    TaskDialog.Show("Project Setup", "'" + filePath + "' Model file not found.");
                    return Result.Failed;
                }
                catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                {
                    TaskDialog.Show("Project Setup", "'" + filePath + "' Model is not a Revit Project or is saved in a later version of Revit and cannot be used.");
                    return Result.Failed;
                }
            }
            
            // Architecture Directory Checks
            if (!Regex.IsMatch(projectSetupFiles.filePathArchitect.Text, @"\\K-BIM\\Working\\Linked Files\\Architecture\\", RegexOptions.IgnoreCase) &&
                !Regex.IsMatch(projectSetupFiles.filePathArchitect.Text, @"\\K-DRAWINGS\\2-NGB_DRAWINGS\\Working\\BIM Model\\Architecture\\", RegexOptions.IgnoreCase))
            {
                isNGBFolder = false;
                modelType = "Architecture";
                correctPath = "\\K-BIM\\Working\\Linked Files\\Architecture\\Model.rvt\nOR\n\\K-DRAWINGS\\2-NGB_DRAWINGS\\Working\\BIM Model\\Architecture\\Model.rvt";
            }

            // Additional Model Directory Checks
            foreach (string filePath in additionalFiles)
            {
                if (!Regex.IsMatch(filePath, @"\\K-BIM\\Working\\Linked Files\\", RegexOptions.IgnoreCase) &&
                    !Regex.IsMatch(filePath, @"\\K-DRAWINGS\\2-NGB_DRAWINGS\\Working\\BIM Model\\", RegexOptions.IgnoreCase))
                {
                    isNGBFolder = false;
                    modelType = "Additional";
                    correctPath = "\\K-BIM\\Working\\Linked Files\\<Model Folder>\\Model.rvt\nOR\n\\K-DRAWINGS\\2-NGB_DRAWINGS\\Working\\BIM Model\\<Model Folder>\\Model.rvt";
                    break;
                }
            }

            // Output Directory Checks
            if (!Regex.IsMatch(projectSetupFiles.filePathOutput.Text, @"\\K-BIM\\Working\\Revit\\", RegexOptions.IgnoreCase) &&
                !Regex.IsMatch(projectSetupFiles.filePathOutput.Text, @"\\K-DRAWINGS\\2-NGB_DRAWINGS\\Working\\BIM Model\\", RegexOptions.IgnoreCase))
            {
                isNGBFolder = false;
                modelType = "Output";
                correctPath = "\\K-BIM\\Working\\Revit\\Output.rvt\nOR\n\\K-DRAWINGS\\2-NGB_DRAWINGS\\Working\\BIM Model\\Output.rvt";
            }

            // Display warning if non-NGB folder structure is flagged
            if (!isNGBFolder)
            {
                TaskDialog tDB = new TaskDialog("Project Setup");
                tDB.MainInstruction = modelType + " file is not located in correct Bailey file structure.";
                tDB.MainContent = "Below is an example of the correct file structure to use:\n\n" + correctPath + "\n\nDo you wish to continue with the locations as they are?";
                tDB.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
                tDB.DefaultButton = TaskDialogResult.Yes;
                if (tDB.Show() == TaskDialogResult.No)
                return Result.Failed;
            }

            // Create / Show Process Window and calculates steps
            ProcessingWindow pWindow = new ProcessingWindow("Project Setup", "Opening NGB Template and saving.", true);
            Rectangle rW = uiApp.MainWindowExtents;
            pWindow.Location = new System.Drawing.Point(
                rW.Left + ((rW.Right - rW.Left) - pWindow.Width) / 2,
                rW.Top + ((rW.Bottom - rW.Top) - pWindow.Height) / 2
                );
            pWindow.Show();
            System.Windows.Forms.Application.DoEvents();
            int step = 60 / (projectSetupFiles.openFileAdditional1.FileNames.Count() +
                projectSetupFiles.openFileAdditional2.FileNames.Count() +
                projectSetupFiles.openFileAdditional3.FileNames.Count() +
                projectSetupFiles.openFileAdditional4.FileNames.Count() +
                projectSetupFiles.openFileAdditional5.FileNames.Count() + 1);
            int prc = 0;
            
            // Open NGB Template based on version
            try
            {
#if REVIT2015
                uiApp.Application.NewProjectDocument(@"C:\rpmBIM\families\NGB Template 2015.rte").SaveAs(projectSetupFiles.filePathOutput.Text, new SaveAsOptions() { OverwriteExistingFile = true });
                doc = uiApp.OpenAndActivateDocument(projectSetupFiles.filePathOutput.Text).Document;
                doc.EnableWorksharing("Shared Levels and Grids", "Workset1");
#endif
#if REVIT2016
                uiApp.Application.NewProjectDocument(@"C:\rpmBIM\families\NGB Template 2016.rte").SaveAs(projectSetupFiles.filePathOutput.Text, new SaveAsOptions() { OverwriteExistingFile = true });
                doc = uiApp.OpenAndActivateDocument(projectSetupFiles.filePathOutput.Text).Document;
                doc.EnableWorksharing("Shared Levels and Grids", "Workset1");
#endif
#if REVIT2017
                uiApp.Application.NewProjectDocument(@"C:\rpmBIM\families\NGB Template 2017.rte").SaveAs(projectSetupFiles.filePathOutput.Text, new SaveAsOptions() { OverwriteExistingFile = true });
                doc = uiApp.OpenAndActivateDocument(projectSetupFiles.filePathOutput.Text).Document;
                doc.EnableWorksharing("Shared Levels and Grids", "Workset1");
#endif
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                pWindow.Close();
                TaskDialog.Show("Project Setup", "NGB Template could not be found for this version of Revit.");
                return Result.Failed;
            }
            catch (Autodesk.Revit.Exceptions.FileAccessException)
            {
                pWindow.Close();
                TaskDialog.Show("Project Setup", "Output was unable to overwrite the file, file is being used by another Revit instance.");
                return Result.Failed;
            }

            // Clean all elements from the template model
            using (Transaction t = new Transaction(doc, "Deleting Template Elements"))
            {
                t.Start();
                doc.Delete(new FilteredElementCollector(doc, doc.ActiveView.Id)
                    .WhereElementIsNotElementType()
                    .WhereElementIsViewIndependent()
                    .ToElementIds());
                doc.Delete(new FilteredElementCollector(doc, doc.ActiveView.Id)
                .OfClass(typeof(TextNote))
                    .ToElementIds());
                t.Commit();
            }

            // Update Process Window
            pWindow.Update(prc += step, "Linking Architect Model");

            // Link architect model, pin and turn room bounding on
            using (Transaction t = new Transaction(doc, "Linking Architects Model"))
            {
                t.Start();
                try
                {
                    RevitLinkLoadResult linkTypeA = RevitLinkType.Create(doc, ModelPathUtils.ConvertUserVisiblePathToModelPath(projectSetupFiles.filePathArchitect.Text), new RevitLinkOptions(true));
                    if (RevitLinkLoadResult.IsCodeSuccess(linkTypeA.LoadResult))
                    {
                        if (doc.GetElement(linkTypeA.ElementId).get_Parameter(BuiltInParameter.WALL_ATTR_ROOM_BOUNDING) != null)
                        {
                            doc.GetElement(linkTypeA.ElementId).get_Parameter(BuiltInParameter.WALL_ATTR_ROOM_BOUNDING).Set(1);
                        }
                        linkA = RevitLinkInstance.Create(doc, linkTypeA.ElementId);
                        linkA.Pinned = true;
                        docA = linkA.GetLinkDocument();
                        doc.ActiveProjectLocation.set_ProjectPosition(XYZ.Zero, docA.ActiveProjectLocation.get_ProjectPosition(XYZ.Zero));
                    }
                    else
                    {
                        pWindow.Close();
                        TaskDialog.Show("Project Setup", "Architect model failed to load");
                        return Result.Failed;
                    }
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    pWindow.Close();
                    return Result.Failed;
                }
                t.Commit();
            }

            // Link Additional models and pin
            using (Transaction t = new Transaction(doc, "Linking Additional Models"))
            {
                t.Start();
                try
                {
                    foreach (string filePath in additionalFiles)
                    {
                        // Update Process Window
                        pWindow.Update(prc += step, "Linking Additional Model\n" + filePath); RevitLinkLoadResult linkTypeAdditional = RevitLinkType.Create(doc, ModelPathUtils.ConvertUserVisiblePathToModelPath(filePath), new RevitLinkOptions(true));
                        if (RevitLinkLoadResult.IsCodeSuccess(linkTypeAdditional.LoadResult))
                        {
                            RevitLinkInstance.Create(doc, linkTypeAdditional.ElementId).Pinned = true;
                        }
                        else
                        {
                            pWindow.Close();
                            TaskDialog.Show("Project Setup", "Additional model failed to load at:\n" + filePath);
                            return Result.Failed;
                        }
                    }
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    pWindow.Close();
                    return Result.Failed;
                }
                t.Commit();
            }

            // Open 3D MEP Services View
            Autodesk.Revit.DB.View MEPView = new FilteredElementCollector(doc)
                .OfClass(typeof(Autodesk.Revit.DB.View))
                .Cast<Autodesk.Revit.DB.View>()
                .FirstOrDefault(v => v.ViewName == "00 - MEP Services");

            uiApp.ActiveUIDocument.ActiveView = MEPView;

            // Create elevation views based on architect model
            using (Transaction t = new Transaction(doc, "Creating Elevations"))
            {
                t.Start();
                XYZ min = linkA.get_BoundingBox(doc.ActiveView).Min;
                XYZ max = linkA.get_BoundingBox(doc.ActiveView).Max;
                ViewFamilyType vftElevation = doc.GetViewFamilyType(ViewFamily.Elevation);
                // North Elevation
                ElevationMarker eM = ElevationMarker.CreateElevationMarker(doc, vftElevation.Id, new XYZ(min.X + (max.X - min.X) / 2, max.Y + 5, min.Z + (max.Z - min.Z) / 2), 96);
                eM.Pinned = true;
                ViewSection vS = eM.CreateElevation(doc, doc.ActiveView.Id, 3);
                vS.get_Parameter(BuiltInParameter.VIEWER_BOUND_OFFSET_FAR).Set((max.Y - min.Y) + 10);
                vS.ViewName = "North";
                vS.Scale = 50;
                // East Elevation
                eM = ElevationMarker.CreateElevationMarker(doc, vftElevation.Id, new XYZ(max.X + 5, min.Y + (max.Y - min.Y) / 2, min.Z + (max.Z - min.Z) / 2), 96);
                eM.Pinned = true;
                vS = eM.CreateElevation(doc, doc.ActiveView.Id, 0);
                vS.get_Parameter(BuiltInParameter.VIEWER_BOUND_OFFSET_FAR).Set((max.X - min.X) + 10);
                vS.ViewName = "East";
                vS.Scale = 50;
                // South Elevation
                eM = ElevationMarker.CreateElevationMarker(doc, vftElevation.Id, new XYZ(min.X + (max.X - min.X) / 2, min.Y - 5, min.Z + (max.Z - min.Z) / 2), 96);
                eM.Pinned = true;
                vS = eM.CreateElevation(doc, doc.ActiveView.Id, 1);
                vS.get_Parameter(BuiltInParameter.VIEWER_BOUND_OFFSET_FAR).Set((max.Y - min.Y) + 10);
                vS.ViewName = "South";
                vS.Scale = 50;
                // West Elevation
                eM = ElevationMarker.CreateElevationMarker(doc, vftElevation.Id, new XYZ(min.X - 5, min.Y + (max.Y - min.Y) / 2, min.Z + (max.Z - min.Z) / 2), 96);
                eM.Pinned = true;
                vS = eM.CreateElevation(doc, doc.ActiveView.Id, 2);
                vS.get_Parameter(BuiltInParameter.VIEWER_BOUND_OFFSET_FAR).Set((max.X - min.X) + 10);
                vS.ViewName = "West";
                vS.Scale = 50;
                doc.Regenerate();
                t.Commit();
            }

            // Hide Process Window
            pWindow.Hide();

            // Populate and set project information based architects mode  user input (UI)
            projectSetupInformation projectSetupInformation = new projectSetupInformation();
            projectSetupInformation.projectName.Text = docA.ProjectInformation.Name;
            projectSetupInformation.projectAddress.Text = docA.ProjectInformation.Address;
            projectSetupInformation.buildingName.Text = docA.ProjectInformation.BuildingName;
            projectSetupInformation.clientName.Text = docA.ProjectInformation.ClientName;
            FamilySymbol titleBlockA0 = rpmBIMUtils.findNGBTitleBlock(doc, "A0");
            FamilySymbol titleBlockA1 = rpmBIMUtils.findNGBTitleBlock(doc, "A1");
            FamilySymbol titleBlockA2 = rpmBIMUtils.findNGBTitleBlock(doc, "A2");
            FamilySymbol titleBlockA3 = rpmBIMUtils.findNGBTitleBlock(doc, "A3");
            FamilySymbol titleBlockA4 = rpmBIMUtils.findNGBTitleBlock(doc, "A4");
            foreach (Parameter fp in titleBlockA0.Parameters)
            {
                if (fp.Definition.Name.Contains("NGB") && fp.Definition.ParameterType == ParameterType.YesNo && fp.Definition.ParameterGroup == BuiltInParameterGroup.PG_GRAPHICS)
                    projectSetupInformation.NGBLocation.Items.Add(fp.Definition.Name);
            }
            projectSetupInformation.ShowDialog();
            if (projectSetupInformation.DialogResult == DialogResult.OK)
            {
                using (Transaction t = new Transaction(doc, "Setting Project Information"))
                {
                    t.Start();
                    doc.ProjectInformation.Number = projectSetupInformation.projectNumber.Text;
                    doc.ProjectInformation.Name = projectSetupInformation.projectName.Text;
                    doc.ProjectInformation.BuildingName = projectSetupInformation.buildingName.Text;
                    doc.ProjectInformation.Address = projectSetupInformation.projectAddress.Text;
                    doc.ProjectInformation.ClientName = projectSetupInformation.clientName.Text;
                    doc.ProjectInformation.IssueDate = DateTime.Today.ToString("dd/MM/yy");
                    doc.ProjectInformation.Status = "S3 - For Review & Comment";
                    if (doc.ProjectInformation.LookupParameter("NGB OSM Code") != null)
                        doc.ProjectInformation.LookupParameter("NGB OSM Code").Set(projectSetupInformation.projectOMSCode.Text);
                    if (doc.ProjectInformation.LookupParameter("NGB Eng Code") != null)
                        doc.ProjectInformation.LookupParameter("NGB Eng Code").Set(projectSetupInformation.projectEngCode.Text);
                    if (projectSetupInformation.NGBLocation.SelectedIndex != -1)
                    {
                        if (titleBlockA0.LookupParameter(projectSetupInformation.NGBLocation.Text) != null)
                            titleBlockA0.LookupParameter(projectSetupInformation.NGBLocation.Text).Set(1);
                        if (titleBlockA1.LookupParameter(projectSetupInformation.NGBLocation.Text) != null)
                            titleBlockA1.LookupParameter(projectSetupInformation.NGBLocation.Text).Set(1);
                        if (titleBlockA2.LookupParameter(projectSetupInformation.NGBLocation.Text) != null)
                            titleBlockA2.LookupParameter(projectSetupInformation.NGBLocation.Text).Set(1);
                        if (titleBlockA3.LookupParameter(projectSetupInformation.NGBLocation.Text) != null)
                            titleBlockA3.LookupParameter(projectSetupInformation.NGBLocation.Text).Set(1);
                        if (titleBlockA4.LookupParameter(projectSetupInformation.NGBLocation.Text) != null)
                            titleBlockA4.LookupParameter(projectSetupInformation.NGBLocation.Text).Set(1);
                    }
                    t.Commit();
                }
            }
            else
            {
                TaskDialog.Show("Project Setup", "Project Setup process has been stopped!");
                return Result.Cancelled;
            }

            // Level creation, pinning and Plan View creation creation (UI)
            IList<Level> linkLevels = new FilteredElementCollector(docA)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .ToList();
            ICollection<ElementId> oldLevels = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .ToElementIds();
            if (linkLevels.Count() != 0)
            {
                projectSetupElements = new projectSetupElements();
                foreach (Level level in linkLevels)
                {
                    bool check = level.get_Parameter(BuiltInParameter.LEVEL_IS_BUILDING_STORY).AsInteger() == 1 ? true : false;
                    projectSetupElements.grid.Rows.Add(level.Id, level.Name, Math.Round(level.ProjectElevation * 304.8), check);
                }

                projectSetupElements.grid.Sort(projectSetupElements.grid.Columns[2], ListSortDirection.Ascending);
                projectSetupElements.ShowDialog();
                if (projectSetupElements.DialogResult == DialogResult.OK)
                {
                    // Show & Update Process Window
                    pWindow.Show();
                    pWindow.Update(70, "Creating Project Levels and Plan Views");
                    using (Transaction t = new Transaction(doc, "Setting Project Levels and Plan Views"))
                    {

                        t.Start("Create Project Levels");
                        List<ElementId> copyLevels = new List<ElementId>();
                        foreach (DataGridViewRow row in projectSetupElements.grid.Rows)
                        {
                            if (row.Cells["copy"].Value.ToString() == bool.TrueString)
                            {
                                copyLevels.Add(docA.GetElement(row.Cells["id"].Value as ElementId).Id);
                            }
                        }
                        if (copyLevels.Count() != 0)
                        {
                            CopyPasteOptions copyOptions = new CopyPasteOptions();
                            copyOptions.SetDuplicateTypeNamesHandler(new CopyUseDestination());
                            ICollection<ElementId> newLevelIds = ElementTransformUtils.CopyElements(docA, copyLevels, doc, null, copyOptions);
                            if (oldLevels.Count() != 0)
                            {
                                ElementId linkLevelType = (doc.GetElement(newLevelIds.First()) as Level).GetTypeId();
                                foreach (ElementId levelId in newLevelIds)
                                {
                                    Level level = doc.GetElement(levelId) as Level;
                                    level.ChangeTypeId((doc.GetElement(oldLevels.First()) as Level).GetTypeId());
                                    level.Pinned = true;
                                }
                                if ((doc.GetElement(oldLevels.First()) as Level).GetTypeId() != linkLevelType)
                                doc.Delete(linkLevelType);
                            }
                            ViewFamilyType vftFloorPlan = doc.GetViewFamilyType(ViewFamily.FloorPlan);
                            foreach(ElementId levelId in newLevelIds) {
                                newPlan = ViewPlan.Create(doc, vftFloorPlan.Id, levelId);
                                newPlans.Add(newPlan);
                                if (newPlan.LookupParameter("Sub-Discipline") != null)
                                {
                                    newPlan.LookupParameter("Sub-Discipline").Set("MASTER");
                                }
                            }
                            doc.Regenerate();
                            t.Commit();
                            uiApp.ActiveUIDocument.ActiveView = newPlan;
                            t.Start("Delete Old Levels");
                            doc.Delete(oldLevels);
                            t.Commit();
                        }
                    }
                }
                else
                {
                    doc.Save();
                    TaskDialog.Show("Project Setup", "Project Setup process has been stopped!");
                    return Result.Succeeded;
                }
            }

            // Gridline creation (UI)
            if (projectSetupElements.copyGrids.Checked)
            {
                ICollection<ElementId> linkGrids = new FilteredElementCollector(docA)
                    .OfClass(typeof(Grid))
                    .ToElementIds();
                GridType ngbGridType = new FilteredElementCollector(doc)
                .OfClass(typeof(GridType))
                .Cast<GridType>()
                .FirstOrDefault();
                // Update Process Window
                pWindow.Update(80, "Creating Project Gridlines");
                if (linkGrids.Count() != 0)
                {
                    using (Transaction t = new Transaction(doc, "Creating Project Gridlines"))
                    {
                        t.Start();
                        CopyPasteOptions copyOptions = new CopyPasteOptions();
                        copyOptions.SetDuplicateTypeNamesHandler(new CopyUseDestination());
                        ICollection<ElementId> newGrids = ElementTransformUtils.CopyElements(docA, linkGrids, doc, null, copyOptions);
                        if (ngbGridType != null)
                        {
                            ElementId gridLevelType = (doc.GetElement(newGrids.First()) as Grid).GetTypeId();
                            foreach (ElementId GridId in newGrids)
                            {
                                Grid grid = doc.GetElement(GridId) as Grid;
                                if (grid != null)
                                {
                                    grid.ChangeTypeId(ngbGridType.Id);
                                    grid.Pinned = true;
                                }
                            }
                            if (ngbGridType.Id != gridLevelType)
                                doc.Delete(gridLevelType);
                        }
                        t.Commit();
                    }
                }
            }

            // Space creation (UI)
            if (projectSetupElements.copySpaces.Checked)
            {
                ICollection<ElementId> linkRooms = new FilteredElementCollector(docA)
                    .OfClass(typeof(SpatialElement))
                    .ToElementIds();
                IEnumerable<Level> newLevels = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>();
                // Update Process Window
                pWindow.Update(90, "Creating Project Spaces");
                if (linkRooms.Count() != 0)
                {
                    using (Transaction t = new Transaction(doc, "Creating project spaces"))
                    {
                        t.Start();
                        foreach (ElementId RoomId in linkRooms)
                        {
                            Room room = docA.GetElement(RoomId) as Room;
                            if (room != null)
                            {
                                if (newLevels.Any(l => l.Name == room.Level.Name))
                                {
                                    Level level = newLevels.Where(l => l.Name == room.Level.Name).FirstOrDefault();
                                    Level upperLevel = null;
                                    if (room.UpperLimit != null)
                                        upperLevel = newLevels.Where(l => l.Name == room.UpperLimit.Name).FirstOrDefault();
                                    LocationPoint roomPoint = room.Location as LocationPoint;
                                    if (level != null && roomPoint != null)
                                    {
                                        Space space = doc.Create.NewSpace(level, new UV(roomPoint.Point.X, roomPoint.Point.Y));
                                        space.Number = room.Number;
                                        space.Name = room.Name;
                                        space.BaseOffset = room.BaseOffset;
                                        space.LimitOffset = room.LimitOffset;
                                        space.Pinned = true;
                                        if (upperLevel != null)
                                            space.UpperLimit = upperLevel;
                                    }
                                }
                            }
                        }
                        t.Commit();
                    }
                }
            }

            // Create sheets per floor plans created
            if (titleBlockA0 != null) {
                using (Transaction t = new Transaction(doc, "Creating project sheets"))
                {
                    t.Start();
                    foreach (ViewPlan viewPlan in newPlans)
                    {
                        ViewSheet sheet = ViewSheet.Create(doc, titleBlockA0.Id);
                        sheet.ViewName = viewPlan.ViewName;
                        sheet.SetStatus("S3 - For Review & Comment");
                        sheet.SetSubDiscipline("MASTER");
                        if (sheet.get_Parameter(BuiltInParameter.SHEET_ISSUE_DATE) != null)
                            sheet.get_Parameter(BuiltInParameter.SHEET_ISSUE_DATE).Set(DateTime.Today.ToString("dd/MM/yy"));
                        if (viewPlan.CanViewBeDuplicated(ViewDuplicateOption.AsDependent))
                        {
                            ElementId viewDependentId = viewPlan.Duplicate(ViewDuplicateOption.AsDependent);
                            if (Viewport.CanAddViewToSheet(doc, sheet.Id, viewDependentId))
                            {
                                BoundingBoxUV sheetBox = sheet.Outline;
                                double yPosition = (sheetBox.Max.V - sheetBox.Min.V) / 2 + sheetBox.Min.V;
                                double xPosition = (sheetBox.Max.U - sheetBox.Min.U) / 2 + sheetBox.Min.U;
                                XYZ origin = new XYZ(xPosition, yPosition, 0);
                                Viewport.Create(doc, sheet.Id, viewDependentId, origin);
                            }
                        }
                    }
                    t.Commit();
                }
            }

            // Final Save
            pWindow.Update(100, "Saving Project as central");
            doc.Save();
            pWindow.Close();

            // Finished Process Message
            TaskDialog dialog = new TaskDialog("Project Setup");
            dialog.MainInstruction = "Project setup has been completed.";
            dialog.MainContent = "Proceed to setup shared coordinates or model location issues (if required) and then make a local copy for further editing of this new project.";
            dialog.CommonButtons = TaskDialogCommonButtons.Ok;
            dialog.Show();
            return Result.Succeeded;
        }

        /// <summary>
        /// Removes the duplicate types warning dialog when coping from a linked project to the host project.
        /// </summary>
        public class CopyUseDestination : IDuplicateTypeNamesHandler
        {
            public DuplicateTypeAction OnDuplicateTypeNamesFound(
            DuplicateTypeNamesHandlerArgs args)
            {
                return DuplicateTypeAction.UseDestinationTypes;
            }
        }
    }
}