
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LogScraper.LogProviders.Kubernetes;

namespace LogScraper.LogProviders.Kubernetes
{
    public partial class UserControlKubernetesConfig : UserControl
    {

        private BindingList<KubernetesCluster> _clusters = new BindingList<KubernetesCluster>();
        private BindingList<KubernetesNamespace> _namespaces = new BindingList<KubernetesNamespace>();

        public UserControlKubernetesConfig()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            LstClusters.DataSource = _clusters;
            LstClusters.DisplayMember = "Description";

            LstNamespaces.DataSource = _namespaces;
            LstNamespaces.DisplayMember = "Description";

            TxtClusterDescription.TextChanged += (s, e) => UpdateSelectedCluster();
            TxtClusterBaseUrl.TextChanged += (s, e) => UpdateSelectedCluster();
            TxtClusterId.TextChanged += (s, e) => UpdateSelectedCluster();

            TxtNamespaceDescription.TextChanged += (s, e) => UpdateSelectedNamespace();
            TxtNamespaceName.TextChanged += (s, e) => UpdateSelectedNamespace();
        }

        private void UpdateSelectedCluster()
        {
            if (LstClusters.SelectedItem is KubernetesCluster selected)
            {
                selected.Description = TxtClusterDescription.Text;
                selected.BaseUrl = TxtClusterBaseUrl.Text;
                selected.ClusterId = TxtClusterId.Text;
                LstClusters.DisplayMember = ""; // Force update
                LstClusters.DisplayMember = "Description";
            }
        }

        private void UpdateSelectedNamespace()
        {
            if (LstNamespaces.SelectedItem is KubernetesNamespace selected)
            {
                selected.Description = TxtNamespaceDescription.Text;
                selected.Name = TxtNamespaceName.Text;
                LstNamespaces.DisplayMember = "";
                LstNamespaces.DisplayMember = "Description";
            }
        }

        internal bool TryGetConfiguration(out KubernetesConfig config)
        {
            config = new KubernetesConfig
            {
                Clusters = _clusters.ToList(),
                DefaultLogLayoutDescription = "",
                DefaultLogLayout = null
            };

            foreach (KubernetesCluster cluster in config.Clusters)
            {
                if (string.IsNullOrWhiteSpace(cluster.Description) ||
                    string.IsNullOrWhiteSpace(cluster.BaseUrl) ||
                    string.IsNullOrWhiteSpace(cluster.ClusterId))
                {
                    MessageBox.Show("Elke cluster moet een Description, BaseUrl en ClusterId hebben.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (cluster.Namespaces == null || cluster.Namespaces.Count == 0)
                {
                    MessageBox.Show($"Cluster '{cluster.Description}' bevat geen namespaces.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                foreach (KubernetesNamespace ns in cluster.Namespaces)
                {
                    if (string.IsNullOrWhiteSpace(ns.Description) || string.IsNullOrWhiteSpace(ns.Name))
                    {
                        MessageBox.Show($"Elke namespace in cluster '{cluster.Description}' moet een naam en beschrijving hebben.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }

            return true;
        }

        private void BtnAddCluster_Click(object sender, EventArgs e)
        {
            KubernetesCluster cluster = new KubernetesCluster
            {
                Description = "Nieuwe cluster",
                BaseUrl = string.Empty,
                ClusterId = string.Empty,
                Namespaces = new()
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
            KubernetesCluster selected = LstClusters.SelectedItem as KubernetesCluster;
            if (selected == null) return;

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
            KubernetesCluster selected = LstClusters.SelectedItem as KubernetesCluster;
            if (selected == null) return;

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
            if (LstClusters.SelectedItem is KubernetesCluster cluster)
            {
                KubernetesNamespace ns = new KubernetesNamespace
                {
                    Description = "Nieuwe namespace",
                    Name = ""
                };
                cluster.Namespaces.Add(ns);
                _namespaces.ResetBindings();
                LstNamespaces.SelectedItem = ns;
            }
        }

        private void BtnRemoveNamespace_Click(object sender, EventArgs e)
        {
            if (LstNamespaces.SelectedItem is KubernetesNamespace ns)
            {
                _namespaces.Remove(ns);
            }
        }

        private void BtnNamespaceUp_Click(object sender, EventArgs e)
        {
            KubernetesNamespace selected = LstNamespaces.SelectedItem as KubernetesNamespace;
            if (selected == null) return;

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
            KubernetesNamespace selected = LstNamespaces.SelectedItem as KubernetesNamespace;
            if (selected == null) return;

            int index = _namespaces.IndexOf(selected);
            if (index < _namespaces.Count - 1)
            {
                _namespaces.RemoveAt(index);
                _namespaces.Insert(index + 1, selected);
                LstNamespaces.SelectedIndex = index + 1;
            }
        }

        private void LstClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            KubernetesCluster selected = LstClusters.SelectedItem as KubernetesCluster;
            if (selected != null)
            {
                TxtClusterDescription.Text = selected.Description;
                TxtClusterBaseUrl.Text = selected.BaseUrl;
                TxtClusterId.Text = selected.ClusterId;

                _namespaces = new BindingList<KubernetesNamespace>(selected.Namespaces ?? new List<KubernetesNamespace>());
                LstNamespaces.DataSource = _namespaces;
            }
        }

        private void LstNamespaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            KubernetesNamespace selected = LstNamespaces.SelectedItem as KubernetesNamespace;
            if (selected != null)
            {
                TxtNamespaceDescription.Text = selected.Description;
                TxtNamespaceName.Text = selected.Name;
            }
        }
    }
}

