using System;
using System.Security.Policy;
using System.Windows.Forms;
using LogScraper.Credentials;
using LogScraper.LogProviders.Kubernetes;
using LogScraper.Sources.Adapters.Http;
using LogScraper.Sources.Adapters;

namespace LogScraper.Configuration
{
    public partial class FormConfiguration : Form
    {
        public FormConfiguration()
        {
            InitializeComponent();
        }

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            userControlKubernetesConfig.SetKubernetesConfig(ConfigurationManager.LogProvidersConfig.KubernetesConfig, ConfigurationManager.LogLayouts);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!userControlKubernetesConfig.TryGetConfiguration(out KubernetesConfig kubernetesConfig)) return;

            ConfigurationManager.LogProvidersConfig.KubernetesConfig = kubernetesConfig;
            ConfigurationManager.Save();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
