using System.Collections;
using System.Collections.Generic;
using LogScraper.Log.Content;
using LogScraper.Utilities.IndexDictionary;

namespace LogScraper.Log.Processing
{
    internal static class LogContentMaskBuilder
    {
        /// <summary>
        /// Builds or incrementally updates a dictionary mapping each content property to a BitArray
        /// indicating which log entries (by index) have that property.
        /// When an <paramref name="initialDictionairy"/> is provided, existing BitArrays are expanded
        /// and only the entries beyond the original length are evaluated.
        /// </summary>
        public static IndexDictionary<LogContentProperty, BitArray> Build(List<LogEntry> logEntries, List<LogContentProperty> contentProperties, IndexDictionary<LogContentProperty, BitArray> initialDictionairy = null)
        {
            int totalCount = logEntries.Count;
            int startIndex = 0;
            IndexDictionary<LogContentProperty, BitArray> result = new(contentProperties.Count);

            if (initialDictionairy != null)
            {
                // Carry over existing BitArrays, expanded to the new total length
                foreach (LogContentProperty property in contentProperties)
                {
                    if (initialDictionairy.TryGetValue(property, out BitArray existing))
                    {
                        BitArray expanded = new(totalCount);
                        for (int i = 0; i < existing.Length; i++)
                            expanded[i] = existing[i];
                        result[property] = expanded;
                        startIndex = existing.Length;
                    }
                    else
                    {
                        result[property] = new BitArray(totalCount);
                    }
                }
            }
            else
            {
                foreach (LogContentProperty property in contentProperties)
                    result[property] = new BitArray(totalCount);
            }

            for (int i = startIndex; i < totalCount; i++)
            {
                LogEntry entry = logEntries[i];
                if (entry.LogContentProperties == null) continue;

                foreach (LogContentProperty property in contentProperties)
                {
                    if (entry.LogContentProperties.ContainsKey(property))
                        result[property][i] = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Builds a BitArray where each bit is set if any error content property is present for that log entry.
        /// The result is the OR of the BitArrays of all content properties where <see cref="LogContentProperty.IsErrorProperty"/> is true.
        /// </summary>
        public static BitArray BuildErrorMask(List<LogContentProperty> contentProperties, IndexDictionary<LogContentProperty, BitArray> contentPropertiesMask, int logEntryCount)
        {
            BitArray errorMask = new(logEntryCount);

            foreach (LogContentProperty property in contentProperties)
            {
                if (!property.IsErrorProperty) continue;
                if (!contentPropertiesMask.TryGetValue(property, out BitArray propertyMask)) continue;
                errorMask.Or(propertyMask);
            }

            return errorMask;
        }
    }
}
