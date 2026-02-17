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

        #region Public properties and methods
        /// <summary>
        /// Gets the list of log post-processor kinds that are available and currently visible to the user.
        /// </summary>
        public List<LogPostProcessorKind> VisibleProcessorKinds
        {
            get { return effectiveVisibleKinds; }
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
            isVirtuallyReset = true;
            hasPostProcessingData = new bool[Enum.GetValues<LogPostProcessorKind>().Length];
            RecalculateEffectiveVisibility();
        }
        #endregion

        #region Post processing logic
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

            bool started = postProcessManager.TryRun(kinds, postProcessCancellationSource.Token);

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

            postProcessManager.ProcessingFinished -= Manager_ProcessingFinished;
            CancelPostProcessing();

            LogPostProcessingFinishedEventArgs eventArgs = (LogPostProcessingFinishedEventArgs)e;

            bool dataChanged = false;
            foreach (var kind in Enum.GetValues<LogPostProcessorKind>())
            {
                if (hasPostProcessingData[(int)kind] || !eventArgs.HasChanges[(int)kind]) continue;
                hasPostProcessingData[(int)kind] = true;
                dataChanged = true;
            }

            isVirtuallyReset = false;
            IsProcessing = false;
            UpdateControlsEnabledState();

            List<LogPostProcessorKind> previousKinds = effectiveVisibleKinds;
            RecalculateEffectiveVisibility(); // updates effectiveVisibleKinds, fires event if changed

            // Force fire if RecalculateEffectiveVisibility didnt change and thus didnt fire the event
            if (ReferenceEquals(effectiveVisibleKinds, previousKinds))
                VisibleProcessorsChanged?.Invoke(this, EventArgs.Empty);
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

        /// <summary>
        /// Indicates whether there is any post-processing data available per processorkind.
        /// </summary>
        private bool[] hasPostProcessingData = new bool[Enum.GetValues<LogPostProcessorKind>().Length];

        /// <summary>
        /// Indicates whether the post-processing state has been virtually reset (i.e., no post-processing data is considered to exist).
        /// </summary>
        private bool isVirtuallyReset = true;

        public event EventHandler VisibleProcessorsChanged;
        private void RecalculateEffectiveVisibility()
        {
            List<LogPostProcessorKind> newEffective;

            // If we are virtually reset or there is no post-processing data at all, nothing is visible
            if (isVirtuallyReset || !AnyValueTrue(hasPostProcessingData))
            {
                newEffective = [];
            }
            else
            {
                // Determine per processor if the user wants to see it and if there is data for it
                newEffective = new(preferredVisibleKinds.Count);
                foreach (LogPostProcessorKind kind in preferredVisibleKinds)
                {
                    if (hasPostProcessingData[(int)kind]) newEffective.Add(kind);
                }
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

        #region Control event handlers and enabled state
        private void BtnPostProcess_Click(object sender, EventArgs e)
        {
            StartPostProcessing();

            //Remove the focus from the post process button
            ActiveControl = null;
        }
        private void BtnPostProcess_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            ContextMenuPostProcessing.Show(BtnPostProcess, e.Location);

            //Remove the focus from the post process button
            ActiveControl = null;
        }

        private void ApplyToVisibleLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartPostProcessing();
        }

        private void CancelPostProcessing()
        {
            if (postProcessCancellationSource == null) return;
            postProcessCancellationSource.Cancel();
            postProcessCancellationSource.Dispose();
            postProcessCancellationSource = null;
        }

        private void DeleteAllePostprocessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isVirtuallyReset = true;
            UpdateControlsEnabledState();
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
            StopToolStripMenuItem.Enabled = IsProcessing;
            DeleteAllePostprocessingToolStripMenuItem.Enabled = !IsProcessing && AnyValueTrue(hasPostProcessingData) && !isVirtuallyReset;

            prettyPrintJSONToolStripMenuItem.Enabled = !IsProcessing;
            prettyPrintXMLToolStripMenuItem.Enabled = !IsProcessing;
        }

        private static bool AnyValueTrue(bool[] array)
        {
            foreach (var item in array)
            {
                if (item) return true;
            }
            return false;
        }
        #endregion
    }
}