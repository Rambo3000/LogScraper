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
        private const int ChipLineSpacing = 5;
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

        /// <summary>
        /// Tracks state for each metadata property. A property group is either:
        ///   Expanded  — ValueChips contains one chip per active value in the flow panel,
        ///               CollapsedChip is null.
        ///   Collapsed — CollapsedChip is in the flow panel, ValueChips are kept alive but detached.
        /// </summary>
        private readonly Dictionary<LogMetadataProperty, MetadataPropertyGroup> _propertyGroups = [];

        private readonly ToolTip _toolTip = new();
        private bool _inLayout;

        public ActiveFilterOverviewControl()
        {
            InitializeComponent();
            LblCount.Text = string.Empty;
        }
        #endregion

        #region Inner type — property group

        private sealed class MetadataPropertyGroup(LogMetadataFilter filter)
        {
            /// <summary>The filter this group represents. Updated on each sync.</summary>
            public LogMetadataFilter Filter { get; set; } = filter;

            /// <summary>
            /// One chip per active value. Always kept up to date; may be detached from the
            /// flow panel while the group is collapsed.
            /// </summary>
            public List<FilterChipControl> ValueChips { get; } = [];

            /// <summary>
            /// The single count chip shown when the group is collapsed. Null while expanded.
            /// </summary>
            public FilterChipControl CollapsedChip { get; set; }

            public bool IsCollapsed => CollapsedChip != null;

            /// <summary>Total width of all value chips including margins, as if expanded.</summary>
            public int TotalExpandedWidth =>
                ValueChips.Sum(chip => chip.Width + chip.Margin.Horizontal);
        }
        #endregion

        #region Public API

        public void SetErrorEntries(IReadOnlyList<LogEntry> entries)
        {
            _errorEntries = entries;
            SyncErrorChip();
            RecalculateLayout();
        }

        /// <summary>
        /// Diffs the supplied filter list against the current metadata chips:
        /// adds chips for new properties, updates chips whose values changed,
        /// removes chips whose property is no longer present.
        /// Pass <see langword="null"/> or an empty list to clear all metadata chips.
        /// </summary>
        public void SetMetadataFilters(IReadOnlyList<LogMetadataFilter> filters)
        {
            _metadataFilters = filters ?? [];
            SyncMetadataChips();
            RecalculateLayout();
        }

        /// <summary>
        /// Updates range chips in place, or adds/removes them as needed.
        /// Pass <see langword="null"/> to remove all range chips.
        /// </summary>
        public void SetLogRange(LogRange range)
        {
            _logRange = range;
            SyncRangeChips();
            RecalculateLayout();
        }

        /// <summary>
        /// Updates the visible / total entry count shown at the top-right.
        /// When <paramref name="visible"/> equals <paramref name="total"/>, only the total is displayed.
        /// </summary>
        public void SetCounts(int visible, int total)
        {
            LblCount.Text = visible == total ? $"{total:N0}" : $"{visible:N0} / {total:N0}";
            _toolTip.SetToolTip(LblCount, visible == total
                ? "Totaal aantal logregels"
                : "Zichtbare logregels / Totaal aantal logregels");
            RecalculateLayout();
        }

        internal void Clear()
        {
            _errorEntries = null;
            _metadataFilters = [];
            _logRange = null;
            DisposeAllChips();
            RecalculateLayout();
        }
        #endregion

        #region Chip synchronisation

        /// <summary>
        /// Adds, updates, or removes the error chip to match <see cref="_errorEntries"/>.
        /// </summary>
        private void SyncErrorChip()
        {
            bool shouldExist = _errorEntries?.Count > 0;

            if (shouldExist && _errorChip == null)
            {
                _errorChip = FilterChipControl.FromErrorCount(_errorEntries.Count);
                _errorChip.ChipClicked += OnErrorChipClicked;
                FlowLayoutFilterChips.Controls.Add(_errorChip);
                FlowLayoutFilterChips.Controls.SetChildIndex(_errorChip, 0);
            }
            else if (shouldExist)
            {
                _errorChip.SetErrorCount(_errorEntries.Count);
            }
            else if (_errorChip != null)
            {
                FlowLayoutFilterChips.Controls.Remove(_errorChip);
                _errorChip.ChipClicked -= OnErrorChipClicked;
                _errorChip.Dispose();
                _errorChip = null;
            }
        }

        /// <summary>
        /// Adds, updates, or removes range chips to match <see cref="_logRange"/>.
        /// </summary>
        private void SyncRangeChips()
        {
            SyncSingleRangeChip(
                variant: LogRangeChipVariant.Begin,
                shouldExist: _logRange?.Begin != null,
                existingChip: ref _rangeBeginChip,
                clickHandler: OnRangeBeginChipClicked,
                insertAfter: _errorChip);

            SyncSingleRangeChip(
                variant: LogRangeChipVariant.End,
                shouldExist: _logRange?.End != null,
                existingChip: ref _rangeEndChip,
                clickHandler: OnRangeEndChipClicked,
                insertAfter: _rangeBeginChip ?? _errorChip);
        }

        /// <summary>
        /// Adds, updates, or removes a single range chip (begin or end) to match the corresponding
        /// </summary>
        /// <param name="variant"> Whether this is the begin or end chip.</param>
        /// <param name="shouldExist">Whether the chip should be present based on the current log range.</param>
        /// <param name="existingChip">Reference to the existing chip control, if any. Updated if a new chip is created or removed.</param>
        /// <param name="clickHandler">Event handler to attach if a new chip is created, or detach if an existing chip is removed.</param>
        /// <param name="insertAfter">The chip after which the range chip should be inserted, or null to insert at the start.</param>
        private void SyncSingleRangeChip(LogRangeChipVariant variant, bool shouldExist, ref FilterChipControl existingChip, EventHandler clickHandler, FilterChipControl insertAfter)
        {
            if (shouldExist && existingChip == null)
            {
                existingChip = FilterChipControl.FromLogRange(_logRange, variant);
                existingChip.ChipClicked += clickHandler;
                int insertIndex = insertAfter != null
                    ? FlowLayoutFilterChips.Controls.IndexOf(insertAfter) + 1
                    : 0;
                FlowLayoutFilterChips.Controls.Add(existingChip);
                FlowLayoutFilterChips.Controls.SetChildIndex(existingChip, insertIndex);
            }
            else if (shouldExist)
            {
                existingChip.UpdateLogRange(_logRange);
            }
            else if (existingChip != null)
            {
                FlowLayoutFilterChips.Controls.Remove(existingChip);
                existingChip.ChipClicked -= clickHandler;
                existingChip.Dispose();
                existingChip = null;
            }
        }

        /// <summary>
        /// Diffs _metadataFilters against _propertyGroups. After this call every group
        /// is fully expanded in the flow panel — collapse state is applied in RecalculateLayout.
        /// </summary>
        private void SyncMetadataChips()
        {
            // Remove groups for properties that have disappeared.
            var activeProperties = _metadataFilters.Select(f => f.Property).ToHashSet();
            foreach (var property in _propertyGroups.Keys.Where(p => !activeProperties.Contains(p)).ToList())
            {
                DisposePropertyGroup(_propertyGroups[property]);
                _propertyGroups.Remove(property);
            }

            int insertPosition = GetMetadataInsertBaseIndex();

            foreach (var filter in _metadataFilters)
            {
                if (_propertyGroups.TryGetValue(filter.Property, out var group))
                {
                    // Expand first so index management is straightforward.
                    EnsureGroupExpanded(group, insertPosition);
                    group.Filter = filter;
                    SyncValueChips(group, filter, insertPosition);
                    insertPosition += group.ValueChips.Count;
                }
                else
                {
                    group = new MetadataPropertyGroup(filter);
                    _propertyGroups[filter.Property] = group;
                    BuildValueChips(group, filter, insertPosition);
                    insertPosition += group.ValueChips.Count;
                }
            }
        }

        private void BuildValueChips(MetadataPropertyGroup group, LogMetadataFilter filter, int startIndex)
        {
            int index = startIndex;
            foreach (var value in filter.ActiveValues.Keys)
            {
                var chip = FilterChipControl.FromMetadataFilterValue(filter, value);
                chip.ChipClicked += OnMetadataChipClicked;
                group.ValueChips.Add(chip);
                FlowLayoutFilterChips.Controls.Add(chip);
                FlowLayoutFilterChips.Controls.SetChildIndex(chip, index++);
            }
        }

        /// <summary>
        /// Diffs active values against existing value chips. Assumes the group is expanded
        /// (all value chips are in the flow panel at startIndex..startIndex+n).
        /// </summary>
        private void SyncValueChips(MetadataPropertyGroup group, LogMetadataFilter filter, int startIndex)
        {
            var currentValues = group.ValueChips.Select(c => c.SpecificValue).ToHashSet();
            var newValues = filter.ActiveValues.Keys.ToHashSet();

            // Remove chips for values that are gone.
            foreach (var chip in group.ValueChips.Where(c => !newValues.Contains(c.SpecificValue)).ToList())
            {
                group.ValueChips.Remove(chip);
                chip.ChipClicked -= OnMetadataChipClicked;
                FlowLayoutFilterChips.Controls.Remove(chip);
                chip.Dispose();
            }

            // Add chips for values that are new.
            int appendIndex = startIndex + group.ValueChips.Count;
            foreach (var value in filter.ActiveValues.Keys.Where(v => !currentValues.Contains(v)))
            {
                var chip = FilterChipControl.FromMetadataFilterValue(filter, value);
                chip.ChipClicked += OnMetadataChipClicked;
                group.ValueChips.Add(chip);
                FlowLayoutFilterChips.Controls.Add(chip);
                FlowLayoutFilterChips.Controls.SetChildIndex(chip, appendIndex++);
            }

            // Reorder chips to match filter.ActiveValues order.
            var chipByValue = group.ValueChips.ToDictionary(c => c.SpecificValue);
            group.ValueChips.Clear();
            int position = startIndex;
            foreach (var value in filter.ActiveValues.Keys)
            {
                if (!chipByValue.TryGetValue(value, out var chip)) continue;
                group.ValueChips.Add(chip);
                FlowLayoutFilterChips.Controls.SetChildIndex(chip, position++);
            }
        }

        /// <summary>
        /// If the group is collapsed, removes the collapsed chip and re-inserts value chips
        /// at startIndex. No-op if already expanded.
        /// </summary>
        private void EnsureGroupExpanded(MetadataPropertyGroup group, int startIndex)
        {
            if (!group.IsCollapsed) return;

            FlowLayoutFilterChips.Controls.Remove(group.CollapsedChip);
            group.CollapsedChip.ChipClicked -= OnCollapsedMetadataChipClicked;
            group.CollapsedChip.Dispose();
            group.CollapsedChip = null;

            int index = startIndex;
            foreach (var chip in group.ValueChips)
            {
                FlowLayoutFilterChips.Controls.Add(chip);
                FlowLayoutFilterChips.Controls.SetChildIndex(chip, index++);
            }
        }

        /// <summary>
        /// Removes value chips from the flow panel (keeps them alive), creates a count chip
        /// and inserts it at insertIndex.
        /// </summary>
        private void CollapseGroup(MetadataPropertyGroup group, int insertIndex)
        {
            foreach (var chip in group.ValueChips)
                FlowLayoutFilterChips.Controls.Remove(chip);

            var collapsedChip = FilterChipControl.FromMetadataFilter(group.Filter);
            collapsedChip.IsCollapsed = true;
            collapsedChip.ChipClicked += OnCollapsedMetadataChipClicked;
            group.CollapsedChip = collapsedChip;

            FlowLayoutFilterChips.Controls.Add(collapsedChip);
            FlowLayoutFilterChips.Controls.SetChildIndex(collapsedChip, insertIndex);
        }

        /// <summary>
        /// Removes the collapsed chip and re-inserts value chips at insertIndex.
        /// </summary>
        private void ExpandGroup(MetadataPropertyGroup group, int insertIndex)
        {
            if (!group.IsCollapsed) return;

            FlowLayoutFilterChips.Controls.Remove(group.CollapsedChip);
            group.CollapsedChip.ChipClicked -= OnCollapsedMetadataChipClicked;
            group.CollapsedChip.Dispose();
            group.CollapsedChip = null;

            int index = insertIndex;
            foreach (var chip in group.ValueChips)
            {
                FlowLayoutFilterChips.Controls.Add(chip);
                FlowLayoutFilterChips.Controls.SetChildIndex(chip, index++);
            }
        }
        #endregion

        #region Chip disposal

        private void DisposeAllChips()
        {
            foreach (var group in _propertyGroups.Values)
                DisposePropertyGroup(group);
            _propertyGroups.Clear();

            foreach (Control control in FlowLayoutFilterChips.Controls)
                control.Dispose();
            FlowLayoutFilterChips.Controls.Clear();

            _errorChip = null;
            _rangeBeginChip = null;
            _rangeEndChip = null;
        }

        private void DisposePropertyGroup(MetadataPropertyGroup group)
        {
            foreach (var chip in group.ValueChips)
            {
                chip.ChipClicked -= OnMetadataChipClicked;
                FlowLayoutFilterChips.Controls.Remove(chip);
                chip.Dispose();
            }
            group.ValueChips.Clear();

            if (group.CollapsedChip != null)
            {
                group.CollapsedChip.ChipClicked -= OnCollapsedMetadataChipClicked;
                FlowLayoutFilterChips.Controls.Remove(group.CollapsedChip);
                group.CollapsedChip.Dispose();
                group.CollapsedChip = null;
            }
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

                int labelY = Math.Max(0, (ChipHeight - LblCount.Height) / 2);
                LblCount.Location = new Point(Width - LblCount.Width, labelY);

                int flowWidth = LblCount.Text.Length > 0
                    ? Math.Max(20, LblCount.Left - CountLabelLeftGap)
                    : Math.Max(20, Width);

                FlowLayoutFilterChips.Location = new Point(0, 0);
                FlowLayoutFilterChips.Width = flowWidth;

                ApplyCollapseToFit(flowWidth);

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
        /// Expands all groups, then collapses them right-to-left until the total width fits.
        /// Each group is either fully expanded or fully collapsed — no partial state.
        /// </summary>
        private void ApplyCollapseToFit(int availableWidth)
        {
            var orderedGroups = _metadataFilters
                .Select(f => _propertyGroups.TryGetValue(f.Property, out var g) ? g : null)
                .Where(g => g != null)
                .ToList();

            // Expand all groups to a known baseline.
            int expandIndex = GetMetadataInsertBaseIndex();
            foreach (var group in orderedGroups)
            {
                ExpandGroup(group, expandIndex);
                expandIndex += group.ValueChips.Count;
            }

            int totalWidth = CalculateTotalChipWidth();
            if (totalWidth <= availableWidth)
                return;

            // Collapse groups right-to-left until it fits.
            for (int i = orderedGroups.Count - 1; i >= 0 && totalWidth > availableWidth; i--)
            {
                var group = orderedGroups[i];

                int expandedWidth = group.TotalExpandedWidth;
                int collapsedWidth = MeasureCollapsedGroupWidth(group.Filter);
                int saving = expandedWidth - collapsedWidth;

                // Even if saving <= 0 we still collapse — the group is taking up space
                // and collapsing is the only tool we have. Skip only when sizes are identical.
                if (saving == 0) continue;

                int collapseIndex = GetGroupFlowIndex(group, orderedGroups);
                CollapseGroup(group, collapseIndex);
                totalWidth -= saving;
            }
        }

        /// <summary>
        /// Returns the flow panel index of the first chip belonging to targetGroup,
        /// accounting for the current collapsed/expanded state of all preceding groups.
        /// </summary>
        private int GetGroupFlowIndex(MetadataPropertyGroup targetGroup, List<MetadataPropertyGroup> orderedGroups)
        {
            int index = GetMetadataInsertBaseIndex();
            foreach (var group in orderedGroups)
            {
                if (group == targetGroup) return index;
                index += group.IsCollapsed ? 1 : group.ValueChips.Count;
            }
            return index;
        }

        /// <summary>
        /// Measures the width a collapsed chip for this filter would occupy (including margin),
        /// using a temporary chip that is immediately disposed.
        /// </summary>
        private static int MeasureCollapsedGroupWidth(LogMetadataFilter filter)
        {
            using var probe = FilterChipControl.FromMetadataFilter(filter);
            probe.IsCollapsed = true;
            return probe.CollapsedWidth + probe.Margin.Horizontal;
        }

        private int GetMetadataInsertBaseIndex()
        {
            int index = 0;
            if (_errorChip != null)
                index = FlowLayoutFilterChips.Controls.IndexOf(_errorChip) + 1;
            if (_rangeBeginChip != null)
                index = FlowLayoutFilterChips.Controls.IndexOf(_rangeBeginChip) + 1;
            if (_rangeEndChip != null)
                index = FlowLayoutFilterChips.Controls.IndexOf(_rangeEndChip) + 1;
            return index;
        }

        private int CalculateTotalChipWidth()
        {
            int total = 0;
            foreach (Control chip in FlowLayoutFilterChips.Controls)
                total += chip.Width + chip.Margin.Horizontal;
            return total;
        }

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

        /// <summary>
        /// Fired by a per-value chip. Passes the specific value so the caller can
        /// remove exactly that value from the filter.
        /// </summary>
        private void OnMetadataChipClicked(object sender, EventArgs e)
        {
            if (sender is not FilterChipControl chip) return;
            FilterRemoved?.Invoke(this, new FilterRemovedEventArgs(chip.MetadataFilter.Property, chip.SpecificValue));
        }

        /// <summary>
        /// Fired by a collapsed count chip. Passes null value so the caller removes
        /// the entire property filter.
        /// </summary>
        private void OnCollapsedMetadataChipClicked(object sender, EventArgs e)
        {
            if (sender is not FilterChipControl chip) return;
            FilterRemoved?.Invoke(this, new FilterRemovedEventArgs(chip.MetadataFilter.Property, null));
        }
        #endregion
    }
}