using System.Collections.Generic;
namespace LogScraper.Log.Layout
{
    /// <summary>
    /// Represents the configuration for log layouts.
    /// This class holds a collection of log layouts that define how logs are structured and processed.
    /// </summary>
    internal class LogLayoutsConfig
    {
        /// <summary>
        /// A list of log layouts available in the configuration.
        /// Each layout defines the structure, metadata, and content filters for a specific type of log.
        /// </summary>
        public List<LogLayout> layouts = [];
    }
}
