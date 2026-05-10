using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LogScraper.Controls.Configuration;
using LogScraper.Log.Layout;
using LogScraper.Log.LogAppState;
using LogScraper.LogProviders;
using LogScraper.LogProviders.File;
using LogScraper.LogProviders.Kubernetes;
using LogScraper.LogProviders.Runtime;

namespace LogScraper.Configuration
{
    public partial class FormConfiguration : Form
    {
        private readonly KubernetesConfig oldKubernetesConfig = ConfigAppState.Instance.LogProvidersConfig.Value.KubernetesConfig;
        private readonly RuntimeConfig oldRuntimeConfig = ConfigAppState.Instance.LogProvidersConfig.Value.RuntimeConfig;
        private readonly FileConfig oldFileConfig = ConfigAppState.Instance.LogProvidersConfig.Value.FileConfig;
        private readonly GenericConfig oldGenericConfig = ConfigAppState.Instance.GenericConfig.Value;
        private readonly LogLayoutsConfig logLayoutsConfig = ConfigAppState.Instance.LogLayoutsConfig.Value;

        public FormConfiguration()
        {
            InitializeComponent();
        }

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            userControlKubernetesConfig.SetKubernetesConfig(ConfigAppState.Instance.LogProvidersConfig.Value.KubernetesConfig, ConfigAppState.Instance.LogLayoutsConfig.Value.layouts);
            userControlRuntimeConfig.SetRuntimeConfig(ConfigAppState.Instance.LogProvidersConfig.Value.RuntimeConfig, ConfigAppState.Instance.LogLayoutsConfig.Value.layouts);
            userControlFileConfig.SetFileConfig(ConfigAppState.Instance.LogProvidersConfig.Value.FileConfig, ConfigAppState.Instance.LogLayoutsConfig.Value.layouts);
            userControlGenericConfig.SetGenericConfig(ConfigAppState.Instance.GenericConfig.Value);
            userControlLogLayoutConfig.SetLogLayoutsConfig(ConfigAppState.Instance.LogLayoutsConfig.Value.layouts);
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
            if (!userControlGenericConfig.TryGetGenericConfig(out GenericConfig config)) return;
            if (!userControlLogLayoutConfig.TryGetLogLayoutsConfig(out LogLayoutsConfig logLayoutsConfig)) return;

            // Detect changes against the snapshots taken at form open, before writing to state
            bool genericConfigChanged = !oldGenericConfig.Equals(config);
            bool logLayoutsChanged = !this.logLayoutsConfig.Equals(logLayoutsConfig);
            bool kubernetesChanged = !oldKubernetesConfig.Equals(kubernetesConfig);
            bool runtimeChanged = !oldRuntimeConfig.Equals(runtimeConfig);
            bool fileChanged = !oldFileConfig.Equals(fileConfig);

            bool providerConfigChanged = kubernetesChanged || runtimeChanged || fileChanged;
            if (providerConfigChanged || logLayoutsChanged)
            {
                if (MessageBox.Show("De instellingen zijn gewijzigd. De applicatie wordt gereset om deze toe te passen. Wilt u doorgaan?", "Instellingen gewijzigd", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    LogAppState.Instance.Reset(keepFilters: false);
                }
                else
                {
                    return;
                }
            }

            if (genericConfigChanged) ConfigAppState.Instance.GenericConfig.Set(config);
            if (logLayoutsChanged) ConfigAppState.Instance.LogLayoutsConfig.Set(logLayoutsConfig);
            if (providerConfigChanged)
            {
                LogProvidersConfig providersConfig = ConfigAppState.Instance.LogProvidersConfig.Value;
                providersConfig.KubernetesConfig = kubernetesConfig;
                providersConfig.RuntimeConfig = runtimeConfig;
                providersConfig.FileConfig = fileConfig;
                ConfigAppState.Instance.LogProvidersConfig.ForceSet(providersConfig);
            }

            if (genericConfigChanged) ConfigurationManager.SaveGenericConfig();
            if (logLayoutsChanged) ConfigurationManager.SaveLogLayout();
            if (providerConfigChanged) ConfigurationManager.SaveLogProviders();

            DialogResult = DialogResult.OK;
            Close();
        }

        #region Import / Export

        private void BtnImport_Click(object sender, EventArgs e)
        {
            using FormConfigImportExport form = new();
            if (form.ShowDialog(this) != DialogResult.OK) return;

            ConfigurationExport importConfig = form.SelectedImportConfig;
            List<LogLayout> referencingLogLayouts = null;

            if (form.ImportGeneralSettings && importConfig.GenericConfig != null)
            {
                ConfigurationManager.InitializeGenericConfig(importConfig.GenericConfig);
                userControlGenericConfig.SetGenericConfig(importConfig.GenericConfig);
            }
            if (form.ImportLogLayoutSettings && importConfig.LogLayoutsConfig != null)
            {
                ConfigurationManager.InitializeLogLayoutsConfig(importConfig.LogLayoutsConfig);
                userControlLogLayoutConfig.Clear();
                userControlLogLayoutConfig.SetLogLayoutsConfig(importConfig.LogLayoutsConfig.layouts);
                referencingLogLayouts = importConfig.LogLayoutsConfig.layouts;
            }
            if (form.ImportLogProvidersSettings && importConfig.LogProvidersConfig != null)
            {
                if (referencingLogLayouts == null)
                {
                    if (!userControlLogLayoutConfig.TryGetLogLayoutsConfig(out LogLayoutsConfig logLayoutsConfig)) return;
                    referencingLogLayouts = logLayoutsConfig.layouts;
                }
                ConfigurationManager.InitializeLogProvidersConfig(importConfig.LogProvidersConfig, referencingLogLayouts);
                if (importConfig.LogProvidersConfig.KubernetesConfig != null)
                {
                    userControlKubernetesConfig.Clear();
                    userControlKubernetesConfig.SetKubernetesConfig(importConfig.LogProvidersConfig.KubernetesConfig, referencingLogLayouts);
                }
                if (importConfig.LogProvidersConfig.RuntimeConfig != null)
                {
                    userControlRuntimeConfig.Clear();
                    userControlRuntimeConfig.SetRuntimeConfig(importConfig.LogProvidersConfig.RuntimeConfig, referencingLogLayouts);
                }
                if (importConfig.LogProvidersConfig.FileConfig != null)
                {
                    userControlFileConfig.Clear();
                    userControlFileConfig.SetFileConfig(importConfig.LogProvidersConfig.FileConfig, referencingLogLayouts);
                }
            }
            MessageBox.Show("Succesvol geïmporteerd. Sluit het instellingenscherm met OK om de wijzigingen op te slaan.", "Importeren", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            ConfigurationExport exportObject = new();

            exportObject.GenericConfig = userControlGenericConfig.GetLogScraperConfig();

            if (!userControlLogLayoutConfig.TryGetLogLayoutsConfig(out LogLayoutsConfig logLayoutsConfig)) return;
            exportObject.LogLayoutsConfig = logLayoutsConfig;

            if (!userControlKubernetesConfig.TryGetConfiguration(out KubernetesConfig kubernetesConfig)) return;
            if (!userControlRuntimeConfig.TryGetConfiguration(out RuntimeConfig runtimeConfig)) return;
            if (!userControlFileConfig.TryGetConfiguration(out FileConfig fileConfig)) return;
            exportObject.LogProvidersConfig = new() { KubernetesConfig = kubernetesConfig, RuntimeConfig = runtimeConfig, FileConfig = fileConfig };

            using FormConfigImportExport form = new(exportObject);
            form.ShowDialog(this);
        }

        #endregion
    }
}
