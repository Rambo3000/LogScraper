namespace LogScraper.LogProviders.Kubernetes
{
    public class KubernetesNamespace
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}
