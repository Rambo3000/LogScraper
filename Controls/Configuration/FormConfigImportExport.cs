using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Utilities.Extensions;

namespace LogScraper.Controls.Configuration
{
    public enum ImportExportMode { Import, Export }

    public partial class FormConfigImportExport : Form
    {
        private readonly ImportExportMode _mode;
        private readonly ConfigurationExport _exportConfig;

        // Exposed after a successful import dialog
        internal ConfigurationExport SelectedImportConfig { get; private set; }
        public bool ImportGeneralSettings => ChkImportGeneralSettings.Checked;
        public bool ImportLogLayoutSettings => ChkImportLogLayoutSettings.Checked;
        public bool ImportLogProvidersSettings => ChkImportLogProvidersSettings.Checked;

        // Import mode: user selects file in Load
        public FormConfigImportExport()
        {
            _mode = ImportExportMode.Import;
            InitializeComponent();
            BtnImportExport.ImageIndex = 0; // Set default image for import action
        }

        // Export mode: full export object provided by caller
        internal FormConfigImportExport(ConfigurationExport exportConfig)
        {
            _mode = ImportExportMode.Export;
            _exportConfig = exportConfig;
            InitializeComponent();
            BtnImportExport.ImageIndex = 1; // Set default image for export action
        }

        private void FormConfigImportExport_Load(object sender, EventArgs e)
        {
            if (_mode == ImportExportMode.Import)
                SetupImportMode();
            else
                SetupExportMode();
        }

        private void SetupImportMode()
        {
            Text = "Instellingen importeren";
            BtnImportExport.Text = "Importeren";
            LblExplenation.Text = "Selecteer welke instellingen u wilt importeren uit het gekozen bestand.";

            OpenFileDialog dialog = new()
            {
                Title = "Selecteer het instellingenbestand",
                Filter = "JSON-bestanden (*.json)|*.json|Alle bestanden (*.*)|*.*",
                InitialDirectory = GetDefaultOpenFolder()
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            try
            {
                bool success = ConfigurationManager.TryLoadConfigurationExportFromFile(dialog.FileName, out ConfigurationExport config);
                if (!success)
                {
                    MessageBox.Show("Het bestand bevat geen herkenbare instellingen.", "Importeren", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                SelectedImportConfig = config;
                SetCheckboxStates(config.GenericConfig != null, config.LogLayoutsConfig != null, config.LogProvidersConfig != null);
                UpdateActionButton();
            }
            catch (JsonException jex)
            {
                MessageBox.Show("Importeren mislukt – ongeldig JSON-bestand:\n" + jex.Message, "Importeerfout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                jex.LogStackTraceToFile("Error parsing JSON during import");
                DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Importeren mislukt:\n" + ex.Message, "Importeerfout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.LogStackTraceToFile("Error during import");
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void SetupExportMode()
        {
            Text = "Instellingen exporteren";
            BtnImportExport.Text = "Exporteren...";
            LblExplenation.Text = "Selecteer welke instellingen u wilt exporteren naar een bestand.";

            SetCheckboxStates(true, true, true);
            UpdateActionButton();
        }

        private void SetCheckboxStates(bool general, bool layouts, bool providers)
        {
            ChkImportGeneralSettings.Checked = general;
            ChkImportGeneralSettings.Enabled = _mode == ImportExportMode.Export || general;

            ChkImportLogLayoutSettings.Checked = layouts;
            ChkImportLogLayoutSettings.Enabled = _mode == ImportExportMode.Export || layouts;

            ChkImportLogProvidersSettings.Checked = providers;
            ChkImportLogProvidersSettings.Enabled = _mode == ImportExportMode.Export || providers;
        }

        private void UpdateActionButton()
        {
            BtnImportExport.Enabled = ChkImportGeneralSettings.Checked
                || ChkImportLogLayoutSettings.Checked
                || ChkImportLogProvidersSettings.Checked;
            BtnCancel.Enabled = true;
        }

        private void BtnImportExport_Click(object sender, EventArgs e)
        {
            if (_mode == ImportExportMode.Import)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                PerformExport();
            }
        }

        private void PerformExport()
        {
            ConfigurationExport exportObj = new();
            if (ChkImportGeneralSettings.Checked) exportObj.GenericConfig = _exportConfig.GenericConfig;
            if (ChkImportLogLayoutSettings.Checked) exportObj.LogLayoutsConfig = _exportConfig.LogLayoutsConfig;
            if (ChkImportLogProvidersSettings.Checked) exportObj.LogProvidersConfig = _exportConfig.LogProvidersConfig;

            string filePath = GetExportFilePath();
            if (string.IsNullOrEmpty(filePath)) return;

            if (File.Exists(filePath))
            {
                if (MessageBox.Show("Het bestand bestaat al. Wilt u het overschrijven?", "Bestand overschrijven?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }

            try
            {
                ConfigurationManager.SaveToFile(filePath, exportObj);
                MessageBox.Show("Instellingen zijn succesvol geëxporteerd naar:\n" + filePath,
                    "Exporteren", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exporteren mislukt:\n" + ex.Message, "Exporteerfout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.LogStackTraceToFile("Error during export");
            }
        }

        private string GetExportFilePath()
        {
            SaveFileDialog dialog = new()
            {
                Title = "Exporteer instellingen",
                Filter = "JSON-bestanden (*.json)|*.json|Alle bestanden (*.*)|*.*",
                FileName = CreateDefaultExportFileName(),
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                OverwritePrompt = false
            };

            return dialog.ShowDialog(this) == DialogResult.OK ? dialog.FileName : null;
        }

        private string CreateDefaultExportFileName()
        {
            List<string> parts = ["LogScraperInstellingen"];
            if (!(ChkImportGeneralSettings.Checked && ChkImportLogLayoutSettings.Checked && ChkImportLogProvidersSettings.Checked))
            {
                if (ChkImportGeneralSettings.Checked) parts.Add("Algemeen");
                if (ChkImportLogLayoutSettings.Checked) parts.Add("LogLayouts");
                if (ChkImportLogProvidersSettings.Checked) parts.Add("LogBronnen");
            }
            return string.Join("_", parts) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";
        }

        private static string GetDefaultOpenFolder()
        {
            string downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            return Directory.Exists(downloads) ? downloads : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        }

        private void ChkChanged(object sender, EventArgs e) => UpdateActionButton();

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
