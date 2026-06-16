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

        private ConfigAppState() 
        {
            // When layouts change, automatically re-link provider configs to the new layout objects
            LogLayoutsConfig.Changed += (s, e) => UpdateProviderConfigLayoutReferences();
        }

        public StateSlice<GenericConfig> GenericConfig { get; } = new();
        public StateSlice<LogLayoutsConfig> LogLayoutsConfig { get; } = new();
        public StateSlice<LogProvidersConfig> LogProvidersConfig { get; } = new();

        /// <summary>
        /// Re-links provider config DefaultLogLayout references to the current layout objects
        /// when layouts are updated. This ensures provider configs always reference the correct layout instances.
        /// </summary>
        private void UpdateProviderConfigLayoutReferences()
        {
            LogLayoutsConfig layoutsConfig = LogLayoutsConfig.Value;
            LogProvidersConfig providersConfig = LogProvidersConfig.Value;

            if (layoutsConfig?.layouts == null || providersConfig == null) return;

            // Re-link each provider config to the new layout objects
            if (providersConfig.FileConfig != null)
                ConfigurationManager.SetDefaultLogLayoutsForLogProvider(providersConfig.FileConfig, layoutsConfig.layouts);
            if (providersConfig.RuntimeConfig != null)
                ConfigurationManager.SetDefaultLogLayoutsForLogProvider(providersConfig.RuntimeConfig, layoutsConfig.layouts);
            if (providersConfig.KubernetesConfig != null)
                ConfigurationManager.SetDefaultLogLayoutsForLogProvider(providersConfig.KubernetesConfig, layoutsConfig.layouts);

            // Force trigger the Changed event since we modified the provider configs
            LogProvidersConfig.ForceSet(providersConfig);
        }
    }
}
