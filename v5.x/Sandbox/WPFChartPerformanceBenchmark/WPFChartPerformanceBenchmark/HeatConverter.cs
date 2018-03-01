using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFChartPerformanceBenchmark
{
    public class HeatConverter : IValueConverter
    {
        private readonly HeatProvider _heatProvider;

        public HeatConverter(HeatProvider heatProvider)
        {
            _heatProvider = heatProvider;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double fps = (double) value;
            var brush = _heatProvider.GetHeat(fps);
            brush.Opacity = 0.5;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}