using System;
using System.Text.Json;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors.Implementations
{
    public class JsonPostProcessor : ILogPostProcessor
    {
        public bool TryProcess(LogEntry logEntry, out string prettyPrintedJson)
        {
            prettyPrintedJson = null;

            if (string.IsNullOrEmpty(logEntry.Entry) || logEntry.StartIndexContent < 0) return false;

            int possibleStartIndex = logEntry.Entry.IndexOf('{', logEntry.StartIndexContent);
            if (possibleStartIndex < 0) return false;

            int possibleEndIndex = logEntry.Entry.LastIndexOf('}');
            if (possibleEndIndex < possibleStartIndex) return false;

            // Additional checks to filter out non-JSON data
            int possibleSubComma = logEntry.Entry.IndexOf('\"', possibleStartIndex + 1, possibleEndIndex - possibleStartIndex - 2);
            if (possibleSubComma < 0) return false;
            int possibleColon= logEntry.Entry.IndexOf(':', possibleStartIndex + 1, possibleEndIndex - possibleStartIndex - 2);
            if (possibleColon < 0) return false;

            return TryJsonPrettyPrint(logEntry.Entry[possibleStartIndex..(possibleEndIndex + 1)], out prettyPrintedJson);
        }

        private static readonly JsonDocumentOptions documentOptions = new()
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip
        };
        private static readonly JsonSerializerOptions serializerOptions = new()
        {
            WriteIndented = true
        };

        private static bool TryJsonPrettyPrint(string text, out string prettyPrintedJson)
        {
            prettyPrintedJson = null;

            try
            {
                using JsonDocument jsonDocument = JsonDocument.Parse(text, documentOptions);

                prettyPrintedJson = JsonSerializer.Serialize(jsonDocument.RootElement, serializerOptions);

                return true;
            }
            catch (JsonException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
