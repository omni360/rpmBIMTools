using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace rpmBIMTools
{
    public partial class purgeScopeBox : System.Windows.Forms.Form
    {
        // Setup basic shared variables
        private Document doc = rpmBIMTools.Load.liveDoc;

        public purgeScopeBox()
        {
            InitializeComponent();
        }

        private void purgeScopeBox_Load(object sender, EventArgs e)
        {
            // Built collection of all scopeboxes
            foreach (Element sb in
                new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory
                .OST_VolumeOfInterest).ToElements()) {
                    scopeBoxGrid.Rows.Add(sb.Id, false, sb.Name, 0, Properties.Resources.ErrorElements16);
            }

            // Calculate scope box usage counters
            foreach (Autodesk.Revit.DB.View view in
                new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Views)
                .OfClass(typeof(Autodesk.Revit.DB.View))
                .Cast<Autodesk.Revit.DB.View>()
                .ToList()) {
                if (view.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP) != null)
                {
                    ElementId scopeBoxId = view.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP).AsElementId();
                    if (scopeBoxId != ElementId.InvalidElementId)
                    {
                         DataGridViewRow row = scopeBoxGrid.Rows.Cast<DataGridViewRow>()
                             .Where(r => r.Cells[0].Value.ToString().Equals(scopeBoxId.ToString()))
                             .First();
                        scopeBoxGrid.Rows[row.Index].Cells[3].Value = (int)scopeBoxGrid.Rows[row.Index].Cells[3].Value + 1;
                    }
                }
            }

            // updaate usage icons
            foreach (DataGridViewRow row in scopeBoxGrid.Rows) {
                if ((int)row.Cells[3].Value > 0)
                    row.Cells[4].Value = Properties.Resources.LinkedElements16;
                else
                    purgeUnusedButton.Enabled = true;
            }

            // Apply Default Name Sorting Order
            scopeBoxGrid.Sort(scopeBoxGrid.Columns[2], ListSortDirection.Ascending);

            // Add checkbox to header
            System.Drawing.Rectangle rect = scopeBoxGrid.GetCellDisplayRectangle(0, -1, true);
            rect.Y = 3;
            rect.X = rect.Location.X + (rect.Width / 4) + 10;
            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.Location = rect.Location;
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);
            scopeBoxGrid.Controls.Add(checkboxHeader);
        }

        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerBox = ((CheckBox)scopeBoxGrid.Controls.Find("checkboxHeader", true)[0]);
            foreach (DataGridViewRow row in scopeBoxGrid.Rows)
            {
                row.Cells[1].Value = headerBox.Checked;
            }
            scopeBoxGrid.EndEdit();
        }

        private void updateFilter(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in scopeBoxGrid.Rows)
            {
                row.Visible = row.Cells[2].Value.ToString().ToLower().Contains(filterByName.Text.ToLower()) &&
                ((inUseCheck.Checked && (int)row.Cells[3].Value > 0) ||
                (unusedCheck.Checked && (int)row.Cells[3].Value == 0));
            }
        }

        private void purgeUnused(object sender, EventArgs e)
        {
            using (Transaction t = new Transaction(doc, "Purge Unused Scope Boxes"))
            {
                t.Start();
                foreach (DataGridViewRow row in scopeBoxGrid.Rows)
                {
                    if ((int)row.Cells[3].Value == 0)
                        doc.Delete((ElementId)row.Cells[0].Value);
                }
                Close();
                t.Commit();
            }
        }

        private void purgeSelected(object sender, EventArgs e)
        {
            using (Transaction t = new Transaction(doc, "Purge Selected Scope Boxes"))
            {
                t.Start();
                foreach (DataGridViewRow row in scopeBoxGrid.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[1].Value) == true)
                        doc.Delete((ElementId)row.Cells[0].Value);
                }
                Close();
                t.Commit();
            }
        }

        private void checkBoxChanged(object sender, DataGridViewCellEventArgs e)
        {
            bool trueFlag = false;
            foreach (DataGridViewRow row in scopeBoxGrid.Rows)
            {
                if (Convert.ToBoolean(row.Cells[1].Value) == true) trueFlag = true;
            }
            purgeSelectedButton.Enabled = trueFlag;
        }

        private void scopeBoxGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (scopeBoxGrid.CurrentCell.ColumnIndex == 1 && scopeBoxGrid.IsCurrentCellDirty)
            {
                scopeBoxGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}