using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LogScraper.Log;
using System.ComponentModel;

namespace LogScraper.Utilities.UserControls
{
    public partial class BookMarksControl : UserControl
    {
        public event EventHandler<LogEntry> NavigateToEntryRequested;
        public event EventHandler BookmarksChanged;

        private readonly SortedList<int, LogEntry> bookmarks = [];
        private LogEntry selectedLogEntry;

        public BookMarksControl()
        {
            InitializeComponent();
        }

        public IEnumerable<LogEntry> Bookmarks => bookmarks.Values;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogEntry SelectedLogEntry
        {
            get => selectedLogEntry;
            set
            {
                selectedLogEntry = value;
                UpdateButtons();
            }
        }

        public void Clear()
        {
            if (bookmarks.Count == 0) return;
            bookmarks.Clear();
            OnBookmarksChanged();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool hasBookmarks = bookmarks.Count > 0;
            int anchor = selectedLogEntry?.Index ?? -1;

            BtnBookMark.Enabled = selectedLogEntry != null;
            BtnBookMark.ImageIndex = selectedLogEntry == null ? 0 : Convert.ToInt32(bookmarks.ContainsKey(anchor));

            BtnPrevious.Enabled = hasBookmarks && anchor > bookmarks.Keys[0];
            BtnPrevious.ImageIndex = BtnPrevious.Enabled ? 1 : 0;
            BtnNext.Enabled = hasBookmarks && anchor < bookmarks.Keys[bookmarks.Count - 1];
            BtnNext.ImageIndex = BtnNext.Enabled ? 1 : 0;
            BtnReset.Enabled = hasBookmarks;
        }

        private void BtnBookMark_Click(object sender, EventArgs e)
        {
            if (selectedLogEntry == null) return;
            if (!bookmarks.Remove(selectedLogEntry.Index))
                bookmarks.Add(selectedLogEntry.Index, selectedLogEntry);
            OnBookmarksChanged();
            UpdateButtons();
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (bookmarks.Count == 0) return;
            int anchor = selectedLogEntry?.Index ?? int.MaxValue;
            for (int i = bookmarks.Count - 1; i >= 0; i--)
            {
                if (bookmarks.Keys[i] < anchor)
                {
                    NavigateToEntry(bookmarks.Values[i]);
                    return;
                }
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (bookmarks.Count == 0) return;
            int anchor = selectedLogEntry?.Index ?? int.MinValue;
            for (int i = 0; i < bookmarks.Count; i++)
            {
                if (bookmarks.Keys[i] > anchor)
                {
                    NavigateToEntry(bookmarks.Values[i]);
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