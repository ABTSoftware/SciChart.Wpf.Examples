using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Oscilloscope
{
    class MyTestConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
