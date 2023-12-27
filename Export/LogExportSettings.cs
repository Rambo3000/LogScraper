using LogScraper.Log.Collection;

namespace LogScraper.Export
{
    public class LogExportSettings
    {
        public LogLine LoglineBegin { get; set; }
        public int ExtraLinesBegin { get; set; }
        public LogLine LogLineEnd { get; set; }
        public int ExtraLinesEnd { get; set; }
        public LogExportSettingsMetadata LogExportSettingsMetadata { get; set; }
    }
}
