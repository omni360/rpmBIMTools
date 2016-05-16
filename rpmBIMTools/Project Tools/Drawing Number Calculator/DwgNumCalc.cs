using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

namespace rpmBIMTools
{
    public partial class DwgNumCalc : System.Windows.Forms.Form
    {
        Document doc = rpmBIMTools.Load.liveDoc;
        FamilySymbol titleBlock = null;
        string[] treatment = {
                              "Spare (M10)",
                              "Spare (M11)",
                              "Drainage",
                              "Domestic",
                              "Gas / Compressed Air / Medical Gas",
                              "Chilled Water / Refrigeration",
                              "Heating",
                              "Ventilation",
                              "Spare (M18)",
                              "Fire Engineering",
                              "--------------------",
                              "Main HV Distribution",
                              "Main LV Distribution",
                              "Small Power",
                              "Lighting",
                              "CCTV / TV",
                              "Communications",
                              "Containment",
                              "Fire Alarm",
                              "Security / Access Control",
                              "Lightning Protection / Earthing"
                          };
        string[] schematic = {
                              "Spare (M30)",
                              "Spare (M31)",
                              "Drainage",
                              "Domestic",
                              "Gas / Compressed Air / Medical Gas",
                              "Chilled Water / Refrigeration",
                              "Heating",
                              "Ventilation",
                              "Spare (M38)",
                              "Fire Engineering",
                              "--------------------",
                              "Main HV Distribution",
                              "Main LV Distribution",
                              "Small Power",
                              "Lighting",
                              "CCTV / TV",
                              "Communications",
                              "Containment",
                              "Fire Alarm",
                              "Security / Access Control",
                              "Lightning Protection / Earthing"
                          };
        string[] electrical = {
                              "Main HV Distribution",
                              "Main LV Distribution",
                              "Small Power",
                              "Lighting",
                              "CCTV / TV",
                              "Communications",
                              "Containment",
                              "Fire Alarm",
                              "Security / Access Control",
                              "Lightning Protection / Earthing"
                          };
        string[] mechanical = {
                              "Spare (M50)",
                              "Spare (M51)",
                              "Drainage",
                              "Domestic",
                              "Gas / Compressed Air / Medical Gas",
                              "Chilled Water / Refrigeration",
                              "Heating",
                              "Ventilation",
                              "Spare (M58)",
                              "Fire Engineering"
                          };
        string[] coordinated = {
                                  "Coordinated Layout",
                                  "Coordinated Sections",
                                  "Coordinated Details",
                                  "Spare (M73)",
                                  "Spare (M74)"
                               };
        string[] builderswork = {
                                  "Builderswork Layout",
                                  "Builderswork Sections",
                                  "Builderswork Details",
                                  "Spare (M78)",
                                  "Spare (M79)"
                               };
        string[] detailsSections = {
                                  "General Details & Sections",
                                  "Electrical Details & Sections",
                                  "Drainage",
                                  "Domestic",
                                  "Gas / Compressed Air / Medical Gas",
                                  "Chilled Water / Refrigeration",
                                  "Heating",
                                  "Ventilation",
                                  "Spare (M88)",
                                  "Fire Engineering",
                                  };
        string[] offsiteSpool = {
                                  "Spool Drawings",
                                  "Module Drawings",
                                  "Prefab Plantroom",
                                  "Riser",
                                  "Distribution Boards"
                                };
        string[] master = {
                                  "Master Drawing"
                          };

        public DwgNumCalc()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // Find Revit Project Number and use
            ProjectNumber.Text = string.IsNullOrEmpty(doc.ProjectInformation.Number) ?
                "#####" : string.Concat(doc.ProjectInformation.Number.Take(ProjectNumber.MaxLength));
            // Enable 'Copy To Title Block' button if active view is a sheet and the title block is found
            Autodesk.Revit.DB.View view = doc.ActiveView;
            CopyToSheet.Enabled = view is ViewSheet ? true : false;
            // Set sheet size to default 'A0'
            SheetSize.SelectedIndex = 0;
        }

        private void ZoneType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZoneNum.Enabled = ZoneType.Text == "All Zones" ? false : true;
            ZoneNum.MaxLength = ZoneType.Text == "All Zones" || ZoneType.Text == "Custom" ? 2 : 1;
            ZoneNum.Text = ZoneType.Text == "All Zones" ? "ZZ" :
                ZoneType.Text == "Custom" ? "##" : "1";
            SheetNum.Value = 1;
            Update_DwgNum(sender, e);
        }

        private void LevelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LevelNum.Enabled = LevelType.Text == "General" || LevelType.Text == "Basement" || LevelType.Text == "Mezzanine" ? true : false;
            LevelNum.Minimum = LevelType.Text == "General" ? 0 : 1;
            LevelNum.Maximum = LevelType.Text == "General" ? 99 : 9;
            LevelNum.Value = LevelType.Text == "General" ? 0 : 1;
            SheetNum.Value = 1;
            Update_DwgNum(sender, e);
        }

        private void ResetSheetNumber(object sender, EventArgs e)
        {
            SheetNum.Value = 1;
            SheetNum.Maximum = DrawingType.Text == "Offsite Drawings" ? 999 : 99;
            Update_DwgNum(sender, e);
        }

        private void UpdateServiceTypes(object sender, EventArgs e)
        {
            ServiceType.Items.Clear();
            ServiceType.Items.AddRange(
                DrawingType.Text == "Electrical Services" ? electrical :
                DrawingType.Text == "Mechanical Services" ? mechanical :
                DrawingType.Text == "Coordinated Services" ? coordinated :
                DrawingType.Text == "Builderswork" ? builderswork :
                DrawingType.Text == "Schematic" ? schematic :
                DrawingType.Text == "Treatment Drawing" ? treatment :
                DrawingType.Text == "Offsite Drawings" ? offsiteSpool :
                DrawingType.Text == "Master Drawings" ? master : detailsSections
            );
            ServiceType.SelectedIndex = 0;
        }

        private string Get_DwgNum()
        {
            string DwgNum = "";
            DwgNum += ZoneType.Text == "All Zones" ? "ZZ-" :
                      ZoneType.Text == "Zone" ? "Z" + ZoneNum.Text + "-" :
                      ZoneType.Text == "Riser" ? "R" + ZoneNum.Text + "-" :
                      ZoneType.Text == "Custom" ? ZoneNum.Text + "-" : "##-"; // Zone
            DwgNum += LevelType.Text == "General" ? LevelNum.Value.ToString("00") + "-" :
                      LevelType.Text == "Basement" ? "B" + LevelNum.Value + "-" :
                      LevelType.Text == "Mezzanine" ? "M" + LevelNum.Value + "-" :
                      LevelType.Text == "Multiple Levels" ? "ZZ-" :
                      LevelType.Text == "External" ? "EX-" :
                      LevelType.Text == "Not Applicable" ? "XX-" : "##-"; // Level
            DwgNum += DrawingType.Text == "Mechanical Services" ? "DR-" :
                      DrawingType.Text == "Electrical Services" ? "DR-" :
                      DrawingType.Text == "Coordinated Services" ? "DR-" :
                      DrawingType.Text == "Builderswork" ? "DR-" :
                      DrawingType.Text == "Schematic" ? "DR-" :
                      DrawingType.Text == "Treatment Drawing" ? "DR-" :
                      DrawingType.Text == "Offsite Drawings" ? "DR-" :
                      DrawingType.Text == "Details & Sections" ? "DR-" :
                      DrawingType.Text == "Master Drawings" ? "DR-" : "##-"; // Type
            DwgNum += Get_ServiceCode();
            DwgNum += ServiceType.Text == "Spool Drawings" ? SheetNum.Value.ToString("000") : SheetNum.Value.ToString("00");
            return DwgNum;
        }

        public string Get_ServiceCode()
        {
            return DrawingType.Text == "Offsite Drawings" ?
                      ServiceType.Text == "Spool Drawings" ? "M-9" :
                      ServiceType.Text == "Module Drawings" ? "M-01" :
                      ServiceType.Text == "Prefab Plantroom" ? "M-02" :
                      ServiceType.Text == "Riser" ? "M-03" :
                      ServiceType.Text == "Distribution Boards" ? "M-04" : "#-##" // Offsite Drawings Services
                      : DrawingType.Text == "Treatment Drawing" ?
                      ServiceType.Text == "Spare (M10)" ? "M-10" :
                      ServiceType.Text == "Spare (M11)" ? "M-11" :
                      ServiceType.Text == "Drainage" ? "M-12" :
                      ServiceType.Text == "Domestic" ? "M-13" :
                      ServiceType.Text == "Gas / Compressed Air / Medical Gas" ? "M-14" :
                      ServiceType.Text == "Chilled Water / Refrigeration" ? "M-15" :
                      ServiceType.Text == "Heating" ? "M-16" :
                      ServiceType.Text == "Ventilation" ? "M-17" :
                      ServiceType.Text == "Spare (M18)" ? "M-18" :
                      ServiceType.Text == "Fire Engineering" ? "M-19" :
                      ServiceType.Text == "Main HV Distribution" ? "E-20" :
                      ServiceType.Text == "Main LV Distribution" ? "E-21" :
                      ServiceType.Text == "Small Power" ? "E-22" :
                      ServiceType.Text == "Lighting" ? "E-23" :
                      ServiceType.Text == "CCTV / TV" ? "E-24" :
                      ServiceType.Text == "Communications" ? "E-25" :
                      ServiceType.Text == "Containment" ? "E-26" :
                      ServiceType.Text == "Fire Alarm" ? "E-27" :
                      ServiceType.Text == "Security / Access Control" ? "E-28" :
                      ServiceType.Text == "Lightning Protection / Earthing" ? "E-29" : "#-##" // Treatment Drawings Services
                      : DrawingType.Text == "Schematic" ?
                      ServiceType.Text == "Spare (M30)" ? "M-30" :
                      ServiceType.Text == "Spare (M31)" ? "M-31" :
                      ServiceType.Text == "Drainage" ? "M-32" :
                      ServiceType.Text == "Domestic" ? "M-33" :
                      ServiceType.Text == "Gas / Compressed Air / Medical Gas" ? "M-34" :
                      ServiceType.Text == "Chilled Water / Refrigeration" ? "M-35" :
                      ServiceType.Text == "Heating" ? "M-36" :
                      ServiceType.Text == "Ventilation" ? "M-37" :
                      ServiceType.Text == "Spare (M38)" ? "M-38" :
                      ServiceType.Text == "Fire Engineering" ? "M-39" :
                      ServiceType.Text == "Main HV Distribution" ? "E-40" :
                      ServiceType.Text == "Main LV Distribution" ? "E-41" :
                      ServiceType.Text == "Small Power" ? "E-42" :
                      ServiceType.Text == "Lighting" ? "E-43" :
                      ServiceType.Text == "CCTV / TV" ? "E-44" :
                      ServiceType.Text == "Communications" ? "E-45" :
                      ServiceType.Text == "Containment" ? "E-46" :
                      ServiceType.Text == "Fire Alarm" ? "E-47" :
                      ServiceType.Text == "Security / Access Control" ? "E-48" :
                      ServiceType.Text == "Lightning Protection / Earthing" ? "E-49" : "#-##" // Schematic Services
                      : DrawingType.Text == "Mechanical Services" ?
                      ServiceType.Text == "Spare (M50)" ? "M-50" :
                      ServiceType.Text == "Spare (M51)" ? "M-51" :
                      ServiceType.Text == "Drainage" ? "M-52" :
                      ServiceType.Text == "Domestic" ? "M-53" :
                      ServiceType.Text == "Gas / Compressed Air / Medical Gas" ? "M-54" :
                      ServiceType.Text == "Chilled Water / Refrigeration" ? "M-55" :
                      ServiceType.Text == "Heating" ? "M-56" :
                      ServiceType.Text == "Ventilation" ? "M-57" :
                      ServiceType.Text == "Spare (M58)" ? "M-58" :
                      ServiceType.Text == "Fire Engineering" ? "M-59" : "#-##" // Mechanical Services
                      : DrawingType.Text == "Electrical Services" ?
                      ServiceType.Text == "Main HV Distribution" ? "E-60" :
                      ServiceType.Text == "Main LV Distribution" ? "E-61" :
                      ServiceType.Text == "Small Power" ? "E-62" :
                      ServiceType.Text == "Lighting" ? "E-63" :
                      ServiceType.Text == "CCTV / TV" ? "E-64" :
                      ServiceType.Text == "Communications" ? "E-65" :
                      ServiceType.Text == "Containment" ? "E-66" :
                      ServiceType.Text == "Fire Alarm" ? "E-67" :
                      ServiceType.Text == "Security / Access Control" ? "E-68" :
                      ServiceType.Text == "Lightning Protection / Earthing" ? "E-69" : "#-##" // Electrical Services
                      : DrawingType.Text == "Coordinated Services" ?
                      ServiceType.Text == "Coordinated Layout" ? "M-70" :
                      ServiceType.Text == "Coordinated Sections" ? "M-71" :
                      ServiceType.Text == "Coordinated Details" ? "M-72" :
                      ServiceType.Text == "Spare (M73)" ? "M-73" :
                      ServiceType.Text == "Spare (M74)" ? "M-74" : "#-##" // Coordinated Services
                      : DrawingType.Text == "Builderswork" ?
                      ServiceType.Text == "Builderswork Layout" ? "M-75" :
                      ServiceType.Text == "Builderswork Sections" ? "M-76" :
                      ServiceType.Text == "Builderswork Details" ? "M-77" :
                      ServiceType.Text == "Spare (M78)" ? "M-78" :
                      ServiceType.Text == "Spare (M79)" ? "M-79" : "#-##" // Builderswork Services
                      : DrawingType.Text == "Details & Sections" ?
                      ServiceType.Text == "General Details & Sections" ? "M-80" :
                      ServiceType.Text == "Electrical Details & Sections" ? "E-81" :
                      ServiceType.Text == "Drainage" ? "M-82" :
                      ServiceType.Text == "Domestic" ? "M-83" :
                      ServiceType.Text == "Gas / Compressed Air / Medical Gas" ? "M-84" :
                      ServiceType.Text == "Chilled Water / Refrigeration" ? "M-85" :
                      ServiceType.Text == "Heating" ? "M-86" :
                      ServiceType.Text == "Ventilation" ? "M-87" :
                      ServiceType.Text == "Spare (M88)" ? "M-88" :
                      ServiceType.Text == "Fire Engineering" ? "M-89" : "#-##" // Details & Sections Services
                      : "#-##";
        }

        public string Get_Title()
        {
            string title = "";
            title += DrawingType.Text + ", "; // Drawing Type
            title += ServiceType.Text + ", "; // Service
            title += LevelType.Text == "General" ? "Level " + LevelNum.Value + ", " :
                LevelType.Text == "Basement" ? "Basement " + LevelNum.Value + ", " :
                LevelType.Text == "Mezzanine" ? "Mezzanine " + LevelNum.Value + ", " :
                LevelType.Text == "Multiple Levels" ? LevelType.Text + ", " :
                LevelType.Text == "External" ? LevelType.Text + ", " :
                LevelType.Text == "Not Applicable" ? "" : ""; // Level
            title += ZoneType.Text == "All Zones" ? ZoneType.Text + ", " :
                ZoneType.Text == "Zone" || ZoneType.Text == "Custom" ? "Zone " + ZoneNum.Text + ", " :
                ZoneType.Text == "Riser" ? "Riser " + ZoneNum.Text + ", " : ""; // Zone
            title += "Sheet " + SheetNum.Value;
            return title;
        }

        private void Update_DwgNum(object sender, EventArgs e)
        {
            string DwgNum = "";
            DwgNum += ProjectNumber.Text + "-"; // Project Number
            DwgNum += "NGB-"; // Originator
            DwgNum += Get_DwgNum(); // Get rest of drawing number
            this.DwgNum.Text = DwgNum; // Sets final drawing number generated
        }

        private bool FormValidation()
        {
            // Form Validation
            return string.IsNullOrEmpty(ProjectNumber.Text) ||
                string.IsNullOrEmpty(ZoneType.Text) ||
                string.IsNullOrWhiteSpace(ZoneNum.Text) ||
                string.IsNullOrEmpty(LevelType.Text) ||
                string.IsNullOrEmpty(DrawingType.Text) ||
                string.IsNullOrEmpty(ServiceType.Text) ||
                ServiceType.Text == "--------------------" ? false : true;
        }

        private void ZoomToTitleBlock()
        {
            // Zoom to title block information on view creation event
            UIView uiview = null;
            IList<UIView> uiviews = rpmBIMTools.Load.uiApp.ActiveUIDocument.GetOpenUIViews();
            foreach (UIView view in uiviews)
            {
                if (view.ViewId.Equals(doc.ActiveView.Id))
                {
                    uiview = view;
                    break;
                }
            }
            rpmBIMTools.Load.uiApp.ActiveUIDocument.RefreshActiveView();
            uiview.ZoomAndCenterRectangle(new XYZ(-0.43, 0.07, 0), new XYZ(0.01, 0.10, 0));
        }

        private void CopyToClipboard_Click(object sender, EventArgs e)
        {
            if (FormValidation())
            {
                Clipboard.SetText(DwgNum.Text);
                Close();
            }
            else
            {
                TaskDialog.Show("Copy To Sheet", "Please ensure all fields have been selected.");
            }
        }

        private void CopyToNewSheet_Click(object sender, EventArgs e)
        {
            if (FormValidation())
            {

                titleBlock = rpmBIMUtils.findNGBTitleBlock(doc, SheetSize.Text);
                if (titleBlock == null)
                {
                    TaskDialog.Show("Create New Sheet", "No " + SheetSize.Text + " size NGB title block family found in this project");
                    return;
                }

                // Find if sheet already exists
                IEnumerable<ViewSheet> sheetNames = from elem in new FilteredElementCollector(doc)
                                                        .OfClass(typeof(ViewSheet))
                                                        .OfCategory(BuiltInCategory.OST_Sheets)
                                                    let type = elem as ViewSheet
                                                    where type.SheetNumber.Contains(Get_DwgNum())
                                                    select type;
                try
                {
                    using (Transaction t = new Transaction(doc, "Create New Sheet"))
                    {
                        t.Start();
                        ViewSheet viewSheet = ViewSheet.Create(doc, titleBlock.Id);
                        Update_Titleblock(viewSheet);
                        t.Commit();
                        rpmBIMTools.Load.uiApp.ActiveUIDocument.ActiveView = viewSheet;
                        ZoomToTitleBlock();
                        CopyToSheet.Enabled = true;
                    }
                }
                catch (Autodesk.Revit.Exceptions.ArgumentException)
                {
                    TaskDialog.Show("Create New Sheet", "A sheet with that number already exists");
                    return;
                }
                catch (System.InvalidOperationException)
                {
                    TaskDialog.Show("Create New Sheet", "No NGB title block family found in this project");
                    return;
                }
            }
            else
            {
                TaskDialog.Show("Create New Sheet", "Please ensure all fields have been selected.");
            }
        }

        private void CopyToSheet_Click(object sender, EventArgs e)
        {
            if (FormValidation())
            {
                // Find if sheet already exists
                IEnumerable<ViewSheet> sheetNames = from elem in new FilteredElementCollector(doc)
                                                           .OfClass(typeof(ViewSheet))
                                                           .OfCategory(BuiltInCategory.OST_Sheets)
                                                    let type = elem as ViewSheet
                                                    where type.SheetNumber.Contains(Get_DwgNum())
                                                    select type;
                if (sheetNames.Count() > 0)
                {
                    TaskDialog.Show("Copy To Sheet", "A sheet with that number already exists");
                    return;
                }
                using (Transaction t = new Transaction(doc, "Set Drawing Number"))
                {
                    t.Start();
                    Autodesk.Revit.DB.ViewSheet viewSheet = doc.ActiveView as ViewSheet;
                    Update_Titleblock(viewSheet);
                    t.Commit();
                    ZoomToTitleBlock();
                }
            }
            else
            {
                TaskDialog.Show("Copy To Sheet", "Please ensure all fields have been selected.");
            }
        }

        public void Update_Titleblock(ViewSheet viewSheet)
        {
            string username = new String(Environment.UserName.Where(Char.IsLetter).ToArray());
            username = username.ToUpper().Substring(username.Length - 1, 1) + " " + username.Substring(0, 1) + username.Substring(1, username.Length - 2);
            viewSheet.SheetNumber = Get_DwgNum();
            viewSheet.Name = Get_Title();
            viewSheet.get_Parameter(BuiltInParameter.SHEET_ISSUE_DATE).Set(DateTime.Today.ToString("dd/MM/yy"));
            if (viewSheet.LookupParameter("STATUS") != null)
                viewSheet.LookupParameter("STATUS").Set("S3 - For Review & Comment");
            if (viewSheet.LookupParameter("Sub-Discipline") != null)
                viewSheet.LookupParameter("Sub-Discipline").Set(Get_ServiceCode().Replace("-", "") + " - " + ServiceType.Text);
            if (viewSheet.LookupParameter("Drawn By") != null)
                viewSheet.LookupParameter("Drawn By").Set(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(username));
        }
    }
}