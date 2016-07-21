namespace rpmBIMTools {

    partial class exportImportSchedules
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(exportImportSchedules));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.exportTab = new System.Windows.Forms.TabPage();
            this.exportMethodLabel = new System.Windows.Forms.Label();
            this.exportPrefixLabel = new System.Windows.Forms.Label();
            this.exportPrefix = new System.Windows.Forms.TextBox();
            this.exportMethodSingle = new System.Windows.Forms.RadioButton();
            this.exportMethodMultiple = new System.Windows.Forms.RadioButton();
            this.exportFolderLabel = new System.Windows.Forms.Label();
            this.exportFolderButton = new System.Windows.Forms.Button();
            this.exportFolder = new System.Windows.Forms.TextBox();
            this.exportScheduleList = new System.Windows.Forms.CheckedListBox();
            this.exportScheduleListToggle = new System.Windows.Forms.CheckBox();
            this.exportImage = new System.Windows.Forms.PictureBox();
            this.importTab = new System.Windows.Forms.TabPage();
            this.importScheduleListToggle = new System.Windows.Forms.CheckBox();
            this.importScheduleList = new System.Windows.Forms.CheckedListBox();
            this.importFileLabel = new System.Windows.Forms.Label();
            this.importFileButton = new System.Windows.Forms.Button();
            this.importFile = new System.Windows.Forms.TextBox();
            this.importImage = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.cancelButton = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.importFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl.SuspendLayout();
            this.exportTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exportImage)).BeginInit();
            this.importTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.importImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.exportTab);
            this.tabControl.Controls.Add(this.importTab);
            this.tabControl.ImageList = this.imageList1;
            this.tabControl.ItemSize = new System.Drawing.Size(80, 19);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(360, 402);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // exportTab
            // 
            this.exportTab.Controls.Add(this.exportMethodLabel);
            this.exportTab.Controls.Add(this.exportPrefixLabel);
            this.exportTab.Controls.Add(this.exportPrefix);
            this.exportTab.Controls.Add(this.exportMethodSingle);
            this.exportTab.Controls.Add(this.exportMethodMultiple);
            this.exportTab.Controls.Add(this.exportFolderLabel);
            this.exportTab.Controls.Add(this.exportFolderButton);
            this.exportTab.Controls.Add(this.exportFolder);
            this.exportTab.Controls.Add(this.exportScheduleList);
            this.exportTab.Controls.Add(this.exportScheduleListToggle);
            this.exportTab.Controls.Add(this.exportImage);
            this.exportTab.ImageIndex = 1;
            this.exportTab.Location = new System.Drawing.Point(4, 23);
            this.exportTab.Name = "exportTab";
            this.exportTab.Padding = new System.Windows.Forms.Padding(3);
            this.exportTab.Size = new System.Drawing.Size(352, 375);
            this.exportTab.TabIndex = 1;
            this.exportTab.Text = "Export";
            this.exportTab.UseVisualStyleBackColor = true;
            // 
            // exportMethodLabel
            // 
            this.exportMethodLabel.AutoSize = true;
            this.exportMethodLabel.Location = new System.Drawing.Point(7, 327);
            this.exportMethodLabel.Name = "exportMethodLabel";
            this.exportMethodLabel.Size = new System.Drawing.Size(81, 13);
            this.exportMethodLabel.TabIndex = 12;
            this.exportMethodLabel.Text = "Output Method:";
            // 
            // exportPrefixLabel
            // 
            this.exportPrefixLabel.AutoSize = true;
            this.exportPrefixLabel.Location = new System.Drawing.Point(7, 302);
            this.exportPrefixLabel.Name = "exportPrefixLabel";
            this.exportPrefixLabel.Size = new System.Drawing.Size(71, 13);
            this.exportPrefixLabel.TabIndex = 11;
            this.exportPrefixLabel.Text = "Output Prefix:";
            // 
            // exportPrefix
            // 
            this.exportPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportPrefix.Location = new System.Drawing.Point(94, 298);
            this.exportPrefix.Name = "exportPrefix";
            this.exportPrefix.Size = new System.Drawing.Size(223, 20);
            this.exportPrefix.TabIndex = 10;
            // 
            // exportMethodSingle
            // 
            this.exportMethodSingle.AutoSize = true;
            this.exportMethodSingle.Checked = true;
            this.exportMethodSingle.Location = new System.Drawing.Point(94, 327);
            this.exportMethodSingle.Name = "exportMethodSingle";
            this.exportMethodSingle.Size = new System.Drawing.Size(162, 17);
            this.exportMethodSingle.TabIndex = 9;
            this.exportMethodSingle.TabStop = true;
            this.exportMethodSingle.Text = "One File, Sheet per schedule";
            this.exportMethodSingle.UseVisualStyleBackColor = true;
            // 
            // exportMethodMultiple
            // 
            this.exportMethodMultiple.AutoSize = true;
            this.exportMethodMultiple.Location = new System.Drawing.Point(94, 350);
            this.exportMethodMultiple.Name = "exportMethodMultiple";
            this.exportMethodMultiple.Size = new System.Drawing.Size(171, 17);
            this.exportMethodMultiple.TabIndex = 8;
            this.exportMethodMultiple.Text = "Multiple Files, File per schedule";
            this.exportMethodMultiple.UseVisualStyleBackColor = true;
            // 
            // exportFolderLabel
            // 
            this.exportFolderLabel.AutoSize = true;
            this.exportFolderLabel.Location = new System.Drawing.Point(7, 276);
            this.exportFolderLabel.Name = "exportFolderLabel";
            this.exportFolderLabel.Size = new System.Drawing.Size(74, 13);
            this.exportFolderLabel.TabIndex = 7;
            this.exportFolderLabel.Text = "Output Folder:";
            // 
            // exportFolderButton
            // 
            this.exportFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportFolderButton.Location = new System.Drawing.Point(323, 271);
            this.exportFolderButton.Name = "exportFolderButton";
            this.exportFolderButton.Size = new System.Drawing.Size(24, 20);
            this.exportFolderButton.TabIndex = 6;
            this.exportFolderButton.Text = "...";
            this.exportFolderButton.UseVisualStyleBackColor = true;
            this.exportFolderButton.Click += new System.EventHandler(this.exportFolderButton_Click);
            // 
            // exportFolder
            // 
            this.exportFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportFolder.Location = new System.Drawing.Point(94, 272);
            this.exportFolder.Name = "exportFolder";
            this.exportFolder.ReadOnly = true;
            this.exportFolder.Size = new System.Drawing.Size(223, 20);
            this.exportFolder.TabIndex = 5;
            this.exportFolder.TextChanged += new System.EventHandler(this.exportScheduleList_SelectedIndexChanged);
            // 
            // exportScheduleList
            // 
            this.exportScheduleList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportScheduleList.CheckOnClick = true;
            this.exportScheduleList.FormattingEnabled = true;
            this.exportScheduleList.Location = new System.Drawing.Point(6, 44);
            this.exportScheduleList.Name = "exportScheduleList";
            this.exportScheduleList.Size = new System.Drawing.Size(340, 199);
            this.exportScheduleList.Sorted = true;
            this.exportScheduleList.TabIndex = 4;
            this.exportScheduleList.SelectedIndexChanged += new System.EventHandler(this.exportScheduleList_SelectedIndexChanged);
            // 
            // exportScheduleListToggle
            // 
            this.exportScheduleListToggle.AutoSize = true;
            this.exportScheduleListToggle.Location = new System.Drawing.Point(9, 249);
            this.exportScheduleListToggle.Name = "exportScheduleListToggle";
            this.exportScheduleListToggle.Size = new System.Drawing.Size(107, 17);
            this.exportScheduleListToggle.TabIndex = 3;
            this.exportScheduleListToggle.Text = "Select All / None";
            this.exportScheduleListToggle.UseVisualStyleBackColor = true;
            this.exportScheduleListToggle.CheckedChanged += new System.EventHandler(this.exportScheduleListToggle_CheckedChanged);
            // 
            // exportImage
            // 
            this.exportImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportImage.Image = global::rpmBIMTools.Properties.Resources.ExportSchedule32;
            this.exportImage.Location = new System.Drawing.Point(314, 6);
            this.exportImage.Name = "exportImage";
            this.exportImage.Size = new System.Drawing.Size(32, 32);
            this.exportImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.exportImage.TabIndex = 1;
            this.exportImage.TabStop = false;
            // 
            // importTab
            // 
            this.importTab.Controls.Add(this.importScheduleListToggle);
            this.importTab.Controls.Add(this.importScheduleList);
            this.importTab.Controls.Add(this.importFileLabel);
            this.importTab.Controls.Add(this.importFileButton);
            this.importTab.Controls.Add(this.importFile);
            this.importTab.Controls.Add(this.importImage);
            this.importTab.ImageIndex = 0;
            this.importTab.Location = new System.Drawing.Point(4, 23);
            this.importTab.Name = "importTab";
            this.importTab.Padding = new System.Windows.Forms.Padding(3);
            this.importTab.Size = new System.Drawing.Size(352, 375);
            this.importTab.TabIndex = 0;
            this.importTab.Text = "Import";
            this.importTab.UseVisualStyleBackColor = true;
            // 
            // importScheduleListToggle
            // 
            this.importScheduleListToggle.AutoSize = true;
            this.importScheduleListToggle.Location = new System.Drawing.Point(9, 350);
            this.importScheduleListToggle.Name = "importScheduleListToggle";
            this.importScheduleListToggle.Size = new System.Drawing.Size(107, 17);
            this.importScheduleListToggle.TabIndex = 6;
            this.importScheduleListToggle.Text = "Select All / None";
            this.importScheduleListToggle.UseVisualStyleBackColor = true;
            this.importScheduleListToggle.CheckedChanged += new System.EventHandler(this.importScheduleListToggle_CheckedChanged);
            // 
            // importScheduleList
            // 
            this.importScheduleList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importScheduleList.CheckOnClick = true;
            this.importScheduleList.FormattingEnabled = true;
            this.importScheduleList.Location = new System.Drawing.Point(6, 44);
            this.importScheduleList.Name = "importScheduleList";
            this.importScheduleList.Size = new System.Drawing.Size(340, 289);
            this.importScheduleList.TabIndex = 5;
            this.importScheduleList.SelectedIndexChanged += new System.EventHandler(this.importScheduleList_SelectedIndexChanged);
            // 
            // importFileLabel
            // 
            this.importFileLabel.AutoSize = true;
            this.importFileLabel.Location = new System.Drawing.Point(6, 16);
            this.importFileLabel.Name = "importFileLabel";
            this.importFileLabel.Size = new System.Drawing.Size(26, 13);
            this.importFileLabel.TabIndex = 4;
            this.importFileLabel.Text = "File:";
            // 
            // importFileButton
            // 
            this.importFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importFileButton.Location = new System.Drawing.Point(284, 12);
            this.importFileButton.Name = "importFileButton";
            this.importFileButton.Size = new System.Drawing.Size(24, 20);
            this.importFileButton.TabIndex = 2;
            this.importFileButton.Text = "...";
            this.importFileButton.UseVisualStyleBackColor = true;
            this.importFileButton.Click += new System.EventHandler(this.importFileButton_Click);
            // 
            // importFile
            // 
            this.importFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importFile.Location = new System.Drawing.Point(38, 12);
            this.importFile.Name = "importFile";
            this.importFile.ReadOnly = true;
            this.importFile.Size = new System.Drawing.Size(240, 20);
            this.importFile.TabIndex = 1;
            // 
            // importImage
            // 
            this.importImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importImage.Image = global::rpmBIMTools.Properties.Resources.ImportSchedule32;
            this.importImage.Location = new System.Drawing.Point(314, 6);
            this.importImage.Name = "importImage";
            this.importImage.Size = new System.Drawing.Size(32, 32);
            this.importImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.importImage.TabIndex = 0;
            this.importImage.TabStop = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ImportSchedule16.ico");
            this.imageList1.Images.SetKeyName(1, "ExportSchedule16.ico");
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(297, 420);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.importButton.Enabled = false;
            this.importButton.Location = new System.Drawing.Point(216, 420);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 30);
            this.importButton.TabIndex = 5;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Visible = false;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.Enabled = false;
            this.exportButton.Location = new System.Drawing.Point(216, 420);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(75, 30);
            this.exportButton.TabIndex = 6;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // importFileDialog
            // 
            this.importFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";
            // 
            // exportImportSchedules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 462);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.tabControl);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 500);
            this.Name = "exportImportSchedules";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export / Import Schedules";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.helpButtonClick);
            this.Load += new System.EventHandler(this.exportImportSchedules_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.helpRequest);
            this.tabControl.ResumeLayout(false);
            this.exportTab.ResumeLayout(false);
            this.exportTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exportImage)).EndInit();
            this.importTab.ResumeLayout(false);
            this.importTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.importImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage importTab;
        private System.Windows.Forms.TabPage exportTab;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox importImage;
        private System.Windows.Forms.TextBox importFile;
        private System.Windows.Forms.PictureBox exportImage;
        private System.Windows.Forms.Button importFileButton;
        private System.Windows.Forms.CheckBox exportScheduleListToggle;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Label importFileLabel;
        private System.Windows.Forms.CheckedListBox exportScheduleList;
        private System.Windows.Forms.CheckedListBox importScheduleList;
        private System.Windows.Forms.Label exportFolderLabel;
        private System.Windows.Forms.Button exportFolderButton;
        private System.Windows.Forms.TextBox exportFolder;
        private System.Windows.Forms.Label exportMethodLabel;
        private System.Windows.Forms.Label exportPrefixLabel;
        private System.Windows.Forms.TextBox exportPrefix;
        private System.Windows.Forms.RadioButton exportMethodSingle;
        private System.Windows.Forms.RadioButton exportMethodMultiple;
        private System.Windows.Forms.OpenFileDialog importFileDialog;
        private System.Windows.Forms.CheckBox importScheduleListToggle;
    }
}