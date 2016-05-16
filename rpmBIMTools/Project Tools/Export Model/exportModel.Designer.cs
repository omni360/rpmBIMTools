namespace rpmBIMTools
{
    partial class exportModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(exportModel));
            this.exportButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.exportToLabel = new System.Windows.Forms.Label();
            this.exportToButton = new System.Windows.Forms.Button();
            this.purgeViews_Group_PlanViews = new System.Windows.Forms.GroupBox();
            this.includeView_AreaPlans = new System.Windows.Forms.CheckBox();
            this.includeView_StructuredPlans = new System.Windows.Forms.CheckBox();
            this.includeView_CeilingPlans = new System.Windows.Forms.CheckBox();
            this.includeView_FloorPlans = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.exportOptions = new System.Windows.Forms.TabPage();
            this.exportOptions_Group_LandingPage = new System.Windows.Forms.GroupBox();
            this.exportOptions_LandingPage = new System.Windows.Forms.ComboBox();
            this.exportOptions_Group_Compression = new System.Windows.Forms.GroupBox();
            this.exportOptions_Compression = new System.Windows.Forms.CheckBox();
            this.exportOptions_Group_Worksharing = new System.Windows.Forms.GroupBox();
            this.exportOptions_Label = new System.Windows.Forms.Label();
            this.exportOptions_DisableWorksharing = new System.Windows.Forms.CheckBox();
            this.exportOptions_Group_Clean = new System.Windows.Forms.GroupBox();
            this.exportOptions_IncludeViewsAll = new System.Windows.Forms.RadioButton();
            this.exportOptions_IncludeSheets = new System.Windows.Forms.CheckBox();
            this.exportOptions_IncludeViewsSelected = new System.Windows.Forms.RadioButton();
            this.exportOptions_Audit = new System.Windows.Forms.CheckBox();
            this.exportOptions_IncludeViewsOnSheets = new System.Windows.Forms.RadioButton();
            this.exportOptions_PurgeUnused = new System.Windows.Forms.CheckBox();
            this.exportOptions_Group_AddFiles = new System.Windows.Forms.GroupBox();
            this.exportOptions_IncludeExportReport = new System.Windows.Forms.CheckBox();
            this.exportOptions_IncludeDWFLinks = new System.Windows.Forms.CheckBox();
            this.exportOptions_IncludeDecals = new System.Windows.Forms.CheckBox();
            this.exportOptions_IncludeCADLinks = new System.Windows.Forms.CheckBox();
            this.exportOptions_IncludeRevitLinks = new System.Windows.Forms.CheckBox();
            this.purgeViews = new System.Windows.Forms.TabPage();
            this.purgeViews_SelectAll = new System.Windows.Forms.Button();
            this.purgeViews_ClearAll = new System.Windows.Forms.Button();
            this.purgeViews_Group_scheduleViews = new System.Windows.Forms.GroupBox();
            this.includeView_ColumnSchedules = new System.Windows.Forms.CheckBox();
            this.includeView_PanelSchedules = new System.Windows.Forms.CheckBox();
            this.includeView_Schedules = new System.Windows.Forms.CheckBox();
            this.purgeViews_Group_2DViews = new System.Windows.Forms.GroupBox();
            this.includeView_Legends = new System.Windows.Forms.CheckBox();
            this.includeView_DraftingViews = new System.Windows.Forms.CheckBox();
            this.includeView_Details = new System.Windows.Forms.CheckBox();
            this.includeView_Elevations = new System.Windows.Forms.CheckBox();
            this.includeView_Sections = new System.Windows.Forms.CheckBox();
            this.purgeViews_Group_3DViews = new System.Windows.Forms.GroupBox();
            this.includeView_Walkthroughs = new System.Windows.Forms.CheckBox();
            this.includeView_Renderings = new System.Windows.Forms.CheckBox();
            this.includeView_3DViews = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.exportTo = new System.Windows.Forms.TextBox();
            this.saveSettings = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.purgeViews_Group_PlanViews.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.exportOptions.SuspendLayout();
            this.exportOptions_Group_LandingPage.SuspendLayout();
            this.exportOptions_Group_Compression.SuspendLayout();
            this.exportOptions_Group_Worksharing.SuspendLayout();
            this.exportOptions_Group_Clean.SuspendLayout();
            this.exportOptions_Group_AddFiles.SuspendLayout();
            this.purgeViews.SuspendLayout();
            this.purgeViews_Group_scheduleViews.SuspendLayout();
            this.purgeViews_Group_2DViews.SuspendLayout();
            this.purgeViews_Group_3DViews.SuspendLayout();
            this.SuspendLayout();
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(206, 490);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(80, 30);
            this.exportButton.TabIndex = 5;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(292, 490);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 30);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // exportToLabel
            // 
            this.exportToLabel.AutoSize = true;
            this.exportToLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportToLabel.Location = new System.Drawing.Point(12, 9);
            this.exportToLabel.Name = "exportToLabel";
            this.exportToLabel.Size = new System.Drawing.Size(62, 15);
            this.exportToLabel.TabIndex = 2;
            this.exportToLabel.Text = "Export To:";
            // 
            // exportToButton
            // 
            this.exportToButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.exportToButton.Location = new System.Drawing.Point(351, 30);
            this.exportToButton.Name = "exportToButton";
            this.exportToButton.Size = new System.Drawing.Size(21, 21);
            this.exportToButton.TabIndex = 2;
            this.exportToButton.Text = "...";
            this.exportToButton.UseVisualStyleBackColor = true;
            this.exportToButton.Click += new System.EventHandler(this.exportToButton_Click);
            // 
            // purgeViews_Group_PlanViews
            // 
            this.purgeViews_Group_PlanViews.Controls.Add(this.includeView_AreaPlans);
            this.purgeViews_Group_PlanViews.Controls.Add(this.includeView_StructuredPlans);
            this.purgeViews_Group_PlanViews.Controls.Add(this.includeView_CeilingPlans);
            this.purgeViews_Group_PlanViews.Controls.Add(this.includeView_FloorPlans);
            this.purgeViews_Group_PlanViews.Location = new System.Drawing.Point(9, 6);
            this.purgeViews_Group_PlanViews.Name = "purgeViews_Group_PlanViews";
            this.purgeViews_Group_PlanViews.Size = new System.Drawing.Size(162, 125);
            this.purgeViews_Group_PlanViews.TabIndex = 1;
            this.purgeViews_Group_PlanViews.TabStop = false;
            this.purgeViews_Group_PlanViews.Text = "Plan Views";
            // 
            // includeView_AreaPlans
            // 
            this.includeView_AreaPlans.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_AreaPlans;
            this.includeView_AreaPlans.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_AreaPlans", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_AreaPlans.Location = new System.Drawing.Point(11, 98);
            this.includeView_AreaPlans.Name = "includeView_AreaPlans";
            this.includeView_AreaPlans.Size = new System.Drawing.Size(135, 19);
            this.includeView_AreaPlans.TabIndex = 4;
            this.includeView_AreaPlans.Text = "Area Plans";
            this.includeView_AreaPlans.UseVisualStyleBackColor = true;
            // 
            // includeView_StructuredPlans
            // 
            this.includeView_StructuredPlans.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_StructuredPlans;
            this.includeView_StructuredPlans.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_StructuredPlans", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_StructuredPlans.Location = new System.Drawing.Point(11, 73);
            this.includeView_StructuredPlans.Name = "includeView_StructuredPlans";
            this.includeView_StructuredPlans.Size = new System.Drawing.Size(135, 19);
            this.includeView_StructuredPlans.TabIndex = 3;
            this.includeView_StructuredPlans.Text = "Structured Plans";
            this.includeView_StructuredPlans.UseVisualStyleBackColor = true;
            // 
            // includeView_CeilingPlans
            // 
            this.includeView_CeilingPlans.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_CeilingPlans;
            this.includeView_CeilingPlans.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_CeilingPlans", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_CeilingPlans.Location = new System.Drawing.Point(11, 48);
            this.includeView_CeilingPlans.Name = "includeView_CeilingPlans";
            this.includeView_CeilingPlans.Size = new System.Drawing.Size(135, 19);
            this.includeView_CeilingPlans.TabIndex = 2;
            this.includeView_CeilingPlans.Text = "Ceiling Plans";
            this.includeView_CeilingPlans.UseVisualStyleBackColor = true;
            // 
            // includeView_FloorPlans
            // 
            this.includeView_FloorPlans.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_FloorPlans;
            this.includeView_FloorPlans.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_FloorPlans", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_FloorPlans.Location = new System.Drawing.Point(11, 23);
            this.includeView_FloorPlans.Name = "includeView_FloorPlans";
            this.includeView_FloorPlans.Size = new System.Drawing.Size(135, 19);
            this.includeView_FloorPlans.TabIndex = 1;
            this.includeView_FloorPlans.Text = "Floor Plans";
            this.includeView_FloorPlans.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.exportOptions);
            this.tabControl.Controls.Add(this.purgeViews);
            this.tabControl.Location = new System.Drawing.Point(12, 62);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(360, 420);
            this.tabControl.TabIndex = 3;
            // 
            // exportOptions
            // 
            this.exportOptions.Controls.Add(this.exportOptions_Group_LandingPage);
            this.exportOptions.Controls.Add(this.exportOptions_Group_Compression);
            this.exportOptions.Controls.Add(this.exportOptions_Group_Worksharing);
            this.exportOptions.Controls.Add(this.exportOptions_Group_Clean);
            this.exportOptions.Controls.Add(this.exportOptions_Group_AddFiles);
            this.exportOptions.Location = new System.Drawing.Point(4, 24);
            this.exportOptions.Name = "exportOptions";
            this.exportOptions.Padding = new System.Windows.Forms.Padding(3);
            this.exportOptions.Size = new System.Drawing.Size(352, 392);
            this.exportOptions.TabIndex = 0;
            this.exportOptions.Text = "Export Options";
            this.exportOptions.UseVisualStyleBackColor = true;
            // 
            // exportOptions_Group_LandingPage
            // 
            this.exportOptions_Group_LandingPage.Controls.Add(this.exportOptions_LandingPage);
            this.exportOptions_Group_LandingPage.Location = new System.Drawing.Point(9, 213);
            this.exportOptions_Group_LandingPage.Name = "exportOptions_Group_LandingPage";
            this.exportOptions_Group_LandingPage.Size = new System.Drawing.Size(334, 63);
            this.exportOptions_Group_LandingPage.TabIndex = 9;
            this.exportOptions_Group_LandingPage.TabStop = false;
            this.exportOptions_Group_LandingPage.Text = "Landing Page";
            // 
            // exportOptions_LandingPage
            // 
            this.exportOptions_LandingPage.DropDownHeight = 250;
            this.exportOptions_LandingPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.exportOptions_LandingPage.DropDownWidth = 600;
            this.exportOptions_LandingPage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.exportOptions_LandingPage.FormattingEnabled = true;
            this.exportOptions_LandingPage.IntegralHeight = false;
            this.exportOptions_LandingPage.Location = new System.Drawing.Point(11, 25);
            this.exportOptions_LandingPage.Name = "exportOptions_LandingPage";
            this.exportOptions_LandingPage.Size = new System.Drawing.Size(312, 23);
            this.exportOptions_LandingPage.TabIndex = 0;
            this.toolTip.SetToolTip(this.exportOptions_LandingPage, "Landing page is kept and will be set as the starting view.\r\n");
            // 
            // exportOptions_Group_Compression
            // 
            this.exportOptions_Group_Compression.Controls.Add(this.exportOptions_Compression);
            this.exportOptions_Group_Compression.Location = new System.Drawing.Point(181, 160);
            this.exportOptions_Group_Compression.Name = "exportOptions_Group_Compression";
            this.exportOptions_Group_Compression.Size = new System.Drawing.Size(162, 50);
            this.exportOptions_Group_Compression.TabIndex = 3;
            this.exportOptions_Group_Compression.TabStop = false;
            this.exportOptions_Group_Compression.Text = "Compression";
            // 
            // exportOptions_Compression
            // 
            this.exportOptions_Compression.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_Compression;
            this.exportOptions_Compression.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportOptions_Compression.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_Compression", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_Compression.Location = new System.Drawing.Point(11, 23);
            this.exportOptions_Compression.Name = "exportOptions_Compression";
            this.exportOptions_Compression.Size = new System.Drawing.Size(145, 19);
            this.exportOptions_Compression.TabIndex = 1;
            this.exportOptions_Compression.Text = "Enable Compression";
            this.exportOptions_Compression.UseVisualStyleBackColor = true;
            // 
            // exportOptions_Group_Worksharing
            // 
            this.exportOptions_Group_Worksharing.Controls.Add(this.exportOptions_Label);
            this.exportOptions_Group_Worksharing.Controls.Add(this.exportOptions_DisableWorksharing);
            this.exportOptions_Group_Worksharing.Location = new System.Drawing.Point(9, 282);
            this.exportOptions_Group_Worksharing.Name = "exportOptions_Group_Worksharing";
            this.exportOptions_Group_Worksharing.Size = new System.Drawing.Size(334, 100);
            this.exportOptions_Group_Worksharing.TabIndex = 4;
            this.exportOptions_Group_Worksharing.TabStop = false;
            this.exportOptions_Group_Worksharing.Text = "Worksharing";
            // 
            // exportOptions_Label
            // 
            this.exportOptions_Label.AutoSize = true;
            this.exportOptions_Label.Location = new System.Drawing.Point(6, 45);
            this.exportOptions_Label.Name = "exportOptions_Label";
            this.exportOptions_Label.Size = new System.Drawing.Size(325, 45);
            this.exportOptions_Label.TabIndex = 8;
            this.exportOptions_Label.Text = "If worksharing is disabled all worksets will be removed and\r\nthe exported file wi" +
    "ll be detached from the central model.\r\nKeeping worksharing enabled will create " +
    "a new local copy.";
            // 
            // exportOptions_DisableWorksharing
            // 
            this.exportOptions_DisableWorksharing.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_DisableWorksharing;
            this.exportOptions_DisableWorksharing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportOptions_DisableWorksharing.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_DisableWorksharing", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_DisableWorksharing.Location = new System.Drawing.Point(11, 23);
            this.exportOptions_DisableWorksharing.Name = "exportOptions_DisableWorksharing";
            this.exportOptions_DisableWorksharing.Size = new System.Drawing.Size(151, 19);
            this.exportOptions_DisableWorksharing.TabIndex = 1;
            this.exportOptions_DisableWorksharing.Text = "Disable Worksharing";
            this.exportOptions_DisableWorksharing.UseVisualStyleBackColor = true;
            // 
            // exportOptions_Group_Clean
            // 
            this.exportOptions_Group_Clean.Controls.Add(this.exportOptions_IncludeViewsAll);
            this.exportOptions_Group_Clean.Controls.Add(this.exportOptions_IncludeSheets);
            this.exportOptions_Group_Clean.Controls.Add(this.exportOptions_IncludeViewsSelected);
            this.exportOptions_Group_Clean.Controls.Add(this.exportOptions_Audit);
            this.exportOptions_Group_Clean.Controls.Add(this.exportOptions_IncludeViewsOnSheets);
            this.exportOptions_Group_Clean.Controls.Add(this.exportOptions_PurgeUnused);
            this.exportOptions_Group_Clean.Location = new System.Drawing.Point(9, 6);
            this.exportOptions_Group_Clean.Name = "exportOptions_Group_Clean";
            this.exportOptions_Group_Clean.Size = new System.Drawing.Size(162, 204);
            this.exportOptions_Group_Clean.TabIndex = 1;
            this.exportOptions_Group_Clean.TabStop = false;
            this.exportOptions_Group_Clean.Text = "Clean";
            // 
            // exportOptions_IncludeViewsAll
            // 
            this.exportOptions_IncludeViewsAll.AutoSize = true;
            this.exportOptions_IncludeViewsAll.Checked = true;
            this.exportOptions_IncludeViewsAll.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_IncludeSheets", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_IncludeViewsAll.Enabled = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_IncludeSheets;
            this.exportOptions_IncludeViewsAll.Location = new System.Drawing.Point(21, 97);
            this.exportOptions_IncludeViewsAll.Name = "exportOptions_IncludeViewsAll";
            this.exportOptions_IncludeViewsAll.Size = new System.Drawing.Size(71, 19);
            this.exportOptions_IncludeViewsAll.TabIndex = 4;
            this.exportOptions_IncludeViewsAll.TabStop = true;
            this.exportOptions_IncludeViewsAll.Text = "All views";
            this.exportOptions_IncludeViewsAll.UseVisualStyleBackColor = true;
            this.exportOptions_IncludeViewsAll.CheckedChanged += new System.EventHandler(this.exportOptions_IncludeViews_Checked);
            // 
            // exportOptions_IncludeSheets
            // 
            this.exportOptions_IncludeSheets.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_IncludeSheets;
            this.exportOptions_IncludeSheets.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_IncludeSheets", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_IncludeSheets.Location = new System.Drawing.Point(11, 73);
            this.exportOptions_IncludeSheets.Name = "exportOptions_IncludeSheets";
            this.exportOptions_IncludeSheets.Size = new System.Drawing.Size(145, 19);
            this.exportOptions_IncludeSheets.TabIndex = 3;
            this.exportOptions_IncludeSheets.Text = "Include all sheets and";
            this.exportOptions_IncludeSheets.UseVisualStyleBackColor = true;
            this.exportOptions_IncludeSheets.CheckedChanged += new System.EventHandler(this.exportOptions_IncludeSheets_CheckedChanged);
            // 
            // exportOptions_IncludeViewsSelected
            // 
            this.exportOptions_IncludeViewsSelected.AutoSize = true;
            this.exportOptions_IncludeViewsSelected.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.exportOptions_IncludeViewsSelected.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_IncludeSheets", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_IncludeViewsSelected.Enabled = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_IncludeSheets;
            this.exportOptions_IncludeViewsSelected.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.exportOptions_IncludeViewsSelected.Location = new System.Drawing.Point(21, 148);
            this.exportOptions_IncludeViewsSelected.Name = "exportOptions_IncludeViewsSelected";
            this.exportOptions_IncludeViewsSelected.Size = new System.Drawing.Size(128, 34);
            this.exportOptions_IncludeViewsSelected.TabIndex = 6;
            this.exportOptions_IncludeViewsSelected.Text = "Views on sheets\r\nand selected views";
            this.exportOptions_IncludeViewsSelected.UseVisualStyleBackColor = true;
            this.exportOptions_IncludeViewsSelected.CheckedChanged += new System.EventHandler(this.exportOptions_IncludeViews_Checked);
            // 
            // exportOptions_Audit
            // 
            this.exportOptions_Audit.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_Audit;
            this.exportOptions_Audit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportOptions_Audit.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_Audit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_Audit.Location = new System.Drawing.Point(11, 48);
            this.exportOptions_Audit.Name = "exportOptions_Audit";
            this.exportOptions_Audit.Size = new System.Drawing.Size(135, 19);
            this.exportOptions_Audit.TabIndex = 2;
            this.exportOptions_Audit.Text = "Perform Audit";
            this.exportOptions_Audit.UseVisualStyleBackColor = true;
            // 
            // exportOptions_IncludeViewsOnSheets
            // 
            this.exportOptions_IncludeViewsOnSheets.AutoSize = true;
            this.exportOptions_IncludeViewsOnSheets.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_IncludeSheets", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_IncludeViewsOnSheets.Enabled = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_IncludeSheets;
            this.exportOptions_IncludeViewsOnSheets.Location = new System.Drawing.Point(21, 122);
            this.exportOptions_IncludeViewsOnSheets.Name = "exportOptions_IncludeViewsOnSheets";
            this.exportOptions_IncludeViewsOnSheets.Size = new System.Drawing.Size(113, 19);
            this.exportOptions_IncludeViewsOnSheets.TabIndex = 5;
            this.exportOptions_IncludeViewsOnSheets.Text = "Views on sheets";
            this.exportOptions_IncludeViewsOnSheets.UseVisualStyleBackColor = true;
            this.exportOptions_IncludeViewsOnSheets.CheckedChanged += new System.EventHandler(this.exportOptions_IncludeViews_Checked);
            // 
            // exportOptions_PurgeUnused
            // 
            this.exportOptions_PurgeUnused.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_PurgeUnused;
            this.exportOptions_PurgeUnused.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportOptions_PurgeUnused.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_PurgeUnused", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_PurgeUnused.Location = new System.Drawing.Point(11, 23);
            this.exportOptions_PurgeUnused.Name = "exportOptions_PurgeUnused";
            this.exportOptions_PurgeUnused.Size = new System.Drawing.Size(135, 19);
            this.exportOptions_PurgeUnused.TabIndex = 1;
            this.exportOptions_PurgeUnused.Text = "Purge Unused";
            this.exportOptions_PurgeUnused.UseVisualStyleBackColor = true;
            // 
            // exportOptions_Group_AddFiles
            // 
            this.exportOptions_Group_AddFiles.Controls.Add(this.exportOptions_IncludeExportReport);
            this.exportOptions_Group_AddFiles.Controls.Add(this.exportOptions_IncludeDWFLinks);
            this.exportOptions_Group_AddFiles.Controls.Add(this.exportOptions_IncludeDecals);
            this.exportOptions_Group_AddFiles.Controls.Add(this.exportOptions_IncludeCADLinks);
            this.exportOptions_Group_AddFiles.Controls.Add(this.exportOptions_IncludeRevitLinks);
            this.exportOptions_Group_AddFiles.Location = new System.Drawing.Point(181, 6);
            this.exportOptions_Group_AddFiles.Name = "exportOptions_Group_AddFiles";
            this.exportOptions_Group_AddFiles.Size = new System.Drawing.Size(162, 150);
            this.exportOptions_Group_AddFiles.TabIndex = 2;
            this.exportOptions_Group_AddFiles.TabStop = false;
            this.exportOptions_Group_AddFiles.Text = "Include Files";
            // 
            // exportOptions_IncludeExportReport
            // 
            this.exportOptions_IncludeExportReport.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_IncludeExportReport;
            this.exportOptions_IncludeExportReport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exportOptions_IncludeExportReport.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_IncludeExportReport", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_IncludeExportReport.Location = new System.Drawing.Point(11, 123);
            this.exportOptions_IncludeExportReport.Name = "exportOptions_IncludeExportReport";
            this.exportOptions_IncludeExportReport.Size = new System.Drawing.Size(145, 19);
            this.exportOptions_IncludeExportReport.TabIndex = 5;
            this.exportOptions_IncludeExportReport.Text = "Export Report";
            this.exportOptions_IncludeExportReport.UseVisualStyleBackColor = true;
            // 
            // exportOptions_IncludeDWFLinks
            // 
            this.exportOptions_IncludeDWFLinks.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_IncludeDWFLinks;
            this.exportOptions_IncludeDWFLinks.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_IncludeDWFLinks", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_IncludeDWFLinks.Location = new System.Drawing.Point(11, 73);
            this.exportOptions_IncludeDWFLinks.Name = "exportOptions_IncludeDWFLinks";
            this.exportOptions_IncludeDWFLinks.Size = new System.Drawing.Size(145, 19);
            this.exportOptions_IncludeDWFLinks.TabIndex = 3;
            this.exportOptions_IncludeDWFLinks.Text = "Linked DWF Markups";
            this.exportOptions_IncludeDWFLinks.UseVisualStyleBackColor = true;
            // 
            // exportOptions_IncludeDecals
            // 
            this.exportOptions_IncludeDecals.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_IncludeDecals;
            this.exportOptions_IncludeDecals.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_IncludeDecals", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_IncludeDecals.Location = new System.Drawing.Point(11, 98);
            this.exportOptions_IncludeDecals.Name = "exportOptions_IncludeDecals";
            this.exportOptions_IncludeDecals.Size = new System.Drawing.Size(140, 19);
            this.exportOptions_IncludeDecals.TabIndex = 4;
            this.exportOptions_IncludeDecals.Text = "Linked Decals";
            this.exportOptions_IncludeDecals.UseVisualStyleBackColor = true;
            // 
            // exportOptions_IncludeCADLinks
            // 
            this.exportOptions_IncludeCADLinks.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_IncludeCADLinks;
            this.exportOptions_IncludeCADLinks.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_IncludeCADLinks", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_IncludeCADLinks.Location = new System.Drawing.Point(11, 48);
            this.exportOptions_IncludeCADLinks.Name = "exportOptions_IncludeCADLinks";
            this.exportOptions_IncludeCADLinks.Size = new System.Drawing.Size(140, 19);
            this.exportOptions_IncludeCADLinks.TabIndex = 2;
            this.exportOptions_IncludeCADLinks.Text = "Linked CAD Files";
            this.exportOptions_IncludeCADLinks.UseVisualStyleBackColor = true;
            // 
            // exportOptions_IncludeRevitLinks
            // 
            this.exportOptions_IncludeRevitLinks.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_exportOptions_IncludeRevitLinks;
            this.exportOptions_IncludeRevitLinks.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_exportOptions_IncludeRevitLinks", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.exportOptions_IncludeRevitLinks.Location = new System.Drawing.Point(11, 23);
            this.exportOptions_IncludeRevitLinks.Name = "exportOptions_IncludeRevitLinks";
            this.exportOptions_IncludeRevitLinks.Size = new System.Drawing.Size(140, 19);
            this.exportOptions_IncludeRevitLinks.TabIndex = 1;
            this.exportOptions_IncludeRevitLinks.Text = "Linked Revit Models";
            this.exportOptions_IncludeRevitLinks.UseVisualStyleBackColor = true;
            // 
            // purgeViews
            // 
            this.purgeViews.Controls.Add(this.purgeViews_SelectAll);
            this.purgeViews.Controls.Add(this.purgeViews_ClearAll);
            this.purgeViews.Controls.Add(this.purgeViews_Group_scheduleViews);
            this.purgeViews.Controls.Add(this.purgeViews_Group_2DViews);
            this.purgeViews.Controls.Add(this.purgeViews_Group_3DViews);
            this.purgeViews.Controls.Add(this.purgeViews_Group_PlanViews);
            this.purgeViews.Location = new System.Drawing.Point(4, 24);
            this.purgeViews.Name = "purgeViews";
            this.purgeViews.Padding = new System.Windows.Forms.Padding(3);
            this.purgeViews.Size = new System.Drawing.Size(352, 392);
            this.purgeViews.TabIndex = 1;
            this.purgeViews.Text = "Selected Views";
            this.purgeViews.UseVisualStyleBackColor = true;
            // 
            // purgeViews_SelectAll
            // 
            this.purgeViews_SelectAll.Location = new System.Drawing.Point(183, 216);
            this.purgeViews_SelectAll.Name = "purgeViews_SelectAll";
            this.purgeViews_SelectAll.Size = new System.Drawing.Size(75, 30);
            this.purgeViews_SelectAll.TabIndex = 5;
            this.purgeViews_SelectAll.Text = "Select All";
            this.purgeViews_SelectAll.UseVisualStyleBackColor = true;
            this.purgeViews_SelectAll.Click += new System.EventHandler(this.purgeViews_SelectAll_Click);
            // 
            // purgeViews_ClearAll
            // 
            this.purgeViews_ClearAll.Location = new System.Drawing.Point(266, 216);
            this.purgeViews_ClearAll.Name = "purgeViews_ClearAll";
            this.purgeViews_ClearAll.Size = new System.Drawing.Size(75, 30);
            this.purgeViews_ClearAll.TabIndex = 6;
            this.purgeViews_ClearAll.Text = "Clear All";
            this.purgeViews_ClearAll.UseVisualStyleBackColor = true;
            this.purgeViews_ClearAll.Click += new System.EventHandler(this.purgeViews_ClearAll_Click);
            // 
            // purgeViews_Group_scheduleViews
            // 
            this.purgeViews_Group_scheduleViews.Controls.Add(this.includeView_ColumnSchedules);
            this.purgeViews_Group_scheduleViews.Controls.Add(this.includeView_PanelSchedules);
            this.purgeViews_Group_scheduleViews.Controls.Add(this.includeView_Schedules);
            this.purgeViews_Group_scheduleViews.Location = new System.Drawing.Point(181, 110);
            this.purgeViews_Group_scheduleViews.Name = "purgeViews_Group_scheduleViews";
            this.purgeViews_Group_scheduleViews.Size = new System.Drawing.Size(162, 100);
            this.purgeViews_Group_scheduleViews.TabIndex = 4;
            this.purgeViews_Group_scheduleViews.TabStop = false;
            this.purgeViews_Group_scheduleViews.Text = "Schedule Views";
            // 
            // includeView_ColumnSchedules
            // 
            this.includeView_ColumnSchedules.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_ColumnSchedules;
            this.includeView_ColumnSchedules.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_ColumnSchedules", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_ColumnSchedules.Location = new System.Drawing.Point(11, 73);
            this.includeView_ColumnSchedules.Name = "includeView_ColumnSchedules";
            this.includeView_ColumnSchedules.Size = new System.Drawing.Size(135, 19);
            this.includeView_ColumnSchedules.TabIndex = 3;
            this.includeView_ColumnSchedules.Text = "Column Schedules";
            this.includeView_ColumnSchedules.UseVisualStyleBackColor = true;
            // 
            // includeView_PanelSchedules
            // 
            this.includeView_PanelSchedules.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_PanelSchedules;
            this.includeView_PanelSchedules.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_PanelSchedules", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_PanelSchedules.Location = new System.Drawing.Point(11, 48);
            this.includeView_PanelSchedules.Name = "includeView_PanelSchedules";
            this.includeView_PanelSchedules.Size = new System.Drawing.Size(135, 19);
            this.includeView_PanelSchedules.TabIndex = 2;
            this.includeView_PanelSchedules.Text = "Panel Schedules";
            this.includeView_PanelSchedules.UseVisualStyleBackColor = true;
            // 
            // includeView_Schedules
            // 
            this.includeView_Schedules.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_Schedules;
            this.includeView_Schedules.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_Schedules", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_Schedules.Location = new System.Drawing.Point(11, 23);
            this.includeView_Schedules.Name = "includeView_Schedules";
            this.includeView_Schedules.Size = new System.Drawing.Size(135, 19);
            this.includeView_Schedules.TabIndex = 1;
            this.includeView_Schedules.Text = "Schedules";
            this.includeView_Schedules.UseVisualStyleBackColor = true;
            // 
            // purgeViews_Group_2DViews
            // 
            this.purgeViews_Group_2DViews.Controls.Add(this.includeView_Legends);
            this.purgeViews_Group_2DViews.Controls.Add(this.includeView_DraftingViews);
            this.purgeViews_Group_2DViews.Controls.Add(this.includeView_Details);
            this.purgeViews_Group_2DViews.Controls.Add(this.includeView_Elevations);
            this.purgeViews_Group_2DViews.Controls.Add(this.includeView_Sections);
            this.purgeViews_Group_2DViews.Location = new System.Drawing.Point(9, 135);
            this.purgeViews_Group_2DViews.Name = "purgeViews_Group_2DViews";
            this.purgeViews_Group_2DViews.Size = new System.Drawing.Size(162, 150);
            this.purgeViews_Group_2DViews.TabIndex = 2;
            this.purgeViews_Group_2DViews.TabStop = false;
            this.purgeViews_Group_2DViews.Text = "2D Views";
            // 
            // includeView_Legends
            // 
            this.includeView_Legends.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_Legends;
            this.includeView_Legends.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_Legends", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_Legends.Location = new System.Drawing.Point(11, 123);
            this.includeView_Legends.Name = "includeView_Legends";
            this.includeView_Legends.Size = new System.Drawing.Size(135, 19);
            this.includeView_Legends.TabIndex = 5;
            this.includeView_Legends.Text = "Legends";
            this.includeView_Legends.UseVisualStyleBackColor = true;
            // 
            // includeView_DraftingViews
            // 
            this.includeView_DraftingViews.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_DraftingViews;
            this.includeView_DraftingViews.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_DraftingViews", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_DraftingViews.Location = new System.Drawing.Point(11, 98);
            this.includeView_DraftingViews.Name = "includeView_DraftingViews";
            this.includeView_DraftingViews.Size = new System.Drawing.Size(135, 19);
            this.includeView_DraftingViews.TabIndex = 4;
            this.includeView_DraftingViews.Text = "Drafting Views";
            this.includeView_DraftingViews.UseVisualStyleBackColor = true;
            // 
            // includeView_Details
            // 
            this.includeView_Details.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_Details;
            this.includeView_Details.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_Details", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_Details.Location = new System.Drawing.Point(11, 73);
            this.includeView_Details.Name = "includeView_Details";
            this.includeView_Details.Size = new System.Drawing.Size(135, 19);
            this.includeView_Details.TabIndex = 3;
            this.includeView_Details.Text = "Details";
            this.includeView_Details.UseVisualStyleBackColor = true;
            // 
            // includeView_Elevations
            // 
            this.includeView_Elevations.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_Elevations;
            this.includeView_Elevations.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_Elevations", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_Elevations.Location = new System.Drawing.Point(11, 48);
            this.includeView_Elevations.Name = "includeView_Elevations";
            this.includeView_Elevations.Size = new System.Drawing.Size(135, 19);
            this.includeView_Elevations.TabIndex = 2;
            this.includeView_Elevations.Text = "Elevations";
            this.includeView_Elevations.UseVisualStyleBackColor = true;
            // 
            // includeView_Sections
            // 
            this.includeView_Sections.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_Sections;
            this.includeView_Sections.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_Sections", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_Sections.Location = new System.Drawing.Point(11, 23);
            this.includeView_Sections.Name = "includeView_Sections";
            this.includeView_Sections.Size = new System.Drawing.Size(135, 19);
            this.includeView_Sections.TabIndex = 1;
            this.includeView_Sections.Text = "Sections";
            this.includeView_Sections.UseVisualStyleBackColor = true;
            // 
            // purgeViews_Group_3DViews
            // 
            this.purgeViews_Group_3DViews.Controls.Add(this.includeView_Walkthroughs);
            this.purgeViews_Group_3DViews.Controls.Add(this.includeView_Renderings);
            this.purgeViews_Group_3DViews.Controls.Add(this.includeView_3DViews);
            this.purgeViews_Group_3DViews.Location = new System.Drawing.Point(181, 6);
            this.purgeViews_Group_3DViews.Name = "purgeViews_Group_3DViews";
            this.purgeViews_Group_3DViews.Size = new System.Drawing.Size(162, 100);
            this.purgeViews_Group_3DViews.TabIndex = 3;
            this.purgeViews_Group_3DViews.TabStop = false;
            this.purgeViews_Group_3DViews.Text = "3D Views";
            // 
            // includeView_Walkthroughs
            // 
            this.includeView_Walkthroughs.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_Walkthroughs;
            this.includeView_Walkthroughs.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_Walkthroughs", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_Walkthroughs.Location = new System.Drawing.Point(11, 73);
            this.includeView_Walkthroughs.Name = "includeView_Walkthroughs";
            this.includeView_Walkthroughs.Size = new System.Drawing.Size(135, 19);
            this.includeView_Walkthroughs.TabIndex = 3;
            this.includeView_Walkthroughs.Text = "Walkthroughs";
            this.includeView_Walkthroughs.UseVisualStyleBackColor = true;
            // 
            // includeView_Renderings
            // 
            this.includeView_Renderings.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_Renderings;
            this.includeView_Renderings.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_Renderings", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_Renderings.Location = new System.Drawing.Point(11, 48);
            this.includeView_Renderings.Name = "includeView_Renderings";
            this.includeView_Renderings.Size = new System.Drawing.Size(135, 19);
            this.includeView_Renderings.TabIndex = 2;
            this.includeView_Renderings.Text = "Renderings";
            this.includeView_Renderings.UseVisualStyleBackColor = true;
            // 
            // includeView_3DViews
            // 
            this.includeView_3DViews.Checked = global::rpmBIMTools.Properties.Settings.Default.exportModel_includeView_3DViews;
            this.includeView_3DViews.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::rpmBIMTools.Properties.Settings.Default, "exportModel_includeView_3DViews", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.includeView_3DViews.Location = new System.Drawing.Point(11, 23);
            this.includeView_3DViews.Name = "includeView_3DViews";
            this.includeView_3DViews.Size = new System.Drawing.Size(135, 19);
            this.includeView_3DViews.TabIndex = 1;
            this.includeView_3DViews.Text = "3D Views";
            this.includeView_3DViews.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Select a folder for exported files to be created.";
            // 
            // exportTo
            // 
            this.exportTo.Location = new System.Drawing.Point(12, 30);
            this.exportTo.Name = "exportTo";
            this.exportTo.ReadOnly = true;
            this.exportTo.Size = new System.Drawing.Size(333, 21);
            this.exportTo.TabIndex = 1;
            // 
            // saveSettings
            // 
            this.saveSettings.Checked = true;
            this.saveSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveSettings.Location = new System.Drawing.Point(20, 497);
            this.saveSettings.Name = "saveSettings";
            this.saveSettings.Size = new System.Drawing.Size(176, 19);
            this.saveSettings.TabIndex = 4;
            this.saveSettings.Text = "Save settings for next time.";
            this.saveSettings.UseVisualStyleBackColor = true;
            // 
            // exportModel
            // 
            this.AcceptButton = this.exportButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 532);
            this.Controls.Add(this.saveSettings);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.exportToButton);
            this.Controls.Add(this.exportToLabel);
            this.Controls.Add(this.exportTo);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.exportButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 570);
            this.Name = "exportModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Model";
            this.Load += new System.EventHandler(this.exportModel_Load);
            this.purgeViews_Group_PlanViews.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.exportOptions.ResumeLayout(false);
            this.exportOptions_Group_LandingPage.ResumeLayout(false);
            this.exportOptions_Group_Compression.ResumeLayout(false);
            this.exportOptions_Group_Worksharing.ResumeLayout(false);
            this.exportOptions_Group_Worksharing.PerformLayout();
            this.exportOptions_Group_Clean.ResumeLayout(false);
            this.exportOptions_Group_Clean.PerformLayout();
            this.exportOptions_Group_AddFiles.ResumeLayout(false);
            this.purgeViews.ResumeLayout(false);
            this.purgeViews_Group_scheduleViews.ResumeLayout(false);
            this.purgeViews_Group_2DViews.ResumeLayout(false);
            this.purgeViews_Group_3DViews.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label exportToLabel;
        private System.Windows.Forms.Button exportToButton;
        private System.Windows.Forms.GroupBox purgeViews_Group_PlanViews;
        private System.Windows.Forms.CheckBox includeView_AreaPlans;
        private System.Windows.Forms.CheckBox includeView_StructuredPlans;
        private System.Windows.Forms.CheckBox includeView_CeilingPlans;
        private System.Windows.Forms.CheckBox includeView_FloorPlans;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage purgeViews;
        private System.Windows.Forms.GroupBox purgeViews_Group_3DViews;
        private System.Windows.Forms.CheckBox includeView_Walkthroughs;
        private System.Windows.Forms.CheckBox includeView_Renderings;
        private System.Windows.Forms.CheckBox includeView_3DViews;
        private System.Windows.Forms.GroupBox purgeViews_Group_2DViews;
        private System.Windows.Forms.CheckBox includeView_DraftingViews;
        private System.Windows.Forms.CheckBox includeView_Details;
        private System.Windows.Forms.CheckBox includeView_Elevations;
        private System.Windows.Forms.CheckBox includeView_Sections;
        private System.Windows.Forms.Button purgeViews_SelectAll;
        private System.Windows.Forms.Button purgeViews_ClearAll;
        private System.Windows.Forms.CheckBox includeView_Legends;
        private System.Windows.Forms.GroupBox purgeViews_Group_scheduleViews;
        private System.Windows.Forms.CheckBox includeView_ColumnSchedules;
        private System.Windows.Forms.CheckBox includeView_PanelSchedules;
        private System.Windows.Forms.CheckBox includeView_Schedules;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.CheckBox saveSettings;
        private System.Windows.Forms.TabPage exportOptions;
        private System.Windows.Forms.GroupBox exportOptions_Group_Worksharing;
        private System.Windows.Forms.CheckBox exportOptions_DisableWorksharing;
        private System.Windows.Forms.GroupBox exportOptions_Group_Clean;
        private System.Windows.Forms.RadioButton exportOptions_IncludeViewsAll;
        private System.Windows.Forms.CheckBox exportOptions_IncludeSheets;
        private System.Windows.Forms.RadioButton exportOptions_IncludeViewsSelected;
        private System.Windows.Forms.CheckBox exportOptions_Audit;
        private System.Windows.Forms.RadioButton exportOptions_IncludeViewsOnSheets;
        private System.Windows.Forms.CheckBox exportOptions_PurgeUnused;
        private System.Windows.Forms.GroupBox exportOptions_Group_AddFiles;
        private System.Windows.Forms.CheckBox exportOptions_IncludeDWFLinks;
        private System.Windows.Forms.CheckBox exportOptions_IncludeCADLinks;
        private System.Windows.Forms.CheckBox exportOptions_IncludeRevitLinks;
        private System.Windows.Forms.CheckBox exportOptions_IncludeDecals;
        private System.Windows.Forms.CheckBox exportOptions_IncludeExportReport;
        private System.Windows.Forms.GroupBox exportOptions_Group_Compression;
        private System.Windows.Forms.CheckBox exportOptions_Compression;
        private System.Windows.Forms.TextBox exportTo;
        private System.Windows.Forms.GroupBox exportOptions_Group_LandingPage;
        private System.Windows.Forms.ComboBox exportOptions_LandingPage;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label exportOptions_Label;
    }
}