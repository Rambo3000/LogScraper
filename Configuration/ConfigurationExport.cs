using LogScraper.Configuration.Generic;
using LogScraper.Log.Layout;
using LogScraper.LogProviders;

namespace LogScraper.Configuration
{
    /// <summary>
    /// Represents a container for configuration sections used to export or import settings.
    /// </summary>
    internal class ConfigurationExport
    {
        public LogProvidersConfig LogProvidersConfig { get; set;} 
        public GenericConfig GenericConfig { get; set; }
        public LogLayoutsConfig LogLayoutsConfig { get; set; }
    }
}