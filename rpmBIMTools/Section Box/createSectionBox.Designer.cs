namespace rpmBIMTools
{
    partial class createSectionBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(createSectionBox));
            this.viewName = new System.Windows.Forms.TextBox();
            this.createButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.viewLabel = new System.Windows.Forms.Label();
            this.sectionBoxOffset = new System.Windows.Forms.TrackBar();
            this.previewWindow = new System.Windows.Forms.Integration.ElementHost();
            this.displayStyle = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBoxOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // viewName
            // 
            this.viewName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewName.Location = new System.Drawing.Point(48, 446);
            this.viewName.Name = "viewName";
            this.viewName.Size = new System.Drawing.Size(262, 20);
            this.viewName.TabIndex = 0;
            this.viewName.TextChanged += new System.EventHandler(this.viewName_TextChanged);
            // 
            // createButton
            // 
            this.createButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createButton.Enabled = false;
            this.createButton.Location = new System.Drawing.Point(316, 440);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 30);
            this.createButton.TabIndex = 1;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(397, 440);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // viewLabel
            // 
            this.viewLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.viewLabel.AutoSize = true;
            this.viewLabel.Location = new System.Drawing.Point(9, 449);
            this.viewLabel.Name = "viewLabel";
            this.viewLabel.Size = new System.Drawing.Size(33, 13);
            this.viewLabel.TabIndex = 3;
            this.viewLabel.Text = "View:";
            // 
            // sectionBoxOffset
            // 
            this.sectionBoxOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionBoxOffset.AutoSize = false;
            this.sectionBoxOffset.Location = new System.Drawing.Point(12, 404);
            this.sectionBoxOffset.Maximum = 50;
            this.sectionBoxOffset.Name = "sectionBoxOffset";
            this.sectionBoxOffset.Size = new System.Drawing.Size(298, 30);
            this.sectionBoxOffset.TabIndex = 5;
            this.sectionBoxOffset.Scroll += new System.EventHandler(this.updatePreview);
            // 
            // previewWindow
            // 
            this.previewWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewWindow.Location = new System.Drawing.Point(12, 12);
            this.previewWindow.Name = "previewWindow";
            this.previewWindow.Size = new System.Drawing.Size(460, 386);
            this.previewWindow.TabIndex = 7;
            this.previewWindow.Text = "elementHost1";
            this.previewWindow.Child = null;
            // 
            // displayStyle
            // 
            this.displayStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.displayStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.displayStyle.FormattingEnabled = true;
            this.displayStyle.Items.AddRange(new object[] {
            "Wireframe",
            "Hidden Line",
            "Shaded",
            "Consistent Colors",
            "Realistic"});
            this.displayStyle.Location = new System.Drawing.Point(316, 409);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(156, 21);
            this.displayStyle.TabIndex = 8;
            this.displayStyle.SelectedIndexChanged += new System.EventHandler(this.updatePreview);
            // 
            // createSectionBox
            // 
            this.AcceptButton = this.createButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(484, 482);
            this.Controls.Add(this.displayStyle);
            this.Controls.Add(this.previewWindow);
            this.Controls.Add(this.sectionBoxOffset);
            this.Controls.Add(this.viewLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.viewName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 520);
            this.Name = "createSectionBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Section Box";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.createSectionBox_FormClosing);
            this.Load += new System.EventHandler(this.createSectionBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sectionBoxOffset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox viewName;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label viewLabel;
        private System.Windows.Forms.TrackBar sectionBoxOffset;
        private System.Windows.Forms.Integration.ElementHost previewWindow;
        private System.Windows.Forms.ComboBox displayStyle;
    }
}