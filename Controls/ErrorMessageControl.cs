using System;
using System.Windows.Forms;
using LogScraper.Log.LogAppState;

namespace LogScraper.Controls
{
    public partial class ErrorMessageControl : UserControl
    {
        public ErrorMessageControl()
        {
            InitializeComponent();
            LogAppState.Instance.StatusMessage.Changed += (s,e) => ShowError();
        }

        public void ShowError()
        {
            (string message, bool isSucces) = LogAppState.Instance.StatusMessage.Value;
            TxtErrorMessage.Text = message;
            Visible = !isSucces;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public static void Clear()
        {
            LogAppState.Instance.StatusMessage.Set((string.Empty, true));
        }
    }
}