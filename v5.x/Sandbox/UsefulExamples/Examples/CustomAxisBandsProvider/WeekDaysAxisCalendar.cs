using System;
using SciChart.Charting.Numerics.Calendars;

namespace CustomAxisBandsProvider
{
    public class WeekDaysAxisCalendar : DiscontinuousDateTimeCalendarBase
    {
        public WeekDaysAxisCalendar()
        {
            SkipDaysInWeek.Add(DayOfWeek.Saturday);
            SkipDaysInWeek.Add(DayOfWeek.Sunday);
        }
    }
}