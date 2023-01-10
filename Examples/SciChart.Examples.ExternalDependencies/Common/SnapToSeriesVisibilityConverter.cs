using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SciChart.Charting.ChartModifiers;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class SnapToSeriesVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length >= 2 && values[0] is CursorSnappingMode snappingMode && values[1] is bool hasSeriesNames)
            {
                return hasSeriesNames && snappingMode != CursorSnappingMode.TooltipToCrosshair ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}