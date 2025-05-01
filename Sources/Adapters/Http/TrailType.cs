namespace LogScraper.Sources.Adapters.Http
{
    /// <summary>
    /// Represents the types of trailing log queries supported by the HTTP source adapter.
    /// </summary>
    /// <remarks>
    /// This enum is used to specify how the HTTP source adapter should handle trailing log data.
    /// For example, it can be used to fetch logs from Kubernetes with specific query parameters.
    /// </remarks>
    internal enum TrailType
    {
        /// <summary>
        /// No trailing log query is applied.
        /// </summary>
        None,

        /// <summary>
        /// Fetches logs from a Kubernetes source using the "sinceSeconds" query parameter.
        /// </summary>
        Kubernetes
    }
}
