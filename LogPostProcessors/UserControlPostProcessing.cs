using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors
{
    //TODO: invullen van logtekst
    //TODO: instelbaar maken welke postprocessing standaard is geselecteerd
    public partial class UserControlPostProcessing : UserControl
    {
        public event EventHandler PostProcessingResultChanged;
        public UserControlPostProcessing()
        {
            InitializeComponent();
            UpdateControlsEnabledState();
        }

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

        public bool IsProcessing { get; private set; } = false;

        private void BtnPostProcess_Click(object sender, EventArgs e)
        {
            ContextMenuPostProcessing.Show(BtnPostProcess, new System.Drawing.Point(0, BtnPostProcess.Height));
            //Remove the focus from the post process button
            this.ActiveControl = null;
        }

        private void UpdateControlsEnabledState()
        {
            BtnPostProcess.ImageIndex = IsProcessing ? 1 : 0;
            ApplyToVisibleLogToolStripMenuItem.Enabled = !IsProcessing;
            prettyPrintJSONToolStripMenuItem.Enabled = !IsProcessing;
            prettyPrintXMLToolStripMenuItem.Enabled = !IsProcessing;
            DeleteAllePostprocessingToolStripMenuItem.Enabled = !IsProcessing;
            StopToolStripMenuItem.Enabled = IsProcessing;
        }

        private void BtnPostProcess_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right && e.Button != MouseButtons.Left)
            {
                return;
            }

            //ContextMenuPostProcessing.Show(BtnPostProcess, e.Location);
        }
        private LogPostProcessManager postProcessManager;
        private CancellationTokenSource postProcessCancellationSource;

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


        private void Manager_ProcessingFinished(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => Manager_ProcessingFinished(sender, e)));
                return;
            }

            IsProcessing = false;
            UpdateControlsEnabledState();

            LogPostProcessingFinishedEventArgs eventArgs = (LogPostProcessingFinishedEventArgs)e;
            if (eventArgs.HasChanges && !eventArgs.WasCanceled) OnPostProcessingResultChanged(sender, e);
            postProcessManager.ProcessingFinished -= Manager_ProcessingFinished;

            CancelPostProcessing();
        }

        private void OnPostProcessingResultChanged(object sender, EventArgs e)
        {
            PostProcessingResultChanged?.Invoke(this, e);
        }

        private void DeleteAllePostprocessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogCollection.Instance.PostProcessCollection = new();
            OnPostProcessingResultChanged(sender, e);
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            postProcessCancellationSource?.Cancel();
        }
    }
}
