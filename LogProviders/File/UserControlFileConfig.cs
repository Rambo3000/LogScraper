using System.Collections.Generic;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.LogProviders.File;

namespace LogScraper.LogProviders.Kubernetes
{
    public partial class UserControlFileConfig : UserControl
    {
        public UserControlFileConfig()
        {
            InitializeComponent();
        }
        internal void SetFileConfig(FileConfig config, List<LogLayout> logLayouts)
        {
            CboLogLayout.Items.Clear();
            foreach (var layout in logLayouts)
            {
                CboLogLayout.Items.Add(layout);
                if (config.DefaultLogLayout != null && layout.Description == config.DefaultLogLayout.Description)
                {
                    CboLogLayout.SelectedItem = layout;
                }
            }
        }
        internal bool TryGetConfiguration(out FileConfig config)
        {
            List<string> errorMessages = [];

            if (CboLogLayout.SelectedIndex == -1)
            {
                errorMessages.Add($"De standaard layout moet geselecteerd zijn.");
            }

            config = null;
            if (errorMessages.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorMessages), "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            config = new FileConfig
            {
                DefaultLogLayout = CboLogLayout.SelectedItem as LogLayout,
                DefaultLogLayoutDescription = (CboLogLayout.SelectedItem as LogLayout).Description
            };

            return true;
        }
    }
}