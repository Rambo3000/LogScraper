namespace LogScraper.LogProviders.Runtime
{
    public class RuntimeInstance
    {
        public string Description { get; set; }
        public string UrlRuntimeLog { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}
