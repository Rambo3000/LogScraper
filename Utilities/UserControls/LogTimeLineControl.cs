using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Rendering;

namespace LogScraper.Utilities.UserControls
{
    /// <summary>
    /// A timeline visualization control that displays log entries as a histogram with error markers
    /// and a visible range indicator showing the current scroll position.
    /// </summary>
    public partial class LogTimeLineControl : UserControl
    {
        #region Fields

        // Log entry collections
        private List<LogEntry> allLogEntries = [];
        private List<LogEntry> displayedLogEntries = [];
        private readonly Dictionary<DateTime, List<LogEntry>> buckets = [];
        private List<LogEntry> errorLogEntries = [];
        private List<LogEntry> allBookmarkLogEntries = [];
        private List<LogEntry> bookmarkLogEntries = [];

        // Full collection span (for minimap)
        private DateTime fullSpanMinimum;
        private DateTime fullSpanMaximum;

        // Log range
        private LogRange _logRange;
        private bool _showFullTimeline = true;

        // Bucketing and scaling
        private TimeSpan currentBucketSize;
        private DateTime minimumTimestamp;
        private DateTime maximumTimestamp;
        private double maximumRawValue;

        // Hover tracking
        private int hoveredBucketIndex = -1;
        private int hoveredErrorIndex = -1;
        private int hoveredBookmarkIndex = -1;
        private bool hoveredMinimap = false;

        // Visible range tracking (timestamp-based to match timeline bars)
        private DateTime? visibleRangeStart = null;
        private DateTime? visibleRangeEnd = null;

        // Constants
        private const double SCALE_POWER = 0.4;
        private const int MINIMUM_BUCKET_COUNT = 256;
        private const int ERROR_MARKER_SIZE = 8;
        private const int MINIMUM_VISIBLE_RANGE_WIDTH = 1;
        private const int MINIMAP_HEIGHT = 5;
        private const int MINIMAP_ACCENT_WIDTH = 2;

        // Colors
        private static readonly Color BAR_COLOR = Color.LightGray;
        private static readonly Color BAR_HOVER_COLOR = Color.FromArgb(100, 160, 210);
        private static readonly Color LABEL_COLOR = Color.FromArgb(100, 100, 100);
        private static readonly Color VISIBLE_RANGE_COLOR = Color.FromArgb(80, 100, 160, 210);
        private static readonly Color VISIBLE_RANGE_BORDER_COLOR = Color.FromArgb(150, 100, 160, 210);
        private static readonly Color ERROR_MARKER_COLOR = Color.FromArgb(220, 50, 50);
        private static readonly Color ERROR_MARKER_HOVER_COLOR = Color.FromArgb(255, 80, 80);
        private static readonly Color BOOKMARK_MARKER_COLOR = Color.SteelBlue;
        private static readonly Color BOOKMARK_MARKER_HOVER_COLOR = Color.FromArgb(255, 120, 180, 230);
        private static readonly Color BOOKMARK_MARKER_OUT_OF_RANGE_COLOR = Color.FromArgb(100, 70, 130, 180);
        private static readonly Color MINIMAP_BACKGROUND_COLOR = Color.FromArgb(230, 230, 230);
        private static readonly Color MINIMAP_RANGE_COLOR = Color.FromArgb(153, 70, 130, 180); // SteelBlue ~60% opacity
        private static readonly Color MINIMAP_ACCENT_COLOR = Color.SteelBlue;

        // Tooltip
        private readonly ToolTip toolTip = new();

        #endregion

        #region Events

        /// <summary>
        /// Raised when a histogram bar is clicked.
        /// </summary>
        public event EventHandler<LogEntry> CellClicked;

        /// <summary>
        /// Raised when an error marker is clicked.
        /// </summary>
        public event EventHandler<LogEntry> ErrorMarkerClicked;

        /// <summary>
        /// Raised when a bookmark marker is clicked.
        /// </summary>
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
            this.Resize += (s, e) => this.Invalidate();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the log entries displayed in the timeline.
        /// Supports incremental updates if new entries are appended to existing data.
        /// The full collection is used to determine the minimap time span.
        /// </summary>
        public void UpdateLogEntries(List<LogEntry> entries, LogCollection fullCollection)
        {
            // Store full span from collection for minimap
            if (fullCollection?.LogEntries?.Count > 0)
            {
                fullSpanMinimum = fullCollection.LogEntries[0].TimeStamp;
                fullSpanMaximum = fullCollection.LogEntries[^1].TimeStamp;
            }

            if (entries == null || entries.Count == 0)
            {
                allLogEntries.Clear();
                displayedLogEntries.Clear();
                buckets.Clear();
                errorLogEntries.Clear();
                this.Invalidate();
                return;
            }

            bool isAppend = false;

            // Check if this is an incremental append
            if (allLogEntries.Count > 0 && entries.Count > allLogEntries.Count)
            {
                LogEntry currentFirst = allLogEntries[0];
                LogEntry currentLast = allLogEntries[^1];

                LogEntry newFirst = entries[0];
                LogEntry newLast = entries[allLogEntries.Count - 1];

                if (currentFirst.TimeStamp == newFirst.TimeStamp && currentLast.TimeStamp == newLast.TimeStamp)
                    isAppend = true;
            }

            if (isAppend)
            {
                List<LogEntry> newEntries = [.. entries.Skip(allLogEntries.Count)];
                allLogEntries.AddRange(newEntries);
            }
            else
            {
                allLogEntries = [.. entries];
            }

            RebuildDisplayedEntries();
            errorLogEntries = [.. displayedLogEntries.Where(entry => entry.IsErrorLogEntry)];

            this.Invalidate();
        }

        /// <summary>
        /// Updates the bookmark entries displayed in the timeline.
        /// </summary>
        public void SetBookmarks(List<LogEntry> bookmarks)
        {
            allBookmarkLogEntries = bookmarks ?? [];
            RebuildFilteredBookmarkMarkers();
            this.Invalidate();
        }

        /// <summary>
        /// Updates the log range used to filter the histogram and bookmark markers.
        /// </summary>
        public void SetLogRange(LogRange logRange)
        {
            _logRange = logRange;
            RebuildDisplayedEntries();
            RebuildFilteredBookmarkMarkers();
            errorLogEntries = [.. displayedLogEntries.Where(entry => entry.IsErrorLogEntry)];
            this.Invalidate();
        }

        /// <summary>
        /// Sets the visible range of log entries currently shown in the log viewer.
        /// </summary>
        public void SetVisibleRange(DateTime startTimestamp, DateTime endTimestamp)
        {
            visibleRangeStart = startTimestamp;
            visibleRangeEnd = endTimestamp;
            this.Invalidate();
        }

        /// <summary>
        /// Clears the visible range indicator.
        /// </summary>
        public void ClearVisibleRange()
        {
            visibleRangeStart = null;
            visibleRangeEnd = null;
            this.Invalidate();
        }

        #endregion

        #region Display Entry Management

        /// <summary>
        /// Rebuilds the displayed entry set based on the current range and show-full-timeline state.
        /// When no range is active or show full timeline is on, all entries are displayed.
        /// </summary>
        private void RebuildDisplayedEntries()
        {
            bool rangeIsConstrained = _logRange?.IsConstrained == true;

            if (!rangeIsConstrained || _showFullTimeline)
            {
                displayedLogEntries = allLogEntries;
            }
            else
            {
                int rangeBegin = _logRange.Begin?.Index ?? int.MinValue;
                int rangeEnd = _logRange.End?.Index ?? int.MaxValue;
                displayedLogEntries = [.. allLogEntries.Where(entry => entry.Index >= rangeBegin && entry.Index <= rangeEnd)];
            }

            RecalculateBuckets();
        }

        private void RebuildFilteredBookmarkMarkers()
        {
            int rangeBegin = _logRange?.Begin?.Index ?? int.MinValue;
            int rangeEnd = _logRange?.End?.Index ?? int.MaxValue;

            bookmarkLogEntries = [.. allBookmarkLogEntries.Where(entry => entry.Index >= rangeBegin && entry.Index <= rangeEnd)];
        }

        #endregion

        #region Bucketing Logic

        /// <summary>
        /// Recalculates time buckets for the histogram based on displayed log entries.
        /// </summary>
        private void RecalculateBuckets()
        {
            buckets.Clear();

            // Determine the time span to use regardless of entry count
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
                // Nothing to work with at all
                return;
            }

            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;
            if (totalSpan.TotalSeconds < 1)
                totalSpan = TimeSpan.FromSeconds(1);

            int availableWidth = this.Width;
            int desiredBucketCount = Math.Max(MINIMUM_BUCKET_COUNT, availableWidth / 10);
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
            long ticks = timestamp.Ticks;
            long bucketTicks = bucketSize.Ticks;
            long roundedTicks = (ticks / bucketTicks) * bucketTicks;
            return new DateTime(roundedTicks);
        }

        private static DateTime GetBucketKeyForTimestamp(DateTime timestamp, DateTime bucketStart, TimeSpan bucketSize)
        {
            TimeSpan offset = timestamp - bucketStart;
            long bucketIndex = (long)(offset.TotalSeconds / bucketSize.TotalSeconds);
            return bucketStart.Add(TimeSpan.FromSeconds(bucketIndex * bucketSize.TotalSeconds));
        }

        #endregion

        #region Coordinate Conversion

        /// <summary>
        /// Converts a timestamp to an x-coordinate position in the control.
        /// </summary>
        /// <returns>X position, or -1 if timestamp is out of range</returns>
        private float GetXPositionForTimestamp(DateTime timestamp)
        {
            if (displayedLogEntries.Count == 0)
                return 0;

            if (timestamp < minimumTimestamp || timestamp > maximumTimestamp)
                return -1;

            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;
            TimeSpan offset = timestamp - minimumTimestamp;

            double percentage = offset.TotalSeconds / totalSpan.TotalSeconds;
            return (float)(percentage * this.Width);
        }

        /// <summary>
        /// Converts a timestamp to an x-coordinate using the full collection span.
        /// Used for overlay calculations that must reflect the entire log, not just displayed entries.
        /// </summary>
        private float GetXPositionForTimestampInFullSpan(DateTime timestamp)
        {
            if (fullSpanMaximum == fullSpanMinimum)
                return 0;

            TimeSpan fullSpan = fullSpanMaximum - fullSpanMinimum;
            TimeSpan offset = timestamp - fullSpanMinimum;

            double percentage = offset.TotalSeconds / fullSpan.TotalSeconds;
            return (float)(Math.Clamp(percentage, 0.0, 1.0) * this.Width);
        }

        private int GetHistogramHeight()
        {
            return _logRange?.IsConstrained == true
                ? this.Height - MINIMAP_HEIGHT
                : this.Height;
        }

        /// <summary>
        /// Returns true if the given y coordinate falls within the minimap strip.
        /// </summary>
        private bool IsInMinimapStrip(int y)
        {
            return _logRange?.IsConstrained == true && y >= this.Height - MINIMAP_HEIGHT;
        }

        #endregion

        #region Painting

        /// <summary>
        /// Main paint handler for the control.
        /// </summary>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.Clear(this.BackColor);

            int drawableWidth = this.Width;
            int drawableHeight = GetHistogramHeight();

            if (drawableWidth <= 0 || drawableHeight <= 0)
                return;

            // Need at least a time span to draw anything meaningful
            if (minimumTimestamp == default && maximumTimestamp == default)
                return;

            // When range is defined but full timeline is shown, dim everything outside the log range
            if (_logRange?.IsConstrained == true && _showFullTimeline)
            {
                float rangeStartX = _logRange.Begin != null ? GetXPositionForTimestampInFullSpan(_logRange.Begin.TimeStamp) : 0;
                float rangeEndX = _logRange.End != null ? GetXPositionForTimestampInFullSpan(_logRange.End.TimeStamp) : drawableWidth;

                if (rangeStartX < 0) rangeStartX = 0;
                if (rangeEndX < 0) rangeEndX = drawableWidth;

                using SolidBrush dimBrush = new(Color.FromArgb(60, 0, 0, 0));

                if (rangeStartX > 0)
                    graphics.FillRectangle(dimBrush, 0, 0, rangeStartX, drawableHeight);

                if (rangeEndX < drawableWidth)
                    graphics.FillRectangle(dimBrush, rangeEndX, 0, drawableWidth - rangeEndX, drawableHeight);
            }

            // Draw histogram bars, tick marks, and markers only when buckets are populated
            if (buckets.Count > 0)
            {
                List<DateTime> sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
                int bucketCount = sortedBucketKeys.Count;
                float totalBarWidth = (float)drawableWidth / bucketCount;
                float barWidth = totalBarWidth - 1;

                for (int i = 0; i < bucketCount; i++)
                {
                    DateTime bucketKey = sortedBucketKeys[i];
                    int entryCount = buckets[bucketKey].Count;

                    if (entryCount > 0)
                    {
                        double scaledValue = Math.Pow(entryCount, SCALE_POWER);
                        float barHeight = (float)((scaledValue / maximumRawValue) * drawableHeight);

                        float x = i * totalBarWidth;
                        float y = drawableHeight - barHeight;

                        Color barColor = (i == hoveredBucketIndex) ? BAR_HOVER_COLOR : BAR_COLOR;

                        using SolidBrush brush = new(barColor);
                        graphics.FillRectangle(brush, x, y, barWidth, barHeight);
                    }
                }

                DrawErrorMarkers(graphics, drawableHeight);
                DrawBookmarkMarkers(graphics, drawableHeight);

                if (visibleRangeStart.HasValue && visibleRangeEnd.HasValue)
                    DrawVisibleRangeIndicator(graphics, drawableHeight);

                if (hoveredBucketIndex >= 0 && hoveredBucketIndex < bucketCount)
                    DrawTooltip(graphics, sortedBucketKeys[hoveredBucketIndex], totalBarWidth);
                else if (hoveredErrorIndex >= 0 && hoveredErrorIndex < errorLogEntries.Count)
                    DrawErrorTooltip(graphics, errorLogEntries[hoveredErrorIndex]);
            }

            DrawTimeTickMarks(graphics, drawableHeight);

            if (_logRange?.IsConstrained == true)
                DrawMinimapStrip(graphics);
        }

        /// <summary>
        /// Draws the minimap strip at the bottom of the control showing the active range
        /// relative to the full log collection span. Click anywhere on the strip to toggle
        /// between zoomed range view and full timeline view.
        /// </summary>
        private void DrawMinimapStrip(Graphics graphics)
        {
            if (fullSpanMaximum == fullSpanMinimum)
                return;

            int stripY = this.Height - MINIMAP_HEIGHT;
            int stripWidth = this.Width;

            // Background
            using (SolidBrush backgroundBrush = new(MINIMAP_BACKGROUND_COLOR))
                graphics.FillRectangle(backgroundBrush, 0, stripY, stripWidth, MINIMAP_HEIGHT);

            TimeSpan fullSpan = fullSpanMaximum - fullSpanMinimum;

            DateTime rangeBeginTimestamp = _logRange.Begin?.TimeStamp ?? fullSpanMinimum;
            DateTime rangeEndTimestamp = _logRange.End?.TimeStamp ?? fullSpanMaximum;

            float beginX = (float)((rangeBeginTimestamp - fullSpanMinimum).TotalSeconds / fullSpan.TotalSeconds * stripWidth);
            float endX = (float)((rangeEndTimestamp - fullSpanMinimum).TotalSeconds / fullSpan.TotalSeconds * stripWidth);
            float rangeWidth = Math.Max(endX - beginX, 2f);

            // Range fill
            using (SolidBrush rangeBrush = new(MINIMAP_RANGE_COLOR))
                graphics.FillRectangle(rangeBrush, beginX, stripY, rangeWidth, MINIMAP_HEIGHT);

            // Accent lines at range boundaries
            using Pen accentPen = new(MINIMAP_ACCENT_COLOR, MINIMAP_ACCENT_WIDTH);
            graphics.DrawLine(accentPen, beginX, stripY, beginX, this.Height);
            graphics.DrawLine(accentPen, endX, stripY, endX, this.Height);
        }

        /// <summary>
        /// Draws error markers at their exact timestamp positions on top of histogram bars.
        /// </summary>
        private void DrawErrorMarkers(Graphics graphics, int drawableHeight)
        {
            if (buckets.Count == 0)
                return;

            List<DateTime> sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
            int bucketCount = sortedBucketKeys.Count;
            float totalBarWidth = (float)this.Width / bucketCount;

            for (int i = 0; i < errorLogEntries.Count; i++)
            {
                LogEntry errorEntry = errorLogEntries[i];

                int bucketIndex = FindBucketIndexContainingTimestamp(errorEntry.TimeStamp, sortedBucketKeys);

                if (bucketIndex < 0)
                    continue;

                DateTime bucketKey = sortedBucketKeys[bucketIndex];

                float bucketStartX = bucketIndex * totalBarWidth;
                double fractionWithinBucket = (errorEntry.TimeStamp - bucketKey).TotalSeconds / currentBucketSize.TotalSeconds;
                float exactX = bucketStartX + (float)(fractionWithinBucket * totalBarWidth);

                Color markerColor = (i == hoveredErrorIndex) ? ERROR_MARKER_HOVER_COLOR : ERROR_MARKER_COLOR;

                float markerX = exactX - (ERROR_MARKER_SIZE / 2);
                float markerY = drawableHeight - (ERROR_MARKER_SIZE + 2);

                using SolidBrush brush = new(markerColor);
                graphics.FillEllipse(brush, markerX, markerY, ERROR_MARKER_SIZE, ERROR_MARKER_SIZE);
            }
        }

        /// <summary>
        /// Draws bookmark markers one band above error markers at their exact timestamp positions.
        /// Out-of-range bookmarks are drawn faded when showing the full timeline.
        /// </summary>
        private void DrawBookmarkMarkers(Graphics graphics, int drawableHeight)
        {
            if (buckets.Count == 0)
                return;

            List<DateTime> sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
            int bucketCount = sortedBucketKeys.Count;
            float totalBarWidth = (float)this.Width / bucketCount;

            // Draw out-of-range bookmarks first (behind in-range ones) when showing full timeline
            if (_showFullTimeline && _logRange?.IsConstrained == true)
            {
                HashSet<int> inRangeIndices = new(bookmarkLogEntries.Select(entry => entry.Index));

                foreach (LogEntry bookmarkEntry in allBookmarkLogEntries)
                {
                    if (inRangeIndices.Contains(bookmarkEntry.Index))
                        continue;

                    DrawSingleBookmarkMarker(graphics, bookmarkEntry, sortedBucketKeys, totalBarWidth, drawableHeight, BOOKMARK_MARKER_OUT_OF_RANGE_COLOR);
                }
            }

            // Draw in-range bookmarks
            for (int i = 0; i < bookmarkLogEntries.Count; i++)
            {
                Color markerColor = (i == hoveredBookmarkIndex) ? BOOKMARK_MARKER_HOVER_COLOR : BOOKMARK_MARKER_COLOR;
                DrawSingleBookmarkMarker(graphics, bookmarkLogEntries[i], sortedBucketKeys, totalBarWidth, drawableHeight, markerColor);
            }
        }

        private void DrawSingleBookmarkMarker(Graphics graphics, LogEntry bookmarkEntry, List<DateTime> sortedBucketKeys, float totalBarWidth, int drawableHeight, Color markerColor)
        {
            int bucketIndex = FindBucketIndexContainingTimestamp(bookmarkEntry.TimeStamp, sortedBucketKeys);

            if (bucketIndex < 0)
                return;

            DateTime bucketKey = sortedBucketKeys[bucketIndex];

            float bucketStartX = bucketIndex * totalBarWidth;
            double fractionWithinBucket = (bookmarkEntry.TimeStamp - bucketKey).TotalSeconds / currentBucketSize.TotalSeconds;
            float exactX = bucketStartX + (float)(fractionWithinBucket * totalBarWidth);

            float markerX = exactX - (ERROR_MARKER_SIZE / 2);
            float markerY = drawableHeight - (ERROR_MARKER_SIZE * 2 + 4);

            using SolidBrush brush = new(markerColor);
            graphics.FillEllipse(brush, markerX, markerY, ERROR_MARKER_SIZE, ERROR_MARKER_SIZE);
        }

        /// <summary>
        /// Draws a semi-transparent overlay indicating the currently visible range in the log viewer.
        /// </summary>
        private void DrawVisibleRangeIndicator(Graphics graphics, int drawableHeight)
        {
            if (displayedLogEntries.Count == 0 || buckets.Count == 0)
                return;

            List<DateTime> sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
            int bucketCount = sortedBucketKeys.Count;
            float totalBarWidth = (float)this.Width / bucketCount;

            int startBucketIndex = FindBucketIndexContainingTimestamp(visibleRangeStart.Value, sortedBucketKeys);
            int endBucketIndex = FindBucketIndexContainingTimestamp(visibleRangeEnd.Value, sortedBucketKeys);

            if (startBucketIndex < 0 || endBucketIndex < 0)
                return;

            float startX;
            float endX;

            if (startBucketIndex == 0 && visibleRangeStart == minimumTimestamp)
            {
                startX = 0;
            }
            else
            {
                DateTime startBucket = sortedBucketKeys[startBucketIndex];
                float startBucketX = startBucketIndex * totalBarWidth;
                double startFraction = (visibleRangeStart.Value - startBucket).TotalSeconds / currentBucketSize.TotalSeconds;
                startX = startBucketX + (float)(startFraction * totalBarWidth);
            }

            if (endBucketIndex == bucketCount - 1 && visibleRangeEnd == maximumTimestamp)
            {
                endX = this.Width;
            }
            else
            {
                DateTime endBucket = sortedBucketKeys[endBucketIndex];
                float endBucketX = endBucketIndex * totalBarWidth;
                double endFraction = (visibleRangeEnd.Value - endBucket).TotalSeconds / currentBucketSize.TotalSeconds;
                endX = endBucketX + (float)(endFraction * totalBarWidth);
            }

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

        private int FindBucketIndexContainingTimestamp(DateTime timestamp, List<DateTime> sortedBucketKeys)
        {
            for (int i = 0; i < sortedBucketKeys.Count; i++)
            {
                DateTime bucketStart = sortedBucketKeys[i];
                DateTime bucketEnd = bucketStart.Add(currentBucketSize);

                if (timestamp >= bucketStart && timestamp < bucketEnd)
                    return i;
            }

            if (timestamp < sortedBucketKeys[0])
                return 0;
            if (timestamp >= sortedBucketKeys[^1].Add(currentBucketSize))
                return sortedBucketKeys.Count - 1;

            return -1;
        }

        /// <summary>
        /// Draws time tick marks along the x-axis of the histogram area.
        /// </summary>
        private void DrawTimeTickMarks(Graphics graphics, int drawableHeight)
        {
            using Pen tickPen = new(Color.FromArgb(150, 180, 180, 180), 1);
            using Font font = new("Segoe UI", 7);
            using SolidBrush brush = new(Color.FromArgb(120, 120, 120));
            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;

            int tickCount = Math.Max(2, this.Width / 65);

            for (int i = 0; i <= tickCount; i++)
            {
                float x = (float)i / tickCount * this.Width;

                TimeSpan offset = TimeSpan.FromSeconds(totalSpan.TotalSeconds * i / tickCount);
                DateTime tickTime = minimumTimestamp.Add(offset);

                string label = FormatTickLabelBySpan(tickTime, totalSpan);
                SizeF labelSize = graphics.MeasureString(label, font);

                float labelX = x - labelSize.Width / 2;

                if (i == 0) labelX = 0;
                else if (i == tickCount) labelX = this.Width - labelSize.Width;

                graphics.DrawString(label, font, brush, labelX, 0);
            }
        }

        private static string FormatTickLabelBySpan(DateTime timestamp, TimeSpan totalSpan)
        {
            if (totalSpan.TotalDays >= 2)
                return timestamp.ToString("MMM-dd");
            else if (totalSpan.TotalDays >= 1)
                return timestamp.ToString("d HH:mm");
            else if (totalSpan.TotalHours >= 1)
                return timestamp.ToString("HH:mm");
            else if (totalSpan.TotalMinutes >= 1)
                return timestamp.ToString("H:mm:ss");
            else if (totalSpan.TotalSeconds >= 5)
                return timestamp.ToString("H:mm:ss");
            else if (totalSpan.TotalSeconds >= 1)
                return timestamp.ToString("s.ff") + " s";
            else
                return timestamp.ToString("s.fff") + " s";
        }

        private void DrawTooltip(Graphics graphics, DateTime bucketKey, float barWidth)
        {
            List<LogEntry> bucketEntries = buckets[bucketKey];
            int entryCount = bucketEntries.Count;

            string tooltipText;
            if (entryCount == 0)
            {
                tooltipText = $"0 events at {bucketKey:yyyy-MM-dd HH:mm:ss}";
            }
            else
            {
                LogEntry firstEntry = bucketEntries[0];
                string eventText = entryCount == 1 ? "event" : "events";
                tooltipText = $"{entryCount} {eventText} at {firstEntry.TimeStamp:yyyy-MM-dd HH:mm:ss}";
            }

            using Font font = new("Segoe UI", 9);
            SizeF textSize = graphics.MeasureString(tooltipText, font);

            float tooltipWidth = textSize.Width + 2;
            float tooltipHeight = textSize.Height;

            float tooltipX = (hoveredBucketIndex * barWidth) + (barWidth / 2) - (tooltipWidth / 2);
            float tooltipY = 2;

            if (tooltipX < 5) tooltipX = 5;
            if (tooltipX + tooltipWidth > this.Width - 5)
                tooltipX = this.Width - tooltipWidth - 5;

            using (SolidBrush backgroundBrush = new(Color.FromArgb(150, 50, 50, 50)))
            using (Pen borderPen = new(Color.FromArgb(100, 100, 100), 1))
            {
                graphics.FillRectangle(backgroundBrush, tooltipX, tooltipY, tooltipWidth, tooltipHeight);
                graphics.DrawRectangle(borderPen, tooltipX, tooltipY, tooltipWidth, tooltipHeight);
            }

            using SolidBrush textBrush = new(Color.White);
            graphics.DrawString(tooltipText, font, textBrush, tooltipX, tooltipY);
        }

        private void DrawErrorTooltip(Graphics graphics, LogEntry errorEntry)
        {
            string tooltipText = $"Error at {errorEntry.TimeStamp:yyyy-MM-dd HH:mm:ss}";

            using Font font = new("Segoe UI", 9);
            SizeF textSize = graphics.MeasureString(tooltipText, font);

            float tooltipWidth = textSize.Width + 16;
            float tooltipHeight = textSize.Height + 4;

            float xPosition = GetXPositionForTimestamp(errorEntry.TimeStamp);
            float tooltipX = xPosition - (tooltipWidth / 2);
            float tooltipY = 20;

            if (tooltipX < 5) tooltipX = 5;
            if (tooltipX + tooltipWidth > this.Width - 5)
                tooltipX = this.Width - tooltipWidth - 5;

            using (SolidBrush backgroundBrush = new(Color.FromArgb(240, 80, 20, 20)))
            using (Pen borderPen = new(ERROR_MARKER_COLOR, 1))
            {
                graphics.FillRectangle(backgroundBrush, tooltipX, tooltipY, tooltipWidth, tooltipHeight);
                graphics.DrawRectangle(borderPen, tooltipX, tooltipY, tooltipWidth, tooltipHeight);
            }

            using SolidBrush textBrush = new(Color.White);
            graphics.DrawString(tooltipText, font, textBrush, tooltipX + 8, tooltipY + 2);
        }

        #endregion

        #region Mouse Interaction

        /// <summary>
        /// Handles mouse movement for hover effects, including minimap strip hover.
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // Minimap strip hover — handle before anything else
            if (IsInMinimapStrip(e.Y))
            {
                if (!hoveredMinimap)
                {
                    hoveredMinimap = true;
                    this.Invalidate();
                }

                string tooltipText = _showFullTimeline
                    ? "Klik om in te zoomen op het bereik"
                    : "Klik om het volledige tijdlijn te tonen";

                toolTip.SetToolTip(this, tooltipText);

                if (this.Cursor != Cursors.Hand)
                    this.Cursor = Cursors.Hand;

                return;
            }

            if (hoveredMinimap)
            {
                hoveredMinimap = false;
                toolTip.SetToolTip(this, string.Empty);
                this.Cursor = Cursors.Default;
                this.Invalidate();
            }

            if (buckets.Count == 0)
            {
                if (this.Cursor != Cursors.Default)
                    this.Cursor = Cursors.Default;
                return;
            }

            bool cursorChanged = false;

            // Check for bookmark marker hover
            int newHoveredBookmarkIndex = GetBookmarkMarkerIndexAtPosition(e.X, e.Y);

            if (newHoveredBookmarkIndex != -1)
            {
                if (newHoveredBookmarkIndex != hoveredBookmarkIndex)
                {
                    hoveredBookmarkIndex = newHoveredBookmarkIndex;
                    hoveredErrorIndex = -1;
                    hoveredBucketIndex = -1;
                    this.Invalidate();
                }

                if (this.Cursor != Cursors.Hand)
                    this.Cursor = Cursors.Hand;

                cursorChanged = true;
            }
            else
            {
                if (hoveredBookmarkIndex != -1)
                {
                    hoveredBookmarkIndex = -1;
                    this.Invalidate();
                }

                // Check for error marker hover
                int newHoveredErrorIndex = GetErrorMarkerIndexAtPosition(e.X, e.Y);

                if (newHoveredErrorIndex != -1)
                {
                    if (newHoveredErrorIndex != hoveredErrorIndex)
                    {
                        hoveredErrorIndex = newHoveredErrorIndex;
                        hoveredBucketIndex = -1;
                        this.Invalidate();
                    }

                    if (this.Cursor != Cursors.Hand)
                        this.Cursor = Cursors.Hand;

                    cursorChanged = true;
                }
                else
                {
                    if (hoveredErrorIndex != -1)
                    {
                        hoveredErrorIndex = -1;
                        this.Invalidate();
                    }

                    // Check for bucket hover
                    int drawableWidth = this.Width;
                    List<DateTime> sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
                    int bucketCount = sortedBucketKeys.Count;
                    float barWidth = (float)drawableWidth / bucketCount;

                    int newHoveredIndex = (int)(e.X / barWidth);

                    if (newHoveredIndex >= 0 && newHoveredIndex < bucketCount)
                    {
                        DateTime bucketKey = sortedBucketKeys[newHoveredIndex];
                        int entryCount = buckets[bucketKey].Count;
                        bool hasEntries = entryCount > 0;

                        if (newHoveredIndex != hoveredBucketIndex)
                        {
                            hoveredBucketIndex = newHoveredIndex;
                            this.Invalidate();
                        }

                        if (hasEntries && this.Cursor != Cursors.Hand)
                            this.Cursor = Cursors.Hand;
                        else if (!hasEntries && this.Cursor != Cursors.Default)
                            this.Cursor = Cursors.Default;

                        cursorChanged = true;
                    }
                }
            }

            if (!cursorChanged)
            {
                if (hoveredBucketIndex != -1)
                {
                    hoveredBucketIndex = -1;
                    this.Invalidate();
                }
                if (this.Cursor != Cursors.Default)
                    this.Cursor = Cursors.Default;
            }
        }

        private int GetBookmarkMarkerIndexAtPosition(int mouseX, int mouseY)
        {
            if (buckets.Count == 0)
                return -1;

            List<DateTime> sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
            int bucketCount = sortedBucketKeys.Count;
            float totalBarWidth = (float)this.Width / bucketCount;
            int drawableHeight = GetHistogramHeight();

            for (int i = 0; i < bookmarkLogEntries.Count; i++)
            {
                LogEntry bookmarkEntry = bookmarkLogEntries[i];

                int bucketIndex = FindBucketIndexContainingTimestamp(bookmarkEntry.TimeStamp, sortedBucketKeys);

                if (bucketIndex < 0)
                    continue;

                DateTime bucketKey = sortedBucketKeys[bucketIndex];

                float bucketStartX = bucketIndex * totalBarWidth;
                double fractionWithinBucket = (bookmarkEntry.TimeStamp - bucketKey).TotalSeconds / currentBucketSize.TotalSeconds;
                float exactX = bucketStartX + (float)(fractionWithinBucket * totalBarWidth);

                float markerX = exactX - (ERROR_MARKER_SIZE / 2);
                float markerY = drawableHeight - (ERROR_MARKER_SIZE * 2 + 4);

                if (mouseX >= markerX && mouseX <= markerX + ERROR_MARKER_SIZE &&
                    mouseY >= markerY && mouseY <= markerY + ERROR_MARKER_SIZE)
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetErrorMarkerIndexAtPosition(int mouseX, int mouseY)
        {
            if (buckets.Count == 0)
                return -1;

            List<DateTime> sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
            int bucketCount = sortedBucketKeys.Count;
            float totalBarWidth = (float)this.Width / bucketCount;
            int drawableHeight = GetHistogramHeight();

            for (int i = 0; i < errorLogEntries.Count; i++)
            {
                LogEntry errorEntry = errorLogEntries[i];

                int bucketIndex = FindBucketIndexContainingTimestamp(errorEntry.TimeStamp, sortedBucketKeys);

                if (bucketIndex < 0)
                    continue;

                DateTime bucketKey = sortedBucketKeys[bucketIndex];

                float bucketStartX = bucketIndex * totalBarWidth;
                double fractionWithinBucket = (errorEntry.TimeStamp - bucketKey).TotalSeconds / currentBucketSize.TotalSeconds;
                float exactX = bucketStartX + (float)(fractionWithinBucket * totalBarWidth);

                float markerX = exactX - (ERROR_MARKER_SIZE / 2);
                float markerY = drawableHeight - (ERROR_MARKER_SIZE + 2);

                if (mouseX >= markerX && mouseX <= markerX + ERROR_MARKER_SIZE &&
                    mouseY >= markerY && mouseY <= markerY + ERROR_MARKER_SIZE)
                {
                    return i;
                }
            }

            return -1;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            if (hoveredBucketIndex != -1)
            {
                hoveredBucketIndex = -1;
                this.Invalidate();
            }
            if (hoveredErrorIndex != -1)
            {
                hoveredErrorIndex = -1;
                this.Invalidate();
            }
            if (hoveredBookmarkIndex != -1)
            {
                hoveredBookmarkIndex = -1;
                this.Invalidate();
            }
            if (hoveredMinimap)
            {
                hoveredMinimap = false;
                toolTip.SetToolTip(this, string.Empty);
                this.Invalidate();
            }
            if (this.Cursor != Cursors.Default)
                this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles mouse clicks on the minimap strip, histogram bars, error markers, and bookmark markers.
        /// </summary>
        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            // Minimap strip click — toggle between zoomed range and full timeline
            if (IsInMinimapStrip(e.Y))
            {
                _showFullTimeline = !_showFullTimeline;
                RebuildDisplayedEntries();
                errorLogEntries = [.. displayedLogEntries.Where(entry => entry.IsErrorLogEntry)];
                this.Invalidate();
                return;
            }

            if (buckets.Count == 0)
                return;

            // Check for bookmark marker click
            int clickedBookmarkIndex = GetBookmarkMarkerIndexAtPosition(e.X, e.Y);

            if (clickedBookmarkIndex != -1)
            {
                BookmarkMarkerClicked?.Invoke(this, bookmarkLogEntries[clickedBookmarkIndex]);
                return;
            }

            // Check for error marker click
            int clickedErrorIndex = GetErrorMarkerIndexAtPosition(e.X, e.Y);

            if (clickedErrorIndex != -1)
            {
                ErrorMarkerClicked?.Invoke(this, errorLogEntries[clickedErrorIndex]);
                return;
            }

            // Check for bucket click
            int drawableWidth = this.Width;
            List<DateTime> sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
            int bucketCount = sortedBucketKeys.Count;
            float barWidth = (float)drawableWidth / bucketCount;

            int clickedBucketIndex = (int)(e.X / barWidth);

            if (clickedBucketIndex >= 0 && clickedBucketIndex < bucketCount)
            {
                DateTime bucketKey = sortedBucketKeys[clickedBucketIndex];
                List<LogEntry> bucketEntries = buckets[bucketKey];

                if (bucketEntries.Count > 0)
                {
                    LogEntry firstEntry = bucketEntries[0];
                    CellClicked?.Invoke(this, firstEntry);
                }
            }
        }

        #endregion
    }
}