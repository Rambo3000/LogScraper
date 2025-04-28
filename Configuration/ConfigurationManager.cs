using LogScraper.Configuration.Generic;
using LogScraper.Log.Layout;
using LogScraper.LogProviders;
using LogScraper.LogTransformers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace LogScraper.Configuration
{
    internal class ConfigurationManager
    {
        private static ConfigurationManager instance;
        private static readonly Lock lockObject = new();

        private GenericConfig genericConfig;
        private LogLayoutsConfig logLayoutsConfig;
        private readonly LogProvidersConfig logProvidersConfig;

        private ConfigurationManager()
        {
            // Load configuration from file during initialization
            genericConfig = LoadFromFile<GenericConfig>("LogScraperConfig.json");
            logLayoutsConfig = LoadFromFile<LogLayoutsConfig>("LogScraperLogLayouts.json");
            logProvidersConfig = LoadFromFile<LogProvidersConfig>("LogScraperLogProviders.json");

            SetAllLayoutsTransformers();

            // Limit the automatic read time to a maximum of 5 minutes
            if (genericConfig.AutomaticReadTimeMinutes > 5) genericConfig.AutomaticReadTimeMinutes = 1;
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.FileConfig);
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.RuntimeConfig);
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.KubernetesConfig);
        }
        private static void CreateInstanceIfNeeded()
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
        }
        public static void SaveLogProviders()
        {
            CreateInstanceIfNeeded();
            SaveToFile("LogScraperLogProviders.json", instance.logProvidersConfig);
        }
        public static void SaveLogLayout()
        {
            CreateInstanceIfNeeded();
            SaveToFile("LogScraperLogLayouts.json", instance.logLayoutsConfig);
        }
        public static void SaveGenericConfig()
        {
            CreateInstanceIfNeeded();
            SaveToFile("LogScraperConfig.json", instance.genericConfig);
        }

        private static void SaveToFile<T>(string filePath, T data)
        {
            string jsonContent = JsonConvert.SerializeObject(
                data,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            );

            if (File.Exists(filePath))
            {
                // Back up the existing file
                string backupPath = filePath + ".bak";
                File.Copy(filePath, backupPath, overwrite: true);
            }

            File.WriteAllText(filePath, jsonContent, Encoding.UTF8);
        }


        private void SetDefaultLogLayoutsForLogProvider(ILogProviderConfig logProviderConfig)
        {
            foreach (var loglayout in logLayoutsConfig.layouts)
            {
                if (loglayout.Description == logProviderConfig.DefaultLogLayoutDescription)
                {
                    logProviderConfig.DefaultLogLayout = loglayout;
                    break;
                }
            }
        }

        private void SetAllLayoutsTransformers()
        {
            foreach (var logLayout in logLayoutsConfig.layouts)
            {
                SetLayoutTransformers(logLayout);
            }
        }
        private static void SetLayoutTransformers(LogLayout logLayout)
        {
            if (logLayout.LogTransformersConfig == null) return;

            logLayout.LogTransformers = [];
            foreach (var logTransformerConfig in logLayout.LogTransformersConfig)
            {
                logLayout.LogTransformers.Add(logTransformerConfig.CreateTransformer());
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

        public static GenericConfig GenericConfig
        {
            get
            {
                CreateInstanceIfNeeded();
                return instance.genericConfig;
            }
            set
            {
                // Use a lock to ensure only one thread creates the instance
                lock (lockObject)
                {
                    instance.genericConfig = value;
                }
            }
        }

        public static LogLayoutsConfig LogLayoutsConfig
        {
            get
            {
                CreateInstanceIfNeeded();
                return instance.logLayoutsConfig;
            }
            set
            {
                // Use a lock to ensure only one thread creates the instance
                lock (lockObject)
                {
                    instance.logLayoutsConfig = value;
                }
            }
        }
        public static List<LogLayout> LogLayouts
        {
            get
            {
                CreateInstanceIfNeeded();
                return instance.logLayoutsConfig.layouts;
            }
        }
        public static LogProvidersConfig LogProvidersConfig
        {
            get
            {
                CreateInstanceIfNeeded();
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