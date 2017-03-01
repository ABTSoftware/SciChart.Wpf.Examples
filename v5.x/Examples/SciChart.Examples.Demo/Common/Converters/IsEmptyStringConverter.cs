using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class IsEmptyStringConverter : IValueConverter
    {
        private const string InvertionFlag = "INVERSE";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringParam = parameter as string;
            var inverse = string.Equals(stringParam, InvertionFlag, StringComparison.InvariantCultureIgnoreCase);

            var onTrue= inverse ? Visibility.Visible : Visibility.Collapsed;
            var onFalse = inverse ? Visibility.Collapsed : Visibility.Visible;

            return string.IsNullOrEmpty(value as string) ? onTrue : onFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}