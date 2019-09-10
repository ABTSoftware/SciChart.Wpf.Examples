using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SciChart.Drawing.Common;
using SciChart.Drawing.DirectX.Context.D3D10;
using SciChart.Drawing.HighQualityRasterizer;
using SciChart.Drawing.HighSpeedRasterizer;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class RendererTypeToDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
            {
                Type renderType = (Type)parameter;

                return renderType;
            }
            return null;
        }
    }
}
