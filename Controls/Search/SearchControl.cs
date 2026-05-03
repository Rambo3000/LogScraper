using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Rendering;
using LogScraper.Utilities;

namespace LogScraper.Controls.Search
{
    public partial class SearchControl : UserControl
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

        public SearchControl()
        {
            InitializeComponent();
            TxtSearch.PlaceholderText = DefaultSearchText;
            TxtSearch.TextChanged += TxtSearch_TextChanged;
            TxtSearch.Reset += TxtSearch_Reset;
            TxtSearch.KeyDown += TxtSearch_KeyDown;
            ItemAll.Click += ItemAll_Click;
            ItemCaseSensitive.Click += ItemCaseSensitive_CheckedChanged;
            ItemNext.Click += ItemNext_Click;
            ItemPrevious.Click += ItemPrevious_Click;
            ItemWholeWords.Click += ItemWholeWords_CheckedChanged;
            LogAppState.Instance.FilterResultWithRange.Changed += OnFilterResultWithRangeChanged;

            ShortcutManager.Register(this, AppShortcut.FocusSearch, FocusSearchInput);
            ShortcutManager.Register(this, AppShortcut.SearchNext, SearchNext);
            ShortcutManager.Register(this, AppShortcut.SearchPrevious, SearchPrevious);
            ShortcutManager.Register(this, AppShortcut.SearchAll, SearchAll);
        }



        private void OnFilterResultWithRangeChanged(object sender, EventArgs e)
        {
            logEntries = LogAppState.Instance.FilterResultWithRange.Value?.LogEntries;
            lastFiredSearchSettings = null;
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                SplitButton1_ButtonClick(this, EventArgs.Empty);
            }
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

        public void FocusSearchInput()
        {
            if (!Visible || !Enabled) return;
            TxtSearch.Focus();
        }

        public void SearchNext()
        {
            if (!Enabled) return;
            ItemNext_Click(this, EventArgs.Empty);
        }

        public void SearchPrevious()
        {
            if (!Enabled) return;
            ItemPrevious_Click(this, EventArgs.Empty);
        }

        public void SearchAll()
        {
            if (!Enabled) return;
            TxtSearch.Focus();
            ItemAll_Click(this, EventArgs.Empty);
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
                // Only if the metadata is shown, then allow searching in it.
                IsMetadataSearchEnabled = LogAppState.Instance.RenderOriginalMetadata.Value,
                Direction = direction,
                WrapAround = ItemWrapAround.Checked,
                LogRenderSettings = LogAppState.Instance.RenderSettings.Value
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