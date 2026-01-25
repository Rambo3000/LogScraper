using System;
using System.IO;
using System.Text;
using System.Text.Json;
using LogScraper.Log;

namespace LogScraper.LogPostProcessors.Implementations
{
    /// <summary>
    /// Provides functionality to extract and pretty-print JSON content from log entries.
    /// </summary>
    /// <remarks>This class implements the <see cref="ILogPostProcessor"/> interface to identify JSON
    /// fragments within log entries and format them for improved readability. It is typically used in logging pipelines
    /// to post-process log data containing embedded JSON. Thread safety is not guaranteed; if used concurrently,
    /// external synchronization is required.</remarks>
    public class JsonPostProcessor : ILogPostProcessor
    {
        /// <summary>
        /// Attempts to extract and pretty-print a JSON object from the specified log entry.
        /// </summary>
        /// <remarks>This method scans the log entry for a JSON object and attempts to format it for
        /// readability. If the log entry does not contain valid JSON or the formatting fails, the method returns false
        /// and the output parameter is set to null.</remarks>
        /// <param name="logEntry">The log entry to process. Must contain a valid JSON object starting at or after the specified start index.</param>
        /// <param name="prettyPrintedJson">When this method returns, contains the pretty-printed JSON string if extraction and formatting succeed;
        /// otherwise, null.</param>
        /// <returns>true if a valid JSON object was found and successfully pretty-printed; otherwise, false.</returns>
        public bool TryProcess(LogEntry logEntry, out string prettyPrintedJson)
        {
            prettyPrintedJson = null;

            if (string.IsNullOrEmpty(logEntry.Entry) || logEntry.StartIndexContent < 0) return false;

            int possibleStartIndex = logEntry.Entry.IndexOf('{', logEntry.StartIndexContent);
            if (possibleStartIndex < 0)
            {
                possibleStartIndex = logEntry.Entry.IndexOf('[', logEntry.StartIndexContent);
                if (possibleStartIndex < 0) return false;
            }

            int possibleEndIndex = logEntry.Entry.LastIndexOf('}');
            if (possibleEndIndex < possibleStartIndex)
            {
                possibleEndIndex = logEntry.Entry.LastIndexOf(']');
                if (possibleEndIndex < possibleStartIndex) return false;
            }

            // Additional checks to filter out non-JSON data
            int possibleSubComma = logEntry.Entry.IndexOf('\"', possibleStartIndex + 1, possibleEndIndex - possibleStartIndex - 2);
            if (possibleSubComma < 0) return false;
            int possibleColon = logEntry.Entry.IndexOf(':', possibleStartIndex + 1, possibleEndIndex - possibleStartIndex - 2);
            if (possibleColon < 0) return false;

            return TryJsonPrettyPrint(logEntry.Entry[possibleStartIndex..(possibleEndIndex + 1)], out prettyPrintedJson);
        }

        /// <summary>
        /// Provides default options for parsing JSON documents, enabling trailing commas and skipping comments.
        /// </summary>
        /// <remarks>These options configure the JSON parser to allow trailing commas in arrays and
        /// objects, and to ignore comments during parsing. This can be useful when working with JSON inputs that do not
        /// strictly adhere to the standard format.</remarks>
        private static readonly JsonDocumentOptions documentOptions = new()
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip
        };

        /// <summary>
        /// Provides preconfigured options for JSON serialization with indented formatting and relaxed character
        /// escaping.
        /// </summary>
        /// <remarks>The options enable human-readable output by setting <see
        /// cref="JsonSerializerOptions.WriteIndented"/> to <see langword="true"/> and allow serialization of a wider
        /// range of characters by using <see cref="System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping"/>. 
        /// This prevents unnecessary escaping of characters in the output JSON.</remarks>
        private static readonly JsonSerializerOptions serializerOptions = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        /// <summary>
        /// Attempts to format the specified JSON string with indentation and line breaks for improved readability.
        /// </summary>
        /// <remarks>This method does not throw exceptions for invalid JSON input. If the input is not
        /// valid JSON, the method returns false and the output parameter is set to null.</remarks>
        /// <param name="text">The JSON string to be formatted. Must not be null or empty.</param>
        /// <param name="prettyPrintedJson">When this method returns, contains the formatted JSON string if formatting succeeds; otherwise, null.</param>
        /// <returns>true if the input string is valid JSON and was successfully formatted; otherwise, false.</returns>
        private static bool TryJsonPrettyPrint(string text, out string prettyPrintedJson)
        {
            prettyPrintedJson = null;

            try
            {
                using JsonDocument jsonDocument = JsonDocument.Parse(text, documentOptions);

                prettyPrintedJson = JsonSerializer.Serialize(jsonDocument.RootElement, serializerOptions);
                prettyPrintedJson = ConvertJsonIndentSpacesToTabs(prettyPrintedJson, 2);

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
        /// <summary>
        /// Converts the indentation in a JSON string from spaces to tabs based on the specified number of spaces per
        /// indent level.
        /// </summary>
        /// <remarks>This method only replaces leading spaces at the start of each line with tabs. It does
        /// not modify spaces elsewhere in the JSON or validate the JSON structure.</remarks>
        /// <param name="json">The JSON string in which to replace leading spaces with tabs on each line.</param>
        /// <param name="spacesPerIndent">The number of spaces that represent a single indentation level. Must be greater than zero.</param>
        /// <returns>A string containing the JSON with indentation converted from spaces to tabs. Lines without leading spaces
        /// are left unchanged.</returns>
        private static string ConvertJsonIndentSpacesToTabs(string json, int spacesPerIndent)
        {
            StringBuilder stringBuilder = new(json.Length);

            using (StringReader reader = new(json))
            {
                string line;
                bool firstLine = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (!firstLine)
                    {
                        stringBuilder.Append(Environment.NewLine);
                    }
                    firstLine = false;

                    int spaceCount = 0;
                    while (spaceCount < line.Length && line[spaceCount] == ' ')
                    {
                        spaceCount++;
                    }

                    int tabCount = spaceCount / spacesPerIndent;

                    stringBuilder.Append('\t', tabCount);
                    stringBuilder.Append(line.AsSpan(spaceCount));
                }
            }

            return stringBuilder.ToString();
        }


    }
}
