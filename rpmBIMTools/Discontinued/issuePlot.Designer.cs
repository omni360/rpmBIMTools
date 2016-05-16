namespace rpmBIMTools
{
    partial class issuePlot
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
            this.sheetSelectionList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sheetListBoxBox = new System.Windows.Forms.GroupBox();
            this.startCheckPlot = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.filterBox = new System.Windows.Forms.GroupBox();
            this.filterSelectionList = new System.Windows.Forms.ComboBox();
            this.selectionBox = new System.Windows.Forms.GroupBox();
            this.selectInvert = new System.Windows.Forms.Button();
            this.selectNone = new System.Windows.Forms.Button();
            this.selectAll = new System.Windows.Forms.Button();
            this.startIssuePlot = new System.Windows.Forms.Button();
            this.toPlotBox = new System.Windows.Forms.GroupBox();
            this.plotSelectionList = new System.Windows.Forms.ListBox();
            this.listTransferRight = new System.Windows.Forms.Button();
            this.listTransferLeft = new System.Windows.Forms.Button();
            this.sheetListBoxBox.SuspendLayout();
            this.filterBox.SuspendLayout();
            this.selectionBox.SuspendLayout();
            this.toPlotBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // sheetSelectionList
            // 
            this.sheetSelectionList.FormattingEnabled = true;
            this.sheetSelectionList.ItemHeight = 16;
            this.sheetSelectionList.Location = new System.Drawing.Point(13, 25);
            this.sheetSelectionList.Name = "sheetSelectionList";
            this.sheetSelectionList.ScrollAlwaysVisible = true;
            this.sheetSelectionList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.sheetSelectionList.Size = new System.Drawing.Size(224, 404);
            this.sheetSelectionList.TabIndex = 1;
            this.sheetSelectionList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listTransferRight_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(151, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(553, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "------------------------ IssuePLOT ------------------------";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            // 
            // sheetListBoxBox
            // 
            this.sheetListBoxBox.Controls.Add(this.sheetSelectionList);
            this.sheetListBoxBox.Location = new System.Drawing.Point(151, 34);
            this.sheetListBoxBox.Name = "sheetListBoxBox";
            this.sheetListBoxBox.Padding = new System.Windows.Forms.Padding(10);
            this.sheetListBoxBox.Size = new System.Drawing.Size(250, 445);
            this.sheetListBoxBox.TabIndex = 0;
            this.sheetListBoxBox.TabStop = false;
            this.sheetListBoxBox.Text = "Sheet List";
            // 
            // startCheckPlot
            // 
            this.startCheckPlot.Location = new System.Drawing.Point(422, 510);
            this.startCheckPlot.Name = "startCheckPlot";
            this.startCheckPlot.Size = new System.Drawing.Size(90, 40);
            this.startCheckPlot.TabIndex = 7;
            this.startCheckPlot.Text = "CheckPlot";
            this.startCheckPlot.UseVisualStyleBackColor = true;
            this.startCheckPlot.Click += new System.EventHandler(this.startCheckPlot_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(614, 510);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(90, 40);
            this.cancel.TabIndex = 9;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // filterBox
            // 
            this.filterBox.Controls.Add(this.filterSelectionList);
            this.filterBox.Location = new System.Drawing.Point(151, 485);
            this.filterBox.Name = "filterBox";
            this.filterBox.Padding = new System.Windows.Forms.Padding(10);
            this.filterBox.Size = new System.Drawing.Size(250, 65);
            this.filterBox.TabIndex = 5;
            this.filterBox.TabStop = false;
            this.filterBox.Text = "Service Filter";
            // 
            // filterSelectionList
            // 
            this.filterSelectionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterSelectionList.Location = new System.Drawing.Point(13, 25);
            this.filterSelectionList.Name = "filterSelectionList";
            this.filterSelectionList.Size = new System.Drawing.Size(224, 24);
            this.filterSelectionList.TabIndex = 4;
            this.filterSelectionList.SelectedIndexChanged += new System.EventHandler(this.filterList_SelectedIndexChanged);
            // 
            // selectionBox
            // 
            this.selectionBox.Controls.Add(this.selectInvert);
            this.selectionBox.Controls.Add(this.selectNone);
            this.selectionBox.Controls.Add(this.selectAll);
            this.selectionBox.Location = new System.Drawing.Point(12, 34);
            this.selectionBox.Name = "selectionBox";
            this.selectionBox.Padding = new System.Windows.Forms.Padding(10);
            this.selectionBox.Size = new System.Drawing.Size(125, 143);
            this.selectionBox.TabIndex = 6;
            this.selectionBox.TabStop = false;
            this.selectionBox.Text = "Selection";
            // 
            // selectInvert
            // 
            this.selectInvert.Location = new System.Drawing.Point(13, 97);
            this.selectInvert.Name = "selectInvert";
            this.selectInvert.Size = new System.Drawing.Size(100, 30);
            this.selectInvert.TabIndex = 3;
            this.selectInvert.Text = "Invert";
            this.selectInvert.UseVisualStyleBackColor = true;
            this.selectInvert.Click += new System.EventHandler(this.selectInvert_Click);
            // 
            // selectNone
            // 
            this.selectNone.Location = new System.Drawing.Point(13, 61);
            this.selectNone.Name = "selectNone";
            this.selectNone.Size = new System.Drawing.Size(100, 30);
            this.selectNone.TabIndex = 2;
            this.selectNone.Text = "Clear All";
            this.selectNone.UseVisualStyleBackColor = true;
            this.selectNone.Click += new System.EventHandler(this.selectNone_Click);
            // 
            // selectAll
            // 
            this.selectAll.Location = new System.Drawing.Point(13, 25);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(100, 30);
            this.selectAll.TabIndex = 1;
            this.selectAll.Text = "Select All";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // startIssuePlot
            // 
            this.startIssuePlot.Location = new System.Drawing.Point(518, 510);
            this.startIssuePlot.Name = "startIssuePlot";
            this.startIssuePlot.Size = new System.Drawing.Size(90, 40);
            this.startIssuePlot.TabIndex = 8;
            this.startIssuePlot.Text = "IssuePlot";
            this.startIssuePlot.UseVisualStyleBackColor = true;
            this.startIssuePlot.Click += new System.EventHandler(this.startIssuePlot_Click);
            // 
            // toPlotBox
            // 
            this.toPlotBox.Controls.Add(this.plotSelectionList);
            this.toPlotBox.Location = new System.Drawing.Point(454, 34);
            this.toPlotBox.Name = "toPlotBox";
            this.toPlotBox.Padding = new System.Windows.Forms.Padding(10);
            this.toPlotBox.Size = new System.Drawing.Size(250, 445);
            this.toPlotBox.TabIndex = 0;
            this.toPlotBox.TabStop = false;
            this.toPlotBox.Text = "To Plot";
            // 
            // plotSelectionList
            // 
            this.plotSelectionList.FormattingEnabled = true;
            this.plotSelectionList.ItemHeight = 16;
            this.plotSelectionList.Location = new System.Drawing.Point(13, 25);
            this.plotSelectionList.Name = "plotSelectionList";
            this.plotSelectionList.ScrollAlwaysVisible = true;
            this.plotSelectionList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.plotSelectionList.Size = new System.Drawing.Size(224, 404);
            this.plotSelectionList.TabIndex = 1;
            this.plotSelectionList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.toPlotSelectionList_DoubleClick);
            // 
            // listTransferRight
            // 
            this.listTransferRight.Location = new System.Drawing.Point(407, 208);
            this.listTransferRight.Name = "listTransferRight";
            this.listTransferRight.Size = new System.Drawing.Size(41, 30);
            this.listTransferRight.TabIndex = 10;
            this.listTransferRight.Text = ">";
            this.listTransferRight.UseVisualStyleBackColor = true;
            this.listTransferRight.Click += new System.EventHandler(this.listTransferRight_Click);
            // 
            // listTransferLeft
            // 
            this.listTransferLeft.Location = new System.Drawing.Point(407, 244);
            this.listTransferLeft.Name = "listTransferLeft";
            this.listTransferLeft.Size = new System.Drawing.Size(41, 30);
            this.listTransferLeft.TabIndex = 11;
            this.listTransferLeft.Text = "<";
            this.listTransferLeft.UseVisualStyleBackColor = true;
            this.listTransferLeft.Click += new System.EventHandler(this.listTransferLeft_Click);
            // 
            // issuePlot
            // 
            this.AcceptButton = this.startCheckPlot;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(716, 562);
            this.ControlBox = false;
            this.Controls.Add(this.listTransferLeft);
            this.Controls.Add(this.listTransferRight);
            this.Controls.Add(this.toPlotBox);
            this.Controls.Add(this.startIssuePlot);
            this.Controls.Add(this.selectionBox);
            this.Controls.Add(this.filterBox);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.startCheckPlot);
            this.Controls.Add(this.sheetListBoxBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "issuePlot";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CheckPlot";
            this.Load += new System.EventHandler(this.issuePlot_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form_MouseUp);
            this.sheetListBoxBox.ResumeLayout(false);
            this.filterBox.ResumeLayout(false);
            this.selectionBox.ResumeLayout(false);
            this.toPlotBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox sheetSelectionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox sheetListBoxBox;
        private System.Windows.Forms.Button startCheckPlot;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox filterBox;
        private System.Windows.Forms.ComboBox filterSelectionList;
        private System.Windows.Forms.GroupBox selectionBox;
        private System.Windows.Forms.Button selectAll;
        private System.Windows.Forms.Button selectInvert;
        private System.Windows.Forms.Button selectNone;
        private System.Windows.Forms.Button startIssuePlot;
        private System.Windows.Forms.GroupBox toPlotBox;
        private System.Windows.Forms.ListBox plotSelectionList;
        private System.Windows.Forms.Button listTransferRight;
        private System.Windows.Forms.Button listTransferLeft;



    }
}