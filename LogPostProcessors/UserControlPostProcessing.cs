using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors
{
    public partial class UserControlPostProcessing : UserControl
    {
        #region Initialization
        public UserControlPostProcessing()
        {
            InitializeComponent();
            UpdateControlsEnabledState();
            UpdatePreferredVisibilityFromUi();
        }
        #endregion

        #region Public properties
        public List<LogPostProcessorKind> VisibleProcessorKinds
        {
            get { return effectiveVisibleKinds; }
        }

        public bool IsProcessing { get; private set; } = false;
        #endregion

        #region Processing logic
        private LogPostProcessManager postProcessManager;
        private CancellationTokenSource postProcessCancellationSource;
        private void StartPostProcessing()
        {
            IsProcessing = true;
            UpdateControlsEnabledState();

            CancelPostProcessing();

            postProcessManager = new(LogCollection.Instance);
            postProcessCancellationSource = new CancellationTokenSource();
            postProcessManager.ProcessingFinished += Manager_ProcessingFinished;

            List<LogPostProcessorKind> kinds = [];
            if (prettyPrintJSONToolStripMenuItem.Checked)
            {
                kinds.Add(LogPostProcessorKind.JsonPrettyPrint);
            }
            if (prettyPrintXMLToolStripMenuItem.Checked)
            {
                kinds.Add(LogPostProcessorKind.XmlPrettyPrint);
            }

            bool started = postProcessManager.TryRun(0, LogCollection.Instance.LogEntries.Count - 1, kinds, postProcessCancellationSource.Token);

            if (!started)
            {
                postProcessCancellationSource?.Dispose();
                postProcessCancellationSource = null;
            }
        }

        private void Manager_ProcessingFinished(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => Manager_ProcessingFinished(sender, e)));
                return;
            }
            LogPostProcessingFinishedEventArgs eventArgs = (LogPostProcessingFinishedEventArgs)e;

            if (eventArgs.HasChanges) hasPostProcessingData = true;
            isVirtuallyReset = false;

            RecalculateEffectiveVisibility();

            IsProcessing = false;
            UpdateControlsEnabledState();

            if (eventArgs.HasChanges && !eventArgs.WasCanceled) RecalculateEffectiveVisibility();
            postProcessManager.ProcessingFinished -= Manager_ProcessingFinished;

            CancelPostProcessing();
        }

        #endregion

        #region Control event handlers and enabled state
        private void ApplyToVisibleLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartPostProcessing();
        }
        private void BtnPostProcess_Click(object sender, EventArgs e)
        {
            ContextMenuPostProcessing.Show(BtnPostProcess, new System.Drawing.Point(0, BtnPostProcess.Height));
            //Remove the focus from the post process button
            this.ActiveControl = null;
        }

        private void CancelPostProcessing()
        {
            RecalculateEffectiveVisibility();

            if (postProcessCancellationSource == null) return;

            postProcessCancellationSource.Cancel();
            postProcessCancellationSource.Dispose();
            postProcessCancellationSource = null;
        }

        private void DeleteAllePostprocessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isVirtuallyReset = true;
            RecalculateEffectiveVisibility();
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            postProcessCancellationSource?.Cancel();
        }

        private void PrettyPrintJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateControlsEnabledState();
            UpdatePreferredVisibilityFromUi();
        }
        private void PrettyPrintXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateControlsEnabledState();
            UpdatePreferredVisibilityFromUi();
        }

        private void UpdateControlsEnabledState()
        {
            bool processorSelected = prettyPrintJSONToolStripMenuItem.Checked || prettyPrintXMLToolStripMenuItem.Checked;

            BtnPostProcess.ImageIndex = IsProcessing ? 1 : 0;
            ApplyToVisibleLogToolStripMenuItem.Enabled = !IsProcessing && processorSelected;
            prettyPrintJSONToolStripMenuItem.Enabled = !IsProcessing;
            prettyPrintXMLToolStripMenuItem.Enabled = !IsProcessing;
            DeleteAllePostprocessingToolStripMenuItem.Enabled = !IsProcessing;
            StopToolStripMenuItem.Enabled = IsProcessing;
        }
        #endregion

        #region Preffered and effective visibility management
        /// <summary>
        /// What the user prefers to be shown
        /// </summary>
        private List<LogPostProcessorKind> preferredVisibleKinds = [];

        /// <summary>
        /// What is effectively shown
        /// </summary>
        private List<LogPostProcessorKind> effectiveVisibleKinds = [];

        // Whether any post-processing data exists
        private bool hasPostProcessingData;

        /// <summary>
        /// Indicates whether the post-processing state has been virtually reset (i.e., no post-processing data is considered to exist).
        /// </summary>
        private bool isVirtuallyReset = true;

        public event EventHandler VisibleProcessorsChanged;
        private void RecalculateEffectiveVisibility()
        {
            List<LogPostProcessorKind> newEffective;

            if (isVirtuallyReset || !hasPostProcessingData)
            {
                newEffective = [];
            }
            else
            {
                newEffective = [.. preferredVisibleKinds];
            }

            if (AreSameKinds(effectiveVisibleKinds, newEffective)) return;

            effectiveVisibleKinds = newEffective;
            VisibleProcessorsChanged?.Invoke(this, EventArgs.Empty);
        }
        private void UpdatePreferredVisibilityFromUi()
        {
            List<LogPostProcessorKind> preferred = new(2);

            if (prettyPrintJSONToolStripMenuItem.Checked)
                preferred.Add(LogPostProcessorKind.JsonPrettyPrint);

            if (prettyPrintXMLToolStripMenuItem.Checked)
                preferred.Add(LogPostProcessorKind.XmlPrettyPrint);

            if (AreSameKinds(preferredVisibleKinds, preferred))
                return;

            preferredVisibleKinds = preferred;
            RecalculateEffectiveVisibility();
        }
        private static bool AreSameKinds(List<LogPostProcessorKind> first, List<LogPostProcessorKind> second)
        {
            if (ReferenceEquals(first, second)) return true;

            if (first == null || second == null) return false;

            if (first.Count != second.Count) return false;

            for (int i = 0; i < first.Count; i++)
            {
                if (first[i] != second[i])
                    return false;
            }

            return true;
        }
        #endregion
    }
}
