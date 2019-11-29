// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ThresholdBackgroundConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using System.Windows.Media;

namespace SciChart.Examples.Examples.InspectDatapoints
{
    public class ThresholdBackgroundConverter : IValueConverter
    {
        public SolidColorBrush DefaultBrush { get; set; }
        public SolidColorBrush HighThresholdBrush { get; set; }
        public SolidColorBrush LowThresholdBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vm = value as TooltipViewModel;
            if (vm == null) return null;

            var brush = DefaultBrush;

            if (vm.YValue > vm.HighThreshold) brush = HighThresholdBrush;
            if (vm.YValue < vm.LowThreshold) brush = LowThresholdBrush;

            var opacityStr = parameter as String;
            if (opacityStr != null)
            {
                double opacity;
                Double.TryParse(opacityStr, NumberStyles.Number, CultureInfo.InvariantCulture, out opacity);

                brush = brush.Clone();
                brush.Opacity = opacity;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
