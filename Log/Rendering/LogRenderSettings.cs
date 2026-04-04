using System.Collections.Generic;
using LogScraper.Log.Layout;
using LogScraper.Log.Metadata;
using LogScraper.LogPostProcessors;

namespace LogScraper.Log.Rendering
{
    /// <summary>
    /// Represents the settings used for rendering log entries to the screen or a file.
    /// </summary>
    public class LogRenderSettings
    {
        public LogRange LogRange { get; set; }

        /// <summary>
        /// The layout of the log file, including metadata and content properties.
        /// </summary>
        public LogLayout LogLayout { get; set; }

        /// <summary>
        /// Indicates whether the original metadata should be included in the log entries to be rendered.
        /// </summary>
        public bool ShowOriginalMetadata { get; set; }

        /// <summary>
        /// A list of metadata properties to include the log entries to be rendered.
        /// </summary>
        public List<LogMetadataProperty> SelectedMetadataProperties { get; set; }

        /// <summary>
        /// Settings for rendering a log flow tree, if applicable. If null, no log flow tree will be rendered.
        /// </summary>
        public LogFlowTreeRenderSettings LogFlowTreeRenderSettings { get; set; }

        /// <summary>
        /// A list of log post processor kinds to use render for each log entry.
        /// </summary>
        public List<LogPostProcessorKind> LogPostProcessorKinds { get; set; }
    }
}