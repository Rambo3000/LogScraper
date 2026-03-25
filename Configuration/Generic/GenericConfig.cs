using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LogScraper.LogProviders;

namespace LogScraper.Configuration.Generic
{
    /// <summary>
    /// Represents the generic configuration settings for the application.
    /// Includes default log provider type, file export settings, and other general configurations.
    /// </summary>
    internal class GenericConfig
    {
        /// <summary>
        /// Gets or sets the default log provider type for the application.
        /// This determines which log provider (e.g., Kubernetes, Runtime, File) is used by default.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogProviderType LogProviderTypeDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether logs should be exported to a file.
        /// Default value is <c>true</c>.
        /// </summary>
        public bool ExportToFile { get; set; } = true;

        /// <summary>
        /// Gets or sets the name of the file to be opened in an external editor.
        /// </summary>
        public string EditorFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the file where logs will be exported.
        /// </summary>
        public string ExportFileName { get; set; }

        /// <summary>
        /// Gets or sets the timeout value (in seconds) for HTTP client requests.
        /// Default value is 30 seconds.
        /// </summary>
        public int HttpCLientTimeOUtSeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets the automatic read time (in minutes) for logs.
        /// Default value is 2 minutes.
        /// </summary>
        public int AutomaticReadTimeMinutes { get; set; } = 2;

        /// <summary>
        /// Gets or sets a value indicating whether to seperate show error lines in the begin and end filters.
        /// Default value is <c>true</c>.
        /// </summary>
        public bool ShowErrorLinesInBeginAndEndFilters { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to automatically toggle the hierarchy when the session filter is applied
        /// </summary>
        public bool AutoToggleHierarchy { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to show the timeline by default when opening a log session.
        /// </summary>
        public bool ShowTimelineByDefault { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to check for updates for beta releases.
        /// </summary>
        public bool IncludeBetaUpdates { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether debug mode is enabled for the application.
        /// </summary>
        public bool IsDebugModeEnabled { get; set; } = false;
    }
}

