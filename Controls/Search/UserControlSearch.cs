using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Rendering;

namespace LogScraper.Controls.Search
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
            TxtSearch.PlaceholderText = DefaultSearchText;
            TxtSearch.TextChanged += TxtSearch_TextChanged;
            TxtSearch.Reset += TxtSearch_Reset;
            ItemAll.Click += ItemAll_Click;
            ItemCaseSensitive.Click += ItemCaseSensitive_CheckedChanged;
            ItemNext.Click += ItemNext_Click;
            ItemPrevious.Click += ItemPrevious_Click;
            ItemWholeWords.Click += ItemWholeWords_CheckedChanged;
        }

        private void TxtSearch_Reset(object sender, EventArgs e)
        {
            Clear();
            UpdateButtons();
            Search?.Invoke(BuildSearchSettings(SearchDirection.Forward));
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
            TxtSearch.Clear();
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
                    End = logEntries?.Count > 0 ? logEntries[^1] : null
                },
                SearchText = TxtSearch.Text.Trim(),
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
            Search?.Invoke(BuildSearchSettings(SearchDirection.Forward));
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

        private void ItemCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
              TxtSearch.Focus();
        }

        private void ItemWholeWords_CheckedChanged(object sender, EventArgs e)
        {
            TxtSearch.Focus();
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        #endregion

        #region Private helpers

        private void UpdateButtons()
        {
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
            if (string.IsNullOrEmpty(TxtSearch.Text.Trim())) return;

            SearchSettings current = BuildSearchSettings();

            if (!force && current.Equals(lastFiredSearchSettings)) return;

            lastFiredSearchSettings = current;
            SearchSettingsChanged?.Invoke(current);
        }

        #endregion
    }
}