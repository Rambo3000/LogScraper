using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Controls.Generic;
using LogScraper.Log;
using LogScraper.Log.Content;
using LogScraper.Log.Filtering;
using LogScraper.Log.FlowTree;
using LogScraper.Log.Layout;
using LogScraper.Log.Rendering;
using ScintillaNET;
using static System.Net.Mime.MediaTypeNames;
using static LogScraper.Controls.LogEntriesTextbox.ScintillaControlExtensions;

namespace LogScraper.Controls.LogEntriesTextbox
{
    public partial class UserControlLogEntriesTextBox : UserControl
    {
        #region Private objects and initialization

        private LogFilterResultWithRange _logFilterResultWithRange;
        private LogRenderSettings _logRenderSettings;

        private List<LogContentProperty> _contentPropertiesWithCustomColoring;
        private LogEntry _selectedLogEntry = null;
        private LogEntry _logEntryAtCursor = null;
        private IEnumerable<LogEntry> _bookmarks = null;

        public UserControlLogEntriesTextBox()
        {
            InitializeComponent();
            TxtLogEntries.Initialize();
            TxtLogEntries.UseDefaultFont(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;
            LogAppState.Instance.FilterResultWithRangeChanged += OnFilterResultWithRangeChanged;
        }

        private void OnFilterResultWithRangeChanged(object sender, EventArgs e)
        {
            _logFilterResultWithRange = LogAppState.Instance.FilterResultWithRange;
            _logRenderSettings = LogAppState.Instance.RenderSettings;
            RenderLogEntries();
        }

        #endregion

        #region Selected log entry
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogEntry SelectedLogEntry
        {
            get
            {
                return _selectedLogEntry;
            }
            set
            {
                if (_selectedLogEntry != value)
                {
                    if (value == null) return;

                    if (LogEntryVisualIndexCalculator.TryGetVisualLineIndex(value, _logEntriesRenderMapCache, out int selectedIndex))
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
            _contentPropertiesWithCustomColoring = [.. logLayout.LogContentProperties.Where(item => item.IsCustomStyleEnabled)];
            // Update the styles in the text box based on the new layout
            TxtLogEntries.UpdateStyles(_contentPropertiesWithCustomColoring);
        }

        /// <summary>
        /// Cache of the last rendered log layout, used to preserve scroll position across renders.
        /// </summary>
        private LogEntriesRenderMap _logEntriesRenderMapCache = null;

        /// <summary>
        /// Renders the visible log entries into the text box, applying the current
        /// render settings, flow tree visualization, and post-processing effects.
        /// This method also preserves the scroll position based on the previously visible log entry and offset.
        /// </summary>
        private void RenderLogEntries()
        {
            // Nothing to render if there is no data or render configuration
            if (_logFilterResultWithRange == null || _logFilterResultWithRange.MetadataFilterResult == null || _logRenderSettings == null)
            {
                Clear();
                return;
            }

            LogEntry logEntryAtCarot = null;
            if (TxtLogEntries.SelectionStart > 0)
            {
                if (LogEntryVisualIndexCalculator.TryGetRenderPosition(TxtLogEntries.CurrentLine, _logEntriesRenderMapCache, out LogEntryRenderPosition logEntryRenderPosition))
                {
                    logEntryAtCarot = logEntryRenderPosition.LogEntry;
                }
            }

            this.SuspendDrawing();
            List<LogEntry> visibleLogEntries = _logFilterResultWithRange.LogEntries;
            LogCollection sourceLogCollection = _logFilterResultWithRange.MetadataFilterResult.SourceLogCollection;

            // Try to get the log entry and offset currently at the top of the view
            // Do this before updateing the text to preserve scroll position
            LogEntryVisualIndexCalculator.TryGetRenderPosition(TxtLogEntries.FirstVisibleLine, _logEntriesRenderMapCache, out LogEntryRenderPosition preRenderTopLogEntryRenderPosition);

            // Optional flow tree visualization based on the selected content property
            LogFlowTree logFlowTree = null;
            if (_logRenderSettings.LogFlowTreeRenderSettings != null && _logRenderSettings.LogFlowTreeRenderSettings.ShowTree && _logRenderSettings.LogFlowTreeRenderSettings.LogContentProperty != null)
            {
                logFlowTree = _logFilterResultWithRange.MetadataFilterResult.LogFlowTrees[_logRenderSettings.LogFlowTreeRenderSettings.LogContentProperty];
            }

            // Render all visible log entries into a single text representation
            Text = LogRenderer.RenderLogEntriesAsString(visibleLogEntries, _logRenderSettings, _logRenderSettings.LogFlowTreeRenderSettings.LogContentProperty, logFlowTree, _logRenderSettings.LogPostProcessorKinds);

            // Build a render map that calculates the visual line index for each log entry based on the rendered text and active post-processors
            LogEntriesRenderMap postRenderLogEntriesRenderMap = LogEntryVisualIndexCalculator.BuildRenderMap(visibleLogEntries, _logRenderSettings.LogPostProcessorKinds, sourceLogCollection.LogEntries.Count);

            // Apply syntax highlighting based on the content properties with custom coloring, using the visual line indexes from the render map
            TxtLogEntries.StyleLines(_contentPropertiesWithCustomColoring, LogEntryVisualIndexCalculator.GetVisualLineIndexesPerContentProperty(visibleLogEntries, _contentPropertiesWithCustomColoring, postRenderLogEntriesRenderMap));

            if (preRenderTopLogEntryRenderPosition != null)
            {
                // Restore scroll position based on the previously visible log entry
                // This keeps the view stable when filters or visual spans change
                if (LogEntryVisualIndexCalculator.TryGetScrollToPosition(preRenderTopLogEntryRenderPosition, _logEntriesRenderMapCache, postRenderLogEntriesRenderMap, TxtLogEntries.LinesOnScreen, out int scrollToPosition))
                {
                    TxtLogEntries.FirstVisibleLine = scrollToPosition;
                }
            }

            // Update caches for the next render cycle
            _logEntriesRenderMapCache = postRenderLogEntriesRenderMap;

            // Show bookmarks if used, after the logEntriesRenderMapCache is set
            ApplyBookmarks();

            //Raise the event that the visible range changed
            RaiseVisibleRangeChanged(true);

            // Try to restore the log entry at the carot after the render, if it is still visible
            if (LogEntryVisualIndexCalculator.TryGetVisualLineIndex(logEntryAtCarot, _logEntriesRenderMapCache, out int selectedIndex))
            {
                TxtLogEntries.ScrollToLine(selectedIndex);
                TxtLogEntries.ClearAndHighlightSingleLine(selectedIndex, INDICATOR_CAROT_LINE);
            }

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
                    _logEntriesRenderMapCache = null;
                }
            }
        }
        public void Clear()
        {
            Text = string.Empty;
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
                _logEntryAtCursor = null;
                lastKnownLine = null;
                _selectedLogEntry = _logEntryAtCursor;
                LogEntryAtCursorChanged?.Invoke(this, _logEntryAtCursor);
                return;
            }

            if (currentLineChanged || !isCarotInDefaultPosition)
            {
                if (LogEntryVisualIndexCalculator.TryGetRenderPosition(TxtLogEntries.CurrentLine, _logEntriesRenderMapCache, out LogEntryRenderPosition cursorPosition))
                {
                    LogEntry logEntryAtCursorNew = cursorPosition.LogEntry;
                    if (logEntryAtCursorNew != _logEntryAtCursor)
                    {
                        _logEntryAtCursor = logEntryAtCursorNew;
                        _selectedLogEntry = _logEntryAtCursor;
                        LogEntryAtCursorChanged?.Invoke(this, _logEntryAtCursor);
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

            if (!LogEntryVisualIndexCalculator.TryGetRenderPosition(topVisibleLine, _logEntriesRenderMapCache, out LogEntryRenderPosition topPosition)) return;

            if (!LogEntryVisualIndexCalculator.TryGetRenderPosition(bottomVisibleLine, _logEntriesRenderMapCache, out LogEntryRenderPosition bottomPosition)) return;

            VisibleRangeChanged?.Invoke(this, new VisibleRangeChangedEventArgs(topPosition, bottomPosition));
        }
        #endregion

        public void UpdateBookMarks(IEnumerable<LogEntry> logEntries)
        {
            _bookmarks = logEntries;
            ApplyBookmarks();
        }

        private void ApplyBookmarks()
        {
            IEnumerable<int> bookmarksLineToApply = _bookmarks == null ? [] : _bookmarks
                    .Select(logEntry => LogEntryVisualIndexCalculator.TryGetVisualLineIndex(logEntry, _logEntriesRenderMapCache, out int visualIndex) ? (int?)visualIndex : null)
                    .Where(index => index.HasValue)
                    .Select(index => index.Value);

            TxtLogEntries.SetBookMargeMarginVisibily(bookmarksLineToApply.Any());
            TxtLogEntries.ApplyLineIndicators(INDICATOR_BOOKMARK, MARKER_BOOKMARK, bookmarksLineToApply);
        }
    }
}