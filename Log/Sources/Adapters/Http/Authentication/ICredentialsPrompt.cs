using System;

namespace LogScraper.Sources.Adapters.Http.Authentication
{
    /// <summary>
    /// Abstraction for asking the user to supply or correct HTTP authentication credentials.
    /// </summary>
    /// <remarks>
    /// This interface exists so that <see cref="HttpSourceAdapter"/> and the rest of the
    /// Sources/Adapters code have no dependency on any specific UI technology (such as
    /// Windows Forms). Hosting applications provide a concrete implementation (for example
    /// one backed by a dialog) and pass it in when creating an HTTP source adapter.
    /// </remarks>
    public interface ICredentialsPrompt
    {
        /// <summary>
        /// Prompts the user to provide or update authentication credentials for the given URL.
        /// </summary>
        /// <param name="current">The authentication data currently known, used to pre-fill the prompt.</param>
        /// <param name="url">The URL the credentials are being requested for.</param>
        /// <returns>
        /// The updated <see cref="HttpAuthenticationData"/> if the user confirmed, or <see langword="null"/>
        /// if the user cancelled the prompt.
        /// </returns>
        HttpAuthenticationData PromptForCredentials(HttpAuthenticationData current, string url);
    }
}
