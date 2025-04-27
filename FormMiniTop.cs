using System.Threading;
using System.Windows.Forms;

namespace LogScraper
{
    public partial class FormMiniTop : Form
    {
        readonly FormLogScraper LogScraperForm = null;

        public FormMiniTop(FormLogScraper logScraperForm)
        {
            InitializeComponent();
            LogScraperForm = logScraperForm;
        }

        private void BtnPlay_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnPlay_Click(sender, e);
        }

        private void BtnPlayWithTimer_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnPlayWithTimer_Click(sender, e);
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
            LogScraperForm.BtnStop_Click(sender, e);
            Hide();
            LogScraperForm.WindowState = FormWindowState.Normal;
        }

        private void FormMiniTop_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            LogScraperForm.WindowState = FormWindowState.Normal;
            e.Cancel = true;
        }
        public void UpdateButtonsFromMainWindow()
        {
            lblLogEntriesTotalCount.Text = LogScraperForm.lblLogEntriesTotalValue.Text;
            lblLogEntriesFilteredCount.Text = LogScraperForm.lblNumberOfLogEntriesFiltered.Text;
            lblLogEntriesFilteredWithErrorCount.Text = LogScraperForm.lblNumberOfLogEntriesFilteredWithError.Text;
            lblLogEntriesFilteredWithErrorCount.ForeColor = LogScraperForm.lblNumberOfLogEntriesFilteredWithError.ForeColor;
            lblError.ForeColor = LogScraperForm.lblLogEntriesFilteredWithError.ForeColor;

            btnRead.Enabled = LogScraperForm.BtnPlay.Enabled;
            btnRead.Visible = LogScraperForm.BtnPlay.Visible;
            btnStop.Visible = LogScraperForm.btnStop.Visible;
            BtnPlayWithTimer.Text = LogScraperForm.BtnPlayWithTimes.Text;
            BtnPlayWithTimer.Image = LogScraperForm.BtnPlayWithTimes.Image;
            BtnPlayWithTimer.Enabled = LogScraperForm.BtnPlayWithTimes.Enabled;
            btnErase.Enabled = LogScraperForm.BtnErase.Enabled;
            btnOpenWithEditor.Enabled = LogScraperForm.btnOpenWithEditor.Enabled;
        }

        private void BtnOpenWithEditor_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnOpenWithEditor_Click(sender, e);
        }
    }
}
