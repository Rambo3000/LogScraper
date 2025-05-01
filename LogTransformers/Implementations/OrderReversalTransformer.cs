using System;

namespace LogScraper.LogTransformers.Implementations
{
    /// <summary>
    /// A log transformer that reverses the order of log entries.
    /// </summary>
    /// <remarks>
    /// This transformer modifies the provided array of log entries by reversing their order in place.
    /// </remarks>
    internal class OrderReversalTransformer : ILogTransformer
    {
        /// <summary>
        /// Reverses the order of the provided log entries.
        /// </summary>
        /// <param name="logEntries">An array of log entries to reverse.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logEntries"/> is null.</exception>
        public void Transform(string[] logEntries)
        {
            // Ensure the log entries array is not null.
            ArgumentNullException.ThrowIfNull(logEntries);

            // Reverse the order of the log entries in place.
            Array.Reverse(logEntries);
        }
    }
}
