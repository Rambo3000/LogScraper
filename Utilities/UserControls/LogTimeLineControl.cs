using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log;

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
        private readonly Dictionary<DateTime, List<LogEntry>> buckets = [];
        private List<LogEntry> errorLogEntries = [];

        // Bucketing and scaling
        private TimeSpan currentBucketSize;
        private DateTime minimumTimestamp;
        private DateTime maximumTimestamp;
        private double maximumRawValue;

        // Hover tracking
        private int hoveredBucketIndex = -1;
        private int hoveredErrorIndex = -1;

        // Visible range tracking (timestamp-based to match timeline bars)
        private DateTime? visibleRangeStart = null;
        private DateTime? visibleRangeEnd = null;

        // Constants
        private const double SCALE_POWER = 0.4;
        private const int MINIMUM_BUCKET_COUNT = 50;
        private const int ERROR_MARKER_SIZE = 8;
        private const int MINIMUM_VISIBLE_RANGE_WIDTH = 1; // Minimum width in pixels for visibility

        // Colors
        private static readonly Color BAR_COLOR = Color.LightGray;
        private static readonly Color BAR_HOVER_COLOR = Color.FromArgb(100, 160, 210);
        private static readonly Color LABEL_COLOR = Color.FromArgb(100, 100, 100);
        private static readonly Color VISIBLE_RANGE_COLOR = Color.FromArgb(80, 100, 160, 210);
        private static readonly Color VISIBLE_RANGE_BORDER_COLOR = Color.FromArgb(150, 100, 160, 210);
        private static readonly Color ERROR_MARKER_COLOR = Color.FromArgb(220, 50, 50);
        private static readonly Color ERROR_MARKER_HOVER_COLOR = Color.FromArgb(255, 80, 80);

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
        /// </summary>
        public void UpdateLogEntries(List<LogEntry> entries)
        {
            if (entries == null || entries.Count == 0)
            {
                allLogEntries.Clear();
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
                {
                    isAppend = true;
                }
            }

            if (isAppend)
            {
                // Only add new entries
                List<LogEntry> newEntries = [.. entries.Skip(allLogEntries.Count)];
                allLogEntries.AddRange(newEntries);
                RecalculateBuckets();
            }
            else
            {
                // Full replacement
                allLogEntries = [.. entries];
                RecalculateBuckets();
            }

            // Update error entries list
            errorLogEntries = [.. allLogEntries.Where(entry => entry.IsErrorLogEntry)];

            this.Invalidate();
        }

        /// <summary>
        /// Sets the visible range of log entries currently shown in the log viewer.
        /// Uses timestamps to accurately match the time-based timeline visualization.
        /// </summary>
        /// <param name="startTimestamp">Timestamp of first visible log entry</param>
        /// <param name="endTimestamp">Timestamp of last visible log entry</param>
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

        #region Bucketing Logic

        /// <summary>
        /// Recalculates time buckets for the histogram based on current log entries.
        /// </summary>
        private void RecalculateBuckets()
        {
            buckets.Clear();

            if (allLogEntries.Count == 0)
                return;

            minimumTimestamp = allLogEntries[0].TimeStamp;
            maximumTimestamp = allLogEntries[^1].TimeStamp;

            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;
            if (totalSpan.TotalSeconds < 1)
                totalSpan = TimeSpan.FromSeconds(1);

            // Calculate bucket size based on available width
            int availableWidth = this.Width;
            int desiredBucketCount = Math.Max(MINIMUM_BUCKET_COUNT, availableWidth / 10);
            currentBucketSize = CalculateOptimalBucketSize(totalSpan, desiredBucketCount);

            // Create bucket structure
            DateTime bucketStart = RoundDownToNearestBucket(minimumTimestamp, currentBucketSize);
            DateTime bucketEnd = RoundDownToNearestBucket(maximumTimestamp, currentBucketSize).Add(currentBucketSize);

            DateTime currentBucket = bucketStart;
            while (currentBucket < bucketEnd)
            {
                buckets[currentBucket] = [];
                currentBucket = currentBucket.Add(currentBucketSize);
            }

            // Distribute log entries into buckets
            foreach (LogEntry entry in allLogEntries)
            {
                DateTime bucketKey = GetBucketKeyForTimestamp(entry.TimeStamp, bucketStart, currentBucketSize);

                if (buckets.TryGetValue(bucketKey, out List<LogEntry> value))
                    value.Add(entry);
            }

            // Calculate maximum scaled value for normalization
            if (buckets.Count > 0)
            {
                int maximumCount = buckets.Values.Max(list => list.Count);
                if (maximumCount > 0)
                    maximumRawValue = Math.Pow(maximumCount, SCALE_POWER);
            }
        }

        /// <summary>
        /// Calculates an optimal bucket size from a set of nice intervals.
        /// </summary>
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

        /// <summary>
        /// Rounds a timestamp down to the nearest bucket boundary.
        /// </summary>
        private static DateTime RoundDownToNearestBucket(DateTime timestamp, TimeSpan bucketSize)
        {
            long ticks = timestamp.Ticks;
            long bucketTicks = bucketSize.Ticks;
            long roundedTicks = (ticks / bucketTicks) * bucketTicks;
            return new DateTime(roundedTicks);
        }

        /// <summary>
        /// Gets the bucket key for a given timestamp.
        /// </summary>
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
            if (allLogEntries.Count == 0)
                return 0;

            if (timestamp < minimumTimestamp || timestamp > maximumTimestamp)
                return -1;

            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;
            TimeSpan offset = timestamp - minimumTimestamp;

            double percentage = offset.TotalSeconds / totalSpan.TotalSeconds;
            return (float)(percentage * this.Width);
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

            if (buckets.Count == 0)
                return;

            int drawableWidth = this.Width;
            int drawableHeight = this.Height;

            if (drawableWidth <= 0 || drawableHeight <= 0)
                return;

            // Draw histogram bars
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

            DrawTimeTickMarks(graphics);

            // Draw error markers at the top of bars
            DrawErrorMarkers(graphics, drawableHeight);

            // Draw visible range indicator
            if (visibleRangeStart.HasValue && visibleRangeEnd.HasValue)
            {
                DrawVisibleRangeIndicator(graphics, drawableHeight);
            }

            // Draw tooltips
            if (hoveredBucketIndex >= 0 && hoveredBucketIndex < bucketCount)
            {
                DrawTooltip(graphics, sortedBucketKeys[hoveredBucketIndex], totalBarWidth);
            }
            else if (hoveredErrorIndex >= 0 && hoveredErrorIndex < errorLogEntries.Count)
            {
                DrawErrorTooltip(graphics, errorLogEntries[hoveredErrorIndex]);
            }
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

                // Find which bucket this error belongs to
                int bucketIndex = FindBucketIndexContainingTimestamp(errorEntry.TimeStamp, sortedBucketKeys);

                if (bucketIndex < 0)
                    continue;

                DateTime bucketKey = sortedBucketKeys[bucketIndex];

                // Calculate exact x position within the bucket
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
        /// Draws a semi-transparent overlay indicating the currently visible range in the log viewer.
        /// Uses bucket-based calculation to match the histogram layout, with special handling for edge buckets.
        /// Includes a minimum width for visibility and thicker border lines.
        /// </summary>
        private void DrawVisibleRangeIndicator(Graphics graphics, int drawableHeight)
        {
            if (allLogEntries.Count == 0 || buckets.Count == 0)
                return;

            List<DateTime> sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
            int bucketCount = sortedBucketKeys.Count;
            float totalBarWidth = (float)this.Width / bucketCount;

            // Find which buckets contain the visible range timestamps
            int startBucketIndex = FindBucketIndexContainingTimestamp(visibleRangeStart.Value, sortedBucketKeys);
            int endBucketIndex = FindBucketIndexContainingTimestamp(visibleRangeEnd.Value, sortedBucketKeys);

            if (startBucketIndex < 0 || endBucketIndex < 0)
                return;

            float startX;
            float endX;

            // For the first bucket, use x = 0
            if (startBucketIndex == 0 && visibleRangeStart == allLogEntries[0].TimeStamp)
            {
                startX = 0;
            }
            else
            {
                // Calculate position within the bucket
                DateTime startBucket = sortedBucketKeys[startBucketIndex];
                float startBucketX = startBucketIndex * totalBarWidth;
                double startFraction = (visibleRangeStart.Value - startBucket).TotalSeconds / currentBucketSize.TotalSeconds;
                startX = startBucketX + (float)(startFraction * totalBarWidth);
            }

            // For the last bucket, use x = control.Width
            if (endBucketIndex == bucketCount - 1 && visibleRangeEnd == allLogEntries[^1].TimeStamp)
            {
                endX = this.Width;
            }
            else
            {
                // Calculate position within the bucket
                DateTime endBucket = sortedBucketKeys[endBucketIndex];
                float endBucketX = endBucketIndex * totalBarWidth;
                double endFraction = (visibleRangeEnd.Value - endBucket).TotalSeconds / currentBucketSize.TotalSeconds;
                endX = endBucketX + (float)(endFraction * totalBarWidth);
            }

            float width = endX - startX;

            // Ensure minimum width for visibility
            if (width < MINIMUM_VISIBLE_RANGE_WIDTH)
            {
                float center = (startX + endX) / 2;
                startX = center - (MINIMUM_VISIBLE_RANGE_WIDTH / 2f);
                endX = center + (MINIMUM_VISIBLE_RANGE_WIDTH / 2f);
                width = MINIMUM_VISIBLE_RANGE_WIDTH;
            }

            if (width <= 0) return;

            // Draw semi-transparent fill
            using (SolidBrush fillBrush = new(VISIBLE_RANGE_COLOR))
            {
                graphics.FillRectangle(fillBrush, startX, 0, width, drawableHeight);
            }

            // Draw thicker border lines on left and right edges
            using Pen borderPen = new(VISIBLE_RANGE_BORDER_COLOR, 2);
            graphics.DrawLine(borderPen, startX, 0, startX, drawableHeight);
            graphics.DrawLine(borderPen, endX, 0, endX, drawableHeight);
        }

        /// <summary>
        /// Finds the bucket index that contains the given timestamp.
        /// </summary>
        private int FindBucketIndexContainingTimestamp(DateTime timestamp, List<DateTime> sortedBucketKeys)
        {
            for (int i = 0; i < sortedBucketKeys.Count; i++)
            {
                DateTime bucketStart = sortedBucketKeys[i];
                DateTime bucketEnd = bucketStart.Add(currentBucketSize);

                // Check if timestamp falls within this bucket
                if (timestamp >= bucketStart && timestamp < bucketEnd)
                    return i;
            }

            // If not found in any bucket, find the closest one
            if (timestamp < sortedBucketKeys[0])
                return 0;
            if (timestamp >= sortedBucketKeys[^1].Add(currentBucketSize))
                return sortedBucketKeys.Count - 1;

            return -1;
        }
        /// <summary>
        /// Draws time tick marks along the x-axis of the timeline, with labels formatted based on the total time span of the data.
        /// </summary>
        private void DrawTimeTickMarks(Graphics graphics)
        {
            using Pen tickPen = new(Color.FromArgb(150, 180, 180, 180), 1);
            using Font font = new("Segoe UI", 7);
            using SolidBrush brush = new(Color.FromArgb(120, 120, 120));
            TimeSpan totalSpan = maximumTimestamp - minimumTimestamp;

            int tickCount = Math.Min(10, buckets.Count / 5);
            if (tickCount < 3) tickCount = 3;

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
        /// <summary>
        /// Formats tick labels based on the total time span of the data, using more granular formats for shorter spans and more general formats for longer spans.
        /// </summary>
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

        /// <summary>
        /// Draws a tooltip for a histogram bucket.
        /// </summary>
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
            float tooltipY = 2; // Position below error markers

            // Keep tooltip within bounds
            if (tooltipX < 5)
                tooltipX = 5;
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

        /// <summary>
        /// Draws a tooltip for an error marker.
        /// </summary>
        private void DrawErrorTooltip(Graphics graphics, LogEntry errorEntry)
        {
            string tooltipText = $"Error at {errorEntry.TimeStamp:yyyy-MM-dd HH:mm:ss}";

            using Font font = new("Segoe UI", 9);
            SizeF textSize = graphics.MeasureString(tooltipText, font);

            float tooltipWidth = textSize.Width + 16;
            float tooltipHeight = textSize.Height + 4;

            float xPosition = GetXPositionForTimestamp(errorEntry.TimeStamp);
            float tooltipX = xPosition - (tooltipWidth / 2);
            float tooltipY = 20; // Position below error markers

            // Keep tooltip within bounds
            if (tooltipX < 5)
                tooltipX = 5;
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
        /// Handles mouse movement for hover effects.
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (buckets.Count == 0)
            {
                if (this.Cursor != Cursors.Default)
                    this.Cursor = Cursors.Default;
                return;
            }

            bool cursorChanged = false;

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

        /// <summary>
        /// Determines if the mouse is over an error marker.
        /// </summary>
        /// <returns>Index of error marker, or -1 if none</returns>
        private int GetErrorMarkerIndexAtPosition(int mouseX, int mouseY)
        {
            if (buckets.Count == 0)
                return -1;

            List<DateTime> sortedBucketKeys = [.. buckets.Keys.OrderBy(key => key)];
            int bucketCount = sortedBucketKeys.Count;
            float totalBarWidth = (float)this.Width / bucketCount;
            int drawableHeight = this.Height;

            for (int i = 0; i < errorLogEntries.Count; i++)
            {
                LogEntry errorEntry = errorLogEntries[i];

                // Find which bucket this error belongs to
                int bucketIndex = FindBucketIndexContainingTimestamp(errorEntry.TimeStamp, sortedBucketKeys);

                if (bucketIndex < 0)
                    continue;

                DateTime bucketKey = sortedBucketKeys[bucketIndex];

                // Calculate exact x position within the bucket (same as DrawErrorMarkers)
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

        /// <summary>
        /// Handles mouse leave events to clear hover states.
        /// </summary>
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
            if (this.Cursor != Cursors.Default)
                this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles mouse clicks on buckets and error markers.
        /// </summary>
        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (buckets.Count == 0 || e.Button != MouseButtons.Left)
                return;

            // Check for error marker click
            int clickedErrorIndex = GetErrorMarkerIndexAtPosition(e.X, e.Y);

            if (clickedErrorIndex != -1)
            {
                LogEntry errorEntry = errorLogEntries[clickedErrorIndex];
                ErrorMarkerClicked?.Invoke(this, errorEntry);
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