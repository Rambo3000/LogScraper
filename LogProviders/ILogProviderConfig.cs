using Newtonsoft.Json;
using LogScraper.Log.Layout;

namespace LogScraper.LogProviders
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
