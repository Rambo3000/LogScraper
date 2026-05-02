using System;
using LogScraper.Log.Layout;
using LogScraper.Utilities.Extensions;
using System.Text.Json.Serialization;

namespace LogScraper.LogProviders.File
{
    /// <summary>
    /// Represents the configuration for a file-based log provider.
    /// This class implements the <see cref="ILogProviderConfig"/> interface.
    /// </summary>
    internal class FileConfig : ILogProviderConfig, IEquatable<FileConfig>
    {
        /// <summary>
        /// Gets or sets the description of the default log layout for the file-based log provider.
        /// This description is used to identify the default layout configuration.
        /// </summary>
        public string DefaultLogLayoutDescription { get; set; }

        /// <summary>
        /// Gets or sets the default log layout for the file-based log provider.
        /// This property is ignored during JSON serialization to avoid circular references.
        /// </summary>
        [JsonIgnore]
        public LogLayout DefaultLogLayout { get; set; }

        /// <summary>
        /// Gets the type of the log provider, which is always <see cref="LogProviderType.File"/> for this configuration.
        /// </summary>
        [JsonIgnore]
        public LogProviderType LogProviderType
        {
            get { return LogProviderType.File; }
        }

        /// <summary>
        /// Returns a string representation of the file-based log provider configuration.
        /// </summary>
        /// <returns>A localized string representing the log provider ("Lokaal bestand").</returns>
        public override string ToString()
        {
            return "Bestand";
        }

        public bool Equals(FileConfig other) => this.IsEqualByJsonComparison(other);

        public override bool Equals(object obj)
        {
            return Equals(obj as FileConfig);
        }

        public override int GetHashCode()
        {
             return DefaultLogLayoutDescription.GetHashCode();
        }
    }
}
