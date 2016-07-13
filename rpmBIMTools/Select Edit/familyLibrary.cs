using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;

namespace rpmBIMTools
{
    [Transaction(TransactionMode.Manual)]
    public class familyLibrary : IExternalCommand
    {
        public Family family;
        public FamilySymbol familySymbol;
        public FileInfo familyFile;
        public IList<string> familySymbolNames;

        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            Load.uiApp = commandData.Application;
            Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            Document doc = Load.liveDoc;
            UIApplication uiApp = Load.uiApp;

            // Cancel if family document
            if (doc.IsFamilyDocument)
            {
                TaskDialog.Show("Family Library", "Cannot be used in family editor");
                return Result.Failed;
            }

            familyLibrarySelection familyLibrarySelection = new familyLibrarySelection();

            // Show Family Library UI
            if (familyLibrarySelection.ShowDialog() == DialogResult.OK)
            {
                familyFile = new FileInfo(familyLibrarySelection.familyDirectory.FullName + familyLibrarySelection.FamilyTree.SelectedNode.FullPath + ".rfa");
                if (familyFile.Exists)
                {

                    // Load single family type Mode
                    using (Transaction t = new Transaction(doc, "Temp Load Family"))
                    {
                        // Load family or find in project
                        t.Start();
                        family = new FilteredElementCollector(doc).OfClass(typeof(Family)).FirstOrDefault(f => f.Name == Path.GetFileNameWithoutExtension(familyFile.Name)) as Family;
                        if (family != null)
                        {
                            doc.Delete(family.Id); // delete family
                        }

                        bool loaded = doc.LoadFamily(familyFile.FullName, new familyLoadOptions(), out family);

                        if (!loaded)
                        {
                            family = new FilteredElementCollector(doc).OfClass(typeof(Family)).FirstOrDefault(f => f.Name == Path.GetFileNameWithoutExtension(familyFile.Name)) as Family;
                            if (family == null)
                            {
                                TaskDialog.Show("Family Library", "Unable to load family");
                                return Result.Failed;
                            }
                        }
                        familySymbolNames = family.GetFamilySymbolIds().Select(eId => (doc.GetElement(eId) as FamilySymbol).Name).ToList();
                        t.RollBack();
                    }

                    // Load all family type Mode
                    if (familyLibrarySelection.loadAllTypes.Checked)
                    {

                        using (Transaction t = new Transaction(doc, "Load Family"))
                        {
                            // Load family or find in project
                            t.Start();
                            bool loaded = doc.LoadFamily(familyFile.FullName, new familyLoadOptions(), out family);
                            if (!loaded)
                            {
                                family = new FilteredElementCollector(doc).OfClass(typeof(Family)).FirstOrDefault(f => f.Name == Path.GetFileNameWithoutExtension(familyFile.Name)) as Family;
                                if (family == null)
                                {
                                    TaskDialog.Show("Family Library", "Unable to load family");
                                    t.RollBack();
                                    return Result.Failed;
                                }
                            }

                            // Add additional types if count doesn't match
                            if (familySymbolNames.Count != family.GetFamilySymbolIds().Count)
                            {
                                foreach (string symbol in familySymbolNames)
                                {
                                    Load.liveDoc.LoadFamilySymbol(familyFile.FullName, symbol, new familyLoadOptions(), out familySymbol);
                                }
                            }
                            t.Commit();
                        }

                        // Add type selection here
                        IList<ElementId> familySymbolIds = family.GetFamilySymbolIds().ToList();
                        if (familySymbolIds.Count == 1)
                        {
                            familySymbol = doc.GetElement(familySymbolIds.First()) as FamilySymbol;
                            ElementType familyType = doc.GetElement(familySymbolIds.First()) as ElementType;
                            uiApp.ActiveUIDocument.PostRequestForElementTypePlacement(familyType);
                        }
                        else
                        {
                            symbolLibrarySelection symbolLibrarySelection = new symbolLibrarySelection();
                            int top = 0;
                            foreach (ElementId fs in familySymbolIds)
                            {
                                FamilySymbol symbol = doc.GetElement(fs) as FamilySymbol;
                                if (symbol != null)
                                {
                                    RadioButton radioButton = new RadioButton();
                                    radioButton.Text = symbol.Name;
                                    radioButton.Left = 5;
                                    radioButton.Top = top;
                                    radioButton.Width = 225;
                                    radioButton.Height = 30;
                                    radioButton.Click += symbolSelectionAllTypes;
                                    symbolLibrarySelection.symbolPanel.Controls.Add(radioButton);
                                    top = top + 30;
                                }
                            }
                            symbolLibrarySelection.ShowDialog();
                        }
                    }
                    else
                    {
                        // Add type selection here

                        if (familySymbolNames.Count() == 1)
                        {
                            using (Transaction t = new Transaction(Load.liveDoc, "Load Family Symbol"))
                            {
                                // Load family or find in project
                                t.Start();
                                bool loaded = Load.liveDoc.LoadFamilySymbol(familyFile.FullName, familySymbolNames.First(), new familyLoadOptions(), out familySymbol);
                                t.Commit();
                                if (!loaded)
                                {
                                    familySymbol = new FilteredElementCollector(Load.liveDoc)
                                        .OfClass(typeof(FamilySymbol))
                                        .Cast<FamilySymbol>()
                                        .FirstOrDefault(fs => fs.Family.Name == Path.GetFileNameWithoutExtension(familyFile.Name) && fs.Name == familySymbolNames.First());
                                }

                                if (familySymbol != null)
                                {
                                    ElementType familyType = Load.liveDoc.GetElement(familySymbol.Id) as ElementType;
                                    Load.uiApp.ActiveUIDocument.PostRequestForElementTypePlacement(familyType);
                                }
                                else
                                {
                                    TaskDialog.Show("Family Library", "Unable to load family symbol");
                                    return Result.Failed;
                                }

                            }
                        }
                        else
                        {
                            symbolLibrarySelection symbolLibrarySelection = new symbolLibrarySelection();
                            int top = 0;
                            foreach (string symbolName in familySymbolNames)
                            {
                                RadioButton radioButton = new RadioButton();
                                radioButton.Text = symbolName;
                                radioButton.Left = 5;
                                radioButton.Top = top;
                                radioButton.Width = 225;
                                radioButton.Height = 30;
                                radioButton.Click += symbolSelectionSingleType;
                                symbolLibrarySelection.symbolPanel.Controls.Add(radioButton);
                                top = top + 30;
                            }
                            symbolLibrarySelection.ShowDialog();
                        }
                    }
                }
            }
            return Result.Succeeded;
        }

        private void symbolSelectionAllTypes(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.FindForm().DialogResult = DialogResult.OK;
            familySymbol = family.GetFamilySymbolIds().Select(eId => Load.liveDoc.GetElement(eId) as FamilySymbol).FirstOrDefault(fs => fs.Name == rb.Text);
            if (familySymbol != null)
            {
                ElementType familyType = Load.liveDoc.GetElement(familySymbol.Id) as ElementType;
                Load.uiApp.ActiveUIDocument.PostRequestForElementTypePlacement(familyType);
            }
        }

        private void symbolSelectionSingleType(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.FindForm().DialogResult = DialogResult.OK;
            using (Transaction t = new Transaction(Load.liveDoc, "Load Family Symbol"))
            {
                // Load family or find in project
                t.Start();
                bool loaded = Load.liveDoc.LoadFamilySymbol(familyFile.FullName, rb.Text, out familySymbol);
                t.Commit();
                if (!loaded)
                {
                    familySymbol = new FilteredElementCollector(Load.liveDoc)
                        .OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(fs => fs.Family.Name == Path.GetFileNameWithoutExtension(familyFile.Name) && fs.Name == rb.Text);
                }

                if (familySymbol != null)
                {
                    ElementType familyType = Load.liveDoc.GetElement(familySymbol.Id) as ElementType;
                    Load.uiApp.ActiveUIDocument.PostRequestForElementTypePlacement(familyType);
                }
                else
                {
                    TaskDialog.Show("Family Library", "Unable to load family symbol");
                    return;
                }
            }
        }

    }


    public class familyLoadOptions : IFamilyLoadOptions
    {
        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            if (!familyInUse)
            {
                overwriteParameterValues = true;
                return true;
            }
            else
            {
                overwriteParameterValues = true;
                return true;
            }
        }
        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
        {
            if (!familyInUse)
            {
                source = FamilySource.Family;
                overwriteParameterValues = true;
                return true;
            }
            else
            {
                source = FamilySource.Family;
                overwriteParameterValues = true;
                return true;
            }
        }
    }
}