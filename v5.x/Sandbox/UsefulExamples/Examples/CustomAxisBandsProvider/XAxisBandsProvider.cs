using System;
using System.Windows.Media;
using SciChart.Charting.Visuals.Axes.AxisBandProviders;
using SciChart.Data.Model;

namespace CustomAxisBandsProvider
{
    public static class ColorProvider
    {
        public static Color[] PaletteColors = {Colors.Green, Colors.Gray, Colors.Blue, Colors.AliceBlue, Colors.Aqua};
    }

    public class XAxisBandsProvider : DateTimeAxisBandsProvider
    {
        public XAxisBandsProvider()
        {
            var rand = new Random();

            var startDataDate = new DateTime(2017, 9, 1, 12, 0, 0);
            var startBandsDate = startDataDate.AddYears(-1);
            for (int i = 0; i < 100; ++i)
            {
                var endDate = startBandsDate.AddDays(rand.Next(1, 100));

                var bandInfo =
                    new AxisBandInfo<DateRange>(new DateRange(startBandsDate, endDate),
                        ColorProvider.PaletteColors[i % ColorProvider.PaletteColors.Length]);
                AxisBands.Add(bandInfo);

                startBandsDate = endDate.AddDays(rand.Next(1, 100));
            }
        }
    }

    public class YAxisBandsProvider : NumericAxisBandsProvider
    {
        public YAxisBandsProvider()
        {
            var rand = new Random();

            var startRange = new DoubleRange(-100, -90);
            for (int i = 0; i < 100; ++i)
            {
                var bandInfo =
                    new AxisBandInfo<DoubleRange>(startRange,
                        ColorProvider.PaletteColors[i % ColorProvider.PaletteColors.Length]);
                AxisBands.Add(bandInfo);

                var min = startRange.Max + rand.Next(1, 100);
                var max = min + rand.Next(1, 100);
                startRange = new DoubleRange(min, max);
            }
        }
    }
}