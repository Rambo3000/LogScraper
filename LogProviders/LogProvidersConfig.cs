using LogScraper.LogProviders.File;
using LogScraper.LogProviders.Kubernetes;
using LogScraper.LogProviders.Runtime;

namespace LogScraper.LogProviders
{
    internal class LogProvidersConfig
    {
        public FileConfig FileConfig { get; set; }
        public RuntimeConfig RuntimeConfig { get; set; }
        public KubernetesConfig KubernetesConfig { get; set; }
    }
}
