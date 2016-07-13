using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Win32;

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;

using FolderSelect;
using rpmBIMTools.Create;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace rpmBIMTools
{
    public partial class exportImportSchedules : System.Windows.Forms.Form
    {
        Document doc = rpmBIMTools.Load.liveDoc;
        UIApplication uiApp = rpmBIMTools.Load.uiApp;
        FolderSelectDialog exportFolderDialog = new FolderSelectDialog();
        int rIndex, cIndex;

        public exportImportSchedules()
        {
            InitializeComponent();
        }

        private void exportImportSchedules_Load(object sender, EventArgs e)
        {
            // Schedule Views Collection
            ICollection<ViewSchedule> scheduleViews = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSchedule))
                .Cast<ViewSchedule>()
                .Where(vs => !vs.IsTitleblockRevisionSchedule)
                .ToList();

            // Create Export Schedule List 
            exportScheduleList.DisplayMember = "Name";
            exportScheduleList.ValueMember = "Id";
            foreach (ViewSchedule sv in scheduleViews)
            {
                exportScheduleList.Items.Add(new ElementItem { Name = sv.ViewName, Id = sv.Id }, false);
            }

            // Export Folder Select Default Settings
            exportFolderDialog.Title = "Select Export Folder";
            exportFolderDialog.InitialDirectory = Properties.Settings.Default.exportImportSchedule_OutputFolder;
            exportFolder.Text = Properties.Settings.Default.exportImportSchedule_OutputFolder;
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            exportButton.Visible = tabControl.SelectedIndex == 0;
            importButton.Visible = tabControl.SelectedIndex == 1;
        }

        private void exportScheduleListToggle_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < exportScheduleList.Items.Count; i++)
            {
                exportScheduleList.SetItemChecked(i, exportScheduleListToggle.Checked);
            }
            exportScheduleList_SelectedIndexChanged(null, null);
        }

        private void importScheduleListToggle_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < importScheduleList.Items.Count; i++)
            {
                importScheduleList.SetItemChecked(i, importScheduleListToggle.Checked);
            }
            importScheduleList_SelectedIndexChanged(null, null);
        }

        private void exportScheduleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            exportButton.Enabled = exportScheduleList.CheckedItems.Count > 0 && !string.IsNullOrWhiteSpace(exportFolder.Text);
        }

        private void importScheduleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            importButton.Enabled = importScheduleList.CheckedItems.Count > 0 && !string.IsNullOrWhiteSpace(importFile.Text);
        }

        private void exportFolderButton_Click(object sender, EventArgs e)
        {
            if (exportFolderDialog.ShowDialog())
            {
                exportFolder.Text = exportFolderDialog.FileName;
                exportFolderDialog.InitialDirectory = exportFolderDialog.FileName;
                Properties.Settings.Default.exportImportSchedule_OutputFolder = exportFolder.Text;
                Properties.Settings.Default.Save();
            }

        }

        private void importFileButton_Click(object sender, EventArgs e)
        {
            if (importFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(importFileDialog.FileName);
                if (file.IsFileLocked())
                {
                    TaskDialog.Show("Import Schedules", "Input file is read only or currently open by another application");
                    return;
                }
                importFile.Text = importFileDialog.SafeFileName;
                ExcelPackage excel = new ExcelPackage(file);
                importScheduleList.Items.Clear();
                importScheduleList.DisplayMember = "Name";
                importScheduleList.ValueMember = "Id";
                importScheduleListToggle.Checked = false;
                foreach (ExcelWorksheet ws in excel.Workbook.Worksheets)
                {
                    ViewSchedule vs = doc.GetElement(ws.Cells[1, 1].Text) as ViewSchedule;
                    if (vs != null)
                        importScheduleList.Items.Add(new ReferenceItem { Name = vs.ViewName, Index = ws.Index }, false);
                }
            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {

            // Checks before exporting
            if (!Directory.Exists(exportFolder.Text))
            {
                TaskDialog.Show("Export / Import Schedules", "Output folder does not exist");
                return;
            }

            // Cleaning prefix before FileInfo checks
            string prefix = exportPrefix.Text.Trim();
            prefix = rpmBIMUtils.GetSafeFilename(prefix);
            if (prefix.Length > 0) prefix += '_';

            // File Overwrite Counter
            int fileExistCounter = 0;

            // Setting Exporting schedules Details
            ExcelPackage excel = new ExcelPackage();
            excel.File = new FileInfo(exportFolder.Text + @"\" + prefix + Path.GetFileNameWithoutExtension(doc.Title) + ".xlsx");
            excel.Compression = CompressionLevel.BestCompression;

            // Check if file(s) are locked
            if (exportMethodSingle.Checked)
            {
                if (excel.File.Exists)
                {
                    fileExistCounter++;
                    if (excel.File.IsFileLocked())
                    {
                        TaskDialog.Show("Export Schedules", "Output file is read only or currently open by another application");
                        return;
                    }
                }
            }
            else
            {
                foreach (ElementItem CheckedItem in exportScheduleList.CheckedItems)
                {
                    ViewSchedule schedule = doc.GetElement(CheckedItem.Id) as ViewSchedule;
                    FileInfo file = new FileInfo(exportFolder.Text + @"\" + prefix + Path.GetFileNameWithoutExtension(schedule.ViewName) + ".xlsx");
                    if (file.Exists)
                    {
                        fileExistCounter++;
                        if (file.IsFileLocked())
                        {
                            TaskDialog.Show("Export Schedules", "Output file is read only or currently open by another application");
                            return;
                        }
                    }
                }
            }

            // Show Overwrite Prompt if one or more files being exported already exist
            if (fileExistCounter > 0) {
                TaskDialog overWritePrompt = new TaskDialog("Overwrite export files");
                overWritePrompt.MainInstruction = "Export file(s) alrady exist";
                overWritePrompt.MainContent = "Export folder has " + fileExistCounter + " file(s) with conflicting names";
                overWritePrompt.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Overwrite the existing file(s)");
                overWritePrompt.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Cancel the export process");
                if (overWritePrompt.Show() != TaskDialogResult.CommandLink1 ) return;
            }

            Hide();
            Close();

            // Start ProcessingWindow
            ProcessingWindow pWindow = new ProcessingWindow("Exporting Schedules", "Starting Export Process", true);
            pWindow.Location = new System.Drawing.Point(
                uiApp.MainWindowExtents.Left + ((uiApp.MainWindowExtents.Right - uiApp.MainWindowExtents.Left) - pWindow.Width) / 2,
                uiApp.MainWindowExtents.Top + ((uiApp.MainWindowExtents.Bottom - uiApp.MainWindowExtents.Top) - pWindow.Height) / 2
                );
            pWindow.Show();
            int counter = 1;
             
            foreach (ElementItem CheckedItem in exportScheduleList.CheckedItems)
            {
                // Get ViewSchedule Data 
                ViewSchedule schedule = doc.GetElement(CheckedItem.Id) as ViewSchedule;

                // Update ProcessingWindow
                int prc = counter++ * 100 / exportScheduleList.CheckedItems.Count;
                pWindow.Update(prc, "Exporting Schedule:\n\n" + schedule.ViewName);

                // Multi-File Support - Start
                if (exportMethodMultiple.Checked)
                {
                    excel = new ExcelPackage();
                    excel.File = new FileInfo(exportFolder.Text + @"\" + prefix + Path.GetFileNameWithoutExtension(schedule.ViewName) + ".xlsx");
                    excel.Compression = CompressionLevel.BestCompression;
                }

                // Create Workbook
                string wSheetName;
                if (schedule.ViewName.Length <= 31)
                {
                    wSheetName = schedule.ViewName;
                }
                else
                {
                    int count = excel.Workbook.Worksheets.Count(ews => ews.Name.StartsWith(schedule.ViewName.Substring(0, 27))) + 1;
                        wSheetName = schedule.ViewName.Substring(0, 27) + " " + count.ToString("000");
                }
                
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add(wSheetName);

                // Protect workbook
                ws.Protection.IsProtected = true;

                // Store GUID of schedule
                ws.Cells[1, 1].Value = schedule.UniqueId;

                // Set Header Row(s)
                ws.Cells[2, 1].Value = "GUID"; // GUID Default
                cIndex = 2;
                ScheduleDefinition sd = schedule.Definition;
                foreach (ScheduleFieldId sfId in sd.GetFieldOrder())
                {
                    ScheduleField sf = sd.GetField(sfId);
                    if (sf.FieldType == ScheduleFieldType.Formula) continue; // Skip formula fields
                    ws.Column(cIndex).Width = sf.SheetColumnWidth * 250;
                    ws.Column(cIndex).Hidden = sf.IsHidden;
                    ws.Column(cIndex).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Column(cIndex).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[1, cIndex].Value = sf.ParameterId.IntegerValue;
                    ws.Cells[2, cIndex].Value = sf.GetName();
                    cIndex++;
                }

                // Collect schedule elements and order by
                List<Element> elements = new FilteredElementCollector(doc, schedule.Id).ToList();
                IOrderedEnumerable<Element> sortedElements = null;
                if (elements.Count != 0)
                {
                    foreach (ScheduleSortGroupField sortField in sd.GetSortGroupFields())
                    {
                        ScheduleField sf = sd.GetField(sortField.FieldId);
                        if (sf.FieldType == ScheduleFieldType.Formula) continue; // Skip formula fields
                        if (sortedElements == null)
                            sortedElements = sortField.SortOrder == ScheduleSortOrder.Ascending ?
                                elements.OrderBy(elem => Convert.ToString(elem.GetScheduleParameter(sf).AsBaseValue()), new NaturalSortComparer()) :
                                elements.OrderByDescending(elem => Convert.ToString(elem.GetScheduleParameter(sf).AsBaseValue()), new NaturalSortComparer());
                        else
                            sortedElements = sortField.SortOrder == ScheduleSortOrder.Ascending ?
                                sortedElements.ThenBy(elem => Convert.ToString(elem.GetScheduleParameter(sf).AsBaseValue()), new NaturalSortComparer()) :
                                sortedElements.ThenByDescending(elem => Convert.ToString(elem.GetScheduleParameter(sf).AsBaseValue()), new NaturalSortComparer());
                    }
                    
                    // No ordering found, returning original order
                    if (sortedElements == null)
                        sortedElements = elements.OrderBy(a => 1);

                    // Set Element & Parameter Values
                    rIndex = 3;
                    foreach (Element elem in sortedElements)
                    {
                        cIndex = 2;
                        ws.Cells[rIndex, 1].Value = elem.UniqueId;
                        foreach (ScheduleFieldId sfId in sd.GetFieldOrder())
                        {
                            ScheduleField sf = sd.GetField(sfId);
                            if (sf.FieldType == ScheduleFieldType.Formula) continue; // Skip formula fields
                            Parameter p = elem.GetScheduleParameter(sf);
                            if (p != null)
                            {
                                if (rIndex == 3)
                                {
                                    // Lock ElementID Fields from editing
                                    if (p.StorageType == StorageType.ElementId || p.IsReadOnly)
                                    {
                                        ws.Column(cIndex).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Column(cIndex).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                        ws.Column(cIndex).Style.Locked = true;
                                    }
                                    else
                                    {
                                        ws.Column(cIndex).Style.Locked = false;
                                    }
                                }
                                ws.Cells[rIndex, cIndex].Value = p.AsBaseValue();
                            }
                            else
                            {
                                ws.Column(cIndex).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Column(cIndex).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                ws.Column(cIndex).Style.Locked = true;
                            }
                            cIndex++;
                        }
                        rIndex++;
                    }
                }

                // Excel Grouping for fields that require sorting
                if (!sd.IsItemized)
                {
                    IList<ScheduleField> groupFields = 
                        sd.GetSortGroupFields()
                        .Select(f => sd.GetField(f.FieldId))
                        .Where(f => f.FieldType != ScheduleFieldType.Formula)
                        .ToList();
                    ScheduleField last = groupFields.Last();
                    foreach (ScheduleField sf in groupFields)
                    { // Columns that require grouping
                        int sortRow = 3;
                        int sortCount = 0;
                        int col = 0;

                        // Find column index location
                        for (int colIndex = 2; colIndex <= ws.Dimension.End.Column; colIndex++)
                        {
                            if (sf.GetName() == ws.Cells[2, colIndex].Text)
                                col = colIndex;
                        }

                        // Fill each cel
                        for (int row = 3; row <= ws.Dimension.End.Row + sortCount + 1; row++)
                        { // Each row in columns
                            if (ws.Cells[row, col].Text != ws.Cells[sortRow, col].Text)
                            {
                                if (row - sortRow != -1)
                                {
                                    ws.InsertRow(row, 1);
                                    ws.Row(row).Style.Border.Left.Style = ExcelBorderStyle.None;
                                    ws.Row(row).Style.Border.Right.Style = ExcelBorderStyle.None;
                                    ws.Row(row).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    ws.Row(row).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    ws.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Row(row).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                                    ws.Row(row).Style.Locked = true;
                                    ws.Cells[row, col].Value = ws.Cells[sortRow, col].Text + " : " + (row - sortRow);
                                    // Add any length calculations for last grouping field
                                    if (sf == last) {
                                        for (int colIndex = 2; colIndex <= ws.Dimension.End.Column; colIndex++)
                                        {
                                            if (ws.Cells[row, colIndex].Text == string.Empty && ws.Cells[row - 1, colIndex].Value is double)
                                            {
                                                ws.Cells[row, colIndex].Formula = "SUM(" + ws.Cells[sortRow, colIndex, row - 1, colIndex].Address + ")";
                                                ws.Cells[row, colIndex].Style.Numberformat.Format = "0";
                                            }
                                                
                                        }
                                    }
                                        sortRow = row + 1;
                                    while (ws.Row(sortRow).Style.Fill.PatternType == ExcelFillStyle.Solid)
                                    {
                                        sortRow++;
                                        sortCount++;
                                    }
                                    sortCount++;
                                    row++;
                                    if (ws.Cells[row, 1].Text != string.Empty)
                                    {
                                        ws.Row(sortRow).OutlineLevel = 1;
                                        ws.Row(sortRow).Collapsed = true;
                                    }
                                }
                            }
                            else
                            {
                                if (ws.Cells[row, 1].Text != string.Empty)
                                {
                                    ws.Row(row).OutlineLevel = 1;
                                    ws.Row(row).Collapsed = true;
                                }
                            }
                        }
                    }
                }

                // Style Header
                ws.Row(2).Height = 30;
                ws.Row(2).Style.Font.Bold = true;
                ws.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Row(2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Row(2).Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                ws.Row(2).Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Row(2).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSteelBlue);
                ws.Row(2).Style.Locked = true;

                // Hide CodeFu in the Worksheet
                ws.Row(1).Hidden = true;
                ws.Column(1).Hidden = true;

                // Multi-File Support - End
                if (exportMethodMultiple.Checked)
                    excel.Save();
            }

            // Save single file version
            if (exportMethodSingle.Checked)
                excel.Save();

            pWindow.Hide();
            pWindow.Close();
            pWindow.Dispose();

            TaskDialog.Show("Export Schedules", "Export Process has been completed.");
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            Hide();
            Close();

            // Start ProcessingWindow
            ProcessingWindow pWindow = new ProcessingWindow("Importing Schedules", "Starting Import Process", true);
            pWindow.Location = new System.Drawing.Point(
                uiApp.MainWindowExtents.Left + ((uiApp.MainWindowExtents.Right - uiApp.MainWindowExtents.Left) - pWindow.Width) / 2,
                uiApp.MainWindowExtents.Top + ((uiApp.MainWindowExtents.Bottom - uiApp.MainWindowExtents.Top) - pWindow.Height) / 2
                );
            pWindow.Show();
            int counter = 1;

            // Update ProcessingWindow
            foreach (ReferenceItem CheckedItem in importScheduleList.CheckedItems)
            {
                int prc = counter++ * 100 / importScheduleList.CheckedItems.Count;
                ExcelPackage excel = new ExcelPackage(new FileInfo(importFileDialog.FileName));
                ExcelWorksheet ws = excel.Workbook.Worksheets[CheckedItem.Index];
                if (ws == null) continue;
                ViewSchedule vs = doc.GetElement(ws.Cells[1, 1].Text) as ViewSchedule;
                if (vs == null) continue;
                pWindow.Update(prc, "Updating elements in Schedule:\n\n" + vs.ViewName);
                ICollection<Element> elements = new FilteredElementCollector(doc, vs.Id).ToElements();
                using (Transaction t = new Transaction(doc, "Importing Schedule Data"))
                {
                    t.Start();
                    for (rIndex = 3; rIndex <= ws.Dimension.End.Row; rIndex++)
                    { // Each Element
                        Element elem = elements.FirstOrDefault(el => el.UniqueId == ws.Cells[rIndex, 1].Text);
                        if (elem == null) continue;
                        for (cIndex = 2; cIndex <= ws.Dimension.End.Column; cIndex++)
                        { // Each Parameter of Each Element
                            Parameter p = elem.GetScheduleParameter(Convert.ToInt32(ws.Cells[1, cIndex].Value), ws.Cells[2, cIndex].Text);
                            if (p == null) continue;
                            p.SetBaseValue(ws.Cells[rIndex, cIndex].Value);

                        }
                    }
                    t.Commit();
                }
            }
            pWindow.Hide();
            pWindow.Close();
            pWindow.Dispose();

            TaskDialog.Show("Import Schedules", "Impoer Process has been completed.");
        }

        internal class NaturalSortComparer : IComparer<string>
        {
            private bool isAscending;

            /// <summary>
            /// Creates a new instance of <see cref="NaturalSortComparer"/>.
            /// </summary>
            /// <param name="inAscendingOrder">Sorts in ascending order, otherwise descending.</param>
            public NaturalSortComparer(bool inAscendingOrder = true)
            {
                this.isAscending = inAscendingOrder;
            }

#region IComparer<string> Members

            /// <summary>
            /// Compares two strings.
            /// </summary>
            /// <param name="x">First string.</param>
            /// <param name="y">Second string.</param>
            /// <returns>0 - the same objects, -1, +1 otherwise depending whether the first string precedes the second one or not.</returns>
            public int Compare(string x, string y)
            {
                throw new NotImplementedException();
            }

#endregion

#region IComparer<string> Members

            int IComparer<string>.Compare(string x, string y)
            {
                if (x == y)
                    return 0;

                if (x == null || y == null)
                    return string.Compare(x, y);

                string[] x1, y1;

                if (!table.TryGetValue(x, out x1))
                {
                    x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                    table.Add(x, x1);
                }

                if (!table.TryGetValue(y, out y1))
                {
                    y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
                    table.Add(y, y1);
                }

                int returnVal;

                for (int i = 0; i < x1.Length && i < y1.Length; i++)
                {
                    if (x1[i] != y1[i])
                    {
                        returnVal = PartCompare(x1[i], y1[i]);
                        return isAscending ? returnVal : -returnVal;
                    }
                }

                if (y1.Length > x1.Length)
                {
                    returnVal = 1;
                }
                else if (x1.Length > y1.Length)
                {
                    returnVal = -1;
                }
                else
                {
                    returnVal = 0;
                }

                return isAscending ? returnVal : -returnVal;
            }

            private static int PartCompare(string left, string right)
            {
                int x, y;
                if (!int.TryParse(left, out x))
                    return left.CompareTo(right);

                if (!int.TryParse(right, out y))
                    return left.CompareTo(right);

                return x.CompareTo(y);
            }

#endregion

            private Dictionary<string, string[]> table = new Dictionary<string, string[]>();
        }
    }
}