using LogScraper.Log;
using LogScraper.LogProviders;

namespace LogScraper.Configuration.LogProviderConfig
{
    internal interface ILogProviderConfig
    {
        public string DefaultLogLayoutDescription { get; set; }
        public LogLayout DefaultLogLayout { get; set; }

        public abstract LogProviderType LogProviderType { get; }
    }
}
