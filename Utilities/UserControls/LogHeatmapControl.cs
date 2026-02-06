using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log;

namespace LogScraper.Utilities.UserControls
{
    public partial class LogHeatmapControl : UserControl
    {
        private List<LogEntry> allLogEntries = new List<LogEntry>();
        private Dictionary<DateTime, List<LogEntry>> buckets = new Dictionary<DateTime, List<LogEntry>>();
        private List<LogEntry> errorLogEntries = new List<LogEntry>();
        private TimeSpan currentBucketSize;
        private DateTime minimumTimestamp;
        private DateTime maximumTimestamp;
        private double maximumRawValue;
        private int hoveredBucketIndex = -1;
        private int hoveredErrorIndex = -1;
        private DateTime? currentScrollPosition = null;
        private const double SCALE_POWER = 0.4;
        private const int MINIMUM_BUCKET_COUNT = 10;
        private const int ERROR_MARKER_SIZE = 8;

        private static readonly Color BAR_COLOR = Color.LightGray;
        private static readonly Color BAR_HOVER_COLOR = Color.FromArgb(100, 160, 210);
        private static readonly Color LABEL_COLOR = Color.FromArgb(225, 225, 225);
        private static readonly Color SCROLL_INDICATOR_COLOR = Color.FromArgb(100, 128, 128, 128);
        private static readonly Color ERROR_MARKER_COLOR = Color.FromArgb(220, 50, 50);
        private static readonly Color ERROR_MARKER_HOVER_COLOR = Color.FromArgb(255, 80, 80);

        public event EventHandler<LogEntry> HeatmapCellClicked;
        public event EventHandler<LogEntry> ErrorMarkerClicked;

        public LogHeatmapControl()
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

            if (allLogEntries.Count > 0 && entries.Count > allLogEntries.Count)
            {
                LogEntry currentFirst = allLogEntries[0];
                LogEntry currentLast = allLogEntries[allLogEntries.Count - 1];

                LogEntry newFirst = entries[0];
                LogEntry newLast = entries[allLogEntries.Count - 1];

                if (currentFirst.TimeStamp == newFirst.TimeStamp && currentLast.TimeStamp == newLast.TimeStamp)
                {
                    isAppend = true;
                }
            }

            if (isAppend)
            {
                List<LogEntry> newEntries = entries.Skip(allLogEntries.Count).ToList();
                allLogEntries.AddRange(newEntries);
                RecalculateBuckets();
            }
            else
            {
                allLogEntries = new List<LogEntry>(entries);
                RecalculateBuckets();
            }

            errorLogEntries = allLogEntries.Where(entry => entry.IsErrorLogEntry).ToList();

            this.Invalidate();
        }

        public void SetScrollPosition(DateTime timestamp)
        {
            currentScrollPosition = timestamp;
            this.Invalidate();
        }

        public void ClearScrollPosition()
        {
            currentScrollPosition = null;
            this.Invalidate();
        }

        private void RecalculateBuckets()
        {
            buckets.Clear();

            if (allLogEntries.Count == 0)
                return;

            minimumTimestamp = allLogEntries[0].TimeStamp;
            maximumTimestamp = allLogEntries[allLogEntries.Count - 1].TimeStamp;

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
                buckets[currentBucket] = new List<LogEntry>();
                currentBucket = currentBucket.Add(currentBucketSize);
            }

            foreach (LogEntry entry in allLogEntries)
            {
                DateTime bucketKey = GetBucketKeyForTimestamp(entry.TimeStamp, bucketStart, currentBucketSize);

                if (buckets.ContainsKey(bucketKey))
                    buckets[bucketKey].Add(entry);
            }

            if (buckets.Count > 0)
            {
                int maximumCount = buckets.Values.Max(list => list.Count);
                if (maximumCount > 0)
                    maximumRawValue = Math.Pow(maximumCount, SCALE_POWER);
            }
        }

        private TimeSpan CalculateOptimalBucketSize(TimeSpan totalSpan, int desiredBuckets)
        {
            double secondsPerBucket = totalSpan.TotalSeconds / desiredBuckets;

            TimeSpan[] niceIntervals = new[]
            {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(30),
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(5),
            TimeSpan.FromMinutes(10),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromHours(1),
            TimeSpan.FromHours(6),
            TimeSpan.FromHours(12),
            TimeSpan.FromDays(1),
            TimeSpan.FromDays(7),
            TimeSpan.FromDays(30)
        };

            foreach (TimeSpan interval in niceIntervals)
            {
                if (interval.TotalSeconds >= secondsPerBucket)
                    return interval;
            }

            return TimeSpan.FromDays(30);
        }

        private DateTime RoundDownToNearestBucket(DateTime timestamp, TimeSpan bucketSize)
        {
            long ticks = timestamp.Ticks;
            long bucketTicks = bucketSize.Ticks;
            long roundedTicks = (ticks / bucketTicks) * bucketTicks;
            return new DateTime(roundedTicks);
        }

        private DateTime GetBucketKeyForTimestamp(DateTime timestamp, DateTime bucketStart, TimeSpan bucketSize)
        {
            TimeSpan offset = timestamp - bucketStart;
            long bucketIndex = (long)(offset.TotalSeconds / bucketSize.TotalSeconds);
            return bucketStart.Add(TimeSpan.FromSeconds(bucketIndex * bucketSize.TotalSeconds));
        }

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

            List<DateTime> sortedBucketKeys = buckets.Keys.OrderBy(key => key).ToList();
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

                    using (SolidBrush brush = new SolidBrush(barColor))
                    {
                        graphics.FillRectangle(brush, x, y, barWidth, barHeight);
                    }
                }
            }

            DrawTimeLabels(graphics);

            DrawErrorMarkers(graphics, drawableHeight);

            if (currentScrollPosition.HasValue)
            {
                DrawScrollIndicator(graphics, drawableHeight);
            }

            if (hoveredBucketIndex >= 0 && hoveredBucketIndex < bucketCount)
            {
                DrawTooltip(graphics, sortedBucketKeys[hoveredBucketIndex], totalBarWidth);
            }
            else if (hoveredErrorIndex >= 0 && hoveredErrorIndex < errorLogEntries.Count)
            {
                DrawErrorTooltip(graphics, errorLogEntries[hoveredErrorIndex]);
            }
        }

        private void DrawErrorMarkers(Graphics graphics, int drawableHeight)
        {
            for (int i = 0; i < errorLogEntries.Count; i++)
            {
                LogEntry errorEntry = errorLogEntries[i];
                float xPosition = GetXPositionForTimestamp(errorEntry.TimeStamp);

                if (xPosition < 0)
                    continue;

                Color markerColor = (i == hoveredErrorIndex) ? ERROR_MARKER_HOVER_COLOR : ERROR_MARKER_COLOR;

                float markerX = xPosition - (ERROR_MARKER_SIZE / 2);
                float markerY = drawableHeight - ERROR_MARKER_SIZE;

                using (SolidBrush brush = new SolidBrush(markerColor))
                {
                    graphics.FillEllipse(brush, markerX, markerY, ERROR_MARKER_SIZE, ERROR_MARKER_SIZE);
                }
            }
        }

        private void DrawScrollIndicator(Graphics graphics, int drawableHeight)
        {
            if (allLogEntries.Count == 0)
                return;

            DateTime scrollTime = currentScrollPosition.Value;
            float xPosition = GetXPositionForTimestamp(scrollTime);

            if (xPosition < 0)
                return;

            using (Pen pen = new Pen(SCROLL_INDICATOR_COLOR, 2))
            {
                graphics.DrawLine(pen, xPosition, 0, xPosition, drawableHeight);
            }
        }

        private void DrawTimeLabels(Graphics graphics)
        {
            using (Font font = new Font("Segoe UI", 8))
            using (SolidBrush brush = new SolidBrush(LABEL_COLOR))
            {
                string startTime = minimumTimestamp.ToString("HH:mm");
                string endTime = maximumTimestamp.ToString("HH:mm");

                graphics.DrawString(startTime, font, brush, 5, 5);

                SizeF endTimeSize = graphics.MeasureString(endTime, font);
                graphics.DrawString(endTime, font, brush, this.Width - endTimeSize.Width - 5, 5);
            }
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

            using (Font font = new Font("Segoe UI", 9))
            {
                SizeF textSize = graphics.MeasureString(tooltipText, font);

                float tooltipWidth = textSize.Width + 16;
                float tooltipHeight = textSize.Height + 4;

                float tooltipX = (hoveredBucketIndex * barWidth) + (barWidth / 2) - (tooltipWidth / 2);
                float tooltipY = 5;

                if (tooltipX < 5)
                    tooltipX = 5;
                if (tooltipX + tooltipWidth > this.Width - 5)
                    tooltipX = this.Width - tooltipWidth - 5;

                using (SolidBrush backgroundBrush = new SolidBrush(Color.FromArgb(240, 50, 50, 50)))
                using (Pen borderPen = new Pen(Color.FromArgb(100, 100, 100), 1))
                {
                    graphics.FillRectangle(backgroundBrush, tooltipX, tooltipY, tooltipWidth, tooltipHeight);
                    graphics.DrawRectangle(borderPen, tooltipX, tooltipY, tooltipWidth, tooltipHeight);
                }

                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    graphics.DrawString(tooltipText, font, textBrush, tooltipX + 8, tooltipY + 2);
                }
            }
        }

        private void DrawErrorTooltip(Graphics graphics, LogEntry errorEntry)
        {
            string tooltipText = $"Error at {errorEntry.TimeStamp:yyyy-MM-dd HH:mm:ss}";

            using (Font font = new Font("Segoe UI", 9))
            {
                SizeF textSize = graphics.MeasureString(tooltipText, font);

                float tooltipWidth = textSize.Width + 16;
                float tooltipHeight = textSize.Height + 4;

                float xPosition = GetXPositionForTimestamp(errorEntry.TimeStamp);
                float tooltipX = xPosition - (tooltipWidth / 2);
                float tooltipY = 5;

                if (tooltipX < 5)
                    tooltipX = 5;
                if (tooltipX + tooltipWidth > this.Width - 5)
                    tooltipX = this.Width - tooltipWidth - 5;

                using (SolidBrush backgroundBrush = new SolidBrush(Color.FromArgb(240, 80, 20, 20)))
                using (Pen borderPen = new Pen(ERROR_MARKER_COLOR, 1))
                {
                    graphics.FillRectangle(backgroundBrush, tooltipX, tooltipY, tooltipWidth, tooltipHeight);
                    graphics.DrawRectangle(borderPen, tooltipX, tooltipY, tooltipWidth, tooltipHeight);
                }

                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    graphics.DrawString(tooltipText, font, textBrush, tooltipX + 8, tooltipY + 2);
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (buckets.Count == 0)
            {
                if (this.Cursor != Cursors.Default)
                    this.Cursor = Cursors.Default;
                return;
            }

            bool cursorChanged = false;

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

                int drawableWidth = this.Width;
                List<DateTime> sortedBucketKeys = buckets.Keys.OrderBy(key => key).ToList();
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

        private int GetErrorMarkerIndexAtPosition(int mouseX, int mouseY)
        {
            int drawableHeight = this.Height;

            for (int i = 0; i < errorLogEntries.Count; i++)
            {
                LogEntry errorEntry = errorLogEntries[i];
                float xPosition = GetXPositionForTimestamp(errorEntry.TimeStamp);

                if (xPosition < 0)
                    continue;

                float markerX = xPosition - (ERROR_MARKER_SIZE / 2);
                float markerY = drawableHeight - ERROR_MARKER_SIZE;

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
            if (this.Cursor != Cursors.Default)
                this.Cursor = Cursors.Default;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (buckets.Count == 0 || e.Button != MouseButtons.Left)
                return;

            int clickedErrorIndex = GetErrorMarkerIndexAtPosition(e.X, e.Y);

            if (clickedErrorIndex != -1)
            {
                LogEntry errorEntry = errorLogEntries[clickedErrorIndex];
                ErrorMarkerClicked?.Invoke(this, errorEntry);
                return;
            }

            int drawableWidth = this.Width;

            List<DateTime> sortedBucketKeys = buckets.Keys.OrderBy(key => key).ToList();
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
                    HeatmapCellClicked?.Invoke(this, firstEntry);
                }
            }
        }
    }
}
