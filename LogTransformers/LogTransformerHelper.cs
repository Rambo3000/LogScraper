using LogScraper.LogTransformers.Implementations;
using System;

namespace LogScraper.LogTransformers
{
    internal static class LogTransformerHelper
    {
        public static ILogTransformer CreateTransformer(this LogTransformerConfig config)
        {
            return config.Type.ToLower() switch
            {
                "reverseorder" => new OrderReversalTransformer(),
                "extractjsonpathfromeachline" => new JsonPathExtractionTranformer(config.JsonPath),
                _ => throw new ArgumentException($"Unknown transformer type: {config.Type}"),
            };
        }
        public static LogTransformerConfig CreateTransformerConfig(this ILogTransformer transformer)
        {

            LogTransformerConfig logTransformerConfig = new();
            if (transformer is OrderReversalTransformer)
            {
                logTransformerConfig.Type = "ReverseOrder";
                return logTransformerConfig;
            }
            if (transformer is JsonPathExtractionTranformer jsonPathExtractionTranformer)
            {
                logTransformerConfig.Type = "ExtractJsonPathFromEachLine";
                logTransformerConfig.JsonPath = jsonPathExtractionTranformer.JsonPath;
                return logTransformerConfig;
            }
            throw new ArgumentException($"Unknown transformer type: {transformer.GetType()}");
        }
    }
}
