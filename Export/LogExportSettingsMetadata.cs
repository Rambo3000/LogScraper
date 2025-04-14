using LogScraper.Log.Filter;
using LogScraper.Log.Metadata;
using System.Collections.Generic;

namespace LogScraper.Export
{
    public class LogExportSettingsMetadata
    {
        public bool ShowOriginalMetadata { get; set; }
        public FilterCriteriaWithStartPosition RemoveMetaDataCriteria { get; set; }
        public List<LogMetadataProperty> SelectedMetadataProperties { get; set; }
    }
}
