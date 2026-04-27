using System;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Rendering;

namespace LogScraper.Controls
{
    public partial class LogRangeSelectionControl : UserControl
    {
        private LogRange _range = new();
        private LogEntry _selectedLogEntry = null;
        public LogRange Range
        {
            get
            {
                return _range;
            }
        }

        public LogRangeSelectionControl()
        {
            InitializeComponent();
            UpdateButtons();
        }
        private void LogRangeSelectionControl_Load(object sender, EventArgs e)
        {
            LogAppState.Instance.ViewportSelectedLogEntry.Changed += (s, e) => UpdateSelectedLogEntry();
            LogAppState.Instance.ResetRequested += (ss, ee) => Reset();
        }

        private void UpdateSelectedLogEntry()
        {
            _selectedLogEntry = LogAppState.Instance.ViewportSelectedLogEntry.Value;
            UpdateButtons();
        }

        public void Reset()
        {
            UpdateCheckboxes = true;

            ChkBegin.Checked = false;
            ChkEnd.Checked = false;

            UpdateCheckboxes = false;

            _range = new LogRange();
            UpdateButtons();
        }

        public void ClearBegin()
        {
            if (_range.Begin == null) return;

            UpdateCheckboxes = true;
            ChkBegin.Checked = false;
            UpdateCheckboxes = false;

            _range.Begin = null;
            UpdateButtons();
            RaiseRangeChanged();
        }

        public void ClearEnd()
        {
            if (_range.End == null) return;

            UpdateCheckboxes = true;
            ChkEnd.Checked = false;
            UpdateCheckboxes = false;

            _range.End = null;
            UpdateButtons();
            RaiseRangeChanged();
        }

        public void UpdateButtons()
        {
            ChkBegin.Enabled = _selectedLogEntry != null;
            ChkEnd.Enabled = _selectedLogEntry != null;
            BtnReset.Enabled = _range.IsBeginOrEndSet;
        }

        private void RaiseRangeChanged()
        {
            LogAppState.Instance.Range.ForceSet(_range);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            Reset();
            RaiseRangeChanged();
        }

        private bool UpdateCheckboxes = false;
        private void ChkBegin_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateCheckboxes) return;

            if (_selectedLogEntry == null) return;

            UpdateCheckboxes = true;

            bool checkState = _selectedLogEntry != _range.Begin;
            ChkBegin.Checked = checkState;

            UpdateCheckboxes = false;

            _range.Begin = checkState ? _selectedLogEntry : null;
            UpdateButtons();
            RaiseRangeChanged();
        }

        private void ChkEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateCheckboxes) return;

            if (_selectedLogEntry == null) return;

            UpdateCheckboxes = true;

            bool checkState = _selectedLogEntry != _range.End;
            ChkEnd.Checked = checkState;

            UpdateCheckboxes = false;

            _range.End = checkState ? _selectedLogEntry : null;
            UpdateButtons();
            RaiseRangeChanged();
        }
    }
}