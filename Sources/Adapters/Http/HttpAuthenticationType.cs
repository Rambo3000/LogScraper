namespace LogScraper.Sources.Adapters.Http
{
    /// <summary>
    /// Represents the types of HTTP authentication supported by the application.
    /// </summary>
    /// <remarks>
    /// This enum is used to specify the authentication method for HTTP requests.
    /// Depending on the selected type, different properties in <see cref="HttpAuthenticationData"/>
    /// will be used to provide the necessary credentials.
    /// </remarks>
    public enum HttpAuthenticationType
    {
        /// <summary>
        /// No authentication is required.
        /// </summary>
        None,

        /// <summary>
        /// API key-based authentication.
        /// </summary>
        /// <remarks>
        /// Requires the <see cref="HttpAuthenticationData.Key"/> and optionally the <see cref="HttpAuthenticationData.Secret"/>.
        /// </remarks>
        ApiKey,

        /// <summary>
        /// Bearer token-based authentication.
        /// </summary>
        /// <remarks>
        /// Requires the <see cref="HttpAuthenticationData.BearerToken"/>.
        /// </remarks>
        BearerToken,

        /// <summary>
        /// Basic authentication using a username and password.
        /// </summary>
        /// <remarks>
        /// Requires the <see cref="HttpAuthenticationData.UserName"/> and <see cref="HttpAuthenticationData.Password"/>.
        /// </remarks>
        BasicAuthentication
    }
}
