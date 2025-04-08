using System.Collections.Generic;
using Newtonsoft.Json;
using LogScraper.Configuration.LogProviderConfig;
using LogScraper.Log;

namespace LogScraper.LogProviders.Kubernetes
{
    internal class KubernetesConfig : ILogProviderConfig
    {
        public string DefaultLogLayoutDescription { get; set; }
        public List<KubernetesCluster> Clusters { get; set; }
        [JsonIgnore]
        public LogLayout DefaultLogLayout { get; set; }
        public override string ToString()
        {
            return "Kubernetes";
        }
        [JsonIgnore]
        public LogProviderType LogProviderType
        {
            get { return LogProviderType.Kubernetes; }
        }
    }
}
