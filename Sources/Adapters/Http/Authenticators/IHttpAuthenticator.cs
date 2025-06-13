using System.Net.Http;
using System.Threading.Tasks;

namespace LogScraper.Sources.Adapters.Http.Authenticators
{
    /// <summary>
    /// Defines methods for configuring and authenticating an <see cref="HttpClient"/> instance.
    /// </summary>
    /// <remarks>Implementations of this interface provide functionality to create and authenticate HTTP
    /// clients based on specific authentication settings and data. Use this interface to integrate custom
    /// authentication mechanisms into HTTP client workflows.</remarks>
    public interface IHttpClientAuthenticator 
    {
        /// <summary>
        /// Creates and returns a new <see cref="HttpClient"/> instance configured with the specified timeout.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout duration, in seconds, for HTTP requests made using the returned <see cref="HttpClient"/>. Must
        /// be greater than zero.</param>
        /// <returns>A configured <see cref="HttpClient"/> instance with the specified timeout applied.</returns>
        HttpClient GetHttpClient(int timeoutSeconds);
        /// <summary>
        /// Authenticates a user asynchronously using the provided HTTP client and authentication settings.
        /// </summary>
        /// <remarks>This method performs an asynchronous authentication operation using the provided HTTP
        /// client and settings. Ensure that the <paramref name="client"/> is properly configured and that the <paramref
        /// name="httpAuthenticationSettings"/> and <paramref name="data"/> contain valid values required for the
        /// authentication process.</remarks>
        /// <param name="client">The <see cref="HttpClient"/> instance used to send the authentication request. Must not be null.</param>
        /// <param name="httpAuthenticationSettings">The settings that define the authentication configuration, such as endpoint URLs and headers. Must not be
        /// null.</param>
        /// <param name="data">The authentication data, including credentials or tokens, required for the authentication process. Must not
        /// be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if
        /// authentication succeeds; otherwise, <see langword="false"/>.</returns>
        Task<bool> AuthenticateAsync(HttpClient client, HttpAuthenticationSettings httpAuthenticationSettings, HttpAuthenticationData data);
        /// <summary>
        /// Determines whether the specified HTTP authentication type is applicable.
        /// </summary>
        /// <param name="type">The HTTP authentication type to evaluate.</param>
        /// <returns><see langword="true"/> if the specified authentication type is applicable; otherwise, <see
        /// langword="false"/>. </returns>
        bool IsApplicable(HttpAuthenticationType type);
    }

}