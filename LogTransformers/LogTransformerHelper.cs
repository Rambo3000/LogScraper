using LogScraper.LogTransformers.Implementations;
using System;

namespace LogScraper.LogTransformers
{
    internal class LogTransformerHelper
    {
        public static ILogTransformer CreateTransformerFromConfig(LogTransformerConfig config)
        {
            return config.Type.ToLower() switch
            {
                "reverseorder" => new OrderReversalTransformer(),
                "extractjsonpathfromeachline" => new JsonPathExtractionTranformer(config.JsonPath),
                _ => throw new ArgumentException($"Unknown transformer type: {config.Type}"),
            };
        }
    }
}
