// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// GanttItemViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows.Media;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.GanttChart
{
    /// <summary>
    /// View-model for a single Gantt task row. Holds the task metadata (name, department, color),
    /// the working-day-aligned start/end dates, and the current completion percentage.
    /// One <see cref="GanttItemViewModel"/> maps to one Y-axis and one stripe series in the chart.
    /// </summary>
    public class GanttItemViewModel : BaseViewModel
    {
        private bool _isCurrent;
        private double _completion;

        private Color _color;
        private DateTime _start, _end;

        public GanttItemViewModel(int itemId)
        {
            Id = itemId;
        }

        /// <summary>Unique identifier used to pair each item with its Y-axis and series.</summary>
        public int Id { get; }

        public string Name { get; set; }

        public string Department { get; set; }

        /// <summary>
        /// Task color. Setting it also raises <see cref="Fill"/> change notification so that
        /// the left-panel accent strip and the chart stripe bar stay in sync.
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
                OnPropertyChanged(nameof(Fill));
            }
        }

        /// <summary>Brush derived from <see cref="Color"/>, used by the chart stripe series fill.</summary>
        public Brush Fill => new SolidColorBrush(Color);

        /// <summary>
        /// Task start date, snapped forward to the nearest weekday so the bar aligns with
        /// the discontinuous (weekend-skipping) X-axis.
        /// </summary>
        public DateTime Start
        {
            get => _start;
            set
            {
                _start = WeekDayStartDate(value);
                OnPropertyChanged(nameof(Start));
            }
        }

        /// <summary>
        /// Task end date, snapped backward to the nearest weekday for the same reason as
        /// <see cref="Start"/>.
        /// </summary>
        public DateTime End
        {
            get => _end;
            set
            {
                _end = WeekDayEndDate(value);
                OnPropertyChanged(nameof(End));
            }
        }

        /// <summary>Completion percentage (0–100) shown in the left-panel list.</summary>
        public double Completion
        {
            get => _completion;
            set
            {
                _completion = value;
                OnPropertyChanged(nameof(Completion));
            }
        }

        /// <summary>
        /// <see langword="true"/> when the current-date marker falls within this task's range.
        /// Drives the colored accent strip in the left panel (grey when not current).
        /// </summary>
        public bool IsCurrent
        {
            get => _isCurrent;
            set
            {
                _isCurrent = value;
                OnPropertyChanged(nameof(IsCurrent));
            }
        }

        /// <summary>
        /// Recalculates <see cref="IsCurrent"/> and <see cref="Completion"/> based on
        /// where <paramref name="currentDate"/> falls relative to this task's date range.
        /// Called whenever the user drags the current-date marker on the chart.
        /// </summary>
        public void CheckCompletion(DateTime currentDate)
        {
            IsCurrent = currentDate >= Start && currentDate <= End;

            if (IsCurrent)
            {
                // Interpolate linearly between start and end ticks.
                Completion = (currentDate.Ticks - Start.Ticks) * 100 / (End.Ticks - Start.Ticks);
            }
            else
            {
                Completion = currentDate > End ? 100d : 0d;
            }
        }

        /// <summary>
        /// Advances a Saturday by 2 days or a Sunday by 1 day so that task starts land on a weekday.
        /// </summary>
        private DateTime WeekDayStartDate(DateTime startDate)
        {
            if (startDate.DayOfWeek == DayOfWeek.Saturday)
                return startDate.AddDays(2);

            if (startDate.DayOfWeek == DayOfWeek.Sunday)
                return startDate.AddDays(1);

            return startDate;
        }

        /// <summary>
        /// Retreats a Saturday by 1 day or a Sunday by 2 days so that task ends land on a weekday.
        /// </summary>
        private DateTime WeekDayEndDate(DateTime endDate)
        {
            if (endDate.DayOfWeek == DayOfWeek.Saturday)
                return endDate.AddDays(-1);

            if (endDate.DayOfWeek == DayOfWeek.Sunday)
                return endDate.AddDays(-2);

            return endDate;
        }
    }
}