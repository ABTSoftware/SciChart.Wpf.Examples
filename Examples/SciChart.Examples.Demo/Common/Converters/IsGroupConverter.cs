using System;
using System.Globalization;
using System.Windows.Data;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class IsGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EverythingGroupViewModel)
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}