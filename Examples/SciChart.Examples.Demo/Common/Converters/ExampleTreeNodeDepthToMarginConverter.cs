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