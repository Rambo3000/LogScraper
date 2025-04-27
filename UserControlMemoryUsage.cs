using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogScraper
{
    public partial class UserControlMemoryUsage : UserControl
    {
        private readonly Timer timerMemoryUsage = new();
        public UserControlMemoryUsage()
        {
            InitializeComponent();
        }
        private void UpdateMemoryUsage(object sender, EventArgs e)
        {
            LblMemoryUsageValue.Text = (Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024)).ToString() + " MB";
        }

        private void UserControlMemoryUsage_Load(object sender, EventArgs e)
        {
            // Set the timer here and not in the constructor to avoid running the code in design time
            timerMemoryUsage.Interval = 1000;
            timerMemoryUsage.Tick += new EventHandler(UpdateMemoryUsage);
            timerMemoryUsage.Start();
        }
    }
}
