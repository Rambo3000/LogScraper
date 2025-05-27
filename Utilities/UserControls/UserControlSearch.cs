using System;
using System.Drawing;
using System.Windows.Forms;

namespace LogScraper
{
    public partial class UserControlSearch : UserControl
    {
        public event Action<string, SearchDirectionUserControl, bool, bool, bool> Search;
        public enum SearchDirectionUserControl
        {
            Forward,
            Backward
        }

        public UserControlSearch()
        {
            InitializeComponent();
            TxtSearch_Leave(null, null);
            ToolTip.SetToolTip(chkWholeWordsOnly, "Alleen hele woorden zoeken");
            ToolTip.SetToolTip(chkCaseSensitive, "Hoofdletter gevoelig zoeken");
            ToolTip.SetToolTip(chkWrapAround, "Zoek verder vanaf het begin");
            ToolTip.SetToolTip(btnSearchNext, "Volgende zoeken");
            ToolTip.SetToolTip(btnSearchPrevious, "Vorige zoeken");
        }

        public void SetResultsFound(bool resultsFound)
        {
            lblNoResults.Visible = !resultsFound;
        }

        private void BtnSearchNext_Click(object sender, EventArgs e)
        {
            Search?.Invoke(txtSearch.Text, SearchDirectionUserControl.Forward, chkCaseSensitive.Checked, chkWholeWordsOnly.Checked, chkWrapAround.Checked);
        }

        private void BtnSearchPrevious_Click(object sender, EventArgs e)
        {
            Search?.Invoke(txtSearch.Text, SearchDirectionUserControl.Backward, chkCaseSensitive.Checked, chkWholeWordsOnly.Checked, chkWrapAround.Checked);
        }

        private bool IsSearchEmpty()
        {
            string search = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(search) || search == DefaulSearchtText) return true;

            return false;
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

        private const string DefaulSearchtText = "Zoeken...";

        private void TxtSearch_Enter(object sender, EventArgs e)
        {

            if (txtSearch.Text == DefaulSearchtText)
            {
                txtSearch.Text = string.Empty;
                txtSearch.ForeColor = SystemColors.ControlText;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = DefaulSearchtText;
                txtSearch.ForeColor = Color.DarkGray;
            }
        }
    }
}
