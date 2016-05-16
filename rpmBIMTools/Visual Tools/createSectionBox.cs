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
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Utility;

namespace rpmBIMTools
{
    public partial class createSectionBox : System.Windows.Forms.Form
    {
        // Setup basic shared variables
        private Document doc = rpmBIMTools.Load.liveDoc;
        private UIApplication uiApp = rpmBIMTools.Load.uiApp;
        private View3D preview = null;
        private Transaction t = null;
        private PreviewControl previewControl = null;
        private BoundingBoxXYZ sectionBoundingBox = new BoundingBoxXYZ();
        public ICollection<Element> selectedElements = new List<Element>();
        public string dateTimeTag =  DateTime.Now.ToString("yyyyMMddHHmmss");

        public createSectionBox()
        {
            InitializeComponent();
        }

        private void createSectionBox_Load(object sender, EventArgs e)
        {
            t = new Transaction(doc, "Create Section Box");
            t.Start();
            ViewFamilyType vft = rpmBIMUtils.GetViewFamilyType(doc, ViewFamily.ThreeDimensional);
            preview = View3D.CreateIsometric(doc, vft.Id);
            preview.ViewName = "Section Box " + dateTimeTag;
            preview.DetailLevel = ViewDetailLevel.Fine;
            preview.DisplayStyle = DisplayStyle.Realistic;
            // Calculate sectionBoundingBox
            XYZ min = new XYZ( selectedElements.Min(sb => sb.get_BoundingBox(null).Min.X),
                selectedElements.Min(sb => sb.get_BoundingBox(null).Min.Y),
                selectedElements.Min(sb => sb.get_BoundingBox(null).Min.Z));
            XYZ max = new XYZ(selectedElements.Max(sb => sb.get_BoundingBox(null).Max.X),
                selectedElements.Max(sb => sb.get_BoundingBox(null).Max.Y),
                selectedElements.Max(sb => sb.get_BoundingBox(null).Max.Z));
            sectionBoundingBox.Min = min;
            sectionBoundingBox.Max = max;
            preview.SetSectionBox(getBoundingBoxFromElements(0));
            doc.Regenerate();
            // Hide Section and Scope Boxes
            ICollection<ElementId> scopeBoxes = new FilteredElementCollector(doc, preview.Id)
                .OfCategory(BuiltInCategory.OST_VolumeOfInterest)
                .Where(elem => elem.CanBeHidden(preview))
                .Select(elem => elem.Id)
                .ToList();
            if (scopeBoxes.Count() != 0 ) preview.HideElements(scopeBoxes);
            preview.CropBoxVisible = false;
            ICollection<ElementId> sectionBoxes = new FilteredElementCollector(doc, preview.Id)
                .OfCategory(BuiltInCategory.OST_SectionBox)
                .Where(elem => elem.CanBeHidden(preview))
                .Select(elem => elem.Id)
                .ToList();
            if (sectionBoxes.Count() != 0) preview.HideElements(sectionBoxes);
            // Apply preview view to dialog window
            doc.Regenerate();
            previewControl = new PreviewControl(doc, preview.Id);
            previewWindow.Child = previewControl;
        }

        private BoundingBoxXYZ getBoundingBoxFromElements(int offSet)
        {
            BoundingBoxXYZ box = new BoundingBoxXYZ();
            box.Min = new XYZ(sectionBoundingBox.Min.X - offSet, sectionBoundingBox.Min.Y - offSet, sectionBoundingBox.Min.Z - offSet);
            box.Max = new XYZ(sectionBoundingBox.Max.X + offSet, sectionBoundingBox.Max.Y + offSet, sectionBoundingBox.Max.Z + offSet);
            return box;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (preview != null)
            {
                preview.SetSectionBox(getBoundingBoxFromElements(sectionBoxOffset.Value));
                doc.Regenerate();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            t.RollBack();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            // Do checks before making section box
            bool viewExists = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Views)
                .OfClass(typeof(View3D))
                .Cast<View3D>()
                .Any(v => v.ViewName == viewName.Text);
            if (viewExists)
            {
                TaskDialog.Show("Create Section Box", "3D View with the name '" + viewName + "' already exists.");
                return;
            }
            doc.Regenerate();
            View3D SectionBoxView = doc.GetElement(preview.Duplicate(ViewDuplicateOption.Duplicate)) as View3D;
            SectionBoxView.ViewName = viewName.Text;
            doc.Delete(preview.Id);
            t.Commit();
            Close();
            rpmBIMTools.Load.uiApp.ActiveUIDocument.ActiveView = SectionBoxView;
        }

        private void createSectionBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t.GetStatus() == TransactionStatus.Started) { t.RollBack(); }                
        }

        private void viewName_TextChanged(object sender, EventArgs e)
        {
            createButton.Enabled = !string.IsNullOrWhiteSpace(viewName.Text);
        }
    }
}