using System;
using HtmlAgilityPack;
using LogScraper.LogTransformers.Implementations;

namespace LogScraper.LogTransformers
{
    /// <summary>
    /// Provides helper methods for creating and configuring log transformers.
    /// </summary>
    internal static class LogTransformerHelper
    {
        /// <summary>
        /// Creates an instance of a log transformer based on the provided configuration.
        /// </summary>
        /// <param name="config">The configuration specifying the type and parameters of the transformer.</param>
        /// <returns>An instance of <see cref="ILogTransformer"/> corresponding to the specified configuration.</returns>
        /// <exception cref="ArgumentException">Thrown if the transformer type in the configuration is unknown.</exception>
        public static ILogTransformer CreateTransformer(this LogTransformerConfig config)
        {
            // Determine the transformer type based on the configuration and create the appropriate instance.
            return config.Type.ToLower() switch
            {
                "extractjsonpathfromeachline" => new JsonPathExtractionTranformer(config.JsonPath),
                _ => null,
            };
        }

        /// <summary>
        /// Creates a configuration object for the specified log transformer.
        /// </summary>
        /// <param name="transformer">The log transformer to create a configuration for.</param>
        /// <returns>A <see cref="LogTransformerConfig"/> object representing the configuration of the transformer.</returns>
        /// <exception cref="ArgumentException">Thrown if the transformer type is unknown.</exception>
        public static LogTransformerConfig CreateTransformerConfig(this ILogTransformer transformer)
        {
            // Initialize a new configuration object.
            LogTransformerConfig logTransformerConfig = new();

            if (transformer is JsonPathExtractionTranformer jsonPathExtractionTranformer)
            {
                logTransformerConfig.Type = "ExtractJsonPathFromEachLine";
                logTransformerConfig.JsonPath = jsonPathExtractionTranformer.JsonPath;
                return logTransformerConfig;
            }

            // Throw an exception if the transformer type is not recognized.
            throw new ArgumentException($"Unknown transformer type: {transformer.GetType()}");
        }
    }
}
