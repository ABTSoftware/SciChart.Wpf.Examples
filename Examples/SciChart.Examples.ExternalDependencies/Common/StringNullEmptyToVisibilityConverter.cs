using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class StringNullEmptyToVisibilityConverter : IValueConverter
    {
        public Visibility NullOrEmpty { get; set; }

        public Visibility NotNullOrEmpty { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? NullOrEmpty : NotNullOrEmpty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}