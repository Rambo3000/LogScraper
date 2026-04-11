using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Metadata;
using LogScraper.Log.Rendering;

namespace LogScraper.Controls
{
    /// <summary>
    /// A timeline visualization control that displays log entries as a histogram with error markers
    /// and a visible range indicator showing the current scroll position.
    /// When a log range is constrained, zoom in/out buttons appear in a reserved right-side strip.
    /// </summary>
    public partial class LogTimeLineControl : UserControl
    {
        #region Fields

        // Log entry collections
        private List<LogEntry> filteredLogEntries = [];
        private BitArray filteredLogEntriesMask = null;
        private List<LogEntry> filteredLogEntriesInRange = [];
        private List<LogEntry> displayedLogEntries = [];

        // Cached error entries
        private List<LogEntry> errorLogEntriesInRange = [];
        private List<LogEntry> errorLogEntriesOutOfRange = [];

        // Cached bookmarks
        private List<LogEntry> allBookmarkLogEntries = [];
        private List<LogEntry> bookmarkLogEntriesInRange = [];
        private List<LogEntry> bookmarkLogEntriesOutOfRange = [];
        private List<LogEntry> bookmarkLogEntriesNotAvailable = [];

        // Full collection span (for full-timeline mode and out-of-range markers)
        private DateTime fullSpanMinimum;
        private DateTime fullSpanMaximum;

        // Log range
        private LogRange _logRange;

        // Zoom state: when true the histogram spans the full collection; when false it clips to the range
        private bool _showFullTimeline = true;

        // Bucketing
        private readonly Dictionary<DateTime, List<LogEntry>> buckets = [];
        private List<DateTime> sortedBucketKeys = [];
        private TimeSpan currentBucketSize;
        private DateTime minimumTimestamp;
        private DateTime maximumTimestamp;
        private double maximumRawValue;
        private float totalBarWidth;

        // Hover tracking
        private int hoveredBucketIndex = -1;
        private int hoveredErrorIndex = -1;
        private int hoveredBookmarkIndex = -1;
        private ZoomButton hoveredZoomButton = ZoomButton.None;

        // Time range currently represented on screen used for hover hit testing and out-of-range marker placement
        private DateTime? onScreenRangeStart = null;
        private DateTime? onScreenRangeEnd = null;

        // Zoom button rects (in control coordinates)
        private Rectangle zoomInButtonRect = Rectangle.Empty;
        private Rectangle zoomOutButtonRect = Rectangle.Empty;

        // Constants
        private const double SCALE_POWER = 0.4;
        private const int MINIMUM_BUCKET_COUNT = 256;
        private const int MARKER_SIZE = 8;
        private const int MINIMUM_VISIBLE_RANGE_WIDTH = 1;
        private const int BOOKMARK_MARKER_Y_OFFSET = MARKER_SIZE * 2 + 1;
        private const int ERROR_MARKER_Y_OFFSET = MARKER_SIZE + 2;
        private const int ZOOM_BUTTON_SIZE = 18;
        private const int ZOOM_BUTTON_MARGIN = 2;
        private const int ZOOM_BUTTON_SPACING = 3;
        private const int ZOOM_STRIP_WIDTH = ZOOM_BUTTON_SIZE + ZOOM_BUTTON_MARGIN * 2;

        // Colors
        private static readonly Color BAR_COLOR = Color.LightGray;
        private static readonly Color BAR_HOVER_COLOR = Color.FromArgb(100, 160, 210);
        private static readonly Color VISIBLE_RANGE_COLOR = Color.FromArgb(80, 100, 160, 210);
        private static readonly Color VISIBLE_RANGE_BORDER_COLOR = Color.FromArgb(150, 100, 160, 210);
        private static readonly Color ERROR_MARKER_COLOR = Color.FromArgb(220, 50, 50);
        private static readonly Color ERROR_MARKER_HOVER_COLOR = Color.FromArgb(255, 80, 80);
        private static readonly Color ERROR_MARKER_OUT_OF_RANGE_COLOR = Color.FromArgb(100, 160, 40, 40);
        private static readonly Color BOOKMARK_MARKER_COLOR = Color.SteelBlue;
        private static readonly Color BOOKMARK_MARKER_HOVER_COLOR = Color.FromArgb(255, 120, 180, 230);
        private static readonly Color BOOKMARK_MARKER_OUT_OF_RANGE_COLOR = Color.FromArgb(100, 70, 130, 180);
        private static readonly Color ZOOM_STRIP_BACKGROUND_COLOR = Color.FromArgb(245, 247, 250);
        private static readonly Color ZOOM_BUTTON_BACKGROUND_COLOR = Color.FromArgb(225, 235, 245);
        private static readonly Color ZOOM_BUTTON_HOVER_BACKGROUND_COLOR = Color.FromArgb(190, 215, 240);
        private static readonly Color ZOOM_BUTTON_BORDER_COLOR = Color.FromArgb(155, 185, 215);
        private static readonly Color ZOOM_BUTTON_SYMBOL_COLOR = Color.FromArgb(55, 95, 145);
        private static readonly Color ZOOM_BUTTON_INACTIVE_BACKGROUND_COLOR = Color.FromArgb(235, 237, 240);
        private static readonly Color ZOOM_BUTTON_INACTIVE_SYMBOL_COLOR = Color.FromArgb(180, 190, 200);
        private static readonly Color ZOOM_BUTTON_INACTIVE_BORDER_COLOR = Color.FromArgb(210, 215, 220);

        // Tooltip
        private readonly ToolTip toolTip = new();

        private enum ZoomButton { None, ZoomIn, ZoomOut }

        #endregion

        #region Events

        /// <summary>Raised when a histogram bar is clicked.</summary>
        public event EventHandler<LogEntry> CellClicked;

        /// <summary>Raised when an error marker is clicked.</summary>
        public event EventHandler<LogEntry> ErrorMarkerClicked;

        /// <summary>Raised when a bookmark marker is clicked.</summary>
        public event EventHandler<LogEntry> BookmarkMarkerClicked;

        #endregion

        #region Constructor

        public LogTimeLineControl()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Cursor = Cursors.Default;

            this.Paint += OnPaint;
            this.MouseClick += OnMouseClick;
            this.MouseMove += OnMouseMove;
            this.MouseLeave += OnMouseLeave;
            this.Resize += (s, e) => { RebuildBuckets(); this.Invalidate(); };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the log entries displayed in the timeline.
        /// Supports incremental updates if new entries are appended to existing data.
        /// The full collection is used to determine the full-span time range.
        /// </summary>
        public void UpdateLogEntries(LogMetadataFilterResult filterResult, List<LogEntry> visibleEntries, LogRange range, LogCollection fullCollection)
        {
            if (fullCollection?.LogEntries?.Count == 0)
            {
                filteredLogEntries.Clear();
                displayedLogEntries.Clear();
                buckets.Clear();
                sortedBucketKeys.Clear();
                errorLogEntriesInRange.Clear();
                filteredLogEntriesMask = null;
                this.Invalidate();
                return;
            }

            fullSpanMinimum = fullCollection.LogEntries[0].TimeStamp;
            fullSpanMaximum = fullCollection.LogEntries[^1].TimeStamp;
            filteredLogEntriesMask = filterResult.FilteredLogEntriesMask;

            filteredLogEntries = filterResult?.LogEntries == null ? [] : [.. filterResult.LogEntries];
            filteredLogEntriesInRange = (visibleEntries == null) ? [] : [.. visibleEntries];
            _logRange = range;

            RebuildRangeRelatedObjects();

            this.Invalidate();
        }

        /// <summary>Updates the bookmark entries displayed in the timeline.</summary>
        public void SetBookmarks(List<LogEntry> bookmarks)
        {
            allBookmarkLogEntries = bookmarks ?? [];
            RebuildFilteredBookmarkMarkers();
            this.Invalidate();
        }

        #endregion

        #region Entry Filtering
        private void RebuildRangeRelatedObjects()
        {
            SetDisplayedEntries();
            RebuildErrorLogEntries();
            RebuildFilteredBookmarkMarkers();
            RebuildBuckets();
        }

        /// <summary>Sets the visible range of log entries currently shown in the log viewer.</summary>
        public void SetOnScreenDateTimeRange(DateTime startTimestamp, DateTime endTimestamp)
        {
            onScreenRangeStart = startTimestamp;
            onScreenRangeEnd = endTimestamp;
            this.Invalidate();
        }

        /// <summary>
        /// Rebuilds the displayed entry set based on the current range and zoom state.
        /// Full-timeline mode shows all entries; zoomed-in mode clips to the range.
        /// </summary>
        private void SetDisplayedEntries()
        {
            if (_showFullTimeline)
            {
                displayedLogEntries = filteredLogEntries;
            }
            else
            {
                displayedLogEntries = filteredLogEntriesInRange;
            }
        }
        private void RebuildErrorLogEntries()
        {
            errorLogEntriesInRange = displayedLogEntries != null ? [.. filteredLogEntriesInRange.Where(entry => entry.IsErrorLogEntry)] : [];
            errorLogEntriesOutOfRange = [];

            // Draw out-of-range error markers first (faded, behind in-range ones)
            if (_logRange?.IsConstrained == true && _showFullTimeline)
            {
                HashSet<int> inRangeIndices = [.. errorLogEntriesInRange.Select(entry => entry.Index)];

                foreach (LogEntry errorEntry in filteredLogEntries.Where(entry => entry.IsErrorLogEntry))
                {
                    if (inRangeIndices.Contains(errorEntry.Index))
                        continue;

                    errorLogEntriesOutOfRange.Add(errorEntry);
                }
            }
        }
        private void RebuildFilteredBookmarkMarkers()
        {

            List<LogEntry> bookMarkAvailable = [];
            bookmarkLogEntriesNotAvailable = [];

            foreach (LogEntry bookmarkEntry in allBookmarkLogEntries)
            {
                if (filteredLogEntriesMask[bookmarkEntry.Index])
                {
                    bookMarkAvailable.Add(bookmarkEntry);
                }
                else
                {
                    bookmarkLogEntriesNotAvailable.Add(bookmarkEntry);
                }
            }

            int rangeBegin = _logRange?.Begin?.Index ?? int.MinValue;
            int rangeEnd = _logRange?.End?.Index ?? int.MaxValue;

            bookmarkLogEntriesInRange = [.. bookMarkAvailable.Where(entry => entry.Index >= rangeBegin && entry.Index <= rangeEnd)];
            bookmarkLogEntriesOutOfRange = [];

            if (_logRange?.IsConstrained == true && _showFullTimeline)
            {
                bookmarkLogEntriesOutOfRange = [.. bookMarkAvailable.Where(entry => entry.Index < rangeBegin || entry.Index > rangeEnd)];
            }
        }

        #endregion

        #region Bucketing Logic

        /// <summary>
        /// Recalculates time buckets and rebuilds the cached sorted bucket key list.
        /// Uses the histogram width (excluding zoom strip) for bucket count calculation.
        /// </summary>
        private void RebuildBuckets()
        {
            buckets.Clear();
            sortedBucketKeys.Clear();

            bool rangeIsConstrained = _logRange?.IsConstrained == true;

            if (_showFullTimeline && fullSpanMinimum != default)
            {
                minimumTimestamp = fullSpanMinimum;
                maximumTimestamp = fullSpanMaximum;
            }
            else if (displayedLogEntries.Count > 0)
            {
                minimumTimestamp = displayedLogEntries[0].TimeStamp;
                maximumTimestamp = displayedLogEntries[^1].TimeStamp;
            }
            else
            {
                return;
            }

            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;
            if (totalSpan.TotalSeconds < 1)
                totalSpan = TimeSpan.FromSeconds(1);

            int histogramWidth = GetHistogramWidth();
            int desiredBucketCount = Math.Max(MINIMUM_BUCKET_COUNT, histogramWidth / 10);
            currentBucketSize = CalculateOptimalBucketSize(totalSpan, desiredBucketCount);

            DateTime bucketStart = RoundDownToNearestBucket(minimumTimestamp, currentBucketSize);
            DateTime bucketEnd = RoundDownToNearestBucket(maximumTimestamp, currentBucketSize).Add(currentBucketSize);

            DateTime currentBucket = bucketStart;
            while (currentBucket < bucketEnd)
            {
                buckets[currentBucket] = [];
                currentBucket = currentBucket.Add(currentBucketSize);
            }

            foreach (LogEntry entry in displayedLogEntries)
            {
                DateTime bucketKey = GetBucketKeyForTimestamp(entry.TimeStamp, bucketStart, currentBucketSize);
                if (buckets.TryGetValue(bucketKey, out List<LogEntry> value))
                    value.Add(entry);
            }

            if (buckets.Count > 0)
            {
                int maximumCount = buckets.Values.Max(list => list.Count);
                if (maximumCount > 0)
                    maximumRawValue = Math.Pow(maximumCount, SCALE_POWER);
            }

            sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
            totalBarWidth = sortedBucketKeys.Count > 0 ? (float)histogramWidth / sortedBucketKeys.Count : 0;
        }

        private static TimeSpan CalculateOptimalBucketSize(TimeSpan totalSpan, int desiredBuckets)
        {
            double secondsPerBucket = totalSpan.TotalSeconds / desiredBuckets;

            TimeSpan[] niceIntervals =
            [
                TimeSpan.FromMilliseconds(10),
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(30),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(2),
                TimeSpan.FromMinutes(3),
                TimeSpan.FromMinutes(4),
                TimeSpan.FromMinutes(5),
                TimeSpan.FromMinutes(10),
                TimeSpan.FromMinutes(30),
                TimeSpan.FromHours(1),
                TimeSpan.FromHours(6),
                TimeSpan.FromHours(12),
                TimeSpan.FromDays(1),
                TimeSpan.FromDays(7),
                TimeSpan.FromDays(30)
            ];

            foreach (TimeSpan interval in niceIntervals)
            {
                if (interval.TotalSeconds >= secondsPerBucket)
                    return interval;
            }

            return TimeSpan.FromDays(30);
        }

        private static DateTime RoundDownToNearestBucket(DateTime timestamp, TimeSpan bucketSize)
        {
            long roundedTicks = (timestamp.Ticks / bucketSize.Ticks) * bucketSize.Ticks;
            return new DateTime(roundedTicks);
        }

        private static DateTime GetBucketKeyForTimestamp(DateTime timestamp, DateTime bucketStart, TimeSpan bucketSize)
        {
            long bucketIndex = (long)((timestamp - bucketStart).TotalSeconds / bucketSize.TotalSeconds);
            return bucketStart.Add(TimeSpan.FromSeconds(bucketIndex * bucketSize.TotalSeconds));
        }

        private int FindBucketIndexContainingTimestamp(DateTime timestamp, List<DateTime> keys)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (timestamp >= keys[i] && timestamp < keys[i].Add(currentBucketSize))
                    return i;
            }

            if (keys.Count == 0) return -1;
            if (timestamp < keys[0]) return 0;
            if (timestamp >= keys[^1].Add(currentBucketSize)) return keys.Count - 1;

            return -1;
        }

        #endregion

        #region Layout Helpers

        /// <summary>Returns the drawable histogram width, excluding the zoom strip when a range is constrained.</summary>
        private int GetHistogramWidth()
        {
            return _logRange?.IsConstrained == true
                ? Math.Max(0, this.Width - ZOOM_STRIP_WIDTH)
                : this.Width;
        }
        /// <summary>
        /// Returns the x position for an out-of-range marker, projected onto the histogram width
        /// using the full collection span. Returns -1 if position cannot be determined.
        /// Only valid in full-timeline mode where the full span maps to the histogram width.
        /// </summary>
        private float GetOutOfRangeMarkerXPosition(LogEntry entry)
        {
            if (fullSpanMaximum == fullSpanMinimum)
                return -1;

            double percentage = (entry.TimeStamp - fullSpanMinimum).TotalSeconds
                              / (fullSpanMaximum - fullSpanMinimum).TotalSeconds;

            if (percentage < 0 || percentage > 1)
                return -1;

            return (float)(Math.Clamp(percentage, 0.0, 1.0) * GetHistogramWidth());
        }

        /// <summary>
        /// Calculates the x position of a marker for the given entry using the cached bucket layout.
        /// Returns -1 if the entry cannot be placed.
        /// </summary>
        private float GetMarkerXPosition(LogEntry entry)
        {
            int bucketIndex = FindBucketIndexContainingTimestamp(entry.TimeStamp, sortedBucketKeys);
            if (bucketIndex < 0)
                return -1;

            DateTime bucketKey = sortedBucketKeys[bucketIndex];
            double fraction = (entry.TimeStamp - bucketKey).TotalSeconds / currentBucketSize.TotalSeconds;
            return bucketIndex * totalBarWidth + (float)(fraction * totalBarWidth);
        }

        /// <summary>Draws a single marker ellipse centered on the given x position at the given y offset from the bottom.</summary>
        private static void DrawMarkerEllipse(Graphics graphics, float centerX, int drawableHeight, int yOffset, Color color)
        {
            float markerX = centerX - (MARKER_SIZE / 2);
            float markerY = drawableHeight - yOffset;
            using SolidBrush brush = new(color);
            graphics.FillEllipse(brush, markerX, markerY, MARKER_SIZE, MARKER_SIZE);
        }

        /// <summary>Draws only the outline of a marker (hollow ring) at the given position.</summary>
        private static void DrawMarkerOutline(Graphics graphics, float centerX, int drawableHeight, int yOffset, Color color)
        {
            float markerX = centerX - (MARKER_SIZE / 2);
            float markerY = drawableHeight - yOffset;
            using Pen pen = new(color, 1.5f);
            graphics.DrawEllipse(pen, markerX, markerY, MARKER_SIZE, MARKER_SIZE);
        }

        /// <summary>Returns the index of the marker in the given list whose hit rect contains the mouse position, or -1.</summary>
        private int GetMarkerIndexAtPosition(int mouseX, int mouseY, List<LogEntry> entries, int yOffset)
        {
            if (sortedBucketKeys.Count == 0)
                return -1;

            int drawableHeight = this.Height;
            int histogramWidth = GetHistogramWidth();

            for (int i = 0; i < entries.Count; i++)
            {
                float centerX = GetMarkerXPosition(entries[i]);
                if (centerX < 0 || centerX > histogramWidth)
                    continue;

                float markerX = centerX - (MARKER_SIZE / 2);
                float markerY = drawableHeight - yOffset;

                if (mouseX >= markerX && mouseX <= markerX + MARKER_SIZE &&
                    mouseY >= markerY && mouseY <= markerY + MARKER_SIZE)
                    return i;
            }

            return -1;
        }

        /// <summary>Recalculates the zoom button rects. Called at paint time so they track control height.</summary>
        private void RecalculateZoomButtonRects()
        {
            if (_logRange?.IsConstrained != true)
            {
                zoomInButtonRect = Rectangle.Empty;
                zoomOutButtonRect = Rectangle.Empty;
                return;
            }

            int stripLeft = this.Width - ZOOM_STRIP_WIDTH;
            int centerX = stripLeft + (ZOOM_STRIP_WIDTH / 2) - (ZOOM_BUTTON_SIZE / 2);
            int totalButtonsHeight = ZOOM_BUTTON_SIZE * 2 + ZOOM_BUTTON_SPACING;
            int buttonY = this.Height / 2 - totalButtonsHeight / 2;

            zoomInButtonRect = new Rectangle(centerX, buttonY, ZOOM_BUTTON_SIZE, ZOOM_BUTTON_SIZE);
            zoomOutButtonRect = new Rectangle(centerX, buttonY + ZOOM_BUTTON_SIZE + ZOOM_BUTTON_SPACING, ZOOM_BUTTON_SIZE, ZOOM_BUTTON_SIZE);
        }

        #endregion

        #region Painting

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(this.BackColor);

            if (this.Width <= 0 || this.Height <= 0)
                return;

            bool rangeIsConstrained = _logRange?.IsConstrained == true;

            if (rangeIsConstrained)
            {
                RecalculateZoomButtonRects();
                DrawZoomStrip(graphics);
            }

            if (minimumTimestamp == default && maximumTimestamp == default)
                return;

            int histogramWidth = GetHistogramWidth();
            int drawableHeight = this.Height;

            // Clip all histogram drawing to the histogram area
            graphics.SetClip(new Rectangle(0, 0, histogramWidth, drawableHeight));

            DrawRangeOverlay(graphics, histogramWidth, drawableHeight);
            DrawHistogramBars(graphics, drawableHeight);
            DrawErrorMarkers(graphics, histogramWidth, drawableHeight);
            DrawBookmarkMarkers(graphics, histogramWidth, drawableHeight);
            DrawOnScreenRangeIndicator(graphics, histogramWidth, drawableHeight);
            DrawTimeTickMarks(graphics, histogramWidth);

            graphics.ResetClip();

            DrawActiveTooltip(graphics, histogramWidth, drawableHeight);
        }

        #endregion

        #region Drawing

        /// <summary>Draws the zoom strip background and divider line.</summary>
        private void DrawZoomStrip(Graphics graphics)
        {
            int stripLeft = this.Width - ZOOM_STRIP_WIDTH;

            using (SolidBrush backgroundBrush = new(ZOOM_STRIP_BACKGROUND_COLOR))
                graphics.FillRectangle(backgroundBrush, stripLeft, 0, ZOOM_STRIP_WIDTH, this.Height);

            DrawZoomButton(graphics, zoomInButtonRect, isZoomIn: true,
                hovered: hoveredZoomButton == ZoomButton.ZoomIn,
                active: !_showFullTimeline);

            DrawZoomButton(graphics, zoomOutButtonRect, isZoomIn: false,
                hovered: hoveredZoomButton == ZoomButton.ZoomOut,
                active: _showFullTimeline);
        }

        /// <summary>
        /// Dims everything outside the log range. Only drawn in full-timeline mode
        /// so the user can see where the range sits within the full span.
        /// </summary>
        private void DrawRangeOverlay(Graphics graphics, int histogramWidth, int drawableHeight)
        {
            if (!(_logRange?.IsConstrained == true && _showFullTimeline)) return;

            if (fullSpanMaximum == fullSpanMinimum)
                return;

            double totalSeconds = (fullSpanMaximum - fullSpanMinimum).TotalSeconds;

            float rangeStartX = _logRange.Begin != null
                ? (float)((_logRange.Begin.TimeStamp - fullSpanMinimum).TotalSeconds / totalSeconds * histogramWidth)
                : 0;
            float rangeEndX = _logRange.End != null
                ? (float)((_logRange.End.TimeStamp - fullSpanMinimum).TotalSeconds / totalSeconds * histogramWidth)
                : histogramWidth;

            using SolidBrush dimBrush = new(Color.FromArgb(60, 0, 0, 0));

            if (rangeStartX > 0)
                graphics.FillRectangle(dimBrush, 0, 0, rangeStartX, drawableHeight);
            if (rangeEndX < histogramWidth)
                graphics.FillRectangle(dimBrush, rangeEndX, 0, histogramWidth - rangeEndX, drawableHeight);
        }

        /// <summary>Draws the histogram bars for all non-empty buckets.</summary>
        private void DrawHistogramBars(Graphics graphics, int drawableHeight)
        {
            if (buckets.Count == 0)
                return;

            float barWidth = totalBarWidth - 1;

            for (int i = 0; i < sortedBucketKeys.Count; i++)
            {
                int entryCount = buckets[sortedBucketKeys[i]].Count;
                if (entryCount == 0)
                    continue;

                double scaledValue = Math.Pow(entryCount, SCALE_POWER);
                float barHeight = (float)((scaledValue / maximumRawValue) * drawableHeight);
                float x = i * totalBarWidth;
                float y = drawableHeight - barHeight;

                Color barColor = (i == hoveredBucketIndex) ? BAR_HOVER_COLOR : BAR_COLOR;
                using SolidBrush brush = new(barColor);
                graphics.FillRectangle(brush, x, y, barWidth, barHeight);
            }
        }

        private void DrawErrorMarkers(Graphics graphics, int histogramWidth, int drawableHeight)
        {
            for (int i = 0; i < errorLogEntriesOutOfRange.Count; i++)
            {
                float centerX = GetOutOfRangeMarkerXPosition(errorLogEntriesOutOfRange[i]);
                if (centerX < 0 || centerX > histogramWidth) continue;

                DrawMarkerEllipse(graphics, centerX, drawableHeight, ERROR_MARKER_Y_OFFSET, ERROR_MARKER_OUT_OF_RANGE_COLOR);
            }

            for (int i = 0; i < errorLogEntriesInRange.Count; i++)
            {
                float centerX = GetMarkerXPosition(errorLogEntriesInRange[i]);
                if (centerX < 0 || centerX > histogramWidth) continue;

                Color color = (i == hoveredErrorIndex) ? ERROR_MARKER_HOVER_COLOR : ERROR_MARKER_COLOR;
                DrawMarkerEllipse(graphics, centerX, drawableHeight, ERROR_MARKER_Y_OFFSET, color);
            }
        }

        /// <summary>
        /// Draws bookmark markers one band above error markers.
        /// Out-of-range bookmarks are drawn as hollow circles (outlines only) in faded color.
        /// In-range bookmarks are drawn as filled circles.
        /// </summary>
        private void DrawBookmarkMarkers(Graphics graphics, int histogramWidth, int drawableHeight)
        {
            // Draw out-of-range bookmarks as hollow circles (faded outlines)
            foreach (LogEntry bookmarkEntry in bookmarkLogEntriesNotAvailable)
            {
                float centerX = GetOutOfRangeMarkerXPosition(bookmarkEntry);
                if (centerX < 0 || centerX > histogramWidth) continue;

                DrawMarkerOutline(graphics, centerX, drawableHeight, BOOKMARK_MARKER_Y_OFFSET, BOOKMARK_MARKER_OUT_OF_RANGE_COLOR);
            }

            // Draw out-of-range bookmarks as hollow circles (faded outlines)
            foreach (LogEntry bookmarkEntry in bookmarkLogEntriesOutOfRange)
            {
                float centerX = GetOutOfRangeMarkerXPosition(bookmarkEntry);
                if (centerX < 0 || centerX > histogramWidth) continue;

                DrawMarkerEllipse(graphics, centerX, drawableHeight, BOOKMARK_MARKER_Y_OFFSET, BOOKMARK_MARKER_OUT_OF_RANGE_COLOR);
            }

            // Draw in-range bookmarks as filled circles
            for (int i = 0; i < bookmarkLogEntriesInRange.Count; i++)
            {
                float centerX = GetMarkerXPosition(bookmarkLogEntriesInRange[i]);
                if (centerX < 0 || centerX > histogramWidth) continue;

                Color color = (i == hoveredBookmarkIndex) ? BOOKMARK_MARKER_HOVER_COLOR : BOOKMARK_MARKER_COLOR;
                DrawMarkerEllipse(graphics, centerX, drawableHeight, BOOKMARK_MARKER_Y_OFFSET, color);
            }
        }

        private void DrawOnScreenRangeIndicator(Graphics graphics, int histogramWidth, int drawableHeight)
        {
            if (!onScreenRangeStart.HasValue || !onScreenRangeEnd.HasValue)
                return;

            if (sortedBucketKeys.Count == 0)
                return;

            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;
            if (totalSpan.TotalSeconds <= 0)
                return;

            float startX = (float)((onScreenRangeStart.Value - minimumTimestamp).TotalSeconds / totalSpan.TotalSeconds * histogramWidth);
            float endX = (float)((onScreenRangeEnd.Value - minimumTimestamp).TotalSeconds / totalSpan.TotalSeconds * histogramWidth);

            startX = Math.Clamp(startX, 0, histogramWidth);
            endX = Math.Clamp(endX, 0, histogramWidth);

            float width = endX - startX;

            if (width < MINIMUM_VISIBLE_RANGE_WIDTH)
            {
                float center = (startX + endX) / 2;
                startX = center - (MINIMUM_VISIBLE_RANGE_WIDTH / 2f);
                endX = center + (MINIMUM_VISIBLE_RANGE_WIDTH / 2f);
                width = MINIMUM_VISIBLE_RANGE_WIDTH;
            }

            if (width <= 0) return;

            using (SolidBrush fillBrush = new(VISIBLE_RANGE_COLOR))
                graphics.FillRectangle(fillBrush, startX, 0, width, drawableHeight);

            using Pen borderPen = new(VISIBLE_RANGE_BORDER_COLOR, 2);
            graphics.DrawLine(borderPen, startX, 0, startX, drawableHeight);
            graphics.DrawLine(borderPen, endX, 0, endX, drawableHeight);
        }

        private void DrawTimeTickMarks(Graphics graphics, int histogramWidth)
        {
            using Font font = new("Segoe UI", 7);
            using SolidBrush brush = new(Color.FromArgb(120, 120, 120));

            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;
            int tickCount = Math.Max(2, histogramWidth / 65);

            for (int i = 0; i <= tickCount; i++)
            {
                float x = (float)i / tickCount * histogramWidth;
                DateTime tickTime = minimumTimestamp.Add(TimeSpan.FromSeconds(totalSpan.TotalSeconds * i / tickCount));
                string label = FormatTickLabelBySpan(tickTime, totalSpan);
                SizeF labelSize = graphics.MeasureString(label, font);

                float labelX = x - labelSize.Width / 2;
                if (i == 0) labelX = 0;
                else if (i == tickCount) labelX = histogramWidth - labelSize.Width;

                graphics.DrawString(label, font, brush, labelX, 0);
            }
        }

        private static string FormatTickLabelBySpan(DateTime timestamp, TimeSpan totalSpan)
        {
            if (totalSpan.TotalDays >= 2) return timestamp.ToString("MMM-dd");
            if (totalSpan.TotalDays >= 1) return timestamp.ToString("d HH:mm");
            if (totalSpan.TotalHours >= 1) return timestamp.ToString("HH:mm");
            if (totalSpan.TotalMinutes >= 1) return timestamp.ToString("H:mm:ss");
            if (totalSpan.TotalSeconds >= 1) return timestamp.ToString("s.ff") + " s";
            return timestamp.ToString("s.fff") + " s";
        }

        /// <summary>
        /// Draws the tooltip for whichever element is currently hovered.
        /// Called after ResetClip so tooltips can overflow the histogram area if needed.
        /// </summary>
        private void DrawActiveTooltip(Graphics graphics, int histogramWidth, int drawableHeight)
        {
            if (hoveredBookmarkIndex >= 0 && hoveredBookmarkIndex < bookmarkLogEntriesInRange.Count)
                DrawMarkerTooltip(graphics, bookmarkLogEntriesInRange[hoveredBookmarkIndex], histogramWidth, drawableHeight, BOOKMARK_MARKER_Y_OFFSET, BOOKMARK_MARKER_COLOR, "Bookmark");
            else if (hoveredErrorIndex >= 0 && hoveredErrorIndex < errorLogEntriesInRange.Count)
                DrawMarkerTooltip(graphics, errorLogEntriesInRange[hoveredErrorIndex], histogramWidth, drawableHeight, ERROR_MARKER_Y_OFFSET, ERROR_MARKER_COLOR, "Error");
            else if (hoveredBucketIndex >= 0 && hoveredBucketIndex < sortedBucketKeys.Count)
                DrawBucketTooltip(graphics, histogramWidth, sortedBucketKeys[hoveredBucketIndex]);
        }

        private void DrawBucketTooltip(Graphics graphics, int histogramWidth, DateTime bucketKey)
        {
            List<LogEntry> bucketEntries = buckets[bucketKey];
            int entryCount = bucketEntries.Count;

            string tooltipText = entryCount == 0
                ? $"0 events at {bucketKey:yyyy-MM-dd HH:mm:ss}"
                : $"{entryCount} {(entryCount == 1 ? "event" : "events")} at {bucketEntries[0].TimeStamp:yyyy-MM-dd HH:mm:ss}";

            float tooltipX = hoveredBucketIndex * totalBarWidth + totalBarWidth / 2;
            float tooltipY = 2;

            DrawTooltipBox(graphics, tooltipText, ref tooltipX, ref tooltipY, histogramWidth,
                Color.FromArgb(150, 50, 50, 50), Color.FromArgb(100, 100, 100), padding: 0);
        }

        /// <summary>
        /// Draws a tooltip for an error or bookmark marker, anchored just above its marker band.
        /// Shared between error and bookmark marker types.
        /// </summary>
        private void DrawMarkerTooltip(Graphics graphics, LogEntry entry, int histogramWidth, int drawableHeight, int markerYOffset, Color accentColor, string label)
        {
            string tooltipText = $"{label} at {entry.TimeStamp:yyyy-MM-dd HH:mm:ss}";

            float centerX = GetMarkerXPosition(entry);
            if (centerX < 0) return;

            float markerBandTop = drawableHeight - markerYOffset;
            float tooltipY = markerBandTop - 4; // Adjusted; will be calculated within DrawTooltipBox

            DrawTooltipBox(graphics, tooltipText, ref centerX, ref tooltipY, histogramWidth,
                Color.FromArgb(220, 30, 30, 40), accentColor, padding: 8, baselineY: markerBandTop);
        }

        /// <summary>
        /// Shared helper to draw a tooltip box with text, avoiding duplicate code.
        /// </summary>
        private static void DrawTooltipBox(Graphics graphics, string tooltipText, ref float tooltipX, ref float tooltipY,
            int histogramWidth, Color backgroundColor, Color borderColor, int padding = 0, float baselineY = 0)
        {
            using Font font = new("Segoe UI", 9);
            SizeF textSize = graphics.MeasureString(tooltipText, font);

            float tooltipWidth = textSize.Width + (padding > 0 ? padding * 2 : 0);
            float tooltipHeight = textSize.Height + (padding > 0 ? padding / 2 : 0);

            // Position calculation
            if (padding > 0)
            {
                // For markers: anchor above the marker band
                tooltipX = Math.Clamp(tooltipX - tooltipWidth / 2, 5, histogramWidth - tooltipWidth - 5);
                tooltipY = Math.Max(2, baselineY - tooltipHeight - 4);
            }
            else
            {
                // For buckets: center horizontally, position near top
                tooltipX = Math.Clamp(tooltipX - textSize.Width / 2, 5, histogramWidth - textSize.Width - 5);
                tooltipY = Math.Max(2, tooltipY);
            }

            // Draw background and border
            using (SolidBrush backgroundBrush = new(backgroundColor))
            using (Pen borderPen = new(borderColor, 1))
            {
                graphics.FillRectangle(backgroundBrush, tooltipX, tooltipY, tooltipWidth, tooltipHeight);
                graphics.DrawRectangle(borderPen, tooltipX, tooltipY, tooltipWidth, tooltipHeight);
            }

            // Draw text
            using SolidBrush textBrush = new(Color.White);
            float textX = tooltipX + (padding > 0 ? padding : 0);
            float textY = tooltipY + (padding > 0 ? padding / 4 : 0);
            graphics.DrawString(tooltipText, font, textBrush, textX, textY);
        }

        /// <summary>
        /// Draws a single zoom button with a + or - symbol.
        /// Inactive state (button would be a no-op) is rendered greyed out.
        /// </summary>
        private static void DrawZoomButton(Graphics graphics, Rectangle bounds, bool isZoomIn, bool hovered, bool active)
        {
            if (bounds.IsEmpty) return;

            Color backgroundColor = !active
                ? ZOOM_BUTTON_INACTIVE_BACKGROUND_COLOR
                : hovered ? ZOOM_BUTTON_HOVER_BACKGROUND_COLOR : ZOOM_BUTTON_BACKGROUND_COLOR;
            Color borderColor = active ? ZOOM_BUTTON_INACTIVE_BORDER_COLOR : ZOOM_BUTTON_BORDER_COLOR;
            Color symbolColor = active ? ZOOM_BUTTON_INACTIVE_SYMBOL_COLOR : ZOOM_BUTTON_SYMBOL_COLOR;

            using (GraphicsPath path = CreateRoundedRectPath(bounds, 4))
            {
                using SolidBrush backgroundBrush = new(backgroundColor);
                graphics.FillPath(backgroundBrush, path);

                using Pen borderPen = new(borderColor, 1);
                graphics.DrawPath(borderPen, path);
            }

            int padding = 5;
            int centerX = bounds.X + bounds.Width / 2;
            int centerY = bounds.Y + bounds.Height / 2;
            int halfLine = bounds.Width / 2 - padding;

            using Pen symbolPen = new(symbolColor, 1.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };

            // Horizontal bar (shared by both + and −)
            graphics.DrawLine(symbolPen, centerX - halfLine, centerY, centerX + halfLine, centerY);

            if (isZoomIn)
            {
                // Vertical bar for +
                graphics.DrawLine(symbolPen, centerX, centerY - halfLine, centerX, centerY + halfLine);
            }
        }

        private static GraphicsPath CreateRoundedRectPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new();
            int diameter = radius * 2;

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        #endregion

        #region Mouse Interaction

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            bool rangeIsConstrained = _logRange?.IsConstrained == true;

            // Zoom button hover — checked first, takes priority over histogram
            if (rangeIsConstrained)
            {
                ZoomButton newZoomHover = HitTestZoomButton(e.X, e.Y);
                if (newZoomHover != hoveredZoomButton)
                {
                    hoveredZoomButton = newZoomHover;
                    UpdateZoomButtonTooltip(newZoomHover);
                    this.Invalidate();
                }

                if (newZoomHover != ZoomButton.None)
                {
                    SetCursor(Cursors.Hand);
                    return;
                }
            }

            if (sortedBucketKeys.Count == 0)
            {
                SetCursor(Cursors.Default);
                return;
            }

            // Bookmark marker hover
            int newBookmark = GetMarkerIndexAtPosition(e.X, e.Y, bookmarkLogEntriesInRange, BOOKMARK_MARKER_Y_OFFSET);
            if (UpdateHoveredMarker(ref hoveredBookmarkIndex, newBookmark, ref hoveredErrorIndex, ref hoveredBucketIndex))
            {
                SetCursor(Cursors.Hand);
                return;
            }

            // Error marker hover
            int newError = GetMarkerIndexAtPosition(e.X, e.Y, errorLogEntriesInRange, ERROR_MARKER_Y_OFFSET);
            if (UpdateHoveredMarker(ref hoveredErrorIndex, newError, ref hoveredBookmarkIndex, ref hoveredBucketIndex))
            {
                SetCursor(Cursors.Hand);
                return;
            }

            // Bucket hover — only within histogram area
            int histogramWidth = GetHistogramWidth();
            if (e.X < histogramWidth && totalBarWidth > 0)
            {
                int newBucketIndex = (int)(e.X / totalBarWidth);
                if (newBucketIndex >= 0 && newBucketIndex < sortedBucketKeys.Count)
                {
                    if (newBucketIndex != hoveredBucketIndex)
                    {
                        ClearHoveredMarker(ref hoveredBucketIndex);
                        hoveredBucketIndex = newBucketIndex;
                        this.Invalidate();
                    }

                    bool hasEntries = buckets[sortedBucketKeys[newBucketIndex]].Count > 0;
                    SetCursor(hasEntries ? Cursors.Hand : Cursors.Default);
                    return;
                }
            }

            // Nothing hovered
            ClearHoveredMarker(ref hoveredBucketIndex);
            ClearHoveredMarker(ref hoveredErrorIndex);
            ClearHoveredMarker(ref hoveredBookmarkIndex);
            SetCursor(Cursors.Default);
        }

        private ZoomButton HitTestZoomButton(int x, int y)
        {
            if (!zoomInButtonRect.IsEmpty && zoomInButtonRect.Contains(x, y)) return ZoomButton.ZoomIn;
            if (!zoomOutButtonRect.IsEmpty && zoomOutButtonRect.Contains(x, y)) return ZoomButton.ZoomOut;
            return ZoomButton.None;
        }

        private void UpdateZoomButtonTooltip(ZoomButton button)
        {
            string text = button switch
            {
                ZoomButton.ZoomIn => "Zoom in — show selected range only",
                ZoomButton.ZoomOut => "Zoom out — show full timeline",
                _ => string.Empty
            };
            toolTip.SetToolTip(this, text);
        }

        /// <summary>
        /// Updates a hovered marker index, clearing competing hover states.
        /// Returns true if the new index is valid (something is hovered).
        /// </summary>
        private bool UpdateHoveredMarker(ref int target, int newIndex, ref int competing1, ref int competing2)
        {
            if (newIndex == -1)
            {
                ClearHoveredMarker(ref target);
                return false;
            }

            if (newIndex != target)
            {
                ClearHoveredMarker(ref competing1);
                ClearHoveredMarker(ref competing2);
                target = newIndex;
                this.Invalidate();
            }

            return true;
        }

        private void ClearHoveredMarker(ref int index)
        {
            if (index != -1)
            {
                index = -1;
                this.Invalidate();
            }
        }

        private void SetCursor(Cursor cursor)
        {
            if (this.Cursor != cursor)
                this.Cursor = cursor;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            ClearHoveredMarker(ref hoveredBucketIndex);
            ClearHoveredMarker(ref hoveredErrorIndex);
            ClearHoveredMarker(ref hoveredBookmarkIndex);

            if (hoveredZoomButton != ZoomButton.None)
            {
                hoveredZoomButton = ZoomButton.None;
                toolTip.SetToolTip(this, string.Empty);
                this.Invalidate();
            }

            SetCursor(Cursors.Default);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            // Zoom buttons
            if (_logRange?.IsConstrained == true)
            {
                ZoomButton clicked = HitTestZoomButton(e.X, e.Y);
                if (clicked != ZoomButton.None)
                {
                    bool newShowFullTimeline = clicked == ZoomButton.ZoomOut;
                    if (_showFullTimeline != newShowFullTimeline)
                    {
                        _showFullTimeline = newShowFullTimeline;
                        RebuildRangeRelatedObjects();
                        this.Invalidate();
                    }
                    return;
                }
            }

            if (sortedBucketKeys.Count == 0)
                return;

            int clickedBookmark = GetMarkerIndexAtPosition(e.X, e.Y, bookmarkLogEntriesInRange, BOOKMARK_MARKER_Y_OFFSET);
            if (clickedBookmark != -1)
            {
                BookmarkMarkerClicked?.Invoke(this, bookmarkLogEntriesInRange[clickedBookmark]);
                return;
            }

            int clickedError = GetMarkerIndexAtPosition(e.X, e.Y, errorLogEntriesInRange, ERROR_MARKER_Y_OFFSET);
            if (clickedError != -1)
            {
                ErrorMarkerClicked?.Invoke(this, errorLogEntriesInRange[clickedError]);
                return;
            }

            int histogramWidth = GetHistogramWidth();
            if (e.X < histogramWidth && totalBarWidth > 0)
            {
                int clickedBucketIndex = (int)(e.X / totalBarWidth);
                if (clickedBucketIndex >= 0 && clickedBucketIndex < sortedBucketKeys.Count)
                {
                    List<LogEntry> bucketEntries = buckets[sortedBucketKeys[clickedBucketIndex]];
                    if (bucketEntries.Count > 0)
                        CellClicked?.Invoke(this, bucketEntries[0]);
                }
            }
        }

        internal void Clear()
        {
            filteredLogEntries.Clear();
            displayedLogEntries.Clear();
            buckets.Clear();
            sortedBucketKeys.Clear();
            errorLogEntriesInRange.Clear();
            allBookmarkLogEntries.Clear();
            bookmarkLogEntriesInRange.Clear();

            fullSpanMinimum = default;
            fullSpanMaximum = default;
            minimumTimestamp = default;
            maximumTimestamp = default;
            maximumRawValue = 0;
            currentBucketSize = default;
            _logRange = null;
            _showFullTimeline = true;
            hoveredBucketIndex = -1;
            hoveredErrorIndex = -1;
            hoveredBookmarkIndex = -1;
            hoveredZoomButton = ZoomButton.None;
            zoomInButtonRect = Rectangle.Empty;
            zoomOutButtonRect = Rectangle.Empty;

            this.Invalidate();
        }

        #endregion
    }
}