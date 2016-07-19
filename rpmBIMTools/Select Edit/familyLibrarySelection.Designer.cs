namespace rpmBIMTools
{
    partial class familyLibrarySelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(familyLibrarySelection));
            this.insertButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.search = new System.Windows.Forms.TextBox();
            this.searchLabel = new System.Windows.Forms.Label();
            this.FamilyTree = new System.Windows.Forms.TreeView();
            this.familyView = new System.Windows.Forms.Panel();
            this.pictureToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.loadAllTypes = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // insertButton
            // 
            this.insertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.insertButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.insertButton.Enabled = false;
            this.insertButton.Location = new System.Drawing.Point(12, 505);
            this.insertButton.Name = "insertButton";
            this.insertButton.Size = new System.Drawing.Size(100, 30);
            this.insertButton.TabIndex = 0;
            this.insertButton.Text = "Insert Family";
            this.insertButton.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(278, 505);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(100, 30);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // search
            // 
            this.search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.search.Location = new System.Drawing.Point(62, 12);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(316, 20);
            this.search.TabIndex = 2;
            this.search.TextChanged += new System.EventHandler(this.search_TextChanged);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(12, 15);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(44, 13);
            this.searchLabel.TabIndex = 3;
            this.searchLabel.Text = "Search:";
            // 
            // FamilyTree
            // 
            this.FamilyTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FamilyTree.HideSelection = false;
            this.FamilyTree.Location = new System.Drawing.Point(12, 38);
            this.FamilyTree.Name = "FamilyTree";
            this.FamilyTree.Size = new System.Drawing.Size(366, 461);
            this.FamilyTree.TabIndex = 6;
            this.FamilyTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FamilyTree_AfterSelect);
            this.FamilyTree.DoubleClick += new System.EventHandler(this.familySelected);
            // 
            // familyView
            // 
            this.familyView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familyView.AutoScroll = true;
            this.familyView.Location = new System.Drawing.Point(384, 12);
            this.familyView.Name = "familyView";
            this.familyView.Size = new System.Drawing.Size(438, 523);
            this.familyView.TabIndex = 7;
            // 
            // loadAllTypes
            // 
            this.loadAllTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadAllTypes.AutoSize = true;
            this.loadAllTypes.Checked = global::rpmBIMTools.Properties.Settings.Default.familyLibrary_allTypes;
            this.loadAllTypes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadAllTypes.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "familyLibrary_allTypes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.loadAllTypes.Location = new System.Drawing.Point(118, 513);
            this.loadAllTypes.Name = "loadAllTypes";
            this.loadAllTypes.Size = new System.Drawing.Size(96, 17);
            this.loadAllTypes.TabIndex = 8;
            this.loadAllTypes.Text = "Load All Types";
            this.loadAllTypes.UseVisualStyleBackColor = true;
            // 
            // familyLibrarySelection
            // 
            this.AcceptButton = this.insertButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(834, 547);
            this.Controls.Add(this.loadAllTypes);
            this.Controls.Add(this.familyView);
            this.Controls.Add(this.FamilyTree);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.search);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.insertButton);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(850, 585);
            this.Name = "familyLibrarySelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Family Library";
            this.Load += new System.EventHandler(this.familyLibrarySelection_Load);
            this.SizeChanged += new System.EventHandler(this.familyLibrarySelection_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button insertButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TextBox search;
        private System.Windows.Forms.Label searchLabel;
        public System.Windows.Forms.TreeView FamilyTree;
        private System.Windows.Forms.Panel familyView;
        public System.Windows.Forms.CheckBox loadAllTypes;
        private System.Windows.Forms.ToolTip pictureToolTip;
    }
}