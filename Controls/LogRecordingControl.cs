using System;
using System.Windows.Forms;
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
            LogAppState.Instance.ProcessingState.Changed += (s, e) => UpdateButtonStatus();
            LogAppState.Instance.IsSourceValid.Changed += (s, e) => UpdateButtonStatus();
            SourceProcessingManager.Instance.ProgressUpdate += HandleSourceProcessingWorkerProgressUpdate;

            ToolTip.SetToolTip(BtnRecord, "Start logophalen [F5]");
            ToolTip.SetToolTip(BtnRecordWithTimer, "Start automatisch log ophalen [CTRL-F5]");
            ToolTip.SetToolTip(BtnStop, "Stop log ophalen [SHIFT-F5]");
            ToolTip.SetToolTip(BtnStopWithTimer, "Stop automatisch log ophalen [SHIFT-F5]");

            UpdateButtonStatus();
        }
        #endregion

        #region Public methods

        public void Stop()
        {
            BtnStop.Enabled = false;
            LogProcessingService.StopFetching();
        }

        public void Start(bool timerEnabled)
        {
            if (timerEnabled)
                LogProcessingService.StartTimedFetch();
            else
                LogProcessingService.StartSingleFetch();
        }
        #endregion

        #region Buttons
        private void UpdateButtonStatus()
        {
            LogProcessingState fetch = LogAppState.Instance.ProcessingState.Value;

            bool sourceIsValid = LogAppState.Instance.IsSourceValid.Value;
            bool layoutSelected = LogAppState.Instance.Layout.Value != null;

            BtnRecord.Visible = fetch.IsTimed || !fetch.IsActive;
            BtnRecord.Enabled = !fetch.IsActive && sourceIsValid && layoutSelected;

            BtnStop.Visible = !BtnRecord.Visible;
            BtnStop.Enabled = fetch.IsActive;

            BtnRecordWithTimer.Visible = !fetch.IsTimed || !fetch.IsActive;
            BtnRecordWithTimer.Enabled = !fetch.IsActive && sourceIsValid && layoutSelected;

            BtnStopWithTimer.Visible = !BtnRecordWithTimer.Visible;
            BtnStopWithTimer.Enabled = fetch.IsActive;
        }

        private void HandleSourceProcessingWorkerProgressUpdate(int elapsedSeconds, int totalDurationInSeconds)
        {
            // Since this method is called from a non-UI thread, we need to check if we need to invoke it on the UI thread.
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleSourceProcessingWorkerProgressUpdate(elapsedSeconds, totalDurationInSeconds)));
                return;
            }
            BtnStopWithTimer.Text = TimeSpan.FromSeconds(totalDurationInSeconds - elapsedSeconds).ToString("m\\:ss");
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
        #endregion
    }
}
