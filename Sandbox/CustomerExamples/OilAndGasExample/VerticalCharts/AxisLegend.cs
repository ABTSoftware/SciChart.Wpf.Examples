using System.Windows;

namespace OilAndGasExample.VerticalCharts
{
    public class AxisLegend
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached
            ("Content", typeof(object), typeof(AxisLegend), new PropertyMetadata(null));
        
        public static readonly DependencyProperty AxisVisibilityProperty = DependencyProperty.RegisterAttached
            ("AxisVisibility", typeof(Visibility), typeof(AxisLegend), new PropertyMetadata(Visibility.Visible));

        public static void SetContent(DependencyObject target, object value)
        {
            target.SetValue(ContentProperty, value);
        }

        public static object GetContent(DependencyObject target)
        {
            return target.GetValue(ContentProperty);
        }
        
        public static void SetAxisVisibility(DependencyObject target, Visibility value)
        {
            target.SetValue(AxisVisibilityProperty, value);
        }

        public static Visibility GetAxisVisibility(DependencyObject target)
        {
            return (Visibility) target.GetValue(AxisVisibilityProperty);
        }
    }
}