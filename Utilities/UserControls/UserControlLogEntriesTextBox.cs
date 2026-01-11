using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Export;
using LogScraper.Log;
using LogScraper.Log.Content;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Utilities.Extensions;
using static LogScraper.Utilities.Extensions.ScintillaControlExtensions;

namespace LogScraper.Utilities.UserControls
{
    public partial class UserControlLogEntriesTextBox : UserControl
    {
        #region Private objects and initialization

        private List<LogEntry> VisibleLogEntries;
        private LogMetadataFilterResult LogMetadataFilterResult;
        private LogExportSettings LogExportSettings;
        private LogEntry logEntryBegin = null;
        private LogEntry logEntryEnd = null;
        private List<LogContentProperty> contentPropertiesWithCustomColoring;
        private Dictionary<LogContentProperty, List<int>> contentLinesToStyle = null;
        private int? selectedIndex = -1;

        public UserControlLogEntriesTextBox()
        {
            InitializeComponent();
            TxtLogEntries.Initialize();
            TxtLogEntries.UseDefaultFont(this);
            TxtLogEntries.HideUnusedMargins();
        }
        #endregion

        #region Selected log entry
        private LogContentProperty SelectedLogContentProperty
        {
            get
            {
                if (CboLogContentType.SelectedItem == null) return null;
                return ((LogContentProperty)CboLogContentType.SelectedItem);
            }
        }
        #endregion 

        #region Update log layout and filter result
        public void UpdateLogLayout(LogLayout logLayout)
        {
            contentPropertiesWithCustomColoring = [.. logLayout.LogContentProperties.Where(item => item.IsCustomStyleEnabled)];
            contentLinesToStyle = DetermineContentLinesToStyle();
            CboLogContentType.Items.Clear();

            if (logLayout == null || logLayout.LogContentProperties == null || logLayout.LogContentProperties.Count == 0)
            {
                UpdatePnlViewModeSizeAndVisibility();
                return;
            }

            List<LogContentProperty> logContentProperties = [.. logLayout.LogContentProperties.Where(item => item.IsBeginFlowTreeFilter)];
            if (logContentProperties.Count > 0)
            {
                CboLogContentType.Items.AddRange([.. logContentProperties]);
                CboLogContentType.SelectedIndex = 0;
            }
            UpdatePnlViewModeSizeAndVisibility();
        }

        public void UpdateLogMetadataFilterResult(LogMetadataFilterResult logMetadataFilterResultNew, List<LogEntry> visibleLogEntries, LogExportSettings logExportSettings)
        {
            LogMetadataFilterResult = logMetadataFilterResultNew;
            LogExportSettings = logExportSettings;
            VisibleLogEntries = visibleLogEntries;
            contentLinesToStyle = DetermineContentLinesToStyle();
            ShowLogEntries();
        }
        private void ShowLogEntries()
        {
            this.SuspendDrawing();

            // Prevent the log from scrolling
            int firstVisibleLine = TxtLogEntries.FirstVisibleLine;

            if (VisibleLogEntries == null || LogExportSettings == null) return;

            List<LogFlowTreeNode> logFlowTree = null;
            if (ChkShowFlowTree.Checked && SelectedLogContentProperty != null)
            {
                logFlowTree = LogMetadataFilterResult.LogFlowTrees[SelectedLogContentProperty];
            }

            ShowRawLog(LogDataExporter.GetLogEntriesAsString(VisibleLogEntries, LogExportSettings, SelectedLogContentProperty, logFlowTree));
            HighlightLines();
            StyleLines();

            try
            {
                TxtLogEntries.FirstVisibleLine = firstVisibleLine;
            }
            catch
            {
                // Do nothing, do not interrupt the user
            }
            this.ResumeDrawing();
        }
        public void ShowRawLog(string rawLog)
        {
            TxtLogEntries.ReadOnly = false;
            TxtLogEntries.Text = rawLog;
            TxtLogEntries.EmptyUndoBuffer();
            TxtLogEntries.ReadOnly = true;
            UpdatePnlViewModePosition();
        }
        public void Clear()
        {
            TxtLogEntries.Clear();
        }
        #endregion

        #region Highlighting
        private void HighlightLines()
        {
            if (VisibleLogEntries != null && VisibleLogEntries.Count > 0)
            {
                int? beginIndex = (logEntryBegin == null) ? null : 0;
                int? endIndex = (logEntryEnd == null) ? null : TxtLogEntries.Lines.Count - 2;

                TxtLogEntries.HighlightLines(beginIndex, endIndex, selectedIndex);
            }
        }
        /// <summary>
        /// Applies custom styling to the currently visible log entries in the log display
        /// control.
        /// </summary>
        /// <remarks>This method updates the visual appearance of log lines based on their content and any
        /// custom coloring rules. It has no effect if there are no visible log entries.</remarks>
        private void StyleLines()
        {
            if (VisibleLogEntries != null && VisibleLogEntries.Count > 0)
            {
                TxtLogEntries.StyleLines(contentPropertiesWithCustomColoring, contentLinesToStyle);
            }
        }
        /// <summary>
        /// Identifies the line indexes of visible log entries that should be styled for each content property with
        /// custom coloring.
        /// </summary>
        /// <returns>An IndexDictionary mapping each content property with custom coloring to a list of line indexes in the
        /// visible log entries that should be styled. If there are no visible log entries, all lists will be empty.</returns>
        private Dictionary<LogContentProperty, List<int>> DetermineContentLinesToStyle()
        {
            Dictionary<LogContentProperty, List<int>> logEntriesToStylePerContentProperty = new(contentPropertiesWithCustomColoring.Count);

            if (VisibleLogEntries == null) return logEntriesToStylePerContentProperty;

            foreach (LogContentProperty LogContentProperty in contentPropertiesWithCustomColoring)
            {
                List<int> logEntriesIndexes = [];
                logEntriesToStylePerContentProperty[LogContentProperty] = logEntriesIndexes;

                for (int i = 0; i < VisibleLogEntries.Count; i++)
                {
                    LogEntry logEntry = VisibleLogEntries[i];
                    if (logEntry.LogContentProperties == null || logEntry.LogContentProperties.Count == 0) continue;

                    if (!logEntry.LogContentProperties.ContainsKey(LogContentProperty)) continue;

                    if (TryGetLogEntryIndex(logEntry, out int logEntryIndex))
                    {
                        logEntriesIndexes.Add(logEntryIndex);
                    }
                }
            }

            return logEntriesToStylePerContentProperty;
        }

        public void SelectLogEntry(LogEntry selectedLogEntry)
        {
            if (selectedLogEntry == null)
            {
                selectedIndex = null;
                return;
            }

            bool found = TryGetLogEntryIndex(selectedLogEntry, out int selectedIndexNew);

            selectedIndex = found ? selectedIndexNew : null;

            HighlightLines();
            if (selectedIndex != null) TxtLogEntries.ScrollToLine((int)selectedIndex);
        }

        /// <summary>
        /// Attempts to find the index of the specified <see cref="LogEntry"/> within the collection of visible log
        /// entries.
        /// </summary>
        /// <param name="logEntry">The <see cref="LogEntry"/> to locate in the collection.</param>
        /// <param name="logEntryIndex">When this method returns, contains the zero-based index of the specified <paramref name="logEntry"/> if it
        /// is found; otherwise, contains -1. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="logEntry"/> is found in the collection of visible
        /// log entries; otherwise, <see langword="false"/>.</returns>
        private bool TryGetLogEntryIndex(LogEntry logEntry, out int logEntryIndex)
        {
            logEntryIndex = -1;
            foreach (LogEntry logEntryVisible in VisibleLogEntries)
            {
                logEntryIndex++;
                if (logEntryVisible == logEntry) return true;

                // Add the additional log entries to the line count
                if (logEntryVisible.AdditionalLogEntries != null) logEntryIndex += logEntryVisible.AdditionalLogEntries.Count;
            }
            return false;
        }
        #endregion

        #region Begin and end filter
        public void ApplyBeginFilter(LogEntry logEntryBeginNew)
        {
            logEntryBegin = logEntryBeginNew;
            HighlightLines();
            StyleLines();
            if (logEntryBeginNew != null) TxtLogEntries.ScrollToLine(0);
        }
        public void ApplyEndFilter(LogEntry logEntryEndNew)
        {
            logEntryEnd = logEntryEndNew;
            HighlightLines();
            StyleLines();
            if (logEntryEndNew != null) TxtLogEntries.ScrollToLine(TxtLogEntries.Lines.Count - 1);
        }
        #endregion

        #region Search
        internal bool TrySearch(string searchQuery, bool wholeWord, bool caseSensitive, bool wrapAround, SearchDirection searchDirection)
        {
            return TxtLogEntries.Find(searchQuery.Trim(), searchDirection, wholeWord, caseSensitive, wrapAround);
        }
        #endregion

        #region User control events
        private void ChkShowNoTree_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateShowTreeControls(false);
        }

        private void ChkShowFlowTree_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateShowTreeControls(true);
        }

        private void CboLogContentType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ShowLogEntries();
        }
        private void TxtLogEntries_SizeChanged(object sender, System.EventArgs e)
        {
            UpdatePnlViewModePosition();
        }
        #endregion

        #region Update controls
        private bool updateShowTreeInProgress = false;
        private void UpdateShowTreeControls(bool showTree)
        {
            if (updateShowTreeInProgress) return;

            updateShowTreeInProgress = true;
            if (SelectedLogContentProperty == null)
            {
                ChkShowNoTree.Checked = false;
                ChkShowFlowTree.Checked = false;
                ChkShowNoTree.Enabled = false;
                ChkShowFlowTree.Enabled = false;
                CboLogContentType.Enabled = false;
                updateShowTreeInProgress = false;
                return;
            }

            //Also show no tree if previously no tree was available
            ChkShowFlowTree.Checked = showTree;
            ChkShowFlowTree.Enabled = !showTree;
            ChkShowNoTree.Checked = !showTree;
            ChkShowNoTree.Enabled = showTree;
            CboLogContentType.Enabled = showTree;

            ShowLogEntries();
            updateShowTreeInProgress = false;
        }
        private void UpdatePnlViewModePosition()
        {
            int scrollbarWidth = SystemInformation.VerticalScrollBarWidth;

            // Adjust this offset based on your button size/margin
            int offsetX = 4;

            // If the vertical scrollbar is visible, adjust for its width
            bool verticalScrollbarVisible = TxtLogEntries.ClientSize.Width < TxtLogEntries.Width;

            int right = TxtLogEntries.Right - (verticalScrollbarVisible ? scrollbarWidth : 0) - PnlViewMode.Width - offsetX;

            PnlViewMode.Location = new Point(right, TxtLogEntries.Top + offsetX);
        }
        private void UpdatePnlViewModeSizeAndVisibility()
        {
            PnlViewMode.Visible = CboLogContentType.Items.Count > 0;
            // Hide the dropdownbox if there's only one item
            PnlViewMode.Size = new Size((CboLogContentType.Items.Count == 1 ? 0 : CboLogContentType.Width + 4) + ChkShowFlowTree.Width + ChkShowNoTree.Width + 4, PnlViewMode.Height);
            UpdatePnlViewModePosition();
        }
        #endregion
    }
}
