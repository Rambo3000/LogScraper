using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogScraper.Sources.Adapters.Http.Authenticators
{
    public class FormLoginAuthenticator : IHttpClientAuthenticator 
    {
        public bool IsApplicable(HttpAuthenticationType type)
        {
            return type == HttpAuthenticationType.FormLoginWithCsrf;
        }

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
                if (redirectUri != null && Uri.Compare(redirectUri, new Uri(httpAuthenticationSettings.LoginPageUrl), UriComponents.PathAndQuery, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0)
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