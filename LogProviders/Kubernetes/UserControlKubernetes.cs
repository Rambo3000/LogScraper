using LogScraper.Configuration;
using LogScraper.Credentials;
using LogScraper.Log;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Adapters.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ISourceAdapter GetSourceAdapter(string url = null, DateTime? lastTrailTime = null, bool authenticate = true)
        {
            if (Debugger.IsAttached) return SourceAdapterFactory.CreateFileSourceAdapter("Stubs/KubernetesPodLog.txt");

            if (lastTrailTime == null)
            {
                lastTrailTime = ((KubernetesTimespan)CboKubernetesTimespan.SelectedItem).TolastTrailTime();
            }

            if (url == null)
            {
                if (SelectedKubernetesCluster == null || SelectedKubernetesNamespace == null || SelectedKubernetesPod == null) throw new Exception("Er is geen Kubernetes pod geselecteerd");
                url = KubernetesHelper.GetUrlForPodLog(SelectedKubernetesCluster, SelectedKubernetesNamespace, SelectedKubernetesPod);
            }
            return SourceAdapterFactory.CreateHttpSourceAdapter(url, CredentialManager.GenerateTargetLogProvider("Kubernetes", SelectedKubernetesCluster.ClusterId), ConfigurationManager.GenericConfig.HttpCLientTimeOUtSeconds, null, TrailType.Kubernetes, lastTrailTime, authenticate);
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
            if (SelectedKubernetesCluster == null) return;

            cboKubernetesNamespace.Items.Clear();
            cboKubernetesNamespace.Items.AddRange([.. SelectedKubernetesCluster.Namespaces]);
            if (cboKubernetesNamespace.Items.Count > 0)
            {
                cboKubernetesNamespace.SelectedIndex = 0;
            }
        }

        private KubernetesCluster SelectedKubernetesCluster
        {
            get
            {
                if (cboKubernetesCluster == null || cboKubernetesCluster.SelectedItem == null) return null;
                return (KubernetesCluster)cboKubernetesCluster.SelectedItem;
            }
        }

        private KubernetesNamespace SelectedKubernetesNamespace
        {
            get
            {
                if (cboKubernetesNamespace == null || cboKubernetesNamespace.SelectedItem == null) return null;
                return (KubernetesNamespace)cboKubernetesNamespace.SelectedItem;
            }
        }

        private KubernetesPod SelectedKubernetesPod
        {
            get
            {
                if (cboKubernetesPod == null || cboKubernetesPod.SelectedItem == null) return null;
                return (KubernetesPod)cboKubernetesPod.SelectedItem;
            }
        }

        private void PopulateKubernetesPods()
        {
            if (SelectedKubernetesCluster == null) return;

            string selectedKubernetesPodImageName = SelectedKubernetesPod?.ImageName;
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
                    string urlPodsConfiguration = KubernetesHelper.GetUrlForPodConfiguration(SelectedKubernetesCluster, SelectedKubernetesNamespace);
                    sourceAdapter = GetSourceAdapter(urlPodsConfiguration, null, false);

                    if (!((HttpSourceAdapter)sourceAdapter).TryInitiateClientAndAuthenticate(out HttpResponseMessage httpResponseMessage, out string errorMessage))
                    {
                        OnStatusUpdate(errorMessage, false);
                        return;
                    }

                    rawConfig = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    OnStatusUpdate("Ok", true);
                }

                List<KubernetesPod> kubernetesPods = KubernetesHelper.ExtractPodsInfo(rawConfig, SelectedKubernetesNamespace.ShortenPodNames ? SelectedKubernetesNamespace.ShortenPodNamesValues : null);

                cboKubernetesPod.Items.AddRange([.. kubernetesPods]);

                //First try to preselect the previously selected pod
                if (selectedKubernetesPodImageName != null)
                {
                    if (selectedKubernetesPodImageName == null) return;

                    foreach (KubernetesPod kubernetesPod in kubernetesPods)
                    {
                        if (kubernetesPod.ImageName == selectedKubernetesPodImageName)
                        {
                            cboKubernetesPod.SelectedItem = kubernetesPod;
                            return;
                        }
                    }
                }

                //Try to select the default pod based on name parts 
                if (selectedKubernetesPodImageName == null && SelectedKubernetesNamespace.DefaultSelectedPodNameParts.Count > 0)
                {
                    foreach (KubernetesPod kubernetesPod in kubernetesPods)
                    {
                        foreach (string searchValuein in SelectedKubernetesNamespace.DefaultSelectedPodNameParts)
                        {
                            if (!string.IsNullOrEmpty(searchValuein) && kubernetesPod.Name.Contains(searchValuein))
                            {
                                cboKubernetesPod.SelectedItem = kubernetesPod;
                                return;
                            }
                        }
                    }
                }

                //Select the first pod if nothing is selected yet
                if (selectedKubernetesPodImageName == null && cboKubernetesPod.Items.Count > 0)
                {
                    cboKubernetesPod.SelectedIndex = 0;
                    return;
                }
            }
            catch (Exception ex)
            {
                OnStatusUpdate(ex.Message, false);
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
            if ( LogCollection.Instance.LogEntries.Count > 0 &&
                cboKubernetesPod.SelectedIndex != -1 &&
                previousTimeSpan != null && previousTimeSpan != KubernetesTimespan.Everything &&
                (newTimeSpan == KubernetesTimespan.Everything || previousTimeSpan < newTimeSpan) &&
                MessageBox.Show("Om oudere loggegevens op te halen moeten het log eerst gewist worden, wil je dit doen?", "Log wissen", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OnSourceSelectionChanged(EventArgs.Empty);
            }
            previousTimeSpan = newTimeSpan;
        }
    }
}
