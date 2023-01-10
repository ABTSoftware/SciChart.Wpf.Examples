using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Demo.Behaviors
{
    public class AutocompleteHelper
    {
        public static readonly DependencyProperty WatermarkContentProperty = DependencyProperty.RegisterAttached
            ("WatermarkContent", typeof(object), typeof(AutocompleteHelper), new PropertyMetadata(default(object)));

        public static void SetWatermarkContent(DependencyObject element, object value)
        {
            element.SetValue(WatermarkContentProperty, value);
        }

        public static object GetWatermarkContent(DependencyObject element)
        {
            return element.GetValue(WatermarkContentProperty);
        }

        public static readonly DependencyProperty WatermarkContentTemplateProperty = DependencyProperty.RegisterAttached
            ("WatermarkContentTemplate", typeof(DataTemplate), typeof(AutocompleteHelper), new PropertyMetadata(default(DataTemplate)));

        public static void SetWatermarkContentTemplate(DependencyObject element, DataTemplate value)
        {
            element.SetValue(WatermarkContentTemplateProperty, value);
        }

        public static DataTemplate GetWatermarkContentTemplate(DependencyObject element)
        {
            return (DataTemplate)element.GetValue(WatermarkContentTemplateProperty);
        }

        public static readonly DependencyProperty LoseFocusOnEscapeProperty = DependencyProperty.RegisterAttached
            ("LoseFocusOnEscape", typeof(bool), typeof(AutocompleteHelper), new PropertyMetadata(default(bool), OnLoseFocusOnEscapeChanged));

        public static void SetLoseFocusOnEscape(DependencyObject element, bool value)
        {
            element.SetValue(LoseFocusOnEscapeProperty, value);
        }

        public static bool GetLoseFocusOnEscape(DependencyObject element)
        {
            return (bool)element.GetValue(LoseFocusOnEscapeProperty);
        }

        private static void OnLoseFocusOnEscapeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox && (bool)e.NewValue)
            {
                textBox.KeyDown += (s, arg) =>
                {
                    if (arg.Key == Key.Escape)
                    {
                        var parent = textBox.Parent as UIElement;
                        if (parent != null)
                        {
                            var parentControl = parent.FindVisualParent<Control>();
                            if (parentControl != null)
                            {
                                parentControl.Focus();
                            }
                        }
                    }
                };
            }
        }
    }
}
