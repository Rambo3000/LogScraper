using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LogScraper.Configuration.Generic;
using LogScraper.Log.Layout;
using LogScraper.LogProviders.File;
using LogScraper.LogProviders.Kubernetes;
using LogScraper.LogProviders.Runtime;
using LogScraper.Utilities.Extensions;
using Newtonsoft.Json;

namespace LogScraper.Configuration
{
    public partial class FormConfiguration : Form
    {
        private readonly KubernetesConfig oldKubernetesConfig = ConfigurationManager.LogProvidersConfig.KubernetesConfig;
        private readonly RuntimeConfig oldRuntimeConfig = ConfigurationManager.LogProvidersConfig.RuntimeConfig;
        private readonly FileConfig oldFileConfig = ConfigurationManager.LogProvidersConfig.FileConfig;
        private readonly GenericConfig oldGenericConfig = ConfigurationManager.GenericConfig;
        private readonly LogLayoutsConfig logLayoutsConfig = ConfigurationManager.LogLayoutsConfig;

        public FormConfiguration()
        {
            InitializeComponent();
        }

        public void GetConfigurationChangedStatus(out bool genericConfigChanged, out bool logLayoutsChanged, out bool kubernetesChanged, out bool runtimeChanged, out bool fileChanged)
        {
            genericConfigChanged = !oldGenericConfig.IsEqualByJsonComparison(ConfigurationManager.GenericConfig);
            logLayoutsChanged = !logLayoutsConfig.IsEqualByJsonComparison(ConfigurationManager.LogLayoutsConfig);

            kubernetesChanged = !oldKubernetesConfig.IsEqualByJsonComparison(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
            runtimeChanged = !oldRuntimeConfig.IsEqualByJsonComparison(ConfigurationManager.LogProvidersConfig.RuntimeConfig);
            fileChanged = !oldRuntimeConfig.IsEqualByJsonComparison(ConfigurationManager.LogProvidersConfig.RuntimeConfig);
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
            if (!userControlGenericConfig.TryGetGenericConfig(out GenericConfig config)) return;
            if (!userControlLogLayoutConfig.TryGetLogLayoutsConfig(out LogLayoutsConfig logLayoutsConfig)) return;

            ConfigurationManager.LogProvidersConfig.KubernetesConfig = kubernetesConfig;
            ConfigurationManager.LogProvidersConfig.RuntimeConfig = runtimeConfig;
            ConfigurationManager.LogProvidersConfig.FileConfig = fileConfig;
            ConfigurationManager.LogLayoutsConfig = logLayoutsConfig;
            ConfigurationManager.GenericConfig = config;

            GetConfigurationChangedStatus(out bool genericConfigChanged, out bool logLayoutsChanged, out bool kubernetesChanged, out bool runtimeChanged, out bool fileChanged);

            if (genericConfigChanged) ConfigurationManager.SaveGenericConfig();
            if (logLayoutsChanged) ConfigurationManager.SaveLogLayout();
            if (kubernetesChanged || runtimeChanged || fileChanged) ConfigurationManager.SaveLogProviders();

            DialogResult = DialogResult.OK;
            Close();
        }

        #region Import

        // Field to store loaded config until the actual import button is pressed later
        private ConfigurationExport pendingImportConfiguration = null;

        private void BtnImportSelectFile_Click(object sender, EventArgs e)
        {
            LblSelectedImportFile.Text = string.Empty;
            pendingImportConfiguration = null;

            OpenFileDialog dialog = new()
            {
                Title = "Select settings file to import",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                InitialDirectory = GetDefaultOpenFolder()
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            string filePath = dialog.FileName;

            try
            {
                bool LoadingSucces = ConfigurationManager.TryLoadConfigurationExportFromFile(filePath, out ConfigurationExport configurationExport);
                if (!LoadingSucces)
                {
                    MessageBox.Show("File did not contain any recognizable settings.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SetImportCheckboxStates(false, false, false);
                    return;
                }
                ConfigurationExport loadedConfigurationExport = configurationExport;

                SetImportCheckboxStates(loadedConfigurationExport.GenericConfig != null, loadedConfigurationExport.LogLayoutsConfig != null, loadedConfigurationExport.LogProvidersConfig != null);
                LblSelectedImportFile.Text = filePath;
                LblSelectedImportFile.ForeColor = Color.Black;

                // Keep 'loaded' somewhere for the actual import step (e.g. a field)
                pendingImportConfiguration = loadedConfigurationExport;
            }
            catch (JsonException jex)
            {
                MessageBox.Show("Import failed - invalid JSON:\n" + jex.Message, "Import error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import failed:\n" + ex.Message, "Import error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private static string GetDefaultOpenFolder()
        {
            string downloadsFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            if (Directory.Exists(downloadsFolder))
            {
                return downloadsFolder;
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        // Helper that sets the checkbox states: checked+enabled when available, otherwise unchecked+disabled.
        private void SetImportCheckboxStates(bool availableGeneric, bool availableLayouts, bool availableProviders)
        {
            ChkImportGeneralSettings.Checked = availableGeneric;
            ChkImportGeneralSettings.Enabled = availableGeneric;

            ChkImportLogLayoutSettings.Checked = availableLayouts;
            ChkImportLogLayoutSettings.Enabled = availableLayouts;

            ChkImportLogProvidersSettings.Checked = availableProviders;
            ChkImportLogProvidersSettings.Enabled = availableProviders;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            // There always should be a pending import configuration when this button is enabled
            if (pendingImportConfiguration == null) return;

            ConfigurationExport importConfig = pendingImportConfiguration;
            if (ChkImportGeneralSettings.Checked && importConfig.GenericConfig != null)
            {
                ConfigurationManager.InitializeGenericConfig(importConfig.GenericConfig);
                userControlGenericConfig.SetGenericConfig(importConfig.GenericConfig);
            }
            List<LogLayout> referencingLogLayouts = null;
            if (ChkImportLogLayoutSettings.Checked && importConfig.LogLayoutsConfig != null)
            {
                ConfigurationManager.InitializeLogLayoutsConfig(importConfig.LogLayoutsConfig);
                userControlLogLayoutConfig.Clear();
                userControlLogLayoutConfig.SetLogLayoutsConfig(importConfig.LogLayoutsConfig.layouts);

                referencingLogLayouts = importConfig.LogLayoutsConfig.layouts;
            }
            if (ChkImportLogProvidersSettings.Checked && importConfig.LogProvidersConfig != null)
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
            LblSelectedImportFile.Text += " succesvol geïmporteren.";
            LblSelectedImportFile.ForeColor = Color.DarkGreen;
            SetImportCheckboxStates(false, false, false);
            MessageBox.Show("Succesvol geïmporteerd. Sluit het instellingen scherm met OK om de wijzigingen op te slaan.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ChkImportGeneralSettings_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonImport();
        }

        private void ChkImportLogLayoutSettings_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonImport();
        }

        private void ChkImportLogProvidersSettings_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonImport();
        }
        private void UpdateButtonImport()
        {
            BtnImport.Enabled = ChkImportGeneralSettings.Checked || ChkImportLogLayoutSettings.Checked || ChkImportLogProvidersSettings.Checked;
        }
        #endregion

        #region Export
        private void BtnExport_Click(object sender, EventArgs e)
        {
            LblExportFileStatus.Text = "";
            if (!TryCreateExportObject(out ConfigurationExport exportObject)) return;

            string filePath = GetExportFilePath();
            if (string.IsNullOrEmpty(filePath))
            {
                // User cancelled
                return;
            }

            if (File.Exists(filePath))
            {
                DialogResult overwriteResult = MessageBox.Show("The file already exists. Do you want to overwrite it?", "Overwrite existing file?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (overwriteResult == DialogResult.No) return;
            }

            try
            {
                ConfigurationManager.SaveToFile(filePath, exportObject);

                LblExportFileStatus.Text += "Succesvol geëxporteerd naar: " + filePath;
                LblExportFileStatus.ForeColor = Color.DarkGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to export settings:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool TryCreateExportObject(out ConfigurationExport configurationExport)
        {
            configurationExport = null;

            if (!(ChkExportGeneralSettings.Checked || ChkExportLogLayoutSettings.Checked || ChkExportLogProvidersSettings.Checked)) return false;

            ConfigurationExport exportObject = new();

            if (ChkExportGeneralSettings.Checked)
            {
                exportObject.GenericConfig = userControlGenericConfig.GetLogScraperConfig();
            }
            if (ChkExportLogLayoutSettings.Checked)
            {
                if (!userControlLogLayoutConfig.TryGetLogLayoutsConfig(out LogLayoutsConfig logLayoutsConfig)) return false;
                exportObject.LogLayoutsConfig = logLayoutsConfig;
            }
            if (ChkExportLogProvidersSettings.Checked)
            {
                exportObject.LogProvidersConfig = new();

                if (!userControlKubernetesConfig.TryGetConfiguration(out KubernetesConfig kubernetesConfig)) return false;
                exportObject.LogProvidersConfig.KubernetesConfig = kubernetesConfig;

                if (!userControlRuntimeConfig.TryGetConfiguration(out RuntimeConfig runtimeConfig)) return false;
                exportObject.LogProvidersConfig.RuntimeConfig = runtimeConfig;

                if (!userControlFileConfig.TryGetConfiguration(out FileConfig fileConfig)) return false;
                exportObject.LogProvidersConfig.FileConfig = fileConfig;
            }

            configurationExport = exportObject;
            return true;
        }
        private string GetExportFilePath()
        {
            SaveFileDialog dialog = new()
            {
                Title = "Exporteer instellingen",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FileName = CreateDefaultExportFileName(),
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                OverwritePrompt = false // we handle overwrite ourselves
            };

            DialogResult result = dialog.ShowDialog(this);
            if (result != DialogResult.OK)
            {
                return null;
            }

            return dialog.FileName;
        }
        private string CreateDefaultExportFileName()
        {
            List<string> parts = [];
            parts.Add("LogScraperSettings");
            // If all settings are selected, do not specify which settings are included
            if (!(ChkExportGeneralSettings.Checked && ChkExportLogLayoutSettings.Checked && ChkExportLogProvidersSettings.Checked))
            {
                if (ChkExportGeneralSettings.Checked)
                {
                    parts.Add("General");
                }

                if (ChkExportLogLayoutSettings.Checked)
                {
                    parts.Add("LogLayouts");
                }

                if (ChkExportLogProvidersSettings.Checked)
                {
                    parts.Add("LogProviders");
                }
            }

            string fileName = string.Join("_", parts) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";
            return fileName;
        }

        private void ChkExportGeneralSettings_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonExport();
        }

        private void ChkExportLogLayoutSettings_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonExport();
        }

        private void ChkExportLogProvidersSettings_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButtonExport();
        }
        private void UpdateButtonExport()
        {
            BtnExport.Enabled = ChkExportGeneralSettings.Checked || ChkExportLogLayoutSettings.Checked || ChkExportLogProvidersSettings.Checked;
        }
        #endregion
    }
}
