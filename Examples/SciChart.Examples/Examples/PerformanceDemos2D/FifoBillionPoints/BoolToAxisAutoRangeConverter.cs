using SciChart.Charting.Visuals.Axes;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class BoolToAxisAutoRangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isAutoRange = value as bool?;
            return isAutoRange.GetValueOrDefault(false) ? AutoRange.Always : AutoRange.Once;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
