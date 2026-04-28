using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Controls;
using LogScraper.Controls.FilterOverview;
using LogScraper.Controls.Viewport;
using LogScraper.Controls.Search;
using LogScraper.Export;
using LogScraper.Log;
using LogScraper.Log.Filtering;
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
    //TODO: REQUIRED move bookmarks to LogAppState
    //TODO: search list use collapsed splitcontainer by default
    //TODO: reduce flickering on filter overview control
    //TODO: highlighting of visible log entry range and selected entry in navigation filters
    //TODO: navigatie sync optie met log
    //TODO: log provider selection enable/disable aanpassen zodat je m wel kunt openklappen

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

            SourceProcessingManager.Instance.QueueLengthUpdate += HandleLogProviderManagerQueueUpdate;

            LogAppState.Instance.ResetRequested += LogAppState_ResetRequested;

            LogProviderSelectionControl.StatusUpdate += (s, e) => HandleErrorMessages(e.message, e.isSuccess);
            LogProviderSelectionControl.UriChanged += UsrRuntime_UriChanged;
            LogProviderSelectionControl.IsSourceValidChanged += HandleIsSourceValidChanged;
            LogProviderSelectionControl.CollapseStateChanged += UsrLogProviderSelection_CollapseStateChanged;

            LogViewportControl.LogEntriesTextChanged += UserControlLogEntriesTextBox_LogEntriesTextBoxTextChanged;

            BookMarksControl.BookmarksChanged += BookMarksControl_BookmarksChanged;

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

        private void ActiveFilterOverviewControl_Reset(object sender, EventArgs e)
        {
            LogMetadataFiltersOverviewControl.ResetFilters();
            ContentNavigationControl.ResetFilters();
            LogAppState.Instance.Range.ForceSet(LogRange.Full);
        }

        private void ActiveFilterOverviewControl_ErrorChipClicked(object sender, EventArgs e)
        {
            SearchResultListControl.Visible = false;
            ErrorListControl.ShowEntries();
            ErrorListControl.Visible = true;
            SplitContainerViewportAndSearchResultList.Panel2Collapsed = false;
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

        private void BookMarksControl_BookmarksChanged(object sender, EventArgs e)
        {
            LogViewportControl.UpdateBookMarks(BookMarksControl.Bookmarks);
            LogTimeLineControl.SetBookmarks([.. BookMarksControl.Bookmarks]);
        }

        private void UserControlLogEntriesTextBox_LogEntriesTextBoxTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LogViewportControl.Text)) return;
            LogExportWorkerManager.WriteToFile(LogViewportControl.Text);
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
        #endregion

        #region Initiate getting raw log and processing of raw log

        private DateTime? lastTrailTime = null;

        private void FetchRawLogAsync(int intervalInSeconds = -1, int durationInSeconds = -1)
        {
            if (LogAppState.Instance.Layout.Value == null) return;
            try
            {
                BtnRecord.Enabled = false;
                BtnRecordWithTimer.Enabled = false;
                LogProviderSelectionControl.UpdateStatus(LogProviderSelectionControl.StatusType.Retrieving);
                FormCompactView.Instance.UpdateButtonsFromMainWindow();
                Application.DoEvents();

                ISourceAdapter logProvider = LogProviderSelectionControl.GetSelectedSourceAdapter(lastTrailTime);

                SourceProcessingWorker sourceProcessingWorker = new();
                sourceProcessingWorker.DownloadCompleted += ProcessRawLog;
                sourceProcessingWorker.StatusUpdate += HandleErrorMessages;
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

        private void ProcessRawLog(string[] rawLog, DateTime? updatedLastTrailTime)
        {
            try
            {
                LogLayout logLayout = LogAppState.Instance.Layout.Value;
                LogCollection logCollection = LogAppState.Instance.LogCollection.Value ?? new();

                bool newLogEntriesReceived = false;
                try
                {
                    LogProviderSelectionControl.UpdateStatus(LogProviderSelectionControl.StatusType.Processing);
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

                HandleErrorMessages(string.Empty, true);
                FormCompactView.Instance.UpdateButtonsFromMainWindow();
                lastTrailTime = updatedLastTrailTime;
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private void ShowException(Exception ex)
        {
            ex.LogStackTraceToFile();
            HandleErrorMessages(ex.Message, false);
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

            UpdateButtonStatus();
            lastTrailTime = null;
            HandleErrorMessages(string.Empty, true);
            SplitContainerViewportAndSearchResultList.Panel2Collapsed = true;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion

        #region User controls event handling

        private void UpdateTimeLineVisibility()
        {
            SplitContainerTimeLineAndViewport.Panel1Collapsed = !ConfigurationManager.GenericConfig.ShowTimelineByDefault;
        }

        private void HandleErrorMessages(string message, bool isSucces)
        {
            TxtErrorMessage.Text = message;
            TxtErrorMessage.Visible = !isSucces;
        }

        private void HandleLogProviderManagerQueueUpdate()
        {
            UpdateButtonStatus();
        }

        private void HandleIsSourceValidChanged(object sender, bool e)
        {
            UpdateButtonStatus();
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
                LogProviderSelectionControl.UpdateStatus(LogProviderSelectionControl.StatusType.Retrieving);
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
            //TODO: Add worker status to AppState
            bool downloadingInProgress = SourceProcessingManager.Instance.IsWorkerActive;
            bool sourceIsValid = LogProviderSelectionControl.IsSourceValid;
            bool layoutSelected = LogAppState.Instance.Layout.Value != null;
            if (!downloadingInProgress)
            {
                HandleSourceProcessingWorkerProgressUpdate(-1, -1);
                LogProviderSelectionControl.UpdateStatus(LogProviderSelectionControl.StatusType.Finished);
            }

            BtnRecord.Visible = !downloadingInProgress;
            BtnRecord.Enabled = !downloadingInProgress && sourceIsValid && layoutSelected;
            BtnRecordWithTimer.Enabled = !downloadingInProgress && sourceIsValid && layoutSelected;
            BtnFormRecord.Enabled = sourceIsValid && layoutSelected;
            BtnStop.Visible = downloadingInProgress;
            BtnStop.Enabled = downloadingInProgress;
            BtnConfig.Enabled = !downloadingInProgress;
            LogProviderSelectionControl.Enabled = !downloadingInProgress;
            LogProviderSelectionControl.SetEnabled(!downloadingInProgress);

            FormCompactView.Instance.UpdateButtonsFromMainWindow();
        }

        public void BtnRecord_Click(object sender, EventArgs e) => FetchRawLogAsync();

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