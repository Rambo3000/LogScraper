using System.Collections.Generic;
using System.Windows.Forms;
using LogScraper.Export;
using LogScraper.Log;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.Extensions;
using static LogScraper.Utilities.Extensions.ScintillaRichTextBoxExtensions;

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
            TxtLogEntries.InitLineHighlighting();
            TxtLogEntries.UseDefaultFont(this);
            TxtLogEntries.HideUnusedMargins();
        }

        public void UpdateLogMetadataFilterResult(LogMetadataFilterResult logMetadataFilterResultNew, LogExportSettings logExportSettings)
        {
            visibleLogEntries = LogDataExporter.GetLogEntriesActiveRange(logMetadataFilterResultNew, logExportSettings);
            TxtLogEntries.ReadOnly = false;
            TxtLogEntries.Text = LogDataExporter.GetLogEntriesAsString(visibleLogEntries, logExportSettings);
            TxtLogEntries.ReadOnly = true;
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
            if (logEntryEndNew!=null) TxtLogEntries.ScrollToLine(TxtLogEntries.Lines.Count - 1);
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
            TxtLogEntries.ReadOnly = true;
        }

        internal bool TrySearch(string searchQuery, bool wholeWord, bool caseSensitive, bool wrapAround, SearchDirection searchDirection)
        {
            //int scrollPosition = TxtLogEntries.GetCharIndexFromPosition(new Point(0, 0));
            //int selectionStart = TxtLogEntries.SelectionStart;
            //int selectionLenght = TxtLogEntries.SelectionLength;

            //TxtLogEntries.SuspendDrawing();

            //if (!ChkShowAllLogEntries.Checked)
            //{
            //    ChkShowAllLogEntries.Checked = true;
            //    Application.DoEvents();
            //}

            ////Clean the logentry background and reinster begin and end filters
            //TxtLogEntries.ClearHighlighting();
            //HighlightLines();

            ////Return the scrolling to its original position
            //TxtLogEntries.Select(scrollPosition, 0);
            //TxtLogEntries.ScrollToCaret();
            //TxtLogEntries.Select(selectionStart, selectionLenght);

            bool found = TxtLogEntries.Find(searchQuery.Trim(), searchDirection, wholeWord, caseSensitive, wrapAround);
            //txtLogETxtLogEntriesntries.ResumeDrawing();
            return found;
        }
    }
}
