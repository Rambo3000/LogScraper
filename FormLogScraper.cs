using LogScraper.Configuration;
using LogScraper.Export;
using LogScraper.Extensions;
using LogScraper.Log;
using LogScraper.Log.Collection;
using LogScraper.Log.Metadata;
using LogScraper.LogProviders;
using LogScraper.SourceAdapters;
using LogScraper.SourceProcessing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

            logProviderManager.QueueLengthUpdate += HandleLogProviderManagerQueueUpdate;
        }
        private void FormLogScraper_Load(object sender, EventArgs e)
        {
            try
            {
                usrKubernetes.UpdateClusters(ConfigurationManager.Config.KubernetesConfig.Clusters);
                usrRuntime.UpdateRuntimeInstances(ConfigurationManager.Config.RuntimeConfig.Instances);

                PopulateLogProviderControls();

                txtWriteToFilePath.Text = Debugger.IsAttached ? AppContext.BaseDirectory + "Log.log" : ConfigurationManager.Config.ExportFileName;

                if (ConfigurationManager.Config.EditorName != null) btnOpenWithEditor.Text = "Open in " + ConfigurationManager.Config.EditorName;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            isFormInitializing = false;
            UpdateStatisticsLogCollection();
        }
        private void PopulateLogProviderControls()
        {
            if (ConfigurationManager.Config.FileConfig != null)
            {
                cboLogProvider.Items.Add(ConfigurationManager.Config.FileConfig);
                if (ConfigurationManager.Config.LogProviderTypeDefault == LogProviderType.File) cboLogProvider.SelectedItem = ConfigurationManager.Config.FileConfig;
            }

            if (ConfigurationManager.Config.RuntimeConfig != null)
            {
                cboLogProvider.Items.Add(ConfigurationManager.Config.RuntimeConfig);
                if (ConfigurationManager.Config.LogProviderTypeDefault == LogProviderType.Runtime) cboLogProvider.SelectedItem = ConfigurationManager.Config.RuntimeConfig;
            }

            if (ConfigurationManager.Config.KubernetesConfig != null)
            {
                cboLogProvider.Items.Add(ConfigurationManager.Config.KubernetesConfig);
                if (ConfigurationManager.Config.LogProviderTypeDefault == LogProviderType.Kubernetes) cboLogProvider.SelectedItem = ConfigurationManager.Config.KubernetesConfig;
            }
        }
        #endregion

        #region Log provider wrok
        private void StartLogProviderAsync(int intervalInSeconds = -1, int durationInSeconds = -1)
        {
            try
            {
                btnReadFromUrl.Enabled = false;

                if (miniTopForm != null && !miniTopForm.IsDisposed)
                {
                    miniTopForm.btnRead.Enabled = btnReadFromUrl.Enabled;
                }
                Application.DoEvents();

                LogProviderType logProviderType = ((LogProviderGenericConfig)cboLogProvider.SelectedItem).LogProviderType;
                ISourceAdapter logProvider = logProviderType switch
                {
                    LogProviderType.Runtime => usrRuntime.GetSourceAdapter(),
                    LogProviderType.Kubernetes => usrKubernetes.GetSourceAdapter(),
                    LogProviderType.File => usrFileLogProvider.GetSourceAdapter(),
                    _ => throw new NotImplementedException()
                };

                SourceProcessingWorker sourceProcessingWorker = new();
                sourceProcessingWorker.DownloadCompleted += ProcessNewLogStringArray;
                sourceProcessingWorker.StatusUpdate += HandleLogProviderStatusUpdate;
                sourceProcessingWorker.ProgressUpdate += HandleSourceProcessingWorkerProgressUpdate;
                logProviderManager.AddWorker(sourceProcessingWorker, logProvider, intervalInSeconds, durationInSeconds, ((LogProviderGenericConfig)cboLogProvider.SelectedItem).DateTimeFormat);
            }
            catch (Exception ex)
            {
                HandleExceptionWhenReadingLog(ex);
            }
        }
        private void ProcessNewLogStringArray(string[] rawLog)
        {
            LogProviderGenericConfig logProviderGenericConfig = ((LogProviderGenericConfig)cboLogProvider.SelectedItem);
            try
            {
                LogReader.ReadIntoLogCollection(rawLog, LogCollection.Instance, logProviderGenericConfig.DateTimeFormat);

                LogLineClassifier.ClassifyLogLineMetadataProperties(logProviderGenericConfig.LogMetadataProperties, LogCollection.Instance);

                LogLineClassifier.ClassifyLogLineContentProperties(logProviderGenericConfig.LogContentBeginEndFilters, LogCollection.Instance);

                List<LogMetadataPropertyAndValues> logMetadataPropertyAndValues = LogLineClassifier.GetLogLinesListOfMetadataPropertyAndValues(LogCollection.Instance.LogLines, logProviderGenericConfig.LogMetadataProperties);
                UpdateFilterControls(logMetadataPropertyAndValues);
                FilterLoglines();
                UpdateStatisticsLogCollection();
                HandleLogProviderStatusUpdate("Ok (" + DateTime.Now.ToString("HH:mm:ss") + ")", true);
            }
            catch (Exception ex)
            {
                HandleExceptionWhenReadingLog(ex);
            }
        }
        #endregion

        #region Filter processing
        private void UpdateFilterControls(List<LogMetadataPropertyAndValues> filterProperties)
        {
            foreach (LogMetadataPropertyAndValues filterProperty in filterProperties)
            {
                if (!logMetadataPropertyControls.TryGetValue(filterProperty, out var userControlLogFilter))
                {
                    // Create a new UserControlLogMetadataFilter and add it to the dictionary and the form's controls.
                    userControlLogFilter = new UserControlLogMetadataFilter(filterProperty.LogMetadataProperty.Description);
                    logMetadataPropertyControls[filterProperty] = userControlLogFilter;
                    FlowPanelFilters.Controls.Add(userControlLogFilter);
                    userControlLogFilter.FilterChanged += UserControlLogFilter_FilterChanged;
                }
                userControlLogFilter.UpdateListView(filterProperty);
            }
            FlowPanelFilters_SizeChanged(null, null);
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
        private void FilterLoglines()
        {
            // Get all the metadata properties and their values from all the user controls
            List<LogMetadataPropertyAndValues> LogMetadataPropertyAndValuesList = [];
            foreach (UserControlLogMetadataFilter userControlLogFilter in FlowPanelFilters.Controls)
            {
                LogMetadataPropertyAndValuesList.Add(userControlLogFilter.GetCurrentLogMetadataPropertyAndValues());
            }

            // Filter the loglines into the FilterResult and update the count
            currentLogMetadataFilterResult = LogMetadataFilter.GetLogMetadataFilterResult(LogCollection.Instance.LogLines, LogMetadataPropertyAndValuesList);

            UpdateFilterControlsCount(currentLogMetadataFilterResult.LogMetadataPropertyAndValuesList);
            UsrLogContentBegin.UpdateLogLines(currentLogMetadataFilterResult.LogLines);
            UsrLogContentEnd.UpdateLogLines(currentLogMetadataFilterResult.LogLines);

            LblFilteredMetadataValues.Text = CreateMetadataExampleFilterString(currentLogMetadataFilterResult.LogMetadataPropertyAndValuesList);

            UpdateVisibilityControls();

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
        private void UpdateAndWriteExport(LogMetadataFilterResult logMetadataFilterResult)
        {
            LogExportSettings logExportSettings = new()
            {
                LoglineBegin = UsrLogContentBegin.SelectedLogLine,
                ExtraLinesBegin = UsrLogContentBegin.ExtraLineCount,
                LogLineEnd = UsrLogContentEnd.SelectedLogLine,
                ExtraLinesEnd = UsrLogContentEnd.ExtraLineCount,
                LogExportSettingsMetadata = usrControlMetadataFormating.LogExportSettingsMetadata
            };

            logExportSettings.LogExportSettingsMetadata.RemoveMetaDataCriteria = ((LogProviderGenericConfig)cboLogProvider.SelectedItem).RemoveMetaDataCriteria;

            LogExportData logExportData = LogExportDataCreator.CreateLogExportData(logMetadataFilterResult, logExportSettings, true);
            txtLogLines.Text = logExportData.ExportRaw;

            if (txtLogLines.Lines.Length > 0)
            {
                if (UsrLogContentBegin.SelectedItem != null) txtLogLines.HighlightLine(UsrLogContentBegin.ExtraLineCount, Color.Yellow, Color.Black);
                // Minus two because the index is 0 based and there is always an additional empty line (hard to remove)
                if (UsrLogContentEnd.SelectedItem != null) txtLogLines.HighlightLine(txtLogLines.Lines.Length - 2 - UsrLogContentEnd.ExtraLineCount, Color.GreenYellow , Color.Black); ;
            }

            LblBeginDateTime.Text = logExportData.DateTimeFirstLine.Year < 1000 ? "-" : logExportData.DateTimeFirstLine.ToString("yyyy-MM-dd HH:mm:ss");
            LblEndDateTime.Text = logExportData.DateTimeLastLine.Year < 1000 ? "-" : logExportData.DateTimeLastLine.ToString("yyyy-MM-dd HH:mm:ss");

            LblBeginEndFilteringValues.Text = ParseBeginEndFilteringValue(UsrLogContentBegin) + "/" + ParseBeginEndFilteringValue(UsrLogContentEnd);
            lblNumberOfLogLinesFiltered.Text = logExportData.LineCount.ToString();
            UpdateVisibilityControls();

            LogExporterWorker logExporter = new(txtWriteToFilePath.Text);
            logExporter.StatusUpdate += HandleLogExporterStatusUpdate;
            logExportManager.AddWorker(logExporter, logMetadataFilterResult, logExportSettings);
        }
        private static string ParseBeginEndFilteringValue(UserControlBeginEndFilter userControlEventsInLog)
        {
            string value = "-";

            if (userControlEventsInLog.SelectedItem == null) return value;

            value = userControlEventsInLog.SelectedItem[9..];
            if (userControlEventsInLog.ExtraLineCount > 0) value += userControlEventsInLog.ExtraLineCount;

            return value;
        }
        private void UpdateStatisticsLogCollection()
        {
            lblLogLinesTotalValue.Text = LogCollection.Instance.LogLines.Count.ToString();
            lblLogLinesTotalValue.ForeColor = LogCollection.Instance.LogLines.Count > 50000 ? Color.DarkRed : Color.Black;

            int count = LogCollection.Instance.ErrorCount;
            lblNumberOfLogLinesFilteredWithError.Text = count.ToString();
            lblNumberOfLogLinesFilteredWithError.ForeColor = count > 0 ? Color.DarkRed : Color.Black;
            lblLogLinesFilteredWithError.ForeColor = lblNumberOfLogLinesFilteredWithError.ForeColor;
            UpdateVisibilityControls();
        }
        #endregion

        #region Reset and Clear
        private void ClearLog()
        {
            if (isFormInitializing) return;

            LogCollection.Instance.Clear();
            FilterLoglines();
            UsrLogContentBegin.UpdateLogLines(null);
            UsrLogContentEnd.UpdateLogLines(null);

            txtLogLines.Text = "";
            UpdateStatisticsLogCollection();
            UpdateVisibilityControls();
        }
        private void Reset()
        {
            if (isFormInitializing) return;

            currentLogMetadataFilterResult = null;

            foreach (Control control in FlowPanelFilters.Controls)
            {
                UserControlLogMetadataFilter userControlLogMetadataFilter = (UserControlLogMetadataFilter)control;
                userControlLogMetadataFilter.FilterChanged -= UserControlLogFilter_FilterChanged;
            }
            FlowPanelFilters.Controls.Clear();
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
        private void UpdateVisibilityControls()
        {
            bool downloadingInProgress = numberOfSourceProcessingWorkers > 0;
            btnReadFromUrl.Enabled = !downloadingInProgress;
            btnDowloadLogLongTime.Visible = !downloadingInProgress;
            btnStop.Visible = downloadingInProgress;

            cboLogProvider.Enabled = !downloadingInProgress;
            lblLogProvider.Enabled = !downloadingInProgress;
            usrRuntime.Enabled = !downloadingInProgress;
            usrKubernetes.Enabled = !downloadingInProgress;

            if (miniTopForm != null && !miniTopForm.IsDisposed)
            {
                miniTopForm.lblLogLinesTotalCount.Text = lblLogLinesTotalValue.Text;
                miniTopForm.lblLogLinesFilteredCount.Text = lblNumberOfLogLinesFiltered.Text;
                miniTopForm.lblLogLinesFilteredWithErrorCount.Text = lblNumberOfLogLinesFilteredWithError.Text;
                miniTopForm.lblLogLinesFilteredWithErrorCount.ForeColor = lblNumberOfLogLinesFilteredWithError.ForeColor;
                miniTopForm.lblError.ForeColor = lblLogLinesFilteredWithError.ForeColor;

                miniTopForm.btnRead.Enabled = btnReadFromUrl.Enabled;
                miniTopForm.btnStop.Visible = btnStop.Visible;
                miniTopForm.btnStop.Text = btnStop.Text;
                miniTopForm.btnReset.Enabled = BtnClearLog.Enabled;
                miniTopForm.btnReset.Enabled = btnReadFromUrl.Enabled;
            }

            Application.DoEvents();
        }
        private void FlowPanelFilters_SizeChanged(object sender, EventArgs e)
        {
            int totalHeightControls = 0;
            foreach (Control control in FlowPanelFilters.Controls)
            {
                totalHeightControls += control.Height;
            }
            FlowPanelFilters.VerticalScroll.Visible = totalHeightControls + 15 > FlowPanelFilters.Height;
            FlowPanelFilters.HorizontalScroll.Enabled = false;
            FlowPanelFilters.HorizontalScroll.Visible = false;
            foreach (Control control in FlowPanelFilters.Controls)
            {
                control.Width = FlowPanelFilters.Width - (FlowPanelFilters.VerticalScroll.Visible ? 20 : 0);
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
            UpdateVisibilityControls();
        }
        private void HandleLogContentFilterUpdate(object sender, EventArgs e)
        {
            if (currentLogMetadataFilterResult != null) UpdateAndWriteExport(currentLogMetadataFilterResult);
        }
        private void HandleLogContentFilterUpdateBegin(object sender, EventArgs e)
        {
            HandleLogContentFilterUpdate(sender, e);
            txtLogLines.SelectionStart = 1;
            txtLogLines.ScrollToCaret();
        }
        private void HandleLogContentFilterUpdateEnd(object sender, EventArgs e)
        {
            HandleLogContentFilterUpdate(sender, e);
            txtLogLines.SelectionStart = txtLogLines.Text.Length;
            txtLogLines.ScrollToCaret();
        }

        private void HandleLogProviderSourceSelectionChanged(object sender, EventArgs e)
        {
            ClearLog();
        }
        private void HandleExceptionWhenReadingLog(Exception ex)
        {
            try
            {
                File.WriteAllText(AppContext.BaseDirectory + "StackTrace.log", ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch { }

            HandleLogProviderStatusUpdate(ex.Message, false);
        }
        private void HandleSourceProcessingWorkerProgressUpdate(int elapsedSeconds, int duration)
        {
            btnStop.Text = "Stop" + (duration == -1 ? "" : " " + (duration - elapsedSeconds).ToString() + "...");
        }
        #endregion

        #region Form controls events
        public void BtnReadFromUrl_Click(object sender, EventArgs e)
        {
            StartLogProviderAsync();
        }
        public void BtnDowloadLogLongTime_Click(object sender, EventArgs e)
        {
            StartLogProviderAsync(1, 60);
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
            miniTopForm.Show();
            miniTopForm.Focus();
            UpdateVisibilityControls();
        }
        public void BtnOpenWithEditor_Click(object sender, EventArgs e)
        {
            LogExportWorkerManager.OpenFileInExternalEditor(txtWriteToFilePath.Text);
        }
        private void UserControlLogFilter_FilterChanged(object sender, EventArgs e)
        {
            FilterLoglines();
        }
        private void CboLogProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogProviderGenericConfig logProviderGenericConfig = (LogProviderGenericConfig)cboLogProvider.SelectedItem;
            usrRuntime.Visible = logProviderGenericConfig.LogProviderType == LogProviderType.Runtime;
            usrKubernetes.Visible = logProviderGenericConfig.LogProviderType == LogProviderType.Kubernetes;
            usrFileLogProvider.Visible = logProviderGenericConfig.LogProviderType == LogProviderType.File;

            UsrLogContentBegin.UpdateFilterTypes(logProviderGenericConfig.LogContentBeginEndFilters);
            UsrLogContentEnd.UpdateFilterTypes(logProviderGenericConfig.LogContentBeginEndFilters);
            usrControlMetadataFormating.UpdateLogMetadataProperties(logProviderGenericConfig.LogMetadataProperties);

            Reset();
        }
        #endregion
    }
}
