using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogScraper.Sources.Adapters.Http.Authenticators
{
    /// <summary>
    /// Provides functionality for authenticating users via a CSRF-protected form login process.
    /// </summary>
    /// <remarks>This class implements the <see cref="IHttpClientAuthenticator"/> interface to support
    /// authentication workflows that require a form-based login with CSRF protection. It includes methods for
    /// determining applicability, performing authentication, and configuring HTTP clients.</remarks>
    public class FormLoginAuthenticator : IHttpClientAuthenticator
    {
        /// <summary>
        /// Determines whether the specified HTTP authentication type is applicable.
        /// </summary>
        /// <param name="type">The HTTP authentication type to evaluate.</param>
        /// <returns><see langword="true"/> if the specified authentication type is <see
        /// cref="HttpAuthenticationType.FormLoginWithCsrf"/>;  otherwise, <see langword="false"/>. </returns>
        public bool IsApplicable(HttpAuthenticationType type)
        {
            return type == HttpAuthenticationType.FormLoginWithCsrf;
        }

        /// <summary>
        /// Authenticates a user by performing a CSRF-protected login process using the specified HTTP client and
        /// authentication settings.
        /// </summary>
        /// <remarks>This method performs a two-step authentication process: <list type="number"> <item> A
        /// GET request is sent to the login page to retrieve the CSRF token. </item> <item> A POST request is sent to
        /// the login page with the username, password, and CSRF token. </item> </list> If the login page redirects to
        /// the same login page after the POST request, the authentication is considered unsuccessful.</remarks>
        /// <param name="client">The <see cref="HttpClient"/> instance used to send HTTP requests. Must be properly configured for the target
        /// server.</param>
        /// <param name="httpAuthenticationSettings">The settings required for the authentication process, including the login page URL and field names for CSRF,
        /// username, and password. Cannot be null, and the <see cref="HttpAuthenticationSettings.LoginPageUrl"/> must
        /// be a valid, non-empty URL.</param>
        /// <param name="data">The authentication data containing the username and password to be used for login. Cannot be null, and the
        /// username and password must be valid for the target system.</param>
        /// <returns><see langword="true"/> if the authentication is successful; otherwise, <see langword="false"/> if the login
        /// fails or redirects back to the login page.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="httpAuthenticationSettings"/> is null or the <see
        /// cref="HttpAuthenticationSettings.LoginPageUrl"/> is null, empty, or whitespace.</exception>
        /// <exception cref="Exception">Thrown if the login page cannot be fetched successfully or if the CSRF token cannot be extracted from the
        /// login page.</exception>
        public async Task<bool> AuthenticateAsync(HttpClient client, HttpAuthenticationSettings httpAuthenticationSettings, HttpAuthenticationData data)
        {
            if (httpAuthenticationSettings == null || string.IsNullOrWhiteSpace(httpAuthenticationSettings.LoginPageUrl))
                throw new ArgumentException("Login page url is required for CSRF login.");

            // Step 1: GET login page
            HttpResponseMessage loginPageResponse = client.GetAsync(httpAuthenticationSettings.LoginPageUrl).Result;
            if (!loginPageResponse.IsSuccessStatusCode)
                throw new Exception("Failed to fetch login page");

            string loginPageHtml = await loginPageResponse.Content.ReadAsStringAsync();

            // Extract CSRF token from HTML
            string csrfToken = ExtractHiddenInputValue(loginPageHtml, httpAuthenticationSettings.CsrfFieldName ?? "_csrf") ?? throw new Exception("CSRF token not found");

            // Step 2: POST login with Csrf token
            Dictionary<string, string> form = new()
            {
                { httpAuthenticationSettings.UserFieldName?? "username", data.UserName },
                { httpAuthenticationSettings.PasswordFieldName?? "password", data.Password },
                { httpAuthenticationSettings.CsrfFieldName ?? "_csrf", csrfToken ?? "" }
            };

            HttpContent content = new FormUrlEncodedContent(form);
            HttpResponseMessage loginResponse = client.PostAsync(httpAuthenticationSettings.LoginPageUrl, content).Result;

            //Optional debug info
            //MessageBox.Show(httpAuthenticationSettings.LoginPageUrl + ": " +loginResponse.StatusCode.ToString() + " " + content.ReadAsStringAsync().Result);
            if (loginResponse.StatusCode == HttpStatusCode.Found)
            {
                Uri redirectUri = loginResponse.Headers.Location;

                // Redirect naar dezelfde login pagina
                if (redirectUri != null && Uri.Compare(redirectUri, new Uri(httpAuthenticationSettings.LoginPageUrl), UriComponents.Path, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return false;
                }
                // Login is geslaagd
                return true;
            }

            return false;
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

        /// <summary>
        /// Creates and configures an <see cref="HttpClient"/> instance with a specified timeout.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout duration, in seconds, for the <see cref="HttpClient"/> instance. Must be greater than zero.</param>
        /// <returns>An <see cref="HttpClient"/> instance configured with the specified timeout and default settings for cookies
        /// and redirects.</returns>
        public HttpClient GetHttpClient(int timeoutSeconds)
        {
            CookieContainer cookies = new();
            HttpClientHandler handler = new()
            {
                UseCookies = true,
                CookieContainer = cookies,
                AllowAutoRedirect = false
            };

            return new(handler)
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
        }
    }
}