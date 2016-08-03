namespace rpmBIMTools
{
    partial class Rename
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
            this.newLabel = new System.Windows.Forms.Label();
            this.previousLabel = new System.Windows.Forms.Label();
            this.previousText = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.confirmButton = new System.Windows.Forms.Button();
            this.newText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // newLabel
            // 
            this.newLabel.AutoSize = true;
            this.newLabel.Location = new System.Drawing.Point(12, 41);
            this.newLabel.Name = "newLabel";
            this.newLabel.Size = new System.Drawing.Size(32, 13);
            this.newLabel.TabIndex = 0;
            this.newLabel.Text = "New:";
            // 
            // previousLabel
            // 
            this.previousLabel.AutoSize = true;
            this.previousLabel.Location = new System.Drawing.Point(12, 15);
            this.previousLabel.Name = "previousLabel";
            this.previousLabel.Size = new System.Drawing.Size(51, 13);
            this.previousLabel.TabIndex = 1;
            this.previousLabel.Text = "Previous:";
            // 
            // previousText
            // 
            this.previousText.Location = new System.Drawing.Point(71, 12);
            this.previousText.MaxLength = 255;
            this.previousText.Name = "previousText";
            this.previousText.ReadOnly = true;
            this.previousText.Size = new System.Drawing.Size(201, 20);
            this.previousText.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(197, 64);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // confirmButton
            // 
            this.confirmButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.confirmButton.Enabled = false;
            this.confirmButton.Location = new System.Drawing.Point(116, 64);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(75, 23);
            this.confirmButton.TabIndex = 3;
            this.confirmButton.Text = "Ok";
            this.confirmButton.UseVisualStyleBackColor = true;
            // 
            // newText
            // 
            this.newText.Location = new System.Drawing.Point(71, 38);
            this.newText.MaxLength = 255;
            this.newText.Name = "newText";
            this.newText.Size = new System.Drawing.Size(201, 20);
            this.newText.TabIndex = 2;
            this.newText.TextChanged += new System.EventHandler(this.Filter_TextChanged);
            // 
            // Rename
            // 
            this.AcceptButton = this.confirmButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(284, 99);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.newText);
            this.Controls.Add(this.previousText);
            this.Controls.Add(this.previousLabel);
            this.Controls.Add(this.newLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Rename";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label newLabel;
        private System.Windows.Forms.Label previousLabel;
        private System.Windows.Forms.TextBox previousText;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button confirmButton;
        public System.Windows.Forms.TextBox newText;
    }
}