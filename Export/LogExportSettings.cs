using System.Collections.Generic;
using LogScraper.Log.Collection;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;

namespace LogScraper.Export
{
    public class LogExportSettings
    {
        public LogEntry LogEntryBegin { get; set; }
        public int ExtraLogEntriesBegin { get; set; }
        public LogEntry LogEntryEnd { get; set; }
        public int ExtraLogEntriesEnd { get; set; }
        public LogLayout LogLayout { get; set; }
        public bool ShowOriginalMetadata { get; set; }
        public List<LogMetadataProperty> SelectedMetadataProperties { get; set; }
    }
}
