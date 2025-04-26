using LogScraper.Log.Collection;

namespace LogScraper.Export
{
    public class LogExportSettings
    {
        public LogEntry LogEntryBegin { get; set; }
        public int ExtraLinesBegin { get; set; }
        public LogEntry LogEntryEnd { get; set; }
        public int ExtraLinesEnd { get; set; }
        public LogExportSettingsMetadata LogExportSettingsMetadata { get; set; }
    }
}
