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

using Autodesk.Revit.UI;

namespace rpmBIMTools
{
    public partial class Rename : Form
    {
        public Rename(string name)
        {
            InitializeComponent();
            previousText.Text = name;
            newText.Text = name;
            newText.Select();
            newText.SelectAll();

        }
        
        private void Filter_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (System.Windows.Forms.TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            Regex reg = new Regex("[^0-9a-zA-Z -]");
            int matches = reg.Matches(textboxSender.Text).Count;
            textboxSender.Text = reg.Replace(textboxSender.Text, "");
            textboxSender.SelectionStart = (cursorPosition - matches) < 0 ? 0 : cursorPosition - matches;
            confirmButton.Enabled = previousText.Text != textboxSender.Text && !string.IsNullOrWhiteSpace(textboxSender.Text);
        }
    }
}
