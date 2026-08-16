using System.Windows.Forms;
using LogScraper.Controls.Generic;
using LogScraper.Sources.Adapters.Http;
using LogScraper.Sources.Adapters.Http.Authentication;

namespace LogScraper.Controls.Sources
{
    /// <summary>
    /// Windows Forms based implementation of <see cref="ICredentialsPrompt"/> that shows
    /// <see cref="FormHttpCredentials"/> to let the user supply or update credentials.
    /// </summary>
    public class WinFormsCredentialsPrompt : ICredentialsPrompt
    {
        public HttpAuthenticationData PromptForCredentials(HttpAuthenticationData current, string url)
        {
            FormHttpCredentials form = new()
            {
                HttpAuthenticationData = current,
                Url = url
            };

            form.ShowDialog();

            return form.CustomDialogResult == DialogResult.OK ? form.HttpAuthenticationData : null;
        }
    }
}
