using System.Collections.Generic;

namespace LogScraper.LogProviders.Kubernetes
{
    public class KubernetesCluster
    {
        public string Description { get; set; }
        public string BaseUrl { get; set; }
        public string ClusterId { get; set; }
        public List<KubernetesNamespace> Namespaces { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}
