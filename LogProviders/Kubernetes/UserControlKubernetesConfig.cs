using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using LogScraper.LogProviders.Kubernetes;

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

        internal bool TryGetConfiguration(out KubernetesConfig config)
        {
            config = new KubernetesConfig
            {
                Clusters = [.. _clusters],
                DefaultLogLayoutDescription = "",
                DefaultLogLayout = null
            };

            List<string> errorMessages = [];

            foreach (KubernetesCluster cluster in config.Clusters)
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

            if (errorMessages.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorMessages), "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

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
        }

        private void BtnRemoveCluster_Click(object sender, EventArgs e)
        {
            if (LstClusters.SelectedItem is KubernetesCluster cluster)
            {
                _clusters.Remove(cluster);
            }
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
            }
        }

        private void BtnNamespaceDown_Click(object sender, EventArgs e)
        {
            if (LstNamespaces.SelectedItem is not KubernetesNamespace selected) return;

            int index = _namespaces.IndexOf(selected);
            if (index < _namespaces.Count - 1)
            {
                _namespaces.RemoveAt(index);
                _namespaces.Insert(index + 1, selected);
                LstNamespaces.SelectedIndex = index + 1;
            }
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
        }
        internal KubernetesConfig GetKubernetesConfig()
        {
            return new KubernetesConfig
            {
                Clusters = [.. _clusters],
                DefaultLogLayoutDescription = "",
                DefaultLogLayout = null
            };
        }

        internal void SetKubernetesConfig(KubernetesConfig config)
        {
            _clusters.Clear();
            LstClusters.SelectedIndex = -1;

            // TODO copy clusters and namespace
            if (config != null && config.Clusters != null)
            {
                foreach (var cluster in config.Clusters)
                {
                    _clusters.Add(cluster);
                }
                LstClusters.DataSource = _clusters;
                LstClusters.DisplayMember = "";
                LstClusters.DisplayMember = "Description";
                if (config.Clusters.Count > 0) LstClusters.SelectedIndex = 0;
            }
        }

        private void TxtClusterId_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingClusterInformation) return;

            if (LstClusters.SelectedItem is KubernetesCluster selected) selected.ClusterId = TxtClusterId.Text;
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
        }

        private void TxtNamespaceName_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingClusterInformation || UpdatingNamespaceInformation) return;

            if (LstNamespaces.SelectedItem is KubernetesNamespace selected) selected.Name = TxtNamespaceName.Text;
        }

        private void TxtNamespaceDescription_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingClusterInformation || UpdatingNamespaceInformation) return;

            if (LstNamespaces.SelectedItem is KubernetesNamespace selected) selected.Description = TxtNamespaceDescription.Text;
        }
    }
}