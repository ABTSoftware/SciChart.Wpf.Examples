using System;
using System.Windows.Data;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class RendererTypeToDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
            {
                Type renderType = (Type)parameter;

                return renderType;
            }
            return null;
        }
    }
}
