using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Linq;
using LogScraper.Configuration;
using LogScraper.Credentials;
using LogScraper.Log.Layout;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Adapters.Http;

namespace LogScraper.LogProviders.Kubernetes
{
    public partial class UserControlKubernetesConfig : UserControl
    {
        private readonly BindingList<KubernetesCluster> _clusters = [];
        private BindingList<KubernetesNamespace> _namespaces = [];

        public UserControlKubernetesConfig()
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
        public void Clear()
        {
            LstClusters.SelectedIndex = -1;
            _clusters.Clear();
        }
        internal void SetKubernetesConfig(KubernetesConfig config, List<LogLayout> logLayouts)
        {
            CboLogLayout.Items.Clear();
            foreach (var layout in logLayouts)
            {
                CboLogLayout.Items.Add(layout);
                if (config.DefaultLogLayout != null && layout.Description == config.DefaultLogLayout.Description)
                {
                    CboLogLayout.SelectedItem = layout;
                }
            }

            CboKubernetesTimespan.SelectedItem = config.DefaultKubernetesTimespan;

            _clusters.Clear();
            LstClusters.SelectedIndex = -1;

            // Copy clusters and namespace so we dont mix them with the ones already in the config
            if (config != null && config.Clusters != null)
            {
                foreach (var cluster in config.Clusters)
                {
                    _clusters.Add(cluster.Copy());
                }
                LstClusters.DataSource = _clusters;
                LstClusters.DisplayMember = string.Empty;
                LstClusters.DisplayMember = "Description";
                if (config.Clusters.Count > 0) LstClusters.SelectedIndex = 0;
            }
        }
        internal bool TryGetConfiguration(out KubernetesConfig config)
        {
            List<string> errorMessages = [];

            if (CboLogLayout.SelectedIndex == -1)
            {
                errorMessages.Add($"De standaard layout voor Kubernetes moet geselecteerd zijn.");
            }

            foreach (KubernetesCluster cluster in _clusters)
            {
                if (string.IsNullOrWhiteSpace(cluster.Description) ||
                    string.IsNullOrWhiteSpace(cluster.BaseUrl) ||
                    string.IsNullOrWhiteSpace(cluster.ClusterId))
                {
                    errorMessages.Add($"Kubernetes cluster '{cluster.Description}' moet een Description, BaseUrl en ClusterId hebben.");
                }

                if (cluster.Namespaces == null || cluster.Namespaces.Count == 0)
                {
                    errorMessages.Add($"Kubernetes cluster '{cluster.Description}' bevat geen namespaces.");
                }
                else
                {
                    foreach (KubernetesNamespace ns in cluster.Namespaces)
                    {
                        if (string.IsNullOrWhiteSpace(ns.Description) || string.IsNullOrWhiteSpace(ns.Name))
                        {
                            errorMessages.Add($"Kubernetes namespaces in cluster '{cluster.Description}' moeten een naam en beschrijving hebben.");
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
                DefaultKubernetesTimespan = (KubernetesTimespan)CboKubernetesTimespan.SelectedItem,
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
                Name = string.Empty
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
            BtnCopy.Enabled = LstClusters.SelectedItem != null;
            if (LstClusters.SelectedItem is KubernetesCluster selected)
            {
                UpdatingClusterInformation = true;
                TxtClusterDescription.Text = selected.Description;
                TxtClusterBaseUrl.Text = selected.BaseUrl;
                TxtClusterId.Text = selected.ClusterId;


                _namespaces = [.. selected.Namespaces ?? []];
                LstNamespaces.DataSource = _namespaces;
                LstNamespaces.DisplayMember = string.Empty;
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
                TxtNamespaceName.Text = selected.Name;
                TxtFilterPodNames.Text = string.Join(" ", selected.ShortenPodNamesValues ?? []);
                ChkFilterPodNames.Checked = selected.ShortenPodNames;
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

            LstClusters.DisplayMember = string.Empty; // Force update
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

            if (LstNamespaces.SelectedItem is KubernetesNamespace selected)
            {
                selected.Description = TxtNamespaceDescription.Text;
                LstNamespaces.DisplayMember = string.Empty;
                LstNamespaces.DisplayMember = "Description"; // Force update
            }
        }

        private void ChkFilterPodNames_CheckedChanged(object sender, EventArgs e)
        {
            TxtFilterPodNames.Enabled = ChkFilterPodNames.Checked;
            if (UpdatingClusterInformation || UpdatingNamespaceInformation) return;

            if (LstNamespaces.SelectedItem is KubernetesNamespace selected)
            {
                selected.ShortenPodNames = ChkFilterPodNames.Checked;
            }
        }

        private void TxtFilterPodNames_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingClusterInformation || UpdatingNamespaceInformation) return;

            if (LstNamespaces.SelectedItem is KubernetesNamespace selected) selected.ShortenPodNamesValues = [.. TxtFilterPodNames.Text.Split(' ')];
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

                ISourceAdapter sourceAdapter = SourceAdapterFactory.CreateHttpSourceAdapter(url, CredentialManager.GenerateTargetLogProvider("Kubernetes", kubernetesCluster.ClusterId), ConfigurationManager.GenericConfig.HttpCLientTimeOUtSeconds, null, TrailType.Kubernetes, null);
                sourceAdapter.GetLog();

                TxtTestMessage.Text = "Succes";
                TxtTestMessage.Text += Environment.NewLine + url;
                TxtTestMessage.ForeColor = System.Drawing.Color.DarkGreen;
            }
            catch (Exception exception)
            {
                TxtTestMessage.Text = exception.Message;
                TxtTestMessage.Text += Environment.NewLine + url;
                TxtTestMessage.ForeColor = System.Drawing.Color.DarkRed;
            }
            finally
            {
                BtnTest.Enabled = true;
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (LstClusters.SelectedItem is not KubernetesCluster selected) return;

            KubernetesCluster newCluster = selected.Copy();
            newCluster.Description = newCluster.Description + " (kopie)";

            _namespaces = [.. newCluster.Namespaces];
            _clusters.Add(newCluster);

            LstClusters.SelectedItem = newCluster;
            LstNamespaces_SelectedIndexChanged(null, null);
        }
    }
}