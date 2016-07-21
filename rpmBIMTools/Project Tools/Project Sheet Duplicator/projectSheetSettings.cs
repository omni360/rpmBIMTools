using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit.UI;

namespace rpmBIMTools
{
    public partial class projectSheetSettings : Form
    {
        public int sheetWarnings = 0;

        public projectSheetSettings()
        {
            InitializeComponent();
        }

        private void helpRequest(object sender, HelpEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Project-Sheet-Duplicator");
        }

        private void helpButtonClick(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Project-Sheet-Duplicator");
            e.Cancel = true;
        }

        private void templateList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            duplicateButton.Enabled = (duplicateButton.Enabled = templateList.CheckedItems.Count == 1 && e.NewValue == CheckState.Unchecked) ||
                sheetTree.Nodes[0].Nodes.Count == sheetWarnings || sheetTree.Nodes[0].Nodes.ContainsKey("noSheets") ? false : true;
            
        }
    }
}
