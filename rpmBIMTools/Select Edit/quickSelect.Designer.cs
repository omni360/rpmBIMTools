namespace rpmBIMTools
{
    partial class quickSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(quickSelect));
            this.selectionZoneLabel = new System.Windows.Forms.Label();
            this.selectionCategory = new System.Windows.Forms.ComboBox();
            this.selectionCategoryLabel = new System.Windows.Forms.Label();
            this.selectionPropertiesLabel = new System.Windows.Forms.Label();
            this.selectionOperatorLabel = new System.Windows.Forms.Label();
            this.selectButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.selectionProperties = new System.Windows.Forms.ListBox();
            this.selectionZone = new System.Windows.Forms.ComboBox();
            this.selectionValueLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.selectionNew = new System.Windows.Forms.Button();
            this.selectionTypeLabel = new System.Windows.Forms.Label();
            this.selectionType = new System.Windows.Forms.Label();
            this.selectionValue = new System.Windows.Forms.ComboBox();
            this.selectionOperator = new System.Windows.Forms.ComboBox();
            this.selectionApplyToBox = new System.Windows.Forms.GroupBox();
            this.selectionApplyToExclude = new System.Windows.Forms.RadioButton();
            this.selectionApplyToInclude = new System.Windows.Forms.RadioButton();
            this.selectionApplyToCurrentSelectionSet = new System.Windows.Forms.CheckBox();
            this.selectionApplyToBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectionZoneLabel
            // 
            this.selectionZoneLabel.Location = new System.Drawing.Point(12, 29);
            this.selectionZoneLabel.Name = "selectionZoneLabel";
            this.selectionZoneLabel.Size = new System.Drawing.Size(80, 13);
            this.selectionZoneLabel.TabIndex = 0;
            this.selectionZoneLabel.Text = "Select From:";
            this.selectionZoneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // selectionCategory
            // 
            this.selectionCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectionCategory.FormattingEnabled = true;
            this.selectionCategory.Location = new System.Drawing.Point(106, 53);
            this.selectionCategory.Name = "selectionCategory";
            this.selectionCategory.Size = new System.Drawing.Size(177, 21);
            this.selectionCategory.Sorted = true;
            this.selectionCategory.TabIndex = 3;
            this.selectionCategory.SelectedIndexChanged += new System.EventHandler(this.selectionCategory_SelectedIndexChanged);
            // 
            // selectionCategoryLabel
            // 
            this.selectionCategoryLabel.Location = new System.Drawing.Point(12, 56);
            this.selectionCategoryLabel.Name = "selectionCategoryLabel";
            this.selectionCategoryLabel.Size = new System.Drawing.Size(80, 13);
            this.selectionCategoryLabel.TabIndex = 2;
            this.selectionCategoryLabel.Text = "Category:";
            this.selectionCategoryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // selectionPropertiesLabel
            // 
            this.selectionPropertiesLabel.Location = new System.Drawing.Point(12, 83);
            this.selectionPropertiesLabel.Name = "selectionPropertiesLabel";
            this.selectionPropertiesLabel.Size = new System.Drawing.Size(80, 13);
            this.selectionPropertiesLabel.TabIndex = 4;
            this.selectionPropertiesLabel.Text = "Properties:";
            this.selectionPropertiesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // selectionOperatorLabel
            // 
            this.selectionOperatorLabel.Location = new System.Drawing.Point(12, 288);
            this.selectionOperatorLabel.Name = "selectionOperatorLabel";
            this.selectionOperatorLabel.Size = new System.Drawing.Size(80, 13);
            this.selectionOperatorLabel.TabIndex = 6;
            this.selectionOperatorLabel.Text = "Operator:";
            this.selectionOperatorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // selectButton
            // 
            this.selectButton.Enabled = false;
            this.selectButton.Location = new System.Drawing.Point(64, 449);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(100, 25);
            this.selectButton.TabIndex = 101;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(193, 449);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 25);
            this.cancelButton.TabIndex = 102;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // selectionProperties
            // 
            this.selectionProperties.FormattingEnabled = true;
            this.selectionProperties.Location = new System.Drawing.Point(106, 83);
            this.selectionProperties.Name = "selectionProperties";
            this.selectionProperties.ScrollAlwaysVisible = true;
            this.selectionProperties.Size = new System.Drawing.Size(177, 173);
            this.selectionProperties.Sorted = true;
            this.selectionProperties.TabIndex = 4;
            this.selectionProperties.SelectedIndexChanged += new System.EventHandler(this.selectionProperties_SelectedIndexChanged);
            // 
            // selectionZone
            // 
            this.selectionZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectionZone.FormattingEnabled = true;
            this.selectionZone.Items.AddRange(new object[] {
            "Current View",
            "Current Project"});
            this.selectionZone.Location = new System.Drawing.Point(106, 26);
            this.selectionZone.Name = "selectionZone";
            this.selectionZone.Size = new System.Drawing.Size(177, 21);
            this.selectionZone.TabIndex = 1;
            this.selectionZone.SelectedIndexChanged += new System.EventHandler(this.selectionZone_SelectedIndexChanged);
            // 
            // selectionValueLabel
            // 
            this.selectionValueLabel.Location = new System.Drawing.Point(12, 315);
            this.selectionValueLabel.Name = "selectionValueLabel";
            this.selectionValueLabel.Size = new System.Drawing.Size(80, 13);
            this.selectionValueLabel.TabIndex = 12;
            this.selectionValueLabel.Text = "Value:";
            this.selectionValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // selectionNew
            // 
            this.selectionNew.Image = global::rpmBIMTools.Properties.Resources.SelectElements16;
            this.selectionNew.Location = new System.Drawing.Point(289, 25);
            this.selectionNew.Name = "selectionNew";
            this.selectionNew.Size = new System.Drawing.Size(24, 24);
            this.selectionNew.TabIndex = 2;
            this.toolTip1.SetToolTip(this.selectionNew, "Select Elements\r\n\r\nCloses the quick select dialog box so you can select the eleme" +
        "nts you wish to filter from.");
            this.selectionNew.UseVisualStyleBackColor = true;
            this.selectionNew.Click += new System.EventHandler(this.selectionNew_Click);
            // 
            // selectionTypeLabel
            // 
            this.selectionTypeLabel.Location = new System.Drawing.Point(12, 264);
            this.selectionTypeLabel.Name = "selectionTypeLabel";
            this.selectionTypeLabel.Size = new System.Drawing.Size(80, 13);
            this.selectionTypeLabel.TabIndex = 103;
            this.selectionTypeLabel.Text = "Property Type:";
            this.selectionTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // selectionType
            // 
            this.selectionType.Location = new System.Drawing.Point(103, 264);
            this.selectionType.Name = "selectionType";
            this.selectionType.Size = new System.Drawing.Size(80, 13);
            this.selectionType.TabIndex = 104;
            this.selectionType.Text = "None";
            this.selectionType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // selectionValue
            // 
            this.selectionValue.FormattingEnabled = true;
            this.selectionValue.Location = new System.Drawing.Point(106, 312);
            this.selectionValue.Name = "selectionValue";
            this.selectionValue.Size = new System.Drawing.Size(177, 21);
            this.selectionValue.Sorted = true;
            this.selectionValue.TabIndex = 6;
            this.selectionValue.SelectedIndexChanged += new System.EventHandler(this.selectionValue_SelectedIndexChanged);
            this.selectionValue.TextUpdate += new System.EventHandler(this.selectionValue_TextUpdate);
            // 
            // selectionOperator
            // 
            this.selectionOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectionOperator.FormattingEnabled = true;
            this.selectionOperator.Items.AddRange(new object[] {
            "= Equal",
            "<> Not Equal",
            "> Greater Than",
            "< Less Than",
            "Select All"});
            this.selectionOperator.Location = new System.Drawing.Point(106, 285);
            this.selectionOperator.Name = "selectionOperator";
            this.selectionOperator.Size = new System.Drawing.Size(177, 21);
            this.selectionOperator.TabIndex = 5;
            this.selectionOperator.SelectedIndexChanged += new System.EventHandler(this.selectionOperator_SelectedIndexChanged);
            // 
            // selectionApplyToBox
            // 
            this.selectionApplyToBox.Controls.Add(this.selectionApplyToExclude);
            this.selectionApplyToBox.Controls.Add(this.selectionApplyToInclude);
            this.selectionApplyToBox.Location = new System.Drawing.Point(15, 339);
            this.selectionApplyToBox.Name = "selectionApplyToBox";
            this.selectionApplyToBox.Size = new System.Drawing.Size(305, 75);
            this.selectionApplyToBox.TabIndex = 7;
            this.selectionApplyToBox.TabStop = false;
            this.selectionApplyToBox.Text = "How to select:";
            // 
            // selectionApplyToExclude
            // 
            this.selectionApplyToExclude.AutoSize = true;
            this.selectionApplyToExclude.Location = new System.Drawing.Point(14, 45);
            this.selectionApplyToExclude.Name = "selectionApplyToExclude";
            this.selectionApplyToExclude.Size = new System.Drawing.Size(171, 17);
            this.selectionApplyToExclude.TabIndex = 1;
            this.selectionApplyToExclude.Text = "Exclude from new selection set";
            this.selectionApplyToExclude.UseVisualStyleBackColor = true;
            // 
            // selectionApplyToInclude
            // 
            this.selectionApplyToInclude.AutoSize = true;
            this.selectionApplyToInclude.Location = new System.Drawing.Point(14, 22);
            this.selectionApplyToInclude.Name = "selectionApplyToInclude";
            this.selectionApplyToInclude.Size = new System.Drawing.Size(156, 17);
            this.selectionApplyToInclude.TabIndex = 0;
            this.selectionApplyToInclude.Text = "Include in new selection set";
            this.selectionApplyToInclude.UseVisualStyleBackColor = true;
            this.selectionApplyToInclude.CheckedChanged += new System.EventHandler(this.selectionApplyToIncludeExclude_CheckedChanged);
            // 
            // selectionApplyToCurrentSelectionSet
            // 
            this.selectionApplyToCurrentSelectionSet.AutoSize = true;
            this.selectionApplyToCurrentSelectionSet.Checked = global::rpmBIMTools.Properties.Settings.Default.quickSelect_selectionApplyToSelectionSet;
            this.selectionApplyToCurrentSelectionSet.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "quickSelect_selectionApplyToSelectionSet", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.selectionApplyToCurrentSelectionSet.Location = new System.Drawing.Point(23, 420);
            this.selectionApplyToCurrentSelectionSet.Name = "selectionApplyToCurrentSelectionSet";
            this.selectionApplyToCurrentSelectionSet.Size = new System.Drawing.Size(173, 17);
            this.selectionApplyToCurrentSelectionSet.TabIndex = 8;
            this.selectionApplyToCurrentSelectionSet.Text = "Append to current selection set";
            this.selectionApplyToCurrentSelectionSet.UseVisualStyleBackColor = true;
            this.selectionApplyToCurrentSelectionSet.CheckedChanged += new System.EventHandler(this.selectionApplyToCurrentSelectionSet_CheckedChanged);
            // 
            // quickSelect
            // 
            this.AcceptButton = this.selectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(334, 491);
            this.Controls.Add(this.selectionApplyToBox);
            this.Controls.Add(this.selectionApplyToCurrentSelectionSet);
            this.Controls.Add(this.selectionValue);
            this.Controls.Add(this.selectionType);
            this.Controls.Add(this.selectionTypeLabel);
            this.Controls.Add(this.selectionValueLabel);
            this.Controls.Add(this.selectionNew);
            this.Controls.Add(this.selectionProperties);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.selectionOperator);
            this.Controls.Add(this.selectionOperatorLabel);
            this.Controls.Add(this.selectionPropertiesLabel);
            this.Controls.Add(this.selectionCategory);
            this.Controls.Add(this.selectionCategoryLabel);
            this.Controls.Add(this.selectionZone);
            this.Controls.Add(this.selectionZoneLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "quickSelect";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quick Select";
            this.TopMost = true;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.helpButtonClick);
            this.Load += new System.EventHandler(this.quickSelect_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.helpRequest);
            this.selectionApplyToBox.ResumeLayout(false);
            this.selectionApplyToBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label selectionZoneLabel;
        private System.Windows.Forms.ComboBox selectionCategory;
        private System.Windows.Forms.Label selectionCategoryLabel;
        private System.Windows.Forms.Label selectionPropertiesLabel;
        private System.Windows.Forms.Label selectionOperatorLabel;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListBox selectionProperties;
        private System.Windows.Forms.Button selectionNew;
        private System.Windows.Forms.ComboBox selectionZone;
        private System.Windows.Forms.Label selectionValueLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label selectionTypeLabel;
        private System.Windows.Forms.Label selectionType;
        private System.Windows.Forms.ComboBox selectionValue;
        private System.Windows.Forms.ComboBox selectionOperator;
        private System.Windows.Forms.CheckBox selectionApplyToCurrentSelectionSet;
        private System.Windows.Forms.GroupBox selectionApplyToBox;
        private System.Windows.Forms.RadioButton selectionApplyToExclude;
        private System.Windows.Forms.RadioButton selectionApplyToInclude;
    }
}