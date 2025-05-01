using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace LogScraper.LogTransformers.Implementations
{
    /// <summary>
    /// A log transformer that extracts values from JSON log entries using a specified JSON path.
    /// </summary>
    /// <remarks>
    /// This transformer processes each log entry in parallel, extracting the value at the specified JSON path
    /// if the log entry is a valid JSON object.
    /// </remarks>
    internal class JsonPathExtractionTranformer(string jsonPath) : ILogTransformer
    {
        /// <summary>
        /// Gets or sets the JSON path used to extract values from log entries.
        /// </summary>
        public string JsonPath { get; set; } = jsonPath;

        /// <summary>
        /// Transforms the provided log entries by extracting values from JSON objects using the specified JSON path.
        /// </summary>
        /// <param name="logentries">An array of log entries to transform.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logentries"/> is null.</exception>
        public void Transform(string[] logentries)
        {
            // Ensure the log entries array is not null.
            ArgumentNullException.ThrowIfNull(logentries);

            // Process each log entry in parallel to improve performance.
            Parallel.For(0, logentries.Length, i =>
            {
                // Skip processing if the log entry does not start with '{', as it is unlikely to be JSON.
                if (!logentries[i].StartsWith('{')) return;

                try
                {
                    // Parse the log entry as a JSON object.
                    JObject jsonObject = JObject.Parse(logentries[i]);

                    // Extract the value at the specified JSON path.
                    JToken value = jsonObject.SelectToken(JsonPath);

                    // Convert the extracted value to a string and trim whitespace.
                    string valueAsString = value?.ToString().Trim();

                    // If a valid value is extracted, replace the log entry with the extracted value.
                    if (!string.IsNullOrEmpty(valueAsString))
                        logentries[i] = valueAsString;
                }
                catch
                {
                    // If an exception occurs (e.g., invalid JSON or invalid JSON path), leave the log entry unchanged.
                }
            });
        }
    }
}