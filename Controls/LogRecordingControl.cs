using System;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Processing;
using LogScraper.Sources.Workers;

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
            LogProcessingService.Stop();
        }

        public void Start(bool timerEnabled)
        {
            if (LogAppState.Instance.IsSourceProcessingActive.Value) return;

            BtnRecord.Enabled = false;
            BtnRecordWithTimer.Enabled = false;
            Application.DoEvents();

            if (timerEnabled)
                LogProcessingService.StartFetching(1, ConfigAppState.Instance.GenericConfig.Value.AutomaticReadTimeMinutes * 60);
            else
                LogProcessingService.StartFetching();
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

        private void HandleSourceProcessingWorkerProgressUpdate(int elapsedSeconds, int totalDurationInSeconds)
        {
            // Since this method is called from a non-UI thread, we need to check if we need to invoke it on the UI thread.
            if (InvokeRequired) 
            {
                Invoke(new Action(() => HandleSourceProcessingWorkerProgressUpdate(elapsedSeconds, totalDurationInSeconds)));
                return;
            }
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
            ToolTip.SetToolTip(BtnRecordWithTimer, "Lees " + ConfigAppState.Instance.GenericConfig.Value.AutomaticReadTimeMinutes.ToString() + " minuten [CTRL-S]");
        }
        #endregion
    }
}
