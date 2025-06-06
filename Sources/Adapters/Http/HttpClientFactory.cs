using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogScraper.Sources.Adapters.Http
{
    /// <summary>
    /// Provides factory methods for creating and configuring <see cref="HttpClient"/> instances with various
    /// authentication mechanisms.
    /// </summary>
    public static class HttpClientFactory
    {
        /// <summary>
        /// Creates and configures an <see cref="HttpClient"/> instance with the specified authentication method and
        /// timeout.
        /// </summary>
        /// <param name="authData">The authentication data used to configure the <see cref="HttpClient"/>. This determines the type of
        /// authentication to apply, such as API key, bearer token, basic authentication, or form login with CSRF.</param>
        /// <param name="timeoutSeconds">The timeout duration, in seconds, for the <see cref="HttpClient"/> requests. Must be a positive integer.</param>
        /// <returns>A configured <see cref="HttpClient"/> instance ready for use with the specified authentication and timeout
        /// settings.</returns>
        public static async Task<HttpClient> CreateAsyncHttpClient(HttpAuthenticationData authData, HttpAuthenticationSettings httpAuthenticationSettings, int timeoutSeconds)
        {
            if (httpAuthenticationSettings != null && httpAuthenticationSettings.EnforcedAuthenticationType == HttpAuthenticationType.FormLoginWithCsrf)
            {
                return await CreateFormLoginClientAsync(authData, httpAuthenticationSettings);
            }
            HttpClient client = authData.Type switch
            {
                HttpAuthenticationType.ApiKey => CreateApiKeyClient(authData),
                HttpAuthenticationType.BearerToken => CreateBearerTokenClient(authData),
                HttpAuthenticationType.BasicAuthentication => CreateBasicAuthClient(authData),
                _ => new HttpClient(),
            };
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            return client;
        }

        /// <summary>
        /// Creates and configures an <see cref="HttpClient"/> instance with API key-based authentication.
        /// </summary>
        /// Ensure that the <see cref="HttpClient"/> is properly disposed of after use to release resources.</remarks>
        /// <param name="data">The authentication data containing the API key and secret. Cannot be null.</param>
        /// <returns>An <see cref="HttpClient"/> instance with the appropriate authorization header set.</returns>
        private static HttpClient CreateApiKeyClient(HttpAuthenticationData data)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {data.Key}:{data.Secret}");
            return client;
        }

        /// <summary>
        /// Creates an <see cref="HttpClient"/> instance configured with a Bearer token for authorization.
        /// </summary>
        /// <param name="data">The authentication data containing the Bearer token to be used for authorization.</param>
        /// <returns>An <see cref="HttpClient"/> instance with the Authorization header set to use the provided Bearer token.</returns>
        private static HttpClient CreateBearerTokenClient(HttpAuthenticationData data)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data.BearerToken);
            return client;
        }

        /// <summary>
        /// Creates an <see cref="HttpClient"/> instance configured with Basic Authentication headers.
        /// </summary>
        /// <param name="data">The authentication data containing the username and password for Basic Authentication.</param>
        /// <returns>An <see cref="HttpClient"/> instance with the Authorization header set for Basic Authentication.</returns>
        private static HttpClient CreateBasicAuthClient(HttpAuthenticationData data)
        {
            HttpClient client = new();
            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{data.UserName}:{data.Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            return client;
        }

        /// <summary>
        /// Creates and configures an <see cref="HttpClient"/> instance for performing a form-based login using CSRF
        /// protection.
        /// <param name="data">The authentication data required for the login process, including the login page URL,  user credentials, and
        /// CSRF field name.</param>
        /// <returns>An <see cref="HttpClient"/> instance that is authenticated and ready for subsequent requests.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="data"/> contains a null or whitespace <see
        /// cref="HttpAuthenticationData.LoginPageUrl"/>.</exception>
        /// <exception cref="Exception">Thrown if the login page cannot be fetched, the CSRF token cannot be extracted, or the login request fails.</exception>
        private static async Task<HttpClient> CreateFormLoginClientAsync(HttpAuthenticationData data, HttpAuthenticationSettings httpAuthenticationSettings)
        {
            if (httpAuthenticationSettings == null || string.IsNullOrWhiteSpace(httpAuthenticationSettings.LoginPageUrl))
                throw new ArgumentException("Login page url is required for CSRF login.");

            CookieContainer cookies = new();
            HttpClientHandler handler = new()
            {
                CookieContainer = cookies,
                AllowAutoRedirect = true
            };

            HttpClient client = new(handler);

            // Step 1: GET login page
            HttpResponseMessage loginPageResponse = client.GetAsync(httpAuthenticationSettings.LoginPageUrl).Result;
            if (!loginPageResponse.IsSuccessStatusCode)
                throw new Exception("Failed to fetch login page");

            string loginPageHtml = await loginPageResponse.Content.ReadAsStringAsync();

            // Extract CSRF token from HTML
            string csrfToken = ExtractHiddenInputValue(loginPageHtml, httpAuthenticationSettings.CsrfFieldName) ?? throw new Exception("CSRF token not found");

            // Step 2: POST login with Csrf token
            Dictionary<string, string> form = new()
            {
                { httpAuthenticationSettings.UserFieldName?? "username", data.UserName },
                { httpAuthenticationSettings.PasswordFieldName?? "password", data.Password },
                { httpAuthenticationSettings.CsrfFieldName ?? "", csrfToken ?? "" }
            };

            HttpContent content = new FormUrlEncodedContent(form);
            HttpResponseMessage loginResponse = client.PostAsync(httpAuthenticationSettings.LoginPageUrl, content).Result;

            // Now client is authenticated — reuse it
            return client;
        }

        /// <summary>
        /// Extracts the value of a hidden input field from the provided HTML string.
        /// </summary>
        /// <param name="html">The HTML content to search for the hidden input field. Cannot be null.</param>
        /// <param name="inputName">The name attribute of the hidden input field whose value is to be extracted. Cannot be null or empty.</param>
        /// <returns>The value of the hidden input field if found; otherwise, <see langword="null"/>.</returns>
        private static string ExtractHiddenInputValue(string html, string inputName)
        {
            if (string.IsNullOrEmpty(inputName)) return string.Empty;

            string pattern = $"<input[^>]*name=[\"']{Regex.Escape(inputName)}[\"'][^>]*value=[\"']([^\"']+)[\"'][^>]*>";
            Match match = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }

    }

}
