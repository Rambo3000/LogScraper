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
        /// Returns the string representation of the namespace.
        /// </summary>
        /// <returns>The description of the namespace.</returns>
        public override string ToString()
        {
            return Description;
        }
    }
}
