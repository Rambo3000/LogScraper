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
using LogScraper.Log;
using LogScraper.Log.Layout;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Processing;
using LogScraper.Log.Processing.RawLogParsing;
using LogScraper.Log.Rendering;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Workers;
using LogScraper.Utilities;
using LogScraper.Utilities.Extensions;

namespace LogScraper
{
    //TODO: record buttons to seperate control

    //TODO: compact form can be lose connection to the main form
    //TODO: move configuration changed status to AppState, also change the questioning when configuration has changed

    //TODO: Fix keeping viewport logentry visible, doesnt work well with for example processing

    //TODO: color additional log lines?
    //TODO: Add key shortcuts like F3/shift F3
    public partial class FormLogScraper : Form
    {
        #region Form Initialization
        public FormLogScraper()
        {
            InitializeComponent();

            CultureInfo culture = new("nl");
            Thread.CurrentThread.CurrentUICulture = culture;

            FormCompactView.Instance.SetFormLogScraper(this);

            LogAppState.Instance.ResetRequested += LogAppState_ResetRequested;

            LogProviderSelectionControl.UriChanged += UsrRuntime_UriChanged;
            LogProviderSelectionControl.CollapseStateChanged += UsrLogProviderSelection_CollapseStateChanged;

            LogAppState.Instance.IsSourceProcessingActive.Changed += (s, e) => UpdateButtonStatus();
            LogAppState.Instance.IsSourceValid.Changed += (s, e) => UpdateButtonStatus();
            LogAppState.Instance.StatusMessage.Changed += (s, e) => HandleErrorMessages();

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
            SetDynamicToolTips();
        }
        private void FormLogScraper_Load(object sender, EventArgs e)
        {
            try
            {
                //Collapse the bottom panel here, so it is still shown in the designer
                SplitContainerViewportAndSearchResultList.Panel2Collapsed = true;
                btnOpenWithEditor.Enabled = ConfigurationManager.GenericConfig.ExportToFile;

                LogProviderSelectionControl.IsPinned = ConfigurationManager.GenericConfig.PinLogProvidersByDefault;
                LogProviderSelectionControl.PopulateLogProviders();
                LogProviderSelectionControl.PopulateLogLayouts([.. ConfigurationManager.LogLayouts]);
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
            SplitContainerSourceControlAndMetadata.SplitterDistance = (LogProviderSelectionControl.IsCollapsed ? LogProviderSelectionControl.CollapsedHeight : LogProviderSelectionControl.ExpandedHeight) + 3 + BtnRecord.Bottom;
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

        #region Initiate getting raw log and processing of raw log

        private void FetchRawLogAsync(int intervalInSeconds = -1, int durationInSeconds = -1)
        {
            if (LogAppState.Instance.Layout.Value == null) return;
            try
            {
                BtnRecord.Enabled = false;
                BtnRecordWithTimer.Enabled = false;
                LogAppState.Instance.ProcessingStatus.Set(ProcessingStatus.Retrieving);
                FormCompactView.Instance.UpdateButtonsFromMainWindow();
                Application.DoEvents();

                ISourceAdapter logProvider = LogProviderSelectionControl.GetSelectedSourceAdapter(LogAppState.Instance.LastTrailTime.Value);

                SourceProcessingWorker sourceProcessingWorker = new();
                sourceProcessingWorker.DownloadCompleted += ProcessRawLog;
                sourceProcessingWorker.ProgressUpdate += HandleSourceProcessingWorkerProgressUpdate;
                SourceProcessingManager.Instance.AddWorker(sourceProcessingWorker, logProvider, intervalInSeconds, durationInSeconds);
            }
            catch (Exception ex)
            {
                ShowException(ex);
                UpdateButtonStatus();
            }
            finally
            {
                FormCompactView.Instance.UpdateButtonsFromMainWindow();
            }
        }

        private void ProcessRawLog(string[] rawLog, DateTime? updatedLastTrailTime, bool isContinuous)
        {
            try
            {
                LogLayout logLayout = LogAppState.Instance.Layout.Value;
                LogCollection logCollection = LogAppState.Instance.LogCollection.Value ?? new();

                bool newLogEntriesReceived = false;
                try
                {
                    LogAppState.Instance.ProcessingStatus.Set(ProcessingStatus.Processing);
                    newLogEntriesReceived = RawLogParser.TryParseAndAppendLogEntries(rawLog, logCollection, logLayout);
                }
                catch (Exception ex)
                {
                    ex.LogStackTraceToFile("Fout tijdens parsen van raw log.");
                    LogViewportControl.Text = RawLogParser.JoinRawLogIntoString(rawLog);
                    throw;
                }

                if (newLogEntriesReceived)
                {
                    LogEntryClassifier.Classify(logLayout, logCollection);
                    LogAppState.Instance.LogCollection.ForceSet(logCollection);
                }
                LogAppState.Instance.StatusMessage.Set((string.Empty, true));
                FormCompactView.Instance.UpdateButtonsFromMainWindow();
                LogAppState.Instance.LastTrailTime.Set(updatedLastTrailTime);
                if (isContinuous) LogAppState.Instance.ProcessingStatus.Set(ProcessingStatus.Waiting);
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private static void ShowException(Exception ex)
        {
            ex.LogStackTraceToFile();
            LogAppState.Instance.StatusMessage.Set((ex.Message, false));
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

        private void UpdateTimeLineVisibility()
        {
            SplitContainerTimeLineAndViewport.Panel1Collapsed = !ConfigurationManager.GenericConfig.ShowTimelineByDefault;
        }

        private void HandleErrorMessages()
        {
            TxtErrorMessage.Text = LogAppState.Instance.StatusMessage.Value.Message;
            TxtErrorMessage.Visible = !LogAppState.Instance.StatusMessage.Value.IsSuccess;
        }

        private void HandleSourceProcessingWorkerProgressUpdate(int elapsedSeconds, int totalDurationInSeconds)
        {
            if (totalDurationInSeconds == -1)
            {
                BtnRecordWithTimer.Text = string.Empty;
                BtnRecordWithTimer.Image = Properties.Resources.timer_record_outline_24x24;
            }
            else
            {
                LogAppState.Instance.ProcessingStatus.Set(ProcessingStatus.Retrieving);
                TimeSpan tijd = TimeSpan.FromSeconds(totalDurationInSeconds - elapsedSeconds);
                BtnRecordWithTimer.Image = null;
                BtnRecordWithTimer.Text = string.Format("{0}:{1:D2}", (int)tijd.TotalMinutes, tijd.Seconds);
            }
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

        private void SetDynamicToolTips()
        {
            ToolTip.SetToolTip(BtnRecordWithTimer, "Lees " + ConfigurationManager.GenericConfig.AutomaticReadTimeMinutes.ToString() + " minuten [CTRL-S]");
        }
        #endregion

        #region Buttons

        private void UpdateButtonStatus()
        {
            bool isSourceProcessingActive = LogAppState.Instance.IsSourceProcessingActive.Value;
            bool sourceIsValid = LogAppState.Instance.IsSourceValid.Value;
            bool layoutSelected = LogAppState.Instance.Layout.Value != null;
            if (!isSourceProcessingActive)
            {
                HandleSourceProcessingWorkerProgressUpdate(-1, -1);
                LogAppState.Instance.ProcessingStatus.Set(ProcessingStatus.Idle);
            }

            BtnRecord.Visible = !isSourceProcessingActive;
            BtnRecord.Enabled = !isSourceProcessingActive && sourceIsValid && layoutSelected;
            BtnRecordWithTimer.Enabled = !isSourceProcessingActive && sourceIsValid && layoutSelected;
            BtnFormRecord.Enabled = sourceIsValid && layoutSelected;
            BtnStop.Visible = isSourceProcessingActive;
            BtnStop.Enabled = isSourceProcessingActive;
            BtnConfig.Enabled = !isSourceProcessingActive;

            FormCompactView.Instance.UpdateButtonsFromMainWindow();
        }

        public void BtnRecord_Click(object sender, EventArgs e)
        {
            FetchRawLogAsync();
        }

        public void BtnRecordWithTimer_Click(object sender, EventArgs e) =>
            FetchRawLogAsync(1, ConfigurationManager.GenericConfig.AutomaticReadTimeMinutes * 60);

        public void BtnStop_Click(object sender, EventArgs e)
        {
            BtnStop.Enabled = false;
            SourceProcessingManager.Instance.CancelAllWorkers();
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
            if (!SourceProcessingManager.Instance.IsWorkerActive) BtnRecordWithTimer_Click(sender, e);
            FormCompactView.Instance.ShowForm();
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            using FormConfiguration form = new();

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                form.GetConfigurationChangedStatus(out bool genericConfigChanged, out bool logLayoutsChanged, out bool kubernetesChanged, out bool runtimeChanged, out _);

                if (genericConfigChanged)
                {
                    SetDynamicToolTips();
                    btnOpenWithEditor.Enabled = ConfigurationManager.GenericConfig.ExportToFile;
                    UpdateTimeLineVisibility();
                }

                if (logLayoutsChanged) LogProviderSelectionControl.PopulateLogLayouts([.. ConfigurationManager.LogLayouts]);

                if (kubernetesChanged || runtimeChanged)
                {
                    if (MessageBox.Show("De instellingen zijn gewijzigd. Wil je deze direct toepassen? Hierdoor wordt het log gereset", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        LogProviderSelectionControl.UpdateProviderConfig();
                        LogAppState.Instance.Reset(keepFilters: false);
                    }
                }
            }
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
            if (keyData == (Keys.Control | Keys.S))
            {
                if (SourceProcessingManager.Instance.IsWorkerActive)
                    BtnStop_Click(this, EventArgs.Empty);
                else
                    BtnRecordWithTimer_Click(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
    }
}