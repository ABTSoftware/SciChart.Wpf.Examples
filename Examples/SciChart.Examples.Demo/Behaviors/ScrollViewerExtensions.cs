using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Demo.Behaviors
{
    public class ScrollViewerExtensions
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached
            ("CornerRadius", typeof(CornerRadius), typeof(ScrollViewerExtensions), new PropertyMetadata(new CornerRadius(0)));  

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius) element.GetValue(CornerRadiusProperty);
        }

        public static readonly DependencyProperty CanContentScrollProperty = DependencyProperty.RegisterAttached
            ("CanContentScroll", typeof(bool), typeof(ScrollViewerExtensions), new PropertyMetadata(true, OnCanContentScrollChanged));        

        public static void SetCanContentScroll(DependencyObject element, bool value)
        {
            element.SetValue(CanContentScrollProperty, value);
        }

        public static bool GetCanContentScroll(DependencyObject element)
        {
            return (bool) element.GetValue(CanContentScrollProperty);
        }

        private static void OnCanContentScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewer.SetCanContentScroll(d, (bool)e.NewValue);
        }
    }
}