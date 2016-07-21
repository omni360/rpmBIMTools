namespace rpmBIMTools
{
    partial class symbolLibrarySelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(symbolLibrarySelection));
            this.symbolGroup = new System.Windows.Forms.GroupBox();
            this.symbolPanel = new System.Windows.Forms.Panel();
            this.symbolGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // symbolGroup
            // 
            this.symbolGroup.AutoSize = true;
            this.symbolGroup.Controls.Add(this.symbolPanel);
            this.symbolGroup.Location = new System.Drawing.Point(12, 12);
            this.symbolGroup.Name = "symbolGroup";
            this.symbolGroup.Size = new System.Drawing.Size(266, 82);
            this.symbolGroup.TabIndex = 0;
            this.symbolGroup.TabStop = false;
            this.symbolGroup.Text = "Select Family Symbol To Insert";
            // 
            // symbolPanel
            // 
            this.symbolPanel.AutoScroll = true;
            this.symbolPanel.AutoSize = true;
            this.symbolPanel.Location = new System.Drawing.Point(6, 19);
            this.symbolPanel.MaximumSize = new System.Drawing.Size(254, 450);
            this.symbolPanel.Name = "symbolPanel";
            this.symbolPanel.Size = new System.Drawing.Size(254, 44);
            this.symbolPanel.TabIndex = 0;
            // 
            // symbolLibrarySelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(284, 87);
            this.Controls.Add(this.symbolGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "symbolLibrarySelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Family Symbols";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.helpButtonClick);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.helpRequest);
            this.symbolGroup.ResumeLayout(false);
            this.symbolGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox symbolGroup;
        public System.Windows.Forms.Panel symbolPanel;
    }
}