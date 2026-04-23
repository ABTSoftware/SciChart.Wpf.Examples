// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SnappingModeToVisibilityConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using SciChart.Charting.ChartModifiers;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class SnappingModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var snapStr = value as string;

            var visibility = Visibility.Collapsed;
            if (snapStr != null)
            {
                var snappingMode = (CursorSnappingMode)Enum.Parse(typeof(CursorSnappingMode), snapStr);

                visibility = snappingMode == CursorSnappingMode.TooltipToCrosshair
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
