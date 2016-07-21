using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using Newtonsoft.Json;

namespace rpmBIMTools {

    public partial class familyNameEditor : System.Windows.Forms.Form
    {

        // Global Parameters
        Document doc = rpmBIMTools.Load.liveDoc;
        Family selectedFamily;
        Regex regex = new Regex(@"^[MEG]{1}-Pr[0-9a-zA-Z_]*-[MA]{1}[0-9a-zA-Z_-]*$");
        Regex regexBS8541 = new Regex(@"^[0-9a-zA-Z-]*_[0-9a-zA-Z-]*_[0-9a-zA-Z-]*$");
        List<Uniclass> UniclassData = new List<Uniclass>();
        int[] categoryCheckElec =
        {
            (int)BuiltInCategory.OST_ConduitFitting, (int)BuiltInCategory.OST_ConduitFittingTags,
            (int)BuiltInCategory.OST_SecurityDevices, (int)BuiltInCategory.OST_SecurityDeviceTags,
            (int)BuiltInCategory.OST_NurseCallDevices, (int)BuiltInCategory.OST_NurseCallDeviceTags,  
            (int)BuiltInCategory.OST_ElectricalEquipment, (int)BuiltInCategory.OST_ElectricalEquipmentTags,
            (int)BuiltInCategory.OST_ElectricalFixtures, (int)BuiltInCategory.OST_ElectricalFixtureTags, 
            (int)BuiltInCategory.OST_CommunicationDevices, (int)BuiltInCategory.OST_CommunicationDeviceTags,
            (int)BuiltInCategory.OST_CableTray, (int)BuiltInCategory.OST_CableTrayTags,
            (int)BuiltInCategory.OST_CableTrayFitting, (int)BuiltInCategory.OST_CableTrayFittingTags, 
            (int)BuiltInCategory.OST_LightingFixtures, (int)BuiltInCategory.OST_LightingFixtureTags, 
            (int)BuiltInCategory.OST_Conduit, (int)BuiltInCategory.OST_ConduitTags,
            (int)BuiltInCategory.OST_DataDevices, (int)BuiltInCategory.OST_DataDeviceTags,
            (int)BuiltInCategory.OST_TelephoneDevices, (int)BuiltInCategory.OST_TelephoneDeviceTags,
            (int)BuiltInCategory.OST_LightingDevices, (int)BuiltInCategory.OST_LightingDeviceTags,
            (int)BuiltInCategory.OST_FireAlarmDevices, (int)BuiltInCategory.OST_FireAlarmDeviceTags,
            (int)BuiltInCategory.OST_SwitchSystem, (int)BuiltInCategory.OST_ConduitRun,
            (int)BuiltInCategory.OST_CableTrayRun, (int)BuiltInCategory.OST_ElectricalCircuitTags,
            (int)BuiltInCategory.OST_PanelScheduleGraphics, (int)BuiltInCategory.OST_RebarShape
        };
        int[] categoryCheckMech =
        { 
            (int)BuiltInCategory.OST_DuctSystem,
            (int)BuiltInCategory.OST_DuctColorFillLegends, (int)BuiltInCategory.OST_DuctColorFills,
            (int)BuiltInCategory.OST_DuctCurves, (int)BuiltInCategory.OST_DuctTags,
            (int)BuiltInCategory.OST_DuctFitting, (int)BuiltInCategory.OST_DuctFittingTags,
            (int)BuiltInCategory.OST_DuctAccessory, (int)BuiltInCategory.OST_DuctAccessoryTags,
            (int)BuiltInCategory.OST_PipingSystem,
            (int)BuiltInCategory.OST_PipeColorFillLegends, (int)BuiltInCategory.OST_PipeColorFills,
            (int)BuiltInCategory.OST_PipeCurves, (int)BuiltInCategory.OST_PipeTags,
            (int)BuiltInCategory.OST_PipeFitting, (int)BuiltInCategory.OST_PipeFittingTags,
            (int)BuiltInCategory.OST_PipeAccessory, (int)BuiltInCategory.OST_PipeAccessoryTags,
            (int)BuiltInCategory.OST_FlexPipeCurves, (int)BuiltInCategory.OST_FlexPipeTags,
            (int)BuiltInCategory.OST_FlexDuctCurves, (int)BuiltInCategory.OST_FlexDuctTags,
            (int)BuiltInCategory.OST_MechanicalEquipment, (int)BuiltInCategory.OST_MechanicalEquipmentTags,
            (int)BuiltInCategory.OST_Sprinklers, (int)BuiltInCategory.OST_SprinklerTags,
            (int)BuiltInCategory.OST_DuctTerminal, (int)BuiltInCategory.OST_DuctTerminalTags,
            (int)BuiltInCategory.OST_PlumbingFixtures, (int)BuiltInCategory.OST_PlumbingFixtureTags
        };

        public class Uniclass
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public familyNameEditor()
        {
            InitializeComponent();
        }

        private void helpRequest(object sender, HelpEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Family-Name-Editor");
        }

        private void helpButtonClick(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Family-Name-Editor");
            e.Cancel = true;
        }

        private void familyNameEditor_Load(object sender, EventArgs e)
        {
            // Create Uniclass List From Json File
            if (!File.Exists(@"C:\rpmBIM\progs\uniclass2.json.txt")) 
            {
                TaskDialog.Show("Family Name Editor", "Uniclass File Missing");
                this.Close();
                return;
            }
            string UniclassFile = File.ReadAllText(@"C:\rpmBIM\progs\uniclass2015.json.txt");
            var UniclassArray = JsonConvert.DeserializeObject<List<List<string>>>(UniclassFile);

            foreach (var Uniclass in UniclassArray.OrderBy(uc => uc[1], Comparer<string>.Default))
            {
                UniclassData.Add(new Uniclass { Id = Uniclass[0].ToString(), Name = Uniclass[1].ToString() });
            }
            // Populate Uniclass Combobox
            UpdateUniclassField();
            // Collect Family Dataset for List
            ICollection<Family> families = new FilteredElementCollector(doc)
                  .OfClass(typeof(Family))
                    .Cast<Family>()
                    .ToList();
            // Populate & Sort Family Selection DataGridView
            foreach (Family family in families)
            {
                familySelection.Rows.Add(family.UniqueId, family.Name, regex.IsMatch(family.Name) ? "true" : regexBS8541.IsMatch(family.Name) ? "true" : "false");
            }
            familySelection.Sort(familySelection.Columns[1], ListSortDirection.Ascending);
            familySelection.CurrentCell = familySelection[1, 0];
            familySelection_SelectionChanged(sender, e);
        }

        private void UpdateUniclassField()
        {
            familyUniclass2.ValueMember = "Id";
            familyUniclass2.DisplayMember = "Name";
            familyUniclass2.DataSource = UniclassData;
            familyUniclass2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            familyUniclass2.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        
        private void familySelectionPrevious_Click(object sender, EventArgs e)
        {
            int prevRow = familySelection.SelectedRows[0].Index != 0 ? familySelection.SelectedRows[0].Index - 1 : familySelection.RowCount - 1;
            familySelection.CurrentCell = familySelection[1, prevRow];
            familySelection_SelectionChanged(sender, e);
        }

        private void familySelectionNext_Click(object sender, EventArgs e)
        {
            int nextRow = familySelection.SelectedRows[0].Index != familySelection.RowCount - 1 ? familySelection.SelectedRows[0].Index + 1 : 0;
            familySelection.CurrentCell = familySelection[1, nextRow];
            familySelection_SelectionChanged(sender, e);
        }

        private void familyFormatBS8541_CheckedChanged(object sender, EventArgs e)
        {
            familyType.Visible = !familyFormatSwitch2.Checked;
            familySelection_SelectionChanged(sender, e);
        }

        private void familyFormatSwitch_CheckedChanged(object sender, EventArgs e)
        {
            ISet<ElementId> Symbols = selectedFamily.GetFamilySymbolIds();
            if (Symbols.Count() > 0)
            {
                ElementId SymbolId = Symbols.FirstOrDefault();
                ElementType type = doc.GetElement(SymbolId) as ElementType;
                familyIcon.Image = type.GetPreviewImage(new Size(50, 50)) != null ?
                    type.GetPreviewImage(new Size(50, 50)) : Properties.Resources.DrawingSheet32;
                FamilySymbol Symbol = doc.GetElement(SymbolId) as FamilySymbol;
                Parameter Manufacturer = Symbol.LookupParameter("Manufacturer");
                Parameter Model = Symbol.LookupParameter("Model");
                Parameter Description = Symbol.LookupParameter("Description");
                familyPresentation.Enabled = !familyFormatSwitch2.Checked;
                familyUniclass2.Visible = familyFormatSwitch2.Checked;
                familyType.Visible = !familyFormatSwitch2.Checked;
                familyManufacturer.Visible = familyFormatSwitch2.Checked;
                familyModel.Visible = familyFormatSwitch2.Checked;
                fieldLabel4.Visible = familyFormatSwitch2.Checked;
                familyDescription.Visible = familyFormatSwitch2.Checked;
                fieldLabel5.Visible = familyFormatSwitch2.Checked;
                familyManufacturer.Enabled = Manufacturer == null ? false : familyFormatSwitch2.Checked;
                fieldLabel6.Visible = familyFormatSwitch2.Checked;
                familyModel.Enabled = Model == null ? false : familyFormatSwitch2.Checked;
                familyDescription.Enabled = Description == null ? false : familyFormatSwitch2.Checked;
                fieldLabel1.Text = familyFormatSwitch2.Checked ? "Role:" : "Source:";
                fieldLabel2.Text = familyFormatSwitch2.Checked ? "Uniclass:" : "Type:";
                fieldLabel3.Text = familyFormatSwitch2.Checked ? "Presentation:" : "Sub-Type:";
            }
            if (familyFormatSwitch2.Checked)
            {
                familyUniclass2.Focus();
            }
            familySelection_SelectionChanged(sender, e);
        }

        private void familySelection_SelectionChanged(object sender, EventArgs e)
        {
            // Update family information
            familyCounter.Text = (familySelection.SelectedRows[0].Index + 1) + " / " + familySelection.RowCount;
            selectedFamily = doc.GetElement(familySelection.CurrentRow.Cells[0].Value.ToString()) as Family;
            ISet<ElementId> Symbols = selectedFamily.GetFamilySymbolIds();
            familyName.Text = selectedFamily.Name;
            familyInfo.Text = selectedFamily.FamilyCategory.Name + " - " + Symbols.Count() + " Type(s)";
            familyCompliant.Image = (string)familySelection.CurrentRow.Cells[2].Value == "true" ? Properties.Resources.tick32 : Properties.Resources.cross32;
            familyRole.Text = !familyFormatSwitch2.Checked ? "NGB" :
                categoryCheckElec.Contains(selectedFamily.FamilyCategory.Id.IntegerValue) ? "E" :
                categoryCheckMech.Contains(selectedFamily.FamilyCategory.Id.IntegerValue) ? "M" : "G";

            // Update Uniclass Code if found in name
            if (regex.IsMatch(selectedFamily.Name))
                familyUniclass2.SelectedValue = regex.Match(selectedFamily.Name).Value.Split('-').GetValue(1);
            else
                familyUniclass2.SelectedIndex = 0;

            // Set fields based on standard used
            if (familyFormatBS8541.Checked || familyFormatSwitch3.Checked)
            {
                string[] familyNameParts = selectedFamily.Name.Split(new char[] { '_' }, 3);
                familyType.Text = familyNameParts.Count() == 3 ? familyNameParts[1] : selectedFamily.FamilyCategory.Name.Replace(" ", string.Empty);
                familyPresentation.Text = familyNameParts.Count() == 3 ? familyNameParts[2] : selectedFamily.Name.Replace(" ", string.Empty);
                if (Symbols.Count() == 0)
                {
                    familyIcon.Image = Properties.Resources.DrawingSheet32;
                }
                else
                {
                    ElementId SymbolId = Symbols.FirstOrDefault();
                    ElementType type = doc.GetElement(SymbolId) as ElementType;
                    familyIcon.Image = type.GetPreviewImage(new Size(50, 50)) != null ?
                        type.GetPreviewImage(new Size(50, 50)) : Properties.Resources.DrawingSheet32;
                }
            }
            else
            {
                // Other standard used
                familyPresentation.Text = selectedFamily.FamilyCategory.CategoryType.ToString().First().ToString();
                if (Symbols.Count() == 0)
                {
                    familyIcon.Image = Properties.Resources.DrawingSheet32;
                    familyPresentation.Text = "M";
                    familyManufacturer.Text = "< Not Used >";
                    familyManufacturer.Enabled = false;
                    familyModel.Text = "< Not Used >";
                    familyModel.Enabled = false;
                    familyDescription.Text = "< Not Used >";
                    familyDescription.Enabled = false;
                }
                else
                {
                    ElementId SymbolId = Symbols.FirstOrDefault();
                    ElementType type = doc.GetElement(SymbolId) as ElementType;
                    familyIcon.Image = type.GetPreviewImage(new Size(50, 50)) != null ?
                        type.GetPreviewImage(new Size(50, 50)) : Properties.Resources.DrawingSheet32;
                    FamilySymbol Symbol = doc.GetElement(SymbolId) as FamilySymbol;
                    Parameter Manufacturer = Symbol.LookupParameter("Manufacturer");
                    Parameter Model = Symbol.LookupParameter("Model");
                    Parameter Description = Symbol.LookupParameter("Description");
                    familyManufacturer.Text = Manufacturer != null ? Manufacturer.AsString() : "< Not Used >";
                    familyManufacturer.Enabled = Manufacturer != null & familyFormatSwitch2.Checked ? true : false;
                    familyModel.Text = Model != null ? Model.AsString() : "< Not Used >";
                    familyModel.Enabled = Model != null & familyFormatSwitch2.Checked ? true : false;
                    familyDescription.Text = Description != null ? Description.AsString() : "< Not Used >";
                    familyDescription.Enabled = Description != null & familyFormatSwitch2.Checked ? true : false;
                }
            }
            familyCustom.Text = compliantFamilyName();
        }

        private string compliantFamilyName()
        {
            // First field always used
            string familyName = familyRole.Text;

            if (familyFormatSwitch2.Checked)
            {
                familyName += "-" + familyUniclass2.SelectedValue;
                familyName += "-" + familyPresentation.Text;
                familyName += !string.IsNullOrWhiteSpace(familyManufacturer.Text) ? familyManufacturer.Text != "< Not Used >" ?
                    "-" + rpmBIMUtils.GetSafeFamilyName(familyManufacturer.Text) : string.Empty : string.Empty;
                familyName += !string.IsNullOrWhiteSpace(familyModel.Text) ? familyManufacturer.Text != "< Not Used >" ?
                    "-" + rpmBIMUtils.GetSafeFamilyName(familyModel.Text) : string.Empty : string.Empty;
                familyName += !string.IsNullOrWhiteSpace(familyDescription.Text) ? familyDescription.Text != "< Not Used >" ?
                     "-" + rpmBIMUtils.GetSafeFamilyName(familyDescription.Text) : string.Empty : string.Empty;
            }
            else if (familyFormatBS8541.Checked)
            {
                familyName +=  "_" + rpmBIMUtils.GetSafeFamilyName2(familyType.Text);
                familyName +=  "_" + rpmBIMUtils.GetSafeFamilyName2(familyPresentation.Text);
            }
            else
            {
                familyName = selectedFamily.Name;
            }
            return familyName;
        }

        private void familyCustomChange(object sender, EventArgs e)
        {
            familyCustom.Text = compliantFamilyName();
        }

        private void familyUniclass2_TextUpdate(object sender, EventArgs e)
        {
            if (familyUniclass2.DroppedDown) familyUniclass2.DroppedDown = false;
        }

        private void familyFormatSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            familyCustom.Enabled = familyFormatSwitch3.Checked;
            if (familyFormatSwitch3.Checked)
            {
                familyUniclass2.Select(0, 0);
                familyCustom.Focus();
                familyCustom.SelectionStart = familyCustom.TextLength;
                familyType.Enabled = false;
                familyPresentation.Enabled = false;
            } else
            {
                familyType.Enabled = true;
                familyPresentation.Enabled = true;
            }
            familySelection_SelectionChanged(sender, e);
        }

        private void familyApply_Click(object sender, EventArgs e)
        {
            using (Transaction t = new Transaction(doc, "Changing Family Name"))
            {
                try
                {
                    t.Start();
                    if (familyFormatSwitch2.Checked)
                    {
                        if (familyUniclass2.SelectedIndex == -1) familyUniclass2.SelectedIndex = 0;
                        selectedFamily.Name = compliantFamilyName();
                        familySelection.CurrentRow.Cells[2].Value = "true";
                        familyCompliant.Image = Properties.Resources.tick32;
                        // Set Parameters to each FamilySymbol (Type)
                        foreach (ElementId familyId in selectedFamily.GetFamilySymbolIds())
                        {
                            FamilySymbol familySymbol = doc.GetElement(familyId) as FamilySymbol;
                            Parameter manufacturer = familySymbol.LookupParameter("Manufacturer");
                            Parameter model = familySymbol.LookupParameter("Model");
                            Parameter description = familySymbol.LookupParameter("Description");
                            if (manufacturer != null) manufacturer.Set(familyManufacturer.Text);
                            if (model != null) model.Set(familyModel.Text);
                            if (description != null) description.Set(familyDescription.Text);
                        }
                    }
                    else
                    {
                        if (new Regex(@"[^0-9a-zA-Z_ -]").IsMatch(familyCustom.Text))
                        {
                            TaskDialog.Show("Family Name Editor", "Name cannot contain any non-alphanumeric characters.");
                            t.RollBack();
                            return;
                        }
                        selectedFamily.Name = familyCustom.Text;
                        familySelection.CurrentRow.Cells[2].Value = regex.IsMatch(selectedFamily.Name) ? "true" :
                            regexBS8541.IsMatch(selectedFamily.Name) ? "true" : "false";
                        familyCompliant.Image = regex.IsMatch(selectedFamily.Name) ? Properties.Resources.tick32 : Properties.Resources.cross32;
                    }
                    
                    familySelection.CurrentRow.Cells[1].Value = selectedFamily.Name;
                    familyName.Text = selectedFamily.Name;
                    t.Commit();

                    // Continue to next non-compliant family on apply
                    if (familyContinue.Checked)
                    {
                        while (familySelection.CurrentRow.Index != familySelection.RowCount - 1)
                        {
                            familySelection.CurrentCell = familySelection[1, familySelection.CurrentRow.Index + 1];
                            if ((string)familySelection.Rows[familySelection.CurrentRow.Index].Cells[2].Value == "false") break; 
                        }
                        familySelection_SelectionChanged(sender, e);
                    }
                }
                catch (Autodesk.Revit.Exceptions.ArgumentException)
                {
                    TaskDialog.Show("Family Name Editor", "Family with this name already exists.");
                }
            }
        }

        private void familyReset_Click(object sender, EventArgs e)
        {
            familySelection_SelectionChanged(sender, e);
        }

        private void closeEditor_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}