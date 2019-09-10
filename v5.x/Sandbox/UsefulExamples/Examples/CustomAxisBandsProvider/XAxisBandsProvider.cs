using System;
using System.Windows.Media;
using SciChart.Charting.Visuals.Axes.AxisBandProviders;
using SciChart.Data.Model;

namespace CustomAxisBandsProvider
{
    public static class ColorProvider
    {
        public static Color AxisBandColor = (Color)ColorConverter.ConvertFromString("#B4642123");
    }

    public class XAxisBandsProvider : DateTimeAxisBandsProvider
    {
        Random _rand = new Random();

        public XAxisBandsProvider()
        {
            var count = _rand.Next(5, 10);
            var incr = (VisibleRange.Max - VisibleRange.Min).Days / count;

            for (var start = VisibleRange.Min; start < VisibleRange.Max; start = start.AddDays(incr))
            {
                var bandRange = new DateRange(start, start.AddDays(_rand.Next(incr / 2)));

                var bandInfo = new AxisBandInfo<DateRange>(bandRange, ColorProvider.AxisBandColor);
                AxisBands.Add(bandInfo);
            }
        }

        public DateRange VisibleRange => new DateRange(new DateTime(2017, 9, 1), new DateTime(2018, 9, 1));
    }

    public class YAxisBandsProvider : NumericAxisBandsProvider
    {
        Random _rand = new Random();

        public YAxisBandsProvider()
        {
            var count = _rand.Next(5, 10);
            var incr = VisibleRange.Diff / count;

            for (var start = VisibleRange.Min; start < VisibleRange.Max; start += incr)
            {
                var bandRange = new DoubleRange(start, start + _rand.Next((int)incr / 2));

                var bandInfo = new AxisBandInfo<DoubleRange>(bandRange, ColorProvider.AxisBandColor);
                AxisBands.Add(bandInfo);
            }
        }

        public DoubleRange VisibleRange => new DoubleRange(-100, 100);
    }
}