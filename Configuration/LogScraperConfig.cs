using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using LogScraper.Log;
using LogScraper.Log.Content;
using LogScraper.Log.Metadata;
using LogScraper.LogProviders;
using LogScraper.LogProviders.Runtime;
using LogScraper.LogProviders.Kubernetes;

namespace LogScraper.Configuration
{
    internal class LogScraperConfig
    {
        [JsonConverter(typeof(EnumStringConverter<LogProviderType>))]
        public LogProviderType LogProviderTypeDefault { get; set; }
        public FileConfig FileConfig { get; set; }
        public RuntimeConfig RuntimeConfig { get; set; }
        public KubernetesConfig KubernetesConfig { get; set; }
        public string EditorFileName { get; set; }
        public string EditorName { get; set; }
        public string ExportFileName { get; set; }
    }

    internal abstract class LogProviderGenericConfig
    {
        public string DateTimeFormat { get; set; }
        public List<LogMetadataProperty> LogMetadataProperties { get; set; }
        public List<LogContentProperty> LogContentBeginEndFilters { get; set; }
        public abstract LogProviderType LogProviderType { get; }
        public FilterCriteria RemoveMetaDataCriteria { get; set; }
    }

    internal class FileConfig : LogProviderGenericConfig
    {
        public override LogProviderType LogProviderType
        {
            get { return LogProviderType.File; }
        }
        public override string ToString()
        {
            return "Lokaal bestand";
        }
    }

    internal class RuntimeConfig : LogProviderGenericConfig
    {
        public List<RuntimeInstance> Instances { get; set; }

        public override string ToString()
        {
            return "Directe URL";
        }
        public override LogProviderType LogProviderType
        {
            get { return LogProviderType.Runtime; }
        }
    }
    internal class KubernetesConfig : LogProviderGenericConfig
    {
        public List<KubernetesCluster> Clusters { get; set; }
        public override string ToString()
        {
            return "Kubernetes";
        }
        public override LogProviderType LogProviderType
        {
            get { return LogProviderType.Kubernetes; }
        }
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

