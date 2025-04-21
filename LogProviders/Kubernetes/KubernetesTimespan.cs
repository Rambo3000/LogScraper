using System;

namespace LogScraper.LogProviders.Kubernetes
{
    public enum KubernetesTimespan
    {
        Last1Minute,
        Last5Minutes,
        Last10Minutes,
        Last30Minutes,
        Last1Hour,
        Last4Hours,
        Last12Hours,
        Last1Day,
        Everything
    }
    public static class KubernetesTimespanExtensions
    {
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
                KubernetesTimespan.Last12Hours => "Laatst 12 uur",
                KubernetesTimespan.Last1Day => "Laatst 1 dag",
                KubernetesTimespan.Everything => "Alles",
                _ => timeSpan.ToString()
            };
        }
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
