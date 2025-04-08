using Newtonsoft.Json;
using LogScraper.Configuration.LogProviderConfig;
using LogScraper.Log;

namespace LogScraper.LogProviders.File
{
    internal class FileConfig : ILogProviderConfig
    {
        public string DefaultLogLayoutDescription { get; set; }

        [JsonIgnore]
        public LogLayout DefaultLogLayout { get; set; }
        [JsonIgnore]
        public LogProviderType LogProviderType
        {
            get { return LogProviderType.File; }
        }
        public override string ToString()
        {
            return "Lokaal bestand";
        }
    }
}
