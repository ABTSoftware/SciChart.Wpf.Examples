using System;
using System.Globalization;
using System.Windows.Data;
using SciChart.Charting.Visuals.Axes;

namespace SciChart.Sandbox.Examples.MarketProfileTradingExample
{
    public class BoolToAxisAutoRangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? AutoRange.Always : AutoRange.Once;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnumValueToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.GetName(value.GetType(), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
