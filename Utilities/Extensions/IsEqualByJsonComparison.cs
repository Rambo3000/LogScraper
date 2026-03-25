using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogScraper.Utilities.Extensions
{
    /// <summary>
    /// Provides extension methods for object comparison.
    /// </summary>
    public static class ObjectExtensions
    {
        private static readonly JsonSerializerOptions jsonComparisonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Ignore null values during serialization.
            ReferenceHandler = ReferenceHandler.IgnoreCycles // Prevent infinite loops in case of circular references.
        };

        /// <summary>
        /// Compares two objects for equality by serializing them to JSON and comparing the resulting strings.
        /// </summary>
        /// <param name="obj1">The first object to compare.</param>
        /// <param name="obj2">The second object to compare.</param>
        /// <returns>
        /// True if the JSON representations of the two objects are equal; otherwise, false.
        /// </returns>
        public static bool IsEqualByJsonComparison(this object obj1, object obj2)
        {
            string json1 = JsonSerializer.Serialize(obj1, jsonComparisonOptions);
            string json2 = JsonSerializer.Serialize(obj2, jsonComparisonOptions);
            return json1 == json2;
        }
    }
}