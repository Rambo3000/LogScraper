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
using LogScraper.LogPostProcessors;
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
        private LogEntry selectedLogEntry = null;

        public UserControlLogEntriesTextBox()
        {
            InitializeComponent();
            TxtLogEntries.Initialize();
            TxtLogEntries.UseDefaultFont(this);
            TxtLogEntries.HideUnusedMargins();
            UserControlPostProcessing.VisibleProcessorsChanged += UserControlPostProcessing_VisibleProcessorsChanged;
        }

        private void UserControlPostProcessing_VisibleProcessorsChanged(object sender, System.EventArgs e)
        {
            ShowLogEntries();
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
            contentLinesToStyle = LogEntryVisualIndexCalculator.GetVisualLineIndexesPerContentProperty(VisibleLogEntries, contentPropertiesWithCustomColoring, UserControlPostProcessing.VisibleProcessorKinds);
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
            List<LogPostProcessorKind> logPostProcessorKinds = UserControlPostProcessing.VisibleProcessorKinds;
            ShowRawLog(LogDataExporter.GetLogEntriesAsString(VisibleLogEntries, LogExportSettings, SelectedLogContentProperty, logFlowTree, logPostProcessorKinds));
            HighlightLines();
            contentLinesToStyle = LogEntryVisualIndexCalculator.GetVisualLineIndexesPerContentProperty(VisibleLogEntries, contentPropertiesWithCustomColoring, logPostProcessorKinds );
            StyleLines();

            try
            {
                if (selectedLogEntry != null)
                {
                    SelectLogEntry(selectedLogEntry);
                }
                else
                {
                    TxtLogEntries.FirstVisibleLine = firstVisibleLine;
                }
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
                if (LogEntryVisualIndexCalculator.TryGetVisualLineIndex(VisibleLogEntries, selectedLogEntry, UserControlPostProcessing.VisibleProcessorKinds, out int selectedIndex))
                {
                    HighlightLines(selectedIndex);
                }
                else
                {
                    HighlightLines(null);
                }
            }
        }
        private void HighlightLines(int? selectedIndex)
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

        public void SelectLogEntry(LogEntry entry)
        {
            selectedLogEntry = entry;
            if (selectedLogEntry == null) return;

            if (LogEntryVisualIndexCalculator.TryGetVisualLineIndex(VisibleLogEntries, selectedLogEntry, UserControlPostProcessing.VisibleProcessorKinds, out int selectedIndex))
            {
                TxtLogEntries.ScrollToLine(selectedIndex);
                HighlightLines(selectedIndex);
            }
            else
            {
                HighlightLines(null);
            }

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
            UpdatePnlViewModeSizeAndVisibility();
            ActiveControl = null;
        }

        private void ChkShowFlowTree_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateShowTreeControls(true);
            UpdatePnlViewModeSizeAndVisibility();
            ActiveControl = null;
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
            PnlViewMode.SuspendDrawing();
            PnlViewMode.Visible = CboLogContentType.Items.Count > 0;
            // Hide the dropdownbox if there's only one item
            PnlViewMode.Size = new Size((ChkShowFlowTree.Checked && CboLogContentType.Items.Count > 1 ? CboLogContentType.Width + 4 : 0) + UserControlPostProcessing.Right - ChkShowNoTree.Left + 4, PnlViewMode.Height);
            UpdatePnlViewModePosition();
            PnlViewMode.ResumeDrawing();
        }
        #endregion
    }
}
