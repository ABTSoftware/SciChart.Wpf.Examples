using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LegendAxisVisibilityCheckboxExample
{
    public class TwoWayBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                return (Visibility)value == Visibility.Visible ? true : false;
            }

            if (value is bool)
            {
                return (bool)value == true ? Visibility.Visible : Visibility.Collapsed;
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                return (Visibility)value == Visibility.Visible ? true : false;
            }

            if (value is bool)
            {
                return (bool)value == true ? Visibility.Visible : Visibility.Collapsed;
            }

            throw new NotImplementedException();
        }
    }
}