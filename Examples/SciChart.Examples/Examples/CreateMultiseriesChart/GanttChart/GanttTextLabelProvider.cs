// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// GanttTextLabelProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.GanttChart
{
    /// <summary>
    /// Provides the text label drawn inside each Gantt bar, showing the task duration in working days.
    /// Implements <see cref="IPointLabelProvider"/> so SciChart calls it during series rendering.
    /// </summary>
    public class GanttTextLabelProvider : IPointLabelProvider
    {
        private readonly GanttItemViewModel _item;

        public GanttTextLabelProvider(GanttItemViewModel item)
        {
            _item = item;
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries) { }

        /// <summary>
        /// Returns the working-day count formatted as "Nd" (e.g. "42d") for the associated task.
        /// </summary>
        public string GetLabelText(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            var days = CountWeekWorkDays(_item.Start, _item.End);

            return $"{days}d";
        }

        /// <summary>
        /// Counts the number of working (Mon–Fri) days between <paramref name="start"/> and
        /// <paramref name="end"/> inclusive, by handling full 7-day weeks and the partial week
        /// separately.
        /// </summary>
        private int CountWeekWorkDays(DateTime start, DateTime end)
        {
            var days = (end - start).Days + 1;

            return WorkDaysInFullWeek(days) + WorkDaysInPartialWeek(start.DayOfWeek, days);
        }

        /// <summary>
        /// Returns working days contributed by every complete 7-day week in the span (5 per week).
        /// </summary>
        private int WorkDaysInFullWeek(int totalDays)
        {
            return totalDays / 7 * 5;
        }

        /// <summary>
        /// Returns the working days in the leftover partial week (0–6 days) that starts on
        /// <paramref name="firstDay"/>. Weekend days that fall inside the remainder are excluded.
        /// </summary>
        private int WorkDaysInPartialWeek(DayOfWeek firstDay, int totalDays)
        {
            var remainingDays = totalDays % 7;
            var daysToSaturday = (int)DayOfWeek.Saturday - (int)firstDay;

            // Remainder ends before reaching Saturday — all remaining days are working days.
            if (remainingDays <= daysToSaturday)
                return remainingDays;

            // Remainder reaches Saturday or Sunday but not the following Monday — cap at Saturday.
            if (remainingDays <= daysToSaturday + 2)
                return daysToSaturday;

            // Remainder extends past Sunday — subtract the two weekend days.
            return remainingDays - 2;
        }
    }
}