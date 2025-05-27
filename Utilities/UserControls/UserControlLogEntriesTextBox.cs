using System.Collections.Generic;
using System.Windows.Forms;
using LogScraper.Export;
using LogScraper.Log;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.Extensions;
using static LogScraper.Utilities.Extensions.ScintillaControlExtensions;

namespace LogScraper.Utilities.UserControls
{
    public partial class UserControlLogEntriesTextBox : UserControl
    {
        private List<LogEntry> visibleLogEntries;
        private LogEntry logEntryBegin = null;
        private LogEntry logEntryEnd = null;
        private LogEntry logEntrySelected = null;
        private int? selectedIndex = -1;

        public UserControlLogEntriesTextBox()
        {
            InitializeComponent();
            TxtLogEntries.Initialize();
            TxtLogEntries.UseDefaultFont(this);
            TxtLogEntries.HideUnusedMargins();
        }

        public void UpdateLogMetadataFilterResult(LogMetadataFilterResult logMetadataFilterResultNew, LogExportSettings logExportSettings)
        {
            visibleLogEntries = LogDataExporter.GetLogEntriesActiveRange(logMetadataFilterResultNew, logExportSettings);
            ShowRawLog(LogDataExporter.GetLogEntriesAsString(visibleLogEntries, logExportSettings));
            HighlightLines();
        }

        private void HighlightLines()
        {
            if (visibleLogEntries != null && visibleLogEntries.Count > 0)
            {
                int? beginIndex = (logEntryBegin == null) ? null : 0;
                int? endIndex = (logEntryEnd == null) ? null : TxtLogEntries.Lines.Count - 2;

                TxtLogEntries.HighlightLines(beginIndex, endIndex, selectedIndex);
            }
        }
        public void Clear()
        {
            TxtLogEntries.Clear();
        }
        public void ApplyBeginFilter(LogEntry logEntryBeginNew)
        {
            logEntryBegin = logEntryBeginNew;
            HighlightLines();
            if (logEntryBeginNew != null) TxtLogEntries.ScrollToLine(0);
        }
        public void ApplyEndFilter(LogEntry logEntryEndNew)
        {
            logEntryEnd = logEntryEndNew;
            HighlightLines();
            if (logEntryEndNew != null) TxtLogEntries.ScrollToLine(TxtLogEntries.Lines.Count - 1);
        }
        public void SelectLogEntry(LogEntry selectedLogEntry)
        {

            if (selectedLogEntry == null)
            {
                logEntrySelected = null;
                selectedIndex = null;
                return;
            }
            logEntrySelected = selectedLogEntry;


            int selectedIndexNew = -1;
            bool found = false;
            foreach (LogEntry logEntry in visibleLogEntries)
            {
                selectedIndexNew++;
                if (logEntry == logEntrySelected)
                {
                    found = true;
                    break;
                }
                // Add the additional log entries to the line count
                if (logEntry.AdditionalLogEntries != null) selectedIndexNew += logEntry.AdditionalLogEntries.Count;
            }

            selectedIndex = found ? selectedIndexNew : null;

            HighlightLines();
            if (selectedIndex != null) TxtLogEntries.ScrollToLine((int)selectedIndex);
        }

        internal void ShowRawLog(string rawLog)
        {
            TxtLogEntries.ReadOnly = false;
            TxtLogEntries.Text = rawLog;
            TxtLogEntries.EmptyUndoBuffer();
            TxtLogEntries.ReadOnly = true;
        }

        internal bool TrySearch(string searchQuery, bool wholeWord, bool caseSensitive, bool wrapAround, SearchDirection searchDirection)
        {
            return TxtLogEntries.Find(searchQuery.Trim(), searchDirection, wholeWord, caseSensitive, wrapAround);
        }
    }
}
