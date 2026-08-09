using System;
using System.Globalization;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.UserData;

namespace LogScraper
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Force English UI resource strings for every thread, including background worker threads,
            // regardless of the OS culture. CurrentCulture (number/date formatting) is left dynamic
            // so it still follows the OS locale.
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en");

            ConfigurationManager.Initialize();
            UserDataManager.Initialize();

            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLogScraper());
        }
    }
}
