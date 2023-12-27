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
            LogScraperForm.BtnReadFromUrl_Click(sender, e);
        }

        private void BtnRead1Minute_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnDowloadLogLongTime_Click(sender, e);
        }

        private void BtnStop_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnStop_Click(sender, e);
        }

        private void BtnReset_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnClearLog_Click(sender, e);
            LogScraperForm.BtnReadFromUrl_Click(sender, e);
        }

        private void BtnOpen_Click(object sender, System.EventArgs e)
        {
            LogScraperForm.BtnOpenWithEditor_Click(sender, e);
        }
    }
}
