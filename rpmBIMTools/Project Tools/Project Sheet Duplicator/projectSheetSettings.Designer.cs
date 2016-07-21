namespace rpmBIMTools
{
    partial class projectSheetSettings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(projectSheetSettings));
            this.duplicateLabel = new System.Windows.Forms.Label();
            this.infoText = new System.Windows.Forms.Label();
            this.viewTemplateLabel = new System.Windows.Forms.Label();
            this.sheetTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.cancelButton = new System.Windows.Forms.Button();
            this.templateList = new System.Windows.Forms.CheckedListBox();
            this.duplicateButton = new System.Windows.Forms.Button();
            this.includeLegends = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // duplicateLabel
            // 
            this.duplicateLabel.AutoSize = true;
            this.duplicateLabel.Location = new System.Drawing.Point(9, 109);
            this.duplicateLabel.Name = "duplicateLabel";
            this.duplicateLabel.Size = new System.Drawing.Size(155, 13);
            this.duplicateLabel.TabIndex = 0;
            this.duplicateLabel.Text = "Master sheets to be duplicated:";
            // 
            // infoText
            // 
            this.infoText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.infoText.Location = new System.Drawing.Point(12, 9);
            this.infoText.Name = "infoText";
            this.infoText.Size = new System.Drawing.Size(310, 100);
            this.infoText.TabIndex = 0;
            this.infoText.Text = resources.GetString("infoText.Text");
            // 
            // viewTemplateLabel
            // 
            this.viewTemplateLabel.AutoSize = true;
            this.viewTemplateLabel.Location = new System.Drawing.Point(9, 288);
            this.viewTemplateLabel.Name = "viewTemplateLabel";
            this.viewTemplateLabel.Size = new System.Drawing.Size(209, 13);
            this.viewTemplateLabel.TabIndex = 0;
            this.viewTemplateLabel.Text = "View template services to be duplicated to:";
            // 
            // sheetTree
            // 
            this.sheetTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.sheetTree.ImageIndex = 0;
            this.sheetTree.ImageList = this.imageList;
            this.sheetTree.Location = new System.Drawing.Point(12, 128);
            this.sheetTree.Name = "sheetTree";
            this.sheetTree.SelectedImageIndex = 0;
            this.sheetTree.Size = new System.Drawing.Size(310, 154);
            this.sheetTree.TabIndex = 1;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "button2_3.png");
            this.imageList.Images.SetKeyName(1, "sheets.png");
            this.imageList.Images.SetKeyName(2, "views.png");
            this.imageList.Images.SetKeyName(3, "cross_16x16.png");
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(247, 470);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 30);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // templateList
            // 
            this.templateList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.templateList.CheckOnClick = true;
            this.templateList.FormattingEnabled = true;
            this.templateList.Location = new System.Drawing.Point(12, 307);
            this.templateList.Name = "templateList";
            this.templateList.Size = new System.Drawing.Size(310, 154);
            this.templateList.Sorted = true;
            this.templateList.TabIndex = 2;
            this.templateList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.templateList_ItemCheck);
            // 
            // duplicateButton
            // 
            this.duplicateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.duplicateButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.duplicateButton.Enabled = false;
            this.duplicateButton.Location = new System.Drawing.Point(166, 470);
            this.duplicateButton.Name = "duplicateButton";
            this.duplicateButton.Size = new System.Drawing.Size(75, 30);
            this.duplicateButton.TabIndex = 4;
            this.duplicateButton.Text = "Duplicate";
            this.duplicateButton.UseVisualStyleBackColor = true;
            // 
            // includeLegends
            // 
            this.includeLegends.AutoSize = true;
            this.includeLegends.Checked = true;
            this.includeLegends.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeLegends.Location = new System.Drawing.Point(21, 478);
            this.includeLegends.Name = "includeLegends";
            this.includeLegends.Size = new System.Drawing.Size(137, 17);
            this.includeLegends.TabIndex = 3;
            this.includeLegends.Text = "Add Legends to Sheets";
            this.includeLegends.UseVisualStyleBackColor = true;
            // 
            // projectSheetSettings
            // 
            this.AcceptButton = this.duplicateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(334, 512);
            this.Controls.Add(this.includeLegends);
            this.Controls.Add(this.duplicateButton);
            this.Controls.Add(this.templateList);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.sheetTree);
            this.Controls.Add(this.viewTemplateLabel);
            this.Controls.Add(this.infoText);
            this.Controls.Add(this.duplicateLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(350, 550);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 550);
            this.Name = "projectSheetSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Sheet Duplicator";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.helpButtonClick);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.helpRequest);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label duplicateLabel;
        private System.Windows.Forms.Label infoText;
        private System.Windows.Forms.Label viewTemplateLabel;
        public System.Windows.Forms.TreeView sheetTree;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.CheckedListBox templateList;
        private System.Windows.Forms.Button duplicateButton;
        public System.Windows.Forms.CheckBox includeLegends;
    }
}