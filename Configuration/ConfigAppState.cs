using System.Collections.Generic;
using LogScraper.Log.Layout;
using LogScraper.Log.LogAppState;
using LogScraper.LogProviders;

namespace LogScraper.Configuration
{
    /// <summary>
    /// Observable configuration state container.
    /// Populated once at startup by <see cref="ConfigurationManager.Initialize"/>.
    /// Wrap the persisted config objects in StateSlice so controls
    /// can react to changes without manual notification plumbing.
    /// </summary>
    internal sealed class ConfigAppState
    {
        public static ConfigAppState Instance { get; } = new();

        private ConfigAppState() { }

        public StateSlice<GenericConfig> GenericConfig { get; } = new();
        public StateSlice<LogLayoutsConfig> LogLayoutsConfig { get; } = new();
        public StateSlice<LogProvidersConfig> LogProvidersConfig { get; } = new();
    }
}
