using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LogScraper.Log.LogProviders;

namespace LogScraper.Configuration
{
    /// <summary>
    /// A custom JSON converter for <see cref="LogProviderType"/> that serializes/deserializes the enum as a string.
    /// </summary>
    /// <remarks>
    /// The <see cref="LogProviderType.HttpEndpoint"/> member was previously named "Runtime". This converter
    /// accepts the legacy "Runtime" string value (read from existing configuration files) and maps it to
    /// <see cref="LogProviderType.HttpEndpoint"/>, so no migration of existing JSON files is required. New
    /// values are always written using the current enum member name.
    /// </remarks>
    internal class LogProviderTypeJsonConverter : JsonConverter<LogProviderType>
    {
        public override LogProviderType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            // Legacy name for LogProviderType.HttpEndpoint, kept for backwards compatibility with existing config files.
            if (string.Equals(value, "Runtime", StringComparison.OrdinalIgnoreCase))
            {
                return LogProviderType.HttpEndpoint;
            }

            return Enum.Parse<LogProviderType>(value, ignoreCase: true);
        }

        public override void Write(Utf8JsonWriter writer, LogProviderType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
