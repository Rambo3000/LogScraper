using System;
using System.Collections.Generic;
using System.Text.Json;
using LogScraper.Utilities.Extensions;

namespace LogScraper.LogProviders.Kubernetes
{
    /// <summary>
    /// Provides helper methods for interacting with Kubernetes clusters, namespaces, and pods.
    /// Includes methods for generating URLs and extracting pod information from JSON responses.
    /// </summary>
    internal class KubernetesHelper
    {
        /// <summary>
        /// Gets the JSON serializer options used for serializing and deserializing JSON data.
        /// </summary>
        /// <remarks>The options are configured to be case-insensitive when matching property names,
        /// allowing for more flexible JSON handling.</remarks>
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Generates the URL for retrieving the configuration of pods in a specific namespace within a Kubernetes cluster.
        /// </summary>
        /// <param name="kubernetesCluster">The Kubernetes cluster containing the namespace.</param>
        /// <param name="kubernetesNamespace">The namespace within the cluster.</param>
        /// <returns>The URL for retrieving pod configuration.</returns>
        internal static string GetUrlForPodConfiguration(KubernetesCluster kubernetesCluster, KubernetesNamespace kubernetesNamespace)
        {
            return kubernetesCluster.BaseUrl + "/clusters/" + kubernetesCluster.ClusterId + "/api/v1/namespaces/" + kubernetesNamespace.Name + "/pods";
        }

        /// <summary>
        /// Generates the URL for retrieving logs of a specific pod in a namespace within a Kubernetes cluster.
        /// </summary>
        /// <param name="kubernetesCluster">The Kubernetes cluster containing the namespace.</param>
        /// <param name="kubernetesNamespace">The namespace containing the pod.</param>
        /// <param name="kubernetesPod">The pod for which logs are to be retrieved.</param>
        /// <returns>The URL for retrieving pod logs.</returns>
        internal static string GetUrlForPodLog(KubernetesCluster kubernetesCluster, KubernetesNamespace kubernetesNamespace, KubernetesPod kubernetesPod)
        {
            return GetUrlForPodConfiguration(kubernetesCluster, kubernetesNamespace) + "/" + kubernetesPod.Name + "/log";
        }

        /// <summary>
        /// Extracts information about Kubernetes pods from a JSON response.
        /// Parses the JSON to retrieve details such as pod name, image name, version, and description.
        /// </summary>
        /// <param name="jsonFromPodsResponse">The JSON response containing pod information.</param>
        /// <returns>A list of <see cref="KubernetesPod"/> objects containing extracted pod details.</returns>
        internal static List<KubernetesPod> ExtractPodsInfo(string jsonFromPodsResponse, List<string> shortenPodNamesValues)
        {
            List<KubernetesPod> podsInfo = [];
            try
            {
                KubernetesPodsResponse response = JsonSerializer.Deserialize<KubernetesPodsResponse>(jsonFromPodsResponse, jsonOptions);
                foreach (KubernetesPodItem podItem in response.Items)
                {
                    string podName = podItem.Metadata.Name;
                    string podNameShortened = podName;
                    if (shortenPodNamesValues != null && shortenPodNamesValues.Count > 0)
                    {
                        foreach (string shortenValue in shortenPodNamesValues)
                            podNameShortened = podNameShortened.Replace(shortenValue, "");
                    }

                    string containerImage = podItem.Spec.Containers[0].Image;
                    string[] imageParts = containerImage.Split('/');
                    string imageName = imageParts[^1].Split(':')[0];

                    podsInfo.Add(new KubernetesPod()
                    {
                        Description = podName,
                        DescriptionShortened = podNameShortened,
                        Name = podName,
                        ImageName = imageName
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
                ex.LogStackTraceToFile("Error during JSON parsing in ExtractPodsInfo method.");
            }
            return podsInfo;
        }

        private class KubernetesPodsResponse
        {
            public List<KubernetesPodItem> Items { get; set; }
        }

        private class KubernetesPodItem
        {
            public KubernetesPodMetadata Metadata { get; set; }
            public KubernetesPodSpec Spec { get; set; }
        }

        private class KubernetesPodMetadata
        {
            public string Name { get; set; }
        }

        private class KubernetesPodSpec
        {
            public List<KubernetesPodContainer> Containers { get; set; }
        }

        private class KubernetesPodContainer
        {
            public string Image { get; set; }
        }
    }
}