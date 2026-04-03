using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors
{
    public partial class LogPostProcessingControl : UserControl
    {
        #region Initialization
        private bool JsonProcessingIsApplicable = false;
        private bool XmlProcessingIsApplicable = false;

        public LogPostProcessingControl()
        {
            InitializeComponent();
            UpdateControls();
        }
        #endregion

        #region Public properties and methods

        public event EventHandler PostProcessingResultsChanged;

        /// <summary>
        /// Gets the list of log post-processor kinds that are available and currently visible to the user.
        /// </summary>
        public List<LogPostProcessorKind> VisibleProcessorKinds
        {
            get
            {
                List<LogPostProcessorKind> kinds = [];
                if (JsonProcessingIsApplicable) kinds.Add(LogPostProcessorKind.JsonPrettyPrint);
                if (XmlProcessingIsApplicable) kinds.Add(LogPostProcessorKind.XmlPrettyPrint);
                return kinds;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a processing operation is currently in progress.
        /// </summary>
        public bool IsProcessing { get; private set; } = false;

        /// <summary>
        /// Resets the state of the object by clearing all post-processing data and related flags.
        /// </summary>
        public void Clear()
        {
            CancelPostProcessing();
            JsonProcessingIsApplicable = false;
            XmlProcessingIsApplicable = false;
            UpdateControls();
        }
        #endregion

        #region Post processing logic
        private LogPostProcessManager postProcessManager;
        private CancellationTokenSource postProcessCancellationSource;

        private void StartPostProcessing()
        {
            IsProcessing = true;
            UpdateControls();

            CancelPostProcessing();

            postProcessManager = new(LogCollection.Instance);
            postProcessCancellationSource = new CancellationTokenSource();
            postProcessManager.ProcessingFinished += Manager_ProcessingFinished;

            bool started = postProcessManager.TryRun(VisibleProcessorKinds, postProcessCancellationSource.Token);

            if (!started)
            {
                postProcessCancellationSource?.Dispose();
                postProcessCancellationSource = null;
                IsProcessing = false;
                UpdateControls();
            }
        }

        private void Manager_ProcessingFinished(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => Manager_ProcessingFinished(sender, e)));
                return;
            }


            postProcessManager.ProcessingFinished -= Manager_ProcessingFinished;
            CancelPostProcessing();

            IsProcessing = false;
            UpdateControls();
            BtnJson.ImageIndex = 0;
            BtnXml.ImageIndex = 1;

            bool hasChanges = false;
            LogPostProcessingFinishedEventArgs eventArgs = (LogPostProcessingFinishedEventArgs)e;
            foreach (var kind in Enum.GetValues<LogPostProcessorKind>())
            {
                hasChanges = !eventArgs.HasChanges[(int)kind];
                if (hasChanges) break;
            }

            if (hasChanges) PostProcessingResultsChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Control event handlers and enabled state

        private void BtnJson_Click(object sender, EventArgs e)
        {
            JsonProcessingIsApplicable = true;
            BtnJson.ImageIndex = 2;
            StartPostProcessing();
        }

        private void BtnXml_Click(object sender, EventArgs e)
        {
            XmlProcessingIsApplicable = true;
            BtnXml.ImageIndex = 2;
            StartPostProcessing();
        }

        private void CancelPostProcessing()
        {
            if (postProcessCancellationSource == null) return;
            postProcessCancellationSource.Cancel();
            postProcessCancellationSource.Dispose();
            postProcessCancellationSource = null;
        }

        private void UpdateControls()
        {
            BtnJson.Enabled = !IsProcessing;
            BtnXml.Enabled = !IsProcessing;
        }

        #endregion
    }
}