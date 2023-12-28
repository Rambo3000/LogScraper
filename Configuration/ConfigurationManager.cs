using LogScraper.Configuration.LogProviderConfig;
using LogScraper.Log;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogScraper.Configuration
{
    internal class ConfigurationManager
    {
        private static ConfigurationManager instance;
        private static readonly object lockObject = new();
        private readonly LogScraperConfig genericConfig;
        private readonly LogLayoutsConfig logLayouts;
        private readonly LogProvidersConfig logProvidersConfig;

        private ConfigurationManager()
        {
            // Load configuration from file during initialization
            genericConfig = LoadFromFile<LogScraperConfig>("LogScraperConfig.json");
            logLayouts = LoadFromFile<LogLayoutsConfig>("LogScraperLogLayouts.json");
            logProvidersConfig = LoadFromFile<LogProvidersConfig>("LogScraperLogProviders.json");

            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.FileConfig);
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.RuntimeConfig);
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.KubernetesConfig);
        }

        private void SetDefaultLogLayoutsForLogProvider(ILogProviderConfig logProviderConfig)
        {
            foreach (var loglayout in logLayouts.layouts)
            {
                if (loglayout.Description == logProviderConfig.DefaultLogLayoutDescription)
                {
                    logProviderConfig.DefaultLogLayout = loglayout;
                    break;
                }
            }
        }

        private static T LoadFromFile<T>(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath, Encoding.UTF8);

            // Deserialize JSON content using the custom contract resolver
            return JsonConvert.DeserializeObject<T>(
                jsonContent,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            );
        }

        public static LogScraperConfig GenericConfig
        {
            get
            {
                // Check if an instance already exists
                if (instance == null)
                {
                    // Use a lock to ensure only one thread creates the instance
                    lock (lockObject)
                    {
                        instance ??= new ConfigurationManager();
                    }
                }
                return instance.genericConfig;
            }
        }
        public static List<LogLayout> LogLayouts
        {
            get
            {
                // Check if an instance already exists
                if (instance == null)
                {
                    // Use a lock to ensure only one thread creates the instance
                    lock (lockObject)
                    {
                        instance ??= new ConfigurationManager();
                    }
                }
                return instance.logLayouts.layouts;
            }
        }
        public static LogProvidersConfig LogProvidersConfig
        {
            get
            {
                // Check if an instance already exists
                if (instance == null)
                {
                    // Use a lock to ensure only one thread creates the instance
                    lock (lockObject)
                    {
                        instance ??= new ConfigurationManager();
                    }
                }
                return instance?.logProvidersConfig;
            }
        }

        internal class CamelCasePropertyNamesContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                // Convert PascalCase property names to camelCase for JSON serialization
                return char.ToLowerInvariant(propertyName[0]) + propertyName[1..];
            }
        }
    }
}