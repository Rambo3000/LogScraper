using System.Text.Json.Serialization;
using LogScraper.Log.LogProviders.File;
using LogScraper.Log.LogProviders.Kubernetes;
using LogScraper.Log.LogProviders.HttpEndpoint;

namespace LogScraper.Log.LogProviders
{
    /// <summary>
    /// Represents the configuration for all supported log providers.
    /// This class holds configurations for file-based, HTTP endpoint, and Kubernetes log providers.
    /// </summary>
    internal class LogProvidersConfig
    {
        /// <summary>
        /// Gets or sets the configuration for the file-based log provider.
        /// </summary>
        public FileConfig FileConfig { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the HTTP endpoint log provider.
        /// </summary>
        /// <remarks>
        /// The JSON property name is intentionally kept as "runtimeConfig" (its original name) so that
        /// existing LogScraperLogProviders.json files continue to load without requiring a migration.
        /// </remarks>
        [JsonPropertyName("runtimeConfig")]
        public HttpEndpointConfig HttpEndpointConfig { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the Kubernetes log provider.
        /// </summary>
        public KubernetesConfig KubernetesConfig { get; set; }
    }
}
