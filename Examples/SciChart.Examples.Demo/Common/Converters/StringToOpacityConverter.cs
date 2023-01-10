using System;
using System.Windows.Data;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class StringToOpacityConverter : IValueConverter
    {
        public bool InverseOpacityValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (InverseOpacityValue)
            { 
                return string.IsNullOrEmpty(value as string) ? 1d : 0d;
            }
            return string.IsNullOrEmpty(value as string) ? 0d : 1d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}