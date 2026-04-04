using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Content;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.Log.Rendering;
using LogScraper.Utilities.Extensions;
using ScintillaNET;
using static LogScraper.Utilities.Extensions.ScintillaControlExtensions;

namespace LogScraper.Utilities.UserControls
{
    public partial class UserControlLogEntriesTextBox : UserControl
    {
        #region Private objects and initialization

        private List<LogEntry> VisibleLogEntries;
        private LogMetadataFilterResult LogMetadataFilterResult;
        private LogRenderSettings LogRenderSettings;
        private List<LogContentProperty> contentPropertiesWithCustomColoring;
        private LogEntry selectedLogEntry = null;
        private LogEntry logEntryAtCursor = null;
        private IEnumerable<LogEntry> bookmarks = null;

        public UserControlLogEntriesTextBox()
        {
            InitializeComponent();
            TxtLogEntries.Initialize();
            TxtLogEntries.UseDefaultFont(this);
        }

        #endregion

        #region Selected log entry
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogEntry SelectedLogEntry
        {
            get
            {
                return selectedLogEntry;
            }
            set
            {
                if (selectedLogEntry != value)
                {
                    if (value == null) return;

                    if (LogEntryVisualIndexCalculator.TryGetVisualLineIndex(value, logEntriesRenderMapCache, out int selectedIndex))
                    {
                        TxtLogEntries.ScrollToLine(selectedIndex);
                    }
                }
            }
        }
        #endregion 

        #region Update log layout and render log entries

        public void UpdateLogLayout(LogLayout logLayout)
        {
            // Determine content properties with custom coloring
            contentPropertiesWithCustomColoring = [.. logLayout.LogContentProperties.Where(item => item.IsCustomStyleEnabled)];
            // Update the styles in the text box based on the new layout
            TxtLogEntries.UpdateStyles(contentPropertiesWithCustomColoring);
        }

        public void UpdateLogMetadataFilterResult(LogMetadataFilterResult logMetadataFilterResultNew, List<LogEntry> visibleLogEntries, LogRenderSettings logRenderSettings)
        {
            LogEntry logEntryAtCarot = null;
            if (TxtLogEntries.SelectionStart > 0)
            {
                if (LogEntryVisualIndexCalculator.TryGetRenderPosition(TxtLogEntries.CurrentLine, logEntriesRenderMapCache, out LogEntryRenderPosition logEntryRenderPosition))
                {
                    logEntryAtCarot = logEntryRenderPosition.LogEntry;
                }
            }

            LogMetadataFilterResult = logMetadataFilterResultNew;
            LogRenderSettings = logRenderSettings;
            VisibleLogEntries = visibleLogEntries;
            RenderLogEntries();

            // Try to restore the log entry at the carot after the render, if it is still visible
            if (LogEntryVisualIndexCalculator.TryGetVisualLineIndex(logEntryAtCarot, logEntriesRenderMapCache, out int selectedIndex))
            {
                TxtLogEntries.ScrollToLine(selectedIndex);
                TxtLogEntries.ClearAndHighlightSingleLine(selectedIndex, INDICATOR_CAROT_LINE);
            }
        }
        /// <summary>
        /// Cache of the last rendered log layout, used to preserve scroll position across renders.
        /// </summary>
        LogEntriesRenderMap logEntriesRenderMapCache = null;

        /// <summary>
        /// Renders the visible log entries into the text box, applying the current
        /// render settings, flow tree visualization, and post-processing effects.
        /// This method also preserves the scroll position based on the previously visible log entry and offset.
        /// </summary>
        private void RenderLogEntries()
        {
            // Nothing to render if there is no data or render configuration
            if (VisibleLogEntries == null || LogRenderSettings == null) return;

            this.SuspendDrawing();

            // Try to get the log entry and offset currently at the top of the view
            // Do this before updateing the text to preserve scroll position
            LogEntryVisualIndexCalculator.TryGetRenderPosition(TxtLogEntries.FirstVisibleLine, logEntriesRenderMapCache, out LogEntryRenderPosition preRenderTopLogEntryRenderPosition);

            // Optional flow tree visualization based on the selected content property
            LogFlowTree logFlowTree = null;
            if (LogRenderSettings.LogFlowTreeRenderSettings != null && LogRenderSettings.LogFlowTreeRenderSettings.ShowTree && LogRenderSettings.LogFlowTreeRenderSettings.LogContentProperty != null)
            {
                logFlowTree = LogMetadataFilterResult.LogFlowTrees[LogRenderSettings.LogFlowTreeRenderSettings.LogContentProperty];
            }

            // Render all visible log entries into a single text representation
            Text = LogRenderer.RenderLogEntriesAsString(VisibleLogEntries, LogRenderSettings, LogRenderSettings.LogFlowTreeRenderSettings.LogContentProperty, logFlowTree, LogRenderSettings.LogPostProcessorKinds);

            // Build a render map that calculates the visual line index for each log entry based on the rendered text and active post-processors
            LogEntriesRenderMap postRenderLogEntriesRenderMap = LogEntryVisualIndexCalculator.BuildRenderMap(VisibleLogEntries, LogRenderSettings.LogPostProcessorKinds, LogMetadataFilterResult.SourceLogCollection.LogEntries.Count);

            // Apply syntax highlighting based on the content properties with custom coloring, using the visual line indexes from the render map
            TxtLogEntries.StyleLines(contentPropertiesWithCustomColoring, LogEntryVisualIndexCalculator.GetVisualLineIndexesPerContentProperty(VisibleLogEntries, contentPropertiesWithCustomColoring, postRenderLogEntriesRenderMap));

            if (preRenderTopLogEntryRenderPosition != null)
            {
                // Restore scroll position based on the previously visible log entry
                // This keeps the view stable when filters or visual spans change
                if (LogEntryVisualIndexCalculator.TryGetScrollToPosition(preRenderTopLogEntryRenderPosition, logEntriesRenderMapCache, postRenderLogEntriesRenderMap, TxtLogEntries.LinesOnScreen, out int scrollToPosition))
                {
                    TxtLogEntries.FirstVisibleLine = scrollToPosition;
                }
            }

            // Update caches for the next render cycle
            logEntriesRenderMapCache = postRenderLogEntriesRenderMap;

            // Show bookmarks if used, after the logEntriesRenderMapCache is set
            ApplyBookmarks();

            //Raise the event that the visible range changed
            RaiseVisibleRangeChanged(true);

            // Resume drawing once the content and scroll position are fully restored
            this.ResumeDrawing();
        }


        /// <summary>
        /// Updates the text displayed in the log entries text box.
        /// </summary>
        public override string Text
        {
            get { return TxtLogEntries.Text; }
            set
            {
                TxtLogEntries.ReadOnly = false;
                TxtLogEntries.Text = value;
                TxtLogEntries.EmptyUndoBuffer();
                TxtLogEntries.ReadOnly = true;

                // Invalidate caches as the content has changed
                if (string.IsNullOrEmpty(value))
                {
                    logEntriesRenderMapCache = null;
                }
            }
        }
        public void Clear()
        {
            TxtLogEntries.Clear();
            LogEntryAtCursorChanged?.Invoke(this, null);
        }
        /// <summary>
        /// Event that is triggered when the text in the log entries text box changes.
        /// </summary>

        public event EventHandler LogEntriesTextChanged
        {
            add
            {
                TxtLogEntries.TextChanged += value;
            }
            remove
            {
                TxtLogEntries.TextChanged -= value;
            }
        }
        #endregion

        #region Search
        internal bool TrySearch(string searchQuery, bool wholeWord, bool caseSensitive, bool wrapAround, SearchDirection searchDirection)
        {
            return TxtLogEntries.Find(searchQuery.Trim(), searchDirection, wholeWord, caseSensitive, wrapAround);
        }
        #endregion

        #region Scroll and log entry at cursor handling
        public class VisibleRangeChangedEventArgs(LogEntryRenderPosition topPosition, LogEntryRenderPosition bottomPosition) : EventArgs
        {
            public LogEntryRenderPosition TopPosition { get; } = topPosition;
            public LogEntryRenderPosition BottomPosition { get; } = bottomPosition;
        }
        private int lastTopVisibleLine = -1;

        public event EventHandler<VisibleRangeChangedEventArgs> VisibleRangeChanged;

        public event EventHandler<LogEntry> LogEntryAtCursorChanged;

        private int? lastKnownLine = null;

        private void TrackedScintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            RaiseVisibleRangeChanged();

            int currentLine = TxtLogEntries.CurrentLine;
            bool isCarotInDefaultPosition = TxtLogEntries.SelectionStart == 0;
            bool currentLineChanged = lastKnownLine.HasValue && currentLine != lastKnownLine;

            lastKnownLine = currentLine;

            if (currentLineChanged) TxtLogEntries.ClearAndHighlightSingleLine(currentLine, INDICATOR_CAROT_LINE);

            // Make sure to reset if the carot is returned to 0
            if (currentLineChanged && isCarotInDefaultPosition)
            {
                logEntryAtCursor = null;
                lastKnownLine = null;
                selectedLogEntry = logEntryAtCursor;
                LogEntryAtCursorChanged?.Invoke(this, logEntryAtCursor);
                return;
            }

            if (currentLineChanged || !isCarotInDefaultPosition)
            {
                if (LogEntryVisualIndexCalculator.TryGetRenderPosition(TxtLogEntries.CurrentLine, logEntriesRenderMapCache, out LogEntryRenderPosition cursorPosition))
                {
                    LogEntry logEntryAtCursorNew = cursorPosition.LogEntry;
                    if (logEntryAtCursorNew != logEntryAtCursor)
                    {
                        logEntryAtCursor = logEntryAtCursorNew;
                        selectedLogEntry = logEntryAtCursor;
                        LogEntryAtCursorChanged?.Invoke(this, logEntryAtCursor);
                    }
                }
            }
        }

        private void RaiseVisibleRangeChanged(bool force = false)
        {
            if (VisibleRangeChanged == null) return;

            int topVisibleLine = TxtLogEntries.FirstVisibleLine;

            if (!force && topVisibleLine == lastTopVisibleLine)
                return;

            lastTopVisibleLine = topVisibleLine;

            int bottomVisibleLine = Math.Min(topVisibleLine + TxtLogEntries.LinesOnScreen - 1, TxtLogEntries.Lines.Count - 1);

            if (!LogEntryVisualIndexCalculator.TryGetRenderPosition(topVisibleLine, logEntriesRenderMapCache, out LogEntryRenderPosition topPosition)) return;

            if (!LogEntryVisualIndexCalculator.TryGetRenderPosition(bottomVisibleLine, logEntriesRenderMapCache, out LogEntryRenderPosition bottomPosition)) return;

            VisibleRangeChanged?.Invoke(this, new VisibleRangeChangedEventArgs(topPosition, bottomPosition));
        }
        #endregion

        public void UpdateBookMarks(IEnumerable<LogEntry> logEntries)
        {
            bookmarks = logEntries;
            ApplyBookmarks();
        }

        private void ApplyBookmarks()
        {
            TxtLogEntries.ApplyLineIndicators(
                INDICATOR_BOOKMARK,
                MARKER_BOOKMARK,
                bookmarks == null ? [] : bookmarks
                    .Select(logEntry => LogEntryVisualIndexCalculator.TryGetVisualLineIndex(logEntry, logEntriesRenderMapCache, out int visualIndex) ? (int?)visualIndex : null)
                    .Where(index => index.HasValue)
                    .Select(index => index.Value)
            );
        }
    }
}