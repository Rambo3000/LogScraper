using Newtonsoft.Json;

namespace LogScraper.Extensions
{
    /// <summary>
    /// Provides extension methods for object comparison.
    /// </summary>
    public static class ObjectExtensions
    {
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
            // Configure JSON serialization settings to ignore null values and reference loops.
            JsonSerializerSettings settings = new()
            {
                Formatting = Formatting.None, // Minimize JSON formatting for comparison.
                NullValueHandling = NullValueHandling.Ignore, // Ignore null values during serialization.
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Prevent infinite loops in case of circular references.
            };

            // Serialize both objects to JSON strings using the specified settings.
            string json1 = JsonConvert.SerializeObject(obj1, settings);
            string json2 = JsonConvert.SerializeObject(obj2, settings);

            // Compare the JSON strings for equality.
            return json1 == json2;
        }
    }
}
