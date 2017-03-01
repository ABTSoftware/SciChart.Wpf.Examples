using System;
using System.Collections.ObjectModel;
using SciChart.Charting.Numerics.Calendars;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Common
{
    public class NYSECalendar : DiscontinuousDateTimeCalendarBase
    {
        public sealed override ObservableCollection<TimeSpanRange> SkipDayTimeRange { get; set; }
        public sealed override ObservableCollection<DayOfWeek> SkipDaysInWeek { get; set; }
        public sealed override ObservableCollection<DateTime> SkipDaysInMonth { get; set; }
        public sealed override ObservableCollection<DateTime> SkipDaysInYear { get; set; }
        public sealed override ObservableCollection<DateTime> SkipDates { get; set; }

        public NYSECalendar()
        {
            SkipDayTimeRange = new ObservableCollection<TimeSpanRange>
            {
                new TimeSpanRange(new TimeSpan(0, 0, 0), new TimeSpan(9, 30, 0)),
                new TimeSpanRange(new TimeSpan(16, 0, 0), new TimeSpan(24, 0, 0))
            };

            SkipDaysInWeek = new ObservableCollection<DayOfWeek>
            {
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };

            SkipDates = new ObservableCollection<DateTime>
            {
                new DateTime(2015, 12, 25),
                new DateTime(2016, 1, 15),
                new DateTime(2016, 11, 24)
            };
        }
    }

    public class LSECalendar : DiscontinuousDateTimeCalendarBase
    {
        public sealed override ObservableCollection<TimeSpanRange> SkipDayTimeRange { get; set; }
        public sealed override ObservableCollection<DayOfWeek> SkipDaysInWeek { get; set; }
        public sealed override ObservableCollection<DateTime> SkipDaysInMonth { get; set; }
        public sealed override ObservableCollection<DateTime> SkipDaysInYear { get; set; }
        public sealed override ObservableCollection<DateTime> SkipDates { get; set; }

        public LSECalendar()
        {
            SkipDayTimeRange = new ObservableCollection<TimeSpanRange>
            {
                new TimeSpanRange(new TimeSpan(0, 0, 0), new TimeSpan(8, 0, 0)),
                new TimeSpanRange(new TimeSpan(16, 30, 0), new TimeSpan(24, 0, 0))
            };

            SkipDaysInWeek = new ObservableCollection<DayOfWeek>
            {
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };

            SkipDates = new ObservableCollection<DateTime>
            {
                new DateTime(2015, 12, 25),
                new DateTime(2016, 1, 15),
                new DateTime(2016, 11, 24)
            };
        }
    }
}