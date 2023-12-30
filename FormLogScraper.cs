using LogScraper.Configuration;
using LogScraper.Configuration.LogProviderConfig;
using LogScraper.Export;
using LogScraper.Export.Workers;
using LogScraper.Extensions;
using LogScraper.Log;
using LogScraper.Log.Collection;
using LogScraper.Log.Metadata;
using LogScraper.LogProviders;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Workers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
                usrKubernetes.UpdateClusters(ConfigurationManager.LogProvidersConfig.KubernetesConfig.Clusters);
                usrRuntime.UpdateRuntimeInstances(ConfigurationManager.LogProvidersConfig.RuntimeConfig.Instances);

                PopulateLogLayouts();
                PopulateLogProviderControls();

                txtWriteToFilePath.Text = Debugger.IsAttached ? AppContext.BaseDirectory + "Log.log" : ConfigurationManager.GenericConfig.ExportFileName;

                if (ConfigurationManager.GenericConfig.EditorName != null) btnOpenWithEditor.Text = "Open in " + ConfigurationManager.GenericConfig.EditorName;
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
            if (ConfigurationManager.LogLayouts != null)
            {
                cboLogLayout.Items.AddRange(ConfigurationManager.LogLayouts.ToArray());
                if (cboLogLayout.Items.Count > 0) cboLogLayout.SelectedIndex = 0;
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

                LogProviderType logProviderType = ((ILogProviderConfig)cboLogProvider.SelectedItem).LogProviderType;
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
                logProviderManager.AddWorker(sourceProcessingWorker, logProvider, intervalInSeconds, durationInSeconds);
            }
            catch (Exception ex)
            {
                HandleExceptionWhenReadingLog(ex);
            }
        }
        private void ProcessNewLogStringArray(string[] rawLog)
        {
            LogLayout logLayout = (LogLayout)cboLogLayout.SelectedItem;
            try
            {
                LogReader.ReadIntoLogCollection(rawLog, LogCollection.Instance, logLayout);

                LogLineClassifier.ClassifyLogLineMetadataProperties(logLayout.LogMetadataProperties, LogCollection.Instance);

                LogLineClassifier.ClassifyLogLineContentProperties(logLayout.LogContentBeginEndFilters, LogCollection.Instance);

                List<LogMetadataPropertyAndValues> logMetadataPropertyAndValues = LogLineClassifier.GetLogLinesListOfMetadataPropertyAndValues(LogCollection.Instance.LogLines, logLayout.LogMetadataProperties);
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

            logExportSettings.LogExportSettingsMetadata.RemoveMetaDataCriteria = ((LogLayout)cboLogLayout.SelectedItem).RemoveMetaDataCriteria;

            LogExportData logExportData = LogExportDataCreator.CreateLogExportData(logMetadataFilterResult, logExportSettings, !chkShowAllLogLines.Checked);

            if (chkShowAllLogLines.Checked || logExportData.LineCount < 2000)
            {
                lblNumberOfLogLinesShown.Text = logExportData.LineCount.ToString() + " (alle)";
                lblNumberOfLogLinesShown.ForeColor = Color.Black;
            }
            else
            {
                lblNumberOfLogLinesShown.Text = "2000/" + logExportData.LineCount.ToString();
                lblNumberOfLogLinesShown.ForeColor = Color.DarkRed;
            }

            lblNumberOfLogLinesFiltered.Text = logExportData.LineCount.ToString();

            int initialSelectionStart = txtLogLines.SelectionStart;
            int initialSelectionLength = txtLogLines.SelectionLength;

            txtLogLines.SuspendDrawing();
            txtLogLines.Text = logExportData.ExportRaw;
            HighlightBeginAndEndFilterLines();
            txtLogLines.Select(initialSelectionStart, initialSelectionLength);
            txtLogLines.ResumeDrawing();

            UpdateVisibilityControls();

            LogExporterWorker logExporter = new(txtWriteToFilePath.Text);
            logExporter.StatusUpdate += HandleLogExporterStatusUpdate;
            logExportManager.AddWorker(logExporter, logMetadataFilterResult, logExportSettings);
        }

        private void HighlightBeginAndEndFilterLines()
        {
            if (txtLogLines.Lines.Length > 0)
            {
                if (UsrLogContentBegin.SelectedItem != null) txtLogLines.HighlightLine(UsrLogContentBegin.ExtraLineCount, Color.Orange, Color.Black);
                // Minus two because the index is 0 based and there is always an additional empty line (hard to remove)
                if (UsrLogContentEnd.SelectedItem != null) txtLogLines.HighlightLine(txtLogLines.Lines.Length - 2 - UsrLogContentEnd.ExtraLineCount, Color.GreenYellow, Color.Black);
            }
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

        #region Search
        private void UsrSearch_Search(string searchQuery, SearchDirectionUserControl searchDirectionUserControl, bool caseSensitive, bool wholeWord)
        {
            int scrollPosition = txtLogLines.GetCharIndexFromPosition(new Point(0, 0));
            int selectionStart = txtLogLines.SelectionStart;
            int selectionLenght = txtLogLines.SelectionLength;

            txtLogLines.SuspendDrawing();
            usrSearch.Enabled = false;
            Application.DoEvents();
            try
            {
                if (!chkShowAllLogLines.Checked)
                {
                    chkShowAllLogLines.Checked = true;
                    Application.DoEvents();
                }

                //Clean the logline background and reinster begin and end filters
                txtLogLines.ClearHighlighting();
                HighlightBeginAndEndFilterLines();

                //Return the scrolling to its original position
                txtLogLines.Select(scrollPosition, 0);
                txtLogLines.ScrollToCaret();
                txtLogLines.Select(selectionStart, selectionLenght);

                SearchDirection searchDirection = searchDirectionUserControl == SearchDirectionUserControl.Forward ? SearchDirection.Forward : SearchDirection.Backward;
                bool found = txtLogLines.Find(searchQuery.Trim(), searchDirection, wholeWord, caseSensitive);

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
                txtLogLines.ResumeDrawing();
            }
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

            foreach (System.Windows.Forms.Control control in FlowPanelFilters.Controls)
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
            FlowPanelFilters.SuspendDrawing();
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
            FlowPanelFilters.ResumeDrawing();
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
            lblBeginFilterEnabled.Visible = UsrLogContentBegin.FilterIsEnabled;
            lblEndFilterEnabled.Visible=UsrLogContentEnd.FilterIsEnabled;

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
            usrControlMetadataFormating.SuspendDrawing();
            HandleLogContentFilterUpdate(sender, e);
            txtLogLines.SelectionStart = txtLogLines.Text.Length;
            txtLogLines.ScrollToCaret();
            usrControlMetadataFormating.ResumeDrawing();
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
        private void ChkShowAllLogLines_CheckedChanged(object sender, EventArgs e)
        {
            HandleLogContentFilterUpdate(sender, e);
        }
        private void CboLogLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogLayout logLayout = (LogLayout)cboLogLayout.SelectedItem;
            UsrLogContentBegin.UpdateFilterTypes(logLayout.LogContentBeginEndFilters);
            UsrLogContentEnd.UpdateFilterTypes(logLayout.LogContentBeginEndFilters);
            usrControlMetadataFormating.UpdateLogMetadataProperties(logLayout.LogMetadataProperties);

            Reset();
        }
        private void CboLogProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILogProviderConfig logProviderConfig = (ILogProviderConfig)cboLogProvider.SelectedItem;
            usrRuntime.Visible = logProviderConfig.LogProviderType == LogProviderType.Runtime;
            usrKubernetes.Visible = logProviderConfig.LogProviderType == LogProviderType.Kubernetes;
            usrFileLogProvider.Visible = logProviderConfig.LogProviderType == LogProviderType.File;

            if (logProviderConfig.DefaultLogLayout != null) cboLogLayout.SelectedItem = logProviderConfig.DefaultLogLayout;

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
