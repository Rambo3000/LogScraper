using LogScraper.Controls;
using LogScraper.Controls.Content;
using LogScraper.Controls.Generic;
using LogScraper.Controls.Viewport;
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
            BtnConfig = new System.Windows.Forms.Button();
            BtnFormRecord = new System.Windows.Forms.Button();
            LogProviderSelectionControl = new LogProviderSelectionControl();
            SplitContainerViewportAndNavigation = new SplitContainerWithGrip();
            SplitContainerTimeLineAndViewport = new SplitContainerWithGrip();
            LogTimeLineControl = new LogTimeLineControl();
            SplitContainerViewportAndSearchResultList = new SplitContainerWithGrip();
            SplitContainerViewportButtonsAndViewport = new System.Windows.Forms.SplitContainer();
            OpenLogInTextEditor = new OpenLogInTextEditor();
            BookMarksControl = new BookMarksControl();
            SaveLogControl = new SaveLogControl();
            FlowTreeControl = new FlowTreeControl();
            LogMetadataRenderOptionsControl = new LogMetadataRenderOptionsControl();
            LogRangeSelectionControl = new LogRangeSelectionControl();
            LogPostProcessingControl = new LogPostProcessingControl();
            SearchControl = new SearchControl();
            SplitContainerActiveFiltersViewport = new System.Windows.Forms.SplitContainer();
            ActiveFilterOverviewControl = new LogScraper.Controls.FilterOverview.ActiveFilterOverviewControl();
            ErrorMessageControl = new ErrorMessageControl();
            LogViewportControl = new LogViewportControl();
            SearchResultListControl = new SearchResultListControl();
            ErrorListControl = new ErrorListControl();
            ContentNavigationControl = new ContentNavigationControl();
            LogMetadataFiltersOverviewControl = new LogMetadataFiltersOverviewControl();
            SplitContainerMain = new SplitContainerWithGrip();
            SplitContainerSourceControlAndMetadata = new System.Windows.Forms.SplitContainer();
            SplitContainerSourceControlAndLogProviders = new System.Windows.Forms.SplitContainer();
            BtnReset = new System.Windows.Forms.Button();
            LogRecordingControl = new LogRecordingControl();
            BtnErase = new System.Windows.Forms.Button();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            PnlSaveAndExternalEditor = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)SplitContainerViewportAndNavigation).BeginInit();
            SplitContainerViewportAndNavigation.Panel1.SuspendLayout();
            SplitContainerViewportAndNavigation.Panel2.SuspendLayout();
            SplitContainerViewportAndNavigation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainerTimeLineAndViewport).BeginInit();
            SplitContainerTimeLineAndViewport.Panel1.SuspendLayout();
            SplitContainerTimeLineAndViewport.Panel2.SuspendLayout();
            SplitContainerTimeLineAndViewport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainerViewportAndSearchResultList).BeginInit();
            SplitContainerViewportAndSearchResultList.Panel1.SuspendLayout();
            SplitContainerViewportAndSearchResultList.Panel2.SuspendLayout();
            SplitContainerViewportAndSearchResultList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainerViewportButtonsAndViewport).BeginInit();
            SplitContainerViewportButtonsAndViewport.Panel1.SuspendLayout();
            SplitContainerViewportButtonsAndViewport.Panel2.SuspendLayout();
            SplitContainerViewportButtonsAndViewport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainerActiveFiltersViewport).BeginInit();
            SplitContainerActiveFiltersViewport.Panel1.SuspendLayout();
            SplitContainerActiveFiltersViewport.Panel2.SuspendLayout();
            SplitContainerActiveFiltersViewport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainerMain).BeginInit();
            SplitContainerMain.Panel1.SuspendLayout();
            SplitContainerMain.Panel2.SuspendLayout();
            SplitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainerSourceControlAndMetadata).BeginInit();
            SplitContainerSourceControlAndMetadata.Panel1.SuspendLayout();
            SplitContainerSourceControlAndMetadata.Panel2.SuspendLayout();
            SplitContainerSourceControlAndMetadata.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainerSourceControlAndLogProviders).BeginInit();
            SplitContainerSourceControlAndLogProviders.Panel1.SuspendLayout();
            SplitContainerSourceControlAndLogProviders.Panel2.SuspendLayout();
            SplitContainerSourceControlAndLogProviders.SuspendLayout();
            PnlSaveAndExternalEditor.SuspendLayout();
            SuspendLayout();
            // 
            // BtnConfig
            // 
            BtnConfig.Image = (System.Drawing.Image)resources.GetObject("BtnConfig.Image");
            BtnConfig.Location = new System.Drawing.Point(174, 3);
            BtnConfig.Name = "BtnConfig";
            BtnConfig.Size = new System.Drawing.Size(25, 25);
            BtnConfig.TabIndex = 23;
            BtnConfig.TabStop = false;
            BtnConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ToolTip.SetToolTip(BtnConfig, "Instellingen [CTRL-,}");
            BtnConfig.UseVisualStyleBackColor = true;
            BtnConfig.Click += BtnConfig_Click;
            // 
            // BtnFormRecord
            // 
            BtnFormRecord.Image = (System.Drawing.Image)resources.GetObject("BtnFormRecord.Image");
            BtnFormRecord.Location = new System.Drawing.Point(88, 3);
            BtnFormRecord.Name = "BtnFormRecord";
            BtnFormRecord.Size = new System.Drawing.Size(25, 25);
            BtnFormRecord.TabIndex = 11;
            BtnFormRecord.TabStop = false;
            BtnFormRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ToolTip.SetToolTip(BtnFormRecord, "Compacte weergave [CTRL-M]");
            BtnFormRecord.UseVisualStyleBackColor = true;
            BtnFormRecord.Click += BtnCompactView_Click;
            // 
            // LogProviderSelectionControl
            // 
            LogProviderSelectionControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            LogProviderSelectionControl.BackColor = System.Drawing.SystemColors.Control;
            LogProviderSelectionControl.Dock = System.Windows.Forms.DockStyle.Top;
            LogProviderSelectionControl.Location = new System.Drawing.Point(0, 0);
            LogProviderSelectionControl.Name = "LogProviderSelectionControl";
            LogProviderSelectionControl.Size = new System.Drawing.Size(230, 207);
            LogProviderSelectionControl.TabIndex = 25;
            // 
            // SplitContainerViewportAndNavigation
            // 
            SplitContainerViewportAndNavigation.CollapseablePanel = CollapsePanel.Panel2;
            SplitContainerViewportAndNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitContainerViewportAndNavigation.Location = new System.Drawing.Point(0, 0);
            SplitContainerViewportAndNavigation.Name = "SplitContainerViewportAndNavigation";
            // 
            // SplitContainerViewportAndNavigation.Panel1
            // 
            SplitContainerViewportAndNavigation.Panel1.Controls.Add(SplitContainerTimeLineAndViewport);
            // 
            // SplitContainerViewportAndNavigation.Panel2
            // 
            SplitContainerViewportAndNavigation.Panel2.Controls.Add(ContentNavigationControl);
            SplitContainerViewportAndNavigation.Panel2.Padding = new System.Windows.Forms.Padding(0, 2, 1, 0);
            SplitContainerViewportAndNavigation.Size = new System.Drawing.Size(982, 610);
            SplitContainerViewportAndNavigation.SplitterDistance = 658;
            SplitContainerViewportAndNavigation.SplitterWidth = 8;
            SplitContainerViewportAndNavigation.TabIndex = 8;
            SplitContainerViewportAndNavigation.TextSplitter = "Navigatie";
            // 
            // SplitContainerTimeLineAndViewport
            // 
            SplitContainerTimeLineAndViewport.CollapseablePanel = CollapsePanel.None;
            SplitContainerTimeLineAndViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitContainerTimeLineAndViewport.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            SplitContainerTimeLineAndViewport.Location = new System.Drawing.Point(0, 0);
            SplitContainerTimeLineAndViewport.Margin = new System.Windows.Forms.Padding(0);
            SplitContainerTimeLineAndViewport.Name = "SplitContainerTimeLineAndViewport";
            SplitContainerTimeLineAndViewport.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainerTimeLineAndViewport.Panel1
            // 
            SplitContainerTimeLineAndViewport.Panel1.Controls.Add(LogTimeLineControl);
            // 
            // SplitContainerTimeLineAndViewport.Panel2
            // 
            SplitContainerTimeLineAndViewport.Panel2.Controls.Add(SplitContainerViewportAndSearchResultList);
            SplitContainerTimeLineAndViewport.Size = new System.Drawing.Size(658, 610);
            SplitContainerTimeLineAndViewport.SplitterDistance = 48;
            SplitContainerTimeLineAndViewport.SplitterWidth = 8;
            SplitContainerTimeLineAndViewport.TabIndex = 5;
            SplitContainerTimeLineAndViewport.TextSplitter = "";
            // 
            // LogTimeLineControl
            // 
            LogTimeLineControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            LogTimeLineControl.BackColor = System.Drawing.Color.White;
            LogTimeLineControl.Dock = System.Windows.Forms.DockStyle.Fill;
            LogTimeLineControl.Location = new System.Drawing.Point(0, 0);
            LogTimeLineControl.Name = "LogTimeLineControl";
            LogTimeLineControl.Size = new System.Drawing.Size(658, 48);
            LogTimeLineControl.TabIndex = 0;
            // 
            // SplitContainerViewportAndSearchResultList
            // 
            SplitContainerViewportAndSearchResultList.CollapseablePanel = CollapsePanel.Panel2;
            SplitContainerViewportAndSearchResultList.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitContainerViewportAndSearchResultList.Location = new System.Drawing.Point(0, 0);
            SplitContainerViewportAndSearchResultList.Name = "SplitContainerViewportAndSearchResultList";
            SplitContainerViewportAndSearchResultList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainerViewportAndSearchResultList.Panel1
            // 
            SplitContainerViewportAndSearchResultList.Panel1.Controls.Add(SplitContainerViewportButtonsAndViewport);
            // 
            // SplitContainerViewportAndSearchResultList.Panel2
            // 
            SplitContainerViewportAndSearchResultList.Panel2.Controls.Add(SearchResultListControl);
            SplitContainerViewportAndSearchResultList.Panel2.Controls.Add(ErrorListControl);
            SplitContainerViewportAndSearchResultList.Size = new System.Drawing.Size(658, 554);
            SplitContainerViewportAndSearchResultList.SplitterDistance = 442;
            SplitContainerViewportAndSearchResultList.SplitterWidth = 8;
            SplitContainerViewportAndSearchResultList.TabIndex = 44;
            SplitContainerViewportAndSearchResultList.TextSplitter = "";
            // 
            // SplitContainerViewportButtonsAndViewport
            // 
            SplitContainerViewportButtonsAndViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitContainerViewportButtonsAndViewport.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            SplitContainerViewportButtonsAndViewport.IsSplitterFixed = true;
            SplitContainerViewportButtonsAndViewport.Location = new System.Drawing.Point(0, 0);
            SplitContainerViewportButtonsAndViewport.Margin = new System.Windows.Forms.Padding(0);
            SplitContainerViewportButtonsAndViewport.Name = "SplitContainerViewportButtonsAndViewport";
            SplitContainerViewportButtonsAndViewport.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainerViewportButtonsAndViewport.Panel1
            // 
            SplitContainerViewportButtonsAndViewport.Panel1.Controls.Add(PnlSaveAndExternalEditor);
            SplitContainerViewportButtonsAndViewport.Panel1.Controls.Add(BookMarksControl);
            SplitContainerViewportButtonsAndViewport.Panel1.Controls.Add(FlowTreeControl);
            SplitContainerViewportButtonsAndViewport.Panel1.Controls.Add(LogMetadataRenderOptionsControl);
            SplitContainerViewportButtonsAndViewport.Panel1.Controls.Add(LogRangeSelectionControl);
            SplitContainerViewportButtonsAndViewport.Panel1.Controls.Add(LogPostProcessingControl);
            SplitContainerViewportButtonsAndViewport.Panel1.Controls.Add(SearchControl);
            // 
            // SplitContainerViewportButtonsAndViewport.Panel2
            // 
            SplitContainerViewportButtonsAndViewport.Panel2.Controls.Add(SplitContainerActiveFiltersViewport);
            SplitContainerViewportButtonsAndViewport.Size = new System.Drawing.Size(658, 442);
            SplitContainerViewportButtonsAndViewport.SplitterDistance = 25;
            SplitContainerViewportButtonsAndViewport.SplitterWidth = 1;
            SplitContainerViewportButtonsAndViewport.TabIndex = 0;
            // 
            // OpenLogInTextEditor
            // 
            OpenLogInTextEditor.Location = new System.Drawing.Point(25, 0);
            OpenLogInTextEditor.Name = "OpenLogInTextEditor";
            OpenLogInTextEditor.Size = new System.Drawing.Size(25, 25);
            OpenLogInTextEditor.TabIndex = 45;
            // 
            // BookMarksControl
            // 
            BookMarksControl.Location = new System.Drawing.Point(0, 0);
            BookMarksControl.Name = "BookMarksControl";
            BookMarksControl.Size = new System.Drawing.Size(97, 25);
            BookMarksControl.TabIndex = 11;
            // 
            // SaveLogControl
            // 
            SaveLogControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            SaveLogControl.Location = new System.Drawing.Point(0, 0);
            SaveLogControl.Name = "SaveLogControl";
            SaveLogControl.Size = new System.Drawing.Size(25, 25);
            SaveLogControl.TabIndex = 44;
            // 
            // FlowTreeControl
            // 
            FlowTreeControl.Location = new System.Drawing.Point(182, 0);
            FlowTreeControl.MaximumSize = new System.Drawing.Size(40, 25);
            FlowTreeControl.MinimumSize = new System.Drawing.Size(40, 25);
            FlowTreeControl.Name = "FlowTreeControl";
            FlowTreeControl.Size = new System.Drawing.Size(40, 25);
            FlowTreeControl.TabIndex = 43;
            // 
            // LogMetadataRenderOptionsControl
            // 
            LogMetadataRenderOptionsControl.Location = new System.Drawing.Point(228, 0);
            LogMetadataRenderOptionsControl.MaximumSize = new System.Drawing.Size(40, 25);
            LogMetadataRenderOptionsControl.MinimumSize = new System.Drawing.Size(40, 25);
            LogMetadataRenderOptionsControl.Name = "LogMetadataRenderOptionsControl";
            LogMetadataRenderOptionsControl.Size = new System.Drawing.Size(40, 25);
            LogMetadataRenderOptionsControl.TabIndex = 26;
            // 
            // LogRangeSelectionControl
            // 
            LogRangeSelectionControl.Location = new System.Drawing.Point(103, 0);
            LogRangeSelectionControl.Name = "LogRangeSelectionControl";
            LogRangeSelectionControl.Size = new System.Drawing.Size(73, 25);
            LogRangeSelectionControl.TabIndex = 40;
            // 
            // LogPostProcessingControl
            // 
            LogPostProcessingControl.Location = new System.Drawing.Point(274, 0);
            LogPostProcessingControl.Name = "LogPostProcessingControl";
            LogPostProcessingControl.Size = new System.Drawing.Size(40, 25);
            LogPostProcessingControl.TabIndex = 41;
            // 
            // SearchControl
            // 
            SearchControl.Location = new System.Drawing.Point(320, 1);
            SearchControl.Name = "SearchControl";
            SearchControl.Size = new System.Drawing.Size(215, 25);
            SearchControl.TabIndex = 33;
            // 
            // SplitContainerActiveFiltersViewport
            // 
            SplitContainerActiveFiltersViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitContainerActiveFiltersViewport.IsSplitterFixed = true;
            SplitContainerActiveFiltersViewport.Location = new System.Drawing.Point(0, 0);
            SplitContainerActiveFiltersViewport.Margin = new System.Windows.Forms.Padding(0);
            SplitContainerActiveFiltersViewport.Name = "SplitContainerActiveFiltersViewport";
            SplitContainerActiveFiltersViewport.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainerActiveFiltersViewport.Panel1
            // 
            SplitContainerActiveFiltersViewport.Panel1.Controls.Add(ActiveFilterOverviewControl);
            SplitContainerActiveFiltersViewport.Panel1MinSize = 0;
            // 
            // SplitContainerActiveFiltersViewport.Panel2
            // 
            SplitContainerActiveFiltersViewport.Panel2.Controls.Add(ErrorMessageControl);
            SplitContainerActiveFiltersViewport.Panel2.Controls.Add(LogViewportControl);
            SplitContainerActiveFiltersViewport.Size = new System.Drawing.Size(658, 416);
            SplitContainerActiveFiltersViewport.SplitterDistance = 25;
            SplitContainerActiveFiltersViewport.SplitterWidth = 1;
            SplitContainerActiveFiltersViewport.TabIndex = 0;
            // 
            // ActiveFilterOverviewControl
            // 
            ActiveFilterOverviewControl.Dock = System.Windows.Forms.DockStyle.Top;
            ActiveFilterOverviewControl.Location = new System.Drawing.Point(0, 0);
            ActiveFilterOverviewControl.Name = "ActiveFilterOverviewControl";
            ActiveFilterOverviewControl.Size = new System.Drawing.Size(658, 24);
            ActiveFilterOverviewControl.TabIndex = 39;
            // 
            // ErrorMessageControl
            // 
            ErrorMessageControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ErrorMessageControl.Location = new System.Drawing.Point(102, 40);
            ErrorMessageControl.Name = "ErrorMessageControl";
            ErrorMessageControl.Size = new System.Drawing.Size(443, 67);
            ErrorMessageControl.TabIndex = 40;
            ErrorMessageControl.Visible = false;
            // 
            // LogViewportControl
            // 
            LogViewportControl.Dock = System.Windows.Forms.DockStyle.Fill;
            LogViewportControl.Location = new System.Drawing.Point(0, 0);
            LogViewportControl.Name = "LogViewportControl";
            LogViewportControl.Size = new System.Drawing.Size(658, 390);
            LogViewportControl.TabIndex = 38;
            // 
            // SearchResultListControl
            // 
            SearchResultListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            SearchResultListControl.Location = new System.Drawing.Point(0, 0);
            SearchResultListControl.Name = "SearchResultListControl";
            SearchResultListControl.Size = new System.Drawing.Size(658, 104);
            SearchResultListControl.TabIndex = 26;
            // 
            // ErrorListControl
            // 
            ErrorListControl.BackColor = System.Drawing.SystemColors.Control;
            ErrorListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            ErrorListControl.Location = new System.Drawing.Point(0, 0);
            ErrorListControl.Name = "ErrorListControl";
            ErrorListControl.Size = new System.Drawing.Size(658, 104);
            ErrorListControl.TabIndex = 27;
            ErrorListControl.Visible = false;
            // 
            // ContentNavigationControl
            // 
            ContentNavigationControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ContentNavigationControl.BackColor = System.Drawing.SystemColors.Control;
            ContentNavigationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            ContentNavigationControl.Location = new System.Drawing.Point(0, 2);
            ContentNavigationControl.Name = "ContentNavigationControl";
            ContentNavigationControl.Size = new System.Drawing.Size(315, 608);
            ContentNavigationControl.TabIndex = 0;
            // 
            // LogMetadataFiltersOverviewControl
            // 
            LogMetadataFiltersOverviewControl.AutoScroll = true;
            LogMetadataFiltersOverviewControl.BackColor = System.Drawing.SystemColors.Control;
            LogMetadataFiltersOverviewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            LogMetadataFiltersOverviewControl.Location = new System.Drawing.Point(0, 0);
            LogMetadataFiltersOverviewControl.Margin = new System.Windows.Forms.Padding(0);
            LogMetadataFiltersOverviewControl.Name = "LogMetadataFiltersOverviewControl";
            LogMetadataFiltersOverviewControl.Size = new System.Drawing.Size(230, 356);
            LogMetadataFiltersOverviewControl.TabIndex = 0;
            // 
            // SplitContainerMain
            // 
            SplitContainerMain.CollapseablePanel = CollapsePanel.Panel1;
            SplitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitContainerMain.Location = new System.Drawing.Point(0, 0);
            SplitContainerMain.Name = "SplitContainerMain";
            // 
            // SplitContainerMain.Panel1
            // 
            SplitContainerMain.Panel1.Controls.Add(SplitContainerSourceControlAndMetadata);
            SplitContainerMain.Panel1MinSize = 230;
            // 
            // SplitContainerMain.Panel2
            // 
            SplitContainerMain.Panel2.Controls.Add(SplitContainerViewportAndNavigation);
            SplitContainerMain.Panel2MinSize = 400;
            SplitContainerMain.Size = new System.Drawing.Size(1220, 610);
            SplitContainerMain.SplitterDistance = 230;
            SplitContainerMain.SplitterWidth = 8;
            SplitContainerMain.TabIndex = 13;
            SplitContainerMain.TextSplitter = "Logbron & metadata filters";
            // 
            // SplitContainerSourceControlAndMetadata
            // 
            SplitContainerSourceControlAndMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitContainerSourceControlAndMetadata.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            SplitContainerSourceControlAndMetadata.Location = new System.Drawing.Point(0, 0);
            SplitContainerSourceControlAndMetadata.Name = "SplitContainerSourceControlAndMetadata";
            SplitContainerSourceControlAndMetadata.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainerSourceControlAndMetadata.Panel1
            // 
            SplitContainerSourceControlAndMetadata.Panel1.Controls.Add(SplitContainerSourceControlAndLogProviders);
            // 
            // SplitContainerSourceControlAndMetadata.Panel2
            // 
            SplitContainerSourceControlAndMetadata.Panel2.Controls.Add(LogMetadataFiltersOverviewControl);
            SplitContainerSourceControlAndMetadata.Size = new System.Drawing.Size(230, 610);
            SplitContainerSourceControlAndMetadata.SplitterDistance = 250;
            SplitContainerSourceControlAndMetadata.TabIndex = 0;
            // 
            // SplitContainerSourceControlAndLogProviders
            // 
            SplitContainerSourceControlAndLogProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            SplitContainerSourceControlAndLogProviders.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            SplitContainerSourceControlAndLogProviders.IsSplitterFixed = true;
            SplitContainerSourceControlAndLogProviders.Location = new System.Drawing.Point(0, 0);
            SplitContainerSourceControlAndLogProviders.Margin = new System.Windows.Forms.Padding(0);
            SplitContainerSourceControlAndLogProviders.Name = "SplitContainerSourceControlAndLogProviders";
            SplitContainerSourceControlAndLogProviders.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainerSourceControlAndLogProviders.Panel1
            // 
            SplitContainerSourceControlAndLogProviders.Panel1.Controls.Add(BtnReset);
            SplitContainerSourceControlAndLogProviders.Panel1.Controls.Add(LogRecordingControl);
            SplitContainerSourceControlAndLogProviders.Panel1.Controls.Add(BtnErase);
            SplitContainerSourceControlAndLogProviders.Panel1.Controls.Add(BtnConfig);
            SplitContainerSourceControlAndLogProviders.Panel1.Controls.Add(BtnFormRecord);
            SplitContainerSourceControlAndLogProviders.Panel1MinSize = 0;
            // 
            // SplitContainerSourceControlAndLogProviders.Panel2
            // 
            SplitContainerSourceControlAndLogProviders.Panel2.Controls.Add(LogProviderSelectionControl);
            SplitContainerSourceControlAndLogProviders.Panel2MinSize = 0;
            SplitContainerSourceControlAndLogProviders.Size = new System.Drawing.Size(230, 250);
            SplitContainerSourceControlAndLogProviders.SplitterDistance = 44;
            SplitContainerSourceControlAndLogProviders.SplitterWidth = 1;
            SplitContainerSourceControlAndLogProviders.TabIndex = 0;
            // 
            // BtnReset
            // 
            BtnReset.Image = (System.Drawing.Image)resources.GetObject("BtnReset.Image");
            BtnReset.Location = new System.Drawing.Point(143, 3);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new System.Drawing.Size(25, 25);
            BtnReset.TabIndex = 27;
            BtnReset.TabStop = false;
            BtnReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ToolTip.SetToolTip(BtnReset, "Reset [CTRL-SHIFT-L]");
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // LogRecordingControl
            // 
            LogRecordingControl.Location = new System.Drawing.Point(3, 3);
            LogRecordingControl.Name = "LogRecordingControl";
            LogRecordingControl.Size = new System.Drawing.Size(79, 40);
            LogRecordingControl.TabIndex = 50;
            // 
            // BtnErase
            // 
            BtnErase.Image = (System.Drawing.Image)resources.GetObject("BtnErase.Image");
            BtnErase.Location = new System.Drawing.Point(119, 3);
            BtnErase.Name = "BtnErase";
            BtnErase.Size = new System.Drawing.Size(25, 25);
            BtnErase.TabIndex = 26;
            BtnErase.TabStop = false;
            BtnErase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ToolTip.SetToolTip(BtnErase, "Wis het log (filters behouden) [CTRL-L]");
            BtnErase.UseVisualStyleBackColor = true;
            BtnErase.Click += BtnErase_Click;
            // 
            // ToolTip
            // 
            ToolTip.AutoPopDelay = 9999999;
            ToolTip.InitialDelay = 250;
            ToolTip.ReshowDelay = 100;
            // 
            // PnlSaveAndExternalEditor
            // 
            PnlSaveAndExternalEditor.Controls.Add(SaveLogControl);
            PnlSaveAndExternalEditor.Controls.Add(OpenLogInTextEditor);
            PnlSaveAndExternalEditor.Dock = System.Windows.Forms.DockStyle.Right;
            PnlSaveAndExternalEditor.Location = new System.Drawing.Point(608, 0);
            PnlSaveAndExternalEditor.Name = "PnlSaveAndExternalEditor";
            PnlSaveAndExternalEditor.Size = new System.Drawing.Size(50, 25);
            PnlSaveAndExternalEditor.TabIndex = 46;
            // 
            // FormLogScraper
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1220, 610);
            Controls.Add(SplitContainerMain);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "FormLogScraper";
            Text = "LogScraper";
            Load += FormLogScraper_Load;
            SplitContainerViewportAndNavigation.Panel1.ResumeLayout(false);
            SplitContainerViewportAndNavigation.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainerViewportAndNavigation).EndInit();
            SplitContainerViewportAndNavigation.ResumeLayout(false);
            SplitContainerTimeLineAndViewport.Panel1.ResumeLayout(false);
            SplitContainerTimeLineAndViewport.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainerTimeLineAndViewport).EndInit();
            SplitContainerTimeLineAndViewport.ResumeLayout(false);
            SplitContainerViewportAndSearchResultList.Panel1.ResumeLayout(false);
            SplitContainerViewportAndSearchResultList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainerViewportAndSearchResultList).EndInit();
            SplitContainerViewportAndSearchResultList.ResumeLayout(false);
            SplitContainerViewportButtonsAndViewport.Panel1.ResumeLayout(false);
            SplitContainerViewportButtonsAndViewport.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainerViewportButtonsAndViewport).EndInit();
            SplitContainerViewportButtonsAndViewport.ResumeLayout(false);
            SplitContainerActiveFiltersViewport.Panel1.ResumeLayout(false);
            SplitContainerActiveFiltersViewport.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainerActiveFiltersViewport).EndInit();
            SplitContainerActiveFiltersViewport.ResumeLayout(false);
            SplitContainerMain.Panel1.ResumeLayout(false);
            SplitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainerMain).EndInit();
            SplitContainerMain.ResumeLayout(false);
            SplitContainerSourceControlAndMetadata.Panel1.ResumeLayout(false);
            SplitContainerSourceControlAndMetadata.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainerSourceControlAndMetadata).EndInit();
            SplitContainerSourceControlAndMetadata.ResumeLayout(false);
            SplitContainerSourceControlAndLogProviders.Panel1.ResumeLayout(false);
            SplitContainerSourceControlAndLogProviders.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainerSourceControlAndLogProviders).EndInit();
            SplitContainerSourceControlAndLogProviders.ResumeLayout(false);
            PnlSaveAndExternalEditor.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        public System.Windows.Forms.Button BtnErase;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Button BtnFormRecord;
        private SplitContainerWithGrip SplitContainerMain;
        private SplitContainerWithGrip SplitContainerViewportAndNavigation;
        private System.Windows.Forms.SplitContainer SplitContainerSourceControlAndMetadata;
        private SplitContainerWithGrip SplitContainerViewportAndSearchResultList;
        private SplitContainerWithGrip SplitContainerTimeLineAndViewport;
        private ContentNavigationControl ContentNavigationControl;
        private SearchControl SearchControl;
        private System.Windows.Forms.Button BtnConfig;
        private LogMetadataFiltersOverviewControl LogMetadataFiltersOverviewControl;
        private System.Windows.Forms.ToolTip ToolTip;
        private LogViewportControl LogViewportControl;
        private LogTimeLineControl LogTimeLineControl;
        private BookMarksControl BookMarksControl;
        private LogRangeSelectionControl LogRangeSelectionControl;
        private LogPostProcessingControl LogPostProcessingControl;
        private LogMetadataRenderOptionsControl LogMetadataRenderOptionsControl;
        private FlowTreeControl FlowTreeControl;
        private SearchResultListControl SearchResultListControl;
        private LogProviderSelectionControl LogProviderSelectionControl;
        public LogRecordingControl LogRecordingControl;
        private Controls.FilterOverview.ActiveFilterOverviewControl ActiveFilterOverviewControl;
        private ErrorListControl ErrorListControl;
        private SaveLogControl SaveLogControl;
        private ErrorMessageControl ErrorMessageControl;
        private OpenLogInTextEditor OpenLogInTextEditor;
        private System.Windows.Forms.SplitContainer SplitContainerViewportButtonsAndViewport;
        private System.Windows.Forms.SplitContainer SplitContainerActiveFiltersViewport;
        private System.Windows.Forms.SplitContainer SplitContainerSourceControlAndLogProviders;
        private System.Windows.Forms.Panel PnlSaveAndExternalEditor;
    }
}
