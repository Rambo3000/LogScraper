namespace LogScraper.Sources.Adapters.Http
{
    /// <summary>
    /// Represents the authentication data required for HTTP requests.
    /// </summary>
    /// <remarks>
    /// This class supports multiple authentication types, such as API key, bearer token, and basic authentication.
    /// The properties used depend on the selected <see cref="HttpAuthenticationType"/>.
    /// </remarks>
    public class HttpAuthenticationData
    {
        /// <summary>
        /// Gets or sets the type of HTTP authentication to use.
        /// </summary>
        /// <remarks>
        /// The value determines which properties (e.g., <see cref="UserName"/>, <see cref="BearerToken"/>, etc.)
        /// are relevant for the authentication process.
        /// </remarks>
        public HttpAuthenticationType Type { get; set; }

        /// <summary>
        /// Gets or sets the username for basic authentication.
        /// </summary>
        /// <remarks>
        /// This property is used when <see cref="Type"/> is set to <see cref="HttpAuthenticationType.BasicAuthentication"/>.
        /// </remarks>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for basic authentication.
        /// </summary>
        /// <remarks>
        /// This property is used when <see cref="Type"/> is set to <see cref="HttpAuthenticationType.BasicAuthentication"/>.
        /// </remarks>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the bearer token for token-based authentication.
        /// </summary>
        /// <remarks>
        /// This property is used when <see cref="Type"/> is set to <see cref="HttpAuthenticationType.BearerToken"/>.
        /// </remarks>
        public string BearerToken { get; set; }

        /// <summary>
        /// Gets or sets the API key for API key-based authentication.
        /// </summary>
        /// <remarks>
        /// This property is used when <see cref="Type"/> is set to <see cref="HttpAuthenticationType.ApiKey"/>.
        /// </remarks>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the secret associated with the API key.
        /// </summary>
        /// <remarks>
        /// This property is optional and may be used in conjunction with <see cref="Key"/> for certain API key-based authentication schemes.
        /// </remarks>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets the URL of the login page.
        /// </summary>
        /// <remarks>
        /// This property is used when <see cref="Type"/> is set to <see cref="HttpAuthenticationType.FormLoginWithCsrf"/>.
        /// </remarks>
        public string LoginPageUrl { get; set; }
        /// <summary>
        /// Gets or sets the name of the user-defined field.
        /// </summary>
        /// <remarks>
        /// This property is used when <see cref="Type"/> is set to <see cref="HttpAuthenticationType.FormLoginWithCsrf"/>.
        /// </remarks>
        public string UserFieldName { get; set; }
        /// <summary>
        /// Gets or sets the name of the password field used in authentication forms.
        /// </summary>
        /// <remarks>
        /// This property is used when <see cref="Type"/> is set to <see cref="HttpAuthenticationType.FormLoginWithCsrf"/>.
        /// </remarks>
        public string PasswordFieldName { get; set; }
        /// <summary>
        /// Gets or sets the name of the CSRF (Cross-Site Request Forgery) field used in form submissions.
        /// </summary>
        /// <remarks>
        /// This property is used when <see cref="Type"/> is set to <see cref="HttpAuthenticationType.FormLoginWithCsrf"/>.
        /// </remarks>
        public string CsrfFieldName { get; set; }
    }
}