using System;
using System.Windows.Forms;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors
{
    public partial class UserControlPostProcessing : UserControl
    {
        public event EventHandler PostProcessingFinished;
        private readonly Timer timer = new();
        public UserControlPostProcessing()
        {
            InitializeComponent();
            timer.Interval = 500;

            timer.Tick += Timer_Tick;
            timer.Start();
        }
        public void UpdateLogCollection(LogCollection logCollection)
        {

        }

        private void StartPostProcessing()
        {
            IsProcessing = true;
            UpdateControlsEnabledState();
        }
        public void HandlePostProcessingFinished()
        {
            IsProcessing = false;
            UpdateControlsEnabledState();
            PostProcessingFinished(this, EventArgs.Empty);
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

        private void ApplyToVisibleLogToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void DeleteAllePostprocessingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void AutoApplyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
