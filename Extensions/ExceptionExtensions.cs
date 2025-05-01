using System;
using System.IO;
using System.Text;

namespace LogScraper.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Exception"/> class to enhance its functionality.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Retrieves the full stack trace of an exception, including all inner exceptions.
        /// </summary>
        /// <param name="ex">The exception to retrieve the stack trace from.</param>
        /// <returns>A string containing the full stack trace and messages of the exception and its inner exceptions.</returns>
        public static string GetFullStackTrace(this Exception ex)
        {
            // Use a StringBuilder to efficiently build the full stack trace.
            StringBuilder sb = new();

            // Iterate through the exception and its inner exceptions.
            while (ex != null)
            {
                // Append the exception message and stack trace to the StringBuilder.
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                ex = ex.InnerException;
            }

            // Return the complete stack trace as a string.
            return sb.ToString();
        }

        /// <summary>
        /// Logs the full stack trace of an exception to a file.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public static void LogStackTraceToFile(this Exception ex)
        {
            try
            {
                // Get the full stack trace of the exception.
                string fullStackTrace = ex.GetFullStackTrace();

                // Define the log file path in the application's base directory.
                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StackTrace.log");

                // Append the stack trace to the log file with a timestamp.
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {fullStackTrace}\n");
            }
            catch
            {
                // Suppress any exceptions that occur during logging to avoid interfering with the main application flow.
            }
        }
    }
}
