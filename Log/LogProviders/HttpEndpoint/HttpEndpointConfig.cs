using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LogScraper.Log.Generic.Extensions;
using LogScraper.Log.Layout;

namespace LogScraper.Log.LogProviders.HttpEndpoint
{
    /// <summary>
    /// Represents the configuration for an HTTP endpoint log provider.
    /// This class implements the <see cref="ILogProviderConfig"/> interface.
    /// </summary>
    internal class HttpEndpointConfig : ILogProviderConfig, IEquatable<HttpEndpointConfig>
    {
        /// <summary>
        /// Gets or sets the description of the default log layout for the HTTP endpoint log provider.
        /// This description is used to identify the default layout configuration.
        /// </summary>
        public string DefaultLogLayoutDescription { get; set; }

        /// <summary>
        /// Gets or sets the default log layout for the HTTP endpoint log provider.
        /// This property is ignored during JSON serialization to avoid circular references.
        /// </summary>
        [JsonIgnore]
        public LogLayout DefaultLogLayout { get; set; }

        /// <summary>
        /// Gets or sets the list of HTTP endpoint instances associated with this configuration.
        /// Each instance contains details such as its description and log URL.
        /// </summary>
        public List<HttpEndpointInstance> Instances { get; set; }

        /// <summary>
        /// Returns a string representation of the HTTP endpoint log provider configuration.
        /// </summary>
        /// <returns>A string representing the log provider.</returns>
        public override string ToString()
        {
            return "HTTP API";
        }

        /// <summary>
        /// Gets the type of the log provider, which is always <see cref="LogProviderType.HttpEndpoint"/> for this configuration.
        /// </summary>
        [JsonIgnore]
        public LogProviderType LogProviderType
        {
            get { return LogProviderType.HttpEndpoint; }
        }

        public bool Equals(HttpEndpointConfig other) => this.IsEqualByJsonComparison(other);
        public override bool Equals(object obj)
        {
            return Equals(obj as HttpEndpointConfig);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DefaultLogLayoutDescription, Instances);
        }
    }
}
