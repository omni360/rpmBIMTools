using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Net;
using System.IO;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.Attributes;

using rpmBIMTools.Create;

namespace rpmBIMTools
{
    [TransactionAttribute(TransactionMode.Manual)]
    
    public partial class importLVSchematic : IExternalCommand
    {
        // Setup basic shared variables
        public Document doc;
        public UIApplication uiApp;
        public LVSchematic schematic = null;
        public ViewDrafting draftView;
        public Autodesk.Revit.DB.View legendView = null;
        public Element wideLine, hiddenLine, dashedLine;
        public ViewSchedule scheduleView = null;
        public DetailCurve interlockLine1 = null, interlockLine2 = null;
        public SchedulableField schematicReference, cableReference, cableDescription, cableType, cableSize, cableLength;
        public FamilySymbol ngbe0361Symbol, ngbe0366Symbol, ngbe0368Symbol, ngbe0369Symbol, ngbe0370Symbol, ngbe0372Symbol, ngbe0375Symbol, ngbe0377Symbol,
            ngbe0382Symbol, ngbe0405Symbol, ngbe0433Symbol, ngbe0434Symbol, ngbe9001Symbol, ngbe9002Symbol, ngbe9003Symbol, ngbe9004Symbol, ngbe9005Symbol,
            ngbe9006Symbol, ngbe9007Symbol, ngbe9009Symbol, ngbe9010Symbol, ngbe9011Symbol, ngbe9012Symbol, ngbe9013Symbol, ngbe9014Symbol;
        int bus1X, bus1XLife, bus2X, bus2XLife, lifeX, circuitGap;
        double ftMM = 1 / 304.8; // Feet to Millimetres

        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {

            // Command Variables
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            doc = rpmBIMTools.Load.liveDoc;
            uiApp = rpmBIMTools.Load.uiApp;

            // Initialize Schematic Variables
            circuitGap = 20; // Space between each curcuit

            // Get XML File Location
            OpenFileDialog xmlPath = new OpenFileDialog();
            xmlPath.Title = "Import LV Schematic File";
            xmlPath.Filter = "XML File (*.xml)|*.xml";
            if (xmlPath.ShowDialog() != DialogResult.OK)
            {
                return Result.Cancelled;
            }

            // Create XmlReader for validation and importing
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.DtdProcessing = DtdProcessing.Ignore;
            readerSettings.ValidationType = ValidationType.Schema;
            readerSettings.Schemas.Add(XmlSchema.Read(new StringReader(Properties.Resources.LVSchematicSchema), null));
            XmlReader xml = XmlReader.Create(xmlPath.InitialDirectory + xmlPath.FileName, readerSettings);

            // xml validation before importing
            try
            {
                XDocument xDoc = XDocument.Load(xmlPath.InitialDirectory + xmlPath.FileName);
                xDoc.Validate(readerSettings.Schemas, null);
            }
            catch (XmlSchemaValidationException e)
            {
                TaskDialog.Show("Import LV Schematic", "Invalid file structure, could not import.");
                TaskDialog.Show("Warning!", e.Message);
                return Result.Failed;
            }

            // Deserialize XML into Class for importing
            XmlSerializer ser = new XmlSerializer(typeof(LVSchematic));
            schematic = ser.Deserialize(xml) as LVSchematic;

            // Error Checking ----------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------

            // Find if drafting view already exists
            IEnumerable<ViewDrafting> draftingViews = from elem in new FilteredElementCollector(doc)
                                                       .OfClass(typeof(ViewDrafting))
                                                      let type = elem as ViewDrafting
                                                      where type.ViewName.Equals(schematic.Name.Trim())
                                                      select type;
            if (draftingViews.Count() > 0)
            {
                TaskDialog.Show("Import LV Schematic", "Schematic already exists in this project.");
                return Result.Failed;
            }

            // Find A0 NGB Title Block
            FamilySymbol titleBlock = rpmBIMUtils.findNGBTitleBlock(doc, "A0");
            if (titleBlock == null)
            {
                TaskDialog.Show("Import LV Schematic", "No NGB title block family found in this project");
                return Result.Failed;
            }

            // Find required lineStyles
            ICollection<ElementId> lineStyles = rpmBIMUtils.GetLineStyles(doc);
            foreach (ElementId lineStyle in lineStyles)
            {
                Element elem = doc.GetElement(lineStyle);
                if (elem.Name.Equals("Wide Lines")) wideLine = elem;
                if (elem.Name.Equals("<Hidden>")) hiddenLine = elem;
                if (elem.Name.Equals("<Overhead>")) dashedLine = elem;
            }
            if (wideLine == null || hiddenLine == null || dashedLine == null)
            {
                TaskDialog.Show("Import LV Schematic", "Required Line Styles not found");
                return Result.Failed;
            };

            // Element Creation --------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------

            // Find if 2mm Font already exists
            TextNoteType textStyle = rpmBIMUtils.FindElementByName(doc, typeof(TextNoteType), "2mm") as TextNoteType;
            if (textStyle == null)
            {
                using (Transaction t = new Transaction(doc, "Create new TextNoteType for schematic"))
                {
                    t.Start();
                    TextNoteType TextNoteToCopy = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).FirstOrDefault() as TextNoteType;
                    textStyle = TextNoteToCopy.Duplicate("2mm") as TextNoteType;
                    textStyle.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(2 * ftMM);
                    t.Commit();
                }
            }

#if !REVIT2015
            // Setup TextNoteOptions
            TextNoteOptions textHorizontal = new TextNoteOptions();
            textHorizontal.TypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
            TextNoteOptions textVertical = new TextNoteOptions();
            textVertical.TypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
            textVertical.Rotation = Math.PI / 2;
#endif

            // Open waiting window
            WaitingWindow waitingWindow = new WaitingWindow("Loading Required Resources\nPlease Wait...");
            waitingWindow.Location = new System.Drawing.Point(
               uiApp.MainWindowExtents.Left + ((uiApp.MainWindowExtents.Right - uiApp.MainWindowExtents.Left) - waitingWindow.Width) / 2,
               uiApp.MainWindowExtents.Top + ((uiApp.MainWindowExtents.Bottom - uiApp.MainWindowExtents.Top) - waitingWindow.Height) / 2);
            waitingWindow.Show();
            Application.DoEvents();

            // Check if family symbols already exist, if not load them from local drive
            try
            {
                string path = @"C:\rpmBIM\families\elec schematic\";
                ngbe0361Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Transformer", path + "SM - Equipment - Transformer.rfa");
                ngbe0366Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Generator", path + "SM - Equipment - Generator.rfa");
                ngbe0368Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Bus Link Bridge", path + "SM - Device - Bus Link Bridge.rfa");
                ngbe0369Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Air Circuit Breaker", path + "SM - Device - Air Circuit Breaker.rfa");
                ngbe0370Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Fused Switch Disconnector", path + "SM - Device - Fused Switch Disconnector.rfa");
                ngbe0372Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Fused Disconnector", path + "SM - Device - Fused Disconnector.rfa");
                ngbe0375Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Miniture Circuit Breaker", path + "SM - Device - Miniture Circuit Breaker.rfa");
                ngbe0382Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Meter Connection", path + "SM - Device - Meter Connection.rfa");
                ngbe0405Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Metered Symbol", path + "SM - Device - Metered Symbol.rfa");
                ngbe0433Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Moulded Case Circuit Breaker", path + "SM - Device - Moulded Case Circuit Breaker.rfa");
                ngbe0434Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Cable Termination", path + "SM - Device - Cable Termination.rfa");
                ngbe9001Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Annotation - Circuit Reference Symbol", path + "SM - Annotation - Circuit Reference Symbol.rfa");
                ngbe9002Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Distribution Board", path + "SM - Equipment - Distribution Board.rfa");
                ngbe9003Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Undefined", path + "SM - Device - Undefined.rfa");
                ngbe9004Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Sub-Panel Board", path + "SM - Equipment - Sub-Panel Board.rfa");
                ngbe9005Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Isolator", path + "SM - Equipment - Isolator.rfa");
                ngbe9006Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Spare", path + "SM - Equipment - Spare.rfa");
                ngbe9007Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Correction Device", path + "SM - Device - Correction Device.rfa");
                ngbe9009Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Correction Device (with detuning)", path + "SM - Device - Correction Device (with detuning).rfa");
                ngbe9010Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Annotation - Castell Interlock", path + "SM - Annotation - Castell Interlock.rfa");
                ngbe9011Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Life Safety Unit", path + "SM - Equipment - Life Safety Unit.rfa");
                ngbe9012Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Earth Bar", path + "SM - Equipment - Earth Bar.rfa");
                ngbe9013Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Inter-tripping", path + "SM - Equipment - Inter-tripping.rfa");
                ngbe0377Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Device - Netrual Earth Link", path + "SM - Device - Netrual Earth Link.rfa");
                ngbe9014Symbol = rpmBIMUtils.findOrLoadFamilySymbol(doc, "SM - Equipment - Netrual Earth Link Bar", path + "SM - Equipment - Netrual Earth Link Bar.rfa");
            }
            catch (Autodesk.Revit.Exceptions.FileNotFoundException ex)
            {
                TaskDialog.Show("Import LV Schematic", "Unable to load the family symbol located at:\n\n" + ex.Message);
                waitingWindow.Dispose();
                return Result.Failed; ;
            }

            // Update waiting window
            waitingWindow.UpdateText("Importing LV Schematic\nPlease Wait...");

            // Create DraftingView
            using (Transaction trans = new Transaction(doc, "Creating Drafting View"))
            {
                trans.Start();
                ViewFamilyType vft = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewFamilyType))
                    .Cast<ViewFamilyType>()
                    .FirstOrDefault(q => q.ViewFamily == ViewFamily.Drafting);
                draftView = ViewDrafting.Create(doc, vft.Id);
                draftView.LookupParameter("Sub-Discipline").Set("E61 - Main HV / LV Distribution");
                draftView.Discipline = ViewDiscipline.Coordination;
                draftView.ViewName = schematic.Name.Trim();
                draftView.Scale = 1;
                draftView.DetailLevel = ViewDetailLevel.Fine;
                trans.Commit();
                // Set DraftingView as active view
                rpmBIMTools.Load.uiApp.ActiveUIDocument.ActiveView = draftView;
            }

            // Draw Bus Link Bridge
            if (schematic.Bus2 != null)
            {
                using (Transaction trans = new Transaction(doc, "Create Bus Link Bridge"))
                {
                    trans.Start();
                    FamilyInstance bridge = doc.Create.NewFamilyInstance(new XYZ(0, 0, 0) * ftMM, ngbe0368Symbol, draftView); // Bus Link Bridge
                    LocationPoint location = bridge.Location as LocationPoint;
                    Line axis = Line.CreateBound(location.Point, new XYZ(location.Point.X, location.Point.Y, location.Point.Z + 10) * ftMM);
                    bridge.Location.Rotate(axis, Math.PI / 2);
                    if (schematic.Settings.IncludeCastellInterlock)
                    {
                        doc.Create.NewFamilyInstance(new XYZ(0, -7, 0) * ftMM, ngbe9010Symbol, draftView); // Castell Interlock
                    }
                    trans.Commit();
                }
            }

            // Draw Bus 1 Circuits
            bus1X = -18 - (schematic.Bus1.Circuit.Count() * circuitGap);
            // Bus 1 Minimum size
            bus1X = bus1X > -18 - circuitGap * 6 ? -18 - circuitGap * 6 : bus1X;

            using (Transaction trans = new Transaction(doc, "Create Bus 1 Circuits"))
            {
                trans.Start();
                int TempX = bus1X + 20;
                foreach (Circuit circuit in schematic.Bus1.Circuit)
                {
                    FamilySymbol deviceFamily =
                        circuit.Device.Type == "SM - Device - Air Circuit Breaker" ? ngbe0369Symbol :
                        circuit.Device.Type == "SM - Device - Moulded Case Circuit Breaker" ? ngbe0433Symbol :
                        circuit.Device.Type == "SM - Device - Fused Switch Disconnector" ? ngbe0370Symbol :
                        circuit.Device.Type == "SM - Device - Miniture Circuit Breaker" ? ngbe0375Symbol :
                        ngbe9003Symbol;
                    FamilySymbol connectedLoadFamily =
                        circuit.ConnectedLoad.Type == "SM - Equipment - Distribution Board" ? ngbe9002Symbol :
                        circuit.ConnectedLoad.Type == "SM - Equipment - Sub-Panel Board" ? ngbe9004Symbol :
                        circuit.ConnectedLoad.Type == "SM - Equipment - Isolator" ? ngbe9005Symbol :
                        ngbe9006Symbol;
                    double textPosition =
                        circuit.ConnectedLoad.Type == "SM - Equipment - Distribution Board" ? 130.6 :
                        circuit.ConnectedLoad.Type == "SM - Equipment - Sub-Panel Board" ? 147.6 :
                        circuit.ConnectedLoad.Type == "SM - Equipment - Isolator" ? 109.6 :
                        115.2;
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(TempX, 0, 0) * ftMM, new XYZ(TempX, 100, 0) * ftMM)); // Basic Circuit Line
                    FamilyInstance referenceInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 52.2, 0) * ftMM, ngbe9001Symbol, draftView) as FamilyInstance; // Reference Symbol
                    if (referenceInstance.LookupParameter("schematicReference") != null)
                    {
                        referenceInstance.LookupParameter("schematicReference").Set(schematic.Name.Trim());
                    }
                    if (referenceInstance.LookupParameter("cableType") != null)
                    {
                        referenceInstance.LookupParameter("cableType").Set(circuit.Type);
                    }
                    if (referenceInstance.LookupParameter("cableSize") != null)
                    {
                        referenceInstance.LookupParameter("cableSize").Set(circuit.Size);
                    }
                    if (referenceInstance.LookupParameter("cableReference") != null)
                    {
                        referenceInstance.LookupParameter("cableReference").Set(circuit.Ref);
                    }
                    if (referenceInstance.LookupParameter("cableLength") != null)
                    {
                        referenceInstance.LookupParameter("cableLength").Set(circuit.Length);
                    }
                    if (referenceInstance.LookupParameter("cableDescription") != null)
                    {
                        referenceInstance.LookupParameter("cableDescription").Set(circuit.Description);
                    }
                    FamilyInstance deviceInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 18, 0) * ftMM, deviceFamily, draftView); // Device Type
                    if (deviceInstance.LookupParameter("Ref") != null)
                    {
                        deviceInstance.LookupParameter("Ref").Set(circuit.Device.Ref);
                    }
                    if (deviceInstance.LookupParameter("Rating") != null)
                    {
                        deviceInstance.LookupParameter("Rating").Set(circuit.Device.Rating);
                    }
                    if (deviceInstance.LookupParameter("Poles") != null)
                    {
                        deviceInstance.LookupParameter("Poles").Set(circuit.Device.Poles);
                    }
                    if (deviceInstance.LookupParameter("Frame") != null)
                    {
                        deviceInstance.LookupParameter("Frame").Set(circuit.Device.Frame);
                    }
                    FamilyInstance connectedLoadInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 100, 0) * ftMM, connectedLoadFamily, draftView); // Connected Load
                    if (connectedLoadInstance.LookupParameter("DB ZS") != null)
                    {
                        connectedLoadInstance.LookupParameter("DB ZS").Set(circuit.ConnectedLoad.DbZS);
                    }
                    if (connectedLoadInstance.LookupParameter("DB Ways") != null)
                    {
                        connectedLoadInstance.LookupParameter("DB Ways").Set(circuit.ConnectedLoad.DbWays);
                    }
                    if (connectedLoadInstance.LookupParameter("DB Reference") != null)
                    {
                        connectedLoadInstance.LookupParameter("DB Reference").Set(circuit.ConnectedLoad.DbReference);
                    }
                    if (connectedLoadInstance.LookupParameter("DB Rating") != null)
                    {
                        connectedLoadInstance.LookupParameter("DB Rating").Set(circuit.ConnectedLoad.DbRating);
                    }
                    if (connectedLoadInstance.LookupParameter("DB Phase") != null)
                    {
                        connectedLoadInstance.LookupParameter("DB Phase").Set(circuit.ConnectedLoad.DbPhase);
                    }
                    if (connectedLoadInstance.LookupParameter("DB IF") != null)
                    {
                        connectedLoadInstance.LookupParameter("DB IF").Set(circuit.ConnectedLoad.DbIF);
                    }
                    if (connectedLoadInstance.LookupParameter("DB IB") != null)
                    {
                        connectedLoadInstance.LookupParameter("DB IB").Set(circuit.ConnectedLoad.DbIB);
                    }
#if REVIT2015
                    doc.Create.NewTextNote(draftView, new XYZ(TempX, textPosition, 0) * ftMM, XYZ.BasisY, -XYZ.BasisX, 50 * ftMM, TextAlignFlags.TEF_ALIGN_LEFT | TextAlignFlags.TEF_ALIGN_MIDDLE, circuit.Description).TextNoteType = textStyle;
#else
                    TextNote.Create(doc, draftView.Id, new XYZ(TempX - 1.7, textPosition, 0) * ftMM, circuit.Description, textVertical).TextNoteType = textStyle;
#endif
                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(deviceInstance));
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(referenceInstance));
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(connectedLoadInstance));
                    if (circuit.Metered)
                    {
                        FamilyInstance meteredInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 28.2, 0) * ftMM, ngbe0405Symbol, draftView); // Metered Symbol
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(meteredInstance));
                    }
                    if (schematic.Settings.IncludeTerminations)
                    {
                        FamilyInstance terminationInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 40, 0) * ftMM, ngbe0434Symbol, draftView); // Cable Termination
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(terminationInstance));
                    }
                    TempX += circuitGap;
                }
                doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(bus1X + 10, 0, 0) * ftMM, new XYZ(-8, 0, 0) * ftMM)).LineStyle = wideLine;
                trans.Commit();
            }

            // Draw Bus 1 Transformer
            if (schematic.Bus1.SupplyTransformer != null)
            {
                double circuitPosition = (bus1X + 18) * 0.75 - 8;
                using (Transaction trans = new Transaction(doc, "Create Bus 1 Transformer"))
                {
                    trans.Start();
                    double circuitLength = schematic.Bus1.SupplyTransformer.Location == "Internal" ? -72 : -126;
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView,
                        Line.CreateBound(new XYZ(circuitPosition, 0, 0) * ftMM,
                        new XYZ(circuitPosition, circuitLength, 0) * ftMM));
                    FamilyInstance bridge = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe0368Symbol, draftView); // Transformer Bridge
                    if (bridge.LookupParameter("Ref") != null)
                    {
                        bridge.LookupParameter("Ref").Set(schematic.Bus1.SupplyTransformer.LinkBridge.Ref);
                    }
                    if (bridge.LookupParameter("Rating") != null)
                    {
                        bridge.LookupParameter("Rating").Set(schematic.Bus1.SupplyTransformer.LinkBridge.Rating);
                    }
                    if (bridge.LookupParameter("Poles") != null)
                    {
                        bridge.LookupParameter("Poles").Set(schematic.Bus1.SupplyTransformer.LinkBridge.Poles);
                    }
                    if (bridge.LookupParameter("Frame") != null)
                    {
                        bridge.LookupParameter("Frame").Set(schematic.Bus1.SupplyTransformer.LinkBridge.Frame);
                    }
                    FamilyInstance transformer = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, circuitLength, 0) * ftMM, ngbe0361Symbol, draftView); // Transformer
                    if (transformer.LookupParameter("Voltage") != null)
                    {
                        transformer.LookupParameter("Voltage").Set(schematic.Bus1.SupplyTransformer.Voltage);
                    }
                    if (transformer.LookupParameter("Vector Group") != null)
                    {
                        transformer.LookupParameter("Vector Group").Set(schematic.Bus1.SupplyTransformer.VectorGroup);
                    }
                    if (transformer.LookupParameter("Type") != null)
                    {
                        transformer.LookupParameter("Type").Set(schematic.Bus1.SupplyTransformer.Type);
                    }
                    if (transformer.LookupParameter("Ref") != null)
                    {
                        transformer.LookupParameter("Ref").Set(schematic.Bus1.SupplyTransformer.Ref);
                    }
                    if (transformer.LookupParameter("Rating") != null)
                    {
                        transformer.LookupParameter("Rating").Set(schematic.Bus1.SupplyTransformer.Rating);
                    }
                    if (transformer.LookupParameter("Cooling") != null)
                    {
                        transformer.LookupParameter("Cooling").Set(schematic.Bus1.SupplyTransformer.Cooling);
                    }
                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge));
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(transformer));
                    // Draw Bus 1 Inter-tripping (if turned on)
                    if (schematic.Settings.IncludeInterTripping)
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
                    if (schematic.Settings.IncludeMeteringOnMain)
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
                    // Draw Bus 1 Fault Relays (if ANY turned on)
                    insertFaultRelays(circuitPosition, bridge);
                    // Draw Bus 1 Netrual Earth Link (if turned on)
                    insertNELink(circuitPosition);
                    // Draw Bus 1 Castell Interlock
                    insertCastellInterlock(circuitPosition);
                    // Draw Bus 1 Interlock Lines
                    if ((schematic.Settings.IncludeElectricalInterlock || schematic.Settings.IncludeMechanicalInterlock) && (schematic.Bus1.SupplyGenerator != null || schematic.Bus2 != null))
                    {
                        if (schematic.Bus1.SupplyGenerator != null)
                        {
                            interlockLine1 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(circuitPosition + 14, -14, 0) * ftMM)); // Interlock Circuit Line
                            interlockLine1.LineStyle = dashedLine;
                        }
                        else
                        {
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
            if (schematic.Settings.IncludeSurgeSuppression)
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
            if (schematic.Bus1.SupplyGenerator != null)
            {
                using (Transaction trans = new Transaction(doc, "Create Bus 1 Genrator"))
                {
                    trans.Start();
                    double circuitPosition = (bus1X + 18) * 0.25 - 8;
                    double circuitLength = schematic.Bus1.SupplyGenerator.Location == "Internal" ? -72 : -126;
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView,
                        Line.CreateBound(new XYZ(circuitPosition, 0, 0) * ftMM,
                        new XYZ(circuitPosition, circuitLength, 0) * ftMM));
                    FamilyInstance bridge = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe0368Symbol, draftView); // Generator Bridge
                    if (bridge.LookupParameter("Ref") != null)
                    {
                        bridge.LookupParameter("Ref").Set(schematic.Bus1.SupplyGenerator.LinkBridge.Ref);
                    }
                    if (bridge.LookupParameter("Rating") != null)
                    {
                        bridge.LookupParameter("Rating").Set(schematic.Bus1.SupplyGenerator.LinkBridge.Rating);
                    }
                    if (bridge.LookupParameter("Poles") != null)
                    {
                        bridge.LookupParameter("Poles").Set(schematic.Bus1.SupplyGenerator.LinkBridge.Poles);
                    }
                    if (bridge.LookupParameter("Frame") != null)
                    {
                        bridge.LookupParameter("Frame").Set(schematic.Bus1.SupplyGenerator.LinkBridge.Frame);
                    }
                    FamilyInstance generator = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, circuitLength, 0) * ftMM, ngbe0366Symbol, draftView); // Generator
                    if (generator.LookupParameter("Ref") != null)
                    {
                        generator.LookupParameter("Ref").Set(schematic.Bus1.SupplyGenerator.Ref);
                    }
                    if (generator.LookupParameter("Rating") != null)
                    {
                        generator.LookupParameter("Rating").Set(schematic.Bus1.SupplyGenerator.Rating);
                    }
                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge));
                    doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(generator));
                    // Draw Bus 1 Fault Relays (if ANY turned on)
                    insertFaultRelays(circuitPosition, bridge);
                    // Draw Bus 1 Castell Interlock
                    insertCastellInterlock(circuitPosition);
                    // Draw Bus 1 Interlock Lines
                    if (schematic.Settings.IncludeElectricalInterlock || schematic.Settings.IncludeMechanicalInterlock)
                    {
                        if (interlockLine1 != null)
                        {
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
                        if (schematic.Bus2 != null)
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
            if (schematic.Bus1.PowerFactorCorrection != null)
            {
                using (Transaction t = new Transaction(doc, "Create Bus 1 Power Factor Correction"))
                {
                    t.Start();
                    double TempX = (bus1X + 18) * 0.9 - 8;
                    double startPosition = TempX;
                    double textPosition = startPosition - (schematic.Bus1.PowerFactorCorrection.Steps * 10) / 2;
                    string text = schematic.Bus1.PowerFactorCorrection.Total.ToString() + "kVAr PFC,\n" + schematic.Bus1.PowerFactorCorrection.Steps.ToString() + " x " + schematic.Bus1.PowerFactorCorrection.Total / schematic.Bus1.PowerFactorCorrection.Steps + "kVAr Steps";
                    if (schematic.Bus1.PowerFactorCorrection.Type == "SM - Device - Correction Device (with detuning)") { text += "\nc/w detuning reactors"; }
                    FamilySymbol deviceType = schematic.Bus1.PowerFactorCorrection.Type == "SM - Device - Correction Device (with detuning)" ? ngbe9009Symbol : ngbe9007Symbol;
                    doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(startPosition, 0, 0) * ftMM, new XYZ(startPosition, -10, 0) * ftMM)); // Basic Circuit Line
                    DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(startPosition, -10, 0) * ftMM, new XYZ(startPosition - schematic.Bus1.PowerFactorCorrection.Steps * 10, -10, 0) * ftMM)); // Basic Circuit Line
                    for (int d = 0; d < schematic.Bus1.PowerFactorCorrection.Steps; d++)
                    {
                        TempX -= 10;
                        FamilyInstance correctionDevice = doc.Create.NewFamilyInstance(new XYZ(TempX + 5, -10, 0) * ftMM, deviceType, draftView); // Correction Device
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(correctionDevice, true));
                    }
#if REVIT2015
                    doc.Create.NewTextNote(draftView, new XYZ(textPosition, schematic.Bus1.PowerFactorCorrection.Type == "SM - Device - Correction Device (with detuning)" ? -30 : -40, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 30 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_TOP, text).TextNoteType = textStyle;
#else
                    TextNote tn = TextNote.Create(doc, draftView.Id, new XYZ(textPosition, schematic.Bus1.PowerFactorCorrection.Type == "SM - Device - Correction Device (with detuning)" ? -30 : -40, 0) * ftMM, text, textHorizontal);
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
            if (schematic.Bus2 != null)
            {
                using (Transaction trans = new Transaction(doc, "Create Bus 2 Circuits"))
                {
                    trans.Start();
                    bus2X = 18;

                    foreach (Circuit circuit in schematic.Bus2.Circuit)
                    {
                        FamilySymbol deviceFamily =
                           circuit.Device.Type == "SM - Device - Air Circuit Breaker" ? ngbe0369Symbol :
                           circuit.Device.Type == "SM - Device - Moulded Case Circuit Breaker" ? ngbe0433Symbol :
                           circuit.Device.Type == "SM - Device - Fused Switch Disconnector" ? ngbe0370Symbol :
                           circuit.Device.Type == "SM - Device - Miniture Circuit Breaker" ? ngbe0375Symbol :
                           ngbe9003Symbol;
                        FamilySymbol connectedLoadFamily =
                            circuit.ConnectedLoad.Type == "SM - Equipment - Distribution Board" ? ngbe9002Symbol :
                            circuit.ConnectedLoad.Type == "SM - Equipment - Sub-Panel Board" ? ngbe9004Symbol :
                            circuit.ConnectedLoad.Type == "SM - Equipment - Isolator" ? ngbe9005Symbol :
                            ngbe9006Symbol;
                        double textPosition =
                            circuit.ConnectedLoad.Type == "SM - Equipment - Distribution Board" ? 130.6 :
                            circuit.ConnectedLoad.Type == "SM - Equipment - Sub-Panel Board" ? 147.6 :
                            circuit.ConnectedLoad.Type == "SM - Equipment - Isolator" ? 109.6 :
                            115.2;
                        DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(bus2X, 0, 0) * ftMM, new XYZ(bus2X, 100, 0) * ftMM)); // Basic Circuit Line
                        FamilyInstance referenceInstance = doc.Create.NewFamilyInstance(new XYZ(bus2X, 52.2, 0) * ftMM, ngbe9001Symbol, draftView); // Reference Symbol
                        if (referenceInstance.LookupParameter("schematicReference") != null)
                        {
                            referenceInstance.LookupParameter("schematicReference").Set(schematic.Name.Trim());
                        }
                        if (referenceInstance.LookupParameter("cableType") != null)
                        {
                            referenceInstance.LookupParameter("cableType").Set(circuit.Type);
                        }
                        if (referenceInstance.LookupParameter("cableSize") != null)
                        {
                            referenceInstance.LookupParameter("cableSize").Set(circuit.Size);
                        }
                        if (referenceInstance.LookupParameter("cableReference") != null)
                        {
                            referenceInstance.LookupParameter("cableReference").Set(circuit.Ref);
                        }
                        if (referenceInstance.LookupParameter("cableLength") != null)
                        {
                            referenceInstance.LookupParameter("cableLength").Set(circuit.Length);
                        }
                        if (referenceInstance.LookupParameter("cableDescription") != null)
                        {
                            referenceInstance.LookupParameter("cableDescription").Set(circuit.Description);
                        }
                        FamilyInstance deviceInstance = doc.Create.NewFamilyInstance(new XYZ(bus2X, 18, 0) * ftMM, deviceFamily, draftView); // Device Type
                        if (deviceInstance.LookupParameter("Ref") != null)
                        {
                            deviceInstance.LookupParameter("Ref").Set(circuit.Device.Ref);
                        }
                        if (deviceInstance.LookupParameter("Rating") != null)
                        {
                            deviceInstance.LookupParameter("Rating").Set(circuit.Device.Rating);
                        }
                        if (deviceInstance.LookupParameter("Poles") != null)
                        {
                            deviceInstance.LookupParameter("Poles").Set(circuit.Device.Poles);
                        }
                        if (deviceInstance.LookupParameter("Frame") != null)
                        {
                            deviceInstance.LookupParameter("Frame").Set(circuit.Device.Frame);
                        }
                        FamilyInstance connectedLoadInstance = doc.Create.NewFamilyInstance(new XYZ(bus2X, 100, 0) * ftMM, connectedLoadFamily, draftView); // Connected Load
                        if (connectedLoadInstance.LookupParameter("DB ZS") != null)
                        {
                            connectedLoadInstance.LookupParameter("DB ZS").Set(circuit.ConnectedLoad.DbZS);
                        }
                        if (connectedLoadInstance.LookupParameter("DB Ways") != null)
                        {
                            connectedLoadInstance.LookupParameter("DB Ways").Set(circuit.ConnectedLoad.DbWays);
                        }
                        if (connectedLoadInstance.LookupParameter("DB Reference") != null)
                        {
                            connectedLoadInstance.LookupParameter("DB Reference").Set(circuit.ConnectedLoad.DbReference);
                        }
                        if (connectedLoadInstance.LookupParameter("DB Rating") != null)
                        {
                            connectedLoadInstance.LookupParameter("DB Rating").Set(circuit.ConnectedLoad.DbRating);
                        }
                        if (connectedLoadInstance.LookupParameter("DB Phase") != null)
                        {
                            connectedLoadInstance.LookupParameter("DB Phase").Set(circuit.ConnectedLoad.DbPhase);
                        }
                        if (connectedLoadInstance.LookupParameter("DB IF") != null)
                        {
                            connectedLoadInstance.LookupParameter("DB IF").Set(circuit.ConnectedLoad.DbIF);
                        }
                        if (connectedLoadInstance.LookupParameter("DB IB") != null)
                        {
                            connectedLoadInstance.LookupParameter("DB IB").Set(circuit.ConnectedLoad.DbIB);
                        }
#if REVIT2015
                        doc.Create.NewTextNote(draftView, new XYZ(bus2X, textPosition, 0) * ftMM, XYZ.BasisY, -XYZ.BasisX, 50 * ftMM, TextAlignFlags.TEF_ALIGN_LEFT | TextAlignFlags.TEF_ALIGN_MIDDLE, circuit.Description).TextNoteType = textStyle;
#else
                        TextNote.Create(doc, draftView.Id, new XYZ(bus2X - 1.7, textPosition, 0) * ftMM, circuit.Description, textVertical).TextNoteType = textStyle;
#endif
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(deviceInstance));
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(referenceInstance));
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(connectedLoadInstance));
                        if (circuit.Metered)
                        {
                            FamilyInstance meteredInstance = doc.Create.NewFamilyInstance(new XYZ(bus2X, 28.2, 0) * ftMM, ngbe0405Symbol, draftView); // Metered Symbol
                            doc.Regenerate();
                            doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(meteredInstance));
                        }
                        if (schematic.Settings.IncludeTerminations)
                        {
                            FamilyInstance terminationInstance = doc.Create.NewFamilyInstance(new XYZ(bus2X, 40, 0) * ftMM, ngbe0434Symbol, draftView); // Cable Termination
                            doc.Regenerate();
                            doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(terminationInstance));
                        }
                        bus2X += circuitGap;
                    }
                    // Bus 2 Minimum size
                    bus2X = bus2X < 18 + circuitGap * 6 ? 18 + circuitGap * 6 : bus2X;
                    doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(8, 0, 0) * ftMM, new XYZ(bus2X - 10, 0, 0) * ftMM)).LineStyle = wideLine;
                    trans.Commit();
                }

                // Draw Bus 2 Generator
                if (schematic.Bus2.SupplyGenerator != null)
                {
                    using (Transaction trans = new Transaction(doc, "Create Bus 2 Generator"))
                    {
                        trans.Start();
                        double circuitPosition = (bus2X - 18) * 0.75 + 8;
                        double circuitLength = schematic.Bus2.SupplyGenerator.Location == "Internal" ? -72 : -126;
                        DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView,
                            Line.CreateBound(new XYZ(circuitPosition, 0, 0) * ftMM,
                            new XYZ(circuitPosition, circuitLength, 0) * ftMM));
                        FamilyInstance bridge = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe0368Symbol, draftView); // Generator Bridge
                        if (bridge.LookupParameter("Ref") != null)
                        {
                            bridge.LookupParameter("Ref").Set(schematic.Bus2.SupplyGenerator.LinkBridge.Ref);
                        }
                        if (bridge.LookupParameter("Rating") != null)
                        {
                            bridge.LookupParameter("Rating").Set(schematic.Bus2.SupplyGenerator.LinkBridge.Rating);
                        }
                        if (bridge.LookupParameter("Poles") != null)
                        {
                            bridge.LookupParameter("Poles").Set(schematic.Bus2.SupplyGenerator.LinkBridge.Poles);
                        }
                        if (bridge.LookupParameter("Frame") != null)
                        {
                            bridge.LookupParameter("Frame").Set(schematic.Bus2.SupplyGenerator.LinkBridge.Frame);
                        }
                        FamilyInstance generator = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, circuitLength, 0) * ftMM, ngbe0366Symbol, draftView); // Generator
                        if (generator.LookupParameter("Ref") != null)
                        {
                            generator.LookupParameter("Ref").Set(schematic.Bus2.SupplyGenerator.Ref);
                        }
                        if (generator.LookupParameter("Rating") != null)
                        {
                            generator.LookupParameter("Rating").Set(schematic.Bus2.SupplyGenerator.Rating);
                        }
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge));
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(generator));
                        // Draw Bus 2 Fault Relays (if ANY turned on)
                        insertFaultRelays(circuitPosition, bridge);
                        // Draw Bus 2 Castell Interlock
                        insertCastellInterlock(circuitPosition);
                        // Draw Bus 2 Interlock Lines
                        if (schematic.Settings.IncludeElectricalInterlock || schematic.Settings.IncludeMechanicalInterlock)
                        {
                            if (schematic.Bus2.SupplyTransformer != null)
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
                if (schematic.Settings.IncludeSurgeSuppression && schematic.Bus2 != null)
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
                if (schematic.Bus2.SupplyTransformer != null)
                {
                    double circuitPosition = (bus2X - 18) * 0.25 + 8;
                    using (Transaction trans = new Transaction(doc, "Create Bus 2 Transformer"))
                    {
                        trans.Start();
                        double circuitLength = schematic.Bus1.SupplyTransformer.Location == "Internal" ? -72 : -126;
                        DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView,
                            Line.CreateBound(new XYZ(circuitPosition, 0, 0) * ftMM,
                            new XYZ(circuitPosition, circuitLength, 0) * ftMM));
                        FamilyInstance bridge = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe0368Symbol, draftView); // Transformer Bridge
                        if (bridge.LookupParameter("Ref") != null)
                        {
                            bridge.LookupParameter("Ref").Set(schematic.Bus2.SupplyTransformer.LinkBridge.Ref);
                        }
                        if (bridge.LookupParameter("Rating") != null)
                        {
                            bridge.LookupParameter("Rating").Set(schematic.Bus2.SupplyTransformer.LinkBridge.Rating);
                        }
                        if (bridge.LookupParameter("Poles") != null)
                        {
                            bridge.LookupParameter("Poles").Set(schematic.Bus2.SupplyTransformer.LinkBridge.Poles);
                        }
                        if (bridge.LookupParameter("Frame") != null)
                        {
                            bridge.LookupParameter("Frame").Set(schematic.Bus2.SupplyTransformer.LinkBridge.Frame);
                        }
                        FamilyInstance transformer = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, circuitLength, 0) * ftMM, ngbe0361Symbol, draftView); // Transformer
                        if (transformer.LookupParameter("Voltage") != null)
                        {
                            transformer.LookupParameter("Voltage").Set(schematic.Bus2.SupplyTransformer.Voltage);
                        }
                        if (transformer.LookupParameter("Vector Group") != null)
                        {
                            transformer.LookupParameter("Vector Group").Set(schematic.Bus2.SupplyTransformer.VectorGroup);
                        }
                        if (transformer.LookupParameter("Type") != null)
                        {
                            transformer.LookupParameter("Type").Set(schematic.Bus2.SupplyTransformer.Type);
                        }
                        if (transformer.LookupParameter("Ref") != null)
                        {
                            transformer.LookupParameter("Ref").Set(schematic.Bus2.SupplyTransformer.Ref);
                        }
                        if (transformer.LookupParameter("Rating") != null)
                        {
                            transformer.LookupParameter("Rating").Set(schematic.Bus2.SupplyTransformer.Rating);
                        }
                        if (transformer.LookupParameter("Cooling") != null)
                        {
                            transformer.LookupParameter("Cooling").Set(schematic.Bus2.SupplyTransformer.Cooling);
                        }
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(bridge));
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(transformer));
                        // Draw Bus 2 Inter-tripping (if turned on)
                        if (schematic.Settings.IncludeInterTripping)
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
                        if (schematic.Settings.IncludeMeteringOnMain)
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
                        if (schematic.Settings.IncludeElectricalInterlock && schematic.Settings.IncludeMechanicalInterlock)
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
                if (schematic.Bus2.PowerFactorCorrection != null)
                {
                    using (Transaction t = new Transaction(doc, "Create Bus 2 Power Factor Correction"))
                    {
                        t.Start();
                        double TempX = (bus2X - 18) * 0.9 + 8;
                        double startPosition = TempX;
                        double textPosition = startPosition + (schematic.Bus2.PowerFactorCorrection.Steps * 10) / 2;
                        string text = schematic.Bus2.PowerFactorCorrection.Total.ToString() + "kVAr PFC,\n" + schematic.Bus2.PowerFactorCorrection.Steps.ToString() + " x " + schematic.Bus2.PowerFactorCorrection.Total / schematic.Bus2.PowerFactorCorrection.Steps + "kVAr Steps";
                        if (schematic.Bus2.PowerFactorCorrection.Type == "SM - Device - Correction Device (with detuning)") { text += "\nc/w detuning reactors"; }
                        FamilySymbol deviceType = schematic.Bus2.PowerFactorCorrection.Type == "SM - Device - Correction Device (with detuning)" ? ngbe9009Symbol : ngbe9007Symbol;
                        doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(startPosition, 0, 0) * ftMM, new XYZ(startPosition, -10, 0) * ftMM)); // Basic Circuit Line
                        DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(startPosition, -10, 0) * ftMM, new XYZ(startPosition + schematic.Bus2.PowerFactorCorrection.Steps * 10, -10, 0) * ftMM)); // Basic Circuit Line

                        for (int d = 0; d < schematic.Bus2.PowerFactorCorrection.Steps; d++)
                        {
                            TempX += 10;
                            FamilyInstance correctionDevice = doc.Create.NewFamilyInstance(new XYZ(TempX - 5, -10, 0) * ftMM, deviceType, draftView); // Correction Device
                            doc.Regenerate();
                            doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(correctionDevice, true));
                        }
#if REVIT2015
                        doc.Create.NewTextNote(draftView, new XYZ(textPosition, schematic.Bus2.PowerFactorCorrection.Type == "SM - Device - Correction Device (with detuning)" ? -30 : -40, 0) * ftMM, XYZ.BasisX, XYZ.BasisY, 30 * ftMM, TextAlignFlags.TEF_ALIGN_CENTER | TextAlignFlags.TEF_ALIGN_TOP, text).TextNoteType = textStyle;
#else
                        TextNote tn2 = TextNote.Create(doc, draftView.Id, new XYZ(textPosition, schematic.Bus2.PowerFactorCorrection.Type == "SM - Device - Correction Device (with detuning)" ? -30 : -40, 0) * ftMM, text, textHorizontal);
                        tn2.HorizontalAlignment = HorizontalTextAlignment.Center;
                        tn2.TextNoteType = textStyle;
#endif
                        TempX += 10;
                        bus2XLife = bus2X;
                        bus2X = TempX > bus2X ? Convert.ToInt32(TempX) : bus2X;
                        t.Commit();
                    }
                }
            }
            else
            {
                bus2X = 2;
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
            if (schematic.Settings.IncludeExternalEarthBar)
            {
                using (Transaction t = new Transaction(doc, "Create External Earth Bar"))
                {
                    t.Start();
                    FamilyInstance earthBar = doc.Create.NewFamilyInstance(new XYZ(bus1X + 10, -117.5, 0) * ftMM, ngbe9012Symbol, draftView); // Earth Bar
                    t.Commit();
                }
            }

            // Draw Internal Earth Bar (if turned on)
            if (schematic.Settings.IncludeInternalEarthBar)
            {
                using (Transaction t = new Transaction(doc, "Create Internal Earth Bar"))
                {
                    t.Start();
                    FamilyInstance earthBar = doc.Create.NewFamilyInstance(new XYZ(bus1X + 10, -102.5, 0) * ftMM, ngbe9012Symbol, draftView); // Earth Bar
                    t.Commit();
                }
            }

            // Draw Life safety Supplies Section
            if (schematic.LifeSafetySection != null)
            {
                using (Transaction trans = new Transaction(doc, "Create Life Safety Supplies Section"))
                {
                    trans.Start();
                    lifeX = bus1X - (20 + (schematic.LifeSafetySection.LsCircuit.Count() * circuitGap));
                    int TempX = lifeX + 20;
                    foreach (LsCircuit lsCircuit in schematic.LifeSafetySection.LsCircuit)
                    {
                        DetailCurve circuitLine = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(TempX, 0, 0) * ftMM, new XYZ(TempX, 100, 0) * ftMM)); // Basic Circuit Line
                        FamilyInstance referenceInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 52.2, 0) * ftMM, ngbe9001Symbol, draftView); // Reference Symbol
                        if (referenceInstance.LookupParameter("schematicReference") != null)
                        {
                            referenceInstance.LookupParameter("schematicReference").Set(schematic.Name.Trim());
                        }
                        if (referenceInstance.LookupParameter("cableType") != null)
                        {
                            referenceInstance.LookupParameter("cableType").Set(lsCircuit.Type);
                        }
                        if (referenceInstance.LookupParameter("cableSize") != null)
                        {
                            referenceInstance.LookupParameter("cableSize").Set(lsCircuit.Size);
                        }
                        if (referenceInstance.LookupParameter("cableReference") != null)
                        {
                            referenceInstance.LookupParameter("cableReference").Set(lsCircuit.Ref);
                        }
                        if (referenceInstance.LookupParameter("cableLength") != null)
                        {
                            referenceInstance.LookupParameter("cableLength").Set(lsCircuit.Length);
                        }
                        if (referenceInstance.LookupParameter("cableDescription") != null)
                        {
                            referenceInstance.LookupParameter("cableDescription").Set(lsCircuit.Description);
                        }
                        FamilyInstance deviceInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 18, 0) * ftMM, ngbe0372Symbol, draftView); // Device Type
                        if (deviceInstance.LookupParameter("Ref") != null)
                        {
                            deviceInstance.LookupParameter("Ref").Set(lsCircuit.Device.Ref);
                        }
                        if (deviceInstance.LookupParameter("Rating") != null)
                        {
                            deviceInstance.LookupParameter("Rating").Set(lsCircuit.Device.Rating);
                        }
                        if (deviceInstance.LookupParameter("Poles") != null)
                        {
                            deviceInstance.LookupParameter("Poles").Set(lsCircuit.Device.Poles);
                        }
                        if (deviceInstance.LookupParameter("Frame") != null)
                        {
                            deviceInstance.LookupParameter("Frame").Set(lsCircuit.Device.Frame);
                        }
                        FamilyInstance connectedLoadInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 100, 0) * ftMM, ngbe9011Symbol, draftView); // Connected Load
#if REVIT2015
                        doc.Create.NewTextNote(draftView, new XYZ(TempX - 3, 118, 0) * ftMM, XYZ.BasisY, -XYZ.BasisX, 50 * ftMM, TextAlignFlags.TEF_ALIGN_LEFT | TextAlignFlags.TEF_ALIGN_MIDDLE, lsCircuit.Description).TextNoteType = textStyle;
#else
                        TextNote.Create(doc, draftView.Id, new XYZ(TempX - 4.7, 118, 0) * ftMM, lsCircuit.Description, textVertical).TextNoteType = textStyle;
#endif
                        doc.Regenerate();
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(deviceInstance));
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(referenceInstance));
                        doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(connectedLoadInstance));
                        if (schematic.Settings.IncludeTerminations)
                        {
                            FamilyInstance terminationInstance = doc.Create.NewFamilyInstance(new XYZ(TempX, 40, 0) * ftMM, ngbe0434Symbol, draftView); // Cable Termination
                            doc.Regenerate();
                            doc.Create.NewAlignment(draftView, circuitLine.GeometryCurve.Reference, rpmBIMUtils.findReference(terminationInstance));
                        }
                        TempX += circuitGap;
                    }
                    doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(lifeX + 10, 0, 0) * ftMM, new XYZ(bus1X - 10, 0, 0) * ftMM)).LineStyle = wideLine;
                    double connectionPoint = lifeX - (lifeX - bus1X) * 0.5;
                    if (schematic.Bus1.SupplyTransformer != null || schematic.Bus1.SupplyGenerator != null || schematic.Bus2.SupplyTransformer != null || schematic.Bus2.SupplyGenerator != null)
                    {
                        int bus1XTemp = bus1XLife != 0 ? bus1XLife : bus1X;
                        int bus2XTemp = bus2XLife != 0 ? bus2XLife : bus2X;
                        double connectionEndPoint = schematic.Bus1.SupplyTransformer != null ? (bus1XTemp + 18) * 0.75 - 8 :
                            schematic.Bus1.SupplyGenerator != null ? (bus1XTemp + 18) * 0.25 - 8 :
                            schematic.Bus2.SupplyTransformer != null ? (bus2XTemp - 18) * 0.25 + 8 : (bus2XTemp - 18) * 0.75 + 8;
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
            if (schematic.Settings.IncludeSheet)
            {
                // Close created draftView
                foreach (UIView uiv in rpmBIMTools.Load.uiApp.ActiveUIDocument.GetOpenUIViews())
                {
                    if (uiv.ViewId == draftView.Id) { uiv.Close(); }
                }

                // Create scheduleView
                if (schematic.Settings.IncludeSchedule)
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
                            if (sf.GetName(doc) == "cableSize") { cableSize = sf; }
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
                        sd.AddFilter(new ScheduleFilter(sfSR.FieldId, ScheduleFilterType.Equal, schematic.Name.Trim()));
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

                // Create Legend
                if (schematic.Notes != null)
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
                    // Add Notes
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
                        foreach (String note in schematic.Notes.Note)
                        {
                            text += note + "\n";
                        }
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
                                                     where type.SheetNumber.Contains("00-XX-SM-E-61")
                                                     select type.SheetNumber;
                    for (int i = 1; i < 100; i++)
                    {
                        if (sheetNames.Contains("00-XX-SM-E-61" + i.ToString("00"))) continue;
                        string username = new String(Environment.UserName.Where(Char.IsLetter).ToArray());
                        username = username.Substring(username.Length - 1, 1) + " " + username.Substring(0, 1) + username.Substring(1, username.Length - 2);
                        trans.Start();
                        ViewSheet viewSheet = ViewSheet.Create(doc, titleBlock.Id);
                        viewSheet.LookupParameter("Sub-Discipline").Set("E61 - Main HV / LV Distribution");
                        viewSheet.LookupParameter("Drawn By").Set(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(username));
                        viewSheet.SheetNumber = "00-XX-SM-E-61" + i.ToString("00");
                        viewSheet.Name = "Main HV / LV Distribution Schematic, " + schematic.Name.Trim() + ", Sheet " + i.ToString();
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

            // Finish Command
            waitingWindow.Close();
            return Result.Succeeded;
        }

        public void insertFaultRelays(double circuitPosition, FamilyInstance bridge)
        {
            if (schematic.Settings.IncludeRestrictedEarthFaultRelay || schematic.Settings.IncludeOvercurrentRelay || schematic.Settings.IncludeEarthFaultRelay)
            {
                DetailCurve circuitLine2 = doc.Create.NewDetailCurve(draftView, Line.CreateBound(new XYZ(circuitPosition, -28, 0) * ftMM, new XYZ(circuitPosition - 6, -28, 0) * ftMM)); // BMS Output Line
                circuitPosition -= 1.6;
                if (schematic.Settings.IncludeRestrictedEarthFaultRelay)
                {
                    circuitPosition -= 4.4;
                    FamilyInstance faultRelay = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe9001Symbol, draftView); // Overcurrent Fault Relay
                    faultRelay.LookupParameter("cableReference").Set("RE");
                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(faultRelay, true));
                }
                if (schematic.Settings.IncludeOvercurrentRelay)
                {
                    circuitPosition -= 4.4;
                    FamilyInstance faultRelay = doc.Create.NewFamilyInstance(new XYZ(circuitPosition, -28, 0) * ftMM, ngbe9001Symbol, draftView); // Earth Fault Relay
                    faultRelay.LookupParameter("cableReference").Set("OC");
                    doc.Regenerate();
                    doc.Create.NewAlignment(draftView, circuitLine2.GeometryCurve.Reference, rpmBIMUtils.findReference(faultRelay, true));
                }
                if (schematic.Settings.IncludeEarthFaultRelay)
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
            if (schematic.Settings.IncludeNeutralEarthLink)
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
            if (schematic.Settings.IncludeCastellInterlock)
            {
                doc.Create.NewFamilyInstance(new XYZ(circuitPosition - 5, -35, 0) * ftMM, ngbe9010Symbol, draftView); // Castell Interlock
            }
        }

        public string interlockText()
        {
            string text = null;
            text += schematic.Settings.IncludeElectricalInterlock && schematic.Settings.IncludeMechanicalInterlock ? "Elec/Mech" :
                schematic.Settings.IncludeElectricalInterlock ? "Electrical" : "Mechanical";
            text += "\nInterlock";
            return text;
        }
    }
}