using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Controls;
using LogScraper.Controls.FilterOverview;
using LogScraper.Controls.Search;
using LogScraper.Controls.Viewport;
using LogScraper.Export;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Rendering;
using LogScraper.Utilities;
using LogScraper.Utilities.Extensions;

namespace LogScraper
{
    //TODO: fix issue when no valid runtime is selected that you cannot record, the issource valid should be pushed better to the record buttons

    //TODO: Fix keeping viewport logentry visible, doesnt work well with for example processing

    //TODO: color additional log lines?
    //TODO: Add key shortcuts like F3/shift F3
    public partial class FormLogScraper : Form
    {
        #region Form Initialization
        public FormLogScraper()
        {
            // Capture the UI SynchronizationContext before any background work can start,
            // so StateSlice marshals Changed events back to this thread automatically.
            StateSlice.SetSynchronizationContext();

            InitializeComponent();

            CultureInfo culture = new("nl");
            Thread.CurrentThread.CurrentUICulture = culture;

            LogAppState.Instance.ResetRequested += LogAppState_ResetRequested;
            LogAppState.Instance.IsSourceProcessingActive.Changed += (s, e) => UpdateButtonStatus();
            LogAppState.Instance.IsSourceValid.Changed += (s, e) => UpdateButtonStatus();
            LogAppState.Instance.StatusMessage.Changed += (s, e) => HandleErrorMessages();

            ConfigAppState.Instance.GenericConfig.Changed += (s, e) => ApplyGenericConfig();

            FormCompactView.Instance.ReturnToMainFormRequested += (s, e) => WindowState = FormWindowState.Normal;

            LogProviderSelectionControl.UriChanged += UsrRuntime_UriChanged;
            LogProviderSelectionControl.CollapseStateChanged += UsrLogProviderSelection_CollapseStateChanged;

            LogViewportControl.LogEntriesTextChanged += UserControlLogEntriesTextBox_LogEntriesTextBoxTextChanged;

            ActiveFilterOverviewControl.SizeChanged += (s, e) => RepositionLogEntriesTextBox();
            ActiveFilterOverviewControl.FilterRemoved += ActiveFilterOverviewControl_FilterRemoved;
            ActiveFilterOverviewControl.RangeRemoved += ActiveFilterOverviewControl_RangeRemoved;
            ActiveFilterOverviewControl.ErrorChipClicked += ActiveFilterOverviewControl_ErrorChipClicked;
            ActiveFilterOverviewControl.ResetAllFilters += ActiveFilterOverviewControl_Reset;

            PnlFiltersAndLogEntriesTextBox.SizeChanged += (s, e) => RepositionLogEntriesTextBox();

            SearchControl.Search += UsrSearch_Search;
            SearchControl.SearchSettingsChanged += SearchControl_SearchSettingsChanged;

            SearchResultListControl.Close += SearchResultListControl_Close;

            ErrorListControl.Close += (s, e) => HideBottomPanel();

            UpdateTimeLineVisibility();
        }
        private void FormLogScraper_Load(object sender, EventArgs e)
        {
            try
            {
                //Collapse the bottom panel here, so it is still shown in the designer
                SplitContainerViewportAndSearchResultList.Panel2Collapsed = true;
                btnOpenWithEditor.Enabled = ConfigAppState.Instance.GenericConfig.Value.ExportToFile;

                //Enforce autosizing because the IDE overrides the control's autosize settings.
                LogProviderSelectionControl.AutoSize = false;

                AutoSizeLogProviderSelection();
            }
            catch (Exception ex)
            {
                ex.LogStackTraceToFile();
                MessageBox.Show(ex.Message);
            }

            RepositionLogEntriesTextBox();
            GitHubUpdateChecker.CheckForUpdateInSeperateThread();
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
            // Force the control to recalculate its size
            LogProviderSelectionControl.PerformLayout();

            // Suspend layout to avoid flickering
            SplitContainerSourceControlAndMetadata.SuspendLayout();
            SplitContainerSourceControlAndMetadata.Panel1.SuspendLayout();
            SplitContainerSourceControlAndMetadata.SplitterDistance = (LogProviderSelectionControl.IsCollapsed ? LogProviderSelectionControl.CollapsedHeight : LogProviderSelectionControl.ExpandedHeight) + 3 + LogRecordingControl.Bottom;
            SplitContainerSourceControlAndMetadata.Panel1.ResumeLayout(true);
            SplitContainerSourceControlAndMetadata.ResumeLayout(true);
        }

        private void UsrRuntime_UriChanged(object sender, string e)
        {
            string title = "LogScraper";
            if (!string.IsNullOrWhiteSpace(e)) title += $" - {e}";
            Text = title;
        }

        private void SearchResultListControl_Close(object sender, EventArgs e)
        {
            HideBottomPanel();
        }

        private void SearchControl_SearchSettingsChanged(SearchSettings settings)
        {
            if (SearchControl.SelectedSearchMode == SearchControl.SearchMode.All)
            {
                ErrorListControl.Visible = false;
                SearchResultListControl.Visible = true;
                SplitContainerViewportAndSearchResultList.Panel2Collapsed = false;
                SplitContainerViewportAndSearchResultList.TextSplitter = $"Zoekresultaten ({settings.SearchText})";
                SplitContainerViewportAndSearchResultList.Expand();
            }
            if (SearchResultListControl.Visible) SearchResultListControl.UpdateSearchResults(settings);
        }

        private void HideBottomPanel()
        {
            SplitContainerViewportAndSearchResultList.Panel2Collapsed = true;
        }

        private void ActiveFilterOverviewControl_FilterRemoved(object sender, FilterRemovedEventArgs e)
        {
            LogMetadataFiltersOverviewControl.RemoveFilter(e.Property, e.SingleValue);
        }

        private void ActiveFilterOverviewControl_RangeRemoved(object sender, RangeRemovedEventArgs e)
        {
            if (e.Variant == LogRangeChipVariant.Begin)
                LogRangeSelectionControl.ClearBegin();
            else
                LogRangeSelectionControl.ClearEnd();
        }

        private bool _repositioningTextBox;
        private void RepositionLogEntriesTextBox()
        {
            if (_repositioningTextBox) return;
            _repositioningTextBox = true;
            try
            {
                PnlFiltersAndLogEntriesTextBox.SuspendLayout();
                ActiveFilterOverviewControl.Top = 2;
                int top = ActiveFilterOverviewControl.Bottom + 5;
                LogViewportControl.SetBounds(0, top, PnlFiltersAndLogEntriesTextBox.ClientSize.Width, Math.Max(0, PnlFiltersAndLogEntriesTextBox.ClientSize.Height - top));
                PnlFiltersAndLogEntriesTextBox.ResumeLayout(false);
            }
            finally { _repositioningTextBox = false; }
        }

        private void UserControlLogEntriesTextBox_LogEntriesTextBoxTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LogViewportControl.Text)) return;
            LogExportWorkerManager.WriteToFile(LogViewportControl.Text);
        }

        #endregion

        #region Erase and reset

        private void LogAppState_ResetRequested(object sender, ResetEventArgs e)
        {
            if (!e.KeepFilters)
            {
                TxtErrorMessage.Text = string.Empty;
                TxtErrorMessage.Visible = false;
            }

            SplitContainerViewportAndSearchResultList.Panel2Collapsed = true;
            UpdateButtonStatus();
        }

        #endregion

        #region User controls event handling

        private void ApplyGenericConfig()
        {
            btnOpenWithEditor.Enabled = ConfigAppState.Instance.GenericConfig.Value.ExportToFile;
            SplitContainerTimeLineAndViewport.Panel1Collapsed = !ConfigAppState.Instance.GenericConfig.Value.ShowTimelineByDefault;
        }

        private void UpdateTimeLineVisibility()
        {
            SplitContainerTimeLineAndViewport.Panel1Collapsed = !ConfigAppState.Instance.GenericConfig.Value.ShowTimelineByDefault;
        }

        private void HandleErrorMessages()
        {
            TxtErrorMessage.Text = LogAppState.Instance.StatusMessage.Value.Message;
            TxtErrorMessage.Visible = !LogAppState.Instance.StatusMessage.Value.IsSuccess;
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
                MessageBox.Show("Fout tijdens zoeken: " + ex.Message);
                ex.LogStackTraceToFile("Fout tijdens zoeken in log.");
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
            bool isSourceProcessingActive = LogAppState.Instance.IsSourceProcessingActive.Value;
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

        public void BtnOpenWithEditor_Click(object sender, EventArgs e) =>
            LogExportWorkerManager.OpenFileInExternalEditor();

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

        #region Form key shortcuts

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.R))
            {
                BtnCompactView_Click(this, EventArgs.Empty);
                return true;
            }
            if (keyData == (Keys.Control | Keys.F))
            {
                SearchControl.Focus();
                return true;
            }

            // TODO: Refactor key shortcuts
            //if (keyData == (Keys.Control | Keys.S))
            //{
            //    if (SourceProcessingManager.Instance.IsWorkerActive)
            //        BtnStop_Click(this, EventArgs.Empty);
            //    else
            //        BtnRecordWithTimer_Click(this, EventArgs.Empty);
            //    return true;
            //}
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
    }
}