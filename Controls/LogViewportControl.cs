using System;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Rendering;

namespace LogScraper.Controls
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
        }

        public void ClearBegin()
        {
            if (range.Begin == null) return;

            UpdateCheckboxes = true;
            ChkBegin.Checked = false;
            UpdateCheckboxes = false;

            range.Begin = null;
            UpdateButtons();
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ClearEnd()
        {
            if (range.End == null) return;

            UpdateCheckboxes = true;
            ChkEnd.Checked = false;
            UpdateCheckboxes = false;

            range.End = null;
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
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool UpdateCheckboxes = false;
        private void ChkBegin_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateCheckboxes) return;

            if (SelectedLogEntry == null) return;

            UpdateCheckboxes = true;

            bool checkState = SelectedLogEntry != range.Begin;
            ChkBegin.Checked = checkState;

            UpdateCheckboxes = false;

            range.Begin = checkState ? SelectedLogEntry : null;
            UpdateButtons();
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ChkEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateCheckboxes) return;

            if (SelectedLogEntry == null) return;

            UpdateCheckboxes = true;

            bool checkState = SelectedLogEntry != range.End;
            ChkEnd.Checked = checkState;

            UpdateCheckboxes = false;

            range.End = checkState ? SelectedLogEntry : null;
            UpdateButtons();
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}