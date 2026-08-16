using System.Net;

namespace LogScraper.Sources.Adapters.Http.Authentication
{
    /// <summary>
    /// Abstraction over a persistent store for saving and retrieving named credentials.
    /// </summary>
    /// <remarks>
    /// This interface exists so that <see cref="HttpAuthenticationHelper"/> and the rest of the
    /// Sources/Adapters code have no dependency on a specific credential storage mechanism (such as
    /// the Windows Credential Manager). Hosting applications provide a concrete implementation and
    /// pass it in when creating an HTTP source adapter.
    /// </remarks>
    public interface ICredentialStore
    {
        /// <summary>
        /// Saves credentials under the specified target identifier.
        /// </summary>
        /// <param name="target">The target identifier for the credentials.</param>
        /// <param name="username">The username to save.</param>
        /// <param name="password">The password to save.</param>
        void SaveCredentials(string target, string username, string password);

        /// <summary>
        /// Retrieves credentials for the specified target identifier.
        /// </summary>
        /// <param name="target">The target identifier for the credentials.</param>
        /// <returns>A <see cref="NetworkCredential"/> containing the username and password, or <see langword="null"/> if not found.</returns>
        NetworkCredential GetCredentials(string target);
    }
}
