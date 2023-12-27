using LogScraper.Log;
using LogScraper.Log.Metadata;
using System.Collections.Generic;

namespace LogScraper.Export
{
    public class LogExportSettingsMetadata
    {
        public bool ShowOriginalMetadata { get; set; }
        public FilterCriteria RemoveMetaDataCriteria { get; set; }
        public List<LogMetadataProperty> SelectedMetadataProperties { get; set; }
    }
}
