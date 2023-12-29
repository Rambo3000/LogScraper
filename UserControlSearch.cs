using System;
using System.Windows.Forms;

namespace LogScraper
{
    public partial class UserControlSearch : UserControl
    {
        public event Action<string, SearchDirectionUserControl, bool, bool> Search;
        public enum SearchDirectionUserControl
        {
            Forward,
            Backward
        }

        public UserControlSearch()
        {
            InitializeComponent();
            ToolTip.SetToolTip(chkWholeWordsOnly, "Alleen hele woorden zoeken");
            ToolTip.SetToolTip(chkCaseSensitive, "Hoofdletter gevoelig zoeken");
            ToolTip.SetToolTip(btnSearchNext, "Volgende zoeken");
            ToolTip.SetToolTip(btnSearchPrevious, "Vorige zoeken");
        }

        public void SetResultsFound(bool resultsFound)
        { 
            lblNoResults.Visible = !resultsFound;
        }

        private void BtnSearchNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim())) return;
            Search?.Invoke(txtSearch.Text, SearchDirectionUserControl.Forward, chkCaseSensitive.Checked, chkWholeWordsOnly.Checked);
        }

        private void BtnSearchPrevious_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text)) return;
            Search?.Invoke(txtSearch.Text, SearchDirectionUserControl.Backward, chkCaseSensitive.Checked, chkWholeWordsOnly.Checked);
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchNext_Click(sender, e);
                e.Handled = true; // This prevents the system from processing the Enter key further
            }
        }

        private void ChkCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void ChkWholeWordsOnly_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }
    }
}
