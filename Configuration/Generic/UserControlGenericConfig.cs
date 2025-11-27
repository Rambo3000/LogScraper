using System;
using System.IO;
using System.Windows.Forms;
using LogScraper.Configuration.Generic;
using LogScraper.LogProviders;

namespace LogScraper.Configuration
{
    public partial class UserControlGenericConfig : UserControl
    {
        public UserControlGenericConfig()
        {
            InitializeComponent();
            CboLogProviderType.DataSource = Enum.GetValues<LogProviderType>();
            CboAutomaticReadTime.DataSource = new int[] { 1, 2, 3, 4, 5 };
        }
        internal void SetGenericConfig(GenericConfig config)
        {
            CboLogProviderType.SelectedItem = config.LogProviderTypeDefault;
            CboAutomaticReadTime.SelectedItem = config.AutomaticReadTimeMinutes;
            ChkExportToFile.Checked = config.ExportToFile;
            ChkShowErrorsInBeginAndEndFilters.Checked = config.ShowErrorLinesInBeginAndEndFilters;
            ChkAutoToggleHierarchy.Checked = config.AutoToggleHierarchy;
            TxtEditorLocation.Text = config.EditorFileName;
            TxtExportFileName.Text = config.ExportFileName;
            TxtTimeOut.Text = config.HttpCLientTimeOUtSeconds.ToString();
        }

        private void ChkExportToFile_CheckedChanged(object sender, EventArgs e)
        {
            GrpExportSettings.Enabled = ChkExportToFile.Checked;
        }

        internal bool TryGetGenericConfig(out GenericConfig config)
        {
            config = GetLogScraperConfig();
            if (config.HttpCLientTimeOUtSeconds == 0)
            {
                MessageBox.Show("De timeout voor downloaden moet groter dan nul zijn.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (config.ExportToFile)
            {
                if (string.IsNullOrWhiteSpace(config.EditorFileName))
                {
                    MessageBox.Show("De locatie van de editor is verplicht.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(config.ExportFileName))
                {
                    MessageBox.Show("De bestandsnaam is verplicht.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        internal GenericConfig GetLogScraperConfig()
        {
            GenericConfig config = new()
            {
                EditorFileName = TxtEditorLocation.Text,
                ExportFileName = TxtExportFileName.Text,
                ExportToFile = ChkExportToFile.Checked,
                AutoToggleHierarchy = ChkAutoToggleHierarchy.Checked,
                ShowErrorLinesInBeginAndEndFilters = ChkShowErrorsInBeginAndEndFilters.Checked,
                AutomaticReadTimeMinutes = int.TryParse(CboAutomaticReadTime.SelectedItem.ToString(), out int automaticReadTime) ? automaticReadTime : 1,
                LogProviderTypeDefault = (LogProviderType)CboLogProviderType.SelectedItem,
                HttpCLientTimeOUtSeconds = int.TryParse(TxtTimeOut.Text, out int timeout) ? timeout : 0
            };

            return config;
        }

        private void BtnBrowseExportFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Title = "Selecteer exportbestand",
                Filter = "Log bestanden (*.log, *.txt)|*.log;*.txt;|Alle bestanden (*.*)|*.*",
                CheckFileExists = false,
                CheckPathExists = true
            };

            string currentPath = TxtExportFileName.Text;

            if (!string.IsNullOrWhiteSpace(currentPath))
            {
                try
                {
                    string directory = Path.GetDirectoryName(currentPath);
                    if (Directory.Exists(directory))
                    {
                        dialog.InitialDirectory = directory;
                    }
                }
                catch
                {
                    // The current path is invalid, fall back to the default behaviour.
                }
            }

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                TxtExportFileName.Text = dialog.FileName;
            }
        }

        private void BtnBrowseEditor_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Title = "Selecteer een teksteditor",
                Filter = "Uitvoerbare bestanden (*.exe)|*.exe|Alle bestanden (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true
            };

            string currentPath = TxtEditorLocation.Text;

            if (!string.IsNullOrWhiteSpace(currentPath))
            {
                try
                {
                    string directory = Path.GetDirectoryName(currentPath);
                    if (Directory.Exists(directory))
                    {
                        dialog.InitialDirectory = directory;
                    }

                    string filename = Path.GetFileName(currentPath);
                    if (!string.IsNullOrWhiteSpace(filename))
                    {
                        dialog.FileName = filename;
                    }
                }
                catch
                {
                    // ongeldig pad, negeren
                }
            }

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                TxtEditorLocation.Text = dialog.FileName;
            }
        }

        private void LblOpenExecutableFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenExecutableFolder();
        }
        private static void OpenExecutableFolder()
        {
            string executablePath = AppContext.BaseDirectory;
            string folderPath = Path.GetDirectoryName(executablePath);

            if (folderPath == null)
            {
                return;
            }

            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo();
            processStartInfo.FileName = "explorer.exe";
            processStartInfo.Arguments = folderPath;
            processStartInfo.UseShellExecute = true;

            System.Diagnostics.Process.Start(processStartInfo);
        }
    }
}
