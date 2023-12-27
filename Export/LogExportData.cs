using System;

namespace LogScraper.Export
{
    internal class LogExportData
    {
        public string ExportRaw { get; set; }
        public int LineCount { get; set; }
        public DateTime DateTimeFirstLine { get; set; }
        public DateTime DateTimeLastLine { get; set; }
    }
}
