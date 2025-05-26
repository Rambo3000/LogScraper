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
using LogScraper.LogProviders;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Workers;
using LogScraper.Utilities.Extensions;
using static LogScraper.Utilities.Extensions.ScintillaRichTextBoxExtensions;
using static LogScraper.UserControlSearch;
using LogScraper.Utilities;

namespace LogScraper
{
    public partial class FormLogScraper : Form
    {
        #region Form Initialization
        private LogMetadataFilterResult currentLogMetadataFilterResult;
        public FormLogScraper()
        {
            InitializeComponent();

            CultureInfo culture = new("nl"); // of "en", "fr", etc.
            Thread.CurrentThread.CurrentUICulture = culture;

            FormRecord.Instance.SetFormLogScraper(this);

            SourceProcessingManager.Instance.QueueLengthUpdate += HandleLogProviderManagerQueueUpdate;

            usrKubernetes.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrKubernetes.StatusUpdate += HandleErrorMessages;
            usrRuntime.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrRuntime.StatusUpdate += HandleErrorMessages;
            usrFileLogProvider.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrFileLogProvider.StatusUpdate += HandleErrorMessages;

            UserControlContentFilter.BeginEntryChanged += HandleLogContentFilterUpdateBegin;
            UserControlContentFilter.EndEntryChanged += HandleLogContentFilterUpdateEnd;
            UserControlContentFilter.FilterOnMetadata += UsrLogContentBegin_FilterOnMetadata;
            UserControlContentFilter.SelectedItemChanged += HandleLogContentFilterSelectedItemChanged;

            UsrControlMetadataFormating.SelectionChanged += HandleLogContentFilterUpdate;

            usrSearch.Search += UsrSearch_Search;

            SetDynamicToolTips();
        }

        private void UsrLogContentBegin_FilterOnMetadata(Dictionary<LogMetadataProperty, string> logMetadataPropertiesAndValues, bool isEnabled)
        {
            UsrMetadataFilterOverview.EnableFilterOnSpecificMetdataValues(logMetadataPropertiesAndValues, isEnabled);
            UserControlLogEntriesTextBox.SelectLogEntry(UserControlContentFilter.SelectedLogEntry);
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
            catch (Exception ex) { MessageBox.Show(ex.Message); }

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
                FormRecord.Instance.UpdateButtonsFromMainWindow();
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
                FormRecord.Instance.UpdateButtonsFromMainWindow();
            }
        }
        private void ProcessRawLog(string[] rawLog, DateTime? updatedLastTrailTime)
        {
            LogLayout logLayout = (LogLayout)cboLogLayout.SelectedItem;
            try
            {
                try
                {
                    RawLogParser.ParseLogEntriesIntoCollection(rawLog, LogCollection.Instance, logLayout);
                }
                catch (Exception)
                {
                    //Write the raw log to the text box to not leave the user completely in the dark
                    UserControlLogEntriesTextBox.ShowRawLog(RawLogParser.JoinRawLogIntoString(rawLog));
                    throw;
                }

                LogEntryClassifier.ClassifyMetadataAndContentProperties(logLayout, LogCollection.Instance);

                UsrMetadataFilterOverview.UpdateFilterControls(logLayout, LogCollection.Instance);
                FilterLogEntries();
                RefreshLogStatistics();
                HandleErrorMessages(string.Empty, true);
                FormRecord.Instance.UpdateButtonsFromMainWindow();
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
            lblLogEntriesTotalValue.ForeColor = LogCollection.Instance.LogEntries.Count > 50000 ? Color.DarkRed : Color.Black;

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
            // Get all the metadata properties and their values from all the user controls
            List<LogMetadataPropertyAndValues> LogMetadataPropertyAndValuesList = UsrMetadataFilterOverview.GetMetadataPropertyAndValues();

            // Filter the log entries into the FilterResult and update the count
            currentLogMetadataFilterResult = LogMetadataFilter.GetLogMetadataFilterResult(LogCollection.Instance.LogEntries, LogMetadataPropertyAndValuesList);

            UsrMetadataFilterOverview.UpdateFilterControlsCount(currentLogMetadataFilterResult.LogMetadataPropertyAndValues);
            UserControlContentFilter.UpdateLogEntries(currentLogMetadataFilterResult.LogEntries, currentLogMetadataFilterResult.LogMetadataPropertyAndValues);

            WriteLogToScreenAndFile(currentLogMetadataFilterResult);
        }
        private void WriteLogToScreenAndFile(LogMetadataFilterResult logMetadataFilterResult)
        {
            LogExportSettings logExportSettings = new()
            {
                LogEntryBegin = UserControlContentFilter.SelectedBeginLogEntry,
                LogEntryEnd = UserControlContentFilter.SelectedEndLogEntry,
                LogLayout = (LogLayout)cboLogLayout.SelectedItem,
                ShowOriginalMetadata = UsrControlMetadataFormating.ShowOriginalMetadata,
                SelectedMetadataProperties = UsrControlMetadataFormating.SelectedMetadataProperties
            };
            UserControlLogEntriesTextBox.UpdateLogMetadataFilterResult(logMetadataFilterResult, logExportSettings);

            lblNumberOfLogEntriesFiltered.Text = logMetadataFilterResult.LogEntries.Count.ToString();
            LogExportWorkerManager.WriteToFile(logMetadataFilterResult, logExportSettings);
        }

        #endregion

        #region Search
        private void UsrSearch_Search(string searchQuery, SearchDirectionUserControl searchDirectionUserControl, bool caseSensitive, bool wholeWord, bool wrapAround)
        {
            usrSearch.Enabled = false;
            Application.DoEvents();
            try
            {
                SearchDirection searchDirection = searchDirectionUserControl == SearchDirectionUserControl.Forward ? SearchDirection.Forward : SearchDirection.Backward;

                bool found = UserControlLogEntriesTextBox.TrySearch(searchQuery, wholeWord, caseSensitive, wrapAround, searchDirection );
                usrSearch.SetResultsFound(found);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout tijdens zoeken: " + ex.Message);
            }
            finally
            {
                Application.DoEvents();
                usrSearch.Enabled = true;
                usrSearch.Focus();
            }
        }
        #endregion

        #region Erase and reset
        private void Erase()
        {
            LogCollection.Instance.Clear();
            UserControlLogEntriesTextBox.Clear();
            FilterLogEntries();
            RefreshLogStatistics();
            UpdateButtonStatus();
            lastTrailTime = null;
            HandleErrorMessages(string.Empty, true);
            //Force memory cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        private void Reset()
        {
            currentLogMetadataFilterResult = null;

            UsrMetadataFilterOverview.Reset();
            TxtErrorMessage.Text = string.Empty;
            TxtErrorMessage.Visible = false;
            Erase();

            //Force memory cleanup
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
            if (currentLogMetadataFilterResult != null) WriteLogToScreenAndFile(currentLogMetadataFilterResult);
        }
        private void HandleLogContentFilterUpdateBegin(object sender, EventArgs e)
        {
            HandleLogContentFilterUpdate(sender, e);
            UserControlLogEntriesTextBox.ApplyBeginFilter(UserControlContentFilter.SelectedBeginLogEntry);
        }
        private void HandleLogContentFilterUpdateEnd(object sender, EventArgs e)
        {
            HandleLogContentFilterUpdate(sender, e);
            UserControlLogEntriesTextBox.ApplyEndFilter(UserControlContentFilter.SelectedEndLogEntry);
        }

        private void HandleLogContentFilterSelectedItemChanged(object sender, EventArgs e)
        {
            UserControlLogEntriesTextBox.SelectLogEntry(UserControlContentFilter.SelectedLogEntry);
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
            {
                HandleSourceProcessingWorkerProgressUpdate(-1, -1);
            }
            BtnRecord.Visible = !downloadingInProgress;
            BtnRecord.Enabled = !downloadingInProgress;
            BtnRecordWithTimer.Enabled = !downloadingInProgress;
            BtnStop.Visible = downloadingInProgress;
            BtnStop.Enabled = downloadingInProgress;
            BtnConfig.Enabled = !downloadingInProgress;
            GrpSourceAndLayout.Enabled = !downloadingInProgress;
            GrpLogProvidersSettings.Enabled = !downloadingInProgress;

            FormRecord.Instance.UpdateButtonsFromMainWindow();
        }
        public void BtnRecord_Click(object sender, EventArgs e)
        {
            FetchRawLogAsync();
        }
        public void BtnRecordWithTimer_Click(object sender, EventArgs e)
        {
            FetchRawLogAsync(1, ConfigurationManager.GenericConfig.AutomaticReadTimeMinutes * 60);
        }
        public void BtnStop_Click(object sender, EventArgs e)
        {
            BtnStop.Enabled = false;
            SourceProcessingManager.Instance.CancelAllWorkers();
        }
        public void BtnErase_Click(object sender, EventArgs e)
        {
            Erase();
        }
        public void BtnOpenWithEditor_Click(object sender, EventArgs e)
        {
            LogExportWorkerManager.OpenFileInExternalEditor();
        }
        private void BtnFormRecord_Click(object sender, EventArgs e)
        {
            if (!SourceProcessingManager.Instance.IsWorkerActive) { BtnRecordWithTimer_Click(sender, e); }
            FormRecord.Instance.ShowForm();
        }
        private void BtnConfig_Click(object sender, EventArgs e)
        {
            using FormConfiguration form = new();

            // 'this' makes it modal to the main window
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                form.GetConfigurationChangedStatus(out bool genericConfigChanged, out bool logLayoutsChanged, out bool kubernetesChanged, out bool runtimeChanged, out _);

                if (genericConfigChanged)
                {
                    SetDynamicToolTips();
                    btnOpenWithEditor.Enabled = ConfigurationManager.GenericConfig.ExportToFile;
                }

                if (logLayoutsChanged) PopulateLogLayouts();

                if (kubernetesChanged || runtimeChanged)
                {
                    if (MessageBox.Show("De instellingen zijn gewijzigd. Wil je deze direct toepassen? Hierdoor wordt het log gereset", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        if (kubernetesChanged)
                        {
                            usrKubernetes.Update(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
                        }
                        if (runtimeChanged)
                        {
                            usrRuntime.UpdateRuntimeInstances(ConfigurationManager.LogProvidersConfig.RuntimeConfig.Instances);
                        }
                        CboLogProvider_SelectedIndexChanged(null, null);
                    }
                }
            }
        }
        private void UsrControlMetadataFormating_FilterChanged(object sender, EventArgs e)
        {
            FilterLogEntries();
        }
        private void ChkShowAllLogEntries_CheckedChanged(object sender, EventArgs e)
        {
            HandleLogContentFilterUpdate(sender, e);
        }
        #endregion

        #region Dropdowns log providers and layout
        private void PopulateLogProviderControls()
        {
            if (ConfigurationManager.LogProvidersConfig.FileConfig != null)
            {
                cboLogProvider.Items.Add(ConfigurationManager.LogProvidersConfig.FileConfig);
                if (ConfigurationManager.GenericConfig.LogProviderTypeDefault == LogProviderType.File) cboLogProvider.SelectedItem = ConfigurationManager.LogProvidersConfig.FileConfig;
            }

            if (ConfigurationManager.LogProvidersConfig.RuntimeConfig != null)
            {
                cboLogProvider.Items.Add(ConfigurationManager.LogProvidersConfig.RuntimeConfig);
                if (ConfigurationManager.GenericConfig.LogProviderTypeDefault == LogProviderType.Runtime) cboLogProvider.SelectedItem = ConfigurationManager.LogProvidersConfig.RuntimeConfig;
            }

            if (ConfigurationManager.LogProvidersConfig.KubernetesConfig != null)
            {
                cboLogProvider.Items.Add(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
                if (ConfigurationManager.GenericConfig.LogProviderTypeDefault == LogProviderType.Kubernetes) cboLogProvider.SelectedItem = ConfigurationManager.LogProvidersConfig.KubernetesConfig;
            }
        }
        private void PopulateLogLayouts()
        {
            cboLogLayout.Items.Clear();
            if (ConfigurationManager.LogLayouts != null)
            {
                cboLogLayout.Items.AddRange([.. ConfigurationManager.LogLayouts]);
                if (cboLogLayout.Items.Count > 0) cboLogLayout.SelectedIndex = 0;
            }
        }
        private void CboLogLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLogLayout.SelectedItem == null) return;
            LogLayout logLayout = (LogLayout)cboLogLayout.SelectedItem;
            UserControlContentFilter.UpdateLogLayout(logLayout);
            UsrControlMetadataFormating.UpdateLogMetadataProperties(logLayout.LogMetadataProperties);

            Reset();
        }
        private void CboLogProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILogProviderConfig logProviderConfig = (ILogProviderConfig)cboLogProvider.SelectedItem;
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
        /// <summary>
        /// Overrides the default command key processing to handle custom keyboard shortcuts at the form level.
        /// </summary>
        /// <param name="msg">A <see cref="Message"/> structure that represents the window message to process.</param>
        /// <param name="keyData">A <see cref="Keys"/> value that specifies the key or key combination to process.</param>
        /// <returns>
        /// True if the key combination was handled; otherwise, false to allow the base class to process the key.
        /// </returns>
        /// <remarks>
        /// This method intercepts key combinations such as Ctrl+R and Ctrl+F before they are passed to the focused control.
        /// - Ctrl+R triggers the "Record" functionality by invoking <see cref="BtnFormRecord_Click"/>.
        /// - Ctrl+F sets focus to the search control (<see cref="usrSearch"/>).
        /// For all other key combinations, the base class implementation is called.
        /// </remarks>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Check for Ctrl+R key combination
            if (keyData == (Keys.Control | Keys.R))
            {
                BtnFormRecord_Click(this, EventArgs.Empty);
                return true; // Indicate that the key combination has been handled
            }

            // Check for Ctrl+F key combination
            if (keyData == (Keys.Control | Keys.F))
            {
                usrSearch.Focus(); // Set focus to the search control
                return true; // Indicate that the key combination has been handled
            }

            // Check for Ctrl+S key combination
            if (keyData == (Keys.Control | Keys.S))
            {
                if (SourceProcessingManager.Instance.IsWorkerActive)
                {
                    BtnStop_Click(this, EventArgs.Empty);
                }
                else
                {
                    BtnRecordWithTimer_Click(this, EventArgs.Empty);
                }
                return true; // Indicate that the key combination has been handled
            }

            // Let the base class handle other key combinations
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
    }
}
