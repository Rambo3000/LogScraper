using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Credentials;
using LogScraper.Log;

using LogScraper.Configuration;
using LogScraper.LogProviders.Runtime;

namespace LogScraper.Log
{
    public partial class UserControlLogLayoutConfig : UserControl
    {
        private readonly BindingList<LogLayout> _layouts = [];

        public UserControlLogLayoutConfig()
        {
            InitializeComponent();
        }
        internal void SetLogLayoutsConfig(LogLayoutsConfig config)
        {
            /*
            CboLogLayout.Items.Clear();
            foreach (var layout in logLayouts)
            {
                CboLogLayout.Items.Add(layout);
                if (config.DefaultLogLayout != null && layout == config.DefaultLogLayout)
                {
                    CboLogLayout.SelectedItem = layout;
                }
            }

            _instances.Clear();
            LstUrls.SelectedIndex = -1;

            // Copy instances so we dont mix them with the ones already in the config
            if (config != null && config.Instances != null)
            {
                foreach (var runtime in config.Instances)
                {
                    RuntimeInstance runtimeNew = new()
                    {
                        Description = runtime.Description,
                        UrlRuntimeLog = runtime.UrlRuntimeLog
                    };
                    _instances.Add(runtimeNew);
                }
                LstUrls.DataSource = _instances;
                LstUrls.DisplayMember = "";
                LstUrls.DisplayMember = "Description";
                if (config.Instances.Count > 0) LstUrls.SelectedIndex = 0;
            }*/
        }
        internal bool TryGetLogLayoutsConfig(out LogLayoutsConfig config)
        {
            List<string> errorMessages = [];


            foreach (LogLayout layout in _layouts)
            {
                if (string.IsNullOrWhiteSpace(layout.Description) ||
                    string.IsNullOrWhiteSpace(layout.DateTimeFormat))
                {
                    errorMessages.Add($"Runtime '{layout.Description}' moet een omschrijving en datum tijd format hebben.");
                }
            }

            config = null;
            if (errorMessages.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorMessages), "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            config = new LogLayoutsConfig
            {
                /*Instances = [.. _instances],
                DefaultLogLayout = CboLogLayout.SelectedItem as LogLayout,
                DefaultLogLayoutDescription = (CboLogLayout.SelectedItem as LogLayout).Description*/
            };

            return true;
        }

        private void BtnAddLayout_Click(object sender, EventArgs e)
        {
            LogLayout layout = new()
            {
                Description = "Nieuwe layout",
                DateTimeFormat = "yyyy-MM-ddTHH:mm:ss,fff"
            };
            _layouts.Add(layout);
            LstLayouts.SelectedItem = layout;
            UpdateButtons();
        }

        private void BtnRemoveLayout_Click(object sender, EventArgs e)
        {
            if (LstLayouts.SelectedItem is LogLayout instance)
            {
                _layouts.Remove(instance);
            }
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            BtnLayoutRemove.Enabled = LstLayouts.Items.Count > 1;
            BtnLayoutUp.Enabled = LstLayouts.SelectedIndex > 0;
            BtnLayoutDown.Enabled = LstLayouts.SelectedIndex != -1 && LstLayouts.SelectedIndex < (LstLayouts.Items.Count - 1);

        }


        private void BtnUpLayout_Click(object sender, EventArgs e)
        {
            if (LstLayouts.SelectedItem is not LogLayout selected) return;

            int index = _layouts.IndexOf(selected);
            if (index > 0)
            {
                _layouts.RemoveAt(index);
                _layouts.Insert(index - 1, selected);
                LstLayouts.SelectedIndex = index - 1;
            }
            UpdateButtons();
        }

        private void BtnDownLayout_Click(object sender, EventArgs e)
        {
            if (LstLayouts.SelectedItem is not LogLayout selected) return;

            int index = _layouts.IndexOf(selected);
            if (index < _layouts.Count - 1)
            {
                _layouts.RemoveAt(index);
                _layouts.Insert(index + 1, selected);
                LstLayouts.SelectedIndex = index + 1;
            }
            UpdateButtons();
        }


        private bool UpdatingUrlInformation = false;
        private void LstUrls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstLayouts.SelectedItem is RuntimeInstance selected)
            {
                UpdatingUrlInformation = true;
                TxtDescription.Text = selected.Description;
                TxtDateTimeFormat.Text = selected.UrlRuntimeLog;
                UpdatingUrlInformation = false;
            }
            UpdateButtons();
        }


        private void TxtDescription_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstLayouts.SelectedItem is RuntimeInstance selected) selected.Description = TxtDescription.Text;

            LstLayouts.DisplayMember = ""; // Force update
            LstLayouts.DisplayMember = "Description";
        }

        private void TxtUrl_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstLayouts.SelectedItem is RuntimeInstance selected) selected.UrlRuntimeLog = TxtDateTimeFormat.Text;
        }
    }
}