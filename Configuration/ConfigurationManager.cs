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
    /// <summary>
    /// Manages the application's configuration, including generic settings, log layouts, and log providers.
    /// Provides functionality to load, save, and manage configurations in a thread-safe manner.
    /// </summary>
    internal class ConfigurationManager
    {
        private static ConfigurationManager instance;
        private static readonly Lock lockObject = new();

        private GenericConfig genericConfig;
        private LogLayoutsConfig logLayoutsConfig;
        private readonly LogProvidersConfig logProvidersConfig;

        /// <summary>
        /// Private constructor to enforce the singleton pattern.
        /// Loads configuration files and initializes default settings.
        /// </summary>
        private ConfigurationManager()
        {
            // Load configuration from files
            genericConfig = LoadFromFile<GenericConfig>("LogScraperConfig.json");
            logLayoutsConfig = LoadFromFile<LogLayoutsConfig>("LogScraperLogLayouts.json");
            logProvidersConfig = LoadFromFile<LogProvidersConfig>("LogScraperLogProviders.json");

            SetAllLayoutsTransformers();

            // Limit the automatic read time to a maximum of 5 minutes
            if (genericConfig.AutomaticReadTimeMinutes > 5) genericConfig.AutomaticReadTimeMinutes = 1;

            // Set default log layouts for each log provider
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.FileConfig);
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.RuntimeConfig);
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.KubernetesConfig);
        }

        /// <summary>
        /// Ensures the singleton instance is created if it doesn't already exist.
        /// </summary>
        private static void CreateInstanceIfNeeded()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    instance ??= new ConfigurationManager();
                }
            }
        }

        /// <summary>
        /// Saves the log providers configuration to a file.
        /// </summary>
        public static void SaveLogProviders()
        {
            CreateInstanceIfNeeded();
            SaveToFile("LogScraperLogProviders.json", instance.logProvidersConfig);
        }

        /// <summary>
        /// Saves the log layouts configuration to a file.
        /// </summary>
        public static void SaveLogLayout()
        {
            CreateInstanceIfNeeded();
            SaveToFile("LogScraperLogLayouts.json", instance.logLayoutsConfig);
        }

        /// <summary>
        /// Saves the generic configuration to a file.
        /// </summary>
        public static void SaveGenericConfig()
        {
            CreateInstanceIfNeeded();
            SaveToFile("LogScraperConfig.json", instance.genericConfig);
        }

        /// <summary>
        /// Serializes and saves the given data to a file.
        /// Creates a backup of the existing file if it exists.
        /// </summary>
        /// <typeparam name="T">The type of the data to save.</typeparam>
        /// <param name="filePath">The path of the file to save the data to.</param>
        /// <param name="data">The data to serialize and save.</param>
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

        /// <summary>
        /// Sets the default log layout for a given log provider based on its description.
        /// </summary>
        /// <param name="logProviderConfig">The log provider configuration to update.</param>
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

        /// <summary>
        /// Sets transformers for all log layouts in the configuration.
        /// </summary>
        private void SetAllLayoutsTransformers()
        {
            foreach (var logLayout in logLayoutsConfig.layouts)
            {
                SetLayoutTransformers(logLayout);
            }
        }

        /// <summary>
        /// Sets transformers for a specific log layout based on its configuration.
        /// </summary>
        /// <param name="logLayout">The log layout to update with transformers.</param>
        private static void SetLayoutTransformers(LogLayout logLayout)
        {
            if (logLayout.LogTransformersConfig == null) return;

            logLayout.LogTransformers = [];
            foreach (var logTransformerConfig in logLayout.LogTransformersConfig)
            {
                logLayout.LogTransformers.Add(logTransformerConfig.CreateTransformer());
            }
        }

        /// <summary>
        /// Loads a configuration object from a JSON file.
        /// </summary>
        /// <typeparam name="T">The type of the configuration object to load.</typeparam>
        /// <param name="filePath">The path of the JSON file to load from.</param>
        /// <returns>The deserialized configuration object.</returns>
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

        // Properties for accessing and modifying configurations

        /// <summary>
        /// Gets or sets the generic configuration for the application.
        /// </summary>
        public static GenericConfig GenericConfig
        {
            get
            {
                CreateInstanceIfNeeded();
                return instance.genericConfig;
            }
            set
            {
                lock (lockObject)
                {
                    instance.genericConfig = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the log layouts configuration for the application.
        /// </summary>
        public static LogLayoutsConfig LogLayoutsConfig
        {
            get
            {
                CreateInstanceIfNeeded();
                return instance.logLayoutsConfig;
            }
            set
            {
                lock (lockObject)
                {
                    instance.logLayoutsConfig = value;
                }
            }
        }

        /// <summary>
        /// Gets the list of log layouts available in the configuration.
        /// </summary>
        public static List<LogLayout> LogLayouts
        {
            get
            {
                CreateInstanceIfNeeded();
                return instance.logLayoutsConfig.layouts;
            }
        }

        /// <summary>
        /// Gets the log providers configuration for the application.
        /// </summary>
        public static LogProvidersConfig LogProvidersConfig
        {
            get
            {
                CreateInstanceIfNeeded();
                return instance?.logProvidersConfig;
            }
        }

        /// <summary>
        /// Custom contract resolver to convert PascalCase property names to camelCase for JSON serialization.
        /// </summary>
        internal class CamelCasePropertyNamesContractResolver : DefaultContractResolver
        {
            /// <summary>
            /// Resolves a property name by converting it from PascalCase to camelCase.
            /// </summary>
            /// <param name="propertyName">The property name to resolve.</param>
            /// <returns>The resolved property name in camelCase.</returns>
            protected override string ResolvePropertyName(string propertyName)
            {
                return char.ToLowerInvariant(propertyName[0]) + propertyName[1..];
            }
        }
    }
}