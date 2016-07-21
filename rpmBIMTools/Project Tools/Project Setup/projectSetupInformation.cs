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
    public partial class projectSetupInformation : Form
    {
        public projectSetupInformation()
        {
            InitializeComponent();
        }

        private void helpRequest(object sender, HelpEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Project-Setup");
        }

        private void helpButtonClick(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Project-Setup");
            e.Cancel = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            TaskDialog cancelWarning = new TaskDialog("Project Setup - Cancel Process?");
            cancelWarning.MainInstruction = "Are you sure you want to exit the project setup process?";
            cancelWarning.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
            if (cancelWarning.Show() == TaskDialogResult.Yes)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
