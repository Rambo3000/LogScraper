using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Export;
using LogScraper.Export.Workers;
using LogScraper.Extensions;
using LogScraper.Log;
using LogScraper.Log.Collection;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.LogProviders;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Workers;
using static LogScraper.Extensions.RichTextBoxExtensions;
using static LogScraper.UserControlSearch;

namespace LogScraper
{
    public partial class FormLogScraper : Form
    {
        #region Form Initialization
        private LogMetadataFilterResult currentLogMetadataFilterResult;
        public FormLogScraper()
        {
            InitializeComponent();

            ToolTip.SetToolTip(BtnRecordWithTimer, "Lees " + ConfigurationManager.GenericConfig.AutomaticReadTimeMinutes.ToString() + " minuten");

            FormRecord.Instance.SetFormLogScraper(this);

            usrKubernetes.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrKubernetes.StatusUpdate += HandleErrorMessages;
            usrRuntime.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrRuntime.StatusUpdate += HandleErrorMessages;
            usrFileLogProvider.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrFileLogProvider.StatusUpdate += HandleErrorMessages;

            UsrLogContentBegin.FilterChanged += HandleLogContentFilterUpdateBegin;
            UsrLogContentEnd.FilterChanged += HandleLogContentFilterUpdateEnd;
            UsrLogContentEnd.SelectedItemBackColor = Brushes.GreenYellow;
            UsrControlMetadataFormating.SelectionChanged += HandleLogContentFilterUpdate;

            usrSearch.Search += UsrSearch_Search;

            SourceProcessingManager.Instance.QueueLengthUpdate += HandleLogProviderManagerQueueUpdate;
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
        }
        #endregion

        #region Initiate getting raw log and processing of raw log

        private DateTime? lastTrailTime = null;
        private void FetchRawLogAsync(int intervalInSeconds = -1, int durationInSeconds = -1)
        {
            // In case we want to download for a given duration, first get the log and after that start the duration
            if (durationInSeconds != -1) { FetchRawLogAsync(-1, -1); }
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
                    LogReader.ReadIntoLogCollection(rawLog, LogCollection.Instance, logLayout);
                }
                catch (Exception)
                {
                    //Write the raw log to the text box to not leave the user completely in the dark
                    txtLogEntries.Text = LogReader.JoinRawLogIntoString(rawLog);
                    throw;
                }

                LogEntryClassifier.ClassifyLogEntryMetadataProperties(logLayout, LogCollection.Instance);

                LogEntryClassifier.ClassifyLogEntryContentProperties(logLayout, LogCollection.Instance);

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
            UsrLogContentBegin.UpdateLogEntries(currentLogMetadataFilterResult.LogEntries);
            UsrLogContentEnd.UpdateLogEntries(currentLogMetadataFilterResult.LogEntries);

            WriteLogToScreenAndFile(currentLogMetadataFilterResult);
        }
        private void WriteLogToScreenAndFile(LogMetadataFilterResult logMetadataFilterResult)
        {
            LogExportSettings logExportSettings = new()
            {
                LogEntryBegin = UsrLogContentBegin.SelectedLogEntry,
                LogEntryEnd = UsrLogContentEnd.SelectedLogEntry,
                ExtraLogEntriesBegin = UsrLogContentBegin.ExtraLogEntryCount,
                ExtraLogEntriesEnd = UsrLogContentEnd.ExtraLogEntryCount,
                LogLayout = (LogLayout)cboLogLayout.SelectedItem,
                ShowOriginalMetadata = UsrControlMetadataFormating.ShowOriginalMetadata,
                SelectedMetadataProperties = UsrControlMetadataFormating.SelectedMetadataProperties
            };

            string exportedLog = LogDataExporter.CreateExportedLog(logMetadataFilterResult, logExportSettings, !chkShowAllLogEntries.Checked, out int entryCount);

            if (chkShowAllLogEntries.Checked || entryCount < 2000)
            {
                lblNumberOfLogEntriesShown.Text = entryCount.ToString() + " (alle)";
                lblNumberOfLogEntriesShown.ForeColor = Color.Black;
            }
            else
            {
                lblNumberOfLogEntriesShown.Text = "2000/" + entryCount.ToString();
                lblNumberOfLogEntriesShown.ForeColor = Color.DarkRed;
            }

            lblNumberOfLogEntriesFiltered.Text = entryCount.ToString();

            int initialSelectionStart = txtLogEntries.SelectionStart;
            int initialSelectionLength = txtLogEntries.SelectionLength;

            txtLogEntries.SuspendDrawing();
            txtLogEntries.Text = exportedLog;
            HighlightBeginAndEndFilterLines();
            txtLogEntries.Select(initialSelectionStart, initialSelectionLength);
            txtLogEntries.ResumeDrawing();

            LogExportWorkerManager.WriteToFile(logMetadataFilterResult, logExportSettings);
        }

        private void HighlightBeginAndEndFilterLines()
        {
            if (txtLogEntries.Lines.Length > 0)
            {
                if (UsrLogContentBegin.SelectedItem != null) txtLogEntries.HighlightLine(UsrLogContentBegin.ExtraLogEntryCount, Color.Orange, Color.Black);
                // Minus two because the index is 0 based and there is always an additional empty log entry (hard to remove)
                if (UsrLogContentEnd.SelectedItem != null) txtLogEntries.HighlightLine(txtLogEntries.Lines.Length - 2 - UsrLogContentEnd.ExtraLogEntryCount, Color.GreenYellow, Color.Black);
            }
        }
        #endregion

        #region Search
        private void UsrSearch_Search(string searchQuery, SearchDirectionUserControl searchDirectionUserControl, bool caseSensitive, bool wholeWord, bool wrapAround)
        {
            int scrollPosition = txtLogEntries.GetCharIndexFromPosition(new Point(0, 0));
            int selectionStart = txtLogEntries.SelectionStart;
            int selectionLenght = txtLogEntries.SelectionLength;

            txtLogEntries.SuspendDrawing();
            usrSearch.Enabled = false;
            Application.DoEvents();
            try
            {
                if (!chkShowAllLogEntries.Checked)
                {
                    chkShowAllLogEntries.Checked = true;
                    Application.DoEvents();
                }

                //Clean the logentry background and reinster begin and end filters
                txtLogEntries.ClearHighlighting();
                HighlightBeginAndEndFilterLines();

                //Return the scrolling to its original position
                txtLogEntries.Select(scrollPosition, 0);
                txtLogEntries.ScrollToCaret();
                txtLogEntries.Select(selectionStart, selectionLenght);

                SearchDirection searchDirection = searchDirectionUserControl == SearchDirectionUserControl.Forward ? SearchDirection.Forward : SearchDirection.Backward;
                bool found = txtLogEntries.Find(searchQuery.Trim(), searchDirection, wholeWord, caseSensitive, wrapAround);

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
                txtLogEntries.ResumeDrawing();
            }
        }
        #endregion

        #region Erase and reset
        private void Erase()
        {
            LogCollection.Instance.Clear();
            FilterLogEntries();
            UsrLogContentBegin.UpdateLogEntries(null);
            UsrLogContentEnd.UpdateLogEntries(null);

            txtLogEntries.Text = "";
            RefreshLogStatistics();
            UpdateButtonStatus();
            lastTrailTime = null;
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
            lblBeginFilterEnabled.Visible = UsrLogContentBegin.FilterIsEnabled;
            lblEndFilterEnabled.Visible = UsrLogContentEnd.FilterIsEnabled;

            if (currentLogMetadataFilterResult != null) WriteLogToScreenAndFile(currentLogMetadataFilterResult);
        }
        private void HandleLogContentFilterUpdateBegin(object sender, EventArgs e)
        {
            HandleLogContentFilterUpdate(sender, e);
            txtLogEntries.SelectionStart = 1;
            txtLogEntries.ScrollToCaret();
        }
        private void HandleLogContentFilterUpdateEnd(object sender, EventArgs e)
        {
            UsrControlMetadataFormating.SuspendDrawing();
            HandleLogContentFilterUpdate(sender, e);
            txtLogEntries.SelectionStart = txtLogEntries.Text.Length;
            txtLogEntries.ScrollToCaret();
            UsrControlMetadataFormating.ResumeDrawing();
        }

        private void HandleLogProviderSourceSelectionChanged(object sender, EventArgs e)
        {
            Erase();
        }
        private void HandleSourceProcessingWorkerProgressUpdate(int elapsedSeconds, int duration)
        {
            if (duration == -1)
            {
                BtnRecordWithTimer.Text = string.Empty;
                BtnRecordWithTimer.Image = Properties.Resources.timer_record_outline_24x24;
            }
            else
            {
                TimeSpan tijd = TimeSpan.FromSeconds(duration - elapsedSeconds);
                BtnRecordWithTimer.Image = null;
                BtnRecordWithTimer.Text = string.Format("{0}:{1:D2}", (int)tijd.TotalMinutes, tijd.Seconds);
            }
        }
        #endregion

        #region Buttons
        private void UpdateButtonStatus()
        {
            bool downloadingInProgress = SourceProcessingManager.Instance.QueueLength > 0;
            if (!downloadingInProgress)
            {
                HandleSourceProcessingWorkerProgressUpdate(-1, -1);
            }
            BtnRecord.Visible = !downloadingInProgress;
            BtnRecord.Enabled = !downloadingInProgress;
            BtnRecordWithTimer.Enabled = !downloadingInProgress;
            BtnStop.Visible = downloadingInProgress;
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
            SourceProcessingManager.Instance.CancelAllWorkers();
        }
        public void BtnErase_Click(object sender, EventArgs e)
        {
            Erase();
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        public void BtnOpenWithEditor_Click(object sender, EventArgs e)
        {
            LogExportWorkerManager.OpenFileInExternalEditor();
        }
        private void BtnFormRecord_Click(object sender, EventArgs e)
        {
            if (SourceProcessingManager.Instance.QueueLength == 0) { BtnRecordWithTimer_Click(sender, e); }
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
                    ToolTip.SetToolTip(BtnRecordWithTimer, "Lees " + ConfigurationManager.GenericConfig.AutomaticReadTimeMinutes.ToString() + " minuten");
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
            LogLayout logLayout = (LogLayout)cboLogLayout.SelectedItem;
            UsrLogContentBegin.UpdateFilterTypes(logLayout.LogContentProperties);
            UsrLogContentEnd.UpdateFilterTypes(logLayout.LogContentProperties);
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
        private void FormLogScraper_KeyDown(object sender, KeyEventArgs e)
        {
            // Check for Ctrl+F key combination
            if (e.Control && e.KeyCode == Keys.F)
            {
                // Set focus to the search TextBox
                usrSearch.Focus();

                // Suppress the key event to prevent further processing
                e.SuppressKeyPress = true;
            }
        }
        #endregion
    }
}
