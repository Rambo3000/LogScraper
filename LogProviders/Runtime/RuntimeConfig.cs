using System.Collections.Generic;
using LogScraper.Configuration.LogProviderConfig;
using Newtonsoft.Json;
using LogScraper.Log;

namespace LogScraper.LogProviders.Runtime
{
    internal class RuntimeConfig : ILogProviderConfig
    {
        public string DefaultLogLayoutDescription { get; set; }
        [JsonIgnore]
        public LogLayout DefaultLogLayout { get; set; }
        public List<RuntimeInstance> Instances { get; set; }
        public override string ToString()
        {
            return "Directe URL";
        }
        [JsonIgnore]
        public LogProviderType LogProviderType
        {
            get { return LogProviderType.Runtime; }
        }
    }
}
