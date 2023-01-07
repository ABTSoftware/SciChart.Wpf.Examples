using System;
using System.Globalization;
using System.Windows.Data;
using SciChart.Core.Extensions;

namespace OffScreenExportExample
{
    public class TextToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueString = value.ToString();
            return !valueString.IsEmpty();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
