using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

namespace rpmBIMTools
{
    public partial class issuePlot : System.Windows.Forms.Form
    {
        Autodesk.Revit.DB.Document doc = rpmBIMTools.Load.liveDoc;
        private bool mouseIsDown = false;
        private System.Drawing.Point firstPoint;

        // Creating Empty Binding Lists of Sheets
        private BindingList<DrawingSheet> sheetList = new BindingList<DrawingSheet>();
        private BindingList<DrawingSheet> plotList = new BindingList<DrawingSheet>();
        
        public issuePlot()
        {
            InitializeComponent();
        }

        private void issuePlot_Load(object sender, EventArgs e)
        {
            // Find if sheet already exists
            IEnumerable<ViewSheet> sheetCollector = from elem in new FilteredElementCollector(doc)
                                                       .OfClass(typeof(ViewSheet))
                                                       .OfCategory(BuiltInCategory.OST_Sheets)
                                                let type = elem as ViewSheet
                                                select type;

            this.filterSelectionList.Items.Add(new filter { serviceDescription = "All Services", serviceId = "All" });

            // Populate the drawing and filter list
            foreach (ViewSheet viewSheet in sheetCollector)
            {
                //create an instanze of your customclass
                var sheet = new DrawingSheet();
                string[] temp = viewSheet.SheetNumber.Split(new char[]{'-'});
                sheet.id = viewSheet.Id;
                sheet.service = temp.Count() == 5 ? temp[4].Length == 4 ? temp[4].Substring(0, 2) : null : null;
                sheet.sheetNumber = doc.ProjectInformation.Number + "-NGB-" + viewSheet.SheetNumber;

                if (!String.IsNullOrEmpty(sheet.service))
                {
                    filter filterItem = new filter { serviceDescription = getServiceType(sheet.service), serviceId = sheet.service };
                    if(!filterSelectionList.Items.Contains(filterItem)) {
                        filterSelectionList.Items.Add(filterItem);
                    }
                }
                sheetList.Add(sheet);
            }

            // Drawing listbox settings and datasource
            this.filterSelectionList.DisplayMember = "serviceDescription";
            this.filterSelectionList.ValueMember = "serviceId";
            this.sheetSelectionList.DataSource = sheetList;
            this.sheetSelectionList.DisplayMember = "sheetNumber";
            this.sheetSelectionList.ValueMember = "id";
            this.plotSelectionList.DataSource = plotList;
            this.plotSelectionList.DisplayMember = "sheetNumber";
            this.plotSelectionList.ValueMember = "id";

        }

        public class DrawingSheet
        {
            public ElementId id { get; set; }
            public string sheetNumber { get; set; }
            public string service { get; set; }
        }

        public class filter : IEquatable<filter>
        {
            public string serviceId { get; set; }
            public string serviceDescription { get; set; }

            public bool Equals(filter other)
            {
                return (this.serviceId == other.serviceId);
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return base.Equals(obj);

                if (obj is filter)
                    return this.Equals((filter)obj);
                else
                    return false;
            }


            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

        }

        private string getServiceType(string service)
        {
            return service == "50" ? "Coordinated Mechanical Services" :
                      service == "52" ? "Drainage" :
                      service == "53" ? "Domestic" :
                      service == "54" ? "Gas / Compressed Air / Medical Gas" :
                      service == "55" ? "Chilled Water / Refrigeration" :
                      service == "56" ? "Heating" :
                      service == "57" ? "Ventilation" :
                      service == "60" ? "Coordinated Electrical Services" :
                      service == "61" ? "Main HV / LV Distribution" :
                      service == "62" ? "Small Power (inc Ancillaries)" :
                      service == "63" ? "Lighting" :
                      service == "64" ? "CCTV / TV" :
                      service == "65" ? "Communications" :
                      service == "66" ? "Containment" :
                      service == "67" ? "Fire Alarm" :
                      service == "68" ? "Security / Access Control" :
                      service == "69" ? "Lightning Protection / Earthing" : "Spare (" + service.ToUpper() + ")";
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            firstPoint = e.Location;
            mouseIsDown = true;
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            mouseIsDown = false;
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseIsDown)
            {
                // Get the difference between the two points
                int xDiff = firstPoint.X - e.Location.X;
                int yDiff = firstPoint.Y - e.Location.Y;

                // Set the new point
                int x = this.Location.X - xDiff;
                int y = this.Location.Y - yDiff;
                this.Location = new System.Drawing.Point(x, y);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < sheetSelectionList.Items.Count; i++)
            {
                sheetSelectionList.SetSelected(i, true);
            }
        }

        private void selectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < sheetSelectionList.Items.Count; i++)
            {
                sheetSelectionList.SetSelected(i, false);
            }
        }

        private void selectInvert_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < sheetSelectionList.Items.Count; i++)
            {
                sheetSelectionList.SetSelected(i, !sheetSelectionList.GetSelected(i));
            }
        }
        
        private void listTransferRight_Click(object sender, EventArgs e)
        {
            List<DrawingSheet> toRemove = new List<DrawingSheet>();

            if (sheetSelectionList.Items.Count == 0) return;
            foreach(DrawingSheet li in sheetSelectionList.SelectedItems) {
                toRemove.Add(li);
                plotList.Add(li);
            }
            plotSelectionList.SelectedIndex = -1;
            foreach(DrawingSheet li in toRemove)
            {
                plotSelectionList.SetSelected(plotSelectionList.Items.IndexOf(li), true);
                sheetList.Remove(li);
            }
            sheetSelectionList.SelectedIndex = -1;
        }

        private void sheetSelectionList_DoubleClick(object sender, MouseEventArgs e)
        {
            if (sheetSelectionList.Items.Count == 0) return;
            this.plotList.Add((DrawingSheet)sheetSelectionList.SelectedItem);
            this.sheetList.Remove((DrawingSheet)sheetSelectionList.SelectedItem);
        }

        private void listTransferLeft_Click(object sender, EventArgs e)
        {
            List<DrawingSheet> toRemove = new List<DrawingSheet>();

            if (plotSelectionList.Items.Count == 0) return;
            foreach (DrawingSheet li in plotSelectionList.SelectedItems)
            {
                toRemove.Add(li);
                sheetList.Add(li);
            }
            sheetSelectionList.SelectedIndex = -1;
            foreach (DrawingSheet li in toRemove)
            {
                sheetSelectionList.SetSelected(sheetSelectionList.Items.IndexOf(li), true);
                plotList.Remove(li);
            }

            plotSelectionList.SelectedIndex = -1;
        }

        private void toPlotSelectionList_DoubleClick(object sender, MouseEventArgs e)
        {
            if (plotSelectionList.Items.Count == 0) return;
            this.sheetList.Add((DrawingSheet)plotSelectionList.SelectedItem);
            this.plotList.Remove((DrawingSheet)plotSelectionList.SelectedItem);
        }

        private void filterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaskDialog.Show("IssuePlot", "Service Selected: " + (this.filterSelectionList.SelectedItem as filter).serviceId);
        }

        private void startIssuePlot_Click(object sender, EventArgs e)
        {

        }

        private void startCheckPlot_Click(object sender, EventArgs e)
        {

        }

    }
}
