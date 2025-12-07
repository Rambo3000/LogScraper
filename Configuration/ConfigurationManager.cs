using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using LogScraper.Configuration.Generic;
using LogScraper.Log.Content;
using LogScraper.Log.Layout;
using LogScraper.LogProviders;
using LogScraper.LogTransformers;
using LogScraper.Utilities.Extensions;
using LogScraper.Utilities.IndexDictionary;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

            InitializeGenericConfig(genericConfig);
            InitializeLogLayoutsConfig(logLayoutsConfig);
            InitializeLogProvidersConfig(logProvidersConfig, logLayoutsConfig.layouts);
        }
        /// <summary>
        /// Initializes the specified generic configuration object, enforcing limits on its automatic read time setting.
        /// </summary>
        /// <remarks>If the AutomaticReadTimeMinutes property of the configuration exceeds 5, it is reset
        /// to 1 to ensure the value remains within the allowed range.</remarks>
        /// <param name="genericConfig">The configuration object to initialize. Cannot be null.</param>
        public static void InitializeGenericConfig(GenericConfig genericConfig)
        {
            // Limit the automatic read time to a maximum of 5 minutes
            if (genericConfig.AutomaticReadTimeMinutes > 5) genericConfig.AutomaticReadTimeMinutes = 1;
        }
        /// <summary>
        /// Initializes the specified log layouts configuration by updating its criteria and properties to ensure compatibility
        /// with the current logging system.
        /// </summary>
        /// <remarks>This method updates obsolete or legacy configuration fields to their current equivalents and ensures
        /// that all necessary properties are set for correct operation. It should be called before using the configuration in
        /// logging operations.</remarks>
        /// <param name="logLayoutsConfig">The log layouts configuration to initialize. Cannot be null.</param>
        public static void InitializeLogLayoutsConfig(LogLayoutsConfig logLayoutsConfig)
        {
#pragma warning disable CS0612 // Type or member is obsolete
            // Set multiple content criterias if only the single (obsolete) content criteria is set
            ConvertSingleToMultipleCriterias(logLayoutsConfig);
#pragma warning restore CS0612 // Type or member is obsolete
            SetEndFlowTreeContentProperties(logLayoutsConfig);
            AssignLogPropertyIndexes(logLayoutsConfig);
            SetAllLayoutsTransformers(logLayoutsConfig.layouts);
        }
        /// <summary>
        /// Initializes the log provider configurations with the specified log layouts.
        /// </summary>
        /// <param name="logProvidersConfig">The configuration object containing settings for various log providers. Cannot be null.</param>
        /// <param name="logLayouts">A list of log layouts to apply to each log provider configuration. Cannot be null.</param>
        public static void InitializeLogProvidersConfig(LogProvidersConfig logProvidersConfig, List<LogLayout> logLayouts)
        {
            // Set default log layouts for each log provider
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.FileConfig, logLayouts);
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.RuntimeConfig, logLayouts);
            SetDefaultLogLayoutsForLogProvider(logProvidersConfig.KubernetesConfig, logLayouts);
        }

        /// <summary>
        /// Sets the content criterias for log layouts if only the single (absolete) content criteria is set.
        /// </summary>
        /// <param name="logLayoutsConfig">The log layouts configuration to update.</param>
        [Obsolete]
        private static void ConvertSingleToMultipleCriterias(LogLayoutsConfig logLayoutsConfig)
        {
            foreach (LogLayout logLayout in logLayoutsConfig.layouts)
            {
                foreach (LogContentProperty logContentProperty in logLayout.LogContentProperties)
                {
                    // If the content criteria is empty, skip
                    if (logContentProperty.Criteria == null) continue;
                    // If the content criteria is not empty, skip
                    if (logContentProperty.Criterias != null && logContentProperty.Criterias.Count > 0) continue;
                    // Create a new list for multiple content criteria
                    logContentProperty.Criterias = [logContentProperty.Criteria];
                    logContentProperty.Criteria = null;
                }
            }
        }

        /// <summary>
        /// Assigns indexes to log metadata and content properties for each log layout.
        /// </summary>
        /// <param name="logLayoutsConfig">The log layouts configuration to update.</param>
        private static void AssignLogPropertyIndexes(LogLayoutsConfig logLayoutsConfig)
        {
            foreach (LogLayout logLayout in logLayoutsConfig.layouts)
            {
                logLayout.LogMetadataProperties?.AssignIndexes();
                logLayout.LogContentProperties?.AssignIndexes();
            }
        }

        /// <summary>
        /// Sets the end flow tree content properties for log layouts based on their descriptions.
        /// </summary>
        /// <param name="logLayoutsConfig">The log layouts configuration to update.</param>
        private static void SetEndFlowTreeContentProperties(LogLayoutsConfig logLayoutsConfig)
        {
            foreach (LogLayout logLayout in logLayoutsConfig.layouts)
            {
                foreach (LogContentProperty logContentProperty in logLayout.LogContentProperties)
                {
                    if (!logContentProperty.IsBeginFlowTreeFilter ||
                        string.IsNullOrWhiteSpace(logContentProperty.EndFlowTreeContentPropertyDescription))
                    {
                        logContentProperty.EndFlowTreeContentProperty = null;
                        logContentProperty.EndFlowTreeContentPropertyDescription = string.Empty;
                        continue;
                    }

                    foreach (LogContentProperty logContentPropertyEnd in logLayout.LogContentProperties)
                    {
                        if (logContentPropertyEnd.Description == logContentProperty.EndFlowTreeContentPropertyDescription)
                        {
                            logContentProperty.EndFlowTreeContentProperty = logContentPropertyEnd;
                            break;
                        }
                    }
                }
            }
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
                    instance ??= new();
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
        public static void SaveToFile<T>(string filePath, T data)
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
        private static void SetDefaultLogLayoutsForLogProvider(ILogProviderConfig logProviderConfig, List<LogLayout> logLayouts)
        {
            foreach (var loglayout in logLayouts)
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
        private static void SetAllLayoutsTransformers(List<LogLayout> logLayouts)
        {
            foreach (var logLayout in logLayouts)
            {
                if (logLayout.LogTransformersConfig == null) continue;

                logLayout.LogTransformers = [];
                foreach (var logTransformerConfig in logLayout.LogTransformersConfig)
                {
                    logLayout.LogTransformers.Add(logTransformerConfig.CreateTransformer());
                }
            }
        }

        /// <summary>
        /// Loads a configuration object from a JSON file.
        /// </summary>
        /// <typeparam name="T">The type of the configuration object to load.</typeparam>
        /// <param name="filePath">The path of the JSON file to load from.</param>
        /// <returns>The deserialized configuration object.</returns>
        public static T LoadFromFile<T>(string filePath)
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

        /// <summary>
        /// Attempts to load a configuration export from the specified file, supporting multiple configuration formats.
        /// </summary>
        /// <remarks>This method supports loading configuration data stored as a full ConfigurationExport
        /// or as individual configuration objects. It enables compatibility with files saved in different formats by
        /// the user.</remarks>
        /// <param name="filePath">The path to the file containing the configuration data to load. Must not be null or empty.</param>
        /// <param name="configurationExport">When this method returns, contains the loaded configuration export if successful; otherwise, null.</param>
        /// <returns>true if a valid configuration export was loaded from the file; otherwise, false.</returns>
        internal static bool TryLoadConfigurationExportFromFile(string filePath, out ConfigurationExport configurationExport)
        {
            configurationExport = null;

            // Try loading as full ConfigurationExport first
            try
            {
                ConfigurationExport full = ConfigurationManager.LoadFromFile<ConfigurationExport>(filePath);
                if (!full.IsEqualByJsonComparison(new ConfigurationExport()))
                {
                    configurationExport = full;
                    return true;
                }
            }
            catch { }
            // Try loading as individual config objects
            // This way we can also import the files stored as settings by the user previously
            try
            {
                GenericConfig maybeGeneric = ConfigurationManager.LoadFromFile<GenericConfig>(filePath);
                if (!maybeGeneric.IsEqualByJsonComparison(new GenericConfig()))
                {
                    configurationExport = new() { GenericConfig = maybeGeneric };
                    return true;
                }
            }
            catch { }
            try
            {
                LogLayoutsConfig maybeLayouts = ConfigurationManager.LoadFromFile<LogLayoutsConfig>(filePath);
                if (!maybeLayouts.IsEqualByJsonComparison(new LogLayoutsConfig()))
                {
                    configurationExport = new() { LogLayoutsConfig = maybeLayouts };
                    return true;
                }
            }
            catch { }
            try
            {
                LogProvidersConfig maybeProviders = ConfigurationManager.LoadFromFile<LogProvidersConfig>(filePath);
                if (!maybeProviders.IsEqualByJsonComparison(new LogProvidersConfig()))
                {
                    configurationExport = new() { LogProvidersConfig = maybeProviders };
                    return true;
                }
            }
            catch { }

            return false;
        }
    }
}