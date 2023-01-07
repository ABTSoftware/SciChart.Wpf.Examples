﻿using System;
using SciChart.Charting.Numerics.Calendars;

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
        public NYSECalendar()
        {
            // For intraday data, you can add a skip range like this.
            // For daily data, skip ranges will cause the Daily OHLC bars with timestamp at 0:00:00 to be skipped 
            //SkipDayTimeRange.Add(new TimeSpanRange(new TimeSpan(0, 0, 0), new TimeSpan(9, 30, 0))); // NYSE is open at 9:30 am EST
            //SkipDayTimeRange.Add(new TimeSpanRange(new TimeSpan(16, 0, 0), new TimeSpan(24, 0, 0))); // NYSE is closed at 16:00 pm EST

            // NYSE is closed on weekends
            SkipDaysInWeek.Add(DayOfWeek.Saturday);
            SkipDaysInWeek.Add(DayOfWeek.Sunday);

            SkipDates.Add(new DateTime(2015, 12, 25)); // NYSE Closed on Christmas Day 2015
            SkipDates.Add(new DateTime(2016, 1, 1)); // NYSE Closed on New years day 2016
            SkipDates.Add(new DateTime(2016, 1, 15)); // NYSE Clsoed on Martin Luther King Day 2016
            SkipDates.Add(new DateTime(2016, 11, 24)); // NYSE Closed on Thanksgiving  2016
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
        public LSECalendar()
        {
            // For intraday data, you can add a skip range like this.
            // For daily data, skip ranges will cause the Daily OHLC bars with timestamp at 0:00:00 to be skipped 
            //SkipDayTimeRange.Add(new TimeSpanRange(new TimeSpan(0, 0, 0), new TimeSpan(8, 0, 0))); // LSE is open at 08:00am GMT
            //SkipDayTimeRange.Add(new TimeSpanRange(new TimeSpan(16, 30, 0), new TimeSpan(24, 0, 0))); // LSE is closed at 16:30pm GMT

            // LSE is closed on weekends
            SkipDaysInWeek.Add(DayOfWeek.Saturday);
            SkipDaysInWeek.Add(DayOfWeek.Sunday);

            SkipDates.Add(new DateTime(2015, 12, 25)); // LSE Closed on Christmas Day 2015
            SkipDates.Add(new DateTime(2016, 1, 1)); // LSE Closed on New Years day 2016
        }
    }
}