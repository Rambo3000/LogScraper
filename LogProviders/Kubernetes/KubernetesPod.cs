namespace LogScraper.LogProviders.Kubernetes
{
    internal class KubernetesPod
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string DescriptionShortened { get; set; }
        public override string ToString()
        {
            return DescriptionShortened;
        }
    }
}
