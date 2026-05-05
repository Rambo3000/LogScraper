using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Export;
using LogScraper.Utilities;
using LogScraper.Utilities.Extensions;

namespace LogScraper.Controls
{
    public partial class OpenLogInTextEditor : UserControl
    {
        public OpenLogInTextEditor()
        {
            InitializeComponent();
            ConfigAppState.Instance.GenericConfig.Changed += (s, e) => UpdateButtons();
            ShortcutManager.Register(this, AppShortcut.OpenLogInEditor, OpenFileInExternalEditor);
        }

        private void OpenLogInTextEditor_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnOpenWithEditor.Enabled = ConfigAppState.Instance.GenericConfig.Value.ExportToFile;

        }

        private void BtnOpenWithEditor_Click(object sender, EventArgs e)
        {
            OpenFileInExternalEditor();
        }

        /// <summary>
        /// Opens the exported log file in an external editor.
        /// </summary>
        private static void OpenFileInExternalEditor()
        {
            string fileName = LogExportWorkerManager.GetExportFileName();

            // Check if the file exists.
            if (!File.Exists(fileName))
            {
                string path = Path.GetDirectoryName(fileName);

                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    // Path exists but the file is missing.
                    MessageBox.Show($"Bestand is niet gevonden: {fileName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Path is invalid or empty.
                    MessageBox.Show($"Er is een ongeldig locatie opgegeven voor het exporteren van het log: {fileName}, pas deze aan via de instellingen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            try
            {
                // Open the file in the configured external editor.
                Process.Start(ConfigAppState.Instance.GenericConfig.Value.EditorFileName, "\"" + fileName + "\"");
            }
            catch (Exception ex)
            {
                // Show an error message if the file cannot be opened.
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.LogStackTraceToFile("Error when trying to open the exported log file in the external editor.");
            }
        }
    }
}
