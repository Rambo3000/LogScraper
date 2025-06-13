using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LogScraper.Sources.Adapters.Http.Authenticators
{
    /// <summary>
    /// Provides authentication functionality for HTTP clients based on header-based authentication schemes.
    /// </summary>
    /// <remarks>This class supports multiple authentication types, including Basic Authentication, Bearer
    /// Token, and API Key. It configures the HTTP client's headers accordingly based on the provided authentication
    /// data.</remarks>
    public class HeaderBasedAuthenticator : IHttpClientAuthenticator 
    {
        /// <summary>
        /// Determines whether the specified HTTP authentication type is applicable.
        /// </summary>
        /// <param name="type">The HTTP authentication type to evaluate.</param>
        /// <returns>true if the specified authentication type is  HttpAuthenticationType.None, 
        /// HttpAuthenticationType.BasicAuthentication,  HttpAuthenticationType.BearerToken, or 
        /// HttpAuthenticationType.ApiKey; otherwise, false. </returns>
        public bool IsApplicable(HttpAuthenticationType type)
        {
            return type is HttpAuthenticationType.None
                or HttpAuthenticationType.BasicAuthentication
                or HttpAuthenticationType.BearerToken
                or HttpAuthenticationType.ApiKey;
        }

        /// <summary>
        /// Authenticates an <see cref="HttpClient"/> instance using the specified authentication settings and data.
        /// </summary>
        /// <remarks>This method configures the <see cref="HttpClient"/> authorization header based on the
        /// <see cref="HttpAuthenticationType"/> specified in <paramref name="data"/>. Supported authentication types
        /// include Basic Authentication, Bearer Token, and API Key. If <paramref name="data"/> specifies <see
        /// cref="HttpAuthenticationType.None"/>, no authentication is applied.</remarks>
        /// <param name="client">The <see cref="HttpClient"/> instance to be authenticated. Cannot be null.</param>
        /// <param name="httpAuthenticationSettings">The settings that define the authentication behavior. Cannot be null.</param>
        /// <param name="data">The authentication data, including credentials or tokens, used to configure the <see cref="HttpClient"/>.
        /// Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the
        /// authentication was successfully applied.</returns>
        public Task<bool> AuthenticateAsync(HttpClient client, HttpAuthenticationSettings httpAuthenticationSettings, HttpAuthenticationData data)
        {
            switch (data.Type)
            {
                case HttpAuthenticationType.BasicAuthentication:
                    string basicAuth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{data.UserName}:{data.Password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
                    break;

                case HttpAuthenticationType.BearerToken:
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data.BearerToken);
                    break;

                case HttpAuthenticationType.ApiKey:
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{data.Key}:{data.Secret}");
                    break;

                case HttpAuthenticationType.None:
                default:
                    // Do nothing
                    break;
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Creates and returns a new <see cref="HttpClient"/> instance with the specified timeout.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout duration, in seconds, for the <see cref="HttpClient"/> instance. Must be greater than zero.</param>
        /// <returns>A new <see cref="HttpClient"/> instance configured with the specified timeout.</returns>
        public HttpClient GetHttpClient(int timeoutSeconds)
        {
            return new()
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
        }
    }
}
