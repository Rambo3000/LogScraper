using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Rendering;

namespace LogScraper.Controls.Search
{
    public partial class ErrorListControl : UserControl
    {
        #region Private types

        private sealed class LogEntryDisplayObject(LogEntry logEntry, string displayText)
        {
            public LogEntry LogEntry { get; } = logEntry;
            public override string ToString() => displayText;
        }

        public ErrorListControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Public events

        public event EventHandler<LogEntry> ResultSelected;
        public event EventHandler Close;

        #endregion

        #region Public interface

        public void ShowEntries()
        {
            List<LogEntry> entries = LogAppState.Instance.FilterResultWithRange.Value.ErrorLogEntries;
            LogRenderSettings renderSettings = LogAppState.Instance.RenderSettings.Value;

            LstEntries.Items.Clear();
            LblCount.Text = string.Empty;

            if (entries == null) return;

            LstEntries.BeginUpdate();

            List<LogEntryDisplayObject> logEntryDisplayObjects = new(entries.Count);
            foreach (LogEntry entry in entries)
            {
                if (entry?.Entry == null) continue;
                string text = renderSettings != null
                    ? LogRenderer.RenderSingleLogEntry(entry, renderSettings)
                    : entry.Entry;
                logEntryDisplayObjects.Add(new LogEntryDisplayObject(entry, text));
            }
            LstEntries.Items.AddRange([.. logEntryDisplayObjects]);

            LblCount.Text = $"{LstEntries.Items.Count} fout{(LstEntries.Items.Count == 1 ? string.Empty : "en")} gevonden";
            LstEntries.EndUpdate();
        }

        public void Clear()
        {
            LstEntries.Items.Clear();
        }

        #endregion

        #region Control events

        private void LstEntries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstEntries.SelectedItem is LogEntryDisplayObject item)
            {
                ResultSelected?.Invoke(this, item.LogEntry);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close?.Invoke(this, EventArgs.Empty);
            Clear();
        }
        #endregion

    }
}
