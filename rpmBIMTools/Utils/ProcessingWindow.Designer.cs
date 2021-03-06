﻿namespace rpmBIMTools
{
    partial class ProcessingWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.windowText = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // windowText
            // 
            this.windowText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.windowText.Location = new System.Drawing.Point(32, 10);
            this.windowText.Name = "windowText";
            this.windowText.Size = new System.Drawing.Size(320, 55);
            this.windowText.TabIndex = 0;
            this.windowText.Text = "Window Text";
            this.windowText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(32, 73);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(320, 23);
            this.progressBar.TabIndex = 1;
            // 
            // ProcessingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(400, 150);
            this.ControlBox = false;
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.windowText);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProcessingWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ProcessingWindow";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label windowText;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}