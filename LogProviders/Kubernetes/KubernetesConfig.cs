using System.Collections.Generic;
using Newtonsoft.Json;
using LogScraper.Configuration;
using LogScraper.Log.Layout;

namespace LogScraper.LogProviders.Kubernetes
{
    /// <summary>
    /// Represents the configuration for a Kubernetes log provider.
    /// This class implements the <see cref="ILogProviderConfig"/> interface.
    /// </summary>
    public class KubernetesConfig : ILogProviderConfig
    {
        /// <summary>
        /// Gets or sets the description of the default log layout for the Kubernetes log provider.
        /// This description is used to identify the default layout configuration.
        /// </summary>
        public string DefaultLogLayoutDescription { get; set; }

        /// <summary>
        /// Gets or sets the default timespan for fetching logs from the Kubernetes cluster.
        /// The default value is <see cref="KubernetesTimespan.Everything"/>, which fetches all logs.
        /// </summary>
        public KubernetesTimespan DefaultKubernetesTimespan { get; set; } = KubernetesTimespan.Everything;

        /// <summary>
        /// Gets or sets the list of Kubernetes clusters associated with this configuration.
        /// Each cluster contains details such as its description, base URL, and namespaces.
        /// </summary>
        public List<KubernetesCluster> Clusters { get; set; }

        /// <summary>
        /// Gets or sets the default log layout for the Kubernetes log provider.
        /// This property is ignored during JSON serialization and deserialization.
        /// </summary>
        [JsonIgnore]
        public LogLayout DefaultLogLayout { get; set; }

        /// <summary>
        /// Returns a string representation of the Kubernetes log provider configuration.
        /// </summary>
        /// <returns>A string representing the log provider ("Kubernetes").</returns>
        public override string ToString()
        {
            return "Kubernetes";
        }

        /// <summary>
        /// Gets the type of the log provider, which is always <see cref="LogProviderType.Kubernetes"/> for this configuration.
        /// </summary>
        [JsonIgnore]
        public LogProviderType LogProviderType
        {
            get { return LogProviderType.Kubernetes; }
        }
    }
}
