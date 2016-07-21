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
    public partial class projectSetupFiles : Form
    {
        public projectSetupFiles()
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

        private void filePathArchitectSelect_Click(object sender, EventArgs e)
        {
            if (openFileArchitect.ShowDialog() == DialogResult.OK)
            {
                filePathArchitect.Text = openFileArchitect.InitialDirectory + openFileArchitect.FileName;
            }
        }

        private void filePathAdditionalSelect_Click(object sender, EventArgs e)
        {

        }

        private void filePathOutputSelect_Click(object sender, EventArgs e)
        {
            if (saveFileProject.ShowDialog() == DialogResult.OK)
            {
                filePathOutput.Text = saveFileProject.InitialDirectory + saveFileProject.FileName;
            }
        }

        private void fileChecker(object sender, CancelEventArgs e)
        {
            setupButton.Enabled = 
                openFileArchitect.FileName != string.Empty &&
                saveFileProject.FileName != string.Empty ? true : false;
        }

        private void filePathAdditionalSelect1_Click(object sender, EventArgs e)
        {
            if (openFileAdditional1.ShowDialog() == DialogResult.OK)
            {
                filePathAdditional1.Text = string.Empty;
                foreach (string s in openFileAdditional1.SafeFileNames)
                    filePathAdditional1.Text += '"' + s + '"' + ' ';
            }
        }

        private void filePathAdditionalSelect2_Click(object sender, EventArgs e)
        {
            if (openFileAdditional2.ShowDialog() == DialogResult.OK)
            {
                filePathAdditional2.Text = string.Empty;
                foreach (string s in openFileAdditional2.SafeFileNames)
                    filePathAdditional2.Text += '"' + s + '"' + ' ';
            }
        }

        private void filePathAdditionalSelect3_Click(object sender, EventArgs e)
        {
            if (openFileAdditional3.ShowDialog() == DialogResult.OK)
            {
                filePathAdditional3.Text = string.Empty;
                foreach (string s in openFileAdditional3.SafeFileNames)
                    filePathAdditional3.Text += '"' + s + '"' + ' ';
            }
        }

        private void filePathAdditionalSelect4_Click(object sender, EventArgs e)
        {
            if (openFileAdditional4.ShowDialog() == DialogResult.OK)
            {
                filePathAdditional4.Text = string.Empty;
                foreach (string s in openFileAdditional4.SafeFileNames)
                    filePathAdditional4.Text += '"' + s + '"' + ' ';
            }
        }

        private void filePathAdditionalSelect5_Click(object sender, EventArgs e)
        {
            if (openFileAdditional5.ShowDialog() == DialogResult.OK)
            {
                filePathAdditional5.Text = string.Empty;
                foreach (string s in openFileAdditional5.SafeFileNames)
                    filePathAdditional5.Text += '"' + s + '"' + ' ';
            }
        }
    }
}