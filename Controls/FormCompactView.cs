using System;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Processing;
using LogScraper.Utilities;
using LogScraper.Utilities.Extensions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LogScraper.Controls
{
    public partial class FormCompactView : Form
    {
        public event EventHandler ReturnToMainFormRequested;
        private static FormCompactView instance;
        private static readonly Lock lockObject = new();
        public static FormCompactView Instance
        {
            get
            {
                // Check if an instance already exists
                if (instance == null)
                {
                    // Use a lock to ensure only one thread creates the instance
                    lock (lockObject)
                    {
                        instance ??= new();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Updates the visible / total entry count shown at the top-right.
        /// When <paramref name="visible"/> equals <paramref name="total"/>, only the total is displayed.
        /// </summary>
        private void SetCounts()
        {
            int visible = LogAppState.Instance.FilterResultWithRange.Value?.LogEntries?.Count ?? 0;
            int total = LogAppState.Instance.LogCollection.Value?.TotalCount ?? 0;

            LblCount.Text = visible == total ? $"{total:N0}" : $"{visible:N0} / {total:N0}";
            ToolTip.SetToolTip(LblCount, visible == total
                ? "Totaal aantal logregels"
                : "Zichtbare logregels / Totaal aantal logregels");

            int errorCount = LogAppState.Instance.FilterResultWithRange.Value?.ErrorMask?.CountSetBits() ?? 0;
            bool hasError = errorCount > 0;
            LblErrorCount.Text = $"{errorCount:N0} error" + (errorCount > 1 ? "s" : "");
            LblErrorCount.ForeColor = hasError ? System.Drawing.Color.DarkRed : System.Drawing.Color.DimGray;
            LblErrorCount.Font = new System.Drawing.Font(LblErrorCount.Font, hasError ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular);
        }

        public FormCompactView()
        {
            InitializeComponent();

            LogAppState.Instance.FilterResultWithRange.Changed += (s, e) => SetCounts();

            ShortcutManager.Register(this, AppShortcut.ToggleCompactView, HideForm);
            ShortcutManager.Register(this, AppShortcut.StartRecording, LogProcessingService.StartSingleFetch);
            ShortcutManager.Register(this, AppShortcut.StartTimedRecording, LogProcessingService.StartTimedFetch);
            ShortcutManager.Register(this, AppShortcut.StopRecording, LogProcessingService.StopFetching);
            ShortcutManager.Register(this, AppShortcut.ClearLog, () => LogAppState.Instance.Reset(keepFilters: true));
            ShortcutManager.Register(this, AppShortcut.ResetApplication, () => LogAppState.Instance.Reset(keepFilters: false));
        }

        public void ShowForm()
        {
            base.Show();

            BringToFront();
            Activate();
            LogRecordingControl.Start(true);
        }
        public void HideForm()
        {
            Hide();
            ReturnToMainFormRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnErase_Click(object sender, System.EventArgs e)
        {
            LogAppState.Instance.Reset(keepFilters: true);
        }

        private void BtnBack_Click(object sender, System.EventArgs e)
        {
            HideForm();
        }

        private void FormMiniTop_FormClosing(object sender, FormClosingEventArgs e)
        {
            HideForm();
            e.Cancel = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ShortcutManager.ProcessKey(this, keyData))
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
