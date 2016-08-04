using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rpmBIMTools
{
    public partial class symbolLibrarySelection : Form
    {
        public symbolLibrarySelection()
        {
            InitializeComponent();
            symbolPanel.Select();
        }

        private void helpRequest(object sender, HelpEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Family-Library#family-insertion-process");
        }

        private void helpButtonClick(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Family-Library#family-insertion-process");
            e.Cancel = true;
        }
    }
}
