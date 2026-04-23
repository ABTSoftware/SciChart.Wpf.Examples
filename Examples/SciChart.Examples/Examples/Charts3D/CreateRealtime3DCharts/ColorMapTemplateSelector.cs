// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ColorMapTemplateSelector.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D.RenderableSeries;

namespace SciChart.Examples.Examples.Charts3D.CreateRealtime3DCharts
{
    public class ColorMapTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BrushDataTemplate { get; set; }

        public DataTemplate ImageBrushDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var dataTemplate = base.SelectTemplate(item, container);

            if (item is BrushColorPalette brush)
            {
                return brush.Tag?.ToString() == "ImageBrush" ? ImageBrushDataTemplate : BrushDataTemplate;
            }

            return dataTemplate;
        }
    }
}