using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors
{
    public partial class UserControlPostProcessing : UserControl
    {
        public event EventHandler PostProcessingFinished;
        private readonly System.Windows.Forms.Timer timer = new();
        public UserControlPostProcessing()
        {
            InitializeComponent();
            timer.Interval = 500;

            timer.Tick += Timer_Tick;
            timer.Start();
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsProcessing) BtnPostProcess.ImageIndex = 0;

            if (BtnPostProcess.ImageIndex == 0)
                BtnPostProcess.ImageIndex = 1;
            else
                BtnPostProcess.ImageIndex = 0;
        }

        private void BtnPostProcess_Click(object sender, EventArgs e)
        {
            StartPostProcessing();
            //Remove the focus from the post process button
            this.ActiveControl = null;
        }

        private void UpdateControlsEnabledState()
        {
            ApplyToVisibleLogToolStripMenuItem.Enabled = !IsProcessing;
            prettyPrintJSONToolStripMenuItem.Enabled = !IsProcessing;
            prettyPrintXMLToolStripMenuItem.Enabled = !IsProcessing;
            DeleteAllePostprocessingToolStripMenuItem.Enabled = !IsProcessing;
        }

        private void BtnPostProcess_MouseUp(object sender, MouseEventArgs e)
        {

            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            ContextMenuPostProcessing.Show(BtnPostProcess, e.Location);
        }
        private LogPostProcessManager postProcessManager;
        private CancellationTokenSource postProcessCancellationSource;

        private void ApplyToVisibleLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartPostProcessing();
        }
        private void CancelPostProcessing()
        {
            if (postProcessCancellationSource == null)
            {
                return;
            }

            postProcessCancellationSource.Cancel();
            postProcessCancellationSource.Dispose();
            postProcessCancellationSource = null;
        }


        private void Manager_ProcessingFinished(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(HandleProcessingFinished);
                return;
            }

            HandleProcessingFinished();
        }

        private void HandleProcessingFinished()
        {
            IsProcessing = false;
            UpdateControlsEnabledState();

            postProcessManager.ProcessingFinished -= Manager_ProcessingFinished;
            PostProcessingFinished?.Invoke(this, EventArgs.Empty);

            postProcessCancellationSource?.Dispose();
            postProcessCancellationSource = null;

        }

        private void DeleteAllePostprocessingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void AutoApplyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
