using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LogScraper.LogProviders;

namespace LogScraper.Configuration
{
    internal class LogScraperConfig
    {
        [JsonConverter(typeof(EnumStringConverter<LogProviderType>))]
        public LogProviderType LogProviderTypeDefault { get; set; }
        public bool ExportToFile { get; set; } = true;
        public string EditorFileName { get; set; }
        public string EditorName { get; set; }
        public string ExportFileName { get; set; }
        public int HttpCLientTimeOUtSeconds { get; set; } = 30;
        public int AutomaticReadTimeMinutes { get; set; } = 2;
    }

    internal class EnumStringConverter<T> : JsonConverter<T> where T : struct, Enum
    {
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

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}

