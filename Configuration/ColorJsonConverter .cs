using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogScraper.Configuration
{
    /// <summary>
    /// A custom JSON converter for the System.Drawing.Color type, allowing it to be serialized and deserialized as a string in the format "R, G, B".
    /// </summary>
    internal class ColorJsonConverter : JsonConverter<Color>
    {
        /// <summary>
        /// Reads a color value from JSON in the format "R,G,B" and returns a corresponding Color object.
        /// </summary>
        /// <param name="reader">The JSON reader positioned at the value to read. Must contain a string representing the color in "R,G,B"
        /// format.</param>
        /// <param name="typeToConvert">The type of object to convert. This parameter is ignored in this implementation.</param>
        /// <param name="options">Options to control the behavior of the JSON serializer. This parameter is not used in this implementation.</param>
        /// <returns>A Color object created from the parsed RGB values, or Color.Empty if the input is null, empty, or not in the
        /// expected format.</returns>
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value)) return Color.Empty;

            // Try R, G, B format first: "0, 97, 0"
            string[] parts = value.Split(',');
            if (parts.Length == 3 &&
                int.TryParse(parts[0].Trim(), out int red) &&
                int.TryParse(parts[1].Trim(), out int green) &&
                int.TryParse(parts[2].Trim(), out int blue))
            {
                return Color.FromArgb(red, green, blue);
            }

            // Fall back to named color for newtonsoft backwards compatibility: "Silver", "Red", etc.
            Color namedColor = Color.FromName(value.Trim());
            if (namedColor.IsKnownColor) return namedColor;

            return Color.Empty;
        }

        /// <summary>
        /// Writes the RGB values of a specified Color object to a JSON writer in a string format suitable for
        /// serialization.
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter to which the Color value will be written.</param>
        /// <param name="value">The Color object whose RGB values are to be serialized. If the value is Color.Empty, an empty string is
        /// written.</param>
        /// <param name="options">The JsonSerializerOptions that influence the serialization process.</param>
        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            if (value == Color.Empty)
            {
                writer.WriteStringValue(string.Empty);
                return;
            }
            writer.WriteStringValue($"{value.R}, {value.G}, {value.B}");
        }
    }
}