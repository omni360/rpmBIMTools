namespace rpmBIMTools
{
    partial class purgeScopeBox
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(purgeScopeBox));
            this.scopeBoxGrid = new System.Windows.Forms.DataGridView();
            this.elementId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectedScopeBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.counter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewImageColumn();
            this.purgeUnusedButton = new System.Windows.Forms.Button();
            this.purgeSelectedButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.scopeBoxGridLabel = new System.Windows.Forms.Label();
            this.filterByNameLabel = new System.Windows.Forms.Label();
            this.inUseCheck = new System.Windows.Forms.CheckBox();
            this.filterByName = new System.Windows.Forms.TextBox();
            this.unusedCheck = new System.Windows.Forms.CheckBox();
            this.inUseCheckLabel = new System.Windows.Forms.Label();
            this.unusedCheckLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.scopeBoxGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // scopeBoxGrid
            // 
            this.scopeBoxGrid.AllowUserToAddRows = false;
            this.scopeBoxGrid.AllowUserToDeleteRows = false;
            this.scopeBoxGrid.AllowUserToResizeColumns = false;
            this.scopeBoxGrid.AllowUserToResizeRows = false;
            this.scopeBoxGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scopeBoxGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.elementId,
            this.selectedScopeBox,
            this.name,
            this.counter,
            this.status});
            this.scopeBoxGrid.Location = new System.Drawing.Point(12, 79);
            this.scopeBoxGrid.MultiSelect = false;
            this.scopeBoxGrid.Name = "scopeBoxGrid";
            this.scopeBoxGrid.RowHeadersVisible = false;
            this.scopeBoxGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.scopeBoxGrid.Size = new System.Drawing.Size(360, 330);
            this.scopeBoxGrid.TabIndex = 0;
            this.scopeBoxGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.checkBoxChanged);
            this.scopeBoxGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.scopeBoxGrid_CurrentCellDirtyStateChanged);
            // 
            // elementId
            // 
            this.elementId.HeaderText = "";
            this.elementId.Name = "elementId";
            this.elementId.Visible = false;
            // 
            // selectedScopeBox
            // 
            this.selectedScopeBox.HeaderText = "";
            this.selectedScopeBox.Name = "selectedScopeBox";
            this.selectedScopeBox.Width = 30;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // counter
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.counter.DefaultCellStyle = dataGridViewCellStyle1;
            this.counter.HeaderText = "";
            this.counter.Name = "counter";
            this.counter.ReadOnly = true;
            this.counter.Width = 30;
            // 
            // status
            // 
            this.status.HeaderText = "";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.status.Width = 30;
            // 
            // purgeUnusedButton
            // 
            this.purgeUnusedButton.Enabled = false;
            this.purgeUnusedButton.Location = new System.Drawing.Point(12, 420);
            this.purgeUnusedButton.Name = "purgeUnusedButton";
            this.purgeUnusedButton.Size = new System.Drawing.Size(116, 30);
            this.purgeUnusedButton.TabIndex = 1;
            this.purgeUnusedButton.Text = "Purge Unused";
            this.purgeUnusedButton.UseVisualStyleBackColor = true;
            this.purgeUnusedButton.Click += new System.EventHandler(this.purgeUnused);
            // 
            // purgeSelectedButton
            // 
            this.purgeSelectedButton.Enabled = false;
            this.purgeSelectedButton.Location = new System.Drawing.Point(134, 420);
            this.purgeSelectedButton.Name = "purgeSelectedButton";
            this.purgeSelectedButton.Size = new System.Drawing.Size(116, 30);
            this.purgeSelectedButton.TabIndex = 2;
            this.purgeSelectedButton.Text = "Purge Selected";
            this.purgeSelectedButton.UseVisualStyleBackColor = true;
            this.purgeSelectedButton.Click += new System.EventHandler(this.purgeSelected);
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(256, 420);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(116, 30);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // scopeBoxGridLabel
            // 
            this.scopeBoxGridLabel.AutoSize = true;
            this.scopeBoxGridLabel.Location = new System.Drawing.Point(9, 58);
            this.scopeBoxGridLabel.Name = "scopeBoxGridLabel";
            this.scopeBoxGridLabel.Size = new System.Drawing.Size(73, 13);
            this.scopeBoxGridLabel.TabIndex = 4;
            this.scopeBoxGridLabel.Text = "Scope Boxes:";
            // 
            // filterByNameLabel
            // 
            this.filterByNameLabel.AutoSize = true;
            this.filterByNameLabel.Location = new System.Drawing.Point(9, 9);
            this.filterByNameLabel.Name = "filterByNameLabel";
            this.filterByNameLabel.Size = new System.Drawing.Size(78, 13);
            this.filterByNameLabel.TabIndex = 5;
            this.filterByNameLabel.Text = "Filter By Name:";
            // 
            // inUseCheck
            // 
            this.inUseCheck.Checked = true;
            this.inUseCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.inUseCheck.Image = global::rpmBIMTools.Properties.Resources.LinkedElements32;
            this.inUseCheck.Location = new System.Drawing.Point(236, 27);
            this.inUseCheck.Name = "inUseCheck";
            this.inUseCheck.Size = new System.Drawing.Size(65, 44);
            this.inUseCheck.TabIndex = 6;
            this.inUseCheck.UseVisualStyleBackColor = true;
            this.inUseCheck.CheckedChanged += new System.EventHandler(this.updateFilter);
            // 
            // filterByName
            // 
            this.filterByName.Location = new System.Drawing.Point(12, 29);
            this.filterByName.Name = "filterByName";
            this.filterByName.Size = new System.Drawing.Size(200, 20);
            this.filterByName.TabIndex = 7;
            this.filterByName.TextChanged += new System.EventHandler(this.updateFilter);
            // 
            // unusedCheck
            // 
            this.unusedCheck.Checked = true;
            this.unusedCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.unusedCheck.Image = global::rpmBIMTools.Properties.Resources.ErrorElements32;
            this.unusedCheck.Location = new System.Drawing.Point(307, 27);
            this.unusedCheck.Name = "unusedCheck";
            this.unusedCheck.Size = new System.Drawing.Size(65, 44);
            this.unusedCheck.TabIndex = 8;
            this.unusedCheck.UseVisualStyleBackColor = true;
            this.unusedCheck.CheckedChanged += new System.EventHandler(this.updateFilter);
            // 
            // inUseCheckLabel
            // 
            this.inUseCheckLabel.AutoSize = true;
            this.inUseCheckLabel.Location = new System.Drawing.Point(243, 9);
            this.inUseCheckLabel.Name = "inUseCheckLabel";
            this.inUseCheckLabel.Size = new System.Drawing.Size(38, 13);
            this.inUseCheckLabel.TabIndex = 9;
            this.inUseCheckLabel.Text = "In-Use";
            // 
            // unusedCheckLabel
            // 
            this.unusedCheckLabel.AutoSize = true;
            this.unusedCheckLabel.Location = new System.Drawing.Point(311, 9);
            this.unusedCheckLabel.Name = "unusedCheckLabel";
            this.unusedCheckLabel.Size = new System.Drawing.Size(44, 13);
            this.unusedCheckLabel.TabIndex = 10;
            this.unusedCheckLabel.Text = "Unused";
            // 
            // purgeScopeBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(384, 462);
            this.Controls.Add(this.unusedCheckLabel);
            this.Controls.Add(this.inUseCheckLabel);
            this.Controls.Add(this.unusedCheck);
            this.Controls.Add(this.filterByName);
            this.Controls.Add(this.inUseCheck);
            this.Controls.Add(this.filterByNameLabel);
            this.Controls.Add(this.scopeBoxGridLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.purgeSelectedButton);
            this.Controls.Add(this.purgeUnusedButton);
            this.Controls.Add(this.scopeBoxGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "purgeScopeBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Purge Scope Box";
            this.Load += new System.EventHandler(this.purgeScopeBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.scopeBoxGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView scopeBoxGrid;
        private System.Windows.Forms.Button purgeUnusedButton;
        private System.Windows.Forms.Button purgeSelectedButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label scopeBoxGridLabel;
        private System.Windows.Forms.Label filterByNameLabel;
        private System.Windows.Forms.CheckBox inUseCheck;
        private System.Windows.Forms.TextBox filterByName;
        private System.Windows.Forms.CheckBox unusedCheck;
        private System.Windows.Forms.Label inUseCheckLabel;
        private System.Windows.Forms.Label unusedCheckLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn elementId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectedScopeBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn counter;
        private System.Windows.Forms.DataGridViewImageColumn status;
    }
}