using LogScraper.Content;
using LogScraper.Controls;
using LogScraper.Controls.Generic;
using LogScraper.Controls.LogEntriesTextbox;
using LogScraper.Controls.Metadata;
using LogScraper.Controls.Search;

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
            BtnClearFilters = new System.Windows.Forms.Button();
            BtnErase = new SplitButton();
            ContextMenuReset = new System.Windows.Forms.ContextMenuStrip(components);
            ToolStripMenuItemClear = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItemReset = new System.Windows.Forms.ToolStripMenuItem();
            imageListBtnErase = new System.Windows.Forms.ImageList(components);
            BtnConfig = new System.Windows.Forms.Button();
            BtnFormRecord = new System.Windows.Forms.Button();
            BtnStop = new System.Windows.Forms.Button();
            BtnRecordWithTimer = new System.Windows.Forms.Button();
            btnOpenWithEditor = new System.Windows.Forms.Button();
            BtnSave = new System.Windows.Forms.Button();
            UsrLogProviderSelection = new LogProviderSelectionControl();
            splitContainer2 = new SplitContainerWithGrip();
            splitContainer4 = new SplitContainerWithGrip();
            LogTimeLineControl = new LogTimeLineControl();
            splitContainer5 = new SplitContainerWithGrip();
            BookMarksControl = new BookMarksControl();
            UserControlSearch = new UserControlSearch();
            PnlFiltersAndLogEntriesTextBox = new System.Windows.Forms.Panel();
            activeFilterOverviewControl = new LogScraper.Controls.FilterOverview.ActiveFilterOverviewControl();
            TxtErrorMessage = new System.Windows.Forms.TextBox();
            UserControlLogEntriesTextBox = new UserControlLogEntriesTextBox();
            flowTreeControl1 = new FlowTreeControl();
            LogViewport = new LogViewportControl();
            MetadataFormatingControl = new MetadataFormattingControl();
            LogPostProcessing = new LogPostProcessingControl();
            SearchResultListControl = new SearchResultListControl();
            groupBox6 = new System.Windows.Forms.GroupBox();
            UserControlContentFilter = new UserControlLogContentFilter();
            groupBox1 = new System.Windows.Forms.GroupBox();
            UsrMetadataFilterOverview = new UserControlMetadataFilterOverview();
            splitContainer1 = new SplitContainerWithGrip();
            splitContainer3 = new System.Windows.Forms.SplitContainer();
            ToolTip = new System.Windows.Forms.ToolTip(components);
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
            PnlFiltersAndLogEntriesTextBox.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            SuspendLayout();
            // 
            // BtnRecord
            // 
            BtnRecord.Enabled = false;
            BtnRecord.Image = (System.Drawing.Image)resources.GetObject("BtnRecord.Image");
            BtnRecord.Location = new System.Drawing.Point(7, 3);
            BtnRecord.Name = "BtnRecord";
            BtnRecord.Size = new System.Drawing.Size(40, 40);
            BtnRecord.TabIndex = 0;
            BtnRecord.TabStop = false;
            ToolTip.SetToolTip(BtnRecord, "Lees log eenmalig uit");
            BtnRecord.UseVisualStyleBackColor = true;
            BtnRecord.Click += BtnRecord_Click;
            // 
            // BtnClearFilters
            // 
            BtnClearFilters.Image = (System.Drawing.Image)resources.GetObject("BtnClearFilters.Image");
            BtnClearFilters.Location = new System.Drawing.Point(132, 3);
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
            BtnErase.Location = new System.Drawing.Point(171, 3);
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
            BtnConfig.Location = new System.Drawing.Point(223, 3);
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
            BtnFormRecord.Location = new System.Drawing.Point(86, 3);
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
            BtnStop.Location = new System.Drawing.Point(7, 3);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new System.Drawing.Size(40, 40);
            BtnStop.TabIndex = 17;
            BtnStop.TabStop = false;
            ToolTip.SetToolTip(BtnStop, "Stop [CTRL-S]");
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += BtnStop_Click;
            // 
            // BtnRecordWithTimer
            // 
            BtnRecordWithTimer.Enabled = false;
            BtnRecordWithTimer.Image = Properties.Resources.timer_record_outline_24x24;
            BtnRecordWithTimer.Location = new System.Drawing.Point(47, 3);
            BtnRecordWithTimer.Name = "BtnRecordWithTimer";
            BtnRecordWithTimer.Size = new System.Drawing.Size(40, 40);
            BtnRecordWithTimer.TabIndex = 16;
            BtnRecordWithTimer.TabStop = false;
            BtnRecordWithTimer.UseVisualStyleBackColor = true;
            BtnRecordWithTimer.Click += BtnRecordWithTimer_Click;
            // 
            // btnOpenWithEditor
            // 
            btnOpenWithEditor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnOpenWithEditor.Image = (System.Drawing.Image)resources.GetObject("btnOpenWithEditor.Image");
            btnOpenWithEditor.Location = new System.Drawing.Point(622, 0);
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
            BtnSave.Location = new System.Drawing.Point(596, 0);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new System.Drawing.Size(25, 25);
            BtnSave.TabIndex = 26;
            BtnSave.TabStop = false;
            ToolTip.SetToolTip(BtnSave, "Opslaan als...");
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // UsrLogProviderSelection
            // 
            UsrLogProviderSelection.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UsrLogProviderSelection.AutoSize = true;
            UsrLogProviderSelection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            UsrLogProviderSelection.BackColor = System.Drawing.SystemColors.Control;
            UsrLogProviderSelection.Location = new System.Drawing.Point(1, 45);
            UsrLogProviderSelection.Name = "UsrLogProviderSelection";
            UsrLogProviderSelection.Size = new System.Drawing.Size(249, 207);
            UsrLogProviderSelection.TabIndex = 25;
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
            splitContainer2.Size = new System.Drawing.Size(962, 610);
            splitContainer2.SplitterDistance = 647;
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
            splitContainer4.Size = new System.Drawing.Size(647, 610);
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
            LogTimeLineControl.Size = new System.Drawing.Size(647, 48);
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
            splitContainer5.Panel1.Controls.Add(PnlFiltersAndLogEntriesTextBox);
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
            splitContainer5.Size = new System.Drawing.Size(647, 554);
            splitContainer5.SplitterDistance = 442;
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
            UserControlSearch.Location = new System.Drawing.Point(328, 1);
            UserControlSearch.Name = "UserControlSearch";
            UserControlSearch.Size = new System.Drawing.Size(215, 25);
            UserControlSearch.TabIndex = 33;
            // 
            // PnlFiltersAndLogEntriesTextBox
            // 
            PnlFiltersAndLogEntriesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PnlFiltersAndLogEntriesTextBox.Controls.Add(activeFilterOverviewControl);
            PnlFiltersAndLogEntriesTextBox.Controls.Add(TxtErrorMessage);
            PnlFiltersAndLogEntriesTextBox.Controls.Add(UserControlLogEntriesTextBox);
            PnlFiltersAndLogEntriesTextBox.Location = new System.Drawing.Point(0, 28);
            PnlFiltersAndLogEntriesTextBox.Name = "PnlFiltersAndLogEntriesTextBox";
            PnlFiltersAndLogEntriesTextBox.Size = new System.Drawing.Size(644, 523);
            PnlFiltersAndLogEntriesTextBox.TabIndex = 42;
            // 
            // activeFilterOverviewControl
            // 
            activeFilterOverviewControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            activeFilterOverviewControl.Location = new System.Drawing.Point(0, 0);
            activeFilterOverviewControl.Name = "activeFilterOverviewControl";
            activeFilterOverviewControl.Size = new System.Drawing.Size(644, 18);
            activeFilterOverviewControl.TabIndex = 39;
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
            TxtErrorMessage.Size = new System.Drawing.Size(462, 75);
            TxtErrorMessage.TabIndex = 32;
            TxtErrorMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TxtErrorMessage.Visible = false;
            // 
            // UserControlLogEntriesTextBox
            // 
            UserControlLogEntriesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UserControlLogEntriesTextBox.Location = new System.Drawing.Point(0, 26);
            UserControlLogEntriesTextBox.Name = "UserControlLogEntriesTextBox";
            UserControlLogEntriesTextBox.Size = new System.Drawing.Size(644, 497);
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
            groupBox6.Size = new System.Drawing.Size(307, 610);
            groupBox6.TabIndex = 1;
            groupBox6.TabStop = false;
            groupBox6.Text = "Navigeren";
            // 
            // UserControlContentFilter
            // 
            UserControlContentFilter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            UserControlContentFilter.BackColor = System.Drawing.SystemColors.Control;
            UserControlContentFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            UserControlContentFilter.Location = new System.Drawing.Point(3, 19);
            UserControlContentFilter.Name = "UserControlContentFilter";
            UserControlContentFilter.Size = new System.Drawing.Size(301, 588);
            UserControlContentFilter.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(UsrMetadataFilterOverview);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(250, 336);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "Metadata";
            // 
            // UsrMetadataFilterOverview
            // 
            UsrMetadataFilterOverview.AutoScroll = true;
            UsrMetadataFilterOverview.BackColor = System.Drawing.SystemColors.Control;
            UsrMetadataFilterOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            UsrMetadataFilterOverview.Location = new System.Drawing.Point(3, 19);
            UsrMetadataFilterOverview.Margin = new System.Windows.Forms.Padding(0);
            UsrMetadataFilterOverview.Name = "UsrMetadataFilterOverview";
            UsrMetadataFilterOverview.Size = new System.Drawing.Size(244, 314);
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
            splitContainer1.Panel1.Controls.Add(splitContainer3);
            splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Panel2MinSize = 400;
            splitContainer1.Size = new System.Drawing.Size(1220, 610);
            splitContainer1.SplitterDistance = 250;
            splitContainer1.SplitterWidth = 8;
            splitContainer1.TabIndex = 13;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer3.Location = new System.Drawing.Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(BtnClearFilters);
            splitContainer3.Panel1.Controls.Add(UsrLogProviderSelection);
            splitContainer3.Panel1.Controls.Add(BtnErase);
            splitContainer3.Panel1.Controls.Add(BtnRecord);
            splitContainer3.Panel1.Controls.Add(BtnConfig);
            splitContainer3.Panel1.Controls.Add(BtnRecordWithTimer);
            splitContainer3.Panel1.Controls.Add(BtnFormRecord);
            splitContainer3.Panel1.Controls.Add(BtnStop);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(groupBox1);
            splitContainer3.Size = new System.Drawing.Size(250, 610);
            splitContainer3.SplitterDistance = 270;
            splitContainer3.TabIndex = 0;
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
            PnlFiltersAndLogEntriesTextBox.ResumeLayout(false);
            PnlFiltersAndLogEntriesTextBox.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel1.PerformLayout();
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        //Make these controls public so that they can be accessed from the mini controls form
        public System.Windows.Forms.Button BtnRecord;
        public System.Windows.Forms.Button BtnRecordWithTimer;
        public System.Windows.Forms.Button BtnStop;
        public System.Windows.Forms.Button btnOpenWithEditor;
        public SplitButton BtnErase;
        private System.Windows.Forms.Button BtnFormRecord;
        private System.Windows.Forms.GroupBox groupBox1;
        private SplitContainerWithGrip splitContainer1;
        private SplitContainerWithGrip splitContainer2;
        private UserControlLogContentFilter UserControlContentFilter;
        private System.Windows.Forms.GroupBox groupBox6;
        private UserControlSearch UserControlSearch;
        private System.Windows.Forms.Button BtnConfig;
        private UserControlMetadataFilterOverview UsrMetadataFilterOverview;
        private System.Windows.Forms.ToolTip ToolTip;
        private UserControlLogEntriesTextBox UserControlLogEntriesTextBox;
        private System.Windows.Forms.ContextMenuStrip ContextMenuReset;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemReset;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemClear;
        private System.Windows.Forms.ImageList imageListBtnErase;
        private System.Windows.Forms.Button BtnClearFilters;
        private SplitContainerWithGrip splitContainer4;
        private LogTimeLineControl LogTimeLineControl;
        public System.Windows.Forms.Button BtnSave;
        private BookMarksControl BookMarksControl;
        private LogViewportControl LogViewport;
        private LogPostProcessingControl LogPostProcessing;
        private System.Windows.Forms.Panel PnlFiltersAndLogEntriesTextBox;
        private System.Windows.Forms.TextBox TxtErrorMessage;
        private MetadataFormattingControl MetadataFormatingControl;
        private FlowTreeControl flowTreeControl1;
        private SearchResultListControl SearchResultListControl;
        private SplitContainerWithGrip splitContainer5;
        private LogProviderSelectionControl UsrLogProviderSelection;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Controls.FilterOverview.ActiveFilterOverviewControl activeFilterOverviewControl;
    }
}
