// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// WeekDaysAxisCalendar.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Charting.Numerics.Calendars;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.GanttChart
{
    /// <summary>
    /// A discontinuous calendar that skips weekends, so the X-axis only displays working days.
    /// Used with <see cref="SciChart.Charting.Visuals.Axes.DiscontinuousAxis.DoubleScaleDiscontinuousDateTimeAxis"/>
    /// to compress Saturday and Sunday out of the timeline.
    /// </summary>
    public class WeekDaysAxisCalendar : DiscontinuousDateTimeCalendarBase
    {
        public WeekDaysAxisCalendar()
        {
            SkipDaysInWeek.Add(DayOfWeek.Saturday);
            SkipDaysInWeek.Add(DayOfWeek.Sunday);
        }
    }
}