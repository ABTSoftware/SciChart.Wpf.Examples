using System.Windows.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.SciTrader
{
    public class IsSameObjectsMultiConverter : IMultiValueConverter
    {

        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
         {
            return values.Length == 2 
                && values[0].Equals(values[1]);
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
