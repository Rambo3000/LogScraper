using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace LogScraper.LogTransformers.Implementations
{
    internal class JsonPathExtractionTranformer(string jsonPath) : ILogTransformer
    {
        public string JsonPath { get; set; } = jsonPath;

        public void Transform(string[] loglines)
        {
            ArgumentNullException.ThrowIfNull(loglines);

            // Parallelize the processing of log lines
            Parallel.For(0, loglines.Length, i =>
            {
                // Do not parse anything not starting with { to prevent an exception thrown and save on performance
                if (!loglines[i].StartsWith('{')) return;

                try
                {
                    // Parse the JSON content
                    JObject jsonObject = JObject.Parse(loglines[i]);

                    // Extract the value at the specified JSON path
                    JToken value = jsonObject.SelectToken(JsonPath);

                    string valueAsString = value?.ToString().Trim();
                    if (!string.IsNullOrEmpty(valueAsString))
                        loglines[i] = valueAsString;
                }
                catch
                {
                    // Do nothing, keep the line as is
                }
            });
        }
    }
}