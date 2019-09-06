using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class ThicknessToNegativeThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var thickness = (Thickness)value;

            thickness.Bottom = -thickness.Bottom;
            thickness.Top = -thickness.Top;
            thickness.Left = -thickness.Left;
            thickness.Right = -thickness.Right;

            return thickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}