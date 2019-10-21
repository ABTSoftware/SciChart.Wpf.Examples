using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class LinearToLogarithmicValueConverter: IValueConverter
    {
        public double LogarithmicBase { get; set; } = 2;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IConvertible convertible)
            {
                return Math.Pow(LogarithmicBase, convertible.ToDouble(CultureInfo.InvariantCulture));
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IConvertible convertible)
            {
                return Math.Log(convertible.ToDouble(CultureInfo.InvariantCulture), LogarithmicBase);
            }

            return value;
        }
    }
}
