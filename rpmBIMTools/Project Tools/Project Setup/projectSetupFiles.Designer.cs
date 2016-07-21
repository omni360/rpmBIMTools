namespace rpmBIMTools
{
    partial class projectSetupFiles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(projectSetupFiles));
            this.cancelButton = new System.Windows.Forms.Button();
            this.setupButton = new System.Windows.Forms.Button();
            this.openFileArchitect = new System.Windows.Forms.OpenFileDialog();
            this.saveFileProject = new System.Windows.Forms.SaveFileDialog();
            this.filePathArchitect = new System.Windows.Forms.TextBox();
            this.filePathArchitectLabel = new System.Windows.Forms.Label();
            this.filePathArchitectSelect = new System.Windows.Forms.Button();
            this.filePathOutputSelect = new System.Windows.Forms.Button();
            this.filePathOutputLabel = new System.Windows.Forms.Label();
            this.filePathOutput = new System.Windows.Forms.TextBox();
            this.filePathAdditionalSelect1 = new System.Windows.Forms.Button();
            this.filePathAdditionalLabel = new System.Windows.Forms.Label();
            this.filePathAdditional1 = new System.Windows.Forms.TextBox();
            this.openFileAdditional1 = new System.Windows.Forms.OpenFileDialog();
            this.filePathAdditionalSelect2 = new System.Windows.Forms.Button();
            this.filePathAdditional2 = new System.Windows.Forms.TextBox();
            this.filePathAdditionalSelect3 = new System.Windows.Forms.Button();
            this.filePathAdditional3 = new System.Windows.Forms.TextBox();
            this.filePathAdditionalSelect4 = new System.Windows.Forms.Button();
            this.filePathAdditional4 = new System.Windows.Forms.TextBox();
            this.filePathAdditionalSelect5 = new System.Windows.Forms.Button();
            this.filePathAdditional5 = new System.Windows.Forms.TextBox();
            this.openFileAdditional2 = new System.Windows.Forms.OpenFileDialog();
            this.openFileAdditional3 = new System.Windows.Forms.OpenFileDialog();
            this.openFileAdditional4 = new System.Windows.Forms.OpenFileDialog();
            this.openFileAdditional5 = new System.Windows.Forms.OpenFileDialog();
            this.DetailLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(225, 312);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 30);
            this.cancelButton.TabIndex = 16;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // setupButton
            // 
            this.setupButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.setupButton.Enabled = false;
            this.setupButton.Location = new System.Drawing.Point(139, 312);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(80, 30);
            this.setupButton.TabIndex = 15;
            this.setupButton.Text = "Setup";
            this.setupButton.UseVisualStyleBackColor = true;
            // 
            // openFileArchitect
            // 
            this.openFileArchitect.Filter = "Revit Project Files (*.rvt)|*.rvt";
            this.openFileArchitect.Title = "Select Architect Model";
            this.openFileArchitect.FileOk += new System.ComponentModel.CancelEventHandler(this.fileChecker);
            // 
            // saveFileProject
            // 
            this.saveFileProject.Filter = "Revit Project Files (*.rvt)|*.rvt";
            this.saveFileProject.Title = "Project Output Location";
            this.saveFileProject.FileOk += new System.ComponentModel.CancelEventHandler(this.fileChecker);
            // 
            // filePathArchitect
            // 
            this.filePathArchitect.Location = new System.Drawing.Point(15, 133);
            this.filePathArchitect.Name = "filePathArchitect";
            this.filePathArchitect.ReadOnly = true;
            this.filePathArchitect.Size = new System.Drawing.Size(390, 20);
            this.filePathArchitect.TabIndex = 3;
            // 
            // filePathArchitectLabel
            // 
            this.filePathArchitectLabel.AutoSize = true;
            this.filePathArchitectLabel.Location = new System.Drawing.Point(15, 114);
            this.filePathArchitectLabel.Name = "filePathArchitectLabel";
            this.filePathArchitectLabel.Size = new System.Drawing.Size(84, 13);
            this.filePathArchitectLabel.TabIndex = 0;
            this.filePathArchitectLabel.Text = "Architect Model:";
            // 
            // filePathArchitectSelect
            // 
            this.filePathArchitectSelect.Location = new System.Drawing.Point(411, 132);
            this.filePathArchitectSelect.Name = "filePathArchitectSelect";
            this.filePathArchitectSelect.Size = new System.Drawing.Size(21, 21);
            this.filePathArchitectSelect.TabIndex = 4;
            this.filePathArchitectSelect.Text = "..";
            this.filePathArchitectSelect.UseVisualStyleBackColor = true;
            this.filePathArchitectSelect.Click += new System.EventHandler(this.filePathArchitectSelect_Click);
            // 
            // filePathOutputSelect
            // 
            this.filePathOutputSelect.Location = new System.Drawing.Point(411, 87);
            this.filePathOutputSelect.Name = "filePathOutputSelect";
            this.filePathOutputSelect.Size = new System.Drawing.Size(21, 21);
            this.filePathOutputSelect.TabIndex = 2;
            this.filePathOutputSelect.Text = "..";
            this.filePathOutputSelect.UseVisualStyleBackColor = true;
            this.filePathOutputSelect.Click += new System.EventHandler(this.filePathOutputSelect_Click);
            // 
            // filePathOutputLabel
            // 
            this.filePathOutputLabel.AutoSize = true;
            this.filePathOutputLabel.Location = new System.Drawing.Point(15, 69);
            this.filePathOutputLabel.Name = "filePathOutputLabel";
            this.filePathOutputLabel.Size = new System.Drawing.Size(167, 13);
            this.filePathOutputLabel.TabIndex = 0;
            this.filePathOutputLabel.Text = "Location and name for central file:";
            // 
            // filePathOutput
            // 
            this.filePathOutput.Location = new System.Drawing.Point(15, 88);
            this.filePathOutput.Name = "filePathOutput";
            this.filePathOutput.ReadOnly = true;
            this.filePathOutput.Size = new System.Drawing.Size(390, 20);
            this.filePathOutput.TabIndex = 1;
            // 
            // filePathAdditionalSelect1
            // 
            this.filePathAdditionalSelect1.Location = new System.Drawing.Point(411, 177);
            this.filePathAdditionalSelect1.Name = "filePathAdditionalSelect1";
            this.filePathAdditionalSelect1.Size = new System.Drawing.Size(21, 21);
            this.filePathAdditionalSelect1.TabIndex = 6;
            this.filePathAdditionalSelect1.Text = "..";
            this.filePathAdditionalSelect1.UseVisualStyleBackColor = true;
            this.filePathAdditionalSelect1.Click += new System.EventHandler(this.filePathAdditionalSelect1_Click);
            // 
            // filePathAdditionalLabel
            // 
            this.filePathAdditionalLabel.AutoSize = true;
            this.filePathAdditionalLabel.Location = new System.Drawing.Point(15, 159);
            this.filePathAdditionalLabel.Name = "filePathAdditionalLabel";
            this.filePathAdditionalLabel.Size = new System.Drawing.Size(192, 13);
            this.filePathAdditionalLabel.TabIndex = 0;
            this.filePathAdditionalLabel.Text = "Additional Models: (leave blank if none)";
            // 
            // filePathAdditional1
            // 
            this.filePathAdditional1.Location = new System.Drawing.Point(15, 178);
            this.filePathAdditional1.Name = "filePathAdditional1";
            this.filePathAdditional1.ReadOnly = true;
            this.filePathAdditional1.Size = new System.Drawing.Size(390, 20);
            this.filePathAdditional1.TabIndex = 5;
            // 
            // openFileAdditional1
            // 
            this.openFileAdditional1.Filter = "Revit Project Files (*.rvt)|*.rvt";
            this.openFileAdditional1.Multiselect = true;
            this.openFileAdditional1.Title = "Select Additional Models";
            // 
            // filePathAdditionalSelect2
            // 
            this.filePathAdditionalSelect2.Location = new System.Drawing.Point(411, 203);
            this.filePathAdditionalSelect2.Name = "filePathAdditionalSelect2";
            this.filePathAdditionalSelect2.Size = new System.Drawing.Size(21, 21);
            this.filePathAdditionalSelect2.TabIndex = 8;
            this.filePathAdditionalSelect2.Text = "..";
            this.filePathAdditionalSelect2.UseVisualStyleBackColor = true;
            this.filePathAdditionalSelect2.Click += new System.EventHandler(this.filePathAdditionalSelect2_Click);
            // 
            // filePathAdditional2
            // 
            this.filePathAdditional2.Location = new System.Drawing.Point(15, 204);
            this.filePathAdditional2.Name = "filePathAdditional2";
            this.filePathAdditional2.ReadOnly = true;
            this.filePathAdditional2.Size = new System.Drawing.Size(390, 20);
            this.filePathAdditional2.TabIndex = 7;
            // 
            // filePathAdditionalSelect3
            // 
            this.filePathAdditionalSelect3.Location = new System.Drawing.Point(411, 229);
            this.filePathAdditionalSelect3.Name = "filePathAdditionalSelect3";
            this.filePathAdditionalSelect3.Size = new System.Drawing.Size(21, 21);
            this.filePathAdditionalSelect3.TabIndex = 10;
            this.filePathAdditionalSelect3.Text = "..";
            this.filePathAdditionalSelect3.UseVisualStyleBackColor = true;
            this.filePathAdditionalSelect3.Click += new System.EventHandler(this.filePathAdditionalSelect3_Click);
            // 
            // filePathAdditional3
            // 
            this.filePathAdditional3.Location = new System.Drawing.Point(15, 230);
            this.filePathAdditional3.Name = "filePathAdditional3";
            this.filePathAdditional3.ReadOnly = true;
            this.filePathAdditional3.Size = new System.Drawing.Size(390, 20);
            this.filePathAdditional3.TabIndex = 9;
            // 
            // filePathAdditionalSelect4
            // 
            this.filePathAdditionalSelect4.Location = new System.Drawing.Point(411, 255);
            this.filePathAdditionalSelect4.Name = "filePathAdditionalSelect4";
            this.filePathAdditionalSelect4.Size = new System.Drawing.Size(21, 21);
            this.filePathAdditionalSelect4.TabIndex = 12;
            this.filePathAdditionalSelect4.Text = "..";
            this.filePathAdditionalSelect4.UseVisualStyleBackColor = true;
            this.filePathAdditionalSelect4.Click += new System.EventHandler(this.filePathAdditionalSelect4_Click);
            // 
            // filePathAdditional4
            // 
            this.filePathAdditional4.Location = new System.Drawing.Point(15, 256);
            this.filePathAdditional4.Name = "filePathAdditional4";
            this.filePathAdditional4.ReadOnly = true;
            this.filePathAdditional4.Size = new System.Drawing.Size(390, 20);
            this.filePathAdditional4.TabIndex = 11;
            // 
            // filePathAdditionalSelect5
            // 
            this.filePathAdditionalSelect5.Location = new System.Drawing.Point(411, 281);
            this.filePathAdditionalSelect5.Name = "filePathAdditionalSelect5";
            this.filePathAdditionalSelect5.Size = new System.Drawing.Size(21, 21);
            this.filePathAdditionalSelect5.TabIndex = 14;
            this.filePathAdditionalSelect5.Text = "..";
            this.filePathAdditionalSelect5.UseVisualStyleBackColor = true;
            this.filePathAdditionalSelect5.Click += new System.EventHandler(this.filePathAdditionalSelect5_Click);
            // 
            // filePathAdditional5
            // 
            this.filePathAdditional5.Location = new System.Drawing.Point(15, 282);
            this.filePathAdditional5.Name = "filePathAdditional5";
            this.filePathAdditional5.ReadOnly = true;
            this.filePathAdditional5.Size = new System.Drawing.Size(390, 20);
            this.filePathAdditional5.TabIndex = 13;
            // 
            // openFileAdditional2
            // 
            this.openFileAdditional2.Filter = "Revit Project Files (*.rvt)|*.rvt";
            this.openFileAdditional2.Multiselect = true;
            this.openFileAdditional2.Title = "Select Additional Models";
            // 
            // openFileAdditional3
            // 
            this.openFileAdditional3.Filter = "Revit Project Files (*.rvt)|*.rvt";
            this.openFileAdditional3.Multiselect = true;
            this.openFileAdditional3.Title = "Select Additional Models";
            // 
            // openFileAdditional4
            // 
            this.openFileAdditional4.Filter = "Revit Project Files (*.rvt)|*.rvt";
            this.openFileAdditional4.Multiselect = true;
            this.openFileAdditional4.Title = "Select Additional Models";
            // 
            // openFileAdditional5
            // 
            this.openFileAdditional5.Filter = "Revit Project Files (*.rvt)|*.rvt";
            this.openFileAdditional5.Multiselect = true;
            this.openFileAdditional5.Title = "Select Additional Models";
            // 
            // DetailLabel
            // 
            this.DetailLabel.Location = new System.Drawing.Point(15, 17);
            this.DetailLabel.Name = "DetailLabel";
            this.DetailLabel.Size = new System.Drawing.Size(417, 39);
            this.DetailLabel.TabIndex = 17;
            this.DetailLabel.Text = resources.GetString("DetailLabel.Text");
            // 
            // projectSetupFiles
            // 
            this.AcceptButton = this.setupButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(444, 352);
            this.Controls.Add(this.DetailLabel);
            this.Controls.Add(this.filePathAdditionalSelect5);
            this.Controls.Add(this.filePathAdditional5);
            this.Controls.Add(this.filePathAdditionalSelect4);
            this.Controls.Add(this.filePathAdditional4);
            this.Controls.Add(this.filePathAdditionalSelect3);
            this.Controls.Add(this.filePathAdditional3);
            this.Controls.Add(this.filePathAdditionalSelect2);
            this.Controls.Add(this.filePathAdditional2);
            this.Controls.Add(this.filePathAdditionalSelect1);
            this.Controls.Add(this.filePathAdditionalLabel);
            this.Controls.Add(this.filePathAdditional1);
            this.Controls.Add(this.filePathOutputSelect);
            this.Controls.Add(this.filePathOutputLabel);
            this.Controls.Add(this.filePathOutput);
            this.Controls.Add(this.filePathArchitectSelect);
            this.Controls.Add(this.filePathArchitectLabel);
            this.Controls.Add(this.filePathArchitect);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "projectSetupFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Setup";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.helpButtonClick);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.helpRequest);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button setupButton;
        private System.Windows.Forms.OpenFileDialog openFileArchitect;
        private System.Windows.Forms.SaveFileDialog saveFileProject;
        public System.Windows.Forms.TextBox filePathArchitect;
        private System.Windows.Forms.Label filePathArchitectLabel;
        private System.Windows.Forms.Button filePathArchitectSelect;
        private System.Windows.Forms.Button filePathOutputSelect;
        private System.Windows.Forms.Label filePathOutputLabel;
        public System.Windows.Forms.TextBox filePathOutput;
        private System.Windows.Forms.Button filePathAdditionalSelect1;
        private System.Windows.Forms.Label filePathAdditionalLabel;
        public System.Windows.Forms.TextBox filePathAdditional1;
        public System.Windows.Forms.OpenFileDialog openFileAdditional1;
        private System.Windows.Forms.Button filePathAdditionalSelect2;
        public System.Windows.Forms.TextBox filePathAdditional2;
        private System.Windows.Forms.Button filePathAdditionalSelect3;
        public System.Windows.Forms.TextBox filePathAdditional3;
        private System.Windows.Forms.Button filePathAdditionalSelect4;
        public System.Windows.Forms.TextBox filePathAdditional4;
        private System.Windows.Forms.Button filePathAdditionalSelect5;
        public System.Windows.Forms.TextBox filePathAdditional5;
        public System.Windows.Forms.OpenFileDialog openFileAdditional2;
        public System.Windows.Forms.OpenFileDialog openFileAdditional3;
        public System.Windows.Forms.OpenFileDialog openFileAdditional4;
        public System.Windows.Forms.OpenFileDialog openFileAdditional5;
        private System.Windows.Forms.Label DetailLabel;
    }
}