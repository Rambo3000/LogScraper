using System;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Rendering;

namespace LogScraper.Utilities.UserControls
{
    public partial class LogViewportControl : UserControl
    {
        public event EventHandler RangeChanged;

        private LogEntry selectedLogEntry;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogEntry SelectedLogEntry
        {
            get => selectedLogEntry;
            set
            {
                selectedLogEntry = value;
                UpdateButtons();
            }
        }

        private LogRange range = new();

        public LogRange Range
        {
            get
            {
                return range;
            }
        }

        public LogViewportControl()
        {
            InitializeComponent();
            UpdateButtons();
        }

        public void Clear()
        {
            range = new LogRange();
            UpdateButtons();
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateButtons()
        {
            BtnBegin.Enabled = SelectedLogEntry != null;
            BtnEnd.Enabled = SelectedLogEntry != null;
            BtnReset.Enabled = range.IsConstrained;
        }

        private void BtnBegin_Click(object sender, EventArgs e)
        {
            range.Begin = SelectedLogEntry;
            UpdateButtons();
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnEnd_Click(object sender, EventArgs e)
        {
            range.End = SelectedLogEntry;
            UpdateButtons();
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}