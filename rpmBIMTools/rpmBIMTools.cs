namespace rpmBIMTools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Media.Imaging;
    using System.IO;

    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI.Events;
    public class Load : IExternalApplication
    {
        // Setup Live Document Variables
        public static UIApplication uiApp;
        public static Document liveDoc;
        public static string revitVer;
        public static DockablePanes.FamilyLibrary familyLibraryPane;
        
        public Result OnStartup(UIControlledApplication application)
        {
            revitVer = application.ControlledApplication.VersionNumber;
            Autodesk.Windows.RibbonTab modifyTab = Autodesk.Windows.ComponentManager.Ribbon.FindTab("Modify");
            modifyTab.PropertyChanged += AddSpecialCharacterTab;

            // Collect Assembly Information for use
            string rpmBIM_Tools_Path = (new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            string mainTabName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            // Register Dockable Library Family into Revit Application
            familyLibraryPane = new DockablePanes.FamilyLibrary();

            DockablePaneId dpid = new DockablePaneId(new Guid("{516EB33F-39C1-4BE5-9920-042A51C78DA8}"));
            application.RegisterDockablePane(dpid, "NGB Family Library", familyLibraryPane);
            application.ViewActivated += Application_ViewActivated;
            application.Idling += Load.familyLibraryPane.insertFamily;
          
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
            RibbonPanel ribbonTabSectionBox = application.CreateRibbonPanel(mainTabName, "Section Box");

            PushButton buttonCreateSectionBox = ribbonTabSectionBox.AddItem(new PushButtonData("createSetionBox", "Create\nSection Box", rpmBIM_Tools_Path, "rpmBIMTools.createSectionBox")) as PushButton;
            buttonCreateSectionBox.LargeImage = EmbededBitmap(Properties.Resources.CreateSectionBox32);
            buttonCreateSectionBox.LongDescription = "Creates a section box using a interface with controls";
            PushButton buttonToggleSectionBox = ribbonTabSectionBox.AddItem(new PushButtonData("toogleSectionBox", "Toggle\nSection Box", rpmBIM_Tools_Path, "rpmBIMTools.toggleSectionBox")) as PushButton;
            buttonToggleSectionBox.LargeImage = EmbededBitmap(Properties.Resources.ToggleSectionBox32);
            buttonToggleSectionBox.LongDescription = "Toggles any section boxes within a view if found";

            // Scope Box Panel
            RibbonPanel ribbonTabScopeBox = application.CreateRibbonPanel(mainTabName, "Scope Box");

            PushButton buttonPurgeScopeBox = ribbonTabScopeBox.AddItem(new PushButtonData("purgeScopeBox", "Purge\nScope Box", rpmBIM_Tools_Path, "rpmBIMTools.purgeScopeBox")) as PushButton;
            buttonPurgeScopeBox.LargeImage = EmbededBitmap(Properties.Resources.PurgeScopeBox32);
            buttonPurgeScopeBox.LongDescription = "Purging scope boxes with filtering controls";

            // Schedule Panel
            RibbonPanel ribbonTabSchedule = application.CreateRibbonPanel(mainTabName, "Schedule");

            PushButton buttonExportImportSchedules = ribbonTabSchedule.AddItem(new PushButtonData("exportImportSchedules", "Export / Import\nSchedules", rpmBIM_Tools_Path, "rpmBIMTools.exportImportSchedules")) as PushButton;
            buttonExportImportSchedules.LargeImage = EmbededBitmap(Properties.Resources.ExportImportSchedule32);
            buttonExportImportSchedules.LongDescription = "Import and Export of Schematics to Revit Projects";

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
            PushButton buttonFamilyLibrary = ribbonTabSelectEdit.AddItem(new PushButtonData("FamilyLibrary", "Family\nLibrary", rpmBIM_Tools_Path, "rpmBIMTools.toggleFamilyLibraryPane")) as PushButton;
            buttonFamilyLibrary.LargeImage = EmbededBitmap(Properties.Resources.FamilyLibrary32);
            buttonFamilyLibrary.LongDescription = "Utility for loading NGB families";
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

            // About
            RibbonPanel ribbonTabHelpInformation = application.CreateRibbonPanel(mainTabName, "Help");

            PushButton buttonWiki = ribbonTabHelpInformation.AddItem(new PushButtonData("wikiGuides", "Wiki\nGuides", rpmBIM_Tools_Path, "rpmBIMTools.openWikiSite")) as PushButton;
            buttonWiki.LargeImage = EmbededBitmap(Properties.Resources.Guide32);
            buttonWiki.LongDescription = "Opens a web browser to the rpmBIMTools Wiki Guides";
            PushButton buttonAbout = ribbonTabHelpInformation.AddItem(new PushButtonData("aboutRpmBIMTools", "About", rpmBIM_Tools_Path, "rpmBIMTools.TBC")) as PushButton;
            buttonAbout.LargeImage = EmbededBitmap(Properties.Resources.Help32);
            buttonAbout.LongDescription = "Basic information about the current version of rpmBIMTools";

            // Special Character Panel (Hidden by default)
            RibbonPanel ribbonTabCharacter = application.CreateRibbonPanel(mainTabName, "Characters");
            ribbonTabCharacter.AddStackedItems(
                new PushButtonData("°", "°", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Degree"),
                new PushButtonData("Ø", "Ø", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Radius"),
                new PushButtonData("Ω", "Ω", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Ohm")
                );
            ribbonTabCharacter.AddStackedItems(
                new PushButtonData("№", "№", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Numero"),
                new PushButtonData("²", "²", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Superscript2"),
                new PushButtonData("³", "³", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Superscript3")
                );
            ribbonTabCharacter.AddStackedItems(
                new PushButtonData("¼", "¼", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.FractionOneFour"),
                new PushButtonData("½", "½", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.FractionOneTwo"),
                new PushButtonData("¾", "¾", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.FractionThreeFour")
                );
            ribbonTabCharacter.AddStackedItems(
                new PushButtonData("∇", "∇", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Nabia"),
                new PushButtonData("±", "±", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.PlusMinus"),
                new PushButtonData("√", "√", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Squareroot")
                );
            ribbonTabCharacter.AddStackedItems(
                new PushButtonData("∞", "∞", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Infinity"),
                new PushButtonData("∠", "∠", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Angle"),
                new PushButtonData("π", "π", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Pie")
                );
            ribbonTabCharacter.AddStackedItems(
                new PushButtonData("≠", "≠", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.NotEqual"),
                new PushButtonData("≤", "≤", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.LessThan"),
                new PushButtonData("≥", "≥", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.GreaterThan")
                );
            ribbonTabCharacter.AddStackedItems(
                new PushButtonData("®", "®", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Register"),
                new PushButtonData("♀", "♀", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Female"),
                new PushButtonData("♂", "♂", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Male")
                );
            ribbonTabCharacter.AddStackedItems(
                new PushButtonData("€", "€", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Euro"),
                new PushButtonData("™", "™", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Trademark"),
                new PushButtonData("©", "©", rpmBIM_Tools_Path, "rpmBIMTools.InsertSpecialCharacter.Copyright")
                );
            ribbonTabCharacter.Visible = false;

            Autodesk.Windows.RibbonPanel modifyCharacterPanel = Autodesk.Windows.ComponentManager.Ribbon.FindTab(mainTabName).Panels.Last();
            modifyCharacterPanel.Source.Id = "characterPanel";

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// Keeps the document and application varibles updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_ViewActivated(object sender, ViewActivatedEventArgs e)
        {
            Load.liveDoc = e.Document;
            Load.uiApp = sender as UIApplication;
        }

        private BitmapSource EmbededBitmap(System.Drawing.Bitmap bitmap)
        {
            BitmapSource destination;
            destination = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            destination.Freeze();
            return destination;
        }

        public void AddSpecialCharacterTab(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Title")
            {
                string mainTabName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                Autodesk.Windows.RibbonTab ModifyTab = sender as Autodesk.Windows.RibbonTab;
                Autodesk.Windows.RibbonTab rpmBIMToolsTab = Autodesk.Windows.ComponentManager.Ribbon.FindTab(mainTabName);
                Autodesk.Windows.RibbonPanel CharacterPanel = Autodesk.Windows.ComponentManager.Ribbon.FindPanel("characterPanel", false);

                if (ModifyTab.Title == "Modify | Text Notes")
                {
                    if (CharacterPanel.Tab.Title == mainTabName)
                    {
                        CharacterPanel.IsVisible = true;
                        ModifyTab.Panels.Add(CharacterPanel);
                        rpmBIMToolsTab.Panels.Remove(CharacterPanel);
                    }
                }
                else
                {
                    if (CharacterPanel.Tab.Title == "Modify")
                    {
                        CharacterPanel.IsVisible = false;
                        rpmBIMToolsTab.Panels.Add(CharacterPanel);
                        ModifyTab.Panels.Remove(CharacterPanel);
                    }
                }
            }
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
            //Dictionary<string, string> drawingSheetList = DwgNumCalc.GetArrays(commandData.Application.ActiveUIDocument.Document, 5);
            //foreach (KeyValuePair<string, string> drawingSheet in drawingSheetList)
            //{
            //    TaskDialog.Show("Test", drawingSheet.Key + " - " + drawingSheet.Value);
            //}
            return Result.Succeeded;
        }
    }

    /// /////////////////////////////
    /// Toggle Family Library DockablePane
    /// 
    [Transaction(TransactionMode.Manual)]
    
    public class toggleFamilyLibraryPane : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            DockablePaneId dpid = new DockablePaneId(new Guid("{516EB33F-39C1-4BE5-9920-042A51C78DA8}"));
            DockablePane dp = commandData.Application.GetDockablePane(dpid);
            if (dp.IsShown())
            {
                dp.Hide();
            }
            else
            {
                dp.Show();
            }
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
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            generateDialog form = new generateDialog();
            form.ShowDialog();
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]

    class openWikiSite : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki");
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]

    partial class exportImportSchedules : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;

            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Export / Import Schedules", "Cannot be used in family editor");
            }
            else
            {
                exportImportSchedules form = new exportImportSchedules();
                form.ShowDialog();
            }
            return Result.Succeeded;
        }
    }

    [TransactionAttribute(TransactionMode.Manual)]

    public partial class createSectionBox : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
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
                form.selectedElements = rpmBIMTools.Load.uiApp.ActiveUIDocument.Selection.GetElementIds()
                .Select(eid => rpmBIMTools.Load.liveDoc.GetElement(eid))
                .Where(e => e.get_BoundingBox(null) != null)
                .ToList();
                if (form.selectedElements.Count() != 0)
                {
                    rpmBIMTools.Load.uiApp.ActiveUIDocument.Selection.SetElementIds(new List<ElementId>());
                    form.ShowDialog();
                }
                else
                {
                    TaskDialog.Show("Create Section Box", "No model elements were selected.");
                    return Result.Failed;
                }
            }
            return Result.Succeeded;
        }
    }

    [TransactionAttribute(TransactionMode.Manual)]

    public partial class purgeScopeBox : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
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
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
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
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
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
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
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
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
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
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
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