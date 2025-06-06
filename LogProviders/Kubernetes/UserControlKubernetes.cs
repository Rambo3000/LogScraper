using LogScraper.Configuration;
using LogScraper.Credentials;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Adapters.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace LogScraper.LogProviders.Kubernetes
{
    public partial class UserControlKubernetesLogProvider : UserControl, ILogProviderUserControl
    {
        public event EventHandler SourceSelectionChanged;

        public event Action<string, bool> StatusUpdate;

        private List<KubernetesCluster> KubernetesClusters { get; set; }

        public UserControlKubernetesLogProvider()
        {
            InitializeComponent();
            CboKubernetesTimespan.DataSource = Enum.GetValues<KubernetesTimespan>();
            CboKubernetesTimespan.Format += (s, e) =>
            {
                if (e.ListItem is KubernetesTimespan timespan)
                {
                    e.Value = timespan.ToReadableString();
                }
            };
        }

        public void Update(KubernetesConfig kubernetesConfig)
        {
            KubernetesClusters = kubernetesConfig.Clusters;
            CboKubernetesTimespan.SelectedItem = kubernetesConfig.DefaultKubernetesTimespan;
            PopulateKubernetesClusters();
        }
        public ISourceAdapter GetSourceAdapter()
        {
            return GetSourceAdapter(null);
        }
        public ISourceAdapter GetSourceAdapter(string url = null, DateTime? lastTrailTime = null)
        {
            if (Debugger.IsAttached) return SourceAdapterFactory.CreateFileSourceAdapter("Stubs/KubernetesPodLog.txt");

            if (lastTrailTime == null)
            {
                lastTrailTime = ((KubernetesTimespan)CboKubernetesTimespan.SelectedItem).TolastTrailTime();
            }

            KubernetesCluster kubernetesCluster = (KubernetesCluster)cboKubernetesCluster.SelectedItem;

            if (url == null)
            {
                KubernetesNamespace kubernetesNamespace = (KubernetesNamespace)cboKubernetesNamespace.SelectedItem;
                KubernetesPod kubernetesPod = (KubernetesPod)cboKubernetesPod.SelectedItem;

                if (kubernetesCluster == null || kubernetesNamespace == null || kubernetesPod == null) throw new Exception("Er is geen Kubernetes pod geselecteerd");
                url = KubernetesHelper.GetUrlForPodLog(kubernetesCluster, kubernetesNamespace, kubernetesPod);
            }
            return SourceAdapterFactory.CreateHttpSourceAdapter(url, CredentialManager.GenerateTargetLogProvider("Kubernetes", kubernetesCluster.ClusterId), ConfigurationManager.GenericConfig.HttpCLientTimeOUtSeconds, null, TrailType.Kubernetes, lastTrailTime);
        }

        private void PopulateKubernetesClusters()
        {
            cboKubernetesCluster.Items.Clear();
            cboKubernetesNamespace.Items.Clear();
            cboKubernetesPod.Items.Clear();

            cboKubernetesCluster.Items.AddRange([.. KubernetesClusters]);
        }
        private void PopulateKubernetesNamespaces()
        {
            if (cboKubernetesCluster.SelectedItem != null)
            {
                cboKubernetesNamespace.Items.Clear();
                cboKubernetesNamespace.Items.AddRange([.. ((KubernetesCluster)cboKubernetesCluster.SelectedItem).Namespaces]);
                if (cboKubernetesNamespace.Items.Count > 0)
                {
                    cboKubernetesNamespace.SelectedIndex = 0;
                }
            }
        }
        private void PopulateKubernetesPods()
        {
            if (cboKubernetesCluster.SelectedItem != null && cboKubernetesNamespace.SelectedItem != null)
            {
                string selectedKubernetesPodImageName = cboKubernetesPod.SelectedItem == null ? null : ((KubernetesPod)cboKubernetesPod.SelectedItem).ImageName;
                cboKubernetesPod.Items.Clear();
                try
                {
                    ISourceAdapter sourceAdapter;

                    string rawConfig;
                    if (Debugger.IsAttached)
                    {
                        sourceAdapter = SourceAdapterFactory.CreateFileSourceAdapter("Stubs/KubernetesPod.json");
                        rawConfig = sourceAdapter.GetLog();
                    }
                    else
                    {
                        KubernetesCluster kubernetesCluster = (KubernetesCluster)cboKubernetesCluster.SelectedItem;
                        string urlPodsConfiguration = KubernetesHelper.GetUrlForPodConfiguration(kubernetesCluster, (KubernetesNamespace)cboKubernetesNamespace.SelectedItem);
                        sourceAdapter = GetSourceAdapter(urlPodsConfiguration);

                        HttpResponseMessage httpResponseMessage = ((HttpSourceAdapter)sourceAdapter).InitiateClientAndAuthenticate();
                        if (httpResponseMessage == null)
                        {
                            OnStatusUpdate("Failed to connect. No HttpResponseMessage", false);
                            return;
                        }

                        if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                        {
                            OnStatusUpdate(HttpSourceAdapter.ConvertHttpStatusCodeToString(httpResponseMessage), false);
                            return;
                        }

                        rawConfig = httpResponseMessage.Content.ReadAsStringAsync().Result;
                        OnStatusUpdate("Ok", true);
                    }

                    List<KubernetesPod> kubernetesPods = KubernetesHelper.ExtractPodsInfo(rawConfig);

                    cboKubernetesPod.Items.AddRange([.. kubernetesPods]);
                    if (selectedKubernetesPodImageName == null && cboKubernetesPod.Items.Count > 0)
                    {
                        cboKubernetesPod.SelectedIndex = 0;
                    }

                    if (selectedKubernetesPodImageName == null) return;

                    foreach (KubernetesPod kubernetesPod in kubernetesPods)
                    {
                        if (kubernetesPod.ImageName == selectedKubernetesPodImageName)
                        {
                            cboKubernetesPod.SelectedItem = kubernetesPod;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnStatusUpdate(ex.Message, false);
                }
            }
        }

        // Method to raise the custom SourceSelectionChanged event.
        protected virtual void OnSourceSelectionChanged(EventArgs e)
        {
            SourceSelectionChanged?.Invoke(this, e);
        }
        // Method to raise the custom ErrorReadingPodsOccured event.
        protected virtual void OnStatusUpdate(string message, bool isSucces)
        {
            StatusUpdate?.Invoke(message, isSucces);
        }

        private void CboKubernetesCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateKubernetesNamespaces();
        }

        private void CboKubernetesNamespace_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateKubernetesPods();
        }

        private void CboKubernetesPod_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSourceSelectionChanged(EventArgs.Empty);
        }

        private void BtnKubernetesRefresh_Click(object sender, EventArgs e)
        {
            PopulateKubernetesPods();
        }

        KubernetesTimespan? previousTimeSpan = null;
        private void CboKubernetesTimespan_SelectedIndexChanged(object sender, EventArgs e)
        {
            KubernetesTimespan newTimeSpan = (KubernetesTimespan)CboKubernetesTimespan.SelectedItem;
            if (cboKubernetesPod.SelectedIndex != -1 && 
                previousTimeSpan != null && previousTimeSpan != KubernetesTimespan.Everything &&
                ( newTimeSpan == KubernetesTimespan.Everything || previousTimeSpan < newTimeSpan) &&
                MessageBox.Show("Om oudere loggegevens op te halen moeten het log eerst gewist worden, wil je dit doen?", "Log wissen", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OnSourceSelectionChanged(EventArgs.Empty);
            }
            previousTimeSpan = newTimeSpan;
        }
    }
}
