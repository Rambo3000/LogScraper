using LogScraper.LogProviders.File;
using LogScraper.LogProviders.Kubernetes;
using LogScraper.LogProviders.Runtime;

namespace LogScraper.LogProviders
{
    /// <summary>
    /// Represents the configuration for all supported log providers.
    /// This class holds configurations for file-based, runtime, and Kubernetes log providers.
    /// </summary>
    internal class LogProvidersConfig
    {
        /// <summary>
        /// Gets or sets the configuration for the file-based log provider.
        /// </summary>
        public FileConfig FileConfig { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the runtime log provider.
        /// </summary>
        public RuntimeConfig RuntimeConfig { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the Kubernetes log provider.
        /// </summary>
        public KubernetesConfig KubernetesConfig { get; set; }
    }
}
