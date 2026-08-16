using System.Net;
using LogScraper.Sources.Adapters.Http.Authentication;

namespace LogScraper.Credentials
{
    /// <summary>
    /// <see cref="ICredentialStore"/> implementation backed by the Windows Credential Manager
    /// (via <see cref="CredentialManager"/>).
    /// </summary>
    public class WindowsCredentialStore : ICredentialStore
    {
        public void SaveCredentials(string target, string username, string password)
        {
            CredentialManager.SaveCredentials(target, username, password);
        }

        public NetworkCredential GetCredentials(string target)
        {
            return CredentialManager.GetCredentials(target);
        }
    }
}
