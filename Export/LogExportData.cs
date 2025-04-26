using System;

namespace LogScraper.Export
{
    internal class LogExportData
    {
        public string ExportRaw { get; set; }
        public int LogEntryCount { get; set; }
        public DateTime DateTimeFirstLogEntry { get; set; }
        public DateTime DateTimeLastLogEntry { get; set; }
    }
}
