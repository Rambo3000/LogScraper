using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Rendering;
using LogScraper.Utilities.UserControls;

namespace LogScraper
{
    public partial class UserControlSearch : UserControl
    {
        #region Private fields and initialization

        public enum SearchMode
        {
            All,
            Next,
            Previous
        }

        private const string DefaultSearchText = "Zoeken...";

        /// <summary>
        /// Fired when the user triggers a directional search (Next/Previous).
        /// The main form should forward this to the Scintilla navigation layer.
        /// </summary>
        public event Action<SearchSettings> Search;

        /// <summary>
        /// Fired when the search settings have materially changed and the result list should be refreshed.
        /// The main form should call SearchResultListControl.UpdateSearchResults with the provided settings.
        /// Suppressed on Next/Previous when settings are identical to the last fired settings.
        /// </summary>
        public event Action<SearchSettings> SearchSettingsChanged;

        private List<LogEntry> logEntries;
        public SearchMode SelectedSearchMode { get; private set; } = SearchMode.Next;
        private SearchSettings lastFiredSearchSettings;

        public UserControlSearch()
        {
            InitializeComponent();
            TxtSearch_Leave(null, null);
            ItemAll.Click += ItemAll_Click;
            ItemCaseSensitive.Click += ItemCaseSensitive_CheckedChanged;
            ItemNext.Click += ItemNext_Click;
            ItemPrevious.Click += ItemPrevious_Click;
            ItemWholeWords.Click += ItemWholeWords_CheckedChanged;
        }

        #endregion

        #region Enums

        public enum SearchDirection
        {
            Forward,
            Backward
        }

        #endregion

        #region Public interface

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMetadataSearchEnabled { get; set; }

        /// <summary>
        /// Render settings used to produce display strings in the result list.
        /// Set by the main form when log render settings change.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogRenderSettings LogRenderSettings { get; set; }

        public void Clear()
        {
            txtSearch.Clear();
            UpdateButtons();
            FireSearchSettingsChanged(force: false);
        }

        public void UpdateLogEntries(List<LogEntry> entries)
        {
            logEntries = entries;
            lastFiredSearchSettings = null;
        }

        /// <summary>
        /// Builds the current search settings from the control state.
        /// </summary>
        public SearchSettings BuildSearchSettings(SearchDirection direction = SearchDirection.Forward)
        {
            return new SearchSettings
            {
                LogRange = new LogRange
                {
                    Begin = logEntries?.Count > 0 ? logEntries[0] : null,
                    End = logEntries?.Count > 0 ? logEntries[logEntries.Count - 1] : null
                },
                SearchText = IsSearchEmpty() ? string.Empty : txtSearch.Text.Trim(),
                CaseSensitive = ItemCaseSensitive.Checked,
                WholeWord = ItemWholeWords.Checked,
                IsMetadataSearchEnabled = IsMetadataSearchEnabled,
                Direction = direction,
                WrapAround = ItemWrapAround.Checked,
                LogRenderSettings = LogRenderSettings
            };
        }

        #endregion

        #region Control events

        private void ItemAll_Click(object sender, EventArgs e)
        {
            SelectedSearchMode = SearchMode.All;
            UpdateButtons();
            FireSearchSettingsChanged(force: true);
        }

        private void ItemNext_Click(object sender, EventArgs e)
        {
            SelectedSearchMode = SearchMode.Next;
            UpdateButtons();
            Search?.Invoke(BuildSearchSettings(SearchDirection.Forward));
            FireSearchSettingsChanged(force: false);
        }

        private void ItemPrevious_Click(object sender, EventArgs e)
        {
            SelectedSearchMode = SearchMode.Previous;
            UpdateButtons();
            Search?.Invoke(BuildSearchSettings(SearchDirection.Backward));
            FireSearchSettingsChanged(force: false);
        }

        private void SplitButton1_ButtonClick(object sender, EventArgs e)
        {
            switch (SelectedSearchMode)
            {
                case SearchMode.Next:
                    ItemNext_Click(sender, e);
                    break;
                case SearchMode.Previous:
                    ItemPrevious_Click(sender, e);
                    break;
                case SearchMode.All:
                    ItemAll_Click(sender, e);
                    break;
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ItemNext_Click(sender, e);
                e.Handled = true;
            }
            UpdateButtons();
        }

        private void ItemCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void ItemWholeWords_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == DefaultSearchText)
            {
                txtSearch.Text = string.Empty;
                txtSearch.ForeColor = SystemColors.ControlText;
            }
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = DefaultSearchText;
                txtSearch.ForeColor = Color.DarkGray;
            }
            UpdateButtons();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
            UpdateButtons();
        }

        #endregion

        #region Private helpers

        private bool IsSearchEmpty()
        {
            string search = txtSearch.Text.Trim();
            return string.IsNullOrEmpty(search) || search == DefaultSearchText;
        }

        private void UpdateButtons()
        {
            bool isSearchEmpty = IsSearchEmpty();
            BtnClear.Visible = !isSearchEmpty;
            txtSearch.Width = isSearchEmpty ? BtnClear.Right : BtnClear.Left;
            splitButton1.Enabled = !isSearchEmpty;
            splitButton1.ImageIndex = SelectedSearchMode switch
            {
                SearchMode.Next => 0,
                SearchMode.Previous => 1,
                SearchMode.All => 2,
                _ => 0
            };
        }

        /// <summary>
        /// Fires SearchSettingsChanged unless force is false and the settings are identical to the last fired settings.
        /// This prevents unnecessary result list refreshes on repeated Next/Previous with unchanged criteria.
        /// </summary>
        private void FireSearchSettingsChanged(bool force)
        {
            if (IsSearchEmpty()) return;

            SearchSettings current = BuildSearchSettings();

            if (!force && current.Equals(lastFiredSearchSettings)) return;

            lastFiredSearchSettings = current;
            SearchSettingsChanged?.Invoke(current);
        }

        #endregion
    }
}