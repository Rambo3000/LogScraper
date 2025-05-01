namespace LogScraper.LogProviders
{
    /// <summary>
    /// Represents the types of log providers supported by the application.
    /// Each log provider type corresponds to a specific source of log data.
    /// </summary>
    public enum LogProviderType
    {
        /// <summary>
        /// Log provider for Kubernetes logs.
        /// </summary>
        Kubernetes,

        /// <summary>
        /// Log provider based on urls
        /// </summary>
        Runtime,

        /// <summary>
        /// Log provider for file-based logs.
        /// Used for logs stored in local or remote files.
        /// </summary>
        File
    }
}
