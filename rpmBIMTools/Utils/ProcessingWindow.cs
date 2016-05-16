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
    /// <summary>
    /// This class contains options available for creating a process window.
    /// </summary>
    public partial class ProcessingWindow : Form
    {
        /// <summary>
        /// Creates a new processing window.
        /// </summary>
        /// <param name="title">Processing window title.</param>
        /// <param name="messageText">Processing window message text.</param>
        /// <param name="showBar">Progress bar enabled.</param>
        public ProcessingWindow(string title, string messageText, bool showBar)
        {
            InitializeComponent();
            this.Text = title;
            windowText.Text = messageText;
            progressBar.Enabled = showBar;
            progressBar.Visible = showBar;
        }
        /// <summary>
        /// Updates the processing window percent bar.
        /// </summary>
        /// <param name="percent">Percent on progress bar, value can be from 0 to 100</param>
        public void Update(int percent)
        {
            progressBar.Value = percent < 0 ? 0 : percent > 100 ? 100 : percent;
            System.Windows.Forms.Application.DoEvents();
        }
        /// <summary>
        /// Updates the processing window percent bar and message text.
        /// </summary>
        /// <param name="percent">Percent on progress bar, value can be from 0 to 100</param>
        /// <param name="messageText">Processing window message text.</param>
        public void Update(int percent, string messageText) {
            progressBar.Value = percent < 0 ? 0 : percent > 100 ? 100 : percent;
            windowText.Text = messageText;
            System.Windows.Forms.Application.DoEvents();
        }

    }
}