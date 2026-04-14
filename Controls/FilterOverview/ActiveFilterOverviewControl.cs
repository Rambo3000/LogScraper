using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log;
using LogScraper.Log.Metadata;
using LogScraper.Log.Rendering;

namespace LogScraper.Controls.FilterOverview
{
    public partial class ActiveFilterOverviewControl : UserControl
    {
        #region Fields, events, and constructor
        private const int ChipHeight = 18;
        private const int ChipLineSpacing = 3;
        private const int CountLabelLeftGap = 4;

        /// <summary>
        /// Raised when the error chip is clicked.
        /// </summary>
        public event EventHandler<ErrorChipClickedEventArgs> ErrorChipClicked;

        /// <summary>
        /// Raised when a metadata filter chip's remove button is clicked.
        /// </summary>
        public event EventHandler<FilterRemovedEventArgs> FilterRemoved;

        /// <summary>
        /// Raised when a log-range chip's remove button is clicked.
        /// </summary>
        public event EventHandler<RangeRemovedEventArgs> RangeRemoved;

        private IReadOnlyList<LogEntry> _errorEntries;
        private IReadOnlyList<LogMetadataFilter> _metadataFilters = [];
        private LogRange _logRange;
        private FilterChipControl _errorChip;
        private FilterChipControl _rangeBeginChip;
        private FilterChipControl _rangeEndChip;
        private readonly Dictionary<FilterChipControl, (LogMetadataFilter Filter, LogMetadataValue Value)> _metadataChipToValue = [];
        private readonly ToolTip _toolTip = new();
        private bool _inLayout;

        public ActiveFilterOverviewControl()
        {
            InitializeComponent();
            LblCount.Text = string.Empty;
        }
        #endregion

        #region Public API

        /// <summary>
        /// Replaces the current error entries and rebuilds the error chip.
        /// Pass <see langword="null"/> or an empty list to remove the error chip.
        /// </summary>
        public void SetErrorEntries(IReadOnlyList<LogEntry> entries)
        {
            _errorEntries = entries;
            RebuildChips();
        }

        /// <summary>
        /// Replaces the current metadata filters and rebuilds all filter chips.
        /// Pass <see langword="null"/> or an empty list to clear all filter chips.
        /// </summary>
        public void SetMetadataFilters(IReadOnlyList<LogMetadataFilter> filters)
        {
            _metadataFilters = filters ?? [];
            RebuildChips();
        }

        /// <summary>
        /// Replaces the current log range. Creates a Begin chip and/or End chip when those entries
        /// are set on the range. Pass <see langword="null"/> to remove all range chips.
        /// </summary>
        public void SetLogRange(LogRange range)
        {
            _logRange = range;
            RebuildChips();
        }

        /// <summary>
        /// Updates the visible / total entry count shown at the top-right.
        /// When <paramref name="visible"/> equals <paramref name="total"/>, only the total is displayed.
        /// </summary>
        public void SetCounts(int visible, int total)
        {
            LblCount.Text = visible == total ? $"{total}" : $"{visible} / {total}";
            _toolTip.SetToolTip(LblCount, visible == total ? "Totaal aantal logregels" : "Zichtbare logregels / Totaal aantal logregels");
            RecalculateLayout();
        }
        #endregion

        #region Chip management
        private void RebuildChips()
        {
            SuspendLayout();
            DisposeAllChips();

            // 1. Error chip
            if (_errorEntries?.Count > 0)
            {
                _errorChip = FilterChipControl.FromErrorCount(_errorEntries.Count);
                _errorChip.ChipClicked += OnErrorChipClicked;
                FlowLayoutFilterChips.Controls.Add(_errorChip);
            }

            // 2. Range chips (begin then end)
            if (_logRange?.Begin != null)
            {
                _rangeBeginChip = FilterChipControl.FromLogRange(_logRange, LogRangeChipVariant.Begin);
                _rangeBeginChip.ChipClicked += OnRangeBeginChipClicked;
                FlowLayoutFilterChips.Controls.Add(_rangeBeginChip);
            }

            if (_logRange?.End != null)
            {
                _rangeEndChip = FilterChipControl.FromLogRange(_logRange, LogRangeChipVariant.End);
                _rangeEndChip.ChipClicked += OnRangeEndChipClicked;
                FlowLayoutFilterChips.Controls.Add(_rangeEndChip);
            }
            ResumeLayout();
            // 3. Metadata chips — added and sized inside RecalculateLayout
            RecalculateLayout();
        }

        private void DisposeAllChips()
        {
            foreach (var chip in _metadataChipToValue.Keys)
                chip.ChipClicked -= OnMetadataChipClicked;
            _metadataChipToValue.Clear();

            foreach (Control c in FlowLayoutFilterChips.Controls)
                c.Dispose();
            FlowLayoutFilterChips.Controls.Clear();

            _errorChip = null;
            _rangeBeginChip = null;
            _rangeEndChip = null;
        }

        /// <summary>
        /// Removes and disposes only the metadata chips, leaving error and range chips intact.
        /// </summary>
        private void RemoveAndDisposeMetadataChips()
        {
            var chips = _metadataChipToValue.Keys.ToList();
            _metadataChipToValue.Clear();

            foreach (var chip in chips)
            {
                chip.ChipClicked -= OnMetadataChipClicked;
                FlowLayoutFilterChips.Controls.Remove(chip);
                chip.Dispose();
            }
        }

        /// <summary>
        /// Builds metadata chips in either full mode (one chip per active value) or shortened mode
        /// (one chip per filter property). Replaces any existing metadata chips.
        /// </summary>
        private void AddMetadataChips(bool shortened)
        {
            RemoveAndDisposeMetadataChips();

            foreach (var filter in _metadataFilters)
            {
                if (shortened || filter.ActiveValues.Count == 0)
                {
                    AddPropertyLevelChip(filter);
                }
                else
                {
                    foreach (var value in filter.ActiveValues.Keys)
                        AddPerValueChip(filter, value);
                }
            }
        }

        private void AddPropertyLevelChip(LogMetadataFilter filter)
        {
            var chip = FilterChipControl.FromMetadataFilter(filter);
            chip.ChipClicked += OnMetadataChipClicked;

            // If exactly one value is active the caller can identify the specific value to remove;
            // for multi-value filters null signals "remove the whole property filter".
            LogMetadataValue valueForEvent = filter.ActiveValues.Count == 1
                ? filter.ActiveValues.Keys.First()
                : null;

            _metadataChipToValue[chip] = (filter, valueForEvent);
            FlowLayoutFilterChips.Controls.Add(chip);
        }

        private void AddPerValueChip(LogMetadataFilter filter, LogMetadataValue value)
        {
            var chip = FilterChipControl.FromMetadataFilterValue(filter, value);
            chip.ChipClicked += OnMetadataChipClicked;
            _metadataChipToValue[chip] = (filter, value);
            FlowLayoutFilterChips.Controls.Add(chip);
        }
        #endregion

        #region Layout logic
        private void RecalculateLayout()
        {
            if (_inLayout) return;
            _inLayout = true;
            try
            {
                SuspendLayout();

                // Pin LblCount to top-right, vertically centred within the chip row height.
                int lblY = Math.Max(0, (ChipHeight - LblCount.Height) / 2);
                LblCount.Location = new Point(Width - LblCount.Width, lblY);

                // Flow panel fills the remaining width to the left of the count label.
                int flowWidth = LblCount.Text.Length > 0
                    ? Math.Max(20, LblCount.Left - CountLabelLeftGap)
                    : Math.Max(20, Width);

                FlowLayoutFilterChips.Location = new Point(0, 0);
                FlowLayoutFilterChips.Width = flowWidth;

                // Try full mode (one chip per value); fall back to shortened (one chip per property)
                // if the chips wrap beyond a single line.
                AddMetadataChips(shortened: false);
                if (CalculateRequiredLines(flowWidth) > 1)
                    AddMetadataChips(shortened: true);

                // Resize the panel and the control to fit the actual number of chip rows.
                int lines = CalculateRequiredLines(flowWidth);
                int flowHeight = lines * ChipHeight + (lines - 1) * ChipLineSpacing;
                FlowLayoutFilterChips.Height = flowHeight;
                Height = flowHeight;

                ResumeLayout(true);
            }
            finally
            {
                _inLayout = false;
            }
        }

        /// <summary>
        /// Simulates FlowLayoutPanel wrapping to count how many rows the current chips occupy
        /// inside <paramref name="availableWidth"/>.
        /// </summary>
        private int CalculateRequiredLines(int availableWidth)
        {
            if (FlowLayoutFilterChips.Controls.Count == 0)
                return 1;

            int x = 0;
            int lines = 1;

            foreach (Control chip in FlowLayoutFilterChips.Controls)
            {
                int chipFullWidth = chip.Width + chip.Margin.Horizontal;
                if (x > 0 && x + chipFullWidth > availableWidth)
                {
                    lines++;
                    x = 0;
                }
                x += chipFullWidth;
            }

            return lines;
        }
        #endregion

        #region Event handlers
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RecalculateLayout();
        }
        private void OnErrorChipClicked(object sender, EventArgs e)
        {
            ErrorChipClicked?.Invoke(this, new ErrorChipClickedEventArgs(_errorEntries));
        }

        private void OnRangeBeginChipClicked(object sender, EventArgs e)
        {
            RangeRemoved?.Invoke(this, new RangeRemovedEventArgs(LogRangeChipVariant.Begin));
        }

        private void OnRangeEndChipClicked(object sender, EventArgs e)
        {
            RangeRemoved?.Invoke(this, new RangeRemovedEventArgs(LogRangeChipVariant.End));
        }

        private void OnMetadataChipClicked(object sender, EventArgs e)
        {
            if (sender is not FilterChipControl chip) return;
            if (!_metadataChipToValue.TryGetValue(chip, out var entry)) return;

            FilterRemoved?.Invoke(this, new FilterRemovedEventArgs(entry.Filter.Property, entry.Value));
        }

        internal void Clear()
        {
            _metadataChipToValue.Clear();
            _errorEntries = null;
            _metadataFilters = [];
            _logRange = null;


            DisposeAllChips();
            RecalculateLayout();
        }
        #endregion
    }
}

