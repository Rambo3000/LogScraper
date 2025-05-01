using Newtonsoft.Json;
using LogScraper.Log.Layout;

namespace LogScraper.LogProviders
{
    /// <summary>
    /// Represents the configuration interface for a log provider.
    /// Defines properties and methods required for configuring and identifying log providers.
    /// </summary>
    internal interface ILogProviderConfig
    {
        /// <summary>
        /// Gets or sets the description of the default log layout for the log provider.
        /// This description is used to identify the default layout configuration.
        /// </summary>
        public string DefaultLogLayoutDescription { get; set; }

        /// <summary>
        /// Gets or sets the default log layout for the log provider.
        /// This property is ignored during JSON serialization to avoid circular references.
        /// </summary>
        [JsonIgnore]
        public LogLayout DefaultLogLayout { get; set; }

        /// <summary>
        /// Gets the type of the log provider (e.g., Kubernetes, Runtime, File).
        /// This property is read-only and identifies the specific type of log provider.
        /// </summary>
        public LogProviderType LogProviderType { get; }
    }
}
