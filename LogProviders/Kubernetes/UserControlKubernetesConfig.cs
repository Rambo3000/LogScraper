using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Credentials;
using LogScraper.Log;
using LogScraper.Sources.Adapters.Http;
using LogScraper.Sources.Adapters;
using LogScraper.Configuration;

namespace LogScraper.LogProviders.Kubernetes
{
    public partial class UserControlKubernetesConfig : UserControl
    {
        private readonly BindingList<KubernetesCluster> _clusters = [];
        private BindingList<KubernetesNamespace> _namespaces = [];

        public UserControlKubernetesConfig()
        {
            InitializeComponent();
        }
        internal void SetKubernetesConfig(KubernetesConfig config, List<LogLayout> logLayouts)
        {
            CboLogLayout.Items.Clear();
            foreach (var layout in logLayouts)
            {
                CboLogLayout.Items.Add(layout);
                if (config.DefaultLogLayout != null && layout == config.DefaultLogLayout)
                {
                    CboLogLayout.SelectedItem = layout;
                }
            }

            _clusters.Clear();
            LstClusters.SelectedIndex = -1;

            // Copy clusters and namespace so we dont mix them with the ones already in the config
            if (config != null && config.Clusters != null)
            {
                foreach (var cluster in config.Clusters)
                {
                    List<KubernetesNamespace> namespaces = [];
                    foreach (var ns in cluster.Namespaces)
                    {
                        KubernetesNamespace nsNew = new()
                        {
                            Description = ns.Description,
                            Name = ns.Name
                        };
                        namespaces.Add(nsNew);
                    }

                    KubernetesCluster clusterNew = new()
                    {
                        Description = cluster.Description,
                        BaseUrl = cluster.BaseUrl,
                        ClusterId = cluster.ClusterId,
                        Namespaces = namespaces
                    };
                    _clusters.Add(clusterNew);
                }
                LstClusters.DataSource = _clusters;
                LstClusters.DisplayMember = "";
                LstClusters.DisplayMember = "Description";
                if (config.Clusters.Count > 0) LstClusters.SelectedIndex = 0;
            }
        }
        internal bool TryGetConfiguration(out KubernetesConfig config)
        {
            List<string> errorMessages = [];

            if (CboLogLayout.SelectedIndex == -1)
            {
                errorMessages.Add($"De standaard layout moet geselecteerd zijn.");
            }

            foreach (KubernetesCluster cluster in _clusters)
            {
                if (string.IsNullOrWhiteSpace(cluster.Description) ||
                    string.IsNullOrWhiteSpace(cluster.BaseUrl) ||
                    string.IsNullOrWhiteSpace(cluster.ClusterId))
                {
                    errorMessages.Add($"Cluster '{cluster.Description}' moet een Description, BaseUrl en ClusterId hebben.");
                }

                if (cluster.Namespaces == null || cluster.Namespaces.Count == 0)
                {
                    errorMessages.Add($"Cluster '{cluster.Description}' bevat geen namespaces.");
                }
                else
                {
                    foreach (KubernetesNamespace ns in cluster.Namespaces)
                    {
                        if (string.IsNullOrWhiteSpace(ns.Description) || string.IsNullOrWhiteSpace(ns.Name))
                        {
                            errorMessages.Add($"Namespace in cluster '{cluster.Description}' moet een naam en beschrijving hebben.");
                        }
                    }
                }
            }

            config = null;
            if (errorMessages.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorMessages), "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            config = new KubernetesConfig
            {
                Clusters = [.. _clusters],
                DefaultLogLayout = CboLogLayout.SelectedItem as LogLayout,
                DefaultLogLayoutDescription = (CboLogLayout.SelectedItem as LogLayout).Description
            };

            return true;
        }

        private void BtnAddCluster_Click(object sender, EventArgs e)
        {
            KubernetesCluster cluster = new()
            {
                Description = "Nieuwe cluster",
                BaseUrl = string.Empty,
                ClusterId = string.Empty,
                Namespaces = []
            };
            _clusters.Add(cluster);
            LstClusters.SelectedItem = cluster;
            BtnAddNamespace_Click(null, null);
            LstNamespaces_SelectedIndexChanged(null, null);
            UpdateButtons();
        }

        private void BtnRemoveCluster_Click(object sender, EventArgs e)
        {
            if (LstClusters.SelectedItem is KubernetesCluster cluster)
            {
                _clusters.Remove(cluster);
            }
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            BtnRemoveCluster.Enabled = LstClusters.Items.Count > 1;
            BtnClusterUp.Enabled = LstClusters.SelectedIndex > 0;
            BtnClusterDown.Enabled = LstClusters.SelectedIndex != -1 && LstClusters.SelectedIndex < (LstClusters.Items.Count - 1);

            BtnRemoveNamespace.Enabled = LstNamespaces.Items.Count > 1;
            BtnNamespaceUp.Enabled = LstNamespaces.SelectedIndex > 0;
            BtnNamespaceDown.Enabled = LstNamespaces.SelectedIndex != -1 && LstNamespaces.SelectedIndex < (LstNamespaces.Items.Count - 1);
            
            UpdateButtonTest();
        }

        private void UpdateButtonTest()
        {
            BtnTest.Enabled = LstClusters.SelectedItem != null && LstNamespaces.SelectedItem != null
                && !string.IsNullOrWhiteSpace(TxtNamespaceName.Text)
                && !string.IsNullOrWhiteSpace(TxtClusterId.Text)
                && !string.IsNullOrWhiteSpace(TxtClusterBaseUrl.Text);
        }

        private void BtnClusterUp_Click(object sender, EventArgs e)
        {
            if (LstClusters.SelectedItem is not KubernetesCluster selected) return;

            int index = _clusters.IndexOf(selected);
            if (index > 0)
            {
                _clusters.RemoveAt(index);
                _clusters.Insert(index - 1, selected);
                LstClusters.SelectedIndex = index - 1;
            }
            UpdateButtons();
        }

        private void BtnClusterDown_Click(object sender, EventArgs e)
        {
            if (LstClusters.SelectedItem is not KubernetesCluster selected) return;

            int index = _clusters.IndexOf(selected);
            if (index < _clusters.Count - 1)
            {
                _clusters.RemoveAt(index);
                _clusters.Insert(index + 1, selected);
                LstClusters.SelectedIndex = index + 1;
            }
            UpdateButtons();
        }

        private void BtnAddNamespace_Click(object sender, EventArgs e)
        {
            KubernetesNamespace ns = new()
            {
                Description = "Nieuwe namespace",
                Name = ""
            };

            _namespaces.Add(ns);

            if (LstClusters.SelectedItem is KubernetesCluster cluster)
            {
                cluster.Namespaces = [.. _namespaces];
            }
            LstNamespaces.SelectedItem = ns;
            UpdateButtons();
        }

        private void BtnRemoveNamespace_Click(object sender, EventArgs e)
        {
            if (LstNamespaces.SelectedItem is KubernetesNamespace ns)
            {
                _namespaces.Remove(ns);
                if (LstClusters.SelectedItem is KubernetesCluster cluster)
                {
                    cluster.Namespaces = [.. _namespaces];
                }
            }
            UpdateButtons();
        }

        private void BtnNamespaceUp_Click(object sender, EventArgs e)
        {
            if (LstNamespaces.SelectedItem is not KubernetesNamespace selected) return;

            int index = _namespaces.IndexOf(selected);
            if (index > 0)
            {
                _namespaces.RemoveAt(index);
                _namespaces.Insert(index - 1, selected);
                LstNamespaces.SelectedIndex = index - 1;

                if (LstClusters.SelectedItem is KubernetesCluster cluster)
                {
                    cluster.Namespaces = [.. _namespaces];
                }
            }
            UpdateButtons();
        }

        private void BtnNamespaceDown_Click(object sender, EventArgs e)
        {
            if (LstNamespaces.SelectedItem is not KubernetesNamespace selected) return;

            int index = _namespaces.IndexOf(selected);
            if (index < _namespaces.Count - 1)
            {
                _namespaces.RemoveAt(index);
                _namespaces.Insert(index + 1, selected);

                if (LstClusters.SelectedItem is KubernetesCluster cluster)
                {
                    cluster.Namespaces = [.. _namespaces];
                }
                LstNamespaces.SelectedIndex = index + 1;
            }
            UpdateButtons();
        }

        private bool UpdatingClusterInformation = false;
        private void LstClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstClusters.SelectedItem is KubernetesCluster selected)
            {
                UpdatingClusterInformation = true;
                TxtClusterDescription.Text = selected.Description;
                TxtClusterBaseUrl.Text = selected.BaseUrl;
                TxtClusterId.Text = selected.ClusterId;

                _namespaces = [.. selected.Namespaces ?? []];
                LstNamespaces.DataSource = _namespaces;
                LstNamespaces.DisplayMember = "";
                LstNamespaces.DisplayMember = "Description";
                UpdatingClusterInformation = false;
            }
            UpdateButtons();
        }

        private bool UpdatingNamespaceInformation = false;
        private void LstNamespaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstNamespaces.SelectedItem is KubernetesNamespace selected)
            {
                UpdatingNamespaceInformation = true;
                TxtNamespaceDescription.Text = selected.Description;
                TxtNamespaceName.Text = selected.Name; ;
                UpdatingNamespaceInformation = false;
            }
            UpdateButtons();
        }

        private void TxtClusterId_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingClusterInformation) return;

            if (LstClusters.SelectedItem is KubernetesCluster selected) selected.ClusterId = TxtClusterId.Text;
            UpdateButtonTest();
        }

        private void TxtClusterDescription_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingClusterInformation) return;

            if (LstClusters.SelectedItem is KubernetesCluster selected) selected.Description = TxtClusterDescription.Text;

            LstClusters.DisplayMember = ""; // Force update
            LstClusters.DisplayMember = "Description";
        }

        private void TxtClusterBaseUrl_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingClusterInformation) return;

            if (LstClusters.SelectedItem is KubernetesCluster selected) selected.BaseUrl = TxtClusterBaseUrl.Text;
            UpdateButtonTest();
        }

        private void TxtNamespaceName_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingClusterInformation || UpdatingNamespaceInformation) return;

            if (LstNamespaces.SelectedItem is KubernetesNamespace selected) selected.Name = TxtNamespaceName.Text;
            UpdateButtonTest();
        }

        private void TxtNamespaceDescription_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingClusterInformation || UpdatingNamespaceInformation) return;

            if (LstNamespaces.SelectedItem is KubernetesNamespace selected) selected.Description = TxtNamespaceDescription.Text;
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            string url = string.Empty;
            try
            {
                BtnTest.Enabled = false;
                KubernetesCluster kubernetesCluster = LstClusters.SelectedItem as KubernetesCluster;
                KubernetesNamespace kubernetesNamespace = LstNamespaces.SelectedItem as KubernetesNamespace;
                url = KubernetesHelper.GetUrlForPodConfiguration(kubernetesCluster, kubernetesNamespace);

                ISourceAdapter sourceAdapter = SourceAdapterFactory.CreateHttpSourceAdapter(url, CredentialManager.GenerateTargetLogProvider("Kubernetes", kubernetesCluster.ClusterId), ConfigurationManager.GenericConfig.HttpCLientTimeOUtSeconds, TrailType.Kubernetes, null);
                sourceAdapter.GetLog();

                TxtTestMessage.Text = "Succes";
                TxtTestMessage.Text += Environment.NewLine + url;
                TxtTestMessage.ForeColor = System.Drawing.Color.DarkGreen;
            }
            catch (Exception exception)
            {
                TxtTestMessage.Text = "Fout:" + exception.Message;
                TxtTestMessage.Text += Environment.NewLine + url;
                TxtTestMessage.ForeColor = System.Drawing.Color.DarkRed;
            }
            finally
            {
                BtnTest.Enabled = true;
            }
        }
    }
}