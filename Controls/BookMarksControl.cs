using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Filtering;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Rendering;

namespace LogScraper.Controls
{
    public partial class BookMarksControl : UserControl
    {
        public event EventHandler BookmarksChanged;

        private readonly SortedList<int, LogEntry> _bookmarks = [];
        private LogEntry _selectedLogEntry;
        private List<LogEntry> _filteredBookmarks = [];

        private void RebuildFilteredBookmarks()
        {
            LogFilterResultWithRange filterResultWithRange = LogAppState.Instance.FilterResultWithRange.Value ?? null;
            if (filterResultWithRange?.FilteredAndRangedMask == null)
            {
                _filteredBookmarks = [.. _bookmarks.Values];
                return;
            }

            _filteredBookmarks = [];
            foreach (LogEntry logEntry in _bookmarks.Values)
            {
                if (filterResultWithRange.FilteredAndRangedMask[logEntry.Index])
                {
                    _filteredBookmarks.Add(logEntry);
                }
            }
        }


        public BookMarksControl()
        {
            InitializeComponent();
        }
        private void BookMarksControl_Load(object sender, EventArgs e)
        {
            LogAppState.Instance.ViewportSelectedLogEntry.Changed += (s, e) => UpdateSelectedLogEntry();
            LogAppState.Instance.FilterResultWithRange.Changed += OnFilterResultWithRange;
            LogAppState.Instance.ResetRequested += (s, e) => Reset();
        }

        private void OnFilterResultWithRange(object sender, EventArgs e)
        {
            RebuildFilteredBookmarks();
            UpdateButtons();
        }

        public IEnumerable<LogEntry> Bookmarks => _bookmarks.Values;

        public void UpdateSelectedLogEntry()
        {
            _selectedLogEntry = LogAppState.Instance.ViewportSelectedLogEntry.Value;
            UpdateButtons();
        }

        public void Reset()
        {
            if (_bookmarks.Count == 0) return;
            _bookmarks.Clear();
            RebuildFilteredBookmarks();
            OnBookmarksChanged();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool hasFiltered = _filteredBookmarks.Count > 0;
            int anchor = _selectedLogEntry?.Index ?? -1;

            BtnBookMark.Enabled = _selectedLogEntry != null;
            BtnBookMark.ImageIndex = _selectedLogEntry == null ? 0 : Convert.ToInt32(_bookmarks.ContainsKey(anchor));
            BtnPrevious.Enabled = hasFiltered && anchor > _filteredBookmarks[0].Index;
            BtnPrevious.ImageIndex = BtnPrevious.Enabled ? 1 : 0;
            BtnNext.Enabled = hasFiltered && anchor < _filteredBookmarks[^1].Index;
            BtnNext.ImageIndex = BtnNext.Enabled ? 1 : 0;
            BtnReset.Enabled = _bookmarks.Count > 0;
        }

        private void BtnBookMark_Click(object sender, EventArgs e)
        {
            if (_selectedLogEntry == null) return;
            if (!_bookmarks.Remove(_selectedLogEntry.Index))
                _bookmarks.Add(_selectedLogEntry.Index, _selectedLogEntry);
            RebuildFilteredBookmarks();
            OnBookmarksChanged();
            UpdateButtons();
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (_filteredBookmarks.Count == 0) return;
            int anchor = _selectedLogEntry?.Index ?? int.MaxValue;

            for (int i = _filteredBookmarks.Count - 1; i >= 0; i--)
            {
                if (_filteredBookmarks[i].Index < anchor)
                {
                    SetViewportSelectedLogEntry(_filteredBookmarks[i]);
                    return;
                }
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (_filteredBookmarks.Count == 0) return;
            int anchor = _selectedLogEntry?.Index ?? int.MinValue;

            for (int i = 0; i < _filteredBookmarks.Count; i++)
            {
                if (_filteredBookmarks[i].Index > anchor)
                {
                    SetViewportSelectedLogEntry(_filteredBookmarks[i]);
                    return;
                }
            }
        }

        private void BtnReset_Click(object sender, EventArgs e) => Reset();

        private static void SetViewportSelectedLogEntry(LogEntry entry)
        {
            LogAppState.Instance.ViewportSelectedLogEntry.Set(entry);
        }

        private void OnBookmarksChanged()
        {
            BookmarksChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}