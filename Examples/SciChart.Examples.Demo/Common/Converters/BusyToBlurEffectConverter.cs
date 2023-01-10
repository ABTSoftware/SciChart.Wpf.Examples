using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Effects;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class BusyToBlurEffectConverter : IValueConverter
    {
        private readonly BlurEffect _blurEffect = new BlurEffect();

        public double BlurRadius { get; set; } = 5;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isBusy)
            {
                if (_blurEffect.Radius != BlurRadius && BlurRadius >= 0)
                {
                    _blurEffect.Radius = BlurRadius;
                }
                return isBusy ? _blurEffect : null;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}