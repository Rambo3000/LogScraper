using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LogScraper
{
    /// <summary>
    /// A user control that displays the current memory usage of the application.
    /// Updates the memory usage value every second using a timer.
    /// </summary>
    public partial class UserControlMemoryUsage : UserControl
    {
        // Timer to periodically update the memory usage display.
        private readonly Timer timerMemoryUsage = new();

        /// <summary>
        /// Initializes the user control and sets up the memory usage tooltip.
        /// </summary>
        public UserControlMemoryUsage()
        {
            InitializeComponent();

            // Set a tooltip for the memory usage label to provide additional context to the user.
            new ToolTip().SetToolTip(LblMemoryUsageValue, "Geheugengebruik");
        }

        /// <summary>
        /// Updates the memory usage label with the current working set size of the process.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data.</param>
        private void UpdateMemoryUsage(object sender, EventArgs e)
        {
            // Get the current process's memory usage in megabytes and display it in the label.
            LblMemoryUsageValue.Text = (Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024)).ToString() + " MB";
        }

        /// <summary>
        /// Starts the memory usage update timer when the control is loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data.</param>
        private void UserControlMemoryUsage_Load(object sender, EventArgs e)
        {
            // Set the timer interval to 1 second (1000 milliseconds).
            // This ensures the memory usage is updated every second.
            timerMemoryUsage.Interval = 1000;

            // Attach the UpdateMemoryUsage method to the timer's Tick event.
            timerMemoryUsage.Tick += new EventHandler(UpdateMemoryUsage);

            // Start the timer to begin periodic updates.
            timerMemoryUsage.Start();
        }
    }
}
