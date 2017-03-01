using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Examples.Demo.Behaviors;
using SciChart.Examples.Demo.Common;

namespace SciChart.Examples.Demo.Controls.WatermarkedAutocomplete
{
    [TemplatePart(Name = "PART_SearchIcon", Type=typeof(FrameworkElement))]
    [TemplatePart(Name = "Text", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Watermark", Type = typeof(TextBlock))]
    public class WatermarkedAutocomplete : AutoCompleteBoxCompatible
    {
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(WatermarkedAutocomplete), new PropertyMetadata("Watermark"));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }


        public WatermarkedAutocomplete()
        {
            DefaultStyleKey = typeof (WatermarkedAutocomplete);
        }
    }
}
