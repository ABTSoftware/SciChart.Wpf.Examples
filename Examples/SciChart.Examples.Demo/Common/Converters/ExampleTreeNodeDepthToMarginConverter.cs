// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ExampleTreeNodeDepthToMarginConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class ExampleTreeNodeDepthToMarginConverter : IValueConverter
    {
        public double DepthOffset { get; set; } = 10;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ExampleTreeNodeViewModel nodeViewModel)
            {
                var groupName = nodeViewModel.GroupName;

                if (!string.IsNullOrEmpty(groupName))
                {
                    if (nodeViewModel.Example != null)
                    {
                        return new Thickness(DepthOffset * 2, 0, 0, 0);
                    }
                    return new Thickness(DepthOffset, 0, 0, 0);
                }
            }
            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}