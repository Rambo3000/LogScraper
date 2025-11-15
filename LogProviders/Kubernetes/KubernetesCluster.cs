using System.Collections.Generic;
using System.Xml.Linq;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace LogScraper.LogProviders.Kubernetes
{
    /// <summary>
    /// Represents a Kubernetes cluster configuration.
    /// This class contains details about the cluster, including its description, base URL, cluster ID, and namespaces.
    /// </summary>
    public class KubernetesCluster
    {
        /// <summary>
        /// Gets or sets the description of the Kubernetes cluster.
        /// This provides a human-readable name or explanation for the cluster.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the base URL of the Kubernetes cluster.
        /// This URL is used to interact with the cluster's API.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the Kubernetes cluster.
        /// This ID is used to distinguish the cluster from others.
        /// </summary>
        public string ClusterId { get; set; }

        /// <summary>
        /// Gets or sets the list of namespaces available in the Kubernetes cluster.
        /// Each namespace represents a logical grouping of resources within the cluster.
        /// </summary>
        public List<KubernetesNamespace> Namespaces { get; set; }

        /// <summary>
        /// Returns the string representation of the Kubernetes cluster.
        /// </summary>
        /// <returns>The description of the cluster.</returns>
        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        /// Creates a deep copy of the current KubernetesCluster instance, including its namespaces.
        /// </summary>
        /// <remarks>The returned cluster is independent of the original; changes to the copy do not
        /// affect the source instance or its namespaces.</remarks>
        /// <returns>A new KubernetesCluster object that is a deep copy of the current instance. All namespace objects are also
        /// copied.</returns>
        public KubernetesCluster Copy()
        {
            KubernetesCluster cluster = new()
            {
                Description = Description,
                BaseUrl = BaseUrl,
                ClusterId = ClusterId,
                Namespaces = []
            };
            foreach (KubernetesNamespace kubernetesNamespace in Namespaces)
            {
                KubernetesNamespace newSpace = new()
                {
                    Description = kubernetesNamespace.Description,
                    Name = kubernetesNamespace.Name,
                    ShortenPodNames = kubernetesNamespace.ShortenPodNames,
                    ShortenPodNamesValues = kubernetesNamespace.ShortenPodNamesValues
                };
                cluster.Namespaces.Add(newSpace);
            }
            return cluster;
        }
    }
}
