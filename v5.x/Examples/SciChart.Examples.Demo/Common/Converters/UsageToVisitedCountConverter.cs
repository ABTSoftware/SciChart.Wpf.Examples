using System;
using System.Globalization;
using System.Windows.Data;
using SciChart.Wpf.UI.Reactive.Services;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class UsageToVisitedCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rating = (ExampleRating)value;
            return rating.Popularity;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}