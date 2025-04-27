using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Credentials;
using LogScraper.Sources.Adapters.Http;
using LogScraper.Sources.Adapters;
using LogScraper.Configuration;
using LogScraper.LogProviders.Runtime;
using LogScraper.Log.Layout;

namespace LogScraper.LogProviders.Kubernetes
{
    public partial class UserControlRuntimeConfig : UserControl
    {
        private readonly BindingList<RuntimeInstance> _instances = [];

        public UserControlRuntimeConfig()
        {
            InitializeComponent();
        }
        internal void SetRuntimeConfig(RuntimeConfig config, List<LogLayout> logLayouts)
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
            }
        }
        internal bool TryGetConfiguration(out RuntimeConfig config)
        {
            List<string> errorMessages = [];

            if (CboLogLayout.SelectedIndex == -1)
            {
                errorMessages.Add($"De standaard layout voor Directly Url moet geselecteerd zijn.");
            }

            foreach (RuntimeInstance instance in _instances)
            {
                if (string.IsNullOrWhiteSpace(instance.Description) ||
                    string.IsNullOrWhiteSpace(instance.UrlRuntimeLog))
                {
                    errorMessages.Add($"Url '{instance.Description}' moet een Description en Url hebben.");
                }
            }

            config = null;
            if (errorMessages.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorMessages), "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            config = new RuntimeConfig
            {
                Instances = [.. _instances],
                DefaultLogLayout = CboLogLayout.SelectedItem as LogLayout,
                DefaultLogLayoutDescription = (CboLogLayout.SelectedItem as LogLayout).Description
            };

            return true;
        }

        private void BtnAddUrl_Click(object sender, EventArgs e)
        {
            RuntimeInstance instance = new()
            {
                Description = "Nieuwe runtime",
                UrlRuntimeLog = string.Empty
            };
            _instances.Add(instance);
            LstUrls.SelectedItem = instance;
            UpdateButtons();
        }

        private void BtnRemoveUrl_Click(object sender, EventArgs e)
        {
            if (LstUrls.SelectedItem is RuntimeInstance instance)
            {
                _instances.Remove(instance);
            }
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            BtnRemoveUrl.Enabled = LstUrls.Items.Count > 1;
            BtnUrlUp.Enabled = LstUrls.SelectedIndex > 0;
            BtnUrlDown.Enabled = LstUrls.SelectedIndex != -1 && LstUrls.SelectedIndex < (LstUrls.Items.Count - 1);

            UpdateButtonTest();
        }

        private void UpdateButtonTest()
        {
            BtnTest.Enabled = LstUrls.SelectedItem != null && !string.IsNullOrWhiteSpace(TxtUrl.Text);
        }

        private void BtnUrlUp_Click(object sender, EventArgs e)
        {
            if (LstUrls.SelectedItem is not RuntimeInstance selected) return;

            int index = _instances.IndexOf(selected);
            if (index > 0)
            {
                _instances.RemoveAt(index);
                _instances.Insert(index - 1, selected);
                LstUrls.SelectedIndex = index - 1;
            }
            UpdateButtons();
        }

        private void BtnUrlDown_Click(object sender, EventArgs e)
        {
            if (LstUrls.SelectedItem is not RuntimeInstance selected) return;

            int index = _instances.IndexOf(selected);
            if (index < _instances.Count - 1)
            {
                _instances.RemoveAt(index);
                _instances.Insert(index + 1, selected);
                LstUrls.SelectedIndex = index + 1;
            }
            UpdateButtons();
        }


        private bool UpdatingUrlInformation = false;
        private void LstUrls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstUrls.SelectedItem is RuntimeInstance selected)
            {
                UpdatingUrlInformation = true;
                TxtDescription.Text = selected.Description;
                TxtUrl.Text = selected.UrlRuntimeLog;
                UpdatingUrlInformation = false;
            }
            UpdateButtons();
        }


        private void TxtDescription_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstUrls.SelectedItem is RuntimeInstance selected) selected.Description = TxtDescription.Text;

            LstUrls.DisplayMember = ""; // Force update
            LstUrls.DisplayMember = "Description";
        }

        private void TxtUrl_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstUrls.SelectedItem is RuntimeInstance selected) selected.UrlRuntimeLog = TxtUrl.Text;
            UpdateButtonTest();
        }


        private void BtnTest_Click(object sender, EventArgs e)
        {
            string url = string.Empty;
            try
            {
                BtnTest.Enabled = false;
                url = TxtUrl.Text;

                ISourceAdapter sourceAdapter = SourceAdapterFactory.CreateHttpSourceAdapter(url, CredentialManager.GenerateTargetLogProvider("Runtime", TxtDescription.Text), ConfigurationManager.GenericConfig.HttpCLientTimeOUtSeconds, TrailType.None, null);
                sourceAdapter.GetLog();

                TxtTestMessage.ForeColor = System.Drawing.Color.DarkGreen;
                TxtTestMessage.Text = "Succes";
                TxtTestMessage.Text += Environment.NewLine + Environment.NewLine + url;
            }
            catch (Exception exception)
            {
                TxtTestMessage.ForeColor = System.Drawing.Color.DarkRed;
                TxtTestMessage.Text = exception.Message;
                TxtTestMessage.Text += Environment.NewLine + Environment.NewLine + url;
            }
            finally
            {
                BtnTest.Enabled = true;
            }
        }
    }
}