using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace LogScraper.LogTransformers.Implementations
{
    internal class JsonPathExtractionTranformer(string jsonPath) : ILogTransformer
    {
        private readonly string jsonPath = jsonPath;

        public void Transform(string[] loglines)
        {
            if (loglines == null) throw new ArgumentNullException(nameof(loglines));

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
                    JToken value = jsonObject.SelectToken(jsonPath);

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