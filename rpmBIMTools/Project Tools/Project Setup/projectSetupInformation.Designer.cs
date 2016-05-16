namespace rpmBIMTools
{
    partial class projectSetupInformation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(projectSetupInformation));
            this.projectNumberLabel = new System.Windows.Forms.Label();
            this.projectNumber = new System.Windows.Forms.TextBox();
            this.continueButton = new System.Windows.Forms.Button();
            this.projectNameLabel = new System.Windows.Forms.Label();
            this.projectName = new System.Windows.Forms.TextBox();
            this.projectAddressLabel = new System.Windows.Forms.Label();
            this.buildingNameLabel = new System.Windows.Forms.Label();
            this.buildingName = new System.Windows.Forms.TextBox();
            this.clientNameLabel = new System.Windows.Forms.Label();
            this.clientName = new System.Windows.Forms.TextBox();
            this.projectAddress = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.projectOMSCodeLabel = new System.Windows.Forms.Label();
            this.projectOMSCode = new System.Windows.Forms.TextBox();
            this.projectEngCodeLabel = new System.Windows.Forms.Label();
            this.projectEngCode = new System.Windows.Forms.TextBox();
            this.sheetLocation = new System.Windows.Forms.Label();
            this.NGBLocation = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // projectNumberLabel
            // 
            this.projectNumberLabel.AutoSize = true;
            this.projectNumberLabel.Location = new System.Drawing.Point(12, 15);
            this.projectNumberLabel.Name = "projectNumberLabel";
            this.projectNumberLabel.Size = new System.Drawing.Size(83, 13);
            this.projectNumberLabel.TabIndex = 0;
            this.projectNumberLabel.Text = "Project Number:";
            // 
            // projectNumber
            // 
            this.projectNumber.Location = new System.Drawing.Point(115, 12);
            this.projectNumber.Name = "projectNumber";
            this.projectNumber.Size = new System.Drawing.Size(257, 20);
            this.projectNumber.TabIndex = 1;
            // 
            // continueButton
            // 
            this.continueButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.continueButton.Location = new System.Drawing.Point(115, 263);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(80, 30);
            this.continueButton.TabIndex = 8;
            this.continueButton.Text = "Continue";
            this.continueButton.UseVisualStyleBackColor = true;
            // 
            // projectNameLabel
            // 
            this.projectNameLabel.AutoSize = true;
            this.projectNameLabel.Location = new System.Drawing.Point(12, 41);
            this.projectNameLabel.Name = "projectNameLabel";
            this.projectNameLabel.Size = new System.Drawing.Size(74, 13);
            this.projectNameLabel.TabIndex = 0;
            this.projectNameLabel.Text = "Project Name:";
            // 
            // projectName
            // 
            this.projectName.Location = new System.Drawing.Point(115, 38);
            this.projectName.Name = "projectName";
            this.projectName.Size = new System.Drawing.Size(257, 20);
            this.projectName.TabIndex = 2;
            // 
            // projectAddressLabel
            // 
            this.projectAddressLabel.AutoSize = true;
            this.projectAddressLabel.Location = new System.Drawing.Point(12, 67);
            this.projectAddressLabel.Name = "projectAddressLabel";
            this.projectAddressLabel.Size = new System.Drawing.Size(84, 13);
            this.projectAddressLabel.TabIndex = 0;
            this.projectAddressLabel.Text = "Project Address:";
            // 
            // buildingNameLabel
            // 
            this.buildingNameLabel.AutoSize = true;
            this.buildingNameLabel.Location = new System.Drawing.Point(12, 133);
            this.buildingNameLabel.Name = "buildingNameLabel";
            this.buildingNameLabel.Size = new System.Drawing.Size(78, 13);
            this.buildingNameLabel.TabIndex = 0;
            this.buildingNameLabel.Text = "Building Name:";
            // 
            // buildingName
            // 
            this.buildingName.Location = new System.Drawing.Point(115, 130);
            this.buildingName.Name = "buildingName";
            this.buildingName.Size = new System.Drawing.Size(257, 20);
            this.buildingName.TabIndex = 6;
            // 
            // clientNameLabel
            // 
            this.clientNameLabel.AutoSize = true;
            this.clientNameLabel.Location = new System.Drawing.Point(12, 159);
            this.clientNameLabel.Name = "clientNameLabel";
            this.clientNameLabel.Size = new System.Drawing.Size(67, 13);
            this.clientNameLabel.TabIndex = 0;
            this.clientNameLabel.Text = "Client Name:";
            // 
            // clientName
            // 
            this.clientName.Location = new System.Drawing.Point(115, 156);
            this.clientName.Name = "clientName";
            this.clientName.Size = new System.Drawing.Size(257, 20);
            this.clientName.TabIndex = 7;
            // 
            // projectAddress
            // 
            this.projectAddress.AcceptsReturn = true;
            this.projectAddress.Location = new System.Drawing.Point(115, 64);
            this.projectAddress.Multiline = true;
            this.projectAddress.Name = "projectAddress";
            this.projectAddress.Size = new System.Drawing.Size(257, 60);
            this.projectAddress.TabIndex = 3;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(201, 263);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 30);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // projectOMSCodeLabel
            // 
            this.projectOMSCodeLabel.AutoSize = true;
            this.projectOMSCodeLabel.Location = new System.Drawing.Point(12, 185);
            this.projectOMSCodeLabel.Name = "projectOMSCodeLabel";
            this.projectOMSCodeLabel.Size = new System.Drawing.Size(88, 13);
            this.projectOMSCodeLabel.TabIndex = 10;
            this.projectOMSCodeLabel.Text = "NGB OSM Code:";
            // 
            // projectOMSCode
            // 
            this.projectOMSCode.Location = new System.Drawing.Point(115, 182);
            this.projectOMSCode.Name = "projectOMSCode";
            this.projectOMSCode.Size = new System.Drawing.Size(257, 20);
            this.projectOMSCode.TabIndex = 11;
            // 
            // projectEngCodeLabel
            // 
            this.projectEngCodeLabel.AutoSize = true;
            this.projectEngCodeLabel.Location = new System.Drawing.Point(12, 211);
            this.projectEngCodeLabel.Name = "projectEngCodeLabel";
            this.projectEngCodeLabel.Size = new System.Drawing.Size(83, 13);
            this.projectEngCodeLabel.TabIndex = 12;
            this.projectEngCodeLabel.Text = "NGB Eng Code:";
            // 
            // projectEngCode
            // 
            this.projectEngCode.Location = new System.Drawing.Point(115, 208);
            this.projectEngCode.Name = "projectEngCode";
            this.projectEngCode.Size = new System.Drawing.Size(257, 20);
            this.projectEngCode.TabIndex = 13;
            // 
            // sheetLocation
            // 
            this.sheetLocation.AutoSize = true;
            this.sheetLocation.Location = new System.Drawing.Point(12, 237);
            this.sheetLocation.Name = "sheetLocation";
            this.sheetLocation.Size = new System.Drawing.Size(77, 13);
            this.sheetLocation.TabIndex = 14;
            this.sheetLocation.Text = "NGB Location:";
            // 
            // NGBLocation
            // 
            this.NGBLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NGBLocation.FormattingEnabled = true;
            this.NGBLocation.Location = new System.Drawing.Point(115, 234);
            this.NGBLocation.Name = "NGBLocation";
            this.NGBLocation.Size = new System.Drawing.Size(257, 21);
            this.NGBLocation.Sorted = true;
            this.NGBLocation.TabIndex = 15;
            // 
            // projectSetupInformation
            // 
            this.AcceptButton = this.continueButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(384, 302);
            this.Controls.Add(this.NGBLocation);
            this.Controls.Add(this.sheetLocation);
            this.Controls.Add(this.projectEngCodeLabel);
            this.Controls.Add(this.projectEngCode);
            this.Controls.Add(this.projectOMSCodeLabel);
            this.Controls.Add(this.projectOMSCode);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.clientNameLabel);
            this.Controls.Add(this.clientName);
            this.Controls.Add(this.projectAddressLabel);
            this.Controls.Add(this.projectAddress);
            this.Controls.Add(this.buildingNameLabel);
            this.Controls.Add(this.buildingName);
            this.Controls.Add(this.projectNameLabel);
            this.Controls.Add(this.projectName);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.projectNumberLabel);
            this.Controls.Add(this.projectNumber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "projectSetupInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Setup - Project Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label projectNumberLabel;
        public System.Windows.Forms.TextBox projectNumber;
        private System.Windows.Forms.Button continueButton;
        private System.Windows.Forms.Label projectNameLabel;
        public System.Windows.Forms.TextBox projectName;
        private System.Windows.Forms.Label projectAddressLabel;
        private System.Windows.Forms.Label buildingNameLabel;
        public System.Windows.Forms.TextBox buildingName;
        private System.Windows.Forms.Label clientNameLabel;
        public System.Windows.Forms.TextBox clientName;
        public System.Windows.Forms.TextBox projectAddress;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label projectOMSCodeLabel;
        public System.Windows.Forms.TextBox projectOMSCode;
        private System.Windows.Forms.Label projectEngCodeLabel;
        public System.Windows.Forms.TextBox projectEngCode;
        private System.Windows.Forms.Label sheetLocation;
        public System.Windows.Forms.ComboBox NGBLocation;
    }
}