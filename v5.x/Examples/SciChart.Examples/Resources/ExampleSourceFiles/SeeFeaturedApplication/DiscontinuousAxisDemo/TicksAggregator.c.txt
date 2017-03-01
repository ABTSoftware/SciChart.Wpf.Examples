// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2016. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// TicksAggregator.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using System.Collections.Generic;
using SciChart.Charting.Numerics.Calendars;
using SciChart.Charting.Visuals.Axes.DiscontinuousAxis;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.DiscontinuousAxisDemo
{
    public static class TicksAggregator
    {
        public static PriceSeries GetPriceDataByTimeFrame(List<Tick> ticks, int timeFrame, IDiscontinuousDateTimeCalendar calendar, out Tuple<DateTime, double> min, out Tuple<DateTime, double> max)
        {
            var priceSeries = new PriceSeries();
            var dateTime = ticks[0].DateTime;
            min = null;
            max = null;

            double open = 0;
            double high = 0;
            double low = 0;
            double close = 0;
            long volume = 0;

            bool hasTicks = false;

            for (int i = 0; i < ticks.Count; i++)
            {
                if (ticks[i].DateTime < dateTime + TimeSpan.FromMinutes(timeFrame))
                {
                    if (!calendar.IsValueInGap(ticks[i].DateTime))
                    {
                        if (!hasTicks)
                        {
                            high = ticks[i].High;
                            low = ticks[i].Low;
                            open = ticks[i].Open;
                            hasTicks = true;
                        }
                        else
                        {
                            high = high < ticks[i].High ? ticks[i].High : high;
                            low = low > ticks[i].Low ? ticks[i].Low : low;
                        }

                        close = ticks[i].Close;
                        volume += ticks[i].Volume;
                    }
                }
                else
                {
                    if (hasTicks)
                    {
                        var priceBar = new PriceBar
                        {
                            DateTime = dateTime, Open = open, High = high, Low = low, Close = close, Volume = volume
                        };

                        priceSeries.Add(priceBar);

                        if (priceSeries.Count == 1)
                        {
                            min = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.Low);
                            max = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.High);
                        }
                        else
                        {
                            if (priceBar.High > max.Item2)
                                max = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.High);
                            if (priceBar.Low < min.Item2)
                                min = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.Low);
                        }

                        hasTicks = false;
                    }
                    else
                    {
                        dateTime = dateTime + TimeSpan.FromMinutes(timeFrame);
                    }
                }
            }


            return priceSeries;
        }

        public static PriceSeries GetPriceDataByVolume(List<Tick> ticks, int volumeCount, IDiscontinuousDateTimeCalendar calendar, double barTimeFrame, out Tuple<DateTime, double> min, out Tuple<DateTime, double> max)
        {
            var priceSeries = new PriceSeries();
            var dateTime = new DateTime();
            min = null;
            max = null;

            double open = 0;
            double high = 0;
            double low = 0;
            double close = 0;
            long volume = 0;

            bool hasTicks = false;

            for (int i = 0; i < ticks.Count; i++)
            {
                if (volume < volumeCount)
                {
                    if (!calendar.IsValueInGap(ticks[i].DateTime))
                    {
                        if (!hasTicks)
                        {
                            high = ticks[i].High;
                            low = ticks[i].Low;
                            open = ticks[i].Open;

                            if (priceSeries.Count == 0)
                                dateTime = ticks[i].DateTime;
                            hasTicks = true;
                        }
                        else
                        {
                            high = high < ticks[i].High ? ticks[i].High : high;
                            low = low > ticks[i].Low ? ticks[i].Low : low;
                        }

                        close = ticks[i].Close;
                        volume += ticks[i].Volume;
                    }
                }
                else if (hasTicks)
                {
                    var priceBar = new PriceBar
                    {
                        DateTime = calendar.GetValueByOffset(dateTime, priceSeries.Count * barTimeFrame), Open = open, High = high, Low = low, Close = close, Volume = volume
                    };

                    priceSeries.Add(priceBar);

                    if (priceSeries.Count == 1)
                    {
                        min = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.Low);
                        max = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.High);
                    }
                    else
                    {
                        if (priceBar.High > max.Item2)
                            max = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.High);
                        if (priceBar.Low < min.Item2)
                            min = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.Low);
                    }

                    volume = 0;
                    hasTicks = false;
                }
            }

            return priceSeries;
        }

        public static PriceSeries GetPriceDataByRange(List<Tick> ticks, double rangeCount, IDiscontinuousDateTimeCalendar calendar, double barTimeFrame, out Tuple<DateTime, double> min, out Tuple<DateTime, double> max)
        {
            var priceSeries = new PriceSeries();
            var dateTime = new DateTime();
            min = null;
            max = null;

            double open = 0;
            double high = 0;
            double low = 0;
            double close = 0;
            long volume = 0;
            double range = 0;

            bool hasTicks = false;

            for (int i = 0; i < ticks.Count; i++)
            {
                if (Math.Abs(range) < rangeCount)
                {
                    if (!calendar.IsValueInGap(ticks[i].DateTime))
                    {
                        if (!hasTicks)
                        {
                            high = ticks[i].High;
                            low = ticks[i].Low;
                            open = ticks[i].Open;

                            if (priceSeries.Count == 0)
                                dateTime = ticks[i].DateTime;
                            hasTicks = true;
                        }
                        else
                        {
                            high = high < ticks[i].High ? ticks[i].High : high;
                            low = low > ticks[i].Low ? ticks[i].Low : low;
                        }

                        volume += ticks[i].Volume;
                        range = high - low;

                        close = ticks[i].Close > open ? low + rangeCount : high - rangeCount;
                    }
                }
                else if (hasTicks)
                {
                    if (range / rangeCount > 2)
                    {
                        for (int j = 0; j < range / rangeCount; j++)
                        {
                            var priceBar = new PriceBar
                            {
                                DateTime = calendar.GetValueByOffset(dateTime, priceSeries.Count * barTimeFrame),
                                Open = ticks[i].Close > open ? open + j * rangeCount : open - j * rangeCount,
                                High = ticks[i].Close > open ? open + j * rangeCount : open - j * rangeCount,
                                Low = ticks[i].Close > open ? open + j * rangeCount : open - (j + 1) * rangeCount,
                                Close = ticks[i].Close > open ? open + j * rangeCount : open - (j + 1) * rangeCount,
                                Volume = volume
                            };

                            priceSeries.Add(priceBar);

                            if (priceSeries.Count == 1)
                            {
                                min = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.Low);
                                max = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.High);
                            }
                            else
                            {
                                if (priceBar.High > max.Item2)
                                    max = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.High);
                                if (priceBar.Low < min.Item2)
                                    min = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.Low);
                            }
                        }
                    }
                    else
                    {
                        var priceBar = new PriceBar
                        {
                            DateTime = calendar.GetValueByOffset(dateTime, priceSeries.Count * barTimeFrame), Open = open, High = high, Low = low, Close = close, Volume = volume
                        };

                        priceSeries.Add(priceBar);

                        if (priceSeries.Count == 1)
                        {
                            min = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.Low);
                            max = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.High);
                        }
                        else
                        {
                            if (priceBar.High > max.Item2)
                                max = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.High);
                            if (priceBar.Low < min.Item2)
                                min = new Tuple<DateTime, double>(priceBar.DateTime, priceBar.Low);
                        }
                    }

                    range = 0;
                    hasTicks = false;
                }
            }

            return priceSeries;
        }
    }
}