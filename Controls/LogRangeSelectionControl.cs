using System;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Rendering;
using LogScraper.Utilities;

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
            ShortcutManager.Register(this, AppShortcut.SetRangeBegin, () => { if (ChkBegin.Enabled) ChkBegin_CheckedChanged(this, EventArgs.Empty); });
            ShortcutManager.Register(this, AppShortcut.SetRangeEnd, () => { if (ChkEnd.Enabled) ChkEnd_CheckedChanged(this, EventArgs.Empty); });
            ShortcutManager.Register(this, AppShortcut.ClearRange, () => { if (BtnReset.Enabled) BtnReset_Click(this, EventArgs.Empty); });
        }
        private void LogRangeSelectionControl_Load(object sender, EventArgs e)
        {
            LogAppState.Instance.ViewportSelectedLogEntry.Changed += (s, e) => UpdateSelectedLogEntry();
            LogAppState.Instance.ResetRequested += (ss, ee) => Reset();
            LogAppState.Instance.Range.Changed += (s, e) => ApplyExternalRange();
        }

        private void UpdateSelectedLogEntry()
        {
            _selectedLogEntry = LogAppState.Instance.ViewportSelectedLogEntry.Value;
            UpdateButtons();
        }

        /// <summary>
        /// Syncs the control state when the range is changed externally (e.g. by dragging the timeline).
        /// Skips the update if the new range is already equal to the current internal state to avoid loops.
        /// </summary>
        private void ApplyExternalRange()
        {
            LogRange externalRange = LogAppState.Instance.Range.Value;

            // Avoid re-applying our own ForceSet calls
            if (externalRange?.Equals(_range) == true) return;

            UpdateCheckboxes = true;

            _range = externalRange ?? new LogRange();
            ChkBegin.Checked = _range.Begin != null;
            ChkEnd.Checked = _range.End != null;

            UpdateCheckboxes = false;

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