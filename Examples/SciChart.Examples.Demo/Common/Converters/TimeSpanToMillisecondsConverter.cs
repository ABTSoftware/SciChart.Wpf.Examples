using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class TimeSpanToMillisecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {         
            if (value is TimeSpan timeSpan)
            {
                return timeSpan.TotalMilliseconds;
            }
            return 0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double totalMilliseconds)
            {
                return TimeSpan.FromMilliseconds(totalMilliseconds);
            }
            return TimeSpan.Zero;
        }
    }
}