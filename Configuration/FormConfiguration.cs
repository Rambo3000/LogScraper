using System;
using System.Windows.Forms;
using LogScraper.LogProviders.Kubernetes;
using LogScraper.LogProviders.Runtime;
using LogScraper.LogProviders.File;
using LogScraper.Extensions;

namespace LogScraper.Configuration
{
    public partial class FormConfiguration : Form
    {
        private readonly KubernetesConfig oldKubernetesConfig = ConfigurationManager.LogProvidersConfig.KubernetesConfig;
        private readonly RuntimeConfig oldRuntimeConfig = ConfigurationManager.LogProvidersConfig.RuntimeConfig;
        private readonly LogScraperConfig oldGenericConfig = ConfigurationManager.GenericConfig;
        private readonly LogLayoutsConfig logLayoutsConfig = ConfigurationManager.LogLayoutsConfig;

        public FormConfiguration()
        {
            InitializeComponent();

            string version = Application.ProductVersion;
            if (version.Contains('+')) version = version[..version.IndexOf('+')];

            lblVersion.Text = "v" + version;
        }

        public void GetConfigurationChangedStatus(out bool genericConfigChanged, out bool logLayoutsChanged, out bool kubernetesChanged, out bool runtimeChanged)
        {
            genericConfigChanged = !oldGenericConfig.IsEqualByJsonComparison(ConfigurationManager.GenericConfig);
            logLayoutsChanged = !logLayoutsConfig.IsEqualByJsonComparison(ConfigurationManager.LogLayoutsConfig);

            kubernetesChanged = !oldKubernetesConfig.IsEqualByJsonComparison(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
            runtimeChanged = !oldRuntimeConfig.IsEqualByJsonComparison(ConfigurationManager.LogProvidersConfig.RuntimeConfig);
        }
        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            userControlKubernetesConfig.SetKubernetesConfig(ConfigurationManager.LogProvidersConfig.KubernetesConfig, ConfigurationManager.LogLayouts);
            userControlRuntimeConfig.SetRuntimeConfig(ConfigurationManager.LogProvidersConfig.RuntimeConfig, ConfigurationManager.LogLayouts);
            userControlFileConfig.SetFileConfig(ConfigurationManager.LogProvidersConfig.FileConfig, ConfigurationManager.LogLayouts);
            userControlGenericConfig.SetGenericConfig(ConfigurationManager.GenericConfig);
            userControlLogLayoutConfig.SetLogLayoutsConfig(ConfigurationManager.LogLayouts);
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
            if (!userControlLogLayoutConfig.TryGetLogLayoutsConfig(out LogLayoutsConfig logLayoutsConfig)) return;

            ConfigurationManager.LogProvidersConfig.KubernetesConfig = kubernetesConfig;
            ConfigurationManager.LogProvidersConfig.RuntimeConfig = runtimeConfig;
            ConfigurationManager.LogProvidersConfig.FileConfig = fileConfig;
            ConfigurationManager.LogLayoutsConfig = logLayoutsConfig;
            ConfigurationManager.GenericConfig = config;
            ConfigurationManager.Save();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
