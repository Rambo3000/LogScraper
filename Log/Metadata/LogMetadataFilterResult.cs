using System.Collections.Generic;
using LogScraper.Log.Collection;

namespace LogScraper.Log.Metadata
{
    internal class LogMetadataFilterResult
    {
        public List<LogLine> LogLines { get; set; }
        public List<LogMetadataPropertyAndValues> LogMetadataPropertyAndValuesList { get; set; }
    }
}
