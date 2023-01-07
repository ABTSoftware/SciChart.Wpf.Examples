// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AnnotationGetXCoordinateConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting.Visuals.Annotations;

namespace SciChart.Examples.Examples.CreateRealtimeChart.RealTimeStaticAxis
{
    public class AnnotationGetXCoordinateConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var annotation = value as AnnotationBase;

            var xAxis = annotation.XAxis;
            var result = annotation.X1;

            if (xAxis != null && result != null)
            {
                var xCoordCalculator = xAxis.GetCurrentCoordinateCalculator();

                var position = xCoordCalculator == null ? (double)result : xCoordCalculator.GetCoordinate((double)result);
                position = Math.Round(position);

                result = position.ToString(culture) + " px";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
