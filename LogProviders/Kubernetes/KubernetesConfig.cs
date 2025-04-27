using System.Collections.Generic;
using Newtonsoft.Json;
using LogScraper.Configuration;
using LogScraper.Log.Layout;

namespace LogScraper.LogProviders.Kubernetes
{
    public class KubernetesConfig : ILogProviderConfig
    {
        public string DefaultLogLayoutDescription { get; set; }
        public KubernetesTimespan DefaultKubernetesTimespan { get; set; } = KubernetesTimespan.Everything;
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
