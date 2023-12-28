using System.Collections.Generic;
using LogScraper.Configuration.LogProviderConfig;
using LogScraper.Log;

namespace LogScraper.LogProviders.Runtime
{
    internal class RuntimeConfig : ILogProviderConfig
    {
        public string DefaultLogLayoutDescription { get; set; }
        public LogLayout DefaultLogLayout { get; set; }
        public List<RuntimeInstance> Instances { get; set; }

        public override string ToString()
        {
            return "Directe URL";
        }
        public LogProviderType LogProviderType
        {
            get { return LogProviderType.Runtime; }
        }
    }
}
