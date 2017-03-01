using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Demo.Behaviors
{
    public class ScrollViewerExtensions
    {
        public static readonly DependencyProperty CanContentScrollProperty = DependencyProperty.RegisterAttached(
            "CanContentScroll", typeof (bool), typeof (ScrollViewerExtensions), new PropertyMetadata(true, OnCanContentScrollChanged));        

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
#if !SILVERLIGHT
            ScrollViewer.SetCanContentScroll(d, (bool)e.NewValue);
#else
            
#endif

        }
    }
}