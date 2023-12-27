using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text;

namespace LogScraper.Configuration
{
    internal class ConfigurationManager
    {
        private static ConfigurationManager instance;
        private static readonly object lockObject = new();
        private readonly LogScraperConfig config;

        private ConfigurationManager()
        {
            // Load configuration from file during initialization
            config = LoadFromFile("LogScraperConfig.json");
        }

        private static LogScraperConfig LoadFromFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath, Encoding.UTF8);

            // Deserialize JSON content using the custom contract resolver
            return JsonConvert.DeserializeObject<LogScraperConfig>(
                jsonContent,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            );
        }

        public static LogScraperConfig Config
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
                return instance.config;
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