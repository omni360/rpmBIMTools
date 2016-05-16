using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace rpmBIMTools
{
    public partial class quickSelect : System.Windows.Forms.Form
    {
        // Global Parameters
        Document doc = rpmBIMTools.Load.liveDoc;
        UIApplication uiApp = rpmBIMTools.Load.uiApp;
        ICollection<Element> selectedElements = new List<Element>(); // Collection of stored user selected elements
        ICollection<Element> elements = new List<Element>(); // Collection of stored elements based on selected zone
        BindingList<categorySet> categories = new BindingList<categorySet>(); // collection of stored categories
        BindingList<propertySet> properties = new BindingList<propertySet>(); // collection of stored properties

        public quickSelect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Used for storing category data for selection in the form
        /// </summary>
        public class categorySet
        {
            public string name { get; set; }
            public ElementSet elements { get; set; }
        }

        /// <summary>
        /// Used for storing property data for selection in the fomm
        /// </summary>
        public class propertySet
        {
            public string name { get; set; }
            public StorageType storageType { get; set; }
            public ParameterType parameterType { get; set; }
            public ElementSet elements { get; set; }
        }

        private void quickSelect_Load(object sender, EventArgs e)
        {
            // Assigning DataSource Settings
            selectionCategory.DisplayMember = "name";
            selectionCategory.ValueMember = "elements";
            selectionCategory.DataSource = categories;
            selectionProperties.DisplayMember = "name";
            selectionProperties.ValueMember = "parameterType";
            selectionProperties.DataSource = properties;
            selectionApplyToInclude.Checked = Properties.Settings.Default.quickSelect_selectionApplyToIncludeExclude == 1 ? true : false;
            selectionApplyToExclude.Checked = Properties.Settings.Default.quickSelect_selectionApplyToIncludeExclude == 2 ? true : false;
            selectionApplyToBox.Enabled = selectionApplyToCurrentSelectionSet.Checked;
            // Stored any selected elements before the utility was launched
            foreach (ElementId elementId in uiApp.ActiveUIDocument.Selection.GetElementIds())
            {
                if (doc.GetElement(elementId).Category != null)
                selectedElements.Add(doc.GetElement(elementId));
            }
            // Other things
            selectionChange();
        }

        /// <summary>
        /// Process to update the form with the newly selected elements.
        /// </summary>
        public void selectionChange()
        {
            // Remove Selected Elements from combobox (if found)
            if (selectionZone.Items.Count == 3) selectionZone.Items.RemoveAt(0);
            // Check if any elements are stored
            if (selectedElements.Count() > 0 )
            {
                // Add Selected Elements to combobox
                selectionZone.Items.Insert(0, "Selected Elements (" + selectedElements.Count.ToString() + ")");
                if (selectionZone.SelectedIndex == -1 || selectionZone.SelectedIndex == 1)
                {
                    selectionZone.SelectedIndex = 0;
                }
                else
                {
                    selectionZone_SelectedIndexChanged(null, null);
                    selectionCategory_SelectedIndexChanged(null, null);
                }
            }
            else
            {
                if (selectionZone.SelectedIndex != 0)
                {
                    selectionZone.SelectedIndex = 0;
                }
                else
                {
                    selectionZone_SelectedIndexChanged(null, null);
                }
            }
        }

        /// <summary>
        /// Selected a new set of elements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectionNew_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
            return;
        }

        /// <summary>
        /// Populate fields based on selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectionZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectionZone.SelectedItem.ToString() == "Current View")
            {
                elements = new FilteredElementCollector(doc, doc.ActiveView.Id)
                    .WhereElementIsNotElementType()
                    .Where(elem => elem.Category != null && elem.CanBeHidden(doc.ActiveView))
                    .ToList();
            }
            else if (selectionZone.SelectedItem.ToString() == "Current Project")
            {
                elements = new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .Where(elem => elem.Category != null)
                    .ToList();
            }
            else
            {
                elements = selectedElements;
            }

            // Clear and repopulate catogories
            categories.Clear();
            foreach (Element element in elements)
            {
                // Check for duplicate categories
                categorySet categoryCheck = categories.FirstOrDefault(c => c.name == element.Category.Name);
                if (categoryCheck == null)
                {
                    // Duplicate category not found - adding new 
                    ElementSet tempElements = new ElementSet();
                    tempElements.Insert(element);
                    categories.Add(new categorySet() { name = element.Category.Name, elements = tempElements });
                }
                else
                {
                    // Duplicate category found - adding element to category
                    categoryCheck.elements.Insert(element);
                }
            }
            selectionCategory.DisplayMember = "name";
            if (selectionCategory.Items.Count > 0) selectionCategory_SelectedIndexChanged(null, null);
        }

        private void selectionCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear and repopulate properties
            properties.Clear();
            foreach (Element element in (selectionCategory.SelectedItem as categorySet).elements)
            {
                foreach (Parameter parameter in element.Parameters)
                {
                    if (parameter.HasValue && parameter.Id.IntegerValue != -1140362 && parameter.Id.IntegerValue != -1140363)
                    {
                        propertySet propertyCheck = properties.FirstOrDefault(c => c.name == parameter.Definition.Name && c.parameterType == parameter.Definition.ParameterType && c.storageType == parameter.StorageType);
                        if (propertyCheck == null)
                        {
                            ElementSet tempElements = new ElementSet();
                            tempElements.Insert(element);
                            properties.Add(new propertySet()
                            {
                                name = parameter.Definition.Name,
                                storageType = parameter.StorageType,
                                parameterType = parameter.Definition.ParameterType,
                                elements = tempElements
                            });
                        }
                        else
                        {
                            // Duplicate property found - adding element to property
                            propertyCheck.elements.Insert(element);
                        }
                    }
                }
            }
            if (selectionProperties.Items.Count > 0) selectionProperties_SelectedIndexChanged(null, null);
        }

        private void selectionProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear and repopulate type, values and operator
            propertySet ps = selectionProperties.SelectedItem as propertySet;
            // Set Property Type
            selectionType.Text = ps.storageType.ToString();
            // Set Values
            selectionValue.Text = "";
            selectionValue.Items.Clear();
            foreach(Element element in ps.elements) {
                Parameter pv = element.LookupParameter(ps.name);
                string value = ps.storageType == StorageType.Double ?
                    ps.parameterType == ParameterType.Angle ? Math.Round((pv.AsDouble() * (180.0 / Math.PI)), 1).ToString() : Math.Round((pv.AsDouble() * 304.8), 1).ToString() :
                    ps.storageType == StorageType.ElementId ? pv.AsValueString() :
                    ps.storageType == StorageType.Integer ? pv.AsValueString() :
                    ps.storageType == StorageType.String ? !string.IsNullOrWhiteSpace(pv.AsString()) ? pv.AsString() : string.Empty : string.Empty;
                if (!selectionValue.Items.Contains(value)) selectionValue.Items.Add(value);
            }
            // Set Operator
            selectionOperator.Items.Clear();
            if (ps.parameterType == ParameterType.YesNo)
            {
                selectionOperator.Items.AddRange(new string[] { "True", "False", "Select All" });
            }
            else if (ps.storageType == StorageType.ElementId || ps.storageType == StorageType.String)
            {
                selectionOperator.Items.AddRange(new string[] { "= Equal", "<> Not Equal", "Select All" });
            } 
            else
            {
                selectionOperator.Items.AddRange(new string[] { "= Equal", "<> Not Equal", "> Greater Than", "< Less Than", "Select All" });
            }
            selectionOperator.SelectedIndex = 0;
            selectionValue_SelectedIndexChanged(null, null);
        }

        private void selectionOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Select all is used, no need for properties
            selectionProperties.Enabled = selectionOperator.Text == "Select All" ? false : true;
            // Hide value fieldwhen not required
            bool hideValue = (selectionOperator.Text == "Select All" || selectionOperator.Text == "True" || selectionOperator.Text == "False");
            selectionValueLabel.Visible = hideValue ? false : true;
            selectionValue.Visible = hideValue ? false : true;
            if (selectionOperator.Text == "Select All" || selectionOperator.Text == "True" || selectionOperator.Text == "False")
            {
                selectButton.Enabled = true;
            }
            else
            {
                selectionValue_SelectedIndexChanged(null, null);
            }
        }

        private void selectionValue_TextUpdate(object sender, EventArgs e)
        {
            selectionValue_SelectedIndexChanged(null, null);
        }

        private void selectionValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectButton.Enabled = !(string.IsNullOrWhiteSpace(selectionValue.Text) && selectionValue.Visible);
        }

        private void selectionApplyToIncludeExclude_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.quickSelect_selectionApplyToIncludeExclude = selectionApplyToInclude.Checked ? 1 : 2;
        }

        private void selectionApplyToCurrentSelectionSet_CheckedChanged(object sender, EventArgs e)
        {
            selectionApplyToBox.Enabled = selectionApplyToCurrentSelectionSet.Checked;
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            try
            {
            List<ElementId> finalSelection = new List<ElementId>();
            propertySet ps = selectionProperties.SelectedItem as propertySet;
            // Final task to select the objects using the filters selected
            switch (selectionOperator.Text)
            {
                case "Select All":
                    foreach (Element element in (selectionCategory.SelectedItem as categorySet).elements) {
                        finalSelection.Add(element.Id);
                    }
                    break;
                case "True":
                    foreach (Element element in ps.elements) {
                        if (element.LookupParameter(ps.name).AsInteger() == 1) {
                            finalSelection.Add(element.Id);
                        }
                    }
                    break;
                case "False":
                    foreach (Element element in ps.elements) {
                        if (element.LookupParameter(ps.name).AsInteger() == 0) {
                            finalSelection.Add(element.Id);
                        }
                    }
                    break;
                case "= Equal":
                    if (ps.storageType == StorageType.Double) {
                        foreach (Element element in ps.elements)
                        {
                            Double pv = ps.parameterType == ParameterType.Angle ? element.LookupParameter(ps.name).AsDouble() * (180.0 / Math.PI) : element.LookupParameter(ps.name).AsDouble() * 304.8;
                            if (Math.Round(pv, 1) == Convert.ToDouble(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    if (ps.storageType == StorageType.ElementId)
                    {
                        foreach (Element element in ps.elements)
                        {
                            if (element.LookupParameter(ps.name).AsValueString() == Convert.ToString(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    if (ps.storageType == StorageType.Integer) {
                        foreach (Element element in ps.elements)
                        {
                            if (element.LookupParameter(ps.name).AsValueString() == Convert.ToString(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    if (ps.storageType == StorageType.String) {
                        foreach (Element element in ps.elements)
                        {
                            if (element.LookupParameter(ps.name).AsString() == Convert.ToString(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    break;
                case "<> Not Equal":
                    if (ps.storageType == StorageType.Double) {
                        foreach (Element element in ps.elements)
                        {
                            Double pv = ps.parameterType == ParameterType.Angle ? element.LookupParameter(ps.name).AsDouble() * (180.0 / Math.PI) : element.LookupParameter(ps.name).AsDouble() * 304.8;
                            if (Math.Round(pv, 1) != Convert.ToDouble(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    if (ps.storageType == StorageType.ElementId)
                    {
                        foreach (Element element in ps.elements)
                        {
                            if (element.LookupParameter(ps.name).AsValueString() != Convert.ToString(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    if (ps.storageType == StorageType.Integer) {
                        foreach (Element element in ps.elements)
                        {
                            if (element.LookupParameter(ps.name).AsInteger() != Convert.ToInt32(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    if (ps.storageType == StorageType.String) {
                        foreach (Element element in ps.elements)
                        {
                            if (element.LookupParameter(ps.name).AsString() != Convert.ToString(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    break;
                case "> Greater Than":
                    if (ps.storageType == StorageType.Double) {
                        foreach (Element element in ps.elements)
                        {
                            Double pv = ps.parameterType == ParameterType.Angle ? element.LookupParameter(ps.name).AsDouble() * (180.0 / Math.PI) : element.LookupParameter(ps.name).AsDouble() * 304.8;
                            if (Math.Round(pv, 1) > Convert.ToDouble(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    if (ps.storageType == StorageType.Integer) {
                        foreach (Element element in ps.elements)
                        {
                            if (element.LookupParameter(ps.name).AsInteger() > Convert.ToInt32(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    break;
                case "< Less Than":
                    if (ps.storageType == StorageType.Double) {
                        foreach (Element element in ps.elements)
                        {
                            Double pv = ps.parameterType == ParameterType.Angle ? element.LookupParameter(ps.name).AsDouble() * (180.0 / Math.PI) : element.LookupParameter(ps.name).AsDouble() * 304.8;
                            if (Math.Round(pv, 1) < Convert.ToDouble(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    if (ps.storageType == StorageType.Integer) {
                        foreach (Element element in ps.elements)
                        {
                            if (element.LookupParameter(ps.name).AsInteger() < Convert.ToInt32(selectionValue.Text))
                            {
                                finalSelection.Add(element.Id);
                            }
                        }
                    }
                    break;
                default:
                break;
            }
            // Add current selected elements to filter
            if (selectionApplyToCurrentSelectionSet.Checked)
            {
                if (selectionApplyToInclude.Checked)
                {
                    foreach (Element element in selectedElements)
                    {
                        finalSelection.Add(element.Id);
                    }
                }
                else
                {
                    List<ElementId> tempSelection = new List<ElementId>();
                    foreach (Element element in selectedElements)
                    {
                        if (!finalSelection.Contains(element.Id)) tempSelection.Add(element.Id);
                    }
                    finalSelection = tempSelection;
                }
            }
            // Select Elements from filter
            uiApp.ActiveUIDocument.Selection.SetElementIds(finalSelection);
            Properties.Settings.Default.Save();
            Close();
            }
            catch (FormatException)
            {
                TaskDialog.Show("Quick Select", "Value not a " + selectionType.Text);
            }
        }
    }
}