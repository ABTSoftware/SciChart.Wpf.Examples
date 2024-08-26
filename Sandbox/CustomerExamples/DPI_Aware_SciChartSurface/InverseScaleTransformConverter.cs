using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DpiAware_SciChartSurface
{
    [ValueConversion(typeof(ScaleTransform), typeof(ScaleTransform))]
    public class InverseScaleTransformConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ScaleTransform chartScale)
            {
                return new ScaleTransform(1d / chartScale.ScaleX, 1d / chartScale.ScaleY);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}