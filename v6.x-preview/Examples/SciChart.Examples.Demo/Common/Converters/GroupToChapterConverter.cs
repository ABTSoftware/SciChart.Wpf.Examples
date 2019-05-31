using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class GroupToChapterConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value.GetType();

            string result = "";
            if (type.Name == "ChartGroups")
            {
                result = "Charting";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}