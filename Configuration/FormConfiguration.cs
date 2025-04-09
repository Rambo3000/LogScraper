using System;
using System.Security.Policy;
using System.Windows.Forms;
using LogScraper.Credentials;
using LogScraper.LogProviders.Kubernetes;
using LogScraper.Sources.Adapters.Http;
using LogScraper.Sources.Adapters;
using LogScraper.LogProviders.Runtime;

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
            userControlRuntimeConfig1.SetRuntimeConfig(ConfigurationManager.LogProvidersConfig.RuntimeConfig, ConfigurationManager.LogLayouts);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!userControlKubernetesConfig.TryGetConfiguration(out KubernetesConfig kubernetesConfig)) return;
            if (!userControlRuntimeConfig1.TryGetConfiguration(out RuntimeConfig runtimeConfig)) return;

            ConfigurationManager.LogProvidersConfig.KubernetesConfig = kubernetesConfig;
            ConfigurationManager.LogProvidersConfig.RuntimeConfig = runtimeConfig;
            ConfigurationManager.Save();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
