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
            UpdateCheckboxes = true;

            ChkBegin.Checked = false;
            ChkEnd.Checked = false;

            UpdateCheckboxes = false;

            range = new LogRange();
            UpdateButtons();
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateButtons()
        {
            ChkBegin.Enabled = SelectedLogEntry != null;
            ChkEnd.Enabled = SelectedLogEntry != null;
            BtnReset.Enabled = range.IsConstrained;
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private bool UpdateCheckboxes = false;
        private void ChkBegin_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateCheckboxes) return;

            if (SelectedLogEntry == null) return;

            UpdateCheckboxes = true;

            //Make sure the checkbox remains checked, we only uncheck it when the range is cleared,
            //and we want to prevent the user from unchecking it manually while setting the end log entry.
            ChkBegin.Checked = true;

            UpdateCheckboxes = false;

            range.Begin = SelectedLogEntry;
            UpdateButtons();
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ChkEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateCheckboxes) return;

            if (SelectedLogEntry == null) return;

            UpdateCheckboxes = true;

            //Make sure the checkbox remains checked, we only uncheck it when the range is cleared,
            //and we want to prevent the user from unchecking it manually while setting the end log entry.
            ChkEnd.Checked = true;

            UpdateCheckboxes = false;

            range.End = SelectedLogEntry;
            UpdateButtons();
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}