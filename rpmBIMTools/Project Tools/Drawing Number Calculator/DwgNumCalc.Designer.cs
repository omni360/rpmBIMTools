using System;
using System.Windows.Forms;
namespace rpmBIMTools
{
    partial class DwgNumCalc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DwgNumCalc));
            this.CloseButton = new System.Windows.Forms.Button();
            this.ProjectNumberLabel = new System.Windows.Forms.Label();
            this.ProjectNumber = new System.Windows.Forms.TextBox();
            this.CopyToSheet = new System.Windows.Forms.Button();
            this.ZoneTypeLabel = new System.Windows.Forms.Label();
            this.DwgNum = new System.Windows.Forms.Label();
            this.ZoneType = new System.Windows.Forms.ComboBox();
            this.LevelType = new System.Windows.Forms.ComboBox();
            this.LevelTypeLabel = new System.Windows.Forms.Label();
            this.LevelNum = new System.Windows.Forms.NumericUpDown();
            this.DrawingType = new System.Windows.Forms.ComboBox();
            this.DrawingTypeLabel = new System.Windows.Forms.Label();
            this.ServiceType = new System.Windows.Forms.ComboBox();
            this.ServiceTypeLabel = new System.Windows.Forms.Label();
            this.SheetNum = new System.Windows.Forms.NumericUpDown();
            this.SheetNumLabel = new System.Windows.Forms.Label();
            this.CopyToClipboard = new System.Windows.Forms.Button();
            this.CopyToNewSheet = new System.Windows.Forms.Button();
            this.ZoneNum = new System.Windows.Forms.TextBox();
            this.SheetSize = new System.Windows.Forms.ComboBox();
            this.arrayButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LevelNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SheetNum)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CloseButton.Location = new System.Drawing.Point(904, 66);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(4);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(69, 39);
            this.CloseButton.TabIndex = 12;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = false;
            // 
            // ProjectNumberLabel
            // 
            this.ProjectNumberLabel.AutoSize = true;
            this.ProjectNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectNumberLabel.Location = new System.Drawing.Point(11, 7);
            this.ProjectNumberLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ProjectNumberLabel.Name = "ProjectNumberLabel";
            this.ProjectNumberLabel.Size = new System.Drawing.Size(71, 16);
            this.ProjectNumberLabel.TabIndex = 0;
            this.ProjectNumberLabel.Text = "Project No";
            // 
            // ProjectNumber
            // 
            this.ProjectNumber.BackColor = System.Drawing.SystemColors.Window;
            this.ProjectNumber.Enabled = false;
            this.ProjectNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectNumber.Location = new System.Drawing.Point(13, 30);
            this.ProjectNumber.Margin = new System.Windows.Forms.Padding(4);
            this.ProjectNumber.MaxLength = 8;
            this.ProjectNumber.Name = "ProjectNumber";
            this.ProjectNumber.Size = new System.Drawing.Size(87, 23);
            this.ProjectNumber.TabIndex = 1;
            this.ProjectNumber.TextChanged += new System.EventHandler(this.Update_DwgNum);
            // 
            // CopyToSheet
            // 
            this.CopyToSheet.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CopyToSheet.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CopyToSheet.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CopyToSheet.Location = new System.Drawing.Point(736, 66);
            this.CopyToSheet.Margin = new System.Windows.Forms.Padding(4);
            this.CopyToSheet.Name = "CopyToSheet";
            this.CopyToSheet.Size = new System.Drawing.Size(160, 39);
            this.CopyToSheet.TabIndex = 11;
            this.CopyToSheet.Text = "Apply To Current Sheet";
            this.CopyToSheet.UseVisualStyleBackColor = false;
            this.CopyToSheet.Click += new System.EventHandler(this.CopyToSheet_Click);
            // 
            // ZoneTypeLabel
            // 
            this.ZoneTypeLabel.AutoSize = true;
            this.ZoneTypeLabel.Location = new System.Drawing.Point(104, 7);
            this.ZoneTypeLabel.Name = "ZoneTypeLabel";
            this.ZoneTypeLabel.Size = new System.Drawing.Size(77, 17);
            this.ZoneTypeLabel.TabIndex = 0;
            this.ZoneTypeLabel.Text = "Zone Type";
            // 
            // DwgNum
            // 
            this.DwgNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DwgNum.Location = new System.Drawing.Point(13, 71);
            this.DwgNum.Name = "DwgNum";
            this.DwgNum.Size = new System.Drawing.Size(395, 31);
            this.DwgNum.TabIndex = 0;
            this.DwgNum.Text = "#####-NGB-##-##-##-#-####";
            this.DwgNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ZoneType
            // 
            this.ZoneType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ZoneType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ZoneType.FormattingEnabled = true;
            this.ZoneType.Items.AddRange(new object[] {
            "All Zones",
            "Zone",
            "Riser",
            "Custom"});
            this.ZoneType.Location = new System.Drawing.Point(107, 30);
            this.ZoneType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ZoneType.Name = "ZoneType";
            this.ZoneType.Size = new System.Drawing.Size(140, 24);
            this.ZoneType.TabIndex = 2;
            this.ZoneType.SelectedIndexChanged += new System.EventHandler(this.ZoneType_SelectedIndexChanged);
            // 
            // LevelType
            // 
            this.LevelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LevelType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.LevelType.FormattingEnabled = true;
            this.LevelType.IntegralHeight = false;
            this.LevelType.ItemHeight = 16;
            this.LevelType.Items.AddRange(new object[] {
            "General",
            "Basement",
            "Mezzanine",
            "Multiple Levels",
            "External",
            "Not Applicable"});
            this.LevelType.Location = new System.Drawing.Point(299, 30);
            this.LevelType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LevelType.Name = "LevelType";
            this.LevelType.Size = new System.Drawing.Size(120, 24);
            this.LevelType.TabIndex = 4;
            this.LevelType.SelectedIndexChanged += new System.EventHandler(this.LevelType_SelectedIndexChanged);
            // 
            // LevelTypeLabel
            // 
            this.LevelTypeLabel.AutoSize = true;
            this.LevelTypeLabel.Location = new System.Drawing.Point(296, 7);
            this.LevelTypeLabel.Name = "LevelTypeLabel";
            this.LevelTypeLabel.Size = new System.Drawing.Size(78, 17);
            this.LevelTypeLabel.TabIndex = 0;
            this.LevelTypeLabel.Text = "Level Type";
            // 
            // LevelNum
            // 
            this.LevelNum.Enabled = false;
            this.LevelNum.Location = new System.Drawing.Point(425, 30);
            this.LevelNum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LevelNum.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.LevelNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LevelNum.Name = "LevelNum";
            this.LevelNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LevelNum.Size = new System.Drawing.Size(40, 23);
            this.LevelNum.TabIndex = 5;
            this.LevelNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LevelNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LevelNum.ValueChanged += new System.EventHandler(this.Update_DwgNum);
            // 
            // DrawingType
            // 
            this.DrawingType.BackColor = System.Drawing.SystemColors.Window;
            this.DrawingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DrawingType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.DrawingType.FormattingEnabled = true;
            this.DrawingType.Items.AddRange(new object[] {
            "Mechanical Services",
            "Electrical Services",
            "Coordinated Services",
            "Builderswork",
            "Schematic",
            "Treatment Drawing",
            "Offsite Drawings",
            "Details & Sections",
            "Master Drawings"});
            this.DrawingType.Location = new System.Drawing.Point(471, 30);
            this.DrawingType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DrawingType.Name = "DrawingType";
            this.DrawingType.Size = new System.Drawing.Size(159, 24);
            this.DrawingType.TabIndex = 6;
            this.DrawingType.SelectedIndexChanged += new System.EventHandler(this.UpdateServiceTypes);
            // 
            // DrawingTypeLabel
            // 
            this.DrawingTypeLabel.AutoSize = true;
            this.DrawingTypeLabel.Location = new System.Drawing.Point(468, 7);
            this.DrawingTypeLabel.Name = "DrawingTypeLabel";
            this.DrawingTypeLabel.Size = new System.Drawing.Size(95, 17);
            this.DrawingTypeLabel.TabIndex = 0;
            this.DrawingTypeLabel.Text = "Drawing Type";
            // 
            // ServiceType
            // 
            this.ServiceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ServiceType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ServiceType.FormattingEnabled = true;
            this.ServiceType.Location = new System.Drawing.Point(636, 30);
            this.ServiceType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ServiceType.Name = "ServiceType";
            this.ServiceType.Size = new System.Drawing.Size(260, 24);
            this.ServiceType.TabIndex = 7;
            this.ServiceType.SelectedIndexChanged += new System.EventHandler(this.ResetSheetNumber);
            // 
            // ServiceTypeLabel
            // 
            this.ServiceTypeLabel.AutoSize = true;
            this.ServiceTypeLabel.Location = new System.Drawing.Point(633, 7);
            this.ServiceTypeLabel.Name = "ServiceTypeLabel";
            this.ServiceTypeLabel.Size = new System.Drawing.Size(91, 17);
            this.ServiceTypeLabel.TabIndex = 0;
            this.ServiceTypeLabel.Text = "Service Type";
            // 
            // SheetNum
            // 
            this.SheetNum.Location = new System.Drawing.Point(901, 30);
            this.SheetNum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SheetNum.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.SheetNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SheetNum.Name = "SheetNum";
            this.SheetNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SheetNum.Size = new System.Drawing.Size(72, 23);
            this.SheetNum.TabIndex = 8;
            this.SheetNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SheetNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SheetNum.ValueChanged += new System.EventHandler(this.Update_DwgNum);
            // 
            // SheetNumLabel
            // 
            this.SheetNumLabel.AutoSize = true;
            this.SheetNumLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SheetNumLabel.Location = new System.Drawing.Point(899, 9);
            this.SheetNumLabel.Name = "SheetNumLabel";
            this.SheetNumLabel.Size = new System.Drawing.Size(67, 17);
            this.SheetNumLabel.TabIndex = 0;
            this.SheetNumLabel.Text = "Sheet No";
            // 
            // CopyToClipboard
            // 
            this.CopyToClipboard.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CopyToClipboard.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CopyToClipboard.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CopyToClipboard.Location = new System.Drawing.Point(597, 66);
            this.CopyToClipboard.Margin = new System.Windows.Forms.Padding(4);
            this.CopyToClipboard.Name = "CopyToClipboard";
            this.CopyToClipboard.Size = new System.Drawing.Size(131, 39);
            this.CopyToClipboard.TabIndex = 9;
            this.CopyToClipboard.Text = "Copy To Clipboard";
            this.CopyToClipboard.UseVisualStyleBackColor = false;
            this.CopyToClipboard.Click += new System.EventHandler(this.CopyToClipboard_Click);
            // 
            // CopyToNewSheet
            // 
            this.CopyToNewSheet.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CopyToNewSheet.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CopyToNewSheet.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CopyToNewSheet.Location = new System.Drawing.Point(413, 66);
            this.CopyToNewSheet.Margin = new System.Windows.Forms.Padding(4);
            this.CopyToNewSheet.Name = "CopyToNewSheet";
            this.CopyToNewSheet.Size = new System.Drawing.Size(131, 39);
            this.CopyToNewSheet.TabIndex = 10;
            this.CopyToNewSheet.Text = "Create New Sheet";
            this.CopyToNewSheet.UseVisualStyleBackColor = false;
            this.CopyToNewSheet.Click += new System.EventHandler(this.CopyToNewSheet_Click);
            // 
            // ZoneNum
            // 
            this.ZoneNum.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.ZoneNum.Enabled = false;
            this.ZoneNum.Location = new System.Drawing.Point(253, 30);
            this.ZoneNum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ZoneNum.MaxLength = 1;
            this.ZoneNum.Name = "ZoneNum";
            this.ZoneNum.Size = new System.Drawing.Size(40, 23);
            this.ZoneNum.TabIndex = 3;
            this.ZoneNum.Text = "1";
            this.ZoneNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ZoneNum.TextChanged += new System.EventHandler(this.Update_DwgNum);
            // 
            // SheetSize
            // 
            this.SheetSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SheetSize.DropDownWidth = 40;
            this.SheetSize.FormattingEnabled = true;
            this.SheetSize.ItemHeight = 16;
            this.SheetSize.Items.AddRange(new object[] {
            "A0",
            "A1",
            "A2",
            "A3",
            "A4"});
            this.SheetSize.Location = new System.Drawing.Point(551, 76);
            this.SheetSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SheetSize.Name = "SheetSize";
            this.SheetSize.Size = new System.Drawing.Size(40, 24);
            this.SheetSize.TabIndex = 13;
            // 
            // arrayButton
            // 
            this.arrayButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.arrayButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.arrayButton.Location = new System.Drawing.Point(827, 66);
            this.arrayButton.Margin = new System.Windows.Forms.Padding(4);
            this.arrayButton.Name = "arrayButton";
            this.arrayButton.Size = new System.Drawing.Size(69, 39);
            this.arrayButton.TabIndex = 14;
            this.arrayButton.Text = "OK";
            this.arrayButton.UseVisualStyleBackColor = false;
            this.arrayButton.Visible = false;
            this.arrayButton.Click += new System.EventHandler(this.arrayButton_Click);
            // 
            // DwgNumCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(984, 121);
            this.Controls.Add(this.arrayButton);
            this.Controls.Add(this.SheetSize);
            this.Controls.Add(this.ZoneNum);
            this.Controls.Add(this.CopyToNewSheet);
            this.Controls.Add(this.CopyToClipboard);
            this.Controls.Add(this.SheetNumLabel);
            this.Controls.Add(this.SheetNum);
            this.Controls.Add(this.ServiceType);
            this.Controls.Add(this.ServiceTypeLabel);
            this.Controls.Add(this.DrawingTypeLabel);
            this.Controls.Add(this.DrawingType);
            this.Controls.Add(this.LevelNum);
            this.Controls.Add(this.LevelType);
            this.Controls.Add(this.LevelTypeLabel);
            this.Controls.Add(this.ZoneType);
            this.Controls.Add(this.DwgNum);
            this.Controls.Add(this.ZoneTypeLabel);
            this.Controls.Add(this.CopyToSheet);
            this.Controls.Add(this.ProjectNumber);
            this.Controls.Add(this.ProjectNumberLabel);
            this.Controls.Add(this.CloseButton);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DwgNumCalc";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Drawing Number Calculator";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.helpButtonClick);
            this.Load += new System.EventHandler(this.Form_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.helpRequest);
            ((System.ComponentModel.ISupportInitialize)(this.LevelNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SheetNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label ProjectNumberLabel;
        private System.Windows.Forms.TextBox ProjectNumber;
        private System.Windows.Forms.Button CopyToSheet;
        private System.Windows.Forms.Label ZoneTypeLabel;
        private System.Windows.Forms.Label DwgNum;
        private System.Windows.Forms.ComboBox ZoneType;
        private ComboBox LevelType;
        private Label LevelTypeLabel;
        private NumericUpDown LevelNum;
        private ComboBox DrawingType;
        private Label DrawingTypeLabel;
        private ComboBox ServiceType;
        private Label ServiceTypeLabel;
        private NumericUpDown SheetNum;
        private Label SheetNumLabel;
        private Button CopyToClipboard;
        private Button CopyToNewSheet;
        private TextBox ZoneNum;
        private ComboBox SheetSize;
        private Button arrayButton;
    }
}