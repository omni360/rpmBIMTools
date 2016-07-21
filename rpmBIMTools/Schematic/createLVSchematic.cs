using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.Exceptions;

namespace rpmBIMTools
{
    public partial class createLVSchematic : System.Windows.Forms.Form
    {
        // Setup basic shared variables
        Document doc = rpmBIMTools.Load.liveDoc;
        public ViewDrafting draftView;
        public Autodesk.Revit.DB.View legendView = null;
        public ViewSchedule scheduleView = null;
        public Element wideLine, hiddenLine, dashedLine;
        public FamilySymbol ngbe0361Symbol, ngbe0366Symbol, ngbe0368Symbol, ngbe0369Symbol, ngbe0370Symbol, ngbe0372Symbol, ngbe0375Symbol, ngbe0377Symbol,
            ngbe0382Symbol, ngbe0405Symbol, ngbe0433Symbol, ngbe0434Symbol, ngbe9001Symbol, ngbe9002Symbol, ngbe9003Symbol, ngbe9004Symbol, ngbe9005Symbol,
            ngbe9006Symbol, ngbe9007Symbol, ngbe9009Symbol, ngbe9010Symbol, ngbe9011Symbol, ngbe9012Symbol, ngbe9013Symbol, ngbe9014Symbol;
        public SchedulableField schematicReference, cableReference, cableDescription, cableType, cableSize, cableLength;
        public DetailCurve interlockLine1 = null, interlockLine2 = null;
        int bus1X, bus1XLife, bus2X, bus2XLife, lifeX, circuitRef, circuitGap;
        double ftMM = 1 / 304.8; // Feet to Millimetres

        public createLVSchematic()
        {
            InitializeComponent();
        }

        private void helpRequest(object sender, HelpEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/LV-Schematics#create-lv-schematic");
        }

        private void helpButtonClick(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/LV-Schematics#create-lv-schematic");
            e.Cancel = true;
        }

        private void createLVSchematic_Load(object sender, EventArgs e)
        {
            tab1TransformerValue.SelectedIndex = 0;
            tab2TransformerValue.SelectedIndex = 0;
            tabControl.TabPages.Clear();
            tabControl.TabPages.Add(tab1);
        }

        public void insertFaultRelays(double circuitPosition, FamilyInstance bridge) {
            if (extra8.Checked || extra9.Checked || extra10.Checked)
            {
                DetailCurve circuitLine2 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(circuitPosition - 6, -28, 0) * ftMM)); // BMS Output Line
                circuitPosition -= 1.6;
                if (extra8.Checked)
                {
                    circuitPosition -= 4.4;
                    FamilyInstance faultRelay = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe9001Symbol, draftView); // Overcurrent Fault Relay
                    faultRelay.LookupParameter("cableReference").Set("RE");
                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(faultRelay, true));
                }
                if (extra9.Checked)
                {
                    circuitPosition -= 4.4;
                    FamilyInstance faultRelay = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe9001Symbol, draftView); // Earth Fault Relay
                    faultRelay.LookupParameter("cableReference").Set("OC");
                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(faultRelay, true));
                }
                if (extra10.Checked)
                {
                    circuitPosition -= 4.4;
                    FamilyInstance faultRelay = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe9001Symbol, draftView); // Restricted Earth Fault Relay
                    faultRelay.LookupParameter("cableReference").Set("EF");
                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(faultRelay, true));
                }
                doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge, true));
            }
        }

        public void insertNELink(double circuitPosition)
        {
            if (extra11.Checked)
            {
                DetailCurve circuitLine2 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -63.5, 0) * ftMM, new XYZ(circuitPosition - 26, -63.5, 0) * ftMM)); // Basic Circuit Line
                FamilyInstance neLink = doc.Create.NewFamilyInstance(new XYZ(circuitPosition - 11.75, -63.5, 0) * ftMM, ngbe0377Symbol, draftView); // Netrual Earth Link
                DetailCurve circuitLine3 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition - 26, -63.5, 0) * ftMM, new XYZ(circuitPosition - 26, -93, 0) * ftMM)); // Basic Circuit Line
                FamilyInstance linkBar = doc.Create.NewFamilyInstance(new XYZ(circuitPosition - 26, -93.8, 0) * ftMM, ngbe9014Symbol, draftView); // Netrual Earth Link Bar
                doc.Regenerate();
                doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(neLink, true));
                doc.Create.NewAlignment(draftView, circuitLine3.GeometryCurve.Reference, rpmBIMUtils.findReference(linkBar));
            }
        }

        public void insertCastellInterlock(double circuitPosition)
        {
            if (extra12.Checked)
            {
                doc.Create.NewFamilyInstance(new XYZ(circuitPosition - 5, -35, 0) * ftMM, ngbe9010Symbol, draftView); // Castell Interlock
            }
        }

        public string interlockText()
        {
            string text = null;
            text += extra13.Checked && extra14.Checked ? "Elec/Mech" : extra13.Checked ? "Electrical" : "Mechanical";
            text += "\nInterlock";
            return text;
        }

        // UI Control Scripts ------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------

        // Main Interface
        private void createSheet_CheckedChanged(object sender, EventArgs e)
        {
            if (createSheet.Checked && tabControl.Contains(tab2) && !tabControl.Contains(tab5))
            {
                tabControl.TabPages.Add(tab5);
            }
            if (!createSheet.Checked && tabControl.Contains(tab2) && tabControl.Contains(tab5))
            {
                tabControl.TabPages.Remove(tab5);
            }
            extra2.Checked = createSheet.Checked ? extra2.Checked : false;
            extra2.Enabled = createSheet.Checked ? true : false;
        }

        private void numericUpDown_Validating(object sender, CancelEventArgs e)
        {
            var item = (System.Windows.Forms.NumericUpDown)sender;
            item.Value = Math.Round(item.Value, item.DecimalPlaces, MidpointRounding.AwayFromZero);
        }

        // Tab 1 & 2
        private void Transformer_CheckedChanged(object sender, EventArgs e)
        {
            var deviceType = (System.Windows.Forms.RadioButton)sender;
            int tab = Convert.ToInt32(deviceType.Name.Substring(3, 1));
            ((System.Windows.Forms.ComboBox)Controls.Find("tab" + tab.ToString() + "TransformerValue", true)[0])
                .Enabled = deviceType.Checked ? false : true;
            GeneratorTransformer_CheckedChanged();
        }

        private void Generator_CheckedChanged(object sender, EventArgs e)
        {
            var deviceType = (System.Windows.Forms.RadioButton)sender;
            int tab = Convert.ToInt32(deviceType.Name.Substring(3, 1));
            ((System.Windows.Forms.NumericUpDown)Controls.Find("tab" + tab.ToString() + "GeneratorValue", true)[0])
                .Enabled = deviceType.Checked ? false : true;
            GeneratorTransformer_CheckedChanged();
        }

        private void GeneratorTransformer_CheckedChanged()
        {
            // transformer checks
            if (!tab1TransformerNotUsed.Checked || (!tab2TransformerNotUsed.Checked && !string.IsNullOrWhiteSpace(tab2DeviceType1.Text)))
            {
                extra6.Enabled = true;
                extra7.Enabled = true;
                extra11.Enabled = true;
            }
            else
            {
                extra6.Enabled = false;
                extra6.Checked = false;
                extra7.Enabled = false;
                extra7.Checked = false;
                extra11.Enabled = false;
                extra11.Checked = false;
            }

            // transformer & generator checks
            if(!tab1GeneratorNotUsed.Checked || 
               !tab1TransformerNotUsed.Checked ||
               (!tab2GeneratorNotUsed.Checked && !string.IsNullOrWhiteSpace(tab2DeviceType1.Text)) ||
               (!tab2TransformerNotUsed.Checked && !string.IsNullOrWhiteSpace(tab2DeviceType1.Text)))
            { 
                extra8.Enabled = true;
                extra9.Enabled = true;
                extra10.Enabled = true;
            } else {
                extra8.Enabled = false;
                extra8.Checked = false;
                extra9.Enabled = false;
                extra9.Checked = false;
                extra10.Enabled = false;
                extra10.Checked = false;
            }
            Interlock_CheckChanged();
        }

        private void Interlock_CheckChanged()
        {
            int check = 0;
            if (!tab1GeneratorNotUsed.Checked) { check++; }
            if (!tab1TransformerNotUsed.Checked) { check++; }
            if (!tab2GeneratorNotUsed.Checked && !string.IsNullOrWhiteSpace(tab2DeviceType1.Text)) { check++; }
            if (!tab2TransformerNotUsed.Checked && !string.IsNullOrWhiteSpace(tab2DeviceType1.Text)) { check++; }
            if (!string.IsNullOrWhiteSpace(tab2DeviceType1.Text)) { check++; }
            if (check < 2) {
                extra12.Enabled = false;
                extra12.Checked = false;
                extra13.Enabled = false;
                extra13.Checked = false;
                extra14.Enabled = false;
                extra14.Checked = false;
            } else {
                extra12.Enabled = true;
                extra13.Enabled = true;
                extra14.Enabled = true;
            }
        }

        private void DeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var deviceType = (System.Windows.Forms.ComboBox)sender;
            int tab = Convert.ToInt32(deviceType.Name.Substring(3, 1));
            int deviceRow = Convert.ToInt32(deviceType.Name.Substring(deviceType.Name.Length - 1, 1));
            if (!tabControl.Contains(tab2))
            {
                tabControl.TabPages.AddRange(new TabPage[] { tab2, tab3, tab4 });
                if (createSheet.Checked) { tabControl.TabPages.Add(tab5); }
            }
            if (!((System.Windows.Forms.NumericUpDown)Controls.Find("tab" + tab.ToString() + "Quantity" + deviceRow.ToString(), true)[0]).Enabled)
            {
                ((System.Windows.Forms.NumericUpDown)Controls.Find("tab" + tab.ToString() + "Quantity" + deviceRow.ToString(), true)[0]).Enabled = true;
                ((System.Windows.Forms.ComboBox)Controls.Find("tab" + tab.ToString() + "ConnectedLoad" + deviceRow.ToString(), true)[0]).Enabled = true;
                ((System.Windows.Forms.ComboBox)Controls.Find("tab" + tab.ToString() + "ConnectedLoad" + deviceRow.ToString(), true)[0]).Text = "Spare";
                ((System.Windows.Forms.CheckBox)Controls.Find("tab" + tab.ToString() + "Metered" + deviceRow.ToString(), true)[0]).Enabled = true;
            }
            if (deviceRow < 6)
            {
                ((System.Windows.Forms.ComboBox)Controls.Find("tab" + tab.ToString() + "DeviceType" + (deviceRow + 1).ToString(), true)[0]).Enabled = true;
            }
            GeneratorTransformer_CheckedChanged();
        }

        // Tab 3
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tab3LifeSafetyCircuitsSelection.Visible = tab3LifeSafetyStatus.Checked ? true : false;
        }

        // Tab 4
        private void enablePowerFactorCorrection(object sender, EventArgs e)
        {
            var item = (System.Windows.Forms.NumericUpDown)sender;
            int tab = Convert.ToInt32(item.Name.Substring(3, 1));
            ((System.Windows.Forms.NumericUpDown)Controls.Find("powerFactor" + tab.ToString() + "Kvar", true)[0]).Enabled = true;
        }

        private void powerFactorKvar_ValueChanged(object sender, EventArgs e)
        {
            var kVar = (System.Windows.Forms.NumericUpDown)sender;
            int num = Convert.ToInt32(kVar.Name.Substring(11, 1));
            var steps = ((System.Windows.Forms.NumericUpDown)Controls.Find("powerFactor" + num.ToString() + "Steps", true)[0]);
            var detuning = ((System.Windows.Forms.CheckBox)Controls.Find("powerFactor" + num.ToString() + "Detuning", true)[0]);
            if (kVar.Value > 0 && !steps.Enabled)
            {
                steps.Enabled = true;
                detuning.Enabled = true;
            }
            if(kVar.Value == 0 && steps.Enabled ) {
                steps.Enabled = false;
                detuning.Enabled = false;
            }
        }

        private void extraSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 15; i++)
            {
                if (((System.Windows.Forms.CheckBox)Controls.Find("extra" + i.ToString(), true)[0]).Enabled)
                {
                    ((System.Windows.Forms.CheckBox)Controls.Find("extra" + i.ToString(), true)[0]).Checked = true;
                }
            }
        }

        private void extraClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 15; i++)
            {
                ((System.Windows.Forms.CheckBox)Controls.Find("extra" + i.ToString(), true)[0]).Checked = false;
            }
        }

        // Tab 5
        private void noteSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 13; i++)
            {
                ((System.Windows.Forms.CheckBox)Controls.Find("note" + i.ToString(), true)[0]).Checked = true;
            }
        }

        private void noteClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 13; i++)
            {
                ((System.Windows.Forms.CheckBox)Controls.Find("note" + i.ToString(), true)[0]).Checked = false;
            }
        }

        private void noteSingleInput_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (System.Windows.Forms.CheckBox)sender;
            int noteNumber = Convert.ToInt32(checkbox.Name.Substring(4, 1));
            ((System.Windows.Forms.TextBox)Controls.Find("note" + noteNumber.ToString() + "Input", true)[0]).Enabled = checkbox.Checked ? true : false;
        }

        private void noteTwinInput_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (System.Windows.Forms.CheckBox)sender;
            int noteNumber = Convert.ToInt32(checkbox.Name.Substring(4, 1));
            ((System.Windows.Forms.TextBox)Controls.Find("note" + noteNumber.ToString() + "Input1", true)[0]).Enabled = checkbox.Checked ? true : false;
            ((System.Windows.Forms.TextBox)Controls.Find("note" + noteNumber.ToString() + "Input2", true)[0]).Enabled = checkbox.Checked ? true : false;
        }

        private void noteCustomInput_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (System.Windows.Forms.CheckBox)sender;
            int noteNumber = Convert.ToInt32(checkbox.Name.Substring(4, 2));
            ((System.Windows.Forms.TextBox)Controls.Find("note" + noteNumber.ToString() + "Input", true)[0]).Enabled = checkbox.Checked ? true : false;
            ((System.Windows.Forms.TextBox)Controls.Find("note" + noteNumber.ToString() + "Input", true)[0]).Text = checkbox.Checked ? "" : "Enter a custom note here...";
            ((System.Windows.Forms.TextBox)Controls.Find("note" + noteNumber.ToString() + "Input", true)[0]).Focus();
        }

        // LV Schematic Creation Script --------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------

        public void create_Click(object sender, EventArgs e)
        {
            // Form checking
            if (string.IsNullOrWhiteSpace(schematicTitle.Text) || string.IsNullOrWhiteSpace(tab1DeviceType1.Text))
            {
                TaskDialog.Show("Create LV Schematic", "LV Schematic name or device input missing.");
                return;
            }

            // Schematic title check for invalid characters
            Regex r = new Regex(@"^[a-zA-Z0-9\s]+$");
            if (!r.IsMatch(schematicTitle.Text))
            {
                TaskDialog.Show("LV Schematic", "Invalid schematic name, please use only alphanumeric characters");
                return;
            }

            // Find if 2mm Font already exists
            TextNoteType textStyle = rpmBIMUtils.FindElementByName(rpmBIMTools.Load.liveDoc, typeof(TextNoteType), "2mm") as TextNoteType;
            if (textStyle == null)
            {
                using (Transaction t = new Transaction(doc, "Create new TextNoteType for schematic")) {
                    t.Start();
                    TextNoteType TextNoteToCopy = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).FirstOrDefault() as TextNoteType;
                    textStyle = TextNoteToCopy.Duplicate("2mm") as TextNoteType;
                    textStyle.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(2 * ftMM);
                    t.Commit();
                }
            }

            // Find if drafting view already exists
            IEnumerable<ViewDrafting> draftingViews = from elem in new FilteredElementCollector(doc)
                                                       .OfClass(typeof(ViewDrafting))
                                                let type = elem as ViewDrafting
                                                where type.ViewName.Equals(schematicTitle.Text.Trim())
                                                select type;
            if (draftingViews.Count() > 0)
            {
                TaskDialog.Show("Create LV Schematic", "Duplicate LV schematic already exists");
                return;
            }

            // Find A0 NGB Title Block
            FamilySymbol titleBlock = rpmBIMUtils.findNGBTitleBlock(doc, "A0");
            if (titleBlock == null)
            {
                TaskDialog.Show("Create LV Schematic", "No NGB title block family found in this project");
                return;
            }

            // Find required lineStyles
            ICollection<ElementId> lineStyles = rpmBIMUtils.GetLineStyles(doc);
            foreach(ElementId lineStyle in lineStyles) {
              Element elem =  doc.GetElement(lineStyle);
              if (elem.Name.Equals("Wide Lines")) wideLine = elem;
              if (elem.Name.Equals("<Hidden>")) hiddenLine = elem;
              if (elem.Name.Equals("<Overhead>")) dashedLine = elem;
            }
            if (wideLine == null || hiddenLine == null || dashedLine == null) 
            {
                TaskDialog.Show("Create LV Schematic", "Required Line Styles not found");
                return;
            };

#if !REVIT2015
            // Setup TextNoteOptions
            TextNoteOptions textHorizontal = new TextNoteOptions();
            textHorizontal.TypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
            TextNoteOptions textVertical = new TextNoteOptions();
            textVertical.TypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
            textVertical.Rotation = Math.PI / 2;
#endif

            // Open waiting window
            this.Hide();
            WaitingWindow waitingWindow = new WaitingWindow("Loading Required Resources\nPlease Wait...");
            waitingWindow.Location = new System.Drawing.Point(this.Location.X + (this.Width - waitingWindow.Width) / 2, this.Location.Y + (this.Height - waitingWindow.Height) / 2);
            waitingWindow.Show();
            Application.DoEvents();

            // Check if family symbols already exist, if not load them from local drive
            try
            {
                string path = @"C:\rpmBIM\families\elec schematic\";
                if (!tab1TransformerNotUsed.Checked || !tab2TransformerNotUsed.Checked) {
                    ngbe0361Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Transformer", path + "SM - Equipment - Transformer.rfa"); } // Transformer
                if (!tab1GeneratorNotUsed.Checked || !tab2GeneratorNotUsed.Checked) {
                    ngbe0366Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Generator", path + "SM - Equipment - Generator.rfa");
                } // Generator
                ngbe0368Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Bus Link Bridge", path + "SM - Device - Bus Link Bridge.rfa"); // Bus Link Bridge
                ngbe0369Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Air Circuit Breaker", path + "SM - Device - Air Circuit Breaker.rfa"); // Air Circuit Breaker
                ngbe0370Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Fused Switch Disconnector", path + "SM - Device - Fused Switch Disconnector.rfa"); // Fused Switch Disconnector
                if (extra3.Checked || tab3LifeSafetyStatus.Checked) {
                    ngbe0372Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Fused Disconnector", path + "SM - Device - Fused Disconnector.rfa"); } // Fused Disconnector
                ngbe0375Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Miniture Circuit Breaker", path + "SM - Device - Miniture Circuit Breaker.rfa"); // Miniture Circuit Breaker
                if (extra7.Checked) {
                    ngbe0382Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Meter Connection", path + "SM - Device - Meter Connection.rfa"); } // Meter Connection
                ngbe0405Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Metered Symbol", path + "SM - Device - Metered Symbol.rfa"); // Metered Symbol
                ngbe0433Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Moulded Case Circuit Breaker", path + "SM - Device - Moulded Case Circuit Breaker.rfa"); // Moulded Case Circuit Breaker
                if (extra1.Checked) {
                    ngbe0434Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Cable Termination", path + "SM - Device - Cable Termination.rfa"); } // Cable Termination
                ngbe9001Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Annotation - Circuit Reference Symbol", path + "SM - Annotation - Circuit Reference Symbol.rfa"); // Reference Symbol
                ngbe9002Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Distribution Board", path + "SM - Equipment - Distribution Board.rfa"); // Distribution Board
                ngbe9003Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Undefined", path + "SM - Device - Undefined.rfa"); // Undefined
                ngbe9004Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Sub-Panel Board", path + "SM - Equipment - Sub-Panel Board.rfa"); // Sub-Panel Board
                ngbe9005Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Isolator", path + "SM - Equipment - Isolator.rfa"); // Isolator
                ngbe9006Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Spare", path + "SM - Equipment - Spare.rfa"); // Spare
                if (powerFactor1Kvar.Value > 0 || powerFactor2Kvar.Value > 0) {
                    ngbe9007Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Correction Device", path + "SM - Device - Correction Device.rfa");   // Correction Device
                    ngbe9009Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Correction Device (with detuning)", path + "SM - Device - Correction Device (with detuning).rfa"); } // Correction Device (with detuning)
                if (extra12.Checked) { ngbe9010Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Annotation - Castell Interlock", path + "SM - Annotation - Castell Interlock.rfa"); } // Castell Interlock
                if (tab3LifeSafetyStatus.Checked) { ngbe9011Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Life Safety Unit", path + "SM - Equipment - Life Safety Unit.rfa"); } // Life Safety Unit
                if (extra5.Checked || extra6.Checked) { ngbe9012Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Earth Bar", path + "SM - Equipment - Earth Bar.rfa"); } // Earth Bar
                if (extra6.Checked) { ngbe9013Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Inter-tripping", path + "SM - Equipment - Inter-tripping.rfa"); } // Inter-tripping
                if (extra11.Checked) {
                    ngbe0377Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Netrual Earth Link", path + "SM - Device - Netrual Earth Link.rfa");   // Netrual Earth Link
                    ngbe9014Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Netrual Earth Link Bar", path + "SM - Equipment - Netrual Earth Link Bar.rfa"); } // Netrual Earth Link Bar
            }
            catch (Autodesk.Revit.Exceptions.FileNotFoundException ex)
            {
                TaskDialog.Show("Create LV Schematic", "Unable to load the family symbol located at:\n\n" + ex.Message);
                waitingWindow.Dispose();
                this.Close();
                return;
            }

            // Update waiting window
            waitingWindow.UpdateText("Creating LV Schematic\nPlease Wait...");
            
            // Create DraftingView
            using (Transaction trans = new Transaction(doc, "Creating Drafting View"))
            {
                trans.Start();
                draftView = ViewDrafting.Create(doc, doc.GetViewFamilyType(ViewFamily.Drafting).Id);
                draftView.LookupParameter("Sub-Discipline").Set("E61 - Main HV / LV Distribution");
                draftView.Discipline = ViewDiscipline.Coordination;
                draftView.ViewName = schematicTitle.Text.Trim();
                draftView.Scale = 1;
                draftView.DetailLevel = ViewDetailLevel.Fine;
                trans.Commit();
                // Set DraftingView as active view
                rpmBIMTools.Load.uiApp.ActiveUIDocument.ActiveView = draftView;
            }
            
            // Setup basic shared variables
            var uiApp = rpmBIMTools.Load.uiApp.Application;
            circuitGap = 20; // Space between each curcuit
            circuitRef = 1; // Start at circuit reference 1

            // Draw Bus Link Bridge
            if (!string.IsNullOrWhiteSpace(tab2DeviceType1.Text))
            {
                using (Transaction trans = new Transaction(doc, "Create Bus Link Bridge"))
                {
                    trans.Start();
                    FamilyInstance bridge = doc.Create.NewFamilyInstance(new XYZ(0, 0, 0) * ftMM, ngbe0368Symbol, draftView); // Bus Link Bridge
                    LocationPoint location = bridge.Location as LocationPoint;
                    Line axis = Line.CreateBound(location.Point, new XYZ(location.Point.X, location.Point.Y, location.Point.Z + 10) * ftMM);
                    bridge.Location.Rotate(axis, Math.PI / 2);
                    if (extra12.Checked)
                    {
                        doc.Create.NewFamilyInstance(new XYZ(0, -7, 0) * ftMM, ngbe9010Symbol, draftView); // Castell Interlock
                    }
                    trans.Commit();
                }
            }

            // Draw Bus 1 Circuits
            bus1X = -18;
            for (int d = 1; d < 7; d++)
            {
                if (!String.IsNullOrWhiteSpace(((System.Windows.Forms.ComboBox)Controls.Find("tab1DeviceType" + d.ToString(), true)[0]).Text))
                {
                    bus1X -= Convert.ToInt32(((System.Windows.Forms.NumericUpDown)Controls.Find("tab1Quantity" + d.ToString(), true)[0]).Value) * circuitGap ;
                }
            }
            // Bus 1 Minimum size
            bus1X = bus1X > -18 - circuitGap * 6 ? -18 - circuitGap * 6 : bus1X;

            using (Transaction trans = new Transaction(doc, "Create Bus 1 Circuits"))
            {
                trans.Start();
                int TempX = bus1X + 20;
                for (int d = 1; d < 7; d++)
                {
                    if (!String.IsNullOrWhiteSpace(((System.Windows.Forms.ComboBox)Controls.Find("tab1DeviceType" + d.ToString(), true)[0]).Text))
                    {
                        for (int q = 0; q < ((System.Windows.Forms.NumericUpDown)Controls.Find("tab1Quantity" + d.ToString(), true)[0]).Value; q++)
                        {
                            String deviceType = ((System.Windows.Forms.ComboBox)Controls.Find("tab1DeviceType" + d.ToString(), true)[0]).Text;
                            FamilySymbol deviceFamily =
                                deviceType == "Air Circuit Breaker" ? ngbe0369Symbol :
                                deviceType == "Moulded Case Circuit Breaker" ? ngbe0433Symbol :
                                deviceType == "Fused Switch Disconnector" ? ngbe0370Symbol :
                                deviceType == "Miniature Circuit Breaker" ? ngbe0375Symbol :
                                ngbe9003Symbol;
                            String connectedLoadType = ((System.Windows.Forms.ComboBox)Controls.Find("tab1ConnectedLoad" + d.ToString(), true)[0]).Text;
                            FamilySymbol connectedLoadFamily =
                                connectedLoadType == "Distribution Board" ? ngbe9002Symbol :
                                connectedLoadType == "Sub-Panel" ? ngbe9004Symbol :
                                connectedLoadType == "Isolator" ? ngbe9005Symbol :
                                ngbe9006Symbol;
                            double textPosition =
                                    connectedLoadType == "Distribution Board" ? 130.6 :
                                    connectedLoadType == "Sub-Panel" ? 147.6 :
                                    connectedLoadType == "Isolator" ? 109.6 :
                                    115.2;
                            Boolean isMetered = ((System.Windows.Forms.CheckBox)Controls.Find("tab1Metered" + d.ToString(), true)[0]).Checked;

                            DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(TempX, 0, 0) * ftMM, new XYZ(TempX, 100, 0) * ftMM)); // Basic Circuit Line
                            FamilyInstance deviceInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 18, 0) * ftMM, deviceFamily, draftView); // Device Type
                            FamilyInstance referenceInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 52.2, 0) * ftMM, ngbe9001Symbol, draftView) as FamilyInstance; // Reference Symbol
                            referenceInstance.LookupParameter("cableReference").Set(circuitRef.ToString());
                            referenceInstance.LookupParameter("cableDescription").Set("Insert Circuit " + circuitRef.ToString() + " Description Here");
                            referenceInstance.LookupParameter("schematicReference").Set(schematicTitle.Text.Trim());
                            FamilyInstance connectedLoadInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 100, 0) * ftMM, connectedLoadFamily, draftView); // Connected Load
#if REVIT2015
                            doc.Create.NewTextNote(draftView, new XYZ(TempX , textPosition, 0) * ftMM, XYZ.BasisY, -XYZ.BasisX, 50 * ftMM, TextAlignFlags.TEF_ALIGN_LEFT | TextAlignFlags.TEF_ALIGN_MIDDLE, "Insert Circuit " + circuitRef + " Descrition Here").TextNoteType = textStyle;
#else
                            TextNote.Create(doc, draftView.Id, new XYZ(TempX - 1.7, textPosition, 0) * ftMM, "Insert Circuit " + circuitRef + " Descrition Here", textVertical).TextNoteType = textStyle;
#endif
                            doc.Regenerate();
                            doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(deviceInstance));
                            doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(referenceInstance));
                            doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(connectedLoadInstance));
                            if (isMetered) {
                                FamilyInstance meteredInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 28.2, 0) * ftMM, ngbe0405Symbol, draftView); // Metered Symbol
                                doc.Regenerate();
                                doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(meteredInstance));
                            }
                            if (extra1.Checked)
                            {
                                FamilyInstance terminationInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 40, 0) * ftMM, ngbe0434Symbol, draftView); // Cable Termination
                                doc.Regenerate();
                                doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(terminationInstance));
                            }
                            circuitRef++;
                            TempX += circuitGap;
                        }
                    }
                }
                doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(bus1X + 10, 0, 0) * ftMM, new XYZ(-8, 0, 0) * ftMM)).LineStyle = wideLine;
                trans.Commit();
            }

            // Draw Bus 1 Transformer
            if (!tab1TransformerNotUsed.Checked)
            {
                double circuitPosition = (bus1X + 18) * 0.75 - 8;
                using (Transaction trans = new Transaction(doc, "Create Bus 1 Transformer"))
                {
                    trans.Start();
                    double circuitLength = tab1TransformerCloseCoupled.Checked ? -72 : -126;
                    string circuitRating = tab1TransformerValue.Text + " kVA";
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView,
                        Line.CreateBound(new XYZ(circuitPosition, 0, 0) * ftMM,
                        new XYZ(circuitPosition, circuitLength, 0) * ftMM));
                    FamilyInstance bridge = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe0368Symbol, draftView); // Transformer Bridge
                    FamilyInstance transformer = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, circuitLength, 0) * ftMM, ngbe0361Symbol, draftView); // Transformer
                    transformer.LookupParameter("Rating").Set(circuitRating);

                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge));
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(transformer));
                    // Draw Bus 1 Inter-tripping (if turned on)
                    if (extra6.Checked)
                    {
                        double intertrippingPosition = (bus1X + 18) * 0.625 - 8;
                        DetailCurve circuitLine2 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(intertrippingPosition, -28, 0) * ftMM)); // Basic Circuit Line
                        circuitLine2.LineStyle = dashedLine;
                        DetailCurve circuitLine3 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(intertrippingPosition, -28, 0) * ftMM, new XYZ(intertrippingPosition, -100, 0) * ftMM)); // Basic Circuit Line
                        circuitLine3.LineStyle = dashedLine;
                        FamilyInstance intertripping = doc.Create.NewFamilyInstance(new XYZ(intertrippingPosition, -100, 0) * ftMM, ngbe9013Symbol, draftView); // Inter-tripping
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge, true));
                        doc.Create.NewAlignment(draftView, circuitLine3.GeometryCurve.Reference, rpmBIMUtils.findReference(intertripping));
                    }
                    // Draw Bus 1 Metering (if turned on)
                    if (extra7.Checked) {
                        FamilyInstance meteringConnection = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -48, 0) * ftMM, ngbe0382Symbol, draftView); // Metering Connection
                        DetailCurve circuitLine2 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -48, 0) * ftMM, new XYZ(circuitPosition - 6, -48 , 0) * ftMM)); // Basic Circuit Line
                        DetailCurve circuitLine3 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition - 6, -48, 0) * ftMM, new XYZ(circuitPosition - 18, -48, 0) * ftMM)); // BMS Output Line
                        circuitLine3.LineStyle = dashedLine;
                        FamilyInstance meteredInstance = doc.Create.NewFamilyInstance(new XYZ(circuitPosition - 6, -48, 0) * ftMM, ngbe0405Symbol, draftView); // Metering Connection
#if REVIT2015
                        doc.Create.NewTextNote(draftView, new XYZ(circuitPosition - 19, -48, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 25 * ftMM, TextAlignFlags.TEF_ALIGN_RIGHT | TextAlignFlags.TEF_ALIGN_MIDDLE, "BMS Output").TextNoteType = textStyle;
#else
                        TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(circuitPosition - 19, -46.3, 0) * ftMM, "BMS Output", textHorizontal);
                        tn.HorizontalAlignment = HorizontalTextAlignment.Right;
                        tn.TextNoteType = textStyle;
#endif

                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(meteringConnection));
                        doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, circuitLine3.GeometryCurve.Reference);
                        doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(meteringConnection, true));
                        doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(meteredInstance, true));
                    }
                    // Draw Bus 1 Fault Relays (if ANY turned on)
                    insertFaultRelays(circuitPosition, bridge);
                    // Draw Bus 1 Netrual Earth Link (if turned on)
                    insertNELink(circuitPosition);
                    // Draw Bus 1 Castell Interlock
                    insertCastellInterlock(circuitPosition);
                    // Draw Bus 1 Interlock Lines
                    if ((extra13.Checked || extra14.Checked) && (!tab1GeneratorNotUsed.Checked || !string.IsNullOrWhiteSpace(tab2DeviceType1.Text)))
                    {
                        if (!tab1GeneratorNotUsed.Checked)
                        {
                            interlockLine1 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(circuitPosition + 14, -14, 0) * ftMM)); // Interlock Circuit Line
                            interlockLine1.LineStyle = dashedLine;
                        } else {
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(circuitPosition + 14, -14, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition + 14, -14, 0) * ftMM, new XYZ(-14, -14, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(-14, -14, 0) * ftMM, new XYZ(0, 0, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            double textPosition = (bus1X + 18) * 0.3 - 8;
#if REVIT2015
                            doc.Create.NewTextNote(draftView, new XYZ(textPosition, -13, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 15 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_BOTTOM, interlockText()).TextNoteType = textStyle;
#else
                            TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(textPosition, -7, 0) * ftMM, interlockText(), textHorizontal);
                            tn.HorizontalAlignment = HorizontalTextAlignment.Center;
                            tn.TextNoteType = textStyle;
#endif
                        }   
                    }
                    trans.Commit();
                }
            }

            // Draw Bus 1 Surge Suppression (if turned on)
            if (extra3.Checked)
            {
                using (Transaction t = new Transaction(doc, "Create Bus 1 Surge Suppression"))
                {
                    t.Start();
                    double circuitPosition = (bus1X + 18) * 0.5 - 8;
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, 0, 0) * ftMM, new XYZ(circuitPosition, -36, 0) * ftMM)); // Basic Circuit Line
                    FamilyInstance deviceInstance = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe0372Symbol, draftView); // Device Type
                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(deviceInstance));
#if REVIT2015
                    doc.Create.NewTextNote(draftView, new XYZ(circuitPosition, -37, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 30 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_TOP, "Surge\nSuppression").TextNoteType = textStyle;
#else
                    TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(circuitPosition, -37, 0) * ftMM, "Surge\nSuppression", textHorizontal);
                    tn.HorizontalAlignment = HorizontalTextAlignment.Center;
                    tn.TextNoteType = textStyle;
#endif
                    t.Commit();
                }
            }

            // Draw Bus 1 Generator
            if (!tab1GeneratorNotUsed.Checked)
            {
                using (Transaction trans = new Transaction(doc, "Create Bus 1 Genrator"))
                {
                    trans.Start();
                    double circuitPosition = (bus1X + 18) * 0.25 - 8;
                    double circuitLength = tab1GeneratorCloseCoupled.Checked ? -72 : -126;
                    string circuitRating = tab1GeneratorValue.Value.ToString() + " kVA";
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView,
                        Line.CreateBound(new XYZ(circuitPosition, 0, 0) * ftMM,
                        new XYZ(circuitPosition, circuitLength, 0) * ftMM));
                    FamilyInstance bridge = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe0368Symbol, draftView); // Generator Bridge
                    FamilyInstance generator = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, circuitLength, 0) * ftMM, ngbe0366Symbol, draftView); // Generator
                    generator.LookupParameter("Rating").Set(circuitRating);

                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge));
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(generator));
                    // Draw Bus 1 Fault Relays (if ANY turned on)
                    insertFaultRelays(circuitPosition, bridge);
                    // Draw Bus 1 Castell Interlock
                    insertCastellInterlock(circuitPosition);
                     // Draw Bus 1 Interlock Lines
                    if (extra13.Checked || extra14.Checked) {
                        if (interlockLine1 != null) {
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(interlockLine1.GeometryCurve.GetEndPoint(1), new XYZ(circuitPosition - 14, -14, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition - 14, -14, 0) * ftMM, new XYZ(circuitPosition, -28, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            double textPosition = (bus1X + 18) * 0.6 - 8;
#if REVIT2015
                            doc.Create.NewTextNote(draftView, new XYZ(textPosition, -13, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 15 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_BOTTOM, interlockText()).TextNoteType = textStyle;
#else
                            TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(textPosition, -7, 0) * ftMM, interlockText(), textHorizontal);
                            tn.HorizontalAlignment = HorizontalTextAlignment.Center;
                            tn.TextNoteType = textStyle;
#endif
                        }
                        if (!string.IsNullOrWhiteSpace(tab2DeviceType1.Text))
                        {
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(circuitPosition + 14, -14, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition + 14, -14, 0) * ftMM, new XYZ(-14, -14, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(-14, -14, 0) * ftMM, new XYZ(0, 0, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            double textPosition = (bus1X + 18) * 0.1 - 8;
#if REVIT2015
                            doc.Create.NewTextNote(draftView, new XYZ(textPosition, -13, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 15 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_BOTTOM, interlockText()).TextNoteType = textStyle;
#else
                            TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(textPosition, -7, 0) * ftMM, interlockText(), textHorizontal);
                            tn.HorizontalAlignment = HorizontalTextAlignment.Center;
                            tn.TextNoteType = textStyle;
#endif
                        }

                    }
                    trans.Commit();
                }
            }

            // Draw Bus 1 Power Factor Correction
            if (powerFactor1Kvar.Value > 0)
            {
                using (Transaction t = new Transaction(doc, "Create Bus 1 Power Factor Correction"))
                {
                    t.Start();
                    double TempX = (bus1X + 18) * 0.9 - 8;
                    double startPosition = TempX;
                    double textPosition = startPosition - (Convert.ToInt32(powerFactor1Steps.Value) * 10) / 2;
                    string text = powerFactor1Kvar.Value.ToString() + "kVAr PFC,\n" + powerFactor1Steps.Value.ToString() + " x " + Math.Floor((powerFactor1Kvar.Value / powerFactor1Steps.Value)) + "kVAr Steps";
                    if (powerFactor1Detuning.Checked) { text += "\nc/w detuning reactors"; }
                    FamilySymbol deviceType = powerFactor1Detuning.Checked ? ngbe9009Symbol : ngbe9007Symbol;
                    doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(startPosition, 0, 0) * ftMM, new XYZ(startPosition, -10, 0) * ftMM)); // Basic Circuit Line
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(startPosition, -10, 0) * ftMM, new XYZ(startPosition - Convert.ToInt32(powerFactor1Steps.Value) * 10, -10, 0) * ftMM)); // Basic Circuit Line
                    for (int d = 0; d < powerFactor1Steps.Value; d++)
                    {
                        TempX -= 10;
                        FamilyInstance correctionDevice = doc.Create.NewFamilyInstance(new XYZ(TempX + 5, -10, 0) * ftMM, deviceType, draftView); // Correction Device
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(correctionDevice, true));
                    }
#if REVIT2015
                    doc.Create.NewTextNote(draftView, new XYZ(textPosition, powerFactor1Detuning.Checked ? -40 : -30, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 30 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_TOP, text).TextNoteType = textStyle;
#else
                    TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(textPosition, powerFactor1Detuning.Checked ? -40 : -30, 0) * ftMM, text, textHorizontal);
                    tn.HorizontalAlignment = HorizontalTextAlignment.Center;
                    tn.TextNoteType = textStyle;
#endif
                    TempX -= 10;
                    bus1XLife = bus1X;
                    bus1X = TempX < bus1X ? Convert.ToInt32(TempX) : bus1X;
                    t.Commit();
                }
            }

            // Draw Bus 2 Circuits
            if (!string.IsNullOrWhiteSpace(tab2DeviceType1.Text))
            {
                using (Transaction trans = new Transaction(doc, "Create Bus 2 Circuits"))
                {
                    trans.Start();
                    bus2X = 18;
                    for (int d = 1; d < 7; d++)
                    {
                        if (!String.IsNullOrWhiteSpace(((System.Windows.Forms.ComboBox)Controls.Find("tab2DeviceType" + d.ToString(), true)[0]).Text))
                        {
                            for (int q = 0; q < ((System.Windows.Forms.NumericUpDown)Controls.Find("tab2Quantity" + d.ToString(), true)[0]).Value; q++)
                            {
                                String deviceType = ((System.Windows.Forms.ComboBox)Controls.Find("tab2DeviceType" + d.ToString(), true)[0]).Text;
                                FamilySymbol deviceFamily =
                                    deviceType == "Air Circuit Breaker" ? ngbe0369Symbol :
                                    deviceType == "Moulded Case Circuit Breaker" ? ngbe0433Symbol :
                                    deviceType == "Fused Switch Disconnector" ? ngbe0370Symbol :
                                    deviceType == "Miniature Circuit Breaker" ? ngbe0375Symbol :
                                    ngbe9003Symbol;
                                String connectedLoadType = ((System.Windows.Forms.ComboBox)Controls.Find("tab2ConnectedLoad" + d.ToString(), true)[0]).Text;
                                FamilySymbol connectedLoadFamily =
                                    connectedLoadType == "Distribution Board" ? ngbe9002Symbol :
                                    connectedLoadType == "Sub-Panel" ? ngbe9004Symbol :
                                    connectedLoadType == "Isolator" ? ngbe9005Symbol :
                                    ngbe9006Symbol;
                                double textPosition =
                                    connectedLoadType == "Distribution Board" ? 130.6 :
                                    connectedLoadType == "Sub-Panel" ? 147.6 :
                                    connectedLoadType == "Isolator" ? 109.6 :
                                    115.2;
                                Boolean isMetered = ((System.Windows.Forms.CheckBox)Controls.Find("tab2Metered" + d.ToString(), true)[0]).Checked;

                                DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(bus2X, 0, 0) * ftMM, new XYZ(bus2X, 100, 0) * ftMM)); // Basic Circuit Line
                                FamilyInstance deviceInstance = doc.Create.NewFamilyInstance(new XYZ(bus2X, 18, 0) * ftMM, deviceFamily, draftView); // Device Type
                                FamilyInstance referenceInstance = doc.Create.NewFamilyInstance(new XYZ(bus2X, 52.2, 0) * ftMM, ngbe9001Symbol, draftView); // Reference Symbol
                                referenceInstance.LookupParameter("cableReference").Set(circuitRef.ToString());
                                referenceInstance.LookupParameter("cableDescription").Set("Insert Circuit " + circuitRef.ToString() + " Description Here");
                                referenceInstance.LookupParameter("schematicReference").Set(schematicTitle.Text.Trim());

                                FamilyInstance connectedLoadInstance = doc.Create.NewFamilyInstance(new XYZ(bus2X, 100, 0) * ftMM, connectedLoadFamily, draftView); // Connected Load
#if REVIT2015
                                doc.Create.NewTextNote(draftView, new XYZ(bus2X, textPosition, 0) * ftMM, XYZ.BasisY, -XYZ.BasisX, 50 * ftMM, TextAlignFlags.TEF_ALIGN_LEFT | TextAlignFlags.TEF_ALIGN_MIDDLE, "Insert Circuit " + circuitRef + " Descrition Here").TextNoteType = textStyle;
#else
                                TextNote.Create(doc, draftView.Id, new XYZ(bus2X - 1.7, textPosition, 0) * ftMM, "Insert Circuit " + circuitRef + " Descrition Here", textVertical).TextNoteType = textStyle;
#endif
                                doc.Regenerate();
                                doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(deviceInstance));
                                doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(referenceInstance));
                                doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(connectedLoadInstance));
                                if (isMetered) {
                                    FamilyInstance meteredInstance = doc.Create.NewFamilyInstance(new XYZ(bus2X, 28.2, 0) * ftMM, ngbe0405Symbol, draftView); // Metered Symbol
                                    doc.Regenerate();
                                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(meteredInstance));
                                }
                                if (extra1.Checked)
                                {
                                    FamilyInstance terminationInstance = doc.Create.NewFamilyInstance(new XYZ(bus2X, 40, 0) * ftMM, ngbe0434Symbol, draftView); // Cable Termination
                                    doc.Regenerate();
                                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(terminationInstance));
                                }
                                circuitRef++;
                                bus2X += circuitGap;
                            }
                        }
                    }
                    // Bus 2 Minimum size
                    bus2X = bus2X < 18 + circuitGap * 6 ? 18 + circuitGap * 6 : bus2X;
                    doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(8, 0, 0) * ftMM, new XYZ(bus2X - 10, 0, 0) * ftMM)).LineStyle = wideLine;
                    trans.Commit();
                }
            }
            else
            {
                bus2X = 2;
            }

            // Draw Bus 2 Generator
            if (!tab2GeneratorNotUsed.Checked && !string.IsNullOrWhiteSpace(tab2DeviceType1.Text))
            {
                using (Transaction trans = new Transaction(doc, "Create Bus 2 Generator"))
                {
                    trans.Start();
                    double circuitPosition = (bus2X - 18) * 0.75 + 8;
                    double circuitLength = tab2GeneratorCloseCoupled.Checked ? -72 : -126;
                    string circuitRating = tab2GeneratorValue.Value.ToString() + " kVA";
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView,
                        Line.CreateBound(new XYZ(circuitPosition, 0, 0) * ftMM,
                        new XYZ(circuitPosition, circuitLength, 0) * ftMM));
                    FamilyInstance bridge = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe0368Symbol, draftView); // Generator Bridge
                    FamilyInstance generator = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, circuitLength, 0) * ftMM, ngbe0366Symbol, draftView); // Generator
                    generator.LookupParameter("Rating").Set(circuitRating);

                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge));
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(generator));
                    // Draw Bus 2 Fault Relays (if ANY turned on)
                    insertFaultRelays(circuitPosition, bridge);
                    // Draw Bus 2 Castell Interlock
                    insertCastellInterlock(circuitPosition);
                    // Draw Bus 2 Interlock Lines
                    if (extra13.Checked || extra14.Checked)
                    {
                        if (!tab2TransformerNotUsed.Checked)
                        {
                            interlockLine2 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(circuitPosition - 14, -14, 0) * ftMM)); // Interlock Circuit Line
                            interlockLine2.LineStyle = dashedLine;
                        }
                        else
                        {
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(circuitPosition - 14, -14, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition - 14, -14, 0) * ftMM, new XYZ(14, -14, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(14, -14, 0) * ftMM, new XYZ(0, 0, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            double textPosition = (bus2X - 18) * 0.3 + 8;
#if REVIT2015
                            doc.Create.NewTextNote(draftView, new XYZ(textPosition, -13, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 15 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_BOTTOM, interlockText()).TextNoteType = textStyle;
#else
                            TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(textPosition, -7, 0) * ftMM, interlockText(), textHorizontal);
                            tn.HorizontalAlignment = HorizontalTextAlignment.Center;
                            tn.TextNoteType = textStyle;
#endif
                        }
                    }
                    trans.Commit();
                }
            }

            // Draw Bus 2 Surge Suppression (if turned on)
            if (extra3.Checked && !string.IsNullOrWhiteSpace(tab2DeviceType1.Text))
            {
                using (Transaction t = new Transaction(doc, "Create Bus 2 Surge Suppression"))
                {
                    t.Start();
                    double circuitPosition = (bus2X - 18) * 0.5 + 8;
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, 0, 0) * ftMM, new XYZ(circuitPosition, -36, 0) * ftMM)); // Basic Circuit Line
                    FamilyInstance deviceInstance = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe0372Symbol, draftView); // Device Type
                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(deviceInstance));
#if REVIT2015
                    doc.Create.NewTextNote(draftView, new XYZ(circuitPosition, -37, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 30 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_TOP, "Surge\nSuppression").TextNoteType = textStyle;
#else
                    TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(circuitPosition, -37, 0) * ftMM, "Surge\nSuppression", textHorizontal);
                    tn.HorizontalAlignment = HorizontalTextAlignment.Center;
                    tn.TextNoteType = textStyle;
#endif
                    t.Commit();
                }
            }

            // Draw Bus 2 Transformer
            if (!tab2TransformerNotUsed.Checked && !string.IsNullOrWhiteSpace(tab2DeviceType1.Text))
            {
                double circuitPosition = (bus2X - 18) * 0.25 + 8;
                using (Transaction trans = new Transaction(doc, "Create Bus 2 Transformer"))
                {
                    trans.Start();
                    double circuitLength = tab2TransformerCloseCoupled.Checked ? -72 : -126;
                    string circuitRating = tab2TransformerValue.Text + " kVA";
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView,
                        Line.CreateBound(new XYZ(circuitPosition, 0, 0) * ftMM,
                        new XYZ(circuitPosition, circuitLength, 0) * ftMM));
                    FamilyInstance bridge = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe0368Symbol, draftView); // Transformer Bridge
                    FamilyInstance transformer = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, circuitLength, 0) * ftMM, ngbe0361Symbol, draftView); // Transformer
                    transformer.LookupParameter("Rating").Set(circuitRating);

                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge));
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(transformer));
                    // Draw Bus 2 Inter-tripping (if turned on)
                    if (extra6.Checked)
                    {
                        double intertrippingPosition = (bus2X - 18) * 0.375 + 8;
                        DetailCurve circuitLine2 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(intertrippingPosition, -28, 0) * ftMM));  // Basic Circuit Line
                        circuitLine2.LineStyle = dashedLine;
                        DetailCurve circuitLine3 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(intertrippingPosition, -28, 0) * ftMM, new XYZ(intertrippingPosition, -100, 0) * ftMM)); // Basic Circuit Line
                        circuitLine3.LineStyle = dashedLine;
                        FamilyInstance intertripping = doc.Create.NewFamilyInstance(new XYZ(intertrippingPosition, -100, 0) * ftMM, ngbe9013Symbol, draftView); // Inter-tripping
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge, true));
                        doc.Create.NewAlignment(draftView, circuitLine3.GeometryCurve.Reference, rpmBIMUtils.findReference(intertripping));
                    }
                    // Draw Bus 2 Metering (if turned on)
                    if (extra7.Checked)
                    {
                        FamilyInstance meteringConnection = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -48, 0) * ftMM, ngbe0382Symbol, draftView); // Metering Connection
                        DetailCurve circuitLine2 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -48, 0) * ftMM, new XYZ(circuitPosition - 6, -48, 0) * ftMM)); // Basic Circuit Line
                        DetailCurve circuitLine3 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition - 6, -48, 0) * ftMM, new XYZ(circuitPosition - 18, -48, 0) * ftMM)); // BMS Output Line
                        circuitLine3.LineStyle = dashedLine;
                        FamilyInstance meteredInstance = doc.Create.NewFamilyInstance(new XYZ(circuitPosition - 6, -48, 0) * ftMM, ngbe0405Symbol, draftView); // Metering Connection
#if REVIT2015
                        doc.Create.NewTextNote(draftView, new XYZ(circuitPosition - 19, -48, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 25 * ftMM, TextAlignFlags.TEF_ALIGN_RIGHT | TextAlignFlags.TEF_ALIGN_MIDDLE, "BMS Output").TextNoteType = textStyle;
#else
                        TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(circuitPosition - 19, -46.3, 0) * ftMM, "BMS Output", textHorizontal);
                        tn.HorizontalAlignment = HorizontalTextAlignment.Right;
                        tn.TextNoteType = textStyle;
#endif
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(meteringConnection));
                        doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, circuitLine3.GeometryCurve.Reference);
                        doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(meteringConnection, true));
                        doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(meteredInstance, true));
                    }
                    // Draw Bus 2 Fault Relays (if ANY turned on)
                    insertFaultRelays(circuitPosition, bridge);
                    // Draw Bus 2 Netrual Earth Link (if turned on)
                    insertNELink(circuitPosition);
                    // Draw Bus 2 Castell Interlock
                    insertCastellInterlock(circuitPosition);
                    // Draw Bus 2 Interlock Lines
                    if (extra13.Checked || extra14.Checked)
                    {
                        if (interlockLine2 != null)
                        {
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(circuitPosition + 14, -14, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition + 14, -14, 0) * ftMM, interlockLine2.GeometryCurve.GetEndPoint(1))).LineStyle = dashedLine; // Interlock Circuit Line
                            double textPosition = (bus2X - 18) * 0.6 + 8;
#if REVIT2015
                            doc.Create.NewTextNote(draftView, new XYZ(textPosition, -13, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 15 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_BOTTOM, interlockText()).TextNoteType = textStyle;
#else
                            TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(textPosition, -7, 0) * ftMM, interlockText(), textHorizontal);
                            tn.HorizontalAlignment = HorizontalTextAlignment.Center;
                            tn.TextNoteType = textStyle;
#endif
                        }
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(circuitPosition - 14, -14, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition - 14, -14, 0) * ftMM, new XYZ(14, -14, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(14, -14, 0) * ftMM, new XYZ(0, 0, 0) * ftMM)).LineStyle = dashedLine; // Interlock Circuit Line
                            double textPosition2 = (bus2X - 18) * 0.1 + 8;
#if REVIT2015
                            doc.Create.NewTextNote(draftView, new XYZ(textPosition2, -13, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 15 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_BOTTOM, interlockText()).TextNoteType = textStyle;
#else
                            TextNote tn2 = TextNote.Create(doc, draftView.Id, new XYZ(textPosition2, -7, 0) * ftMM, interlockText(), textHorizontal);
                            tn2.HorizontalAlignment = HorizontalTextAlignment.Center;
                            tn2.TextNoteType = textStyle;
#endif
                    }
                    trans.Commit();
                }
            }

            // Draw Bus 2 Power Factor Correction
            if (powerFactor2Kvar.Value > 0)
            {
                using (Transaction t = new Transaction(doc, "Create Bus 2 Power Factor Correction"))
                {
                    t.Start();
                    double TempX = (bus2X - 18) * 0.9 + 8;
                    double startPosition = TempX;
                    double textPosition = startPosition + (Convert.ToInt32(powerFactor2Steps.Value) * 10) / 2;
                    string text = powerFactor2Kvar.Value.ToString() + "kVAr PFC,\n" + powerFactor2Steps.Value.ToString() + " x " + Math.Floor((powerFactor2Kvar.Value / powerFactor2Steps.Value)) + "kVAr Steps";
                    if (powerFactor2Detuning.Checked) { text += "\nc/w detuning reactors"; }
                    FamilySymbol deviceType = powerFactor2Detuning.Checked ? ngbe9009Symbol : ngbe9007Symbol;
                    doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(startPosition, 0, 0) * ftMM, new XYZ(startPosition, -10, 0) * ftMM)); // Basic Circuit Line
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(startPosition, -10, 0) * ftMM, new XYZ(startPosition + Convert.ToInt32(powerFactor2Steps.Value) * 10, -10, 0) * ftMM)); // Basic Circuit Line
                    for (int d = 0; d < powerFactor2Steps.Value; d++)
                    {
                        TempX += 10;
                        FamilyInstance correctionDevice = doc.Create.NewFamilyInstance(new XYZ(TempX - 5, -10, 0) * ftMM, deviceType, draftView); // Correction Device
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(correctionDevice, true));
                    }
#if REVIT2015
                    doc.Create.NewTextNote(draftView, new XYZ(textPosition, powerFactor2Detuning.Checked ? -40 : -30, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 30 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_TOP, text).TextNoteType = textStyle;
#else
                    TextNote tn2 = TextNote.Create(doc, draftView.Id, new XYZ(textPosition, powerFactor2Detuning.Checked ? -40 : -30, 0) * ftMM, text, textHorizontal);
                    tn2.HorizontalAlignment = HorizontalTextAlignment.Center;
                    tn2.TextNoteType = textStyle;
#endif
                    TempX += 10;
                    bus2XLife = bus2X;
                    bus2X = TempX > bus2X ? Convert.ToInt32(TempX) : bus2X;
                    t.Commit();
                }
            }

            // Draw Bus 1 & 2 Outline
            using (Transaction trans = new Transaction(doc, "Create Bus 1 & 2 Outline"))
            {
                trans.Start();
                doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(bus1X, 40, 0) * ftMM, new XYZ(bus1X, -110, 0) * ftMM)).LineStyle = hiddenLine;
                doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(bus2X, 40, 0) * ftMM, new XYZ(bus2X, -110, 0) * ftMM)).LineStyle = hiddenLine;
                DetailCurve topLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(bus1X, 40, 0) * ftMM, new XYZ(bus2X, 40, 0) * ftMM));
                topLine.LineStyle = hiddenLine;
                DetailElementOrderUtils.SendToBack(doc, draftView, topLine.Id);
                doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(bus1X, -110, 0) * ftMM, new XYZ(bus2X, -110, 0) * ftMM)).LineStyle = hiddenLine;
                trans.Commit();
            }

            // Draw External Earth Bar (if turned on)
            if (extra4.Checked)
            {
                using (Transaction t = new Transaction(doc, "Create External Earth Bar"))
                {
                    t.Start();
                    FamilyInstance earthBar = doc.Create.NewFamilyInstance(new XYZ(bus1X + 10, -117.5, 0) * ftMM, ngbe9012Symbol, draftView); // Earth Bar
                    t.Commit();
                }
            }

            // Draw Internal Earth Bar (if turned on)
            if (extra5.Checked)
            {
                using (Transaction t = new Transaction(doc, "Create Internal Earth Bar"))
                {
                    t.Start();
                    FamilyInstance earthBar = doc.Create.NewFamilyInstance(new XYZ(bus1X + 10, -102.5, 0) * ftMM, ngbe9012Symbol, draftView); // Earth Bar
                    t.Commit();
                }
            }

            // Draw Life safety Supplies Section
            if (tab3LifeSafetyStatus.Checked)
            {
                using (Transaction trans = new Transaction(doc, "Create Life Safety Supplies Section"))
                {
                    trans.Start();
                    circuitRef = 1; // Reset circuit reference to 1
                    lifeX = bus1X - (20 + (tab3LifeSafetyCircuits.Value * circuitGap));
                    int TempX = lifeX + 20;
                    for (int d = 1; d < (tab3LifeSafetyCircuits.Value + 1); d++)
                    {
                        DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(TempX, 0, 0) * ftMM, new XYZ(TempX, 100, 0) * ftMM)); // Basic Circuit Line
                        FamilyInstance deviceInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 18, 0) * ftMM, ngbe0372Symbol, draftView); // Device Type
                        FamilyInstance referenceInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 52.2, 0) * ftMM, ngbe9001Symbol, draftView); // Reference Symbol
                        referenceInstance.LookupParameter("cableReference").Set("L" + circuitRef.ToString());
                        referenceInstance.LookupParameter("cableDescription").Set("Life Safety Supply No. L" + circuitRef.ToString());
                        referenceInstance.LookupParameter("schematicReference").Set(schematicTitle.Text.Trim());
                        FamilyInstance connectedLoadInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 100, 0) * ftMM, ngbe9011Symbol, draftView); // Connected Load
#if REVIT2015
                        doc.Create.NewTextNote(draftView, new XYZ(TempX - 3, 118, 0) * ftMM, XYZ.BasisY, -XYZ.BasisX, 50 * ftMM, TextAlignFlags.TEF_ALIGN_LEFT | TextAlignFlags.TEF_ALIGN_MIDDLE, "Life Safety Supply No. L" + circuitRef).TextNoteType = textStyle;
#else
                        TextNote.Create(doc, draftView.Id, new XYZ(TempX - 4.7, 118, 0) * ftMM, "Life Safety Supply No. L" + circuitRef, textVertical).TextNoteType = textStyle;
#endif
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(deviceInstance));
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(referenceInstance));
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(connectedLoadInstance));
                        if (extra1.Checked)
                        {
                            FamilyInstance terminationInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 40, 0) * ftMM, ngbe0434Symbol, draftView); // Cable Termination
                            doc.Regenerate();
                            doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(terminationInstance));
                        }
                        circuitRef++;
                        TempX += circuitGap;
                    }
                    doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(lifeX + 10, 0, 0) * ftMM, new XYZ(bus1X - 10, 0, 0) * ftMM)).LineStyle = wideLine;
                    double connectionPoint = lifeX - (lifeX - bus1X) * 0.5;
                    if (!tab1TransformerNotUsed.Checked || !tab1GeneratorNotUsed.Checked || (!tab2TransformerNotUsed.Checked && !string.IsNullOrWhiteSpace(tab2DeviceType1.Text)) || (!tab2GeneratorNotUsed.Checked && !string.IsNullOrWhiteSpace(tab2DeviceType1.Text)))
                    {
                        int bus1XTemp = bus1XLife != 0 ? bus1XLife : bus1X;
                        int bus2XTemp = bus2XLife != 0 ? bus2XLife : bus2X;
                        double connectionEndPoint = !tab1TransformerNotUsed.Checked ? (bus1XTemp + 18) * 0.75 - 8 : 
                            !tab1GeneratorNotUsed.Checked ? (bus1XTemp + 18) * 0.25 - 8 :
                            !tab2TransformerNotUsed.Checked ? (bus2XTemp- 18) * 0.25 + 8 : (bus2XTemp - 18) * 0.75 + 8;
                        doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(connectionPoint, 0, 0) * ftMM, new XYZ(connectionPoint, -55, 0) * ftMM));
                        doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(connectionPoint, -55, 0) * ftMM, new XYZ(connectionEndPoint, -55, 0) * ftMM));
                    }
                    doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(lifeX, 40, 0) * ftMM, new XYZ(lifeX, -110, 0) * ftMM)).LineStyle = hiddenLine;
                    DetailCurve topLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(lifeX, 40, 0) * ftMM, new XYZ(bus1X, 40, 0) * ftMM));
                    topLine.LineStyle = hiddenLine;
                    DetailElementOrderUtils.SendToBack(doc, draftView, topLine.Id);
                    doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(lifeX, -110, 0) * ftMM, new XYZ(bus1X, -110, 0) * ftMM)).LineStyle = hiddenLine;
                    trans.Commit();
                }
            }

            // If create sheet is active, start crerating viewports based on user selections
            if (createSheet.Checked)
            {

                // Close created draftView
                foreach (UIView uiv in rpmBIMTools.Load.uiApp.ActiveUIDocument.GetOpenUIViews())
                {
                    if (uiv.ViewId == draftView.Id) { uiv.Close();  }
                }

                // Create scheduleView
                if (extra2.Checked)
                {
                    using (Transaction t = new Transaction(doc, "Creating Legend View"))
                    {
                        t.Start();
                        scheduleView = ViewSchedule.CreateSchedule(doc, ngbe9001Symbol.Category.Id);
                        ScheduleDefinition sd = scheduleView.Definition;
                        foreach (SchedulableField sf in sd.GetSchedulableFields())
                        {
                            if (sf.GetName(doc) == "schematicReference") { schematicReference = sf; }
                            if (sf.GetName(doc) == "cableReference") { cableReference = sf; }
                            if (sf.GetName(doc) == "cableDescription") { cableDescription = sf; }
                            if (sf.GetName(doc) == "cableType") { cableType = sf; }
                            if (sf.GetName(doc) == "cableSize") { cableSize = sf;  }
                            if (sf.GetName(doc) == "cableLength") { cableLength = sf; }
                        }
                        ScheduleField sfSR = sd.AddField(schematicReference);
                        sfSR.IsHidden = true;
                        ScheduleField sfCR = sd.AddField(cableReference);
                        sfCR.ColumnHeading = "Ref";
                        sfCR.GridColumnWidth = 15 * ftMM;
                        sfCR.SheetColumnWidth = 15 * ftMM;
                        sfCR.HorizontalAlignment = ScheduleHorizontalAlignment.Center;
                        ScheduleField sfCD = sd.AddField(cableDescription);
                        sfCD.ColumnHeading = "Circuit Description";
                        sfCD.GridColumnWidth = 100 * ftMM;
                        sfCD.SheetColumnWidth = 100 * ftMM;
                        ScheduleField sfCT = sd.AddField(cableType);
                        sfCT.GridColumnWidth = 60 * ftMM;
                        sfCT.SheetColumnWidth = 60 * ftMM;
                        sfCT.ColumnHeading = "Cable Type";
                        ScheduleField sfCS = sd.AddField(cableSize);
                        sfCS.GridColumnWidth = 60 * ftMM;
                        sfCS.SheetColumnWidth = 60 * ftMM;
                        sfCS.ColumnHeading = "Cable Size";
                        ScheduleField sfCL = sd.AddField(cableLength);
                        sfCL.GridColumnWidth = 40 * ftMM;
                        sfCL.SheetColumnWidth = 40 * ftMM;
                        sfCL.ColumnHeading = "Length";
                        sd.AddFilter(new ScheduleFilter(sfSR.FieldId, ScheduleFilterType.Equal, schematicTitle.Text.Trim()));
                        sd.AddSortGroupField(new ScheduleSortGroupField(sfCR.FieldId, ScheduleSortOrder.Ascending));
                        IEnumerable<String> scheduleNames = from elem in new FilteredElementCollector(doc)
                                                                .OfClass(typeof(ViewSchedule))
                                                            let type = elem as ViewSchedule
                                                            where type.ViewName.Contains("Circuit Schedule")
                                                            select type.ViewName;
                        for (int i = 1; i < 100; i++)
                        {
                            if (scheduleNames.Contains("Circuit Schedule " + i.ToString())) continue;
                            scheduleView.ViewName = "Circuit Schedule " + i.ToString();
                            break;
                        }
                        t.Commit();
                    }
                }

                // Create Legend Notes
                if (note1.Checked || note2.Checked || note3.Checked || note4.Checked || note5.Checked || note6.Checked || note7.Checked || note8.Checked ||
                    note9.Checked || note10.Checked || note11.Checked || note12.Checked || note13.Checked || note14.Checked || note15.Checked || note16.Checked ||
                    note17.Checked || note18.Checked)
                {
                    using (Transaction trans = new Transaction(doc, "Creating Legend View"))
                    {
                        trans.Start();
                        Autodesk.Revit.DB.View lv = new FilteredElementCollector(doc)
                            .OfClass(typeof(Autodesk.Revit.DB.View))
                            .Cast<Autodesk.Revit.DB.View>()
                            .FirstOrDefault(q => q.ViewType == ViewType.Legend);

                        string viewTemplateName = "E61 - LV Schematic";
                        IEnumerable<String> viewNames = from elem in new FilteredElementCollector(doc)
                                                             .OfClass(typeof(Autodesk.Revit.DB.View))
                                                             .OfCategory(BuiltInCategory.OST_Views)
                                                         let type = elem as Autodesk.Revit.DB.View
                                                         where type.ViewName.Contains(viewTemplateName)
                                                        select type.ViewName;
                        for (int i = 1; i < 100; i++)
                        {
                            if (viewNames.Contains(viewTemplateName + " " + i.ToString())) continue;
                            legendView = doc.GetElement(lv.Duplicate(ViewDuplicateOption.Duplicate)) as Autodesk.Revit.DB.View;
                            legendView.ViewName = viewTemplateName + " " + i.ToString();
                            legendView.Scale = 1;
                            legendView.DetailLevel = ViewDetailLevel.Fine;
                            legendView.LookupParameter("Sub-Discipline").Set("E61 - Main HV / LV Distribution");
                            trans.Commit();
                            break;
                        }
                    }
                    // Add Notes Here
                    using (Transaction t = new Transaction(doc, "Creating Legend Notes"))
                    {
                        t.Start();

                        // Find if 2mm underlined Font already exists
                        TextNoteType underlinedStyle = rpmBIMUtils.FindElementByName(rpmBIMTools.Load.liveDoc, typeof(TextNoteType), "2mm Underlined") as TextNoteType;
                        if (underlinedStyle == null)
                        {
                                TextNoteType TextNoteToCopy = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).FirstOrDefault() as TextNoteType;
                                underlinedStyle = TextNoteToCopy.Duplicate("2mm Underlined") as TextNoteType;
                                underlinedStyle.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(2 * ftMM);
                                underlinedStyle.get_Parameter(BuiltInParameter.TEXT_STYLE_UNDERLINE).Set(1);
                        }
                        string text = "Notes:";
#if REVIT2015
                        doc.Create.NewTextNote(legendView, new XYZ(0, 0, 0), XYZ.BasisX, XYZ.BasisY, 100 * ftMM, TextAlignFlags.TEF_ALIGN_LEFT | TextAlignFlags.TEF_ALIGN_TOP, text).TextNoteType = underlinedStyle;
#else
                        TextNote.Create(doc, legendView.Id, new XYZ() * ftMM, 100 * ftMM, text, textHorizontal).TextNoteType = underlinedStyle;
#endif
                        text = null;
                        int noteCount = 1;
                        if (note1.Checked) { text += noteCount++.ToString() + ".\tSwitchboard to be constructed to form " + note1Input1.Text.Trim() + " type " + note1Input2.Text.Trim() + ".\n"; }
                        if (note2.Checked) { text += noteCount++.ToString() + ".\tSwitchboard to be rated " + note2Input2.Text.Trim() + "kA for " + note2Input2.Text.Trim() + " second(s).\n"; }
                        if (note3.Checked) { text += noteCount++.ToString() + ".\tSwitchboard to be " + note3Input.Text.Trim() + " access.\n"; }
                        if (note4.Checked) { text += noteCount++.ToString() + ".\tSwitchboard to have fully rated neutrals throughout.\n"; }
                        if (note5.Checked) { text += noteCount++.ToString() + ".\tSwitchboard to be extensible from both ends.\n"; }
                        if (note6.Checked) { text += noteCount++.ToString() + ".\tRotary handles on isolators.\n"; }
                        if (note7.Checked) { text += noteCount++.ToString() + ".\tAll MCCBs larger than " + note7Input.Text.Trim() + "A to have electronic trip unit.\n"; }
                        if (note8.Checked) { text += noteCount++.ToString() + ".\tOutgoing cables to be " + note8Input.Text.Trim() + " entry.\n"; }
                        if (note9.Checked) { text += noteCount++.ToString() + ".\tSwitchboard to have internal earthbar along its continuous length.\n"; }
                        if (note10.Checked) { text += noteCount++.ToString() + ".\tFuse / Switches on life safety section to be lockable in ON position.\n"; }
                        if (note11.Checked) { text += noteCount++.ToString() + ".\tFire alarm supply to have RED coloured door.\n"; }
                        if (note12.Checked) { text += noteCount++.ToString() + ".\tAll ACBs to have adjustable electronic protection units."; }
                        if (note13.Checked) { text += "\n" + noteCount++.ToString() + ".\t" + note13Input.Text.Trim(); }
                        if (note14.Checked) { text += "\n" + noteCount++.ToString() + ".\t" + note14Input.Text.Trim(); }
                        if (note15.Checked) { text += "\n" + noteCount++.ToString() + ".\t" + note15Input.Text.Trim(); }
                        if (note16.Checked) { text += "\n" + noteCount++.ToString() + ".\t" + note16Input.Text.Trim(); }
                        if (note17.Checked) { text += "\n" + noteCount++.ToString() + ".\t" + note17Input.Text.Trim(); }
                        if (note18.Checked) { text += "\n" + noteCount++.ToString() + ".\t" + note18Input.Text.Trim(); }
#if REVIT2015
                        doc.Create.NewTextNote(legendView, new XYZ(0, -5, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 100 * ftMM, TextAlignFlags.TEF_ALIGN_LEFT | TextAlignFlags.TEF_ALIGN_TOP, text).TextNoteType = textStyle;
#else
                        TextNote.Create(doc, legendView.Id, new XYZ(0, -5, 0) * ftMM, 100 * ftMM, text, textHorizontal).TextNoteType = textStyle;
#endif
                        t.Commit();
                    }
                }

                // Create ViewSheet and ViewPorts
                using (Transaction trans = new Transaction(doc, "Create Sheet"))
                {
                    IEnumerable<String> sheetNames = from elem in new FilteredElementCollector(doc)
                                               .OfClass(typeof(ViewSheet))
                                               .OfCategory(BuiltInCategory.OST_Sheets)
                                                        let type = elem as ViewSheet
                                                        where type.SheetNumber.StartsWith("ZZ-XX-DR-E-40")
                                                        select type.SheetNumber;
                    for (int i = 1; i < 100; i++)
                    {
                        if (sheetNames.Contains("ZZ-XX-DR-E-40"+ i.ToString("00"))) continue;
                        string username = new String(Environment.UserName.Where(Char.IsLetter).ToArray());
                        username = username.Substring(username.Length - 1, 1) + " " + username.Substring(0, 1) + username.Substring(1, username.Length - 2);
                        trans.Start();
                        ViewSheet viewSheet = ViewSheet.Create(doc, titleBlock.Id);
                        viewSheet.SetSubDiscipline("E40 - Main HV Distribution");
                        viewSheet.SetDrawnBy(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(username));
                        viewSheet.SheetNumber = "ZZ-XX-DR-E-40" + i.ToString("00");
                        viewSheet.Name = "Main HV / LV Distribution Schematic, " + schematicTitle.Text.Trim() + ", Sheet " + i.ToString();
                        if (scheduleView != null)
                        {
                            ScheduleSheetInstance.Create(doc, viewSheet.Id, scheduleView.Id, new XYZ(-1135, 810, 0) * ftMM);
                        }
                        if (legendView != null)
                        {
                            if (Viewport.CanAddViewToSheet(doc, viewSheet.Id, legendView.Id))
                            {
                                Viewport.Create(doc, viewSheet.Id, legendView.Id, new XYZ(-176, 795, 0) * ftMM);
                            }
                        }
                        if (Viewport.CanAddViewToSheet(doc, viewSheet.Id, draftView.Id))
                        {
                            BoundingBoxUV sheetBox = viewSheet.Outline;
                            double yPosition = (sheetBox.Max.V - sheetBox.Min.V) / 2 + sheetBox.Min.V;
                            double xPosition = (sheetBox.Max.U - sheetBox.Min.U) / 2 + sheetBox.Min.U;
                            XYZ origin = new XYZ(xPosition, yPosition, 0);
                            Viewport.Create(doc, viewSheet.Id, draftView.Id, origin);
                        }

                        trans.Commit();
                        // Set ViewSheet as active view
                        rpmBIMTools.Load.uiApp.ActiveUIDocument.ActiveView = viewSheet;
                        break;
                    }
                }
            }

            // Closes both windows
            waitingWindow.Close();
            this.Close();
        }
    }
}