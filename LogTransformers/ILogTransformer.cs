namespace LogScraper.LogTransformers
{
    /// <summary>
    /// Defines a contract for transforming log entries.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface are responsible for modifying or processing
    /// an array of log entries in a specific way, such as extracting data, reversing order,
    /// or applying other transformations.
    /// </remarks>
    public interface ILogTransformer
    {
        /// <summary>
        /// Transforms the provided array of log entries.
        /// </summary>
        /// <param name="logEntries">An array of log entries to transform.</param>
        /// <remarks>
        /// Implementations should handle null or invalid input appropriately, either by throwing
        /// exceptions or by defining specific behavior.
        /// </remarks>
        void Transform(string[] logEntries);
    }
}
