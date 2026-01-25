using System.Collections.Generic;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;

namespace LogScraper.Log
{
    /// <summary>
    /// Represents the settings used for exporting log data.
    /// </summary>
    public class LogRenderSettings
    {
        /// <summary>
        /// The log entry that marks the beginning of the export range.
        /// </summary>
        public LogEntry LogEntryBegin { get; set; }

        /// <summary>
        /// The log entry that marks the end of the export range.
        /// </summary>
        public LogEntry LogEntryEnd { get; set; }

        /// <summary>
        /// The layout of the log file, including metadata and content properties.
        /// </summary>
        public LogLayout LogLayout { get; set; }

        /// <summary>
        /// Indicates whether the original metadata should be included in the exported log.
        /// </summary>
        public bool ShowOriginalMetadata { get; set; }

        /// <summary>
        /// A list of metadata properties to include in the exported log.
        /// </summary>
        public List<LogMetadataProperty> SelectedMetadataProperties { get; set; }
    }
}
