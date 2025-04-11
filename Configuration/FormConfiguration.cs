using System;
using System.Security.Policy;
using System.Windows.Forms;
using LogScraper.Credentials;
using LogScraper.LogProviders.Kubernetes;
using LogScraper.Sources.Adapters.Http;
using LogScraper.Sources.Adapters;
using LogScraper.LogProviders.Runtime;
using LogScraper.LogProviders.File;

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
            userControlRuntimeConfig.SetRuntimeConfig(ConfigurationManager.LogProvidersConfig.RuntimeConfig, ConfigurationManager.LogLayouts);
            userControlFileConfig.SetFileConfig(ConfigurationManager.LogProvidersConfig.FileConfig, ConfigurationManager.LogLayouts);
            userControlGenericConfig.SetGenericConfig(ConfigurationManager.GenericConfig);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!userControlKubernetesConfig.TryGetConfiguration(out KubernetesConfig kubernetesConfig)) return;
            if (!userControlRuntimeConfig.TryGetConfiguration(out RuntimeConfig runtimeConfig)) return;
            if (!userControlFileConfig.TryGetConfiguration(out FileConfig fileConfig)) return;
            if (!userControlGenericConfig.TryGetGenericConfig(out LogScraperConfig config)) return;

            ConfigurationManager.LogProvidersConfig.KubernetesConfig = kubernetesConfig;
            ConfigurationManager.LogProvidersConfig.RuntimeConfig = runtimeConfig;
            ConfigurationManager.LogProvidersConfig.FileConfig = fileConfig;
            ConfigurationManager.GenericConfig = config;
            ConfigurationManager.Save();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
