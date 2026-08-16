namespace LogScraper.Sources.Adapters.Http
{
    /// <summary>
    /// Represents the settings for HTTP authentication.
    /// </summary>  
    public class HttpAuthenticationSettings
    {
        /// <summary>
        /// Gets or sets the authentication type that is enforced for HTTP requests.
        /// </summary>
        public HttpAuthenticationType EnforcedAuthenticationType { get; set; }

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