using System;
using System.Threading;
using System.Windows.Forms;

namespace LogScraper
{
    public partial class FormRecord : Form
    {
        readonly FormLogScraper LogScraperForm = null;

        public FormRecord(FormLogScraper logScraperForm)
        {
            InitializeComponent();
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
            Focus();
            LogScraperForm.WindowState = FormWindowState.Minimized;
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
            btnStop.Visible = LogScraperForm.btnStop.Visible;
            BtnRecordWithTimer.Text = LogScraperForm.BtnRecordWithTimer.Text;
            BtnRecordWithTimer.Image = LogScraperForm.BtnRecordWithTimer.Image;
            BtnRecordWithTimer.Enabled = LogScraperForm.BtnRecordWithTimer.Enabled;
            btnErase.Enabled = LogScraperForm.BtnErase.Enabled;
            btnOpenWithEditor.Enabled = LogScraperForm.btnOpenWithEditor.Enabled;
        }

    }
}
