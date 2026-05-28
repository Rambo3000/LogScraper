using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Controls.Generic;
using LogScraper.Log;
using LogScraper.Log.Content;
using LogScraper.Log.Filtering;
using LogScraper.Log.Layout;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Rendering;
using ScintillaNET;
using static LogScraper.Controls.Viewport.ScintillaControlExtensions;

namespace LogScraper.Controls.Viewport
{
    public partial class LogViewportControl : UserControl
    {
        #region Private objects and initialization

        private LogFilterResultWithRange _logFilterResultWithRange;
        private LogRenderSettings _logRenderSettings;

        /// <summary>
        /// When true the viewport skips re-renders because the last render was too slow.
        /// It resumes once processing finishes or the log size drops significantly.
        /// </summary>
        private bool _viewportUpdatesSuspended = false;
        private int _logEntryCountAtSuspension = 0;
        private static readonly TimeSpan RenderPauseThreshold = TimeSpan.FromMilliseconds(500);

        private List<LogContentProperty> _contentPropertiesWithCustomColoring;
        private LogEntry _selectedLogEntry = null;
        private LogEntry _logEntryAtCursor = null;
        private IEnumerable<LogEntry> _bookmarks = null;

        public LogViewportControl()
        {
            InitializeComponent();
            TxtLogEntries.Initialize();
            TxtLogEntries.UseDefaultFont(this);
            LogAppState.Instance.FilterResultWithRange.Changed += OnFilterResultWithRangeChanged;
            LogAppState.Instance.RenderSettings.Changed += OnRenderSettingsChanged;
            LogAppState.Instance.Layout.Changed += (s, e) => UpdateLogLayout(LogAppState.Instance.Layout.Value);
            LogAppState.Instance.ResetRequested += (s, e) => Reset();
            LogAppState.Instance.ResetRequested += (s, e) => ClearSuspension();
            LogAppState.Instance.ViewportSelectedLogEntry.Changed += (s, e) => ScrollToAndSelectLogEntry();
            LogAppState.Instance.Bookmarks.Changed += (s, e) => UpdateBookMarks();
            LogAppState.Instance.RawLogFallback.Changed += OnRawLogFallbackChanged;
            LogAppState.Instance.ProcessingState.Changed += OnProcessingStateChanged;
            LblPaused.Click += OnLblPausedClick;
        }

        private void OnLblPausedClick(object sender, EventArgs e)
        {
            ClearSuspension();
            // Do not call RenderLogEntries() here directly — a FilterResultWithRange.Changed
            // event is already queued from the active processing and will trigger the render.
        }

        private void ClearSuspension()
        {
            _viewportUpdatesSuspended = false;
            _logEntryCountAtSuspension = 0;
            LblPaused.Visible = false;
        }

        private void OnProcessingStateChanged(object sender, EventArgs e)
        {
            bool isActive = LogAppState.Instance.ProcessingState.Value?.IsActive ?? false;
            if (!isActive && _viewportUpdatesSuspended)
            {
                ClearSuspension();
                // Do one final render now that processing has finished
                _logFilterResultWithRange = LogAppState.Instance.FilterResultWithRange.Value;
                _logRenderSettings = LogAppState.Instance.RenderSettings.Value;
                RenderLogEntries();
            }
        }

        private void OnFilterResultWithRangeChanged(object sender, EventArgs e)
        {
            LblExplenation.Visible = !LogAppState.Instance.LogCollectionIsAvailable;
            LblExplenation2.Visible = !LogAppState.Instance.LogCollectionIsAvailable;

            LogFilterResultWithRange newFilterResultWithRange = LogAppState.Instance.FilterResultWithRange.Value;

            // Check if the visible entries have actually changed before rerendering
            // This avoids unnecessary rerenders when the LogCollection updates but the visible entries remain the same
            // (e.g., when masks grow with only 0 bits added)
            if (_logFilterResultWithRange != null && newFilterResultWithRange != null && _logFilterResultWithRange.HasSameVisibleEntries(newFilterResultWithRange))
            {
                // Update the reference but skip the rerender
                _logFilterResultWithRange = newFilterResultWithRange;
                return;
            }

            _logFilterResultWithRange = newFilterResultWithRange;

            // If suspended but the log size has dropped to less than half of what triggered the suspension, resume immediately
            if (_viewportUpdatesSuspended)
            {
                int currentCount = _logFilterResultWithRange?.LogEntries?.Count ?? 0;
                if (currentCount < _logEntryCountAtSuspension / 2)
                {
                    ClearSuspension();
                }
            }

            RenderLogEntries();
        }
        private void OnRenderSettingsChanged(object sender, EventArgs e)
        {
            _logRenderSettings = LogAppState.Instance.RenderSettings.Value;
            if (!_viewportUpdatesSuspended) RenderLogEntries();
        }

        private void OnRawLogFallbackChanged(object sender, EventArgs e)
        {
            string rawText = LogAppState.Instance.RawLogFallback.Value;
            if (rawText != null)
            {
                Text = rawText;
            }
        }

        #endregion

        #region Selected log entry
        private void ScrollToAndSelectLogEntry()
        {
            LogEntry newSelectedLogEntry = LogAppState.Instance.ViewportSelectedLogEntry.Value;
            if (newSelectedLogEntry == null || _selectedLogEntry == newSelectedLogEntry) return;

            if (LogEntryVisualIndexCalculator.TryGetVisualLineIndex(newSelectedLogEntry, _logEntriesRenderMapCache, out int selectedIndex))
            {
                TxtLogEntries.ScrollToLine(selectedIndex);
            }
        }
        #endregion 

        #region Update log layout and render log entries

        private void UpdateLogLayout(LogLayout logLayout)
        {
            // Determine content properties with custom coloring
            _contentPropertiesWithCustomColoring = [.. logLayout?.LogContentProperties.Where(item => item.IsCustomStyleEnabled) ?? []];
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
                Reset();
                return;
            }

            // Skip the render when suspended due to a previous slow render during continuous processing
            if (_viewportUpdatesSuspended) return;

            // Show loading indicator immediately
            ShowLoading();

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
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

                // Render all visible log entries into a single text representation
                Text = LogRenderer.RenderLogEntriesAsString(_logFilterResultWithRange, _logRenderSettings);

                // Build a render map that calculates the visual line index for each log entry based on the rendered text and active post-processors
                LogEntriesRenderMap postRenderLogEntriesRenderMap = LogEntryVisualIndexCalculator.BuildRenderMap(visibleLogEntries, _logRenderSettings.LogPostProcessorKinds, sourceLogCollection.LogEntries.Count);

                // Apply syntax highlighting based on the content properties with custom coloring, using the visual line indexes from the render map
                TxtLogEntries.StyleLines(_contentPropertiesWithCustomColoring, LogEntryVisualIndexCalculator.GetVisualLineIndexesPerContentProperty(visibleLogEntries, _contentPropertiesWithCustomColoring, postRenderLogEntriesRenderMap));

                // Try to restore the log entry at the carot after the render, if it is still visible
                // Set this before the scrollposition, so it doesnt interfere
                if (LogEntryVisualIndexCalculator.TryGetVisualLineIndex(logEntryAtCarot, _logEntriesRenderMapCache, out int selectedIndex))
                {
                    TxtLogEntries.GotoPosition(TxtLogEntries.Lines[selectedIndex].Position);
                    TxtLogEntries.ClearAndHighlightSingleLine(selectedIndex, INDICATOR_CAROT_LINE);
                }

                // Restore scroll position based on the previously visible log entry
                // This keeps the view stable when filters or visual spans change
                if (LogEntryVisualIndexCalculator.TryGetScrollToPosition(preRenderTopLogEntryRenderPosition, _logEntriesRenderMapCache, postRenderLogEntriesRenderMap, TxtLogEntries.LinesOnScreen, out int scrollToPosition))
                {
                    TxtLogEntries.FirstVisibleLine = scrollToPosition;
                }

                // Update caches for the next render cycle
                _logEntriesRenderMapCache = postRenderLogEntriesRenderMap;

                // Show bookmarks if used, after the logEntriesRenderMapCache is set
                ApplyBookmarks();

                //Raise the event that the visible range changed
                RaiseVisibleRangeChanged(true);

                // Resume drawing once the content and scroll position are fully restored
                this.ResumeDrawing();

                stopwatch.Stop();

                // Hide loading indicator
                HideLoading();

                if (stopwatch.Elapsed > RenderPauseThreshold && (LogAppState.Instance.ProcessingState.Value?.IsActive ?? false))
                {
                    _viewportUpdatesSuspended = true;
                    _logEntryCountAtSuspension = visibleLogEntries?.Count ?? 0;
                    LblPaused.Visible = true;
                    LblPaused.BringToFront();
                }
            }
            catch
            {
                // Make sure loading indicator is hidden on exception
                HideLoading();
                this.ResumeDrawing();
                throw;
            }
        }

        /// <summary>
        /// Returns the shared size and position for the feedback labels (loading + toast)
        /// so they always occupy exactly the same space.
        /// </summary>
        private (Size size, Point location) GetFeedbackLabelBounds()
        {
            using Graphics g = LblLoading.CreateGraphics();
            SizeF textSize = g.MeasureString("Bezig...", LblLoading.Font);
            int width = (int)Math.Ceiling(textSize.Width) + LblLoading.Padding.Horizontal + 10;
            const int height = 22;
            LblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            int scrollBarWidth = SystemInformation.VerticalScrollBarWidth;
            int x = Width - width - scrollBarWidth - 4;
            int y = 2;
            return (new Size(width, height), new Point(x, y));
        }

        /// <summary>
        /// Shows the loading indicator
        /// </summary>
        private void ShowLoading()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ShowLoading));
                return;
            }

            var (size, location) = GetFeedbackLabelBounds();
            LblLoading.Size = size;
            LblLoading.Location = location;
            LblLoading.Visible = true;
            LblLoading.BringToFront();

            // Force immediate UI update so the loading indicator is actually visible
            LblLoading.Update();
        }

        /// <summary>
        /// Hides the loading indicator
        /// </summary>
        private void HideLoading()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(HideLoading));
                return;
            }

            LblLoading.Visible = false;
        }

        /// <summary>
        /// Custom paint handler to draw a subtle border on feedback labels
        /// </summary>
        private void LblFeedback_Paint(object sender, PaintEventArgs e)
        {
            if (sender is not Label lbl) return;

            // Border is a slightly darker shade of each label's own background
            Color borderColor = lbl == LblLoading
                ? Color.FromArgb(200, 185, 120)   // dim amber to match yellow background
                : Color.FromArgb(150, 195, 150);  // dim sage to match green background

            using Pen pen = new(borderColor, 1);
            e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, lbl.Width - 1, lbl.Height - 1));
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
        public void Reset()
        {
            Text = string.Empty;
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
        private int lastTopVisibleLine = -1;

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
                LogAppState.Instance.ViewportSelectedLogEntry.Set(_logEntryAtCursor);
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
                        LogAppState.Instance.ViewportSelectedLogEntry.Set(_logEntryAtCursor);
                    }
                }
            }
        }

        private void RaiseVisibleRangeChanged(bool force = false)
        {
            int topVisibleLine = TxtLogEntries.FirstVisibleLine;

            if (!force && topVisibleLine == lastTopVisibleLine)
                return;

            lastTopVisibleLine = topVisibleLine;

            int bottomVisibleLine = Math.Min(topVisibleLine + TxtLogEntries.LinesOnScreen - 1, TxtLogEntries.Lines.Count - 1);

            if (!LogEntryVisualIndexCalculator.TryGetRenderPosition(topVisibleLine, _logEntriesRenderMapCache, out LogEntryRenderPosition topPosition)) return;

            if (!LogEntryVisualIndexCalculator.TryGetRenderPosition(bottomVisibleLine, _logEntriesRenderMapCache, out LogEntryRenderPosition bottomPosition)) return;

            LogAppState.Instance.ViewportVisibleRange.ForceSet(new LogRange() { Begin = topPosition.LogEntry, End = bottomPosition.LogEntry });
        }
        #endregion

        private void UpdateBookMarks()
        {
            _bookmarks = LogAppState.Instance.Bookmarks.Value?.Values;
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