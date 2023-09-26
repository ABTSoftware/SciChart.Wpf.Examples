using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SciChart.Drawing.VisualXcceleratorRasterizer;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Common.Converters;

internal class RendererSettingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        object selectedRenderer = DependencyProperty.UnsetValue;
        if (value is SettingsViewModel settingViewModel)
        {
            var renderer = Activator.CreateInstance(settingViewModel.SelectedRenderer);
            if (renderer is VisualXcceleratorRenderSurface vxRenderSurface)
            {
                vxRenderSurface.UseAlternativeFillSource = settingViewModel.UseAlternativeFillSourceD3D;
            }

            selectedRenderer = renderer ?? DependencyProperty.UnsetValue;
        }

        return selectedRenderer;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}