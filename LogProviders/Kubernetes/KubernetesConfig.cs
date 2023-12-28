using System.Collections.Generic;
using LogScraper.Configuration.LogProviderConfig;
using LogScraper.Log;

namespace LogScraper.LogProviders.Kubernetes
{
    internal class KubernetesConfig : ILogProviderConfig
    {
        public string DefaultLogLayoutDescription { get; set; }
        public List<KubernetesCluster> Clusters { get; set; }
        public LogLayout DefaultLogLayout { get; set; }
        public override string ToString()
        {
            return "Kubernetes";
        }
        public LogProviderType LogProviderType
        {
            get { return LogProviderType.Kubernetes; }
        }
    }
}
