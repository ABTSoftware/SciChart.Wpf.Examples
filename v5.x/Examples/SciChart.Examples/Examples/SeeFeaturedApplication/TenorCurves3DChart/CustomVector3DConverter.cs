// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomVector3DConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.TenorCurves3DChart
{
    public class CustomVector3DConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Vector3D vector && parameter is string p)
            {
                switch (p)
                {
                    case "X": return vector.X.ToString("N2");

                    case "Y": return vector.Y.ToString("N2");

                    case "Z": return new DateTime((long)vector.Z).ToString("d");

                    default: return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}