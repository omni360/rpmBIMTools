﻿using System;
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
    public partial class generateDialog : Form
    {
        public generateDialog()
        {
            InitializeComponent();
        }

        private void helpRequest(object sender, HelpEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Generate-GUID");
        }

        private void helpButtonClick(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mcox86/rpmBIMTools/wiki/Generate-GUID");
            e.Cancel = true;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            GUIDText.Text = allCaps.Checked ? Guid.NewGuid().ToString("D").ToUpper() : Guid.NewGuid().ToString("D");
            GUIDText.Focus();
            GUIDText.SelectAll();
        }
    }
}
