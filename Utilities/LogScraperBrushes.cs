using System.Drawing;

namespace LogScraper.Utilities
{
    public static class LogScraperBrushes
    {
        public static readonly Brush GraySelectedBeginOrEnd = new SolidBrush(Color.FromArgb(240, 240, 240));
        public static readonly Brush GrayLogEntriesOutOfScope = new SolidBrush(Color.FromArgb(200, 200, 200));
        public static readonly Brush BlueSelectedLogline = Brushes.LightBlue;
    }
}
