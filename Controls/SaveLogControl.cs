using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Filtering;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Rendering;
using LogScraper.Utilities.Extensions;

namespace LogScraper.Controls
{
    public partial class SaveLogControl : UserControl
    {
        public SaveLogControl()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            LogMetadataFilterResult metadataFilterResult = LogAppState.Instance.MetadataFilterResult.Value;
            if (metadataFilterResult == null || metadataFilterResult.LogEntries == null || metadataFilterResult.LogEntries.Count == 0) return;

            LogRenderSettings logRenderSettings = new()
            {
                LogLayout = LogAppState.Instance.Layout.Value,
                ShowOriginalMetadata = true
            };

            string renderedLog = LogRenderer.RenderLogEntriesAsString(LogAppState.Instance.FilterResultWithRange.Value, logRenderSettings);

            if (renderedLog != null)
            {
                List<LogEntry> logEntriesToRender = LogAppState.Instance.FilterResultWithRange.Value.LogEntries;
                using SaveFileDialog saveFileDialog = new()
                {
                    Filter = "Log files (*.log)|*.log|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    DefaultExt = "log",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    FileName = $"Log from {logEntriesToRender[0].TimeStamp:yyyyMMdd_HHmmss} to {logEntriesToRender[^1].TimeStamp:yyyyMMdd_HHmmss}.log",
                    AddExtension = true
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, renderedLog);

                        ProcessStartInfo processStartInfo = new()
                        {
                            FileName = "explorer.exe",
                            Arguments = $"/select,\"{saveFileDialog.FileName}\"",
                            UseShellExecute = true
                        };

                        Process.Start(processStartInfo);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fout bij opslaan van log: " + ex.Message, "Opslaan mislukt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ex.LogStackTraceToFile("Fout bij opslaan van log.");
                    }
                }
            }
        }
    }
}