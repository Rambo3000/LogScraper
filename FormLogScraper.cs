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
using LogScraper.Controls.LogEntriesTextbox;
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
    //TODO: change error overview to only show errors in visible log entries
    //TODO: highlighting of visible log entry (range) in navigation filters
    //TODO: navigatie sync optie met log
    //TODO: log provider selection enable/disable aanpassen zodat je m wel kunt openklappen

    //TODO: color additional log lines?
    //TODO: Add key shortcuts like F3/shift F3
    public partial class FormLogScraper : Form
    {
        #region Form Initialization
        //TODO: remove rendersettings
        private LogRenderSettings currentRenderSettings;
        public FormLogScraper()
        {
            InitializeComponent();

            CultureInfo culture = new("nl");
            Thread.CurrentThread.CurrentUICulture = culture;

            FormCompactView.Instance.SetFormLogScraper(this);

            SourceProcessingManager.Instance.QueueLengthUpdate += HandleLogProviderManagerQueueUpdate;

            UsrLogProviderSelection.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            UsrLogProviderSelection.StatusUpdate += (s, e) => HandleErrorMessages(e.message, e.isSuccess);
            UsrLogProviderSelection.UriChanged += UsrRuntime_UriChanged;
            UsrLogProviderSelection.IsSourceValidChanged += HandleIsSourceValidChanged;
            UsrLogProviderSelection.LogProviderChanged += UsrLogProviderSelection_LogProviderChanged;
            UsrLogProviderSelection.CollapseStateChanged += UsrLogProviderSelection_CollapseStateChanged;

            UserControlContentFilter.SelectedItemChanged += HandleLogContentFilterSelectedItemChanged;

            UserControlLogEntriesTextBox.LogEntriesTextChanged += UserControlLogEntriesTextBox_LogEntriesTextBoxTextChanged;
            UserControlLogEntriesTextBox.VisibleRangeChanged += UserControlLogEntriesTextBox_VisibleRangeChanged;
            UserControlLogEntriesTextBox.LogEntryAtCursorChanged += UserControlLogEntriesTextBox_LogEntryAtCursorChanged;

            LogTimeLineControl.CellClicked += LogTimelineControl_CellClicked;
            LogTimeLineControl.ErrorMarkerClicked += LogTimelineControl_CellClicked;
            LogTimeLineControl.BookmarkMarkerClicked += LogTimelineControl_CellClicked;

            BookMarksControl.NavigateToEntryRequested += BookMarksControl_NavigateToEntryRequested;
            BookMarksControl.BookmarksChanged += BookMarksControl_BookmarksChanged;

            LogAppState.Instance.LogRange.Changed += (s, e) => UpdateLogRange(true);
            LogAppState.Instance.ResetRequested += LogAppState_ResetRequested;

            activeFilterOverviewControl.SizeChanged += (s, e) => RepositionLogEntriesTextBox();
            activeFilterOverviewControl.FilterRemoved += ActiveFilterOverviewControl_FilterRemoved;
            activeFilterOverviewControl.RangeRemoved += ActiveFilterOverviewControl_RangeRemoved;
            activeFilterOverviewControl.ErrorChipClicked += ActiveFilterOverviewControl_ErrorChipClicked;
            activeFilterOverviewControl.ResetAllFilters += ActiveFilterOverviewControl_Reset;

            PnlFiltersAndLogEntriesTextBox.SizeChanged += (s, e) => RepositionLogEntriesTextBox();

            flowTreeControl1.ShowTreeStateChanged += FlowTreeControl_ShowTreeStateChanged;

            //TODO: REQUIRED Render log on MetadataFormatingControl_SelectionChanged
            //LogPostProcessing.PostProcessingResultsChanged += LogPostProcessing_PostProcessingResultsChanged;

            UserControlSearch.Search += UsrSearch_Search;
            UserControlSearch.SearchSettingsChanged += SearchControl_SearchSettingsChanged;

            SearchResultListControl.ResultSelected += SearchResultListControl_ResultSelected;
            SearchResultListControl.Close += SearchResultListControl_Close;

            errorListControl.ResultSelected += (s, e) => { UserControlLogEntriesTextBox.SelectedLogEntry = e; };
            errorListControl.Close += (s, e) => HideBottomPanel();

            UpdateTimeLineVisibility();
            SetDynamicToolTips();
        }

        private void ActiveFilterOverviewControl_Reset(object sender, EventArgs e)
        {
            UsrMetadataFilterOverview.ResetFilters();
            UserControlContentFilter.ResetFilters();
            LogAppState.Instance.LogRange.ForceSet(LogRange.Full);
            LogViewport.Reset();
        }

        private void ActiveFilterOverviewControl_ErrorChipClicked(object sender, ErrorChipClickedEventArgs e)
        {
            ShowErrorPanel([.. e.ErrorEntries]);
        }

        private void UsrLogProviderSelection_CollapseStateChanged(object sender, EventArgs e)
        {
            AutoSizeLogProviderSelection();
        }

        private void AutoSizeLogProviderSelection()
        {
            // Force the control to recalculate its size
            UsrLogProviderSelection.PerformLayout();

            // Suspend layout to avoid flickering
            splitContainer3.SuspendLayout();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.SplitterDistance = (UsrLogProviderSelection.IsCollapsed ? UsrLogProviderSelection.CollapsedHeight : UsrLogProviderSelection.ExpandedHeight) + 3 + BtnRecord.Bottom;
            splitContainer3.Panel1.ResumeLayout(true);
            splitContainer3.ResumeLayout(true);
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
            if (UserControlSearch.SelectedSearchMode == UserControlSearch.SearchMode.All)
            {
                ShowSearchPanel();
            }

            SearchResultListControl.UpdateSearchResults(settings);
        }

        private void ShowSearchPanel()
        {
            errorListControl.Visible = false;
            SearchResultListControl.Visible = true;
            splitContainer5.Panel2Collapsed = false;
        }

        private void ShowErrorPanel(List<LogEntry> entries)
        {
            SearchResultListControl.Visible = false;
            errorListControl.ShowEntries(entries, currentRenderSettings);
            errorListControl.Visible = true;
            splitContainer5.Panel2Collapsed = false;
        }

        private void HideBottomPanel()
        {
            splitContainer5.Panel2Collapsed = true;
        }

        private void SearchResultListControl_ResultSelected(object sender, LogEntry e)
        {
            UserControlContentFilter.ClearSelectedLogEntry();
            UserControlLogEntriesTextBox.SelectedLogEntry = e;
        }

        private void FlowTreeControl_ShowTreeStateChanged(object sender, EventArgs e)
        {
            RenderLog(LogAppState.Instance.MetadataFilterResult.Value);
        }


        private void ActiveFilterOverviewControl_FilterRemoved(object sender, FilterRemovedEventArgs e)
        {
            UsrMetadataFilterOverview.RemoveFilter(e.Property, e.SingleValue);
        }

        private void ActiveFilterOverviewControl_RangeRemoved(object sender, RangeRemovedEventArgs e)
        {
            if (e.Variant == LogRangeChipVariant.Begin)
                LogViewport.ClearBegin();
            else
                LogViewport.ClearEnd();
        }

        private bool _repositioningTextBox;
        private void RepositionLogEntriesTextBox()
        {
            if (_repositioningTextBox) return;
            _repositioningTextBox = true;
            try
            {
                PnlFiltersAndLogEntriesTextBox.SuspendLayout();
                activeFilterOverviewControl.Top = 2;
                int top = activeFilterOverviewControl.Bottom + 5;
                UserControlLogEntriesTextBox.SetBounds(0, top, PnlFiltersAndLogEntriesTextBox.ClientSize.Width, Math.Max(0, PnlFiltersAndLogEntriesTextBox.ClientSize.Height - top));
                PnlFiltersAndLogEntriesTextBox.ResumeLayout(false);
            }
            finally { _repositioningTextBox = false; }
        }
        private void UpdateLogRange(bool render)
        {
            if (render) RenderLog(LogAppState.Instance.MetadataFilterResult.Value);
        }

        private void BookMarksControl_BookmarksChanged(object sender, EventArgs e)
        {
            UserControlLogEntriesTextBox.UpdateBookMarks(BookMarksControl.Bookmarks);
            LogTimeLineControl.SetBookmarks([.. BookMarksControl.Bookmarks]);
        }

        private void BookMarksControl_NavigateToEntryRequested(object sender, LogEntry logEntry)
        {
            UserControlLogEntriesTextBox.SelectedLogEntry = logEntry;
        }

        private void UserControlLogEntriesTextBox_LogEntryAtCursorChanged(object sender, LogEntry e)
        {
            UsrMetadataFilterOverview.SelectedLogEntry = e;
            BookMarksControl.UpdateSelectedLogEntry(e);
            LogViewport.SelectedLogEntry = e;
        }

        private void UserControlLogEntriesTextBox_VisibleRangeChanged(object sender, UserControlLogEntriesTextBox.VisibleRangeChangedEventArgs e)
        {
            if (ConfigurationManager.GenericConfig.ShowTimelineByDefault)
                LogTimeLineControl.SetOnScreenDateTimeRange(e.TopPosition.LogEntry.TimeStamp, e.BottomPosition.LogEntry.TimeStamp);
        }

        private void LogTimelineControl_CellClicked(object sender, LogEntry e)
        {
            UserControlContentFilter.ClearSelectedLogEntry();
            UserControlLogEntriesTextBox.SelectedLogEntry = e;
        }

        private void UserControlLogEntriesTextBox_LogEntriesTextBoxTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserControlLogEntriesTextBox.Text)) return;
            LogExportWorkerManager.WriteToFile(UserControlLogEntriesTextBox.Text);
        }

        private void FormLogScraper_Load(object sender, EventArgs e)
        {
            try
            {
                //Collapse the bottom panel here, so it is still shown in the designer
                splitContainer5.Panel2Collapsed = true;
                btnOpenWithEditor.Enabled = ConfigurationManager.GenericConfig.ExportToFile;
                UsrLogProviderSelection.IsPinned = ConfigurationManager.GenericConfig.PinLogProvidersByDefault;
                UsrLogProviderSelection.PopulateLogProviders();
                UsrLogProviderSelection.PopulateLogLayouts([.. ConfigurationManager.LogLayouts]);
                //Enforce autosizing because the IDE overrides the control's autosize settings.
                UsrLogProviderSelection.AutoSize = false;
                AutoSizeLogProviderSelection();
            }
            catch (Exception ex)
            {
                ex.LogStackTraceToFile();
                MessageBox.Show(ex.Message);
            }

            RefreshLogStatistics();
            RepositionLogEntriesTextBox();
            GitHubUpdateChecker.CheckForUpdateInSeperateThread();
        }
        #endregion

        #region Initiate getting raw log and processing of raw log

        private DateTime? lastTrailTime = null;

        private void FetchRawLogAsync(int intervalInSeconds = -1, int durationInSeconds = -1)
        {
            if (UsrLogProviderSelection.GetSelectedLogLayout() == null) return;
            try
            {
                BtnRecord.Enabled = false;
                BtnRecordWithTimer.Enabled = false;
                UsrLogProviderSelection.UpdateStatus(LogProviderSelectionControl.StatusType.Retrieving);
                FormCompactView.Instance.UpdateButtonsFromMainWindow();
                Application.DoEvents();

                ISourceAdapter logProvider = UsrLogProviderSelection.GetSelectedSourceAdapter(lastTrailTime);

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
            LogLayout logLayout = UsrLogProviderSelection.GetSelectedLogLayout();
            try
            {
                if (LogAppState.Instance.LogCollection.Value == null) LogAppState.Instance.LogCollection.Set(new());

                bool newLogEntriesReceived = false;
                try
                {
                    UsrLogProviderSelection.UpdateStatus(LogProviderSelectionControl.StatusType.Processing);
                    newLogEntriesReceived = RawLogParser.TryParseAndAppendLogEntries(rawLog, LogAppState.Instance.LogCollection.Value, logLayout);
                }
                catch (Exception ex)
                {
                    ex.LogStackTraceToFile("Fout tijdens parsen van raw log.");
                    UserControlLogEntriesTextBox.Text = RawLogParser.JoinRawLogIntoString(rawLog);
                    throw;
                }

                if (newLogEntriesReceived)
                {
                    LogEntryClassifier.Classify(logLayout, LogAppState.Instance.LogCollection.Value);

                    UsrMetadataFilterOverview.UpdateFilterControls(logLayout, LogAppState.Instance.LogCollection.Value);

                    // TODO: fix total/visible error counts
                    //activeFilterOverviewControl.SetErrorEntries(LogCollection.Instance.ErrorLogEntriesmask);
                    FormCompactView.Instance.SetErrorCont(LogAppState.Instance.LogCollection.Value.ErrorMask.Count);
                    FilterLogEntries();
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

        private void RefreshLogStatistics()
        {
            int countFilteredEntries = LogAppState.Instance.FilterResultWithRange.Value?.LogEntries?.Count ?? 0;
            int totalCount = LogAppState.Instance.LogCollection.Value?.LogEntries?.Count ?? 0;
            activeFilterOverviewControl.SetCounts(countFilteredEntries, totalCount);
            FormCompactView.Instance.SetCounts(countFilteredEntries, totalCount);
        }

        private void ShowException(Exception ex)
        {
            ex.LogStackTraceToFile();
            HandleErrorMessages(ex.Message, false);
        }
        #endregion

        #region Filter and write log to screen and file

        private void FilterLogEntries()
        {
            if (UsrLogProviderSelection.GetSelectedLogLayout() == null || LogAppState.Instance.LogCollection.Value == null) return;

            List<LogMetadataFilter> activeFilters = UsrMetadataFilterOverview.GetActiveFilters();

            activeFilterOverviewControl.SetMetadataFilters(activeFilters);
            LogMetadataFilterResult metadataFilterResult = LogMetadataFilterEngine.Apply(LogAppState.Instance.LogCollection.Value, activeFilters, UsrLogProviderSelection.GetSelectedLogLayout());
            LogAppState.Instance.MetadataFilterResult.Set(metadataFilterResult);

            // Update counts in the filter panel to reflect the filtered result.
            UsrMetadataFilterOverview.UpdateFilterControlsCount([.. metadataFilterResult.FilterStats.Values]);

            RenderLog(metadataFilterResult);
        }

        private void RenderLog(LogMetadataFilterResult logMetadataFilterResult)
        {
            if (logMetadataFilterResult == null) return;
            LogRenderSettings logRenderSettings = new()
            {
                LogRange = LogAppState.Instance.LogRange.Value,
                LogLayout = LogAppState.Instance.LogLayout.Value,
                ShowOriginalMetadata = LogAppState.Instance.RenderOriginalMetadata.Value,
                SelectedMetadataProperties = LogAppState.Instance.RenderSeperateMetadataProperties.Value,
                LogPostProcessorKinds = LogAppState.Instance.RenderProcessorKinds.Value,
                LogFlowTreeRenderSettings = new LogFlowTreeRenderSettings(flowTreeControl1.ShowTree, flowTreeControl1.SelectedContentProperty)
            };

            UserControlSearch.LogRenderSettings = logRenderSettings;
            currentRenderSettings = logRenderSettings;
            LogAppState.Instance.RenderSettings.Set(logRenderSettings);
            LogAppState.Instance.FilterResultWithRange.Set(new(logMetadataFilterResult, LogAppState.Instance.LogRange.Value));

            RefreshLogStatistics();
        }


        #endregion

        #region Erase and reset

        private void LogAppState_ResetRequested(object sender, ResetEventArgs e)
        {
            FormCompactView.Instance.SetErrorCont(0);
            UpdateLogRange(false);

            if (!e.KeepFilters)
            {
                UserControlContentFilter.UpdateLogLayout(UsrLogProviderSelection.GetSelectedLogLayout());
                TxtErrorMessage.Text = string.Empty;
                TxtErrorMessage.Visible = false;
            }

            FilterLogEntries();
            RefreshLogStatistics();
            UpdateButtonStatus();
            lastTrailTime = null;
            HandleErrorMessages(string.Empty, true);
            splitContainer5.Panel2Collapsed = true;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion

        #region User controls event handling

        private void UpdateTimeLineVisibility()
        {
            splitContainer4.Panel1Collapsed = !ConfigurationManager.GenericConfig.ShowTimelineByDefault;
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

        private void MetadataFormatingControl_SelectionChanged(object sender, EventArgs e)
        {
            //TODO: render on MetadataFormatingControl_SelectionChanged
            RenderLog(LogAppState.Instance.MetadataFilterResult.Value);
        }

        private void HandleLogContentFilterSelectedItemChanged(object sender, EventArgs e)
        {
            UserControlLogEntriesTextBox.SelectedLogEntry = UserControlContentFilter.SelectedLogEntry;
        }

        private void HandleLogProviderSourceSelectionChanged(object sender, EventArgs e)
        {
            LogAppState.Instance.Reset(keepFilters: false);
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
                UsrLogProviderSelection.UpdateStatus(LogProviderSelectionControl.StatusType.Retrieving);
                TimeSpan tijd = TimeSpan.FromSeconds(totalDurationInSeconds - elapsedSeconds);
                BtnRecordWithTimer.Image = null;
                BtnRecordWithTimer.Text = string.Format("{0}:{1:D2}", (int)tijd.TotalMinutes, tijd.Seconds);
            }
        }

        private void UsrSearch_Search(SearchSettings searchSettings)
        {
            UserControlSearch.Enabled = false;
            try
            {
                ScintillaControlExtensions.SearchDirection searchDirection = searchSettings.Direction == UserControlSearch.SearchDirection.Forward ? ScintillaControlExtensions.SearchDirection.Forward : ScintillaControlExtensions.SearchDirection.Backward;
                bool found = UserControlLogEntriesTextBox.TrySearch(searchSettings.SearchText, searchSettings.WholeWord, searchSettings.CaseSensitive, searchSettings.WrapAround, searchDirection);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout tijdens zoeken: " + ex.Message);
                ex.LogStackTraceToFile("Fout tijdens zoeken in log.");
            }
            finally
            {
                UserControlSearch.Enabled = true;
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
            bool downloadingInProgress = SourceProcessingManager.Instance.IsWorkerActive;
            bool sourceIsValid = UsrLogProviderSelection.IsSourceValid;
            bool layoutSelected = UsrLogProviderSelection.GetSelectedLogLayout() != null;
            if (!downloadingInProgress)
            {
                HandleSourceProcessingWorkerProgressUpdate(-1, -1);
                UsrLogProviderSelection.UpdateStatus(LogProviderSelectionControl.StatusType.Finished);
            }

            BtnRecord.Visible = !downloadingInProgress;
            BtnRecord.Enabled = !downloadingInProgress && sourceIsValid && layoutSelected;
            BtnRecordWithTimer.Enabled = !downloadingInProgress && sourceIsValid && layoutSelected;
            BtnFormRecord.Enabled = sourceIsValid && layoutSelected;
            BtnStop.Visible = downloadingInProgress;
            BtnStop.Enabled = downloadingInProgress;
            BtnConfig.Enabled = !downloadingInProgress;
            UsrLogProviderSelection.Enabled = !downloadingInProgress;
            UsrLogProviderSelection.SetEnabled(!downloadingInProgress);

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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            LogMetadataFilterResult metadataFilterResult = LogAppState.Instance.MetadataFilterResult.Value;
            if (metadataFilterResult == null || metadataFilterResult.LogEntries == null || metadataFilterResult.LogEntries.Count == 0) return;

            LogRenderSettings logRenderSettings = new()
            {
                LogRange = LogAppState.Instance.LogRange.Value,
                LogLayout = UsrLogProviderSelection.GetSelectedLogLayout(),
                ShowOriginalMetadata = true
            };

            List<LogEntry> logEntriesToRender = LogRenderer.GetLogEntriesRange(metadataFilterResult.LogEntries, logRenderSettings.LogRange);
            string renderedLog = LogRenderer.RenderLogEntriesAsString(logEntriesToRender, logRenderSettings, null, null, null);

            if (renderedLog != null)
            {
                using SaveFileDialog saveFileDialog = new()
                {
                    Filter = "Log files (*.log)|*.log|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    DefaultExt = "log",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    FileName = $"Log from {logEntriesToRender[0].TimeStamp:yyyyMMdd_HHmmss} to {logEntriesToRender[^1].TimeStamp:yyyyMMdd_HHmmss}.log",
                    AddExtension = true
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, renderedLog);

                        ProcessStartInfo processStartInfo = new()
                        {
                            FileName = "explorer.exe",
                            Arguments = $"/select,\"{saveFileDialog.FileName}\"",
                            UseShellExecute = true
                        };

                        Process.Start(processStartInfo);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fout bij opslaan van log: " + ex.Message, "Opslaan mislukt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ex.LogStackTraceToFile("Fout bij opslaan van log.");
                    }
                }

            }
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

                if (logLayoutsChanged) UsrLogProviderSelection.PopulateLogLayouts([.. ConfigurationManager.LogLayouts]);

                if (kubernetesChanged || runtimeChanged)
                {
                    if (MessageBox.Show("De instellingen zijn gewijzigd. Wil je deze direct toepassen? Hierdoor wordt het log gereset", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        UsrLogProviderSelection.UpdateProviderConfig();
                        UsrLogProviderSelection_LogProviderChanged(null, null);
                    }
                }
            }
        }

        private void UsrControlMetadataFormating_FilterChanged(object sender, EventArgs e) => FilterLogEntries();

        #endregion

        #region Dropdowns log providers and layout

        private void UsrLogProviderSelection_LogProviderChanged(object sender, EventArgs e)
        {
            // This is now handled by the LogProviderSelectionControl
            // Just reset the log display
            LogAppState.Instance.Reset(keepFilters: false);
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
                UserControlSearch.Focus();
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