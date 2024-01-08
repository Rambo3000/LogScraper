using System;
using System.Diagnostics;
using System.Windows.Forms;
using LogScraper.Sources.Adapters;

namespace LogScraper.LogProviders.File
{
    public partial class UserControlFileLogProvider : UserControl, ILogProviderUserControl
    {
        public event EventHandler SourceSelectionChanged;

        public event Action<string, bool> StatusUpdate;

        public UserControlFileLogProvider()
        {
            InitializeComponent();
            if (Debugger.IsAttached)
            {
                txtFilePath.Text = "Stubs/JSONInvertedExample.log";
            }
        }

        public ISourceAdapter GetSourceAdapter()
        {
            return SourceAdapterFactory.CreateFileSourceAdapter(txtFilePath.Text);
        }

        protected virtual void OnSourceSelectionChanged(EventArgs e)
        {
            SourceSelectionChanged?.Invoke(this, e);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new();
            openFileDialog.InitialDirectory = "C:\\"; // Set the initial directory (optional)

            // Define file filters
            openFileDialog.Filter = "Text Files|*.txt|CSV Files|*.csv|Tab-Delimited Files|*.tsv|Log Files|*.log|XML Files|*.xml|JSON Files|*.json|All Files|*.*";
            openFileDialog.FilterIndex = 1; // Set the default filter index (optional)
            openFileDialog.RestoreDirectory = true; // Restore the previous directory (optional)

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file path
                string selectedFilePath = openFileDialog.FileName;

                // Do something with the selected file path, e.g., display it in a TextBox
                txtFilePath.Text = selectedFilePath;
                OnStatusUpdate("OK", true);
            }
        }
        protected virtual void OnStatusUpdate(string message, bool isSucces)
        {
            StatusUpdate?.Invoke(message, isSucces);
        }
    }
}