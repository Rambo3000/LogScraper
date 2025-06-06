using System;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Sources.Adapters.Http;

namespace LogScraper.SourceAdapters
{
    public partial class FormHttpCredentials : Form
    {
        private class AuthenticationTypeWithDescription(HttpAuthenticationType authenticationType) : IEquatable<AuthenticationTypeWithDescription>
        {
            public HttpAuthenticationType AuthenticationType { get; private set; } = authenticationType;
            public bool Equals(AuthenticationTypeWithDescription other)
            {
                return null != other && AuthenticationType == other.AuthenticationType;
            }
            public override bool Equals(object obj)
            {
                return Equals(obj as AuthenticationTypeWithDescription);
            }
            public override int GetHashCode()
            {
                return AuthenticationType.GetHashCode();
            }
            public override string ToString()
            {
                return AuthenticationType switch
                {
                    HttpAuthenticationType.ApiKey => "API key",
                    HttpAuthenticationType.None => "Geen authenticatie",
                    HttpAuthenticationType.BearerToken => "Bearer token",
                    HttpAuthenticationType.BasicAuthentication => "Basic authentication",
                    HttpAuthenticationType.FormLoginWithCsrf => "Inloggen via webformulier",
                    _ => string.Empty,
                };
            }
        }

        public FormHttpCredentials()
        {
            InitializeComponent();

            cboAuthenticationType.Items.Add(new AuthenticationTypeWithDescription(HttpAuthenticationType.None));
            //Do not add API Key for now, first determine exactly how API keys are given. This can be in verioous methods. using the X-API-KEY header, or through bearer token or query string
            //cboAuthenticationType.Items.Add(new AuthenticationTypeWithDescription(HttpAuthenticationType.ApiKey));
            cboAuthenticationType.Items.Add(new AuthenticationTypeWithDescription(HttpAuthenticationType.BasicAuthentication));
            cboAuthenticationType.Items.Add(new AuthenticationTypeWithDescription(HttpAuthenticationType.BearerToken));
            cboAuthenticationType.Items.Add(new AuthenticationTypeWithDescription(HttpAuthenticationType.FormLoginWithCsrf));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HttpAuthenticationData HttpAuthenticationData
        {
            get
            {
                HttpAuthenticationData data = new()
                {
                    Type = ((AuthenticationTypeWithDescription)cboAuthenticationType.SelectedItem).AuthenticationType,
                    UserName = txtUsername.Text,
                    Password = txtPassword.Text,
                    BearerToken = txtBearerToken.Text,
                    Key = txtKey.Text,
                    Secret = txtSecret.Text
                };
                return data;
            }
            set
            {
                if (value == null) return;

                cboAuthenticationType.SelectedItem = new AuthenticationTypeWithDescription(value.Type);
                txtUsername.Text = value.UserName;
                txtPassword.Text = value.Password;
                txtBearerToken.Text = value.BearerToken;
                txtKey.Text = value.Key;
                txtSecret.Text = value.Secret;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Url
        {
            get
            {
                return lblUrlValue.Text;
            }
            set
            {
                lblUrlValue.Text = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DialogResult CustomDialogResult { get; private set; } // Define DialogResult property

        private void BtnOK_Click(object sender, EventArgs e)
        {
            HttpAuthenticationType authenticationType = ((AuthenticationTypeWithDescription)cboAuthenticationType.SelectedItem).AuthenticationType;

            if (authenticationType == HttpAuthenticationType.BearerToken && !txtBearerToken.Text.Contains(':', StringComparison.CurrentCulture))
            {
                MessageBox.Show("Geef een bearer correcte token op", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (authenticationType == HttpAuthenticationType.ApiKey && (string.IsNullOrEmpty(txtKey.Text) || string.IsNullOrEmpty(txtSecret.Text)))
            {
                MessageBox.Show("Geef de key en het secret op", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((authenticationType == HttpAuthenticationType.BasicAuthentication || authenticationType == HttpAuthenticationType.FormLoginWithCsrf) && (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text)))
            {
                MessageBox.Show("Geef de gebruikersnaam en het wachtwoord op", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CustomDialogResult = DialogResult.OK; // Set the custom result to OK
            Close(); // Close the dialog
        }

        private void CboAuthenticationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HttpAuthenticationType authenticationType = ((AuthenticationTypeWithDescription)cboAuthenticationType.SelectedItem).AuthenticationType;

            cboAuthenticationType.Enabled = authenticationType != HttpAuthenticationType.FormLoginWithCsrf;

            lblBearerToken.Visible = authenticationType == HttpAuthenticationType.BearerToken;
            txtBearerToken.Visible = authenticationType == HttpAuthenticationType.BearerToken;

            lblKey.Visible = authenticationType == HttpAuthenticationType.ApiKey;
            lblSecret.Visible = authenticationType == HttpAuthenticationType.ApiKey;
            txtKey.Visible = authenticationType == HttpAuthenticationType.ApiKey;
            txtSecret.Visible = authenticationType == HttpAuthenticationType.ApiKey;

            lblPassword.Visible = authenticationType == HttpAuthenticationType.BasicAuthentication || authenticationType == HttpAuthenticationType.FormLoginWithCsrf;
            lblUsername.Visible = authenticationType == HttpAuthenticationType.BasicAuthentication || authenticationType == HttpAuthenticationType.FormLoginWithCsrf;
            txtPassword.Visible = authenticationType == HttpAuthenticationType.BasicAuthentication || authenticationType == HttpAuthenticationType.FormLoginWithCsrf;
            txtUsername.Visible = authenticationType == HttpAuthenticationType.BasicAuthentication || authenticationType == HttpAuthenticationType.FormLoginWithCsrf;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            CustomDialogResult = DialogResult.Cancel; // Set the custom result to Cancel
            Close(); // Close the dialog
        }
    }
}
