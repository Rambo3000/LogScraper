using System;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log;
using LogScraper.Log.Layout;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Processing;
using LogScraper.Log.Processing.RawLogParsing;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Workers;
using LogScraper.Utilities.Extensions;

namespace LogScraper.Controls
{
    public partial class LogRecordingControl : UserControl
    {
        #region Initialization
        public LogRecordingControl()
        {
            InitializeComponent();
        }
        private void LogRecordingControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            LogAppState.Instance.IsSourceProcessingActive.Changed += (s, e) => UpdateButtonStatus();
            LogAppState.Instance.IsSourceValid.Changed += (s, e) => UpdateButtonStatus();
            SourceProcessingManager.Instance.ProgressUpdate += HandleSourceProcessingWorkerProgressUpdate;
            SetDynamicToolTips();
            UpdateButtonStatus();
        }
        #endregion

        #region Public methods

        public void Stop()
        {
            BtnStop.Enabled = false;
            SourceProcessingManager.Instance.CancelAllWorkers();
        }

        public void Start(bool timerEnabled)
        {
            if (LogAppState.Instance.IsSourceProcessingActive.Value) return;

            if (timerEnabled)
            {
                FetchRawLogAsync(1, ConfigurationManager.GenericConfig.AutomaticReadTimeMinutes * 60);
            }
            else
            {
                FetchRawLogAsync();
            }
        }
        #endregion

        #region Retrieval and processing of log
        private void FetchRawLogAsync(int intervalInSeconds = -1, int durationInSeconds = -1)
        {
            if (LogAppState.Instance.Layout.Value == null) return;
            try
            {
                BtnRecord.Enabled = false;
                BtnRecordWithTimer.Enabled = false;
                LogAppState.Instance.ProcessingStatus.Set(ProcessingStatus.Retrieving);
                Application.DoEvents();

                ISourceAdapter sourceAdatapter = LogAppState.Instance.SourceAdapterProvider(LogAppState.Instance.LastTrailTime.Value);

                SourceProcessingWorker sourceProcessingWorker = new();
                sourceProcessingWorker.DownloadCompleted += ProcessRawLog;
                SourceProcessingManager.Instance.AddWorker(sourceProcessingWorker, sourceAdatapter, intervalInSeconds, durationInSeconds);
            }
            catch (Exception ex)
            {
                ShowException(ex);
                UpdateButtonStatus();
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

                    // TODO: REQUIRED have a contraption in place for showing the raw log
                    //LogViewportControl.Text = RawLogParser.JoinRawLogIntoString(rawLog);
                    throw;
                }

                if (newLogEntriesReceived)
                {
                    LogEntryClassifier.Classify(logLayout, logCollection);
                    LogAppState.Instance.LogCollection.ForceSet(logCollection);
                }
                LogAppState.Instance.StatusMessage.Set((string.Empty, true));
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
            BtnStop.Visible = isSourceProcessingActive;
            BtnStop.Enabled = isSourceProcessingActive;
        }

        public void BtnRecord_Click(object sender, EventArgs e)
        {
            Start(false);
        }

        public void BtnRecordWithTimer_Click(object sender, EventArgs e)
        {
            Start(true);
        }

        public void BtnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void SetDynamicToolTips()
        {
            ToolTip.SetToolTip(BtnRecordWithTimer, "Lees " + ConfigurationManager.GenericConfig.AutomaticReadTimeMinutes.ToString() + " minuten [CTRL-S]");
        }
        #endregion
    }
}
