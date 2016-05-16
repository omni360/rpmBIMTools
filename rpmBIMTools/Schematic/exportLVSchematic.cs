using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.Exceptions;

namespace rpmBIMTools
{
    public partial class exportLVSchematic : System.Windows.Forms.Form
    {
        // Command Variables
        Document doc = rpmBIMTools.Load.liveDoc;
        UIApplication uiApp = rpmBIMTools.Load.uiApp;
        IEnumerable<FamilyInstance> schematicReferences;

        public exportLVSchematic()
        {
            InitializeComponent();
        }

        private void viewBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.viewBox.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                exportButton_Click(null, null);
            }
        }

        private void exportLVSchematicUI_Load(object sender, EventArgs e)
        {
            IList<LVSchematicView> views = new List<LVSchematicView>();
            viewBox.DisplayMember = "ViewName";
            viewBox.ValueMember = "Id";
            // Populate views
            foreach(FamilyInstance schematicReference in schematicReferences)
            {
                Autodesk.Revit.DB.View view = doc.GetElement(schematicReference.OwnerViewId) as Autodesk.Revit.DB.View;
                if (view.ViewType == ViewType.DraftingView)
                {
                    viewBox.Items.Add(new LVSchematicView { viewName = view.ViewName, view = view });
                }
            }
            viewBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Class for storing views in the dialog listbox
        /// </summary>
        private class LVSchematicView
        {
            public string viewName { get; set; }
            public Autodesk.Revit.DB.View view { get; set; }
        }

        /// <summary>
        /// Main LV Schematic Export Script
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportButton_Click(object sender, EventArgs e)
        {
            // Get Selected Schematic View To Export
            Autodesk.Revit.DB.View schematicView = (viewBox.SelectedItem as LVSchematicView).view;

            bool schedule = false;
            // Finding Sheet (if exists)
            ViewSheet viewSheet = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .FirstOrDefault(v => v.SheetNumber == schematicView.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NUMBER).AsString());
            // Finding Bus2 Link Bridge (if exists)
            FamilyInstance bus2LinkBridge = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .FirstOrDefault(fi => fi.Name == "SM - Device - Bus Link Bridge" && Math.Round((fi.Location as LocationPoint).Rotation * (180.0 / Math.PI)) == 90);
            // Finding Schedule (if exists)
            if (viewSheet != null)
            {
                schedule = new FilteredElementCollector(doc, viewSheet.Id)
                    .OfClass(typeof(ScheduleSheetInstance))
                    .Cast<ScheduleSheetInstance>()
                    .Any<ScheduleSheetInstance>(s => s.IsTitleblockRevisionSchedule == false);
            }
            // Finding Legend View
            ElementId legendId = viewSheet != null ? viewSheet.GetAllPlacedViews().FirstOrDefault(l => (doc.GetElement(l) as Autodesk.Revit.DB.View).ViewType == ViewType.Legend) : null;

            // Set Internal/External Distance
            double internalExternalLine = bus2LinkBridge != null ? (bus2LinkBridge.Location as LocationPoint).Point.Y * 304.8 - 110 : -110;

            // Collect all circuit references (ordered from left to right on view)
            IEnumerable<FamilyInstance> circuits = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(c => c.Name == "SM - Annotation - Circuit Reference Symbol" && c.LookupParameter("cableReference").AsString()[0] != 'L' && c.LookupParameter("schematicReference").AsString() == schematicView.ViewName)
                .OrderBy(c => (c.Location as LocationPoint).Point.X, Comparer<double>.Default);
            // Collect all life safety circuit references
            IEnumerable<FamilyInstance> lsCircuits = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(c => c.Name == "SM - Annotation - Circuit Reference Symbol" && c.LookupParameter("cableReference").AsString()[0] == 'L' && c.LookupParameter("schematicReference").AsString() == schematicView.ViewName)
                .OrderBy(c => (c.Location as LocationPoint).Point.X, Comparer<double>.Default);
            // Collect all devices
            IEnumerable<FamilyInstance> devices = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(d => d.Name == "SM - Device - Air Circuit Breaker" || d.Name == "SM - Device - Moulded Case Circuit Breaker" || d.Name == "SM - Device - Fused Switch Disconnector" || d.Name == "SM - Device - Miniture Circuit Breaker" || d.Name == "SM - Device - Undefined");
            // Collect all life safety devices
            IEnumerable<FamilyInstance> lsDevices = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(d => d.Name == "SM - Device - Fused Disconnector");
            // Collect all connected loads
            IEnumerable<FamilyInstance> connectedLoads = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(cl => cl.Name == "SM - Equipment - Isolator" || cl.Name == "SM - Equipment - Spare" || cl.Name == "SM - Equipment - Distribution Board" || cl.Name == "SM - Equipment - Sub-Panel Board");
            // Collect all relays
            IEnumerable<FamilyInstance> relays = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(r => r.Name == "SM - Annotation - Circuit Reference Symbol" && (r.LookupParameter("cableReference").AsString() == "RE" || r.LookupParameter("cableReference").AsString() == "OC" || r.LookupParameter("cableReference").AsString() == "EF"));
            // Collect all Link Bridges to generators and transformers
            IEnumerable<FamilyInstance> linkBridges = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(fi => fi.Name == "SM - Device - Bus Link Bridge" && (fi.Location as LocationPoint).Rotation == 0);
            // Collect all Power Factor Correction Steps
            IEnumerable<FamilyInstance> powerFactors = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(fi => fi.Name == "SM - Device - Correction Device" || fi.Name == "SM - Device - Correction Device (with detuning)");
            // Collect all Power Factor Correction TextNotes
            IEnumerable<TextNote> powerFactorNotes = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(TextNote))
                .Cast<TextNote>()
                .Where(tn => tn.Text.Contains("Steps") == true);
            // Colleect all Generators
            IEnumerable<FamilyInstance> generators = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(g => g.Name == "SM - Equipment - Generator");
            // Collect all Transformers
            IEnumerable<FamilyInstance> transformers = new FilteredElementCollector(doc, schematicView.Id)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(g => g.Name == "SM - Equipment - Transformer");

            // Family Document Check
            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Export LV Schematic", "Cannot be used in family editor");
            }
            else
            {
                // Close Dialog
                this.Hide();
                this.Close();

                // Get XML Save Location
                SaveFileDialog xmlPath = new SaveFileDialog();
                xmlPath.Title = "Export LV Schematic File";
                xmlPath.FileName = schematicView.ViewName;
                xmlPath.Filter = "XML File (*.xml)|*.xml";
                if (xmlPath.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                // Start Export LV Schematic Process
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter xml = XmlWriter.Create(xmlPath.InitialDirectory + xmlPath.FileName, settings);
                xml.WriteStartDocument();
                xml.WriteComment("File created by rpmBIMTools LV Schematic exporter");

                // Start Schematic
                xml.WriteStartElement("schematic");
                xml.WriteElementString("name", schematicView.ViewName);

                // Start Settings
                xml.WriteStartElement("settings");
                bool includeSheet = schematicView.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NUMBER).HasValue;
                xml.WriteElementString("includeSheet", includeSheet.ToString().ToLower());
                xml.WriteElementString("includeTerminations", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().Any(fi => fi.Name == "SM - Device - Cable Termination").ToString().ToLower());
                xml.WriteElementString("includeSchedule", schedule.ToString().ToLower());
                xml.WriteElementString("includeSurgeSuppression", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(TextNote)).Cast<TextNote>().Any(tn => tn.Text == "Surge\r\nSuppression").ToString().ToLower());
                xml.WriteElementString("includeExternalEarthBar", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().Any(fi => fi.Name == "SM - Equipment - Earth Bar" && (fi.Location as LocationPoint).Point.Y * 304.8 < internalExternalLine).ToString().ToLower());
                xml.WriteElementString("includeInternalEarthBar", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().Any(fi => fi.Name == "SM - Equipment - Earth Bar" && (fi.Location as LocationPoint).Point.Y * 304.8 > internalExternalLine).ToString().ToLower());
                xml.WriteElementString("includeInterTripping", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().Any(fi => fi.Name == "SM - Equipment - Inter-tripping").ToString().ToLower());
                xml.WriteElementString("includeMeteringOnMain", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().Any(fi => fi.Name == "SM - Device - Meter Connection").ToString().ToLower());
                xml.WriteElementString("includeRestrictedEarthFaultRelay", relays.Any(fi => fi.LookupParameter("cableReference").AsString() == "RE").ToString().ToLower());
                xml.WriteElementString("includeOvercurrentRelay", relays.Any(fi => fi.LookupParameter("cableReference").AsString() == "OC").ToString().ToLower());
                xml.WriteElementString("includeEarthFaultRelay", relays.Any(fi => fi.LookupParameter("cableReference").AsString() == "EF").ToString().ToLower());
                xml.WriteElementString("includeNeutralEarthLink", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().Any(fi => fi.Name == "SM - Device - Netrual Earth Link" || fi.Name == "SM - Equipment - Netrual Earth Link Bar").ToString().ToLower());
                xml.WriteElementString("includeCastellInterlock", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().Any(fi => fi.Name == "SM - Annotation - Castell Interlock").ToString().ToLower());
                xml.WriteElementString("includeElectricalInterlock", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(TextNote)).Cast<TextNote>().Any(tn => tn.Text == "Electrical\r\nInterlock" || tn.Text == "Elec/Mech\r\nInterlock").ToString().ToLower());
                xml.WriteElementString("includeMechanicalInterlock", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(TextNote)).Cast<TextNote>().Any(tn => tn.Text == "Mechanical\r\nInterlock" || tn.Text == "Elec/Mech\r\nInterlock").ToString().ToLower());
                xml.WriteEndElement();
                // End Settings

                // Start Bus 1
                xml.WriteStartElement("bus1");

                // Start Circuit Loop
                IEnumerable<FamilyInstance> bus1circuits = bus2LinkBridge == null ? circuits : circuits.Where(fi => (fi.Location as LocationPoint).Point.X * 304.8 < (bus2LinkBridge.Location as LocationPoint).Point.X);
                foreach (FamilyInstance circuit in bus1circuits)
                {
                    // Start Circuit
                    xml.WriteStartElement("circuit");

                    // Get Circuit X Axis location
                    double circuitPosition = (circuit.Location as LocationPoint).Point.X;

                    // Start Circuit Information
                    xml.WriteElementString("ref", circuit.LookupParameter("cableReference").AsStringOrDefault());
                    xml.WriteElementString("size", circuit.LookupParameter("cableSize").AsStringOrDefault());
                    xml.WriteElementString("type", circuit.LookupParameter("cableType").AsStringOrDefault());
                    xml.WriteElementString("length", circuit.LookupParameter("cableLength").AsStringOrDefault());
                    xml.WriteElementString("description", circuit.LookupParameter("cableDescription").AsStringOrDefault());
                    // End Circuit Information

                    // Start Circuit Metered Check
                    xml.WriteElementString("metered", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().Any(fi => fi.Name == "SM - Device - Metered Symbol" && (fi.Location as LocationPoint).Point.X == circuitPosition).ToString().ToLower());
                    // End Circuit Metered Check

                    // Start Device
                    FamilyInstance device = devices.Where(d => (d.Location as LocationPoint).Point.X == circuitPosition).FirstOrDefault();
                    xml.WriteStartElement("device");
                    xml.WriteElementString("type", device != null ? device.Name : "SM - Device - Undefined");
                    xml.WriteElementString("ref", device != null ? device.LookupParameter("Ref").AsStringOrDefault() : "");
                    xml.WriteElementString("frame", device != null ? device.LookupParameter("Frame").AsStringOrDefault() : "");
                    xml.WriteElementString("rating", device != null ? device.LookupParameter("Rating").AsStringOrDefault() : "");
                    xml.WriteElementString("poles", device != null ? device.LookupParameter("Poles").AsStringOrDefault() : "");
                    xml.WriteEndElement();
                    // End Device

                    // Start Connected Load
                    FamilyInstance connectedLoad = connectedLoads.Where(cl => (cl.Location as LocationPoint).Point.X == circuitPosition).FirstOrDefault();
                    xml.WriteStartElement("connectedLoad");
                    xml.WriteElementString("type", connectedLoad != null ? connectedLoad.Name : "SM - Equipment - Spare");
                    if (connectedLoad != null)
                    {
                        if (connectedLoad.Name == "SM - Equipment - Distribution Board" || connectedLoad.Name == "SM - Equipment - Sub-Panel Board")
                        {
                            xml.WriteElementString("dbReference", connectedLoad != null ? connectedLoad.LookupParameter("DB Reference").AsStringOrDefault() : "");
                            xml.WriteElementString("dbWays", connectedLoad != null ? connectedLoad.LookupParameter("DB Ways").AsStringOrDefault() : "");
                            xml.WriteElementString("dbRating", connectedLoad != null ? connectedLoad.LookupParameter("DB Rating").AsStringOrDefault() : "");
                            xml.WriteElementString("dbPhase", connectedLoad != null ? connectedLoad.LookupParameter("DB Phase").AsStringOrDefault() : "");
                            xml.WriteElementString("dbIF", connectedLoad != null ? connectedLoad.LookupParameter("DB IF").AsStringOrDefault() : "");
                            xml.WriteElementString("dbIB", connectedLoad != null ? connectedLoad.LookupParameter("DB IB").AsStringOrDefault() : "");
                            xml.WriteElementString("dbZS", connectedLoad != null ? connectedLoad.LookupParameter("DB ZS").AsStringOrDefault() : "");
                        }
                    }
                    xml.WriteEndElement();
                    // End Connected load

                    xml.WriteEndElement();
                    // End Circuit
                }
                // End Circuit Loop

                // Start Transformer Existance Check
                FamilyInstance bus1Transformer = bus2LinkBridge == null ? transformers.FirstOrDefault() : transformers.FirstOrDefault(t => (t.Location as LocationPoint).Point.X < (bus2LinkBridge.Location as LocationPoint).Point.X);
                if (bus1Transformer != null)
                {
                    // Start Transformer
                    xml.WriteStartElement("supplyTransformer");

                    // Start Internal / External Check
                    xml.WriteAttributeString("location", (bus1Transformer.Location as LocationPoint).Point.Y * 304.8 < internalExternalLine ? "External" : "Internal");
                    // End Internal / External Check

                    // Start Transformer Information
                    xml.WriteElementString("ref", bus1Transformer.LookupParameter("Ref").AsStringOrDefault());
                    xml.WriteElementString("rating", bus1Transformer.LookupParameter("Rating").AsStringOrDefault());
                    xml.WriteElementString("voltage", bus1Transformer.LookupParameter("Voltage").AsStringOrDefault());
                    xml.WriteElementString("vectorGroup", bus1Transformer.LookupParameter("Vector Group").AsStringOrDefault());
                    xml.WriteElementString("type", bus1Transformer.LookupParameter("Type").AsStringOrDefault());
                    xml.WriteElementString("cooling", bus1Transformer.LookupParameter("Cooling").AsStringOrDefault());
                    // End Transformer Information

                    // Start Link Bridge Information
                    FamilyInstance linkBridge = linkBridges.Where(lb => (lb.Location as LocationPoint).Point.X == (bus1Transformer.Location as LocationPoint).Point.X).FirstOrDefault();
                    xml.WriteStartElement("linkBridge");
                    xml.WriteElementString("ref", linkBridge != null ? linkBridge.LookupParameter("Ref").AsStringOrDefault() : "");
                    xml.WriteElementString("frame", linkBridge != null ? linkBridge.LookupParameter("Frame").AsStringOrDefault() : "");
                    xml.WriteElementString("rating", linkBridge != null ? linkBridge.LookupParameter("Rating").AsStringOrDefault() : "");
                    xml.WriteElementString("poles", linkBridge != null ? linkBridge.LookupParameter("Poles").AsStringOrDefault() : "");
                    xml.WriteEndElement();
                    // End Link Bridge Information

                    xml.WriteEndElement();
                    // End Transformer
                }
                // End Transformer Existance Check

                // Start Generator Existance Check
                FamilyInstance bus1Generator = bus2LinkBridge == null ? generators.FirstOrDefault() : generators.FirstOrDefault(g => (g.Location as LocationPoint).Point.X < (bus2LinkBridge.Location as LocationPoint).Point.X);
                if (bus1Generator != null)
                {
                    // Start Generator
                    xml.WriteStartElement("supplyGenerator");

                    // Start Internal / External Check
                    xml.WriteAttributeString("location", (bus1Generator.Location as LocationPoint).Point.Y * 304.8 < internalExternalLine ? "External" : "Internal");
                    // End Internal / External Check

                    // Start Generator Information
                    xml.WriteElementString("ref", bus1Generator.LookupParameter("Ref").AsStringOrDefault());
                    xml.WriteElementString("rating", bus1Generator.LookupParameter("Rating").AsStringOrDefault());
                    // End Generator Information

                    // Start Link Bridge Information
                    FamilyInstance linkBridge = linkBridges.Where(lb => (lb.Location as LocationPoint).Point.X == (bus1Generator.Location as LocationPoint).Point.X).FirstOrDefault();
                    xml.WriteStartElement("linkBridge");
                    xml.WriteElementString("ref", linkBridge != null ? linkBridge.LookupParameter("Ref").AsStringOrDefault() : "");
                    xml.WriteElementString("frame", linkBridge != null ? linkBridge.LookupParameter("Frame").AsStringOrDefault() : "");
                    xml.WriteElementString("rating", linkBridge != null ? linkBridge.LookupParameter("Rating").AsStringOrDefault() : "");
                    xml.WriteElementString("poles", linkBridge != null ? linkBridge.LookupParameter("Poles").AsStringOrDefault() : "");
                    xml.WriteEndElement();
                    // End Link Bridge Information

                    xml.WriteEndElement();
                    // End Generator
                }
                // End Generator Existance Check

                // Start Power Factor Correction Existance Check
                IEnumerable<FamilyInstance> bus1PowerFactors = bus2LinkBridge == null ? powerFactors.Take(10) : powerFactors.Where(fi => (fi.Location as LocationPoint).Point.X < (bus2LinkBridge.Location as LocationPoint).Point.X).Take(10);
                if (bus1PowerFactors.Count() != 0)
                {
                    // Start Power Factor Correction
                    xml.WriteStartElement("powerFactorCorrection");
                    xml.WriteElementString("type", bus1PowerFactors.First().Name);
                    // Start Power Factor Correction Information
                    TextNote bus1PowerFactorText = bus2LinkBridge == null ? powerFactorNotes.FirstOrDefault() : powerFactorNotes.FirstOrDefault(tn => (tn.Coord.X < (bus2LinkBridge.Location as LocationPoint).Point.X));
                    xml.WriteElementString("total", bus1PowerFactorText != null ? new String(bus1PowerFactorText.Text.TakeWhile(char.IsDigit).ToArray()) : "0");
                    xml.WriteElementString("steps", bus1PowerFactors.Count().ToString());
                    // End Power Factor Correction Information
                    xml.WriteEndElement();
                    // End Power Factor Correction
                }
                // End Power Factor Correction Existance Check

                xml.WriteEndElement();
                // End Bus 1

                // Start Bus 2 Existance Check
                if (bus2LinkBridge != null)
                {
                    // Start Bus 2
                    xml.WriteStartElement("bus2");
                    // Start Bus 2 Loop
                    IEnumerable<FamilyInstance> bus2circuits = circuits.Where(fi => (fi.Location as LocationPoint).Point.X * 304.8 > (bus2LinkBridge.Location as LocationPoint).Point.X);
                    foreach (FamilyInstance circuit in bus2circuits)
                    {
                        // Start Circuit
                        xml.WriteStartElement("circuit");

                        // Get Circuit X Axis location
                        double circuitPosition = (circuit.Location as LocationPoint).Point.X;

                        // Start Circuit Information
                        xml.WriteElementString("ref", circuit.LookupParameter("cableReference").AsStringOrDefault());
                        xml.WriteElementString("size", circuit.LookupParameter("cableSize").AsStringOrDefault());
                        xml.WriteElementString("type", circuit.LookupParameter("cableType").AsStringOrDefault());
                        xml.WriteElementString("length", circuit.LookupParameter("cableLength").AsStringOrDefault());
                        xml.WriteElementString("description", circuit.LookupParameter("cableDescription").AsStringOrDefault());
                        // End Circuit Information

                        // Start Circuit Metered Check
                        xml.WriteElementString("metered", new FilteredElementCollector(doc, schematicView.Id).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().Any(fi => fi.Name == "SM - Device - Metered Symbol" && (fi.Location as LocationPoint).Point.X == circuitPosition).ToString().ToLower());
                        // End Circuit Metered Check

                        // Start Device
                        FamilyInstance device = devices.Where(d => (d.Location as LocationPoint).Point.X == circuitPosition).FirstOrDefault();
                        xml.WriteStartElement("device");
                        xml.WriteElementString("type", device != null ? device.Name : "SM - Device - Undefined");
                        xml.WriteElementString("ref", device != null ? device.LookupParameter("Ref").AsStringOrDefault() : "");
                        xml.WriteElementString("frame", device != null ? device.LookupParameter("Frame").AsStringOrDefault() : "");
                        xml.WriteElementString("rating", device != null ? device.LookupParameter("Rating").AsStringOrDefault() : "");
                        xml.WriteElementString("poles", device != null ? device.LookupParameter("Poles").AsStringOrDefault() : "");
                        xml.WriteEndElement();
                        // End Device

                        // Start Connected Load
                        FamilyInstance connectedLoad = connectedLoads.Where(cl => (cl.Location as LocationPoint).Point.X == circuitPosition).FirstOrDefault();
                        xml.WriteStartElement("connectedLoad");
                        xml.WriteElementString("type", connectedLoad != null ? connectedLoad.Name : "SM - Equipment - Spare");
                        if (connectedLoad != null)
                        {
                            if (connectedLoad.Name == "SM - Equipment - Distribution Board" || connectedLoad.Name == "SM - Equipment - Sub-Panel Board")
                            {
                                xml.WriteElementString("dbReference", connectedLoad != null ? connectedLoad.LookupParameter("DB Reference").AsStringOrDefault() : "");
                                xml.WriteElementString("dbWays", connectedLoad != null ? connectedLoad.LookupParameter("DB Ways").AsStringOrDefault() : "");
                                xml.WriteElementString("dbRating", connectedLoad != null ? connectedLoad.LookupParameter("DB Rating").AsStringOrDefault() : "");
                                xml.WriteElementString("dbPhase", connectedLoad != null ? connectedLoad.LookupParameter("DB Phase").AsStringOrDefault() : "");
                                xml.WriteElementString("dbIF", connectedLoad != null ? connectedLoad.LookupParameter("DB IF").AsStringOrDefault() : "");
                                xml.WriteElementString("dbIB", connectedLoad != null ? connectedLoad.LookupParameter("DB IB").AsStringOrDefault() : "");
                                xml.WriteElementString("dbZS", connectedLoad != null ? connectedLoad.LookupParameter("DB ZS").AsStringOrDefault() : "");
                            }
                        }
                        xml.WriteEndElement();
                        // End Connected load

                        xml.WriteEndElement();
                        // End Circuit
                    }
                    // End Bus 2 Loop

                    // Start Transformer Existance Check
                    FamilyInstance bus2Transformer = transformers.FirstOrDefault(t => (t.Location as LocationPoint).Point.X > (bus2LinkBridge.Location as LocationPoint).Point.X);
                    if (bus2Transformer != null)
                    {
                        // Start Transformer
                        xml.WriteStartElement("supplyTransformer");

                        // Start Internal / External Check
                        xml.WriteAttributeString("location", (bus2Transformer.Location as LocationPoint).Point.Y * 304.8 < internalExternalLine ? "External" : "Internal");
                        // End Internal / External Check

                        // Start Transformer Information
                        xml.WriteElementString("ref", bus2Transformer.LookupParameter("Ref").AsStringOrDefault());
                        xml.WriteElementString("rating", bus2Transformer.LookupParameter("Rating").AsStringOrDefault());
                        xml.WriteElementString("voltage", bus2Transformer.LookupParameter("Voltage").AsStringOrDefault());
                        xml.WriteElementString("vectorGroup", bus2Transformer.LookupParameter("Vector Group").AsStringOrDefault());
                        xml.WriteElementString("type", bus2Transformer.LookupParameter("Type").AsStringOrDefault());
                        xml.WriteElementString("cooling", bus2Transformer.LookupParameter("Cooling").AsStringOrDefault());
                        // End Transformer Information

                        // Start Link Bridge Information
                        FamilyInstance linkBridge = linkBridges.Where(lb => (lb.Location as LocationPoint).Point.X == (bus2Transformer.Location as LocationPoint).Point.X).FirstOrDefault();
                        xml.WriteStartElement("linkBridge");
                        xml.WriteElementString("ref", linkBridge != null ? linkBridge.LookupParameter("Ref").AsStringOrDefault() : "");
                        xml.WriteElementString("frame", linkBridge != null ? linkBridge.LookupParameter("Frame").AsStringOrDefault() : "");
                        xml.WriteElementString("rating", linkBridge != null ? linkBridge.LookupParameter("Rating").AsStringOrDefault() : "");
                        xml.WriteElementString("poles", linkBridge != null ? linkBridge.LookupParameter("Poles").AsStringOrDefault() : "");
                        xml.WriteEndElement();
                        // End Link Bridge Information

                        xml.WriteEndElement();
                        // End Transformer
                    }
                    // End Transformer Existance Check

                    // Start Generator Existance Check
                    FamilyInstance bus2Generator = generators.FirstOrDefault(g => (g.Location as LocationPoint).Point.X > (bus2LinkBridge.Location as LocationPoint).Point.X);
                    if (bus2Generator != null)
                    {
                        // Start Generator
                        xml.WriteStartElement("supplyGenerator");

                        // Start Internal / External Check
                        xml.WriteAttributeString("location", (bus2Generator.Location as LocationPoint).Point.Y * 304.8 < internalExternalLine ? "External" : "Internal");
                        // End Internal / External Check

                        // Start Generator Information
                        xml.WriteElementString("ref", bus2Generator.LookupParameter("Ref").AsStringOrDefault());
                        xml.WriteElementString("rating", bus2Generator.LookupParameter("Rating").AsStringOrDefault());
                        // End Generator Information

                        // Start Link Bridge Information
                        FamilyInstance linkBridge = linkBridges.Where(lb => (lb.Location as LocationPoint).Point.X == (bus2Generator.Location as LocationPoint).Point.X).FirstOrDefault();
                        xml.WriteStartElement("linkBridge");
                        xml.WriteElementString("ref", linkBridge != null ? linkBridge.LookupParameter("Ref").AsStringOrDefault() : "");
                        xml.WriteElementString("frame", linkBridge != null ? linkBridge.LookupParameter("Frame").AsStringOrDefault() : "");
                        xml.WriteElementString("rating", linkBridge != null ? linkBridge.LookupParameter("Rating").AsStringOrDefault() : "");
                        xml.WriteElementString("poles", linkBridge != null ? linkBridge.LookupParameter("Poles").AsStringOrDefault() : "");
                        xml.WriteEndElement();
                        // End Link Bridge Information

                        xml.WriteEndElement();
                        // End Generator
                    }
                    // End Generator Existance Check

                    // Start Power Factor Correction Existance Check
                    IEnumerable<FamilyInstance> bus2PowerFactors = powerFactors.Where(fi => (fi.Location as LocationPoint).Point.X > (bus2LinkBridge.Location as LocationPoint).Point.X).Take(10);
                    if (bus2PowerFactors.Count() != 0)
                    {
                        // Start Power Factor Correction
                        xml.WriteStartElement("powerFactorCorrection");
                        xml.WriteElementString("type", bus1PowerFactors.First().Name);
                        // Start Power Factor Correction Information
                        TextNote bus2PowerFactorText = powerFactorNotes.FirstOrDefault(tn => (tn.Coord.X > (bus2LinkBridge.Location as LocationPoint).Point.X));
                        xml.WriteElementString("total", bus2PowerFactorText != null ? new String(bus2PowerFactorText.Text.TakeWhile(char.IsDigit).ToArray()) : "0");
                        xml.WriteElementString("steps", bus2PowerFactors.Count().ToString());
                        // End Power Factor Correction Information
                        xml.WriteEndElement();
                        // End Power Factor Correction
                    }
                    // End Power Factor Correction Existance Check

                    xml.WriteEndElement();
                    // End Bus 2
                }
                // End Bus 2 Existance Check

                // Start Life Safety Circuit Existance Check
                if (lsCircuits.Count() != 0)
                {
                    // Start Life Safety Section
                    xml.WriteStartElement("lifeSafetySection");

                    // Start Life Safety Loop
                    foreach (FamilyInstance lsCircuit in lsCircuits)
                    {
                        // Start Circuit
                        xml.WriteStartElement("lsCircuit");

                        // Get Circuit X Axis location
                        double circuitPosition = (lsCircuit.Location as LocationPoint).Point.X;

                        // Start Circuit Information
                        xml.WriteElementString("ref", lsCircuit.LookupParameter("cableReference").AsStringOrDefault());
                        xml.WriteElementString("size", lsCircuit.LookupParameter("cableSize").AsStringOrDefault());
                        xml.WriteElementString("type", lsCircuit.LookupParameter("cableType").AsStringOrDefault());
                        xml.WriteElementString("length", lsCircuit.LookupParameter("cableLength").AsStringOrDefault());
                        xml.WriteElementString("description", lsCircuit.LookupParameter("cableDescription").AsStringOrDefault());
                        // End Circuit Information

                        // Start Device
                        FamilyInstance lsDevice = lsDevices.Where(d => (d.Location as LocationPoint).Point.X == circuitPosition).FirstOrDefault();
                        xml.WriteStartElement("device");
                        xml.WriteElementString("type", lsDevice != null ? lsDevice.Name : "SM - Device - Fused Disconnector");
                        xml.WriteElementString("ref", lsDevice != null ? lsDevice.LookupParameter("Ref").AsStringOrDefault() : "");
                        xml.WriteElementString("frame", lsDevice != null ? lsDevice.LookupParameter("Frame").AsStringOrDefault() : "");
                        xml.WriteElementString("rating", lsDevice != null ? lsDevice.LookupParameter("Rating").AsStringOrDefault() : "");
                        xml.WriteElementString("poles", lsDevice != null ? lsDevice.LookupParameter("Poles").AsStringOrDefault() : "");
                        xml.WriteEndElement();
                        // End Device

                        xml.WriteEndElement();
                        // End Circuit
                    }
                    // End Life Safety Loop
                    xml.WriteEndElement();
                    // Start Life Safety Section
                }
                // End Life Safety Check

                // Start Notes Existance Check
                if (legendId != null)
                {
                    TextNote notes = new FilteredElementCollector(doc, legendId)
                        .OfClass(typeof(TextNote))
                        .Cast<TextNote>()
                        .OrderBy(tn => tn.Coord.Y, Comparer<double>.Default)
                        .FirstOrDefault();
                    if (notes != null)
                    {
                        // Start Notes
                        xml.WriteStartElement("notes");

                        // Start Notes Loop
                        foreach (string note in notes.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            // Start Note
                            xml.WriteElementString("note", note);
                            // End Note
                        }
                        // End Notes Loop

                        xml.WriteEndElement();
                        // End Notes
                    }
                }
                // End Notes Existance Check

                xml.WriteEndElement();
                // End Schematic

                xml.WriteEndDocument();
                xml.Flush();
                xml.Close();

            }
            TaskDialog.Show("Export LV Schematic", "Export process complete!");
        }
    }
}
