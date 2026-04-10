using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Metadata;
using LogScraper.Log.Rendering;

namespace LogScraper.Utilities.UserControls
{
    public partial class BookMarksControl : UserControl
    {
        public event EventHandler<LogEntry> NavigateToEntryRequested;
        public event EventHandler BookmarksChanged;

        private readonly SortedList<int, LogEntry> bookmarks = [];
        private LogEntry selectedLogEntry;
        private LogRange _logRange;
        private List<LogEntry> _filteredBookmarks = [];
        private LogMetadataFilterResult logMetadataFilterResult;

        private void RebuildFilteredBookmarks()
        {
            int rangeBegin = _logRange?.Begin?.Index ?? int.MinValue;
            int rangeEnd = _logRange?.End?.Index ?? int.MaxValue;

            List<LogEntry> bookMarksWithinRange = [.. bookmarks.Values.Where(entry => entry.Index >= rangeBegin && entry.Index <= rangeEnd)];

            if (logMetadataFilterResult?.FilteredLogEntriesMask == null)
            {
                _filteredBookmarks = bookMarksWithinRange;
                return;
            }

            _filteredBookmarks = [];
            foreach (LogEntry logEntry in bookMarksWithinRange)
            {
                if (logMetadataFilterResult.FilteredLogEntriesMask[logEntry.Index])
                { 
                    _filteredBookmarks.Add(logEntry);
                }
            }
        }

        public void UpdateMetadataFilterResult(LogMetadataFilterResult result)
        {
            logMetadataFilterResult = result;
            RebuildFilteredBookmarks();
        }

        public BookMarksControl()
        {
            InitializeComponent();
        }

        public IEnumerable<LogEntry> Bookmarks => bookmarks.Values;

        public void UpdateSelectedLogEntry(LogEntry logEntry)
        {
            selectedLogEntry = logEntry;
            UpdateButtons();
        }
        public void SetLogRange(LogRange logRange)
        {
            _logRange = logRange;
            RebuildFilteredBookmarks();
            UpdateButtons();
        }

        public void Clear()
        {
            if (bookmarks.Count == 0) return;
            bookmarks.Clear();
            logMetadataFilterResult = null;
            RebuildFilteredBookmarks();
            OnBookmarksChanged();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool hasFiltered = _filteredBookmarks.Count > 0;
            int anchor = selectedLogEntry?.Index ?? -1;

            BtnBookMark.Enabled = selectedLogEntry != null;
            BtnBookMark.ImageIndex = selectedLogEntry == null ? 0 : Convert.ToInt32(bookmarks.ContainsKey(anchor));
            BtnPrevious.Enabled = hasFiltered && anchor > _filteredBookmarks[0].Index;
            BtnPrevious.ImageIndex = BtnPrevious.Enabled ? 1 : 0;
            BtnNext.Enabled = hasFiltered && anchor < _filteredBookmarks[^1].Index;
            BtnNext.ImageIndex = BtnNext.Enabled ? 1 : 0;
            BtnReset.Enabled = bookmarks.Count > 0;
        }

        private void BtnBookMark_Click(object sender, EventArgs e)
        {
            if (selectedLogEntry == null) return;
            if (!bookmarks.Remove(selectedLogEntry.Index))
                bookmarks.Add(selectedLogEntry.Index, selectedLogEntry);
            RebuildFilteredBookmarks();
            OnBookmarksChanged();
            UpdateButtons();
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (_filteredBookmarks.Count == 0) return;
            int anchor = selectedLogEntry?.Index ?? int.MaxValue;

            for (int i = _filteredBookmarks.Count - 1; i >= 0; i--)
            {
                if (_filteredBookmarks[i].Index < anchor)
                {
                    NavigateToEntry(_filteredBookmarks[i]);
                    return;
                }
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (_filteredBookmarks.Count == 0) return;
            int anchor = selectedLogEntry?.Index ?? int.MinValue;

            for (int i = 0; i < _filteredBookmarks.Count; i++)
            {
                if (_filteredBookmarks[i].Index > anchor)
                {
                    NavigateToEntry(_filteredBookmarks[i]);
                    return;
                }
            }
        }

        private void BtnReset_Click(object sender, EventArgs e) => Clear();

        private void NavigateToEntry(LogEntry entry)
        {
            NavigateToEntryRequested?.Invoke(this, entry);
        }

        private void OnBookmarksChanged()
        {
            BookmarksChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}