using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Log.LogAppState;
using LogScraper.LogPostProcessors;
using LogScraper.Utilities;

namespace LogScraper.Controls
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
            ShortcutManager.Register(this, AppShortcut.PrettyPrint, () => BtnPrettyPrint_Click(this, EventArgs.Empty));
        }
        #endregion

        #region Public properties and methods
        private void LogPostProcessingControl_Load(object sender, EventArgs e)
        {
            LogAppState.Instance.ResetRequested += (ss, ee) => Reset();
            LogAppState.Instance.LogCollection.Changed += (s, e) => UpdateControls();
        }

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
        public void Reset()
        {
            CancelPostProcessing();
            JsonProcessingIsApplicable = false;
            XmlProcessingIsApplicable = false;
            LogAppState.Instance.RenderProcessorKinds.Set(VisibleProcessorKinds);
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

            bool started = false;
            if (LogAppState.Instance.LogCollectionIsAvailable)
            {
                postProcessManager = new(LogAppState.Instance.LogCollection.Value);
                postProcessCancellationSource = new CancellationTokenSource();
                postProcessManager.ProcessingFinished += Manager_ProcessingFinished;

                started = postProcessManager.TryRun(VisibleProcessorKinds, postProcessCancellationSource.Token);
            }
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

            bool hasChanges = false;
            LogPostProcessingFinishedEventArgs eventArgs = (LogPostProcessingFinishedEventArgs)e;
            foreach (var kind in Enum.GetValues<LogPostProcessorKind>())
            {
                hasChanges = !eventArgs.HasChanges[(int)kind];
                if (hasChanges) break;
            }

            if (hasChanges) LogAppState.Instance.RenderProcessorKinds.Set(VisibleProcessorKinds);
        }

        #endregion

        #region Control event handlers and enabled state

        private void CancelPostProcessing()
        {
            if (postProcessCancellationSource == null) return;
            postProcessCancellationSource.Cancel();
            postProcessCancellationSource.Dispose();
            postProcessCancellationSource = null;
        }

        private void UpdateControls()
        {
            bool processingInProgress = IsProcessing;
            BtnPrettyPrint.Enabled = LogAppState.Instance.LogCollectionIsAvailable && !processingInProgress;
            PrettyPrintJSONToolStripMenuItem.Enabled = LogAppState.Instance.LogCollectionIsAvailable && !IsProcessing;
            PrettyPrintXMLToolStripMenuItem.Enabled = LogAppState.Instance.LogCollectionIsAvailable && !IsProcessing;
            BtnPrettyPrint.ImageIndex = IsProcessing && JsonProcessingIsApplicable ? 1 : 0;
            RemoveToolStripMenuItem.Enabled = !IsProcessing && (JsonProcessingIsApplicable || XmlProcessingIsApplicable);
        }
        #endregion

        private void PrettyPrintJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JsonProcessingIsApplicable = true;
            UpdateControls();
            StartPostProcessing();

        }

        private void PrettyPrintXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlProcessingIsApplicable = true;
            UpdateControls();
            StartPostProcessing();
        }

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void BtnPrettyPrint_Click(object sender, EventArgs e)
        {
            JsonProcessingIsApplicable = true;
            XmlProcessingIsApplicable = true;
            UpdateControls();
            StartPostProcessing();
        }
    }
}