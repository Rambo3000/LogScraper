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
using System.Net.Http;

namespace LogScraper.LogProviders.Kubernetes
{
    public partial class UserControlRuntimeConfig : UserControl
    {
        private readonly BindingList<RuntimeInstance> _instances = [];

        public UserControlRuntimeConfig()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            LstUrls.SelectedIndex = -1;
            _instances.Clear();
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
                        UrlRuntimeLog = runtime.UrlRuntimeLog,
                        IsUrlLinkToHtmlFileList = runtime.IsUrlLinkToHtmlFileList,
                        IsUrlLinkToHtmlFolderList = runtime.IsUrlLinkToHtmlFolderList
                    };
                    if (runtime.HttpAuthenticationSettings != null)
                    {
                        runtimeNew.HttpAuthenticationSettings = new HttpAuthenticationSettings
                        {
                            EnforcedAuthenticationType = runtime.HttpAuthenticationSettings.EnforcedAuthenticationType,
                            LoginPageUrl = runtime.HttpAuthenticationSettings.LoginPageUrl,
                            UserFieldName = runtime.HttpAuthenticationSettings.UserFieldName,
                            PasswordFieldName = runtime.HttpAuthenticationSettings.PasswordFieldName,
                            CsrfFieldName = runtime.HttpAuthenticationSettings.CsrfFieldName
                        };
                    }

                    _instances.Add(runtimeNew);
                }
                LstUrls.DataSource = _instances;
                LstUrls.DisplayMember = string.Empty;
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

                if (instance.HttpAuthenticationSettings != null && string.IsNullOrWhiteSpace(instance.HttpAuthenticationSettings.LoginPageUrl))
                {
                    errorMessages.Add($"Url '{instance.Description}' moet een login url hebben");
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
                ChkWebFormLogin.Checked = selected.HttpAuthenticationSettings != null && selected.HttpAuthenticationSettings.EnforcedAuthenticationType == HttpAuthenticationType.FormLoginWithCsrf;
                ChkUrlLinksToHtmlFileList.Checked = selected.IsUrlLinkToHtmlFileList;
                ChkUrlLinksToHtmlFolderList.Checked = selected.IsUrlLinkToHtmlFolderList;
                TxtLoginPageUrl.Text = selected.HttpAuthenticationSettings?.LoginPageUrl ?? string.Empty;
                TxtUserFieldName.Text = selected.HttpAuthenticationSettings?.UserFieldName;
                TxtPasswordFieldName.Text = selected.HttpAuthenticationSettings?.PasswordFieldName;
                TxtCsrfFieldName.Text = selected.HttpAuthenticationSettings?.CsrfFieldName;
                UpdatingUrlInformation = false;
            }
            UpdateButtons();
        }


        private void TxtDescription_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstUrls.SelectedItem is RuntimeInstance selected) selected.Description = TxtDescription.Text;

            LstUrls.DisplayMember = string.Empty; // Force update
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
                HttpAuthenticationSettings httpAuthenticationSettings = null;
                if (LstUrls.SelectedItem is RuntimeInstance selected)
                {
                    httpAuthenticationSettings = selected.HttpAuthenticationSettings;
                }

                ISourceAdapter sourceAdapter = SourceAdapterFactory.CreateHttpSourceAdapter(url, CredentialManager.GenerateTargetLogProvider("Runtime", TxtDescription.Text), ConfigurationManager.GenericConfig.HttpCLientTimeOUtSeconds, httpAuthenticationSettings, TrailType.None, null, false);

                if (((HttpSourceAdapter)sourceAdapter).TryInitiateClientAndAuthenticate(out HttpResponseMessage _, out string errorMessage))
                {
                    TxtTestMessage.ForeColor = System.Drawing.Color.DarkGreen;
                    TxtTestMessage.Text = "Succes";
                    TxtTestMessage.Text += Environment.NewLine + Environment.NewLine + url;
                }
                else
                {
                    TxtTestMessage.ForeColor = System.Drawing.Color.DarkRed;
                    TxtTestMessage.Text = errorMessage;
                    TxtTestMessage.Text += Environment.NewLine + Environment.NewLine + url;
                }
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

        private void ChkWebFormLogin_CheckedChanged(object sender, EventArgs e)
        {
            GrpWebFormSettings.Visible = ChkWebFormLogin.Checked;

            if (UpdatingUrlInformation) return;

            if (LstUrls.SelectedItem is RuntimeInstance selected)
            {
                if (ChkWebFormLogin.Checked)
                {
                    selected.HttpAuthenticationSettings = new()
                    {
                        EnforcedAuthenticationType = HttpAuthenticationType.FormLoginWithCsrf,
                    };
                    TxtLoginPageUrl.Text = string.Empty;
                    TxtUserFieldName.Text = "username";
                    TxtPasswordFieldName.Text = "password";
                    TxtCsrfFieldName.Text = "_csrf";
                }
                else
                {
                    selected.HttpAuthenticationSettings = null;
                }
            }
        }

        private void TxtLoginPageUrl_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstUrls.SelectedItem is RuntimeInstance selected)
            {
                if (selected.HttpAuthenticationSettings != null)
                {
                    selected.HttpAuthenticationSettings.LoginPageUrl = TxtLoginPageUrl.Text;
                }
            }
        }

        private void TxtUserFieldName_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstUrls.SelectedItem is RuntimeInstance selected)
            {
                if (selected.HttpAuthenticationSettings != null)
                {
                    selected.HttpAuthenticationSettings.UserFieldName = TxtUserFieldName.Text;
                }
            }
        }

        private void TxtPasswordFieldName_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstUrls.SelectedItem is RuntimeInstance selected)
            {
                if (selected.HttpAuthenticationSettings != null)
                {
                    selected.HttpAuthenticationSettings.PasswordFieldName = TxtPasswordFieldName.Text;
                }
            }
        }

        private void TxtCsrfFieldName_TextChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstUrls.SelectedItem is RuntimeInstance selected)
            {
                if (selected.HttpAuthenticationSettings != null)
                {
                    selected.HttpAuthenticationSettings.CsrfFieldName = TxtCsrfFieldName.Text;
                }
            }
        }

        private void ChkUrlLinksToHtmlFileList_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstUrls.SelectedItem is RuntimeInstance selected)
            {
                selected.IsUrlLinkToHtmlFileList = ChkUrlLinksToHtmlFileList.Checked;
                if (selected.IsUrlLinkToHtmlFileList)
                {
                    selected.IsUrlLinkToHtmlFolderList = false;
                    ChkUrlLinksToHtmlFolderList.Checked = false;
                }
            }
        }

        private void ChkUrlLinksToHtmlFolderList_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdatingUrlInformation) return;

            if (LstUrls.SelectedItem is RuntimeInstance selected)
            {
                selected.IsUrlLinkToHtmlFolderList = ChkUrlLinksToHtmlFolderList.Checked;
                if (selected.IsUrlLinkToHtmlFolderList)
                {
                    selected.IsUrlLinkToHtmlFileList = false;
                    ChkUrlLinksToHtmlFileList.Checked = false;
                }
            }
        }
    }
}