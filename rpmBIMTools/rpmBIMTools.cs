namespace rpmBIMTools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using System.Windows.Forms;
    using System.Windows.Media.Imaging;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Reflection;
    using System.IO;
    using System.IO.Packaging;

    using Autodesk.Revit;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Architecture;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    using Autodesk.Revit.ApplicationServices;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.Exceptions;

    public class Load : IExternalApplication
    {
        // Setup Live Document Variables
        public static UIApplication uiApp;
        public static Document liveDoc;

        public Result OnStartup(UIControlledApplication application)
        {
            // Collect Assembly Information for use
            string rpmBIM_Tools_Path = (new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            string mainTabName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            // Creating Ribbon Tab
            application.CreateRibbonTab(mainTabName);       

            // Project Panel
            RibbonPanel ribbonTabProject = application.CreateRibbonPanel(mainTabName, "Project Tools");

            PushButton buttonProjectSetup = ribbonTabProject.AddItem(new PushButtonData("ProjectSetup", "Project\nSetup", rpmBIM_Tools_Path, "rpmBIMTools.projectSetup")) as PushButton;
            buttonProjectSetup.LargeImage = EmbededBitmap(Properties.Resources.ProjectSetup32);
            buttonProjectSetup.LongDescription = "Automated project creation using linked file selection.";
            PushButton buttonProjectSheetDuplicator = ribbonTabProject.AddItem(new PushButtonData("ProjectSheetDuplicator", "Project Sheet\nDuplicator", rpmBIM_Tools_Path, "rpmBIMTools.projectSheetDuplicator")) as PushButton;
            buttonProjectSheetDuplicator.LargeImage = EmbededBitmap(Properties.Resources.ProjectSheetDuplicator32);
            buttonProjectSheetDuplicator.LongDescription = "Create a full set of service sheets based on user created template sheets";
            PushButton buttonDwgNumCalc = ribbonTabProject.AddItem(new PushButtonData("DrawingNumberCalculator", "Drawing No.\nCalculator", rpmBIM_Tools_Path, "rpmBIMTools.DwgNumCalc")) as PushButton;
            buttonDwgNumCalc.LargeImage = EmbededBitmap(Properties.Resources.DrawingNumberCalculator32);
            buttonDwgNumCalc.LongDescription = "Calculates the drawing number required based on dropdown selection, can output to clipboard or active sheet";
            PushButton buttonModelExport = ribbonTabProject.AddItem(new PushButtonData("ModelExport", "Model\nExport", rpmBIM_Tools_Path, "rpmBIMTools.exportModel")) as PushButton;
            buttonModelExport.LargeImage = EmbededBitmap(Properties.Resources.ExportModel32);
            buttonModelExport.LongDescription = "Purges everything but the model and exports into a compressed file";

            // Scope Box Panel
            RibbonPanel ribbonTabScopeBox = application.CreateRibbonPanel(mainTabName, "Visual Tools");

            PushButton buttonCreateSectionBox = ribbonTabScopeBox.AddItem(new PushButtonData("createSetionBox", "Create\nSection Box", rpmBIM_Tools_Path, "rpmBIMTools.createSectionBox")) as PushButton;
            buttonCreateSectionBox.LargeImage = EmbededBitmap(Properties.Resources.CreateSectionBox32);
            buttonCreateSectionBox.LongDescription = "TBC";
            PushButton buttonToggleSectionBox = ribbonTabScopeBox.AddItem(new PushButtonData("toogleSectionBox", "Toggle\nSection Box", rpmBIM_Tools_Path, "rpmBIMTools.toggleSectionBox")) as PushButton;
            buttonToggleSectionBox.LargeImage = EmbededBitmap(Properties.Resources.ToggleSectionBox32);
            buttonToggleSectionBox.LongDescription = "TBC";
            PushButton buttonPurgeScopeBox = ribbonTabScopeBox.AddItem(new PushButtonData("purgeScopeBox", "Purge\nScope Box", rpmBIM_Tools_Path, "rpmBIMTools.purgeScopeBox")) as PushButton;
            buttonPurgeScopeBox.LargeImage = EmbededBitmap(Properties.Resources.PurgeScopeBox32);
            buttonPurgeScopeBox.LongDescription = "TBC";

            // Schedule Panel
            RibbonPanel ribbonTabSchedule = application.CreateRibbonPanel(mainTabName, "Schedule");

            PushButton buttonImportExportSchedules = ribbonTabSchedule.AddItem(new PushButtonData("importExportSchedules", "Import / Export\nSchedules", rpmBIM_Tools_Path, "rpmBIMTools.TBC")) as PushButton;
            buttonImportExportSchedules.LargeImage = EmbededBitmap(Properties.Resources.ImportExportSchedule32);
            buttonImportExportSchedules.LongDescription = "TBC";

            // Schematic Panel
            RibbonPanel ribbonTabSchematic = application.CreateRibbonPanel(mainTabName, "Schematic");

            PulldownButton buttonLVSchematic = ribbonTabSchematic.AddItem(new PulldownButtonData("LVSchematic", "LV Schematic")) as PulldownButton;
            buttonLVSchematic.LargeImage = EmbededBitmap(Properties.Resources.LVSchematic32);

            PushButton buttonCreateLVSchematic = buttonLVSchematic.AddPushButton(new PushButtonData("createLVSchematic", "Create\nLV Schematic", rpmBIM_Tools_Path, "rpmBIMTools.createLVSchematic"));
            buttonCreateLVSchematic.LargeImage = EmbededBitmap(Properties.Resources.ItemAdd16);
            buttonCreateLVSchematic.LongDescription = "A simple dialog driven utility which will help to draw a basic LV Schematic diagram";
            PushButton buttonImportLVSchematic = buttonLVSchematic.AddPushButton(new PushButtonData("importLVSchematic", "Import\nLV Schematic", rpmBIM_Tools_Path, "rpmBIMTools.importLVSchematic"));
            buttonImportLVSchematic.LargeImage = EmbededBitmap(Properties.Resources.ItemImport16);
            buttonImportLVSchematic.LongDescription = "Imports an LV Schematic created with the rpmBIM creation tool";
            PushButton buttonExportLVSchematic = buttonLVSchematic.AddPushButton(new PushButtonData("exportLVSchematic", "Export\nLV Schematic", rpmBIM_Tools_Path, "rpmBIMTools.exportLVSchematic"));
            buttonExportLVSchematic.LargeImage = EmbededBitmap(Properties.Resources.ItemExport16);
            buttonExportLVSchematic.LongDescription = "Exports an LV Schematic created with the rpmBIM creation tool";

            // Select & Edit Panel
            RibbonPanel ribbonTabSelectEdit = application.CreateRibbonPanel(mainTabName, "Select / Edit");

            PushButton buttonQuickSelect = ribbonTabSelectEdit.AddItem(new PushButtonData("QuickSelect", "Quick\nSelect", rpmBIM_Tools_Path, "rpmBIMTools.quickSelect")) as PushButton;
            buttonQuickSelect.LargeImage = EmbededBitmap(Properties.Resources.QuickSelect32);
            buttonQuickSelect.LongDescription = "Advanced data selection utililty";
            PushButton buttonoFamilyNameEditor = ribbonTabSelectEdit.AddItem(new PushButtonData("FamilyNameEditor", "Family Name\nEditor", rpmBIM_Tools_Path, "rpmBIMTools.familyNameEditor")) as PushButton;
            buttonoFamilyNameEditor.LargeImage = EmbededBitmap(Properties.Resources.FamilyNameEditor32);
            buttonoFamilyNameEditor.LongDescription = "Select multiple elements with ease";

            // Admin Panel
            RibbonPanel ribbonTabAdmin = application.CreateRibbonPanel(mainTabName, "Admin Tools");

            PushButton buttonGenerateGUID = ribbonTabAdmin.AddItem(new PushButtonData("generateGUID", "Generate\nGUID", rpmBIM_Tools_Path, "rpmBIMTools.generateGUID")) as PushButton;
            buttonGenerateGUID.LargeImage = EmbededBitmap(Properties.Resources.GenerateGUID32);
            buttonGenerateGUID.LongDescription = "Generates a new GUID for the user";
            PushButton buttonBulkFileUpdater = ribbonTabAdmin.AddItem(new PushButtonData("builkFileUpdater", "Bulk File\nUpdater", rpmBIM_Tools_Path, "rpmBIMTools.TBC")) as PushButton;
            buttonBulkFileUpdater.LargeImage = EmbededBitmap(Properties.Resources.BulkFileUpdater32);
            buttonBulkFileUpdater.LongDescription = "TBC";

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private BitmapSource EmbededBitmap(System.Drawing.Bitmap bitmap)
        {
            BitmapSource destination;
            destination = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            destination.Freeze();
            return destination;
        }
    }
    
    /////////////////////////////////
    // TBC Message

    [TransactionAttribute(TransactionMode.Manual)]

    public class TBC : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            TaskDialog.Show("Feature Missing", "The selected feature is still under development");
            return Result.Succeeded;
        }
    }

    /////////////////////////////////
    // Drawing Sheet Tools

    [TransactionAttribute(TransactionMode.Manual)]

    public partial class DwgNumCalc : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Drawing Number Calculator", "Cannot be used in family editor");
            }
            else
            {
                DwgNumCalc form = new DwgNumCalc();
                form.ShowDialog();
            }
            return Result.Succeeded;
        }
    }

    /////////////////////////////////
    // Create Tools

    [TransactionAttribute(TransactionMode.Manual)]

    partial class generateGUID : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            generateDialog form = new generateDialog();
            form.ShowDialog();
            return Result.Succeeded;
        }
    }

    [TransactionAttribute(TransactionMode.Manual)]

    public partial class createSectionBox : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {

            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;

            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Create Section Box", "Cannot be used in family editor");
            }
            else
            {
                createSectionBox form = new createSectionBox();
                try
                {
                    form.selectedElements = rpmBIMTools.Load.uiApp.ActiveUIDocument.Selection.PickObjects(ObjectType.Element, "Select elements to place in section box.")
                    .Select(r => rpmBIMTools.Load.liveDoc.GetElement(r.ElementId))
                    .Where(e => e.get_BoundingBox(null) != null)
                    .ToList();
                    if (form.selectedElements.Count() != 0)
                    {
                        form.ShowDialog();
                    }
                }
                catch (Autodesk.Revit.Exceptions.InvalidOperationException) { }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException) { }
            }
            return Result.Succeeded;
        }
    }

    [TransactionAttribute(TransactionMode.Manual)]

    public partial class purgeScopeBox : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Purge Scope Box", "Cannot be used in family editor");
            }
            else
            {
                purgeScopeBox form = new purgeScopeBox();
                form.ShowDialog();
            }
            return Result.Succeeded;
        }
    }

    [TransactionAttribute(TransactionMode.Manual)]

    public partial class createLVSchematic : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Drawing Number Calculator", "Cannot be used in family editor");
            }
            else
            {
                createLVSchematic form = new createLVSchematic();
                form.ShowDialog();
            }
            return Result.Succeeded;
        }
    }

    [TransactionAttribute(TransactionMode.Manual)]

    public partial class exportLVSchematic : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Export LV Schematic", "Cannot be used in family editor");
            }
            else
            {
                IEnumerable<FamilyInstance> schematicReferences = new FilteredElementCollector(rpmBIMTools.Load.liveDoc)
                    .OfClass(typeof(FamilyInstance))
                    .Cast<FamilyInstance>()
                    .Where(fi => fi.Name == "SM - Annotation - Circuit Reference Symbol")
                    .DistinctBy(fi => fi.OwnerViewId);

                if (schematicReferences.Count() == 0)
                {
                    TaskDialog.Show("Export LV Schematic", "No LV schematics found in project");
                }
                else
                {
                    exportLVSchematic form = new exportLVSchematic();
                    form.schematicReferences = schematicReferences;
                    form.ShowDialog();
                }
            }
            return Result.Succeeded;
        }
    }

    /////////////////////////////////
    // Export Tools

    [TransactionAttribute(TransactionMode.Manual)]

    public partial class exportModel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Export Model", "Cannot be used in family editor");
            }
            else
            {
                exportModel form = new exportModel();
                form.ShowDialog();
            }
            return Result.Succeeded;
        }
    }

    /////////////////////////////////
    // Family Name Editor

    [TransactionAttribute(TransactionMode.Manual)]

    public partial class familyNameEditor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Family Name Editor", "Cannot be used in family editor");
            }
            else
            {
                familyNameEditor form = new familyNameEditor();
                form.ShowDialog();
            }
            return Result.Succeeded;
        }
    }

    /////////////////////////////////
    // Quick Select

    [TransactionAttribute(TransactionMode.Manual)]

    public partial class quickSelect : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            quickSelect form = new quickSelect();

            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Quick Select", "Cannot be used in family editor");
            }
            else
            {
                form.ShowDialog();
                while (form.DialogResult == DialogResult.Retry)
                {
                    try
                    {
                        form.selectedElements = rpmBIMTools.Load.uiApp.ActiveUIDocument.Selection.PickElementsByRectangle();
                    }
                    catch { }
                    form.ShowDialog();
                }
            }
            return Result.Succeeded;
        }
    }
}