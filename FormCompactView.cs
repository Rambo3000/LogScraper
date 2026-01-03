using System;
using System.Threading;
using System.Windows.Forms;
using LogScraper.Sources.Workers;

namespace LogScraper
{
    public partial class FormCompactView : Form
    {
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

        private FormLogScraper LogScraperForm = null;

        public FormCompactView()
        {
            InitializeComponent();
        }
        public void SetFormLogScraper(FormLogScraper logScraperForm)
        {
            LogScraperForm = logScraperForm;
        }

        private void BtnRecord_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnRecord_Click(sender, e);
        }
        public void ShowForm()
        {
            UpdateButtonsFromMainWindow();
            base.Show();
            LogScraperForm.WindowState = FormWindowState.Minimized;

            // Ensure the form is brought to the foreground and activated
            BringToFront();
            Activate();
        }
        public void HideForm()
        {
            LogScraperForm.BtnStop_Click(this, EventArgs.Empty);
            Hide();
            LogScraperForm.WindowState = FormWindowState.Normal;
        }

        private void BtnRecordWithTimer_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnRecordWithTimer_Click(sender, e);
        }

        private void BtnStop_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnStop_Click(sender, e);
            UpdateButtonsFromMainWindow();
        }

        private void BtnErase_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnErase_Click(sender, e);
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
        private void BtnOpenWithEditor_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnOpenWithEditor_Click(sender, e);
        }

        public void UpdateButtonsFromMainWindow()
        {
            lblLogEntriesTotalCount.Text = LogScraperForm.lblLogEntriesTotalValue.Text;
            lblLogEntriesFilteredCount.Text = LogScraperForm.lblNumberOfLogEntriesFiltered.Text;
            lblLogEntriesFilteredWithErrorCount.Text = LogScraperForm.lblNumberOfLogEntriesFilteredWithError.Text;
            lblLogEntriesFilteredWithErrorCount.ForeColor = LogScraperForm.lblNumberOfLogEntriesFilteredWithError.ForeColor;
            lblError.ForeColor = LogScraperForm.lblLogEntriesFilteredWithError.ForeColor;

            BtnRecord.Enabled = LogScraperForm.BtnRecord.Enabled;
            BtnRecord.Visible = LogScraperForm.BtnRecord.Visible;
            btnStop.Visible = LogScraperForm.BtnStop.Visible;
            btnStop.Enabled = LogScraperForm.BtnStop.Enabled;
            BtnRecordWithTimer.Text = LogScraperForm.BtnRecordWithTimer.Text;
            BtnRecordWithTimer.Image = LogScraperForm.BtnRecordWithTimer.Image;
            BtnRecordWithTimer.Enabled = LogScraperForm.BtnRecordWithTimer.Enabled;
            btnErase.Enabled = LogScraperForm.BtnErase.Enabled;
            btnOpenWithEditor.Enabled = LogScraperForm.btnOpenWithEditor.Enabled;
        }

        #region Form key shortcuts
        /// <summary>
        /// Overrides the default command key processing to handle custom keyboard shortcuts at the form level.
        /// </summary>
        /// <param name="msg">A <see cref="Message"/> structure that represents the window message to process.</param>
        /// <param name="keyData">A <see cref="Keys"/> value that specifies the key or key combination to process.</param>
        /// <returns>
        /// True if the key combination was handled; otherwise, false to allow the base class to process the key.
        /// </returns>
        /// <remarks>
        /// This method intercepts key combinations such as Ctrl+R and Ctrl+F before they are passed to the focused control.
        /// - Ctrl+R triggers the "Back" functionality by invoking <see cref="BtnBack_Click"/>.
        /// For all other key combinations, the base class implementation is called.
        /// </remarks>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Check for Ctrl+R key combination
            if (keyData == (Keys.Control | Keys.R))
            {
                BtnBack_Click(this, EventArgs.Empty); // Trigger the desired action
                return true; // Indicate that the key combination has been handled
            }

            // Check for Ctrl+S key combination
            if (keyData == (Keys.Control | Keys.S))
            {
                if (SourceProcessingManager.Instance.IsWorkerActive)
                {
                    BtnStop_Click(this, EventArgs.Empty);
                }
                else
                {
                    BtnRecordWithTimer_Click(this, EventArgs.Empty);
                }
                return true; // Indicate that the key combination has been handled
            }

            // Let the base class handle other key combinations
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
    }
}
