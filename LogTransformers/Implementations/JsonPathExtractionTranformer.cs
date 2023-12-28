using Newtonsoft.Json.Linq;
using System;

namespace LogScraper.LogTransformers.Implementations
{
    internal class JsonPathExtractionTranformer(string jsonPath) : ILogTransformer
    {
        private readonly string jsonPath = jsonPath;

        public void Transform(string[] loglines)
        {
            if (jsonPath == null) { throw new Exception("JSON path is not provided for the json path extraction transformer"); }

            for (int i = 0; i < loglines.Length; i++)
            {
                // Do not parse anything not starting with { to prevent an exception thrown and save on performance
                if (!loglines[i].StartsWith('{')) continue;

                try
                {
                    // Parse the JSON content
                    JObject jsonObject = JObject.Parse(loglines[i]);

                    // Extract the value at the specified JSON path
                    JToken value = jsonObject.SelectToken(jsonPath);

                    // Insert the extracted value into the LogLine object
                    loglines[i] = value?.ToString().Trim();
                }
                catch
                {
                    // Do nothing, keep the line as is
                }
            }
        }
    }
}