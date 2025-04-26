using LogScraper.Configuration;
using LogScraper.Configuration.LogProviderConfig;
using LogScraper.Export;
using LogScraper.Export.Workers;
using LogScraper.Extensions;
using LogScraper.Log;
using LogScraper.Log.Collection;
using LogScraper.Log.Metadata;
using LogScraper.LogProviders;
using LogScraper.LogProviders.Kubernetes;
using LogScraper.LogProviders.Runtime;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Workers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static LogScraper.Extensions.RichTextBoxExtensions;
using static LogScraper.UserControlSearch;

namespace LogScraper
{
    public partial class FormLogScraper : Form
    {
        #region Private fields
        private FormMiniTop miniTopForm = null;
        private bool isFormInitializing = true;

        private readonly LogExportWorkerManager logExportManager = new();
        private readonly SourceProcessingManager logProviderManager = new();

        private int numberOfSourceProcessingWorkers = 0;

        private LogMetadataFilterResult currentLogMetadataFilterResult;

        private readonly Timer timerMemoryUsage = new();

        private readonly Dictionary<LogMetadataPropertyAndValues, UserControlLogMetadataFilter> logMetadataPropertyControls = [];
        #endregion

        #region Initialization
        public FormLogScraper()
        {
            InitializeComponent();

            string version = Application.ProductVersion;
            const string versionSeperator = "+";
            if (version.Contains(versionSeperator))
            {
                version = version[..version.IndexOf(versionSeperator)];
            }

            lblVersion.Text = "v" + version;

            btnDowloadLogLongTime.Text = "Lees " + ConfigurationManager.GenericConfig.AutomaticReadTimeMinutes.ToString() + " min";

            timerMemoryUsage.Interval = 1000;
            timerMemoryUsage.Tick += new EventHandler(UpdateMemoryUsage);
            timerMemoryUsage.Start();

            usrKubernetes.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrKubernetes.StatusUpdate += HandleLogProviderStatusUpdate;
            usrRuntime.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrRuntime.StatusUpdate += HandleLogProviderStatusUpdate;
            usrFileLogProvider.SourceSelectionChanged += HandleLogProviderSourceSelectionChanged;
            usrFileLogProvider.StatusUpdate += HandleLogProviderStatusUpdate;

            UsrLogContentBegin.FilterChanged += HandleLogContentFilterUpdateBegin;
            UsrLogContentEnd.FilterChanged += HandleLogContentFilterUpdateEnd;
            UsrLogContentEnd.SelectedItemBackColor = Brushes.GreenYellow;
            usrControlMetadataFormating.SelectionChanged += HandleLogContentFilterUpdate;

            usrSearch.Search += UsrSearch_Search;

            logProviderManager.QueueLengthUpdate += HandleLogProviderManagerQueueUpdate;
        }
        private void FormLogScraper_Load(object sender, EventArgs e)
        {
            try
            {
                usrKubernetes.Update(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
                usrRuntime.UpdateRuntimeInstances(ConfigurationManager.LogProvidersConfig.RuntimeConfig.Instances);
                grpWriteLog.Visible = ConfigurationManager.GenericConfig.ExportToFile;
                PopulateLogLayouts();
                PopulateLogProviderControls();
                UpdateExportControls();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            isFormInitializing = false;
            UpdateStatisticsLogCollection();
        }
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
        #endregion

        #region Log provider work

        private DateTime? lastTrailTime = null;
        private void StartLogProviderAsync(int intervalInSeconds = -1, int durationInSeconds = -1)
        {
            // In case we want to download for a given duration, first get the log and after that start the duration
            if (durationInSeconds != -1) { StartLogProviderAsync(-1, -1); }
            try
            {
                btnReadFromUrl.Enabled = false;

                UpdateFormMiniControls();
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
                sourceProcessingWorker.DownloadCompleted += ProcessNewLogStringArray;
                sourceProcessingWorker.StatusUpdate += HandleLogProviderStatusUpdate;
                sourceProcessingWorker.ProgressUpdate += HandleSourceProcessingWorkerProgressUpdate;
                logProviderManager.AddWorker(sourceProcessingWorker, logProvider, intervalInSeconds, durationInSeconds);
            }
            catch (Exception ex)
            {
                HandleExceptionWhenReadingLog(ex);
            }
        }
        private void ProcessNewLogStringArray(string[] rawLog, DateTime? updatedLastTrailTime)
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
                    txtLogEntries.Text = JoinRawLogIntoString(rawLog);
                    throw;
                }

                LogEntryClassifier.ClassifyLogEntryMetadataProperties(logLayout, LogCollection.Instance);

                LogEntryClassifier.ClassifyLogEntryContentProperties(logLayout, LogCollection.Instance);

                UpdateFilterControls();
                FilterLogEntries();
                UpdateStatisticsLogCollection();
                HandleLogProviderStatusUpdate("Ok (" + DateTime.Now.ToString("HH:mm:ss") + ")", true);
                UpdateFormMiniControls();
                lastTrailTime = updatedLastTrailTime;
            }
            catch (Exception ex)
            {
                HandleExceptionWhenReadingLog(ex);
            }
        }
        private static string JoinRawLogIntoString(string[] rawLog)
        {
            StringBuilder builder = new();
            for (int i = 0; i < rawLog.Length; i++)
            {
                builder.Append(rawLog[i]);
                if (i < rawLog.Length - 1)
                {
                    builder.AppendLine();
                }
            }
            return builder.ToString();
        }
        #endregion

        #region Filter processing
        private void UpdateFilterControls()
        {
            LogLayout logLayout = (LogLayout)cboLogLayout.SelectedItem;
            List<LogMetadataPropertyAndValues> logMetadataPropertyAndValues = LogEntryClassifier.GetLogEntriesListOfMetadataPropertyAndValues(LogCollection.Instance.LogEntries, logLayout.LogMetadataProperties);
            UserControlLogMetadataFilter previousFilter = null;
            PanelFilters.SuspendDrawing();
            foreach (LogMetadataPropertyAndValues filterProperty in logMetadataPropertyAndValues)
            {
                if (!logMetadataPropertyControls.TryGetValue(filterProperty, out var userControlLogFilter))
                {
                    // Create a new UserControlLogMetadataFilter and add it to the dictionary and the form's controls.
                    userControlLogFilter = new UserControlLogMetadataFilter(filterProperty.LogMetadataProperty.Description)
                    {
                        Width = PanelFilters.ClientSize.Width
                    };
                    userControlLogFilter.FilterChanged += UserControlLogFilter_FilterChanged;

                    PanelFilters.Controls.Add(userControlLogFilter);
                    logMetadataPropertyControls[filterProperty] = userControlLogFilter;
                }
                userControlLogFilter.UpdateListView(filterProperty);

                if (previousFilter != null)
                {
                    userControlLogFilter.Top = previousFilter.Bottom + 5;
                }
                previousFilter = userControlLogFilter;
            }
            PanelFilters.ResumeDrawing();
        }

        private void UpdateFilterControlsCount(List<LogMetadataPropertyAndValues> filterProperties)
        {
            foreach (LogMetadataPropertyAndValues filterProperty in filterProperties)
            {
                if (logMetadataPropertyControls.TryGetValue(filterProperty, out var userControlLogFilter))
                {
                    userControlLogFilter.UpdateCountInListView(filterProperty);
                }
            }
        }
        private void FilterLogEntries()
        {
            // Get all the metadata properties and their values from all the user controls
            List<LogMetadataPropertyAndValues> LogMetadataPropertyAndValuesList = [];
            foreach (UserControlLogMetadataFilter userControlLogFilter in PanelFilters.Controls)
            {
                LogMetadataPropertyAndValuesList.Add(userControlLogFilter.GetCurrentLogMetadataPropertyAndValues());
            }

            // Filter the logentries into the FilterResult and update the count
            currentLogMetadataFilterResult = LogMetadataFilter.GetLogMetadataFilterResult(LogCollection.Instance.LogEntries, LogMetadataPropertyAndValuesList);

            UpdateFilterControlsCount(currentLogMetadataFilterResult.LogMetadataPropertyAndValuesList);
            UsrLogContentBegin.UpdateLogEntries(currentLogMetadataFilterResult.LogEntries);
            UsrLogContentEnd.UpdateLogEntries(currentLogMetadataFilterResult.LogEntries);

            UpdateAndWriteExport(currentLogMetadataFilterResult);
        }
        public static string CreateMetadataExampleFilterString(List<LogMetadataPropertyAndValues> LogMetadataPropertyAndValuesList)
        {
            List<string> filterGroups = [];

            foreach (var item in LogMetadataPropertyAndValuesList)
            {
                if (item.IsFilterEnabled)
                {
                    var values = item.LogMetadataValues.Where(kv => kv.Key.IsFilterEnabled).Select(kv => kv.Key.Value).ToList();

                    if (values.Count > 1)
                    {
                        string group = "(" + string.Join(" of ", values) + ")";
                        filterGroups.Add(group);
                    }
                    else if (values.Count == 1)
                    {
                        filterGroups.Add(values[0]);
                    }
                }
            }

            string result = string.Join(" en ", filterGroups);
            return string.IsNullOrEmpty(result) ? "-" : result;
        }
        #endregion

        #region Export
        private void UpdateExportControls()
        {
            grpWriteLog.Visible = ConfigurationManager.GenericConfig.ExportToFile; 
            if (ConfigurationManager.GenericConfig.EditorName != null) btnOpenWithEditor.Text = "Open in " + ConfigurationManager.GenericConfig.EditorName;

        }
        private void UpdateAndWriteExport(LogMetadataFilterResult logMetadataFilterResult)
        {
            LogExportSettings logExportSettings = new()
            {
                LogEntryBegin = UsrLogContentBegin.SelectedLogEntry,
                ExtraLogEntriesBegin = UsrLogContentBegin.ExtraLogEntryCount,
                LogEntryEnd = UsrLogContentEnd.SelectedLogEntry,
                ExtraLogEntriesEnd = UsrLogContentEnd.ExtraLogEntryCount,
                LogExportSettingsMetadata = usrControlMetadataFormating.LogExportSettingsMetadata,
            };

            logExportSettings.LogExportSettingsMetadata.RemoveMetaDataCriteria = ((LogLayout)cboLogLayout.SelectedItem).RemoveMetaDataCriteria;
            logExportSettings.LogExportSettingsMetadata.MetadataStartPosition = ((LogLayout)cboLogLayout.SelectedItem).StartPosition;

            LogExportData logExportData = LogDataExporter.GenerateExportedLogData(logMetadataFilterResult, logExportSettings, !chkShowAllLogEntries.Checked);

            if (chkShowAllLogEntries.Checked || logExportData.LogEntryCount < 2000)
            {
                lblNumberOfLogEntriesShown.Text = logExportData.LogEntryCount.ToString() + " (alle)";
                lblNumberOfLogEntriesShown.ForeColor = Color.Black;
            }
            else
            {
                lblNumberOfLogEntriesShown.Text = "2000/" + logExportData.LogEntryCount.ToString();
                lblNumberOfLogEntriesShown.ForeColor = Color.DarkRed;
            }

            lblNumberOfLogEntriesFiltered.Text = logExportData.LogEntryCount.ToString();

            int initialSelectionStart = txtLogEntries.SelectionStart;
            int initialSelectionLength = txtLogEntries.SelectionLength;

            txtLogEntries.SuspendDrawing();
            txtLogEntries.Text = logExportData.ExportRaw;
            HighlightBeginAndEndFilterLines();
            txtLogEntries.Select(initialSelectionStart, initialSelectionLength);
            txtLogEntries.ResumeDrawing();

            if (ConfigurationManager.GenericConfig.ExportToFile)
            {
                string fileName = Debugger.IsAttached ? AppContext.BaseDirectory + "Log.log" : ConfigurationManager.GenericConfig.ExportFileName;
                LogExporterWorker logExporter = new(fileName);
                logExporter.StatusUpdate += HandleLogExporterStatusUpdate;
                logExportManager.AddWorker(logExporter, logMetadataFilterResult, logExportSettings);
            }
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
        private void UpdateStatisticsLogCollection()
        {
            lblLogEntriesTotalValue.Text = LogCollection.Instance.LogEntries.Count.ToString();
            lblLogEntriesTotalValue.ForeColor = LogCollection.Instance.LogEntries.Count > 50000 ? Color.DarkRed : Color.Black;

            int count = LogCollection.Instance.ErrorCount;
            lblNumberOfLogEntriesFilteredWithError.Text = count.ToString();
            lblNumberOfLogEntriesFilteredWithError.ForeColor = count > 0 ? Color.DarkRed : Color.Black;
            lblLogEntriesFilteredWithError.ForeColor = lblNumberOfLogEntriesFilteredWithError.ForeColor;
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

        #region Reset and Clear
        private void ClearLog()
        {
            if (isFormInitializing) return;

            LogCollection.Instance.Clear();
            FilterLogEntries();
            UsrLogContentBegin.UpdateLogEntries(null);
            UsrLogContentEnd.UpdateLogEntries(null);

            txtLogEntries.Text = "";
            UpdateStatisticsLogCollection();
            UpdateDownloadControlsReadOnlyStatus();
            lastTrailTime = null;
        }
        private void Reset()
        {
            if (isFormInitializing) return;

            currentLogMetadataFilterResult = null;

            foreach (Control control in PanelFilters.Controls)
            {
                UserControlLogMetadataFilter userControlLogMetadataFilter = (UserControlLogMetadataFilter)control;
                userControlLogMetadataFilter.FilterChanged -= UserControlLogFilter_FilterChanged;
            }
            PanelFilters.Controls.Clear();
            logMetadataPropertyControls.Clear();
            txtStatusRead.Text = string.Empty;
            txtStatusRead.Visible = false;
            ClearLog();

            //Force memory cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion

        #region Form updating related functions
        private void UpdateMemoryUsage(object sender, EventArgs e)
        {
            lblMemoryUsageValue.Text = (Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024)).ToString() + " MB";
        }
        private void UpdateDownloadControlsReadOnlyStatus()
        {
            bool downloadingInProgress = numberOfSourceProcessingWorkers > 0;
            btnReadFromUrl.Enabled = !downloadingInProgress;
            btnDowloadLogLongTime.Visible = !downloadingInProgress;
            btnStop.Visible = downloadingInProgress;
            btnConfig.Enabled = !downloadingInProgress;
            GrpSourceAndLayout.Enabled = !downloadingInProgress;
            GrpLogProvidersSettings.Enabled = !downloadingInProgress;

            UpdateFormMiniControls();
        }
        private void UpdateFormMiniControls()
        {
            if (miniTopForm != null && !miniTopForm.IsDisposed)
            {
                miniTopForm.lblLogEntriesTotalCount.Text = lblLogEntriesTotalValue.Text;
                miniTopForm.lblLogEntriesFilteredCount.Text = lblNumberOfLogEntriesFiltered.Text;
                miniTopForm.lblLogEntriesFilteredWithErrorCount.Text = lblNumberOfLogEntriesFilteredWithError.Text;
                miniTopForm.lblLogEntriesFilteredWithErrorCount.ForeColor = lblNumberOfLogEntriesFilteredWithError.ForeColor;
                miniTopForm.lblError.ForeColor = lblLogEntriesFilteredWithError.ForeColor;

                miniTopForm.btnRead.Enabled = btnReadFromUrl.Enabled;
                miniTopForm.btnStop.Visible = btnStop.Visible;
                miniTopForm.btnStop.Text = btnStop.Text;
                miniTopForm.btnRead1Minute.Text = btnDowloadLogLongTime.Text;
                miniTopForm.btnReset.Enabled = BtnClearLog.Enabled;
                miniTopForm.btnReset.Enabled = btnReadFromUrl.Enabled;
            }
        }
        #endregion

        #region User controls event handling
        private void HandleLogExporterStatusUpdate(string message, bool isSucces)
        {
            txtStatusWrite.Text = message;
            txtStatusWrite.BackColor = txtStatusWrite.BackColor;
            txtStatusWrite.ForeColor = isSucces ? Color.DarkGreen : Color.DarkRed;
        }
        private void HandleLogProviderStatusUpdate(string message, bool isSucces)
        {
            txtStatusRead.Text = message;
            txtStatusRead.Visible = !isSucces;
        }
        private void HandleLogProviderManagerQueueUpdate(int numberOfWorkers)
        {
            numberOfSourceProcessingWorkers = numberOfWorkers;
            UpdateDownloadControlsReadOnlyStatus();
        }
        private void HandleLogContentFilterUpdate(object sender, EventArgs e)
        {
            lblBeginFilterEnabled.Visible = UsrLogContentBegin.FilterIsEnabled;
            lblEndFilterEnabled.Visible = UsrLogContentEnd.FilterIsEnabled;

            if (currentLogMetadataFilterResult != null) UpdateAndWriteExport(currentLogMetadataFilterResult);
        }
        private void HandleLogContentFilterUpdateBegin(object sender, EventArgs e)
        {
            HandleLogContentFilterUpdate(sender, e);
            txtLogEntries.SelectionStart = 1;
            txtLogEntries.ScrollToCaret();
        }
        private void HandleLogContentFilterUpdateEnd(object sender, EventArgs e)
        {
            usrControlMetadataFormating.SuspendDrawing();
            HandleLogContentFilterUpdate(sender, e);
            txtLogEntries.SelectionStart = txtLogEntries.Text.Length;
            txtLogEntries.ScrollToCaret();
            usrControlMetadataFormating.ResumeDrawing();
        }

        private void HandleLogProviderSourceSelectionChanged(object sender, EventArgs e)
        {
            ClearLog();
        }
        private void HandleExceptionWhenReadingLog(Exception ex)
        {
            ex.LogStackTraceToFile();
            HandleLogProviderStatusUpdate(ex.Message, false);
        }
        private void HandleSourceProcessingWorkerProgressUpdate(int elapsedSeconds, int duration)
        {
            TimeSpan tijd = TimeSpan.FromSeconds(duration - elapsedSeconds);
            string tijdFormat = string.Format("{0}:{1:D2}", (int)tijd.TotalMinutes, tijd.Seconds);
            btnStop.Text = "Stop" + (duration == -1 ? "" : " " + tijdFormat + "...");
        }
        #endregion

        #region Form controls events
        public void BtnReadFromUrl_Click(object sender, EventArgs e)
        {
            StartLogProviderAsync();
        }
        public void BtnDowloadLogLongTime_Click(object sender, EventArgs e)
        {
            StartLogProviderAsync(1, ConfigurationManager.GenericConfig.AutomaticReadTimeMinutes * 60);
        }
        public void BtnStop_Click(object sender, EventArgs e)
        {
            logProviderManager.CancelAllWorkers();
        }
        public void BtnClearLog_Click(object sender, EventArgs e)
        {
            ClearLog();
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void BtnMiniTopForm_Click(object sender, EventArgs e)
        {
            if (miniTopForm == null || miniTopForm.IsDisposed)
            {
                miniTopForm = new FormMiniTop(this);
            }
            UpdateFormMiniControls();
            miniTopForm.Show();
            miniTopForm.Focus();
            WindowState = FormWindowState.Minimized;
        }
        public void BtnOpenWithEditor_Click(object sender, EventArgs e)
        {
            string fileName = Debugger.IsAttached ? AppContext.BaseDirectory + "Log.log" : ConfigurationManager.GenericConfig.ExportFileName;
            LogExportWorkerManager.OpenFileInExternalEditor(fileName);
        }
        private void UserControlLogFilter_FilterChanged(object sender, EventArgs e)
        {
            FilterLogEntries();
        }
        private void ChkShowAllLogEntries_CheckedChanged(object sender, EventArgs e)
        {
            HandleLogContentFilterUpdate(sender, e);
        }
        private void CboLogLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogLayout logLayout = (LogLayout)cboLogLayout.SelectedItem;
            UsrLogContentBegin.UpdateFilterTypes(logLayout.LogContentProperties);
            UsrLogContentEnd.UpdateFilterTypes(logLayout.LogContentProperties);
            usrControlMetadataFormating.UpdateLogMetadataProperties(logLayout.LogMetadataProperties);

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
        int previousWidth = 0;
        private void PanelFilters_Resize(object sender, EventArgs e)
        {
            int newWidth = PanelFilters.ClientSize.Width;
            if (newWidth == previousWidth) return;

            PanelFilters.SuspendDrawing();
            PanelFilters.SuspendLayout();
            foreach (Control ctrl in PanelFilters.Controls)
            {
                if (ctrl.Width != newWidth)
                {
                    ctrl.Width = newWidth;
                }
            }
            PanelFilters.ResumeLayout();
            PanelFilters.ResumeDrawing();
            previousWidth = newWidth;
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            KubernetesConfig oldKubernetesConfig = ConfigurationManager.LogProvidersConfig.KubernetesConfig;
            RuntimeConfig oldRuntimeConfig = ConfigurationManager.LogProvidersConfig.RuntimeConfig;
            LogScraperConfig oldGenericConfig = ConfigurationManager.GenericConfig;
            LogLayoutsConfig logLayoutsConfig = ConfigurationManager.LogLayoutsConfig;

            using FormConfiguration form = new();
            DialogResult result = form.ShowDialog(this); // 'this' makes it modal to the main window

            if (result == DialogResult.OK)
            {
                bool kubernetesChanged = !oldKubernetesConfig.IsEqualByJsonComparison(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
                bool runtimeChanged = !oldRuntimeConfig.IsEqualByJsonComparison(ConfigurationManager.LogProvidersConfig.RuntimeConfig);

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

                if (!oldGenericConfig.IsEqualByJsonComparison(ConfigurationManager.GenericConfig))
                {

                    btnDowloadLogLongTime.Text = "Lees " + ConfigurationManager.GenericConfig.AutomaticReadTimeMinutes.ToString() + " min";
                    UpdateExportControls();
                }
                if (!logLayoutsConfig.IsEqualByJsonComparison(ConfigurationManager.LogLayoutsConfig)) PopulateLogLayouts();
            }
        }
    }
}
