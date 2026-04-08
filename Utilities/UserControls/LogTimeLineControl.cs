using System;
using System.Collections.Generic;
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

        // Cached sorted bucket keys — rebuilt in RecalculateBuckets, shared across all drawing and hit-testing
        private List<DateTime> sortedBucketKeys = [];
        private float totalBarWidth;

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
        private const int MARKER_SIZE = 8;
        private const int MINIMUM_VISIBLE_RANGE_WIDTH = 1;
        private const int MINIMAP_HEIGHT = 5;
        private const int MINIMAP_ACCENT_WIDTH = 2;
        private const int BOOKMARK_MARKER_Y_OFFSET = MARKER_SIZE * 2 + 1;
        private const int ERROR_MARKER_Y_OFFSET = MARKER_SIZE + 2;

        // Colors
        private static readonly Color BAR_COLOR = Color.LightGray;
        private static readonly Color BAR_HOVER_COLOR = Color.FromArgb(100, 160, 210);
        private static readonly Color VISIBLE_RANGE_COLOR = Color.FromArgb(80, 100, 160, 210);
        private static readonly Color VISIBLE_RANGE_BORDER_COLOR = Color.FromArgb(150, 100, 160, 210);
        private static readonly Color ERROR_MARKER_COLOR = Color.FromArgb(220, 50, 50);
        private static readonly Color ERROR_MARKER_HOVER_COLOR = Color.FromArgb(255, 80, 80);
        private static readonly Color BOOKMARK_MARKER_COLOR = Color.SteelBlue;
        private static readonly Color BOOKMARK_MARKER_HOVER_COLOR = Color.FromArgb(255, 120, 180, 230);
        private static readonly Color BOOKMARK_MARKER_OUT_OF_RANGE_COLOR = Color.FromArgb(100, 70, 130, 180);
        private static readonly Color MINIMAP_BACKGROUND_COLOR = Color.FromArgb(230, 230, 230);
        private static readonly Color MINIMAP_RANGE_COLOR = Color.FromArgb(153, 70, 130, 180);
        private static readonly Color MINIMAP_ACCENT_COLOR = Color.SteelBlue;

        // Tooltip
        private readonly ToolTip toolTip = new();

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
            this.Resize += (s, e) => { RebuildDisplayedEntries(); this.Invalidate(); };
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
                sortedBucketKeys.Clear();
                errorLogEntries.Clear();
                this.Invalidate();
                return;
            }

            bool isAppend = false;

            if (allLogEntries.Count > 0 && entries.Count > allLogEntries.Count)
            {
                if (allLogEntries[0].TimeStamp == entries[0].TimeStamp &&
                    allLogEntries[^1].TimeStamp == entries[allLogEntries.Count - 1].TimeStamp)
                    isAppend = true;
            }

            if (isAppend)
                allLogEntries.AddRange(entries.Skip(allLogEntries.Count));
            else
                allLogEntries = [.. entries];

            RebuildDisplayedEntries();
            errorLogEntries = [.. displayedLogEntries.Where(entry => entry.IsErrorLogEntry)];

            this.Invalidate();
        }

        /// <summary>Updates the bookmark entries displayed in the timeline.</summary>
        public void SetBookmarks(List<LogEntry> bookmarks)
        {
            allBookmarkLogEntries = bookmarks ?? [];
            RebuildFilteredBookmarkMarkers();
            this.Invalidate();
        }

        /// <summary>Updates the log range used to filter the histogram and bookmark markers.</summary>
        public void SetLogRange(LogRange logRange)
        {
            _logRange = logRange;
            RebuildDisplayedEntries();
            RebuildFilteredBookmarkMarkers();
            errorLogEntries = [.. displayedLogEntries.Where(entry => entry.IsErrorLogEntry)];
            this.Invalidate();
        }

        /// <summary>Sets the visible range of log entries currently shown in the log viewer.</summary>
        public void SetVisibleRange(DateTime startTimestamp, DateTime endTimestamp)
        {
            visibleRangeStart = startTimestamp;
            visibleRangeEnd = endTimestamp;
            this.Invalidate();
        }

        /// <summary>Clears the visible range indicator.</summary>
        public void ClearVisibleRange()
        {
            SetLogRange(new LogRange());
            visibleRangeStart = null;
            visibleRangeEnd = null;
            this.Invalidate();
        }

        #endregion

        #region Entry Filtering

        /// <summary>
        /// Rebuilds the displayed entry set based on the current range and show-full-timeline state.
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
        /// Recalculates time buckets and rebuilds the cached sorted bucket key list.
        /// </summary>
        private void RecalculateBuckets()
        {
            buckets.Clear();
            sortedBucketKeys.Clear();

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

            int desiredBucketCount = Math.Max(MINIMUM_BUCKET_COUNT, this.Width / 10);
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

            // Cache sorted keys and bar width — used by all drawing and hit-testing
            sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
            totalBarWidth = sortedBucketKeys.Count > 0 ? (float)this.Width / sortedBucketKeys.Count : 0;
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

        /// <summary>Converts a timestamp to an x-coordinate using the displayed entry span.</summary>
        private float GetXPositionForTimestamp(DateTime timestamp)
        {
            if (displayedLogEntries.Count == 0)
                return 0;

            if (timestamp < minimumTimestamp || timestamp > maximumTimestamp)
                return -1;

            double percentage = (timestamp - minimumTimestamp).TotalSeconds / (maximumTimestamp - minimumTimestamp).TotalSeconds;
            return (float)(percentage * this.Width);
        }

        /// <summary>Converts a timestamp to an x-coordinate using the full collection span.</summary>
        private float GetXPositionForTimestampInFullSpan(DateTime timestamp)
        {
            if (fullSpanMaximum == fullSpanMinimum)
                return 0;

            double percentage = (timestamp - fullSpanMinimum).TotalSeconds / (fullSpanMaximum - fullSpanMinimum).TotalSeconds;
            return (float)(Math.Clamp(percentage, 0.0, 1.0) * this.Width);
        }

        private int GetHistogramHeight()
        {
            return _logRange?.IsConstrained == true
                ? this.Height - MINIMAP_HEIGHT
                : this.Height;
        }

        private bool IsInMinimapStrip(int y)
        {
            return _logRange?.IsConstrained == true && y >= this.Height - MINIMAP_HEIGHT;
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

        /// <summary>Returns the index of the marker in the given list whose hit rect contains the mouse position, or -1.</summary>
        private int GetMarkerIndexAtPosition(int mouseX, int mouseY, List<LogEntry> entries, int yOffset)
        {
            if (sortedBucketKeys.Count == 0)
                return -1;

            int drawableHeight = GetHistogramHeight();

            for (int i = 0; i < entries.Count; i++)
            {
                float centerX = GetMarkerXPosition(entries[i]);
                if (centerX < 0)
                    continue;

                float markerX = centerX - (MARKER_SIZE / 2);
                float markerY = drawableHeight - yOffset;

                if (mouseX >= markerX && mouseX <= markerX + MARKER_SIZE &&
                    mouseY >= markerY && mouseY <= markerY + MARKER_SIZE)
                    return i;
            }

            return -1;
        }

        #endregion

        #region Painting

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.Clear(this.BackColor);

            int drawableWidth = this.Width;
            int drawableHeight = GetHistogramHeight();

            if (drawableWidth <= 0 || drawableHeight <= 0)
                return;

            if (minimumTimestamp == default && maximumTimestamp == default)
                return;

            DrawRangeOverlay(graphics, drawableWidth, drawableHeight);
            DrawHistogramBars(graphics, drawableHeight);
            DrawErrorMarkers(graphics, drawableHeight);
            DrawBookmarkMarkers(graphics, drawableHeight);
            DrawVisibleRangeIndicator(graphics, drawableHeight);
            DrawActiveTooltip(graphics);
            DrawTimeTickMarks(graphics);
            DrawMinimapStrip(graphics);
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Dims everything outside the log range when the full timeline is shown.
        /// </summary>
        private void DrawRangeOverlay(Graphics graphics, int drawableWidth, int drawableHeight)
        {
            if (_logRange?.IsConstrained != true || !_showFullTimeline)
                return;

            float rangeStartX = _logRange.Begin != null ? GetXPositionForTimestampInFullSpan(_logRange.Begin.TimeStamp) : 0;
            float rangeEndX = _logRange.End != null ? GetXPositionForTimestampInFullSpan(_logRange.End.TimeStamp) : drawableWidth;

            using SolidBrush dimBrush = new(Color.FromArgb(60, 0, 0, 0));

            if (rangeStartX > 0)
                graphics.FillRectangle(dimBrush, 0, 0, rangeStartX, drawableHeight);
            if (rangeEndX < drawableWidth)
                graphics.FillRectangle(dimBrush, rangeEndX, 0, drawableWidth - rangeEndX, drawableHeight);
        }

        /// <summary>
        /// Draws the histogram bars for all non-empty buckets.
        /// </summary>
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

        /// <summary>
        /// Draws the tooltip for whichever element is currently hovered.
        /// </summary>
        private void DrawActiveTooltip(Graphics graphics)
        {
            if (hoveredBucketIndex >= 0 && hoveredBucketIndex < sortedBucketKeys.Count)
                DrawTooltip(graphics, sortedBucketKeys[hoveredBucketIndex]);
            else if (hoveredErrorIndex >= 0 && hoveredErrorIndex < errorLogEntries.Count)
                DrawErrorTooltip(graphics, errorLogEntries[hoveredErrorIndex]);
        }

        private void DrawMinimapStrip(Graphics graphics)
        {
            if (_logRange == null || !_logRange.IsConstrained) return;
            if (fullSpanMaximum == fullSpanMinimum) return;

            int stripY = this.Height - MINIMAP_HEIGHT;
            int stripWidth = this.Width;

            using (SolidBrush backgroundBrush = new(MINIMAP_BACKGROUND_COLOR))
                graphics.FillRectangle(backgroundBrush, 0, stripY, stripWidth, MINIMAP_HEIGHT);

            TimeSpan fullSpan = fullSpanMaximum - fullSpanMinimum;
            DateTime rangeBeginTimestamp = _logRange.Begin?.TimeStamp ?? fullSpanMinimum;
            DateTime rangeEndTimestamp = _logRange.End?.TimeStamp ?? fullSpanMaximum;

            float beginX = (float)((rangeBeginTimestamp - fullSpanMinimum).TotalSeconds / fullSpan.TotalSeconds * stripWidth);
            float endX = (float)((rangeEndTimestamp - fullSpanMinimum).TotalSeconds / fullSpan.TotalSeconds * stripWidth);
            float rangeWidth = Math.Max(endX - beginX, 2f);

            using (SolidBrush rangeBrush = new(MINIMAP_RANGE_COLOR))
                graphics.FillRectangle(rangeBrush, beginX, stripY, rangeWidth, MINIMAP_HEIGHT);

            using Pen accentPen = new(MINIMAP_ACCENT_COLOR, MINIMAP_ACCENT_WIDTH);
            graphics.DrawLine(accentPen, beginX, stripY, beginX, this.Height);
            graphics.DrawLine(accentPen, endX, stripY, endX, this.Height);
        }

        private void DrawErrorMarkers(Graphics graphics, int drawableHeight)
        {
            for (int i = 0; i < errorLogEntries.Count; i++)
            {
                float centerX = GetMarkerXPosition(errorLogEntries[i]);
                if (centerX < 0) continue;

                Color color = (i == hoveredErrorIndex) ? ERROR_MARKER_HOVER_COLOR : ERROR_MARKER_COLOR;
                DrawMarkerEllipse(graphics, centerX, drawableHeight, ERROR_MARKER_Y_OFFSET, color);
            }
        }

        /// <summary>
        /// Draws bookmark markers one band above error markers.
        /// Out-of-range bookmarks are drawn faded when showing the full timeline.
        /// </summary>
        private void DrawBookmarkMarkers(Graphics graphics, int drawableHeight)
        {
            // Draw out-of-range bookmarks first (behind in-range ones)
            if (_showFullTimeline && _logRange?.IsConstrained == true)
            {
                HashSet<int> inRangeIndices = [.. bookmarkLogEntries.Select(entry => entry.Index)];

                foreach (LogEntry bookmarkEntry in allBookmarkLogEntries)
                {
                    if (inRangeIndices.Contains(bookmarkEntry.Index))
                        continue;

                    float centerX = GetMarkerXPosition(bookmarkEntry);
                    if (centerX < 0) continue;

                    DrawMarkerEllipse(graphics, centerX, drawableHeight, BOOKMARK_MARKER_Y_OFFSET, BOOKMARK_MARKER_OUT_OF_RANGE_COLOR);
                }
            }

            // Draw in-range bookmarks on top
            for (int i = 0; i < bookmarkLogEntries.Count; i++)
            {
                float centerX = GetMarkerXPosition(bookmarkLogEntries[i]);
                if (centerX < 0) continue;

                Color color = (i == hoveredBookmarkIndex) ? BOOKMARK_MARKER_HOVER_COLOR : BOOKMARK_MARKER_COLOR;
                DrawMarkerEllipse(graphics, centerX, drawableHeight, BOOKMARK_MARKER_Y_OFFSET, color);
            }
        }

        private void DrawVisibleRangeIndicator(Graphics graphics, int drawableHeight)
        {
            if (!visibleRangeStart.HasValue || !visibleRangeEnd.HasValue)
                return;

            if (sortedBucketKeys.Count == 0)
                return;

            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;
            if (totalSpan.TotalSeconds <= 0)
                return;

            float startX = (float)((visibleRangeStart.Value - minimumTimestamp).TotalSeconds / totalSpan.TotalSeconds * this.Width);
            float endX = (float)((visibleRangeEnd.Value - minimumTimestamp).TotalSeconds / totalSpan.TotalSeconds * this.Width);

            startX = Math.Clamp(startX, 0, this.Width);
            endX = Math.Clamp(endX, 0, this.Width);

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

        private void DrawTimeTickMarks(Graphics graphics)
        {
            using Font font = new("Segoe UI", 7);
            using SolidBrush brush = new(Color.FromArgb(120, 120, 120));

            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;
            int tickCount = Math.Max(2, this.Width / 65);

            for (int i = 0; i <= tickCount; i++)
            {
                float x = (float)i / tickCount * this.Width;
                DateTime tickTime = minimumTimestamp.Add(TimeSpan.FromSeconds(totalSpan.TotalSeconds * i / tickCount));
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
            if (totalSpan.TotalDays >= 2) return timestamp.ToString("MMM-dd");
            if (totalSpan.TotalDays >= 1) return timestamp.ToString("d HH:mm");
            if (totalSpan.TotalHours >= 1) return timestamp.ToString("HH:mm");
            if (totalSpan.TotalMinutes >= 1) return timestamp.ToString("H:mm:ss");
            if (totalSpan.TotalSeconds >= 1) return timestamp.ToString("s.ff") + " s";
            return timestamp.ToString("s.fff") + " s";
        }

        private void DrawTooltip(Graphics graphics, DateTime bucketKey)
        {
            List<LogEntry> bucketEntries = buckets[bucketKey];
            int entryCount = bucketEntries.Count;

            string tooltipText = entryCount == 0
                ? $"0 events at {bucketKey:yyyy-MM-dd HH:mm:ss}"
                : $"{entryCount} {(entryCount == 1 ? "event" : "events")} at {bucketEntries[0].TimeStamp:yyyy-MM-dd HH:mm:ss}";

            using Font font = new("Segoe UI", 9);
            SizeF textSize = graphics.MeasureString(tooltipText, font);

            float tooltipX = hoveredBucketIndex * totalBarWidth + totalBarWidth / 2 - textSize.Width / 2;
            float tooltipY = 2;

            tooltipX = Math.Clamp(tooltipX, 5, this.Width - textSize.Width - 5);

            using (SolidBrush backgroundBrush = new(Color.FromArgb(150, 50, 50, 50)))
            using (Pen borderPen = new(Color.FromArgb(100, 100, 100), 1))
            {
                graphics.FillRectangle(backgroundBrush, tooltipX, tooltipY, textSize.Width, textSize.Height);
                graphics.DrawRectangle(borderPen, tooltipX, tooltipY, textSize.Width, textSize.Height);
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
            float tooltipX = Math.Clamp(xPosition - tooltipWidth / 2, 5, this.Width - tooltipWidth - 5);
            float tooltipY = 20;

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

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // Minimap strip — highest priority
            if (IsInMinimapStrip(e.Y))
            {
                SetHoveredMinimap(true);
                return;
            }

            SetHoveredMinimap(false);

            if (sortedBucketKeys.Count == 0)
            {
                SetCursor(Cursors.Default);
                return;
            }

            // Bookmark marker hover
            int newBookmark = GetMarkerIndexAtPosition(e.X, e.Y, bookmarkLogEntries, BOOKMARK_MARKER_Y_OFFSET);
            if (UpdateHoveredMarker(ref hoveredBookmarkIndex, newBookmark, ref hoveredErrorIndex, ref hoveredBucketIndex))
            {
                SetCursor(Cursors.Hand);
                return;
            }

            // Error marker hover
            int newError = GetMarkerIndexAtPosition(e.X, e.Y, errorLogEntries, ERROR_MARKER_Y_OFFSET);
            if (UpdateHoveredMarker(ref hoveredErrorIndex, newError, ref hoveredBookmarkIndex, ref hoveredBucketIndex))
            {
                SetCursor(Cursors.Hand);
                return;
            }

            // Bucket hover
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

            // Nothing hovered
            ClearHoveredMarker(ref hoveredBucketIndex);
            ClearHoveredMarker(ref hoveredErrorIndex);
            ClearHoveredMarker(ref hoveredBookmarkIndex);
            SetCursor(Cursors.Default);
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

        private void SetHoveredMinimap(bool hovered)
        {
            if (hovered == hoveredMinimap)
                return;

            hoveredMinimap = hovered;

            if (hovered)
            {
                string tooltipText = _showFullTimeline
                    ? "Klik om in te zoomen op het bereik"
                    : "Klik om het volledige tijdlijn te tonen";

                toolTip.SetToolTip(this, tooltipText);
                SetCursor(Cursors.Hand);
            }
            else
            {
                toolTip.SetToolTip(this, string.Empty);
                SetCursor(Cursors.Default);
            }

            this.Invalidate();
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            ClearHoveredMarker(ref hoveredBucketIndex);
            ClearHoveredMarker(ref hoveredErrorIndex);
            ClearHoveredMarker(ref hoveredBookmarkIndex);
            SetHoveredMinimap(false);
            SetCursor(Cursors.Default);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (IsInMinimapStrip(e.Y))
            {
                _showFullTimeline = !_showFullTimeline;
                RebuildDisplayedEntries();
                errorLogEntries = [.. displayedLogEntries.Where(entry => entry.IsErrorLogEntry)];
                this.Invalidate();
                return;
            }

            if (sortedBucketKeys.Count == 0)
                return;

            int clickedBookmark = GetMarkerIndexAtPosition(e.X, e.Y, bookmarkLogEntries, BOOKMARK_MARKER_Y_OFFSET);
            if (clickedBookmark != -1)
            {
                BookmarkMarkerClicked?.Invoke(this, bookmarkLogEntries[clickedBookmark]);
                return;
            }

            int clickedError = GetMarkerIndexAtPosition(e.X, e.Y, errorLogEntries, ERROR_MARKER_Y_OFFSET);
            if (clickedError != -1)
            {
                ErrorMarkerClicked?.Invoke(this, errorLogEntries[clickedError]);
                return;
            }

            int clickedBucketIndex = (int)(e.X / totalBarWidth);
            if (clickedBucketIndex >= 0 && clickedBucketIndex < sortedBucketKeys.Count)
            {
                List<LogEntry> bucketEntries = buckets[sortedBucketKeys[clickedBucketIndex]];
                if (bucketEntries.Count > 0)
                    CellClicked?.Invoke(this, bucketEntries[0]);
            }
        }

        #endregion
    }
}