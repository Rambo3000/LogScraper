using Newtonsoft.Json;
using LogScraper.Log;
using LogScraper.LogProviders;

namespace LogScraper.Configuration.LogProviderConfig
{
    internal interface ILogProviderConfig
    {
        public string DefaultLogLayoutDescription { get; set; }
        [JsonIgnore]
        public LogLayout DefaultLogLayout { get; set; }
        // Use a method since Newtonsoft.Json does not support JsonIgnore on get properties
        public LogProviderType LogProviderType { get; }
    }
}
