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

        private void BtnRead_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnPlay_Click(sender, e);
        }

        private void BtnRead1Minute_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnPlayWithTimer_Click(sender, e);
        }

        private void BtnStop_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnStop_Click(sender, e);
        }

        private void BtnReset_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnErase_Click(sender, e);
            LogScraperForm.BtnPlay_Click(sender, e);
        }

        private void BtnBack_Click(object sender, System.EventArgs e)
        {
            Hide();
            LogScraperForm.WindowState = FormWindowState.Normal;
        }

        private void FormMiniTop_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            LogScraperForm.WindowState = FormWindowState.Normal;
            e.Cancel = true;
        }
    }
}
