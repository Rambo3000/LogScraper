using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log.Layout;
using LogScraper.LogProviders.File;

namespace LogScraper.Controls.Configuration.LogProviders
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

        /// <summary>
        /// Refreshes the layout combo box with the current layouts, preserving the selected layout by reference.
        /// </summary>
        internal void RefreshLayouts(List<LogLayout> logLayouts)
        {
            LogLayout currentSelection = CboLogLayout.SelectedItem as LogLayout;

            CboLogLayout.Items.Clear();
            foreach (var layout in logLayouts)
            {
                CboLogLayout.Items.Add(layout);
            }

            // Try to re-select the same layout object
            if (currentSelection != null)
            {
                // Find matching layout by reference or description as fallback
                LogLayout matchingLayout = logLayouts.FirstOrDefault(l => ReferenceEquals(l, currentSelection)) 
                    ?? logLayouts.FirstOrDefault(l => l.Description == currentSelection.Description);

                if (matchingLayout != null)
                {
                    CboLogLayout.SelectedItem = matchingLayout;
                }
                else if (logLayouts.Count > 0)
                {
                    // If the previously selected layout no longer exists, select the first one
                    CboLogLayout.SelectedIndex = 0;
                }
            }
            else if (logLayouts.Count > 0 && CboLogLayout.SelectedIndex == -1)
            {
                // If nothing was selected, select the first layout
                CboLogLayout.SelectedIndex = 0;
            }
        }
        internal bool TryGetConfiguration(out FileConfig config)
        {
            List<string> errorMessages = [];

            if (CboLogLayout.SelectedIndex == -1)
            {
                errorMessages.Add($"De standaard layout voor lokale bestanden moet geselecteerd zijn.");
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
        public void Clear()
        {
            CboLogLayout.SelectedIndex = -1;
        }
    }
}