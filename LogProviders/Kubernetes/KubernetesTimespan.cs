using System;

namespace LogScraper.LogProviders.Kubernetes
{
    /// <summary>
    /// Represents the timespan options for fetching logs from a Kubernetes cluster.
    /// Each value corresponds to a specific duration or range of time.
    /// </summary>
    public enum KubernetesTimespan
    {
        /// <summary>
        /// Fetch all logs without any time restriction.
        /// </summary>
        Everything,

        /// <summary>
        /// Fetch logs from the last 1 minute.
        /// </summary>
        Last1Minute,

        /// <summary>
        /// Fetch logs from the last 5 minutes.
        /// </summary>
        Last5Minutes,

        /// <summary>
        /// Fetch logs from the last 10 minutes.
        /// </summary>
        Last10Minutes,

        /// <summary>
        /// Fetch logs from the last 30 minutes.
        /// </summary>
        Last30Minutes,

        /// <summary>
        /// Fetch logs from the last 1 hour.
        /// </summary>
        Last1Hour,

        /// <summary>
        /// Fetch logs from the last 4 hours.
        /// </summary>
        Last4Hours,

        /// <summary>
        /// Fetch logs from the last 12 hours.
        /// </summary>
        Last12Hours,

        /// <summary>
        /// Fetch logs from the last 1 day.
        /// </summary>
        Last1Day
    }
    /// <summary>
    /// Provides extension methods for the <see cref="KubernetesTimespan"/> enum.
    /// Includes methods for converting timespan values to readable strings and calculating the start time for logs.
    /// </summary>
    public static class KubernetesTimespanExtensions
    {
        /// <summary>
        /// Converts a <see cref="KubernetesTimespan"/> value to a human-readable string.
        /// </summary>
        /// <param name="timeSpan">The timespan value to convert.</param>
        /// <returns>A localized string representation of the timespan.</returns>
        public static string ToReadableString(this KubernetesTimespan timeSpan)
        {
            return timeSpan switch
            {
                KubernetesTimespan.Last1Minute => "Laatste 1 min",
                KubernetesTimespan.Last5Minutes => "Laatste 5 min",
                KubernetesTimespan.Last10Minutes => "Laatste 10 min",
                KubernetesTimespan.Last30Minutes => "Laatste 30 min",
                KubernetesTimespan.Last1Hour => "Laatste 1 uur",
                KubernetesTimespan.Last4Hours => "Laatste 4 uur",
                KubernetesTimespan.Last12Hours => "Laatste 12 uur",
                KubernetesTimespan.Last1Day => "Laatste 1 dag",
                KubernetesTimespan.Everything => "Alles",
                _ => timeSpan.ToString()
            };
        }

        /// <summary>
        /// Calculates the start time for fetching logs based on the <see cref="KubernetesTimespan"/> value.
        /// </summary>
        /// <param name="timeSpan">The timespan value to calculate the start time for.</param>
        /// <returns>
        /// A <see cref="DateTime"/> representing the start time for the logs, or <c>null</c> if the timespan is "Everything".
        /// </returns>
        public static DateTime? TolastTrailTime(this KubernetesTimespan timeSpan)
        {
            return timeSpan switch
            {
                KubernetesTimespan.Last1Minute => DateTime.Now.AddMinutes(-1),
                KubernetesTimespan.Last5Minutes => DateTime.Now.AddMinutes(-5),
                KubernetesTimespan.Last10Minutes => DateTime.Now.AddMinutes(-10),
                KubernetesTimespan.Last30Minutes => DateTime.Now.AddMinutes(-30),
                KubernetesTimespan.Last1Hour => DateTime.Now.AddHours(-1),
                KubernetesTimespan.Last4Hours => DateTime.Now.AddHours(-4),
                KubernetesTimespan.Last12Hours => DateTime.Now.AddHours(-12),
                KubernetesTimespan.Last1Day => DateTime.Now.AddDays(-1),
                KubernetesTimespan.Everything => null,
                _ => null
            };
        }
    }
}
