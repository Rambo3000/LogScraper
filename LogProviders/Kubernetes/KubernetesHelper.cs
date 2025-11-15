using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogScraper.LogProviders.Kubernetes
{
    /// <summary>
    /// Provides helper methods for interacting with Kubernetes clusters, namespaces, and pods.
    /// Includes methods for generating URLs and extracting pod information from JSON responses.
    /// </summary>
    internal class KubernetesHelper
    {
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
        /// <param name="ShortenPodNamesValues">A list of values used to shorten pod names for descriptions.</param>
        /// <returns>A list of <see cref="KubernetesPod"/> objects containing extracted pod details.</returns>
        internal static List<KubernetesPod> ExtractPodsInfo(string jsonFromPodsResponse, List<string> shortenPodNamesValues)
        {
            // Initialize an empty list to store pod information.
            List<KubernetesPod> podsInfo = [];

            try
            {
                // Parse the JSON response into a JObject.
                JObject jsonObject = JObject.Parse(jsonFromPodsResponse);

                // Extract the array of pod items from the JSON.
                JArray podItems = (JArray)jsonObject["items"];

                // Iterate through each pod item in the array.
                foreach (JObject podItem in podItems.Cast<JObject>())
                {
                    // Extract the pod name from the metadata.
                    string podName = podItem["metadata"]["name"].ToString();

                    // Initialize the shortened pod name.
                    string podNameShortened = podName;
                    if (shortenPodNamesValues != null && shortenPodNamesValues.Count > 0)
                    {
                        // Shorten the pod name for description if applicable.
                        foreach (string shortenValue in shortenPodNamesValues)
                        {
                            podNameShortened = podNameShortened.Replace(shortenValue, "");
                        }
                    }

                    // Extract the container image and split it to retrieve the image name.
                    string containerImage = podItem["spec"]["containers"][0]["image"].ToString();
                    string[] imageParts = containerImage.Split('/');
                    string imageName = imageParts[^1].Split(':')[0];

                    // Create a new KubernetesPod object with the extracted details.
                    KubernetesPod podInfo = new()
                    {
                        Description = podName,
                        DescriptionShortened = podNameShortened,
                        Name = podName,
                        ImageName = imageName
                    };

                    // Add the pod information to the list.
                    podsInfo.Add(podInfo);
                }
            }
            catch (Exception ex)
            {
                // Log an error message if JSON parsing fails.
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }

            // Return the list of extracted pod information.
            return podsInfo;
        }
    }
}
