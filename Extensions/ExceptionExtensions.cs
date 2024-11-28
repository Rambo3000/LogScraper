using System;
using System.IO;
using System.Text;

namespace LogScraper.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetFullStackTrace(this Exception ex)
        {
            StringBuilder sb = new();

            while (ex != null)
            {
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                ex = ex.InnerException;
            }

            return sb.ToString();
        }

        public static void LogStackTraceToFile(this Exception ex)
        {
            try
            {
                string fullStackTrace = ex.GetFullStackTrace();
                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StackTrace.log");

                File.AppendAllText(logFilePath, $"{DateTime.Now}: {fullStackTrace}\n");
            }
            catch { }
        }
    }
}
