using System;
using System.IO;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Controls;
using LogScraper.Controls.FilterOverview;
using LogScraper.Controls.Generic;
using LogScraper.Controls.Search;
using LogScraper.Controls.Viewport;
using LogScraper.Log.Generic.Extensions;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Processing;
using LogScraper.Log.Rendering;
using LogScraper.Utilities;

namespace LogScraper
{
    public partial class FormLogScraper : Form
    {
        #region Form Initialization
        public FormLogScraper()
        {
            // Capture the UI SynchronizationContext before any background work can start,
            // so StateSlice marshals Changed events back to this thread automatically.
            StateSlice.SetSynchronizationContext();

            InitializeComponent();

            FormClosing += FormLogScraper_FormClosing;

            LogAppState.Instance.ResetRequested += LogAppState_ResetRequested;
            LogAppState.Instance.ProcessingState.Changed += (s, e) => UpdateButtonStatus();
            LogAppState.Instance.IsSourceValid.Changed += (s, e) => UpdateButtonStatus();

            ConfigAppState.Instance.GenericConfig.Changed += (s, e) => UpdateTimeLineVisibility();

            FormCompactView.Instance.ReturnToMainFormRequested += (s, e) => WindowState = FormWindowState.Normal;

            LogProviderSelectionControl.UriChanged += UsrLogProviderSelection_UriChanged;
            LogProviderSelectionControl.CollapseStateChanged += UsrLogProviderSelection_CollapseStateChanged;
            LogProviderSelectionControl.SizeChanged += (s, e) => AutoSizeLogProviderSelection();

            ActiveFilterOverviewControl.SizeChanged += (s, e) => UpdateActiveFiltersSplitter();
            ActiveFilterOverviewControl.FilterRemoved += ActiveFilterOverviewControl_FilterRemoved;
            ActiveFilterOverviewControl.ErrorChipClicked += ActiveFilterOverviewControl_ErrorChipClicked;
            ActiveFilterOverviewControl.ResetAllFilters += ActiveFilterOverviewControl_Reset;

            SearchControl.Search += UsrSearch_Search;
            SearchControl.SearchSettingsChanged += SearchControl_SearchSettingsChanged;

            SearchResultListControl.Close += (s,e) => HideBottomPanel();
            ErrorListControl.Close += (s, e) => HideBottomPanel();

            ShortcutManager.Register(this, AppShortcut.StartRecording, LogProcessingService.StartSingleFetch);
            ShortcutManager.Register(this, AppShortcut.StartTimedRecording, LogProcessingService.StartTimedFetch);
            ShortcutManager.Register(this, AppShortcut.StopRecording, LogProcessingService.StopFetching);

            ShortcutManager.Register(this, AppShortcut.ToggleCompactView, () => BtnCompactView_Click(this, EventArgs.Empty));
            ShortcutManager.Register(this, AppShortcut.ClearLog, () => BtnErase_Click(this, EventArgs.Empty));
            ShortcutManager.Register(this, AppShortcut.ResetApplication, () => BtnReset_Click(this, EventArgs.Empty));
            ShortcutManager.Register(this, AppShortcut.OpenConfiguration, () => BtnConfig_Click(this, EventArgs.Empty));
            ShortcutManager.Register(this, AppShortcut.CloseBottomPanel, HideBottomPanel);
            ShortcutManager.Register(this, AppShortcut.ToggleErrorsPanel, ToggleErrorsPanel);

            UpdateTimeLineVisibility();
        }
        private void FormLogScraper_Load(object sender, EventArgs e)
        {
            try
            {
                //Collapse the bottom panel here, so it is still shown in the designer
                SplitContainerViewportAndSearchResultList.Panel2Collapsed = true;

                //Enforce autosizing because the IDE overrides the control's autosize settings.
                LogProviderSelectionControl.AutoSize = false;
                SplitContainerActiveFiltersViewport.IsSplitterFixed = true;
                SplitContainerActiveFiltersViewport.FixedPanel = FixedPanel.Panel1;

                AutoSizeLogProviderSelection();
                UpdateActiveFiltersSplitter();
            }
            catch (Exception ex)
            {
                ex.LogStackTraceToFile();
                MessageBox.Show(ex.Message);
            }

            GitHubUpdateChecker.CheckForUpdateInSeperateThread();

            if (PreReleaseBadgeControl.Visible)
            {
                int badgeRight = PreReleaseBadgeControl.Left + PreReleaseBadgeControl.Width + 4;
                if (SplitContainerMain.SplitterDistance < badgeRight)
                    SplitContainerMain.SplitterDistance = badgeRight;
            }
        }

        private void ActiveFilterOverviewControl_Reset(object sender, EventArgs e)
        {
            LogMetadataFiltersOverviewControl.ResetFilters();
            ContentNavigationControl.ResetFilters();
            LogAppState.Instance.Range.ForceSet(LogRange.Full);
        }

        private void ActiveFilterOverviewControl_ErrorChipClicked(object sender, EventArgs e)
        {
            SearchResultListControl.Visible = false;
            ErrorListControl.Visible = true;
            ErrorListControl.ShowEntries(true);
            SplitContainerViewportAndSearchResultList.Panel2Collapsed = false;
            SplitContainerViewportAndSearchResultList.TextSplitter = "Errors";
            SplitContainerViewportAndSearchResultList.Expand();
        }

        private void UsrLogProviderSelection_CollapseStateChanged(object sender, EventArgs e)
        {
            AutoSizeLogProviderSelection();
        }

        private void AutoSizeLogProviderSelection()
        {
            int buttonsHeight = SplitContainerSourceControlAndLogProviders.SplitterDistance
                + SplitContainerSourceControlAndLogProviders.SplitterWidth;

            SplitContainerSourceControlAndMetadata.SuspendLayout();
            SplitContainerSourceControlAndMetadata.Panel1.SuspendLayout();
            SplitContainerSourceControlAndMetadata.SplitterDistance = LogProviderSelectionControl.Height + buttonsHeight;
            SplitContainerSourceControlAndMetadata.Panel1.ResumeLayout(true);
            SplitContainerSourceControlAndMetadata.ResumeLayout(true);
        }

        private void UsrLogProviderSelection_UriChanged(object sender, string e)
        {
            string title = "LogScraper";
            if (!string.IsNullOrWhiteSpace(e)) title += $" - {e}";
            Text = title;
        }

        private void SearchControl_SearchSettingsChanged(SearchSettings settings)
        {
            if (SearchControl.SelectedSearchMode == SearchControl.SearchMode.All)
            {
                ErrorListControl.Visible = false;
                SearchResultListControl.Visible = true;
                SplitContainerViewportAndSearchResultList.Panel2Collapsed = false;
                SplitContainerViewportAndSearchResultList.TextSplitter = $"Search results ({settings.SearchText})";
                SplitContainerViewportAndSearchResultList.Expand();
            }
            if (SearchResultListControl.Visible) SearchResultListControl.UpdateSearchResults(settings);
        }

        private void HideBottomPanel()
        {
            SplitContainerViewportAndSearchResultList.Panel2Collapsed = true;
        }

        private void ToggleErrorsPanel()
        {
            if (!SplitContainerViewportAndSearchResultList.Panel2Collapsed && ErrorListControl.Visible)
            {
                HideBottomPanel();
                return;
            }
            ActiveFilterOverviewControl_ErrorChipClicked(this, EventArgs.Empty);
        }

        private void ActiveFilterOverviewControl_FilterRemoved(object sender, FilterRemovedEventArgs e)
        {
            LogMetadataFiltersOverviewControl.RemoveFilter(e.Property, e.SingleValue);
        }

        private void UpdateActiveFiltersSplitter()
        {
            int desired = ActiveFilterOverviewControl.Height;
            if (desired < SplitContainerActiveFiltersViewport.Panel1MinSize) return;
            if (SplitContainerActiveFiltersViewport.SplitterDistance == desired) return;
            SplitContainerActiveFiltersViewport.SplitterDistance = desired;
        }

        #endregion

        #region Erase and reset

        private void LogAppState_ResetRequested(object sender, ResetEventArgs e)
        {
            SplitContainerViewportAndSearchResultList.Panel2Collapsed = true;
            UpdateButtonStatus();
        }

        #endregion

        #region User controls event handling

        private void UpdateTimeLineVisibility()
        {
            SplitContainerTimeLineAndViewport.Panel1Collapsed = !ConfigAppState.Instance.GenericConfig.Value.ShowTimelineByDefault;
        }

        private void UsrSearch_Search(SearchSettings searchSettings)
        {
            SearchControl.Enabled = false;
            try
            {
                ScintillaControlExtensions.SearchDirection searchDirection = searchSettings.Direction == SearchControl.SearchDirection.Forward ? ScintillaControlExtensions.SearchDirection.Forward : ScintillaControlExtensions.SearchDirection.Backward;
                bool found = LogViewportControl.TrySearch(searchSettings.SearchText, searchSettings.WholeWord, searchSettings.CaseSensitive, searchSettings.WrapAround, searchDirection);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while searching: " + ex.Message);
                ex.LogStackTraceToFile("Error while searching in log.");
            }
            finally
            {
                SearchControl.Enabled = true;
            }
        }

        #endregion

        #region Buttons
        private void UpdateButtonStatus()
        {
            bool isSourceProcessingActive = LogAppState.Instance.ProcessingState.Value.IsActive;
            bool sourceIsValid = LogAppState.Instance.IsSourceValid.Value;
            bool layoutSelected = LogAppState.Instance.Layout.Value != null;

            BtnFormRecord.Enabled = sourceIsValid && layoutSelected;
            BtnConfig.Enabled = !isSourceProcessingActive;
        }

        public void BtnErase_Click(object sender, EventArgs e)
        {
            LogAppState.Instance.Reset(keepFilters: true);
        }

        public void BtnReset_Click(object sender, EventArgs e)
        {
            LogAppState.Instance.Reset(keepFilters: false);
        }

        private void BtnCompactView_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            FormCompactView.Instance.ShowForm();
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            new FormConfiguration().ShowDialog(this);
        }

        #endregion

        #region Application exit cleanup

        /// <summary>
        /// Offers to remove the default export file (if still present on disk) when the application closes,
        /// since it may contain sensitive log data that was written on demand for the text editor feature.
        /// </summary>
        private void FormLogScraper_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ConfigAppState.Instance.GenericConfig?.Value?.CheckForExternalViewingFilesOnExit == true)
            {
                OpenLogInTextEditor.AskToRemoveTemporaryFile();
            }
        }

        #endregion

        #region Form key shortcuts

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ShortcutManager.ProcessKey(this, keyData))
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
    }
}