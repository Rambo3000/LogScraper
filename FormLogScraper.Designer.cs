using LogScraper.Utilities.Extensions;

namespace LogScraper
{
    partial class FormLogScraper
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogScraper));
            BtnRecord = new System.Windows.Forms.Button();
            lblLogEntriesTotalValue = new System.Windows.Forms.Label();
            lblNumberOfLogEntriesFiltered = new System.Windows.Forms.Label();
            lblLogEntriesFiltered = new System.Windows.Forms.Label();
            LbllogEntriesTotal = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            BtnClearFilters = new System.Windows.Forms.Button();
            BtnErase = new LogScraper.Utilities.UserControls.SplitButton();
            ContextMenuReset = new System.Windows.Forms.ContextMenuStrip(components);
            ToolStripMenuItemClear = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItemReset = new System.Windows.Forms.ToolStripMenuItem();
            imageListBtnErase = new System.Windows.Forms.ImageList(components);
            BtnConfig = new System.Windows.Forms.Button();
            BtnFormRecord = new System.Windows.Forms.Button();
            BtnStop = new System.Windows.Forms.Button();
            lblNumberOfLogEntriesFilteredWithError = new System.Windows.Forms.Label();
            BtnRecordWithTimer = new System.Windows.Forms.Button();
            lblLogEntriesFilteredWithError = new System.Windows.Forms.Label();
            userControlMemoryUsage1 = new UserControlMemoryUsage();
            btnOpenWithEditor = new System.Windows.Forms.Button();
            BtnSave = new System.Windows.Forms.Button();
            cboLogLayout = new System.Windows.Forms.ComboBox();
            usrLogProviderSelection = new LogScraper.Utilities.UserControls.LogProviderSelectionControl();
            splitContainer2 = new SplitContainerWithGrip();
            splitContainer4 = new SplitContainerWithGrip();
            LogTimeLineControl = new LogScraper.Utilities.UserControls.LogTimeLineControl();
            splitContainer5 = new SplitContainerWithGrip();
            BookMarksControl = new LogScraper.Utilities.UserControls.BookMarksControl();
            UserControlSearch = new UserControlSearch();
            panel1 = new System.Windows.Forms.Panel();
            TxtErrorMessage = new System.Windows.Forms.TextBox();
            UserControlLogEntriesTextBox = new LogScraper.Utilities.UserControls.UserControlLogEntriesTextBox();
            flowTreeControl1 = new LogScraper.Log.FlowTree.FlowTreeControl();
            LogViewport = new LogScraper.Utilities.UserControls.LogViewportControl();
            MetadataFormatingControl = new LogScraper.Log.Metadata.MetadataFormattingControl();
            LogPostProcessing = new LogScraper.LogPostProcessors.LogPostProcessingControl();
            SearchResultListControl = new LogScraper.Utilities.UserControls.SearchResultListControl();
            groupBox6 = new System.Windows.Forms.GroupBox();
            UserControlContentFilter = new UserControlLogContentFilter();
            label2 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            UsrMetadataFilterOverview = new LogScraper.Log.Metadata.UserControlMetadataFilterOverview();
            splitContainer1 = new SplitContainerWithGrip();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            groupBox3.SuspendLayout();
            ContextMenuReset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
            splitContainer4.Panel1.SuspendLayout();
            splitContainer4.Panel2.SuspendLayout();
            splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer5).BeginInit();
            splitContainer5.Panel1.SuspendLayout();
            splitContainer5.Panel2.SuspendLayout();
            splitContainer5.SuspendLayout();
            panel1.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnRecord
            // 
            BtnRecord.Enabled = false;
            BtnRecord.Image = (System.Drawing.Image)resources.GetObject("BtnRecord.Image");
            BtnRecord.Location = new System.Drawing.Point(7, 22);
            BtnRecord.Name = "BtnRecord";
            BtnRecord.Size = new System.Drawing.Size(52, 40);
            BtnRecord.TabIndex = 0;
            BtnRecord.TabStop = false;
            ToolTip.SetToolTip(BtnRecord, "Lees log eenmalig uit");
            BtnRecord.UseVisualStyleBackColor = true;
            BtnRecord.Click += BtnRecord_Click;
            // 
            // lblLogEntriesTotalValue
            // 
            lblLogEntriesTotalValue.Location = new System.Drawing.Point(119, 71);
            lblLogEntriesTotalValue.Name = "lblLogEntriesTotalValue";
            lblLogEntriesTotalValue.Size = new System.Drawing.Size(63, 15);
            lblLogEntriesTotalValue.TabIndex = 7;
            lblLogEntriesTotalValue.Text = "0";
            lblLogEntriesTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNumberOfLogEntriesFiltered
            // 
            lblNumberOfLogEntriesFiltered.Location = new System.Drawing.Point(119, 93);
            lblNumberOfLogEntriesFiltered.Name = "lblNumberOfLogEntriesFiltered";
            lblNumberOfLogEntriesFiltered.Size = new System.Drawing.Size(63, 15);
            lblNumberOfLogEntriesFiltered.TabIndex = 17;
            lblNumberOfLogEntriesFiltered.Text = "0";
            lblNumberOfLogEntriesFiltered.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLogEntriesFiltered
            // 
            lblLogEntriesFiltered.AutoSize = true;
            lblLogEntriesFiltered.Location = new System.Drawing.Point(10, 93);
            lblLogEntriesFiltered.Name = "lblLogEntriesFiltered";
            lblLogEntriesFiltered.Size = new System.Drawing.Size(88, 15);
            lblLogEntriesFiltered.TabIndex = 16;
            lblLogEntriesFiltered.Text = "Regels gefilterd";
            // 
            // LbllogEntriesTotal
            // 
            LbllogEntriesTotal.AutoSize = true;
            LbllogEntriesTotal.Location = new System.Drawing.Point(10, 71);
            LbllogEntriesTotal.Name = "LbllogEntriesTotal";
            LbllogEntriesTotal.Size = new System.Drawing.Size(100, 15);
            LbllogEntriesTotal.TabIndex = 10;
            LbllogEntriesTotal.Text = "Regels opgehaald";
            // 
            // groupBox3
            // 
            groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox3.Controls.Add(BtnClearFilters);
            groupBox3.Controls.Add(BtnErase);
            groupBox3.Controls.Add(BtnConfig);
            groupBox3.Controls.Add(BtnFormRecord);
            groupBox3.Controls.Add(BtnRecord);
            groupBox3.Controls.Add(LbllogEntriesTotal);
            groupBox3.Controls.Add(lblLogEntriesFiltered);
            groupBox3.Controls.Add(BtnStop);
            groupBox3.Controls.Add(lblNumberOfLogEntriesFilteredWithError);
            groupBox3.Controls.Add(BtnRecordWithTimer);
            groupBox3.Controls.Add(lblLogEntriesTotalValue);
            groupBox3.Controls.Add(lblLogEntriesFilteredWithError);
            groupBox3.Controls.Add(lblNumberOfLogEntriesFiltered);
            groupBox3.Controls.Add(userControlMemoryUsage1);
            groupBox3.Location = new System.Drawing.Point(0, 220);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(312, 147);
            groupBox3.TabIndex = 19;
            groupBox3.TabStop = false;
            groupBox3.Tag = "test";
            groupBox3.Text = "Uitlezen log";
            // 
            // BtnClearFilters
            // 
            BtnClearFilters.Image = (System.Drawing.Image)resources.GetObject("BtnClearFilters.Image");
            BtnClearFilters.Location = new System.Drawing.Point(164, 22);
            BtnClearFilters.Name = "BtnClearFilters";
            BtnClearFilters.Size = new System.Drawing.Size(40, 40);
            BtnClearFilters.TabIndex = 25;
            BtnClearFilters.TabStop = false;
            BtnClearFilters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ToolTip.SetToolTip(BtnClearFilters, "Alle filters ongedaan maken");
            BtnClearFilters.UseCompatibleTextRendering = true;
            BtnClearFilters.UseVisualStyleBackColor = true;
            BtnClearFilters.Click += BtnClearFilters_Click;
            // 
            // BtnErase
            // 
            BtnErase.DropDownMenu = ContextMenuReset;
            BtnErase.DropDownWidth = 15;
            BtnErase.Icon = (System.Drawing.Image)resources.GetObject("BtnErase.Icon");
            BtnErase.ImageIndex = 0;
            BtnErase.ImageList = imageListBtnErase;
            BtnErase.Location = new System.Drawing.Point(203, 22);
            BtnErase.Name = "BtnErase";
            BtnErase.Size = new System.Drawing.Size(45, 40);
            BtnErase.TabIndex = 10;
            BtnErase.ButtonClick += BtnErase_Click;
            // 
            // ContextMenuReset
            // 
            ContextMenuReset.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ToolStripMenuItemClear, ToolStripMenuItemReset });
            ContextMenuReset.Name = "ContextMenuReset";
            ContextMenuReset.Size = new System.Drawing.Size(211, 48);
            // 
            // ToolStripMenuItemClear
            // 
            ToolStripMenuItemClear.Image = (System.Drawing.Image)resources.GetObject("ToolStripMenuItemClear.Image");
            ToolStripMenuItemClear.Name = "ToolStripMenuItemClear";
            ToolStripMenuItemClear.Size = new System.Drawing.Size(210, 22);
            ToolStripMenuItemClear.Text = "Wis log (filters behouden)";
            ToolStripMenuItemClear.Click += ToolStripMenuItemClear_Click;
            // 
            // ToolStripMenuItemReset
            // 
            ToolStripMenuItemReset.Image = (System.Drawing.Image)resources.GetObject("ToolStripMenuItemReset.Image");
            ToolStripMenuItemReset.Name = "ToolStripMenuItemReset";
            ToolStripMenuItemReset.Size = new System.Drawing.Size(210, 22);
            ToolStripMenuItemReset.Text = "Reset log en filters";
            ToolStripMenuItemReset.Click += ToolStripMenuItemReset_Click;
            // 
            // imageListBtnErase
            // 
            imageListBtnErase.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageListBtnErase.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListBtnErase.ImageStream");
            imageListBtnErase.TransparentColor = System.Drawing.Color.Transparent;
            imageListBtnErase.Images.SetKeyName(0, "trash-can-outline-24x24.png");
            imageListBtnErase.Images.SetKeyName(1, "reset 24x24.png");
            // 
            // BtnConfig
            // 
            BtnConfig.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnConfig.Image = (System.Drawing.Image)resources.GetObject("BtnConfig.Image");
            BtnConfig.Location = new System.Drawing.Point(281, 117);
            BtnConfig.Name = "BtnConfig";
            BtnConfig.Size = new System.Drawing.Size(24, 24);
            BtnConfig.TabIndex = 23;
            BtnConfig.TabStop = false;
            BtnConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ToolTip.SetToolTip(BtnConfig, "Instellingen");
            BtnConfig.UseVisualStyleBackColor = true;
            BtnConfig.Click += BtnConfig_Click;
            // 
            // BtnFormRecord
            // 
            BtnFormRecord.Image = (System.Drawing.Image)resources.GetObject("BtnFormRecord.Image");
            BtnFormRecord.Location = new System.Drawing.Point(119, 22);
            BtnFormRecord.Name = "BtnFormRecord";
            BtnFormRecord.Size = new System.Drawing.Size(40, 40);
            BtnFormRecord.TabIndex = 11;
            BtnFormRecord.TabStop = false;
            BtnFormRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ToolTip.SetToolTip(BtnFormRecord, "Compacte weergave [CTRL-R]");
            BtnFormRecord.UseVisualStyleBackColor = true;
            BtnFormRecord.Click += BtnCompactView_Click;
            // 
            // BtnStop
            // 
            BtnStop.Image = (System.Drawing.Image)resources.GetObject("BtnStop.Image");
            BtnStop.Location = new System.Drawing.Point(7, 22);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new System.Drawing.Size(52, 40);
            BtnStop.TabIndex = 17;
            BtnStop.TabStop = false;
            ToolTip.SetToolTip(BtnStop, "Stop [CTRL-S]");
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += BtnStop_Click;
            // 
            // lblNumberOfLogEntriesFilteredWithError
            // 
            lblNumberOfLogEntriesFilteredWithError.Location = new System.Drawing.Point(119, 114);
            lblNumberOfLogEntriesFilteredWithError.Name = "lblNumberOfLogEntriesFilteredWithError";
            lblNumberOfLogEntriesFilteredWithError.Size = new System.Drawing.Size(63, 15);
            lblNumberOfLogEntriesFilteredWithError.TabIndex = 19;
            lblNumberOfLogEntriesFilteredWithError.Text = "0";
            lblNumberOfLogEntriesFilteredWithError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BtnRecordWithTimer
            // 
            BtnRecordWithTimer.Enabled = false;
            BtnRecordWithTimer.Image = Properties.Resources.timer_record_outline_24x24;
            BtnRecordWithTimer.Location = new System.Drawing.Point(58, 22);
            BtnRecordWithTimer.Name = "BtnRecordWithTimer";
            BtnRecordWithTimer.Size = new System.Drawing.Size(56, 40);
            BtnRecordWithTimer.TabIndex = 16;
            BtnRecordWithTimer.TabStop = false;
            BtnRecordWithTimer.UseVisualStyleBackColor = true;
            BtnRecordWithTimer.Click += BtnRecordWithTimer_Click;
            // 
            // lblLogEntriesFilteredWithError
            // 
            lblLogEntriesFilteredWithError.AutoSize = true;
            lblLogEntriesFilteredWithError.Location = new System.Drawing.Point(10, 114);
            lblLogEntriesFilteredWithError.Name = "lblLogEntriesFilteredWithError";
            lblLogEntriesFilteredWithError.Size = new System.Drawing.Size(93, 15);
            lblLogEntriesFilteredWithError.TabIndex = 18;
            lblLogEntriesFilteredWithError.Text = "Regels met error";
            // 
            // userControlMemoryUsage1
            // 
            userControlMemoryUsage1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            userControlMemoryUsage1.AutoSize = true;
            userControlMemoryUsage1.Location = new System.Drawing.Point(243, 100);
            userControlMemoryUsage1.Name = "userControlMemoryUsage1";
            userControlMemoryUsage1.Size = new System.Drawing.Size(64, 17);
            userControlMemoryUsage1.TabIndex = 24;
            // 
            // btnOpenWithEditor
            // 
            btnOpenWithEditor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnOpenWithEditor.Image = (System.Drawing.Image)resources.GetObject("btnOpenWithEditor.Image");
            btnOpenWithEditor.Location = new System.Drawing.Point(580, 0);
            btnOpenWithEditor.Name = "btnOpenWithEditor";
            btnOpenWithEditor.Size = new System.Drawing.Size(25, 25);
            btnOpenWithEditor.TabIndex = 11;
            btnOpenWithEditor.TabStop = false;
            ToolTip.SetToolTip(btnOpenWithEditor, "Open in externe editor");
            btnOpenWithEditor.UseVisualStyleBackColor = true;
            btnOpenWithEditor.Click += BtnOpenWithEditor_Click;
            // 
            // BtnSave
            // 
            BtnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnSave.Image = (System.Drawing.Image)resources.GetObject("BtnSave.Image");
            BtnSave.Location = new System.Drawing.Point(554, 0);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new System.Drawing.Size(25, 25);
            BtnSave.TabIndex = 26;
            BtnSave.TabStop = false;
            ToolTip.SetToolTip(BtnSave, "Opslaan als...");
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // cboLogLayout
            // 
            cboLogLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cboLogLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLogLayout.FormattingEnabled = true;
            cboLogLayout.Location = new System.Drawing.Point(58, 191);
            cboLogLayout.Name = "cboLogLayout";
            cboLogLayout.Size = new System.Drawing.Size(252, 23);
            cboLogLayout.TabIndex = 23;
            cboLogLayout.SelectedIndexChanged += CboLogLayout_SelectedIndexChanged;
            // 
            // usrLogProviderSelection
            // 
            usrLogProviderSelection.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            usrLogProviderSelection.Location = new System.Drawing.Point(3, 0);
            usrLogProviderSelection.Name = "usrLogProviderSelection";
            usrLogProviderSelection.Size = new System.Drawing.Size(311, 192);
            usrLogProviderSelection.TabIndex = 25;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer4);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(groupBox6);
            splitContainer2.Size = new System.Drawing.Size(898, 610);
            splitContainer2.SplitterDistance = 605;
            splitContainer2.SplitterWidth = 8;
            splitContainer2.TabIndex = 8;
            // 
            // splitContainer4
            // 
            splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer4.Location = new System.Drawing.Point(0, 0);
            splitContainer4.Margin = new System.Windows.Forms.Padding(0);
            splitContainer4.Name = "splitContainer4";
            splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            splitContainer4.Panel1.Controls.Add(LogTimeLineControl);
            // 
            // splitContainer4.Panel2
            // 
            splitContainer4.Panel2.Controls.Add(splitContainer5);
            splitContainer4.Size = new System.Drawing.Size(605, 610);
            splitContainer4.SplitterDistance = 48;
            splitContainer4.SplitterWidth = 8;
            splitContainer4.TabIndex = 5;
            // 
            // LogTimeLineControl
            // 
            LogTimeLineControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            LogTimeLineControl.BackColor = System.Drawing.Color.White;
            LogTimeLineControl.Dock = System.Windows.Forms.DockStyle.Fill;
            LogTimeLineControl.Location = new System.Drawing.Point(0, 0);
            LogTimeLineControl.Name = "LogTimeLineControl";
            LogTimeLineControl.Size = new System.Drawing.Size(605, 48);
            LogTimeLineControl.TabIndex = 0;
            // 
            // splitContainer5
            // 
            splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer5.Location = new System.Drawing.Point(0, 0);
            splitContainer5.Name = "splitContainer5";
            splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            splitContainer5.Panel1.Controls.Add(BookMarksControl);
            splitContainer5.Panel1.Controls.Add(UserControlSearch);
            splitContainer5.Panel1.Controls.Add(panel1);
            splitContainer5.Panel1.Controls.Add(flowTreeControl1);
            splitContainer5.Panel1.Controls.Add(LogViewport);
            splitContainer5.Panel1.Controls.Add(MetadataFormatingControl);
            splitContainer5.Panel1.Controls.Add(btnOpenWithEditor);
            splitContainer5.Panel1.Controls.Add(BtnSave);
            splitContainer5.Panel1.Controls.Add(LogPostProcessing);
            // 
            // splitContainer5.Panel2
            // 
            splitContainer5.Panel2.Controls.Add(SearchResultListControl);
            splitContainer5.Panel2Collapsed = true;
            splitContainer5.Size = new System.Drawing.Size(605, 554);
            splitContainer5.SplitterDistance = 313;
            splitContainer5.SplitterWidth = 8;
            splitContainer5.TabIndex = 44;
            // 
            // BookMarksControl
            // 
            BookMarksControl.Location = new System.Drawing.Point(-1, 0);
            BookMarksControl.Name = "BookMarksControl";
            BookMarksControl.Size = new System.Drawing.Size(97, 25);
            BookMarksControl.TabIndex = 11;
            // 
            // UserControlSearch
            // 
            UserControlSearch.Location = new System.Drawing.Point(328, 0);
            UserControlSearch.Name = "UserControlSearch";
            UserControlSearch.Size = new System.Drawing.Size(215, 25);
            UserControlSearch.TabIndex = 33;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.Controls.Add(TxtErrorMessage);
            panel1.Controls.Add(UserControlLogEntriesTextBox);
            panel1.Location = new System.Drawing.Point(0, 28);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(602, 523);
            panel1.TabIndex = 42;
            // 
            // TxtErrorMessage
            // 
            TxtErrorMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtErrorMessage.BackColor = System.Drawing.SystemColors.Window;
            TxtErrorMessage.ForeColor = System.Drawing.Color.DarkRed;
            TxtErrorMessage.Location = new System.Drawing.Point(102, 34);
            TxtErrorMessage.Multiline = true;
            TxtErrorMessage.Name = "TxtErrorMessage";
            TxtErrorMessage.ReadOnly = true;
            TxtErrorMessage.Size = new System.Drawing.Size(420, 75);
            TxtErrorMessage.TabIndex = 32;
            TxtErrorMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TxtErrorMessage.Visible = false;
            // 
            // UserControlLogEntriesTextBox
            // 
            UserControlLogEntriesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            UserControlLogEntriesTextBox.Location = new System.Drawing.Point(0, 0);
            UserControlLogEntriesTextBox.Name = "UserControlLogEntriesTextBox";
            UserControlLogEntriesTextBox.Size = new System.Drawing.Size(602, 523);
            UserControlLogEntriesTextBox.TabIndex = 38;
            // 
            // flowTreeControl1
            // 
            flowTreeControl1.Location = new System.Drawing.Point(181, 0);
            flowTreeControl1.MaximumSize = new System.Drawing.Size(40, 25);
            flowTreeControl1.MinimumSize = new System.Drawing.Size(40, 25);
            flowTreeControl1.Name = "flowTreeControl1";
            flowTreeControl1.Size = new System.Drawing.Size(40, 25);
            flowTreeControl1.TabIndex = 43;
            // 
            // LogViewport
            // 
            LogViewport.Location = new System.Drawing.Point(102, 0);
            LogViewport.Name = "LogViewport";
            LogViewport.Size = new System.Drawing.Size(73, 25);
            LogViewport.TabIndex = 40;
            // 
            // MetadataFormatingControl
            // 
            MetadataFormatingControl.Location = new System.Drawing.Point(227, 0);
            MetadataFormatingControl.MaximumSize = new System.Drawing.Size(40, 25);
            MetadataFormatingControl.MinimumSize = new System.Drawing.Size(40, 25);
            MetadataFormatingControl.Name = "MetadataFormatingControl";
            MetadataFormatingControl.Size = new System.Drawing.Size(40, 25);
            MetadataFormatingControl.TabIndex = 26;
            // 
            // LogPostProcessing
            // 
            LogPostProcessing.Location = new System.Drawing.Point(273, 0);
            LogPostProcessing.Name = "LogPostProcessing";
            LogPostProcessing.Size = new System.Drawing.Size(49, 25);
            LogPostProcessing.TabIndex = 41;
            // 
            // SearchResultListControl
            // 
            SearchResultListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            SearchResultListControl.Location = new System.Drawing.Point(0, 0);
            SearchResultListControl.Name = "SearchResultListControl";
            SearchResultListControl.Size = new System.Drawing.Size(150, 42);
            SearchResultListControl.TabIndex = 26;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(UserControlContentFilter);
            groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox6.Location = new System.Drawing.Point(0, 0);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new System.Drawing.Size(285, 610);
            groupBox6.TabIndex = 1;
            groupBox6.TabStop = false;
            groupBox6.Text = "Navigeren";
            // 
            // UserControlContentFilter
            // 
            UserControlContentFilter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            UserControlContentFilter.BackColor = System.Drawing.SystemColors.Window;
            UserControlContentFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            UserControlContentFilter.Location = new System.Drawing.Point(3, 19);
            UserControlContentFilter.Name = "UserControlContentFilter";
            UserControlContentFilter.Size = new System.Drawing.Size(279, 588);
            UserControlContentFilter.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(8, 194);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(43, 15);
            label2.TabIndex = 24;
            label2.Text = "Layout";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(UsrMetadataFilterOverview);
            groupBox1.Location = new System.Drawing.Point(0, 373);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(314, 237);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "Metadata";
            // 
            // UsrMetadataFilterOverview
            // 
            UsrMetadataFilterOverview.AutoScroll = true;
            UsrMetadataFilterOverview.BackColor = System.Drawing.Color.White;
            UsrMetadataFilterOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            UsrMetadataFilterOverview.Location = new System.Drawing.Point(3, 19);
            UsrMetadataFilterOverview.Margin = new System.Windows.Forms.Padding(0);
            UsrMetadataFilterOverview.Name = "UsrMetadataFilterOverview";
            UsrMetadataFilterOverview.Size = new System.Drawing.Size(308, 215);
            UsrMetadataFilterOverview.TabIndex = 0;
            UsrMetadataFilterOverview.FilterChanged += UsrControlMetadataFormating_FilterChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(usrLogProviderSelection);
            splitContainer1.Panel1.Controls.Add(groupBox1);
            splitContainer1.Panel1.Controls.Add(groupBox3);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(cboLogLayout);
            splitContainer1.Panel1MinSize = 252;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Panel2MinSize = 450;
            splitContainer1.Size = new System.Drawing.Size(1220, 610);
            splitContainer1.SplitterDistance = 314;
            splitContainer1.SplitterWidth = 8;
            splitContainer1.TabIndex = 13;
            // 
            // ToolTip
            // 
            ToolTip.AutoPopDelay = 9999999;
            ToolTip.InitialDelay = 250;
            ToolTip.ReshowDelay = 100;
            // 
            // FormLogScraper
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1220, 610);
            Controls.Add(splitContainer1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "FormLogScraper";
            Text = "LogScraper";
            Load += FormLogScraper_Load;
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ContextMenuReset.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer4.Panel1.ResumeLayout(false);
            splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
            splitContainer4.ResumeLayout(false);
            splitContainer5.Panel1.ResumeLayout(false);
            splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer5).EndInit();
            splitContainer5.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        //Make these controls public so that they can be accessed from the mini controls form
        public System.Windows.Forms.Button BtnRecord;
        public System.Windows.Forms.Button BtnRecordWithTimer;
        public System.Windows.Forms.Button BtnStop;
        public System.Windows.Forms.Button btnOpenWithEditor;
        public System.Windows.Forms.Label lblLogEntriesTotalValue;
        public System.Windows.Forms.Label lblNumberOfLogEntriesFiltered;
        public System.Windows.Forms.Label lblNumberOfLogEntriesFilteredWithError;
        public System.Windows.Forms.Label lblLogEntriesFilteredWithError;
        public Utilities.UserControls.SplitButton BtnErase;

        private System.Windows.Forms.Label LbllogEntriesTotal;

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblLogEntriesFiltered;
        private System.Windows.Forms.Button BtnFormRecord;
        private System.Windows.Forms.GroupBox groupBox1;
        private SplitContainerWithGrip splitContainer1;
        private SplitContainerWithGrip splitContainer2;
        private UserControlLogContentFilter UserControlContentFilter;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboLogLayout;
        private UserControlSearch UserControlSearch;
        private System.Windows.Forms.Button BtnConfig;
        private Log.Metadata.UserControlMetadataFilterOverview UsrMetadataFilterOverview;
        private System.Windows.Forms.ToolTip ToolTip;
        private UserControlMemoryUsage userControlMemoryUsage1;
        private Utilities.UserControls.UserControlLogEntriesTextBox UserControlLogEntriesTextBox;
        private System.Windows.Forms.ContextMenuStrip ContextMenuReset;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemReset;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemClear;
        private System.Windows.Forms.ImageList imageListBtnErase;
        private System.Windows.Forms.Button BtnClearFilters;
        private SplitContainerWithGrip splitContainer4;
        private Utilities.UserControls.LogTimeLineControl LogTimeLineControl;
        public System.Windows.Forms.Button BtnSave;
        private Utilities.UserControls.BookMarksControl BookMarksControl;
        private Utilities.UserControls.LogViewportControl LogViewport;
        private LogPostProcessors.LogPostProcessingControl LogPostProcessing;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox TxtErrorMessage;
        private Log.Metadata.MetadataFormattingControl MetadataFormatingControl;
        private Log.FlowTree.FlowTreeControl flowTreeControl1;
        private Utilities.UserControls.SearchResultListControl SearchResultListControl;
        private SplitContainerWithGrip splitContainer5;
        private Utilities.UserControls.LogProviderSelectionControl usrLogProviderSelection;
    }
}
