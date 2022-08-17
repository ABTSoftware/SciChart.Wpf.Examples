using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common
{
    public class PointCountToLabelConverter : IValueConverter
    {
        private const double _1B = 1_000_000_000;
        private const double _1M = 1_000_000;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long pointCount = value is int intvalue ? (long)intvalue : (long)value;

            if (pointCount >= _1B)
            {
                return string.Format("{0:#0.##}B", pointCount / _1B);
            }
            return string.Format("{0:#0.##}M", pointCount / _1M);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}