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
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Log.Processing;
using LogScraper.Log.Processing.RawLogParsing;
using LogScraper.Log.Rendering;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Workers;
using LogScraper.Utilities;
using LogScraper.Utilities.Extensions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static LogScraper.Controls.LogProviderSelectionControl;

namespace LogScraper
{
    //TODO: Add key shortcuts like F3/shift F3
    //TODO: bug when continues reading file Stubs/JSONInvertedExample.log with JSON layout
    //TODO: filter overview, reduce redrawing flickering
    //TODO: implement error overview from filter overview
    public partial class FormLogScraper : Form
    {
        #region Form Initialization
        private LogMetadataFilterResult currentLogMetadataFilterResult;
        List<LogEntry> visibleLogEntries;
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
            UsrLogProviderSelection.LogLayoutChanged += UsrLogProviderSelection_LogLayoutChanged;
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

            LogViewport.RangeChanged += LogViewport_RangeChanged;

            activeFilterOverviewControl.SizeChanged += (s, e) => RepositionLogEntriesTextBox();
            activeFilterOverviewControl.FilterRemoved += ActiveFilterOverviewControl_FilterRemoved;
            activeFilterOverviewControl.RangeRemoved += ActiveFilterOverviewControl_RangeRemoved;
            PnlFiltersAndLogEntriesTextBox.SizeChanged += (s, e) => RepositionLogEntriesTextBox();

            flowTreeControl1.ShowTreeStateChanged += FlowTreeControl_ShowTreeStateChanged;

            MetadataFormatingControl.SelectionChanged += HandleLogContentFilterUpdate;

            LogPostProcessing.PostProcessingResultsChanged += LogPostProcessing_PostProcessingResultsChanged;

            UserControlSearch.Search += UsrSearch_Search;
            UserControlSearch.SearchSettingsChanged += SearchControl_SearchSettingsChanged;

            SearchResultListControl.ResultSelected += SearchResultListControl_ResultSelected;
            SearchResultListControl.Close += SearchResultListControl_Close;

            UpdateTimeLineVisibility();
            SetDynamicToolTips();
            UpdateBtnErase();
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
            splitContainer5.Panel2Collapsed = true;
        }

        private void SearchControl_SearchSettingsChanged(SearchSettings settings)
        {
            if (splitContainer5.Panel2Collapsed && UserControlSearch.SelectedSearchMode == UserControlSearch.SearchMode.All)
            {
                splitContainer5.Panel2Collapsed = false;
            }

            SearchResultListControl.UpdateLogEntries(visibleLogEntries);
            SearchResultListControl.UpdateSearchResults(settings);
        }

        private void SearchResultListControl_ResultSelected(object sender, LogEntry e)
        {
            UserControlContentFilter.ClearSelectedLogEntry();
            UserControlLogEntriesTextBox.SelectedLogEntry = e;
        }

        private void FlowTreeControl_ShowTreeStateChanged(object sender, EventArgs e)
        {
            RenderLog(currentLogMetadataFilterResult);
        }

        private void LogViewport_RangeChanged(object sender, EventArgs e)
        {
            UpdateLogRange(true);
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
                int top = activeFilterOverviewControl.Bottom + 3;
                UserControlLogEntriesTextBox.SetBounds(0, top, PnlFiltersAndLogEntriesTextBox.ClientSize.Width, Math.Max(0, PnlFiltersAndLogEntriesTextBox.ClientSize.Height - top));
                PnlFiltersAndLogEntriesTextBox.ResumeLayout(false);
            }
            finally { _repositioningTextBox = false; }
        }
        private void UpdateLogRange(bool render)
        {
            UserControlContentFilter.LogRange = LogViewport.Range;
            BookMarksControl.SetLogRange(LogViewport.Range);
            activeFilterOverviewControl.SetLogRange(LogViewport.Range);
            if (render) RenderLog(currentLogMetadataFilterResult);
        }

        private void LogPostProcessing_PostProcessingResultsChanged(object sender, EventArgs e)
        {
            RenderLog(currentLogMetadataFilterResult);
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
                UsrLogProviderSelection.UpdateStatus(StatusType.Retrieving);
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
                bool newLogEntriesReceived = false;
                try
                {
                    UsrLogProviderSelection.UpdateStatus(StatusType.Processing);
                    newLogEntriesReceived = RawLogParser.TryParseAndAppendLogEntries(rawLog, LogCollection.Instance, logLayout);
                }
                catch (Exception ex)
                {
                    ex.LogStackTraceToFile("Fout tijdens parsen van raw log.");
                    UserControlLogEntriesTextBox.Text = RawLogParser.JoinRawLogIntoString(rawLog);
                    throw;
                }

                if (newLogEntriesReceived)
                {
                    LogEntryClassifier.Classify(logLayout, LogCollection.Instance);

                    // Pass null for stats on initial load — no filters active yet, full counts will be shown.
                    UsrMetadataFilterOverview.UpdateFilterControls(logLayout, LogCollection.Instance, null);
                    activeFilterOverviewControl.SetErrorEntries(LogCollection.Instance.ErrorLogEntries);
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
            activeFilterOverviewControl.SetCounts(visibleLogEntries?.Count ?? 0, LogCollection.Instance.LogEntries.Count);
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
            if (UsrLogProviderSelection.GetSelectedLogLayout() == null) return;

            List<LogMetadataFilter> activeFilters = UsrMetadataFilterOverview.GetActiveFilters();

            activeFilterOverviewControl.SetMetadataFilters(activeFilters);
            currentLogMetadataFilterResult = LogMetadataFilterEngine.Apply(LogCollection.Instance, activeFilters, UsrLogProviderSelection.GetSelectedLogLayout());

            // Update counts in the filter panel to reflect the filtered result.
            UsrMetadataFilterOverview.UpdateFilterControlsCount([.. currentLogMetadataFilterResult.FilterStats.Values]);

            UserControlContentFilter.UpdateLogEntries(currentLogMetadataFilterResult);
            BookMarksControl.UpdateMetadataFilterResult(currentLogMetadataFilterResult);

            RenderLog(currentLogMetadataFilterResult);
        }

        private void RenderLog(LogMetadataFilterResult logMetadataFilterResult)
        {
            if (logMetadataFilterResult == null) return;
            LogRenderSettings logRenderSettings = new()
            {
                LogRange = LogViewport.Range,
                LogLayout = UsrLogProviderSelection.GetSelectedLogLayout(),
                ShowOriginalMetadata = MetadataFormatingControl.ShowOriginalMetadata,
                SelectedMetadataProperties = MetadataFormatingControl.SelectedMetadataProperties,
                LogPostProcessorKinds = LogPostProcessing.VisibleProcessorKinds,
                LogFlowTreeRenderSettings = new LogFlowTreeRenderSettings(flowTreeControl1.ShowTree, flowTreeControl1.SelectedContentProperty)
            };

            UserControlSearch.LogRenderSettings = logRenderSettings;

            visibleLogEntries = LogRenderer.GetLogEntriesRange(logMetadataFilterResult.LogEntries, logRenderSettings.LogRange);

            UserControlLogEntriesTextBox.UpdateLogMetadataFilterResult(logMetadataFilterResult, visibleLogEntries, logRenderSettings);
            UserControlSearch.UpdateLogEntries(visibleLogEntries);
            RefreshTimeLine();
            RefreshLogStatistics();
        }

        private void RefreshTimeLine()
        {
            if (ConfigurationManager.GenericConfig.ShowTimelineByDefault)
            {
                LogTimeLineControl.UpdateLogEntries(currentLogMetadataFilterResult, visibleLogEntries, LogViewport.Range, currentLogMetadataFilterResult.SourceLogCollection);
            }
        }

        #endregion

        #region Erase and reset

        public void Erase()
        {
            LogCollection.Instance.Clear();
            UserControlLogEntriesTextBox.Clear();
            LogTimeLineControl.Clear();
            BookMarksControl.Clear();
            LogViewport.Clear();
            activeFilterOverviewControl.Clear();
            UpdateLogRange(false);
            LogPostProcessing.Clear();
            FilterLogEntries();
            RefreshLogStatistics();
            UpdateButtonStatus();
            lastTrailTime = null;
            HandleErrorMessages(string.Empty, true);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Reset()
        {
            Erase();
            LogTimeLineControl.Clear();
            currentLogMetadataFilterResult = null;

            UsrMetadataFilterOverview.Reset();
            activeFilterOverviewControl.Clear();
            UserControlContentFilter.Reset();
            UserControlContentFilter.UpdateLogLayout(UsrLogProviderSelection.GetSelectedLogLayout());
            TxtErrorMessage.Text = string.Empty;
            TxtErrorMessage.Visible = false;

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

        private void HandleLogContentFilterUpdate(object sender, EventArgs e)
        {
            UserControlSearch.IsMetadataSearchEnabled = MetadataFormatingControl.IsOriginalMetadataShown;
            RenderLog(currentLogMetadataFilterResult);
        }

        private void HandleLogContentFilterSelectedItemChanged(object sender, EventArgs e)
        {
            UserControlLogEntriesTextBox.SelectedLogEntry = UserControlContentFilter.SelectedLogEntry;
        }

        private void HandleLogProviderSourceSelectionChanged(object sender, EventArgs e)
        {
            Reset();
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
                UsrLogProviderSelection.UpdateStatus(StatusType.Retrieving);
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
                UsrLogProviderSelection.UpdateStatus(StatusType.Finished);
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

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            UsrMetadataFilterOverview.ResetFilters();
            UserControlContentFilter.ResetFilters();
            LogViewport.Clear();
            UpdateLogRange(true);
        }

        private bool isResetUiEnabled = false;

        public void BtnErase_Click(object sender, EventArgs e)
        {
            if (isResetUiEnabled) Reset();
            else Erase();
        }

        private void ToolStripMenuItemReset_Click(object sender, EventArgs e)
        {
            isResetUiEnabled = true;
            UpdateBtnErase();
            Reset();
        }

        private void ToolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            isResetUiEnabled = false;
            UpdateBtnErase();
            Erase();
        }

        private void UpdateBtnErase()
        {
            string toolTipText;
            if (!isResetUiEnabled)
            {
                toolTipText = "Wis log (filters behouden)";
                BtnErase.ImageIndex = 0;
            }
            else
            {
                toolTipText = "Reset log en filters";
                BtnErase.ImageIndex = 1;
            }
            ToolTip.SetToolTip(BtnErase, toolTipText);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (currentLogMetadataFilterResult == null || currentLogMetadataFilterResult.LogEntries == null || currentLogMetadataFilterResult.LogEntries.Count == 0) return;

            LogRenderSettings logRenderSettings = new()
            {
                LogRange = LogViewport.Range,
                LogLayout = UsrLogProviderSelection.GetSelectedLogLayout(),
                ShowOriginalMetadata = true
            };

            List<LogEntry> logEntriesToRender = LogRenderer.GetLogEntriesRange(currentLogMetadataFilterResult.LogEntries, logRenderSettings.LogRange);
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

        private void UsrLogProviderSelection_LogLayoutChanged(object sender, EventArgs e)
        {
            LogLayout logLayout = UsrLogProviderSelection.GetSelectedLogLayout();
            if (logLayout == null) return;
            MetadataFormatingControl.UpdateLogMetadataProperties(logLayout.LogMetadataProperties);
            UserControlLogEntriesTextBox.UpdateLogLayout(logLayout);
            flowTreeControl1.UpdateLogLayout(logLayout);
            Reset();
        }

        private void UsrLogProviderSelection_LogProviderChanged(object sender, EventArgs e)
        {
            // This is now handled by the LogProviderSelectionControl
            // Just reset the log display
            Reset();
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