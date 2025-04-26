using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace LogScraper.LogTransformers.Implementations
{
    internal class JsonPathExtractionTranformer(string jsonPath) : ILogTransformer
    {
        public string JsonPath { get; set; } = jsonPath;

        public void Transform(string[] logentries)
        {
            ArgumentNullException.ThrowIfNull(logentries);

            // Parallelize the processing of log entries
            Parallel.For(0, logentries.Length, i =>
            {
                // Do not parse anything not starting with { to prevent an exception thrown and save on performance
                if (!logentries[i].StartsWith('{')) return;

                try
                {
                    // Parse the JSON content
                    JObject jsonObject = JObject.Parse(logentries[i]);

                    // Extract the value at the specified JSON path
                    JToken value = jsonObject.SelectToken(JsonPath);

                    string valueAsString = value?.ToString().Trim();
                    if (!string.IsNullOrEmpty(valueAsString))
                        logentries[i] = valueAsString;
                }
                catch
                {
                    // Do nothing, keep the line as is
                }
            });
        }
    }
}