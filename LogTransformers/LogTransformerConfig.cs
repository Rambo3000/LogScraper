namespace LogScraper.LogTransformers
{
    /// <summary>
    /// Represents the configuration for a log transformer.
    /// </summary>
    /// <remarks>
    /// This class is used to define the settings required for a specific log transformer,
    /// such as the type of transformer and any additional parameters (e.g., JSON path).
    /// </remarks>
    public class LogTransformerConfig
    {
        /// <summary>
        /// Gets or sets the type of the log transformer.
        /// </summary>
        /// <remarks>
        /// The type is typically the name or identifier of the transformer implementation,
        /// such as "JsonPathExtractionTransformer" or "OrderReversalTransformer".
        /// </remarks>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the JSON path used by the transformer (if applicable).
        /// </summary>
        /// <remarks>
        /// This property is relevant for transformers that require a JSON path to extract
        /// specific data from log entries, such as the <see cref="JsonPathExtractionTranformer"/>.
        /// </remarks>
        public string JsonPath { get; set; }
    }
}
