using LogScraper.Log.Collection;

namespace LogScraper.Export
{
    public class LogExportSettings
    {
        public LogEntry LogEntryBegin { get; set; }
        public int ExtraLogEntriesBegin { get; set; }
        public LogEntry LogEntryEnd { get; set; }
        public int ExtraLogEntriesEnd { get; set; }
        public LogExportSettingsMetadata LogExportSettingsMetadata { get; set; }
    }
}
