using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogScraper.LogProviders.Kubernetes
{
    internal class KubernetesHelper
    {
        internal static string GetUrlForPodConfiguration(KubernetesCluster kubernetesCluster, KubernetesNamespace kubernetesNamespace)
        {
            return kubernetesCluster.BaseUrl + "/clusters/" + kubernetesCluster.ClusterId + "/api/v1/namespaces/" + kubernetesNamespace.Name + "/pods";
        }
        internal static string GetUrlForPodLog(KubernetesCluster kubernetesCluster, KubernetesNamespace kubernetesNamespace, KubernetesPod kubernetesPod)
        {
            return GetUrlForPodConfiguration(kubernetesCluster, kubernetesNamespace) + "/" + kubernetesPod.Name + "/log";
        }
        internal static List<KubernetesPod> ExtractPodsInfo(string jsonFromPodsResponse)
        {
            List<KubernetesPod> podsInfo = [];

            try
            {
                JObject jsonObject = JObject.Parse(jsonFromPodsResponse);
                JArray podItems = (JArray)jsonObject["items"];

                foreach (JObject podItem in podItems.Cast<JObject>())
                {
                    string podName = podItem["metadata"]["name"].ToString();
                    string containerImage = podItem["spec"]["containers"][0]["image"].ToString();
                    string[] imageParts = containerImage.Split('/');
                    string imageName = imageParts[^1].Split(':')[0];
                    string version = imageParts[^1].Split(':')[1];

                    string description = podName.Replace("baas-umbrella-deployment-", "") + " (" + version + ")";

                    KubernetesPod podInfo = new()
                    {
                        Description = description, // You can update this as needed
                        Name = podName,
                        Version = version,
                        ImageName = imageName
                    };

                    podsInfo.Add(podInfo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }

            return podsInfo;
        }
    }
}
