using System.Collections.Generic;
using Newtonsoft.Json;
using LogScraper.Log.Layout;

namespace LogScraper.LogProviders.Runtime
{
    /// <summary>
    /// Represents the configuration for a runtime log provider.
    /// This class implements the <see cref="ILogProviderConfig"/> interface.
    /// </summary>
    internal class RuntimeConfig : ILogProviderConfig
    {
        /// <summary>
        /// Gets or sets the description of the default log layout for the runtime log provider.
        /// This description is used to identify the default layout configuration.
        /// </summary>
        public string DefaultLogLayoutDescription { get; set; }

        /// <summary>
        /// Gets or sets the default log layout for the runtime log provider.
        /// This property is ignored during JSON serialization to avoid circular references.
        /// </summary>
        [JsonIgnore]
        public LogLayout DefaultLogLayout { get; set; }

        /// <summary>
        /// Gets or sets the list of runtime instances associated with this configuration.
        /// Each instance contains details such as its description and runtime log URL.
        /// </summary>
        public List<RuntimeInstance> Instances { get; set; }

        /// <summary>
        /// Returns a string representation of the runtime log provider configuration.
        /// </summary>
        /// <returns>A string representing the log provider ("Directe URL").</returns>
        public override string ToString()
        {
            return "Directe URL";
        }

        /// <summary>
        /// Gets the type of the log provider, which is always <see cref="LogProviderType.Runtime"/> for this configuration.
        /// </summary>
        [JsonIgnore]
        public LogProviderType LogProviderType
        {
            get { return LogProviderType.Runtime; }
        }
    }
}
