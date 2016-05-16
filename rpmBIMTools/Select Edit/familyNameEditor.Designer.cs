namespace rpmBIMTools
{
    partial class familyNameEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(familyNameEditor));
            this.familyFormatSwitch = new System.Windows.Forms.RadioButton();
            this.familyFormatSwitch2 = new System.Windows.Forms.RadioButton();
            this.familySelectionPrevious = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.familySelection = new System.Windows.Forms.DataGridView();
            this.familiesIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.familiesNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.familiesCompliantColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.familiesLabel = new System.Windows.Forms.Label();
            this.familySelectionNext = new System.Windows.Forms.Button();
            this.familyCounter = new System.Windows.Forms.Label();
            this.familyCompliant = new System.Windows.Forms.PictureBox();
            this.familyInfoGroup = new System.Windows.Forms.GroupBox();
            this.familyContinue = new System.Windows.Forms.CheckBox();
            this.familyReset = new System.Windows.Forms.Button();
            this.familyApply = new System.Windows.Forms.Button();
            this.familyUniclass2 = new System.Windows.Forms.ComboBox();
            this.familyPresentation = new System.Windows.Forms.TextBox();
            this.familyCustom = new System.Windows.Forms.TextBox();
            this.familyRole = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.familyManufacturer = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.familyModel = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.familyDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.familyInfo = new System.Windows.Forms.Label();
            this.familyName = new System.Windows.Forms.Label();
            this.familyIcon = new System.Windows.Forms.PictureBox();
            this.closeEditor = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.familySelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.familyCompliant)).BeginInit();
            this.familyInfoGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.familyIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // familyFormatSwitch
            // 
            this.familyFormatSwitch.AutoSize = true;
            this.familyFormatSwitch.Checked = true;
            this.familyFormatSwitch.Location = new System.Drawing.Point(31, 30);
            this.familyFormatSwitch.Name = "familyFormatSwitch";
            this.familyFormatSwitch.Size = new System.Drawing.Size(145, 17);
            this.familyFormatSwitch.TabIndex = 0;
            this.familyFormatSwitch.TabStop = true;
            this.familyFormatSwitch.Text = "BS 1192:2007 + A1:2005";
            this.familyFormatSwitch.UseVisualStyleBackColor = true;
            this.familyFormatSwitch.CheckedChanged += new System.EventHandler(this.familyFormatSwitch_CheckedChanged);
            // 
            // familyFormatSwitch2
            // 
            this.familyFormatSwitch2.AutoSize = true;
            this.familyFormatSwitch2.Location = new System.Drawing.Point(31, 244);
            this.familyFormatSwitch2.Name = "familyFormatSwitch2";
            this.familyFormatSwitch2.Size = new System.Drawing.Size(60, 17);
            this.familyFormatSwitch2.TabIndex = 0;
            this.familyFormatSwitch2.Text = "Custom";
            this.familyFormatSwitch2.UseVisualStyleBackColor = true;
            this.familyFormatSwitch2.CheckedChanged += new System.EventHandler(this.familyFormatSwitch2_CheckedChanged);
            // 
            // familySelectionPrevious
            // 
            this.familySelectionPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.familySelectionPrevious.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.familySelectionPrevious.Location = new System.Drawing.Point(3, 425);
            this.familySelectionPrevious.Name = "familySelectionPrevious";
            this.familySelectionPrevious.Size = new System.Drawing.Size(75, 30);
            this.familySelectionPrevious.TabIndex = 1;
            this.familySelectionPrevious.Text = "Previous";
            this.familySelectionPrevious.UseVisualStyleBackColor = true;
            this.familySelectionPrevious.Click += new System.EventHandler(this.familySelectionPrevious_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.familySelection);
            this.splitContainer1.Panel1.Controls.Add(this.familiesLabel);
            this.splitContainer1.Panel1.Controls.Add(this.familySelectionNext);
            this.splitContainer1.Panel1.Controls.Add(this.familySelectionPrevious);
            this.splitContainer1.Panel1.Controls.Add(this.familyCounter);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Panel1MinSize = 300;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.familyCompliant);
            this.splitContainer1.Panel2.Controls.Add(this.familyInfoGroup);
            this.splitContainer1.Panel2.Controls.Add(this.familyInfo);
            this.splitContainer1.Panel2.Controls.Add(this.familyName);
            this.splitContainer1.Panel2.Controls.Add(this.familyIcon);
            this.splitContainer1.Panel2.Controls.Add(this.closeEditor);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Panel2MinSize = 500;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(834, 462);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 6;
            // 
            // familySelection
            // 
            this.familySelection.AllowUserToAddRows = false;
            this.familySelection.AllowUserToDeleteRows = false;
            this.familySelection.AllowUserToResizeColumns = false;
            this.familySelection.AllowUserToResizeRows = false;
            this.familySelection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familySelection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.familySelection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.familiesIdColumn,
            this.familiesNameColumn,
            this.familiesCompliantColumn});
            this.familySelection.Location = new System.Drawing.Point(3, 23);
            this.familySelection.MultiSelect = false;
            this.familySelection.Name = "familySelection";
            this.familySelection.ReadOnly = true;
            this.familySelection.RowHeadersVisible = false;
            this.familySelection.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.familySelection.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.familySelection.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.familySelection.Size = new System.Drawing.Size(290, 396);
            this.familySelection.TabIndex = 0;
            this.familySelection.SelectionChanged += new System.EventHandler(this.familySelection_SelectionChanged);
            // 
            // familiesIdColumn
            // 
            this.familiesIdColumn.HeaderText = "ID";
            this.familiesIdColumn.Name = "familiesIdColumn";
            this.familiesIdColumn.ReadOnly = true;
            this.familiesIdColumn.Visible = false;
            this.familiesIdColumn.Width = 40;
            // 
            // familiesNameColumn
            // 
            this.familiesNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.familiesNameColumn.HeaderText = "Family Name";
            this.familiesNameColumn.Name = "familiesNameColumn";
            this.familiesNameColumn.ReadOnly = true;
            // 
            // familiesCompliantColumn
            // 
            this.familiesCompliantColumn.FalseValue = "false";
            this.familiesCompliantColumn.HeaderText = "✓";
            this.familiesCompliantColumn.MinimumWidth = 40;
            this.familiesCompliantColumn.Name = "familiesCompliantColumn";
            this.familiesCompliantColumn.ReadOnly = true;
            this.familiesCompliantColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.familiesCompliantColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.familiesCompliantColumn.TrueValue = "true";
            this.familiesCompliantColumn.Width = 40;
            // 
            // familiesLabel
            // 
            this.familiesLabel.AutoSize = true;
            this.familiesLabel.Location = new System.Drawing.Point(3, 7);
            this.familiesLabel.Name = "familiesLabel";
            this.familiesLabel.Size = new System.Drawing.Size(80, 13);
            this.familiesLabel.TabIndex = 6;
            this.familiesLabel.Text = "Project Families";
            // 
            // familySelectionNext
            // 
            this.familySelectionNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.familySelectionNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.familySelectionNext.Location = new System.Drawing.Point(218, 425);
            this.familySelectionNext.Name = "familySelectionNext";
            this.familySelectionNext.Size = new System.Drawing.Size(75, 30);
            this.familySelectionNext.TabIndex = 2;
            this.familySelectionNext.Text = "Next";
            this.familySelectionNext.UseVisualStyleBackColor = true;
            this.familySelectionNext.Click += new System.EventHandler(this.familySelectionNext_Click);
            // 
            // familyCounter
            // 
            this.familyCounter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familyCounter.Location = new System.Drawing.Point(3, 425);
            this.familyCounter.Name = "familyCounter";
            this.familyCounter.Size = new System.Drawing.Size(290, 30);
            this.familyCounter.TabIndex = 6;
            this.familyCounter.Text = "## / ##";
            this.familyCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // familyCompliant
            // 
            this.familyCompliant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.familyCompliant.ErrorImage = ((System.Drawing.Image)(resources.GetObject("familyCompliant.ErrorImage")));
            this.familyCompliant.Image = global::rpmBIMTools.Properties.Resources.tick32;
            this.familyCompliant.InitialImage = ((System.Drawing.Image)(resources.GetObject("familyCompliant.InitialImage")));
            this.familyCompliant.Location = new System.Drawing.Point(451, 25);
            this.familyCompliant.Name = "familyCompliant";
            this.familyCompliant.Size = new System.Drawing.Size(50, 50);
            this.familyCompliant.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.familyCompliant.TabIndex = 22;
            this.familyCompliant.TabStop = false;
            // 
            // familyInfoGroup
            // 
            this.familyInfoGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.familyInfoGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.familyInfoGroup.Controls.Add(this.familyContinue);
            this.familyInfoGroup.Controls.Add(this.familyReset);
            this.familyInfoGroup.Controls.Add(this.familyApply);
            this.familyInfoGroup.Controls.Add(this.familyFormatSwitch);
            this.familyInfoGroup.Controls.Add(this.familyFormatSwitch2);
            this.familyInfoGroup.Controls.Add(this.familyUniclass2);
            this.familyInfoGroup.Controls.Add(this.familyPresentation);
            this.familyInfoGroup.Controls.Add(this.familyCustom);
            this.familyInfoGroup.Controls.Add(this.familyRole);
            this.familyInfoGroup.Controls.Add(this.label9);
            this.familyInfoGroup.Controls.Add(this.familyManufacturer);
            this.familyInfoGroup.Controls.Add(this.label8);
            this.familyInfoGroup.Controls.Add(this.familyModel);
            this.familyInfoGroup.Controls.Add(this.label7);
            this.familyInfoGroup.Controls.Add(this.familyDescription);
            this.familyInfoGroup.Controls.Add(this.label6);
            this.familyInfoGroup.Controls.Add(this.label4);
            this.familyInfoGroup.Controls.Add(this.label5);
            this.familyInfoGroup.Location = new System.Drawing.Point(25, 92);
            this.familyInfoGroup.Name = "familyInfoGroup";
            this.familyInfoGroup.Size = new System.Drawing.Size(476, 314);
            this.familyInfoGroup.TabIndex = 3;
            this.familyInfoGroup.TabStop = false;
            this.familyInfoGroup.Text = "Family Rename Format:";
            // 
            // familyContinue
            // 
            this.familyContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.familyContinue.Checked = true;
            this.familyContinue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.familyContinue.Location = new System.Drawing.Point(31, 273);
            this.familyContinue.Margin = new System.Windows.Forms.Padding(0);
            this.familyContinue.Name = "familyContinue";
            this.familyContinue.Size = new System.Drawing.Size(266, 24);
            this.familyContinue.TabIndex = 8;
            this.familyContinue.Text = "Continue to next non-compliant family on apply.";
            this.familyContinue.UseVisualStyleBackColor = true;
            // 
            // familyReset
            // 
            this.familyReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.familyReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.familyReset.Location = new System.Drawing.Point(300, 269);
            this.familyReset.Name = "familyReset";
            this.familyReset.Size = new System.Drawing.Size(60, 30);
            this.familyReset.TabIndex = 9;
            this.familyReset.Text = "Reset";
            this.familyReset.UseVisualStyleBackColor = true;
            this.familyReset.Click += new System.EventHandler(this.familyReset_Click);
            // 
            // familyApply
            // 
            this.familyApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.familyApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.familyApply.Location = new System.Drawing.Point(366, 269);
            this.familyApply.Name = "familyApply";
            this.familyApply.Size = new System.Drawing.Size(91, 30);
            this.familyApply.TabIndex = 10;
            this.familyApply.Text = "Apply";
            this.familyApply.UseVisualStyleBackColor = true;
            this.familyApply.Click += new System.EventHandler(this.familyApply_Click);
            // 
            // familyUniclass2
            // 
            this.familyUniclass2.AllowDrop = true;
            this.familyUniclass2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familyUniclass2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.familyUniclass2.FormattingEnabled = true;
            this.familyUniclass2.Location = new System.Drawing.Point(117, 94);
            this.familyUniclass2.Name = "familyUniclass2";
            this.familyUniclass2.Size = new System.Drawing.Size(340, 21);
            this.familyUniclass2.TabIndex = 2;
            this.familyUniclass2.SelectedIndexChanged += new System.EventHandler(this.familyCustomChange);
            this.familyUniclass2.TextUpdate += new System.EventHandler(this.familyUniclass2_TextUpdate);
            // 
            // familyPresentation
            // 
            this.familyPresentation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familyPresentation.Enabled = false;
            this.familyPresentation.Location = new System.Drawing.Point(117, 121);
            this.familyPresentation.Name = "familyPresentation";
            this.familyPresentation.Size = new System.Drawing.Size(340, 20);
            this.familyPresentation.TabIndex = 3;
            // 
            // familyCustom
            // 
            this.familyCustom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familyCustom.Enabled = false;
            this.familyCustom.Location = new System.Drawing.Point(117, 243);
            this.familyCustom.Name = "familyCustom";
            this.familyCustom.Size = new System.Drawing.Size(340, 20);
            this.familyCustom.TabIndex = 7;
            // 
            // familyRole
            // 
            this.familyRole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familyRole.Enabled = false;
            this.familyRole.Location = new System.Drawing.Point(117, 68);
            this.familyRole.Name = "familyRole";
            this.familyRole.Size = new System.Drawing.Size(340, 20);
            this.familyRole.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 202);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Description:";
            // 
            // familyManufacturer
            // 
            this.familyManufacturer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familyManufacturer.Location = new System.Drawing.Point(117, 147);
            this.familyManufacturer.Name = "familyManufacturer";
            this.familyManufacturer.Size = new System.Drawing.Size(340, 20);
            this.familyManufacturer.TabIndex = 4;
            this.familyManufacturer.TextChanged += new System.EventHandler(this.familyCustomChange);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Model:";
            // 
            // familyModel
            // 
            this.familyModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familyModel.Location = new System.Drawing.Point(117, 173);
            this.familyModel.Name = "familyModel";
            this.familyModel.Size = new System.Drawing.Size(340, 20);
            this.familyModel.TabIndex = 5;
            this.familyModel.TextChanged += new System.EventHandler(this.familyCustomChange);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Manufacturer:";
            // 
            // familyDescription
            // 
            this.familyDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familyDescription.Location = new System.Drawing.Point(117, 199);
            this.familyDescription.Name = "familyDescription";
            this.familyDescription.Size = new System.Drawing.Size(340, 20);
            this.familyDescription.TabIndex = 6;
            this.familyDescription.TextChanged += new System.EventHandler(this.familyCustomChange);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Presentation:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Role:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Uniclass2:";
            // 
            // familyInfo
            // 
            this.familyInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.familyInfo.Location = new System.Drawing.Point(85, 54);
            this.familyInfo.Name = "familyInfo";
            this.familyInfo.Size = new System.Drawing.Size(360, 17);
            this.familyInfo.TabIndex = 19;
            this.familyInfo.Text = "Family Information";
            // 
            // familyName
            // 
            this.familyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.familyName.Location = new System.Drawing.Point(85, 29);
            this.familyName.Name = "familyName";
            this.familyName.Size = new System.Drawing.Size(360, 17);
            this.familyName.TabIndex = 18;
            this.familyName.Text = "Existing Family Name";
            // 
            // familyIcon
            // 
            this.familyIcon.ErrorImage = global::rpmBIMTools.Properties.Resources.DrawingSheet32;
            this.familyIcon.Image = global::rpmBIMTools.Properties.Resources.DrawingSheet32;
            this.familyIcon.InitialImage = global::rpmBIMTools.Properties.Resources.DrawingSheet32;
            this.familyIcon.Location = new System.Drawing.Point(25, 25);
            this.familyIcon.Name = "familyIcon";
            this.familyIcon.Size = new System.Drawing.Size(50, 50);
            this.familyIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.familyIcon.TabIndex = 17;
            this.familyIcon.TabStop = false;
            // 
            // closeEditor
            // 
            this.closeEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeEditor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.closeEditor.Location = new System.Drawing.Point(432, 425);
            this.closeEditor.Name = "closeEditor";
            this.closeEditor.Size = new System.Drawing.Size(91, 30);
            this.closeEditor.TabIndex = 4;
            this.closeEditor.Text = "Close Editor";
            this.closeEditor.UseVisualStyleBackColor = true;
            this.closeEditor.Click += new System.EventHandler(this.closeEditor_Click);
            // 
            // familyNameEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(834, 462);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1500, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(850, 500);
            this.Name = "familyNameEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Family Name Editor";
            this.Load += new System.EventHandler(this.familyNameEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.familySelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.familyCompliant)).EndInit();
            this.familyInfoGroup.ResumeLayout(false);
            this.familyInfoGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.familyIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton familyFormatSwitch;
        private System.Windows.Forms.RadioButton familyFormatSwitch2;
        private System.Windows.Forms.Button familySelectionPrevious;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox familyUniclass2;
        private System.Windows.Forms.Button familySelectionNext;
        private System.Windows.Forms.TextBox familyPresentation;
        private System.Windows.Forms.Button closeEditor;
        private System.Windows.Forms.Label familiesLabel;
        private System.Windows.Forms.Label familyCounter;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox familyDescription;
        private System.Windows.Forms.TextBox familyModel;
        private System.Windows.Forms.TextBox familyManufacturer;
        private System.Windows.Forms.TextBox familyRole;
        private System.Windows.Forms.Label familyInfo;
        private System.Windows.Forms.Label familyName;
        private System.Windows.Forms.PictureBox familyIcon;
        private System.Windows.Forms.TextBox familyCustom;
        private System.Windows.Forms.Button familyApply;
        private System.Windows.Forms.GroupBox familyInfoGroup;
        private System.Windows.Forms.PictureBox familyCompliant;
        private System.Windows.Forms.DataGridView familySelection;
        private System.Windows.Forms.Button familyReset;
        private System.Windows.Forms.CheckBox familyContinue;
        private System.Windows.Forms.DataGridViewTextBoxColumn familiesIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn familiesNameColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn familiesCompliantColumn;
    }
}