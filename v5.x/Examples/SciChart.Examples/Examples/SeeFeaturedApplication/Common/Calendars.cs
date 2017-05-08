using System;
using System.Collections.ObjectModel;
using SciChart.Charting.Numerics.Calendars;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Common
{
    /// <summary>
    /// Example of how to make a Discontinuous DateTime Calender for the New York Stock Exchange
    /// 
    /// If you wish to extend this, ensure that public holidays are set for the year(s) which you wish to show data
    /// e.g. https://www.redcort.com/us-federal-bank-holidays/
    /// </summary>
    public class NYSECalendar : DiscontinuousDateTimeCalendarBase
    {
        public sealed override ObservableCollection<TimeSpanRange> SkipDayTimeRange { get; set; }
        public sealed override ObservableCollection<DayOfWeek> SkipDaysInWeek { get; set; }
        public sealed override ObservableCollection<DateTime> SkipDates { get; set; }

        public NYSECalendar()
        {
            SkipDayTimeRange = new ObservableCollection<TimeSpanRange>
            {
                // NYSE is open at 9:30 am EST
                new TimeSpanRange(new TimeSpan(0, 0, 0), new TimeSpan(9, 30, 0)),
                 // NYSE is closed at 16:00 pm EST
                new TimeSpanRange(new TimeSpan(16, 0, 0), new TimeSpan(24, 0, 0))
            };

            SkipDaysInWeek = new ObservableCollection<DayOfWeek>
            {
                // NYSE is closed on weekends
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };

            SkipDates = new ObservableCollection<DateTime>
            {
                new DateTime(2015, 12, 25), // NYSE Closed on Christmas Day 2015
                new DateTime(2016, 1, 1),   // NYSE Closed on New years day 2016
                new DateTime(2016, 1, 15),  // NYSE Clsoed on Martin Luther King Day 2016
                new DateTime(2016, 11, 24)  // NYSE Closed on Thanksgiving  2016
            };
        }
    }

    /// <summary>
    /// Example of how to make a Discontinuous DateTime Calender for the London Stock Exchange
    /// 
    /// If you wish to extend this, ensure that public holidays are set for the year(s) which you wish to show data
    /// e.g. http://www.lseg.com/areas-expertise/our-markets/london-stock-exchange/equities-markets/trading-services/business-days
    /// </summary>
    public class LSECalendar : DiscontinuousDateTimeCalendarBase
    {
        public sealed override ObservableCollection<TimeSpanRange> SkipDayTimeRange { get; set; }
        public sealed override ObservableCollection<DayOfWeek> SkipDaysInWeek { get; set; }
        public sealed override ObservableCollection<DateTime> SkipDates { get; set; }

        public LSECalendar()
        {
            SkipDayTimeRange = new ObservableCollection<TimeSpanRange>
            {
                // LSE is open at 08:00am GMT
                new TimeSpanRange(new TimeSpan(0, 0, 0), new TimeSpan(8, 0, 0)),
                // LSE is closed at 16:30pm GMT
                new TimeSpanRange(new TimeSpan(16, 30, 0), new TimeSpan(24, 0, 0))
            };

            SkipDaysInWeek = new ObservableCollection<DayOfWeek>
            {
                // LSE is closed on weekends
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };

            SkipDates = new ObservableCollection<DateTime>
            {
                new DateTime(2015, 12, 25), // LSE Closed on Christmas Day 2015
                new DateTime(2016, 1, 1)  // LSE Closed on New Years day 2016
            };
        }
    }
}