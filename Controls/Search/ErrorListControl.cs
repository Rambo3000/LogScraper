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

        #endregion

        #region Private fields

        private List<LogEntry> _cachedEntries = [];
        private LogRenderSettings _cachedRenderSettings;

        #endregion

        public ErrorListControl()
        {
            InitializeComponent();
            LogAppState.Instance.FilterResultWithRange.Changed += (s,e) => ShowEntries();
        }

        #region Public events

        public event EventHandler Close;

        #endregion

        #region Public interface

        public void ShowEntries(bool force = false)
        {
            if (!force && !Visible) return;

            List<LogEntry> entries = LogAppState.Instance.FilterResultWithRange.Value?.ErrorLogEntries ?? [];
            LogRenderSettings renderSettings = LogAppState.Instance.RenderSettings.Value;

            // Check if the error list has actually changed
            if (!force && !HasListChanged(entries, renderSettings))
            {
                return;
            }

            // Cache the current entries and settings
            _cachedEntries = entries ?? [];
            _cachedRenderSettings = renderSettings;

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
            _cachedEntries.Clear();
            _cachedRenderSettings = null;
        }

        #endregion

        #region Private methods

        private bool HasListChanged(List<LogEntry> newEntries, LogRenderSettings newRenderSettings)
        {
            // Quick check: different size means it changed
            if (_cachedEntries.Count != (newEntries?.Count ?? 0))
            {
                return true;
            }

            // Check if render settings changed
            if (_cachedRenderSettings != newRenderSettings)
            {
                return true;
            }

            // No entries, no change
            if (newEntries == null || newEntries.Count == 0)
            {
                return false;
            }

            // Compare each entry
            for (int i = 0; i < newEntries.Count; i++)
            {
                if (!_cachedEntries[i].Equals(newEntries[i]))
                {
                    return true;
                }
            }

            // No changes detected
            return false;
        }

        #endregion

        #region Control events

        private void LstEntries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstEntries.SelectedItem is LogEntryDisplayObject item)
            {
                LogAppState.Instance.ViewportSelectedLogEntry.Set(item.LogEntry);
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
