namespace rpmBIMTools
{
    partial class generateDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(generateDialog));
            this.generateButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.GUIDText = new System.Windows.Forms.TextBox();
            this.allCaps = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(126, 38);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(80, 30);
            this.generateButton.TabIndex = 2;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(212, 38);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 30);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // GUIDText
            // 
            this.GUIDText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GUIDText.Location = new System.Drawing.Point(12, 12);
            this.GUIDText.MaxLength = 100;
            this.GUIDText.Name = "GUIDText";
            this.GUIDText.ReadOnly = true;
            this.GUIDText.Size = new System.Drawing.Size(280, 20);
            this.GUIDText.TabIndex = 0;
            this.GUIDText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // allCaps
            // 
            this.allCaps.AutoSize = true;
            this.allCaps.Location = new System.Drawing.Point(22, 46);
            this.allCaps.Name = "allCaps";
            this.allCaps.Size = new System.Drawing.Size(64, 17);
            this.allCaps.TabIndex = 1;
            this.allCaps.Text = "All Caps";
            this.allCaps.UseVisualStyleBackColor = true;
            // 
            // generateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(304, 77);
            this.Controls.Add(this.allCaps);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.GUIDText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "generateDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate GUID";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.helpButtonClick);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.helpRequest);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button closeButton;
        public System.Windows.Forms.TextBox GUIDText;
        private System.Windows.Forms.CheckBox allCaps;
    }
}