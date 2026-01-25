namespace LogScraper.LogPostProcessors
{
    /// <summary>
    /// Identifies the available post-processors that can be applied to a log entry.
    /// Each kind represents a specific transformation or analysis step.
    /// </summary>
    public enum LogPostProcessorKind
    {
        /// <summary>
        /// Attempts to detect and pretty-print JSON content found in a log entry.
        /// </summary>
        JsonPrettyPrint = 0,

        /// <summary>
        /// Attempts to detect and pretty-print XML content found in a log entry.
        /// </summary>
        XmlPrettyPrint = 1
    }

    /// <summary>
    /// Extension methods for <see cref="LogPostProcessorKind"/>.
    /// </summary>
    public static class LogPostProcessorKindExtensions
    {
        /// <summary>
        /// Converts the post-processor kind into a short, user-friendly name
        /// suitable for UI labels, tooltips, or identifiers.
        /// </summary>
        /// <param name="kind">The post-processor kind to convert.</param>
        /// <returns>A lowercase, human-readable name for the post-processor.</returns>
        public static string ToPrettyName(this LogPostProcessorKind kind)
        {
            return kind switch
            {
                LogPostProcessorKind.JsonPrettyPrint => "json",
                LogPostProcessorKind.XmlPrettyPrint => "xml",
                _ => kind.ToString(),
            };
        }
    }
}

