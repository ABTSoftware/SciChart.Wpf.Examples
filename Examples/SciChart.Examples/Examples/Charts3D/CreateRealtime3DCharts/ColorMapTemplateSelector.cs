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