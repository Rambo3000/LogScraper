using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LogScraper.LogProviders;

namespace LogScraper.Configuration.Generic
{
    /// <summary>
    /// Represents the generic configuration settings for the application.
    /// Includes default log provider type, file export settings, and other general configurations.
    /// </summary>
    internal class GenericConfig
    {
        /// <summary>
        /// Gets or sets the default log provider type for the application.
        /// This determines which log provider (e.g., Kubernetes, Runtime, File) is used by default.
        /// </summary>
        [JsonConverter(typeof(EnumStringConverter<LogProviderType>))]
        public LogProviderType LogProviderTypeDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether logs should be exported to a file.
        /// Default value is <c>true</c>.
        /// </summary>
        public bool ExportToFile { get; set; } = true;

        /// <summary>
        /// Gets or sets the name of the file to be opened in an external editor.
        /// </summary>
        public string EditorFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the file where logs will be exported.
        /// </summary>
        public string ExportFileName { get; set; }

        /// <summary>
        /// Gets or sets the timeout value (in seconds) for HTTP client requests.
        /// Default value is 30 seconds.
        /// </summary>
        public int HttpCLientTimeOUtSeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets the automatic read time (in minutes) for logs.
        /// Default value is 2 minutes.
        /// </summary>
        public int AutomaticReadTimeMinutes { get; set; } = 2;
    }

    /// <summary>
    /// A custom JSON converter for serializing and deserializing enums as strings.
    /// </summary>
    /// <typeparam name="T">The enum type to be converted.</typeparam>
    internal class EnumStringConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        /// <summary>
        /// Reads and converts the JSON string to the specified enum type.
        /// </summary>
        /// <param name="reader">The JSON reader.</param>
        /// <param name="typeToConvert">The type of the enum to convert to.</param>
        /// <param name="options">Serialization options.</param>
        /// <returns>The converted enum value.</returns>
        /// <exception cref="JsonException">Thrown if the JSON string cannot be converted to the enum type.</exception>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string enumString = reader.GetString();
                if (Enum.TryParse(enumString, true, out T value))
                {
                    return value;
                }
            }
            throw new JsonException($"Unable to convert to enum of type {typeof(T)}");
        }

        /// <summary>
        /// Writes the enum value as a string to the JSON output.
        /// </summary>
        /// <param name="writer">The JSON writer.</param>
        /// <param name="value">The enum value to write.</param>
        /// <param name="options">Serialization options.</param>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}

