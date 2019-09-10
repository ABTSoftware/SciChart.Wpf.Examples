using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using SciChart.Charting.ChartModifiers;

namespace SciChart.Examples.Examples.CreateGaugeCharts
{
    public class LegendPlacementToOrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var placement = (LegendPlacement)value;
            Orientation result = Orientation.Vertical;

            if (placement == LegendPlacement.Bottom || placement == LegendPlacement.Top)
            {
                result = Orientation.Horizontal;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}