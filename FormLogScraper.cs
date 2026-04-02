using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Export;
using LogScraper.Log;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Log.Processing;
using LogScraper.Log.Processing.RawLogParsing;
using LogScraper.Log.Rendering;
using LogScraper.LogProviders;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Workers;
using LogScraper.Utilities;
using LogScraper.Utilities.Extensions;
using LogScraper.Utilities.UserControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static LogScraper.UserControlSearch;
using static LogScraper.Utilities.Extensions.ScintillaControlExtensions;

namespace LogScraper
{
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

            usrKubernetes.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrKubernetes.StatusUpdate += HandleErrorMessages;
            usrRuntime.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrRuntime.StatusUpdate += HandleErrorMessages;
            usrFileLogProvider.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrFileLogProvider.StatusUpdate += HandleErrorMessages;

            UsrControlMetadataFormating.SelectionChanged += HandleLogContentFilterUpdate;

            UserControlContentFilter.BeginEntryChanged += HandleLogContentFilterUpdate;
            UserControlContentFilter.EndEntryChanged += HandleLogContentFilterUpdate;
            UserControlContentFilter.SelectedItemChanged += HandleLogContentFilterSelectedItemChanged;

            UserControlLogEntriesTextBox.LogEntriesTextChanged += UserControlLogEntriesTextBox_LogEntriesTextBoxTextChanged;
            UserControlLogEntriesTextBox.TimeLineVisibilityChanged += ChkTimelineVisible_CheckedChanged;
            UserControlLogEntriesTextBox.VisibleRangeChanged += UserControlLogEntriesTextBox_VisibleRangeChanged;
            UserControlLogEntriesTextBox.LogEntryAtCursorChanged += UserControlLogEntriesTextBox_LogEntryAtCursorChanged;

            UserControlLogEntriesTextBox.IsTimelineVisible = ConfigurationManager.GenericConfig.ShowTimelineByDefault;
            ChkTimelineVisible_CheckedChanged(this, EventArgs.Empty);

            LogTimeLineControl.CellClicked += LogTimelineControl_CellClicked;
            LogTimeLineControl.ErrorMarkerClicked += LogTimelineControl_CellClicked;

            UserControlSearch.Search += UsrSearch_Search;
            UserControlSearch.SelectedItemChanged += UserControlSearch_SelectedItemChanged;

            SetDynamicToolTips();
            UpdateBtnErase();
        }

        private void UserControlLogEntriesTextBox_LogEntryAtCursorChanged(object sender, LogEntry e)
        {
            UsrMetadataFilterOverview.SelectedLogEntry = e;
        }

        private void UserControlLogEntriesTextBox_VisibleRangeChanged(object sender, UserControlLogEntriesTextBox.VisibleRangeChangedEventArgs e)
        {
            if (UserControlLogEntriesTextBox.IsTimelineVisible)
                LogTimeLineControl.SetVisibleRange(e.TopPosition.LogEntry.TimeStamp, e.BottomPosition.LogEntry.TimeStamp);
        }

        private void ChkTimelineVisible_CheckedChanged(object sender, EventArgs e)
        {
            splitContainer4.Panel2Collapsed = !UserControlLogEntriesTextBox.IsTimelineVisible;
            if (UserControlLogEntriesTextBox.IsTimelineVisible && visibleLogEntries != null)
                LogTimeLineControl.UpdateLogEntries(visibleLogEntries);
        }

        private void LogTimelineControl_CellClicked(object sender, LogEntry e)
        {
            UserControlContentFilter.ClearSelectedLogEntry();
            UserControlSearch.ClearSelectedLogEntry();
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
                usrKubernetes.Update(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
                usrRuntime.UpdateRuntimeInstances(ConfigurationManager.LogProvidersConfig.RuntimeConfig.Instances);
                btnOpenWithEditor.Enabled = ConfigurationManager.GenericConfig.ExportToFile;
                PopulateLogLayouts();
                PopulateLogProviderControls();
            }
            catch (Exception ex)
            {
                ex.LogStackTraceToFile();
                MessageBox.Show(ex.Message);
            }

            RefreshLogStatistics();
            GitHubUpdateChecker.CheckForUpdateInSeperateThread();
        }
        #endregion

        #region Initiate getting raw log and processing of raw log

        private DateTime? lastTrailTime = null;

        private void FetchRawLogAsync(int intervalInSeconds = -1, int durationInSeconds = -1)
        {
            try
            {
                BtnRecord.Enabled = false;
                BtnRecordWithTimer.Enabled = false;
                FormCompactView.Instance.UpdateButtonsFromMainWindow();
                Application.DoEvents();

                LogProviderType logProviderType = ((ILogProviderConfig)cboLogProvider.SelectedItem).LogProviderType;
                ISourceAdapter logProvider = logProviderType switch
                {
                    LogProviderType.Runtime => usrRuntime.GetSourceAdapter(),
                    LogProviderType.Kubernetes => usrKubernetes.GetSourceAdapter(null, lastTrailTime),
                    LogProviderType.File => usrFileLogProvider.GetSourceAdapter(),
                    _ => throw new NotImplementedException()
                };

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
            LogLayout logLayout = (LogLayout)cboLogLayout.SelectedItem;
            try
            {
                bool newLogEntriesReceived = false;
                try
                {
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
                    FilterLogEntries();
                    RefreshLogStatistics();
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
            lblLogEntriesTotalValue.Text = LogCollection.Instance.LogEntries.Count.ToString();

            int count = LogCollection.Instance.ErrorCount;
            lblNumberOfLogEntriesFilteredWithError.Text = count.ToString();
            lblNumberOfLogEntriesFilteredWithError.ForeColor = count > 0 ? Color.DarkRed : Color.Black;
            lblLogEntriesFilteredWithError.ForeColor = lblNumberOfLogEntriesFilteredWithError.ForeColor;
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
            List<LogMetadataFilter> activeFilters = UsrMetadataFilterOverview.GetActiveFilters();

            currentLogMetadataFilterResult = LogMetadataFilterEngine.Apply(
                LogCollection.Instance.LogEntries,
                activeFilters,
                (LogLayout)cboLogLayout.SelectedItem);

            // Update counts in the filter panel to reflect the filtered result.
            UsrMetadataFilterOverview.UpdateFilterControlsCount(
                [.. currentLogMetadataFilterResult.FilterStats.Values]);

            UserControlContentFilter.UpdateLogEntries(currentLogMetadataFilterResult);
            RenderLog(currentLogMetadataFilterResult);
        }

        private void RenderLog(LogMetadataFilterResult logMetadataFilterResult)
        {
            LogRenderSettings logRenderSettings = new()
            {
                LogEntryBegin = UserControlContentFilter.SelectedBeginLogEntry,
                LogEntryEnd = UserControlContentFilter.SelectedEndLogEntry,
                LogLayout = (LogLayout)cboLogLayout.SelectedItem,
                ShowOriginalMetadata = UsrControlMetadataFormating.ShowOriginalMetadata,
                SelectedMetadataProperties = UsrControlMetadataFormating.SelectedMetadataProperties
            };

            visibleLogEntries = LogRenderer.GetLogEntriesListToRender(logMetadataFilterResult.LogEntries, logRenderSettings);

            UserControlLogEntriesTextBox.UpdateLogMetadataFilterResult(logMetadataFilterResult, visibleLogEntries, logRenderSettings);
            UserControlSearch.UpdateLogEntries(visibleLogEntries);
            if (UserControlLogEntriesTextBox.IsTimelineVisible) LogTimeLineControl.UpdateLogEntries(visibleLogEntries);
            lblNumberOfLogEntriesFiltered.Text = logMetadataFilterResult.LogEntries.Count.ToString();
        }
        #endregion

        #region Erase and reset

        public void Erase()
        {
            LogCollection.Instance.Clear();
            UserControlLogEntriesTextBox.Clear();
            LogTimeLineControl.ClearVisibleRange();
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
            currentLogMetadataFilterResult = null;

            UsrMetadataFilterOverview.Reset();
            UserControlContentFilter.Reset();
            UserControlContentFilter.UpdateLogLayout((LogLayout)cboLogLayout.SelectedItem);
            TxtErrorMessage.Text = string.Empty;
            TxtErrorMessage.Visible = false;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion

        #region User controls event handling

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
            UserControlSearch.IsMetadataSearchEnabled = UsrControlMetadataFormating.IsOriginalMetadataShown;
            if (currentLogMetadataFilterResult != null) RenderLog(currentLogMetadataFilterResult);
        }

        private void HandleLogContentFilterSelectedItemChanged(object sender, EventArgs e)
        {
            UserControlSearch.ClearSelectedLogEntry();
            UserControlLogEntriesTextBox.SelectedLogEntry = UserControlContentFilter.SelectedLogEntry;
        }

        private void UserControlSearch_SelectedItemChanged(object sender, EventArgs e)
        {
            UserControlContentFilter.ClearSelectedLogEntry();
            UserControlLogEntriesTextBox.SelectedLogEntry = UserControlSearch.SelectedLogEntry;
        }

        private void HandleLogProviderSourceSelectionChanged(object sender, EventArgs e)
        {
            Reset();
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
                TimeSpan tijd = TimeSpan.FromSeconds(totalDurationInSeconds - elapsedSeconds);
                BtnRecordWithTimer.Image = null;
                BtnRecordWithTimer.Text = string.Format("{0}:{1:D2}", (int)tijd.TotalMinutes, tijd.Seconds);
            }
        }

        private void UsrSearch_Search(string searchQuery, SearchDirectionUserControl searchDirectionUserControl, bool caseSensitive, bool wholeWord, bool wrapAround)
        {
            UserControlSearch.Enabled = false;
            try
            {
                SearchDirection searchDirection = searchDirectionUserControl == SearchDirectionUserControl.Forward ? SearchDirection.Forward : SearchDirection.Backward;
                bool found = UserControlLogEntriesTextBox.TrySearch(searchQuery, wholeWord, caseSensitive, wrapAround, searchDirection);
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
            if (!downloadingInProgress)
                HandleSourceProcessingWorkerProgressUpdate(-1, -1);

            BtnRecord.Visible = !downloadingInProgress;
            BtnRecord.Enabled = !downloadingInProgress;
            BtnRecordWithTimer.Enabled = !downloadingInProgress;
            BtnStop.Visible = downloadingInProgress;
            BtnStop.Enabled = downloadingInProgress;
            BtnConfig.Enabled = !downloadingInProgress;
            GrpSourceAndLayout.Enabled = !downloadingInProgress;
            GrpLogProvidersSettings.Enabled = !downloadingInProgress;

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
                LogEntryBegin = UserControlContentFilter.SelectedBeginLogEntry,
                LogEntryEnd = UserControlContentFilter.SelectedEndLogEntry,
                LogLayout = (LogLayout)cboLogLayout.SelectedItem,
                ShowOriginalMetadata = true
            };

            List<LogEntry> logEntriesToRender = LogRenderer.GetLogEntriesListToRender(currentLogMetadataFilterResult.LogEntries, logRenderSettings);
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
                        System.IO.File.WriteAllText(saveFileDialog.FileName, renderedLog);
                        MessageBox.Show("Log succesvol opgeslagen.", "Opslaan gelukt", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    UserControlLogEntriesTextBox.IsTimelineVisible = ConfigurationManager.GenericConfig.ShowTimelineByDefault;
                }

                if (logLayoutsChanged) PopulateLogLayouts();

                if (kubernetesChanged || runtimeChanged)
                {
                    if (MessageBox.Show("De instellingen zijn gewijzigd. Wil je deze direct toepassen? Hierdoor wordt het log gereset", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        if (kubernetesChanged)
                            usrKubernetes.Update(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
                        if (runtimeChanged)
                            usrRuntime.UpdateRuntimeInstances(ConfigurationManager.LogProvidersConfig.RuntimeConfig.Instances);
                        CboLogProvider_SelectedIndexChanged(null, null);
                    }
                }
            }
        }

        private void UsrControlMetadataFormating_FilterChanged(object sender, EventArgs e) => FilterLogEntries();

        private void ChkShowAllLogEntries_CheckedChanged(object sender, EventArgs e) => HandleLogContentFilterUpdate(sender, e);

        #endregion

        #region Dropdowns log providers and layout

        private void PopulateLogProviderControls()
        {
            if (ConfigurationManager.LogProvidersConfig.FileConfig != null)
            {
                cboLogProvider.Items.Add(ConfigurationManager.LogProvidersConfig.FileConfig);
                if (ConfigurationManager.GenericConfig.LogProviderTypeDefault == LogProviderType.File)
                    cboLogProvider.SelectedItem = ConfigurationManager.LogProvidersConfig.FileConfig;
            }

            if (ConfigurationManager.LogProvidersConfig.RuntimeConfig != null)
            {
                cboLogProvider.Items.Add(ConfigurationManager.LogProvidersConfig.RuntimeConfig);
                if (ConfigurationManager.GenericConfig.LogProviderTypeDefault == LogProviderType.Runtime)
                    cboLogProvider.SelectedItem = ConfigurationManager.LogProvidersConfig.RuntimeConfig;
            }

            if (ConfigurationManager.LogProvidersConfig.KubernetesConfig != null)
            {
                cboLogProvider.Items.Add(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
                if (ConfigurationManager.GenericConfig.LogProviderTypeDefault == LogProviderType.Kubernetes)
                    cboLogProvider.SelectedItem = ConfigurationManager.LogProvidersConfig.KubernetesConfig;
            }
        }

        private void PopulateLogLayouts()
        {
            cboLogLayout.Items.Clear();
            if (ConfigurationManager.LogLayouts != null)
            {
                cboLogLayout.Items.AddRange([.. ConfigurationManager.LogLayouts]);
                if (cboLogLayout.Items.Count > 0) CboLogProvider_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void CboLogLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLogLayout.SelectedItem == null) return;
            LogLayout logLayout = (LogLayout)cboLogLayout.SelectedItem;
            UsrControlMetadataFormating.UpdateLogMetadataProperties(logLayout.LogMetadataProperties);
            UserControlLogEntriesTextBox.UpdateLogLayout(logLayout);
            Reset();
        }

        private void CboLogProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILogProviderConfig logProviderConfig = (ILogProviderConfig)cboLogProvider.SelectedItem;
            if (logProviderConfig == null) return;

            usrRuntime.Visible = logProviderConfig.LogProviderType == LogProviderType.Runtime;
            usrKubernetes.Visible = logProviderConfig.LogProviderType == LogProviderType.Kubernetes;
            usrFileLogProvider.Visible = logProviderConfig.LogProviderType == LogProviderType.File;

            switch (logProviderConfig.LogProviderType)
            {
                case LogProviderType.Runtime:
                    cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.RuntimeConfig.DefaultLogLayout;
                    GrpLogProvidersSettings.Text = "Directe URL instellingen";
                    break;
                case LogProviderType.Kubernetes:
                    cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.KubernetesConfig.DefaultLogLayout;
                    GrpLogProvidersSettings.Text = "Kubernetes instellingen";
                    break;
                case LogProviderType.File:
                    GrpLogProvidersSettings.Text = "Lokaal bestand instellingen";
                    cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.FileConfig.DefaultLogLayout;
                    break;
            }
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
            if (keyData == (Keys.Control | Keys.B))
            {
                UserControlContentFilter.SelectLogEntryBegin();
                return true;
            }
            if (keyData == (Keys.Control | Keys.E))
            {
                UserControlContentFilter.SelectLogEntryEnd();
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