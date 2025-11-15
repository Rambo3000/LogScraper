using System.Collections.Generic;

namespace LogScraper.LogProviders.Kubernetes
{
    /// <summary>
    /// Represents a namespace within a Kubernetes cluster.
    /// A namespace is a logical grouping of resources in a Kubernetes cluster.
    /// </summary>
    public class KubernetesNamespace
    {
        /// <summary>
        /// Gets or sets the description of the namespace.
        /// This provides a human-readable name or explanation for the namespace.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the namespace.
        /// This is the unique identifier for the namespace within the cluster.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pod names should be shortened in the user interface.
        /// </summary>
        /// <remarks>When enabled, pod names will be truncated or abbreviated to improve readability,
        /// especially in environments where full pod names are lengthy or contain redundant information. This setting
        /// does not affect the actual pod identifiers used internally.</remarks>
        public bool ShortenPodNames { get; set; }

        /// <summary>
        /// Gets or sets the collection of values used to shorten pod names. This list is only used if <see cref="ShortenPodNames"/> is set to true.
        /// </summary>
        public List<string> ShortenPodNamesValues { get; set; }

        /// <summary>
        /// Returns the string representation of the namespace.
        /// </summary>
        /// <returns>The description of the namespace.</returns>
        public override string ToString()
        {
            return Description;
        }
    }
}
