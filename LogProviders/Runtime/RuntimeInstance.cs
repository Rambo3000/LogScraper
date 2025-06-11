using LogScraper.Sources.Adapters.Http;

namespace LogScraper.LogProviders.Runtime
{
    /// <summary>
    /// Represents a configuration for downloading logs directly from a specific URL.
    /// </summary>
    public class RuntimeInstance
    {
        /// <summary>
        /// Gets or sets the description of the endpoint.
        /// This provides a human-readable name or explanation for the endpoint
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL for accessing the runtime logs of this instance.
        /// </summary>
        public string UrlRuntimeLog { get; set; }

        /// <summary>
        /// Gets or sets the HTTP authentication settings used to configure authentication for HTTP requests.
        /// </summary>
        public HttpAuthenticationSettings HttpAuthenticationSettings { get; set; }
        public bool IsUrlLinkToHtmlFolderList { get; set; }
        public bool IsUrlLinkToHtmlFileList { get; set; }

        /// <summary>
        /// Returns the string representation of the endpoint.
        /// </summary>
        /// <returns>The description of the endpoint.</returns>
        public override string ToString()
        {
            return Description;
        }
    }
}
