using System.Collections.Generic;
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
        /// Gets or sets a value indicating whether the names of the log files (shown when browsing an HTML file
        /// list) should be filtered in the user interface.
        /// </summary>
        /// <remarks>When enabled, the configured <see cref="FilterUrlNameValues"/> are removed from the
        /// log file name to improve readability. This setting does not affect the actual URL used to retrieve the
        /// log file.</remarks>
        public bool FilterUrlName { get; set; }

        /// <summary>
        /// Gets or sets the collection of values used to filter the log file name. This list is only used if <see cref="FilterUrlName"/> is set to true.
        /// </summary>
        public List<string> FilterUrlNameValues { get; set; }

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
