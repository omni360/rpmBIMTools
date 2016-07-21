namespace rpmBIMTools
{
    partial class projectSetupElements
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(projectSetupElements));
            this.cancelButton = new System.Windows.Forms.Button();
            this.continueButton = new System.Windows.Forms.Button();
            this.grid = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.elevation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.copy = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label = new System.Windows.Forms.Label();
            this.copyGrids = new System.Windows.Forms.CheckBox();
            this.copySpaces = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(292, 240);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 30);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // continueButton
            // 
            this.continueButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.continueButton.Location = new System.Drawing.Point(206, 240);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(80, 30);
            this.continueButton.TabIndex = 4;
            this.continueButton.Text = "Continue";
            this.continueButton.UseVisualStyleBackColor = true;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeColumns = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.name,
            this.elevation,
            this.copy});
            this.grid.Location = new System.Drawing.Point(12, 27);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(360, 200);
            this.grid.TabIndex = 1;
            // 
            // id
            // 
            this.id.HeaderText = "Id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.id.Visible = false;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // elevation
            // 
            this.elevation.HeaderText = "Elevation";
            this.elevation.Name = "elevation";
            this.elevation.ReadOnly = true;
            this.elevation.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.elevation.Width = 70;
            // 
            // copy
            // 
            this.copy.FalseValue = "False";
            this.copy.HeaderText = "Copy";
            this.copy.Name = "copy";
            this.copy.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.copy.TrueValue = "True";
            this.copy.Width = 50;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(86, 13);
            this.label.TabIndex = 0;
            this.label.Text = "Architect Levels:";
            // 
            // copyGrids
            // 
            this.copyGrids.AutoSize = true;
            this.copyGrids.Checked = true;
            this.copyGrids.CheckState = System.Windows.Forms.CheckState.Checked;
            this.copyGrids.Location = new System.Drawing.Point(12, 235);
            this.copyGrids.Name = "copyGrids";
            this.copyGrids.Size = new System.Drawing.Size(93, 17);
            this.copyGrids.TabIndex = 2;
            this.copyGrids.Text = "Copy Gridlines";
            this.copyGrids.UseVisualStyleBackColor = true;
            // 
            // copySpaces
            // 
            this.copySpaces.AutoSize = true;
            this.copySpaces.Checked = true;
            this.copySpaces.CheckState = System.Windows.Forms.CheckState.Checked;
            this.copySpaces.Location = new System.Drawing.Point(12, 258);
            this.copySpaces.Name = "copySpaces";
            this.copySpaces.Size = new System.Drawing.Size(150, 17);
            this.copySpaces.TabIndex = 3;
            this.copySpaces.Text = "Convert Rooms to Spaces";
            this.copySpaces.UseVisualStyleBackColor = true;
            // 
            // projectSetupElements
            // 
            this.AcceptButton = this.continueButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(384, 282);
            this.Controls.Add(this.copySpaces);
            this.Controls.Add(this.copyGrids);
            this.Controls.Add(this.label);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.continueButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "projectSetupElements";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Setup - Copy Elements";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.helpButtonClick);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.helpRequest);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button continueButton;
        public System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn elevation;
        private System.Windows.Forms.DataGridViewCheckBoxColumn copy;
        public System.Windows.Forms.CheckBox copyGrids;
        public System.Windows.Forms.CheckBox copySpaces;
    }
}