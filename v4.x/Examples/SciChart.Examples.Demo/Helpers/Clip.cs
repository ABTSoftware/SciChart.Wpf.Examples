using System.Windows;
using System.Windows.Media;

namespace SciChart.Examples.Demo.Helpers
{
    public class Clip
    {
        public static readonly DependencyProperty ToBoundsProperty = DependencyProperty.RegisterAttached("ToBounds", typeof(bool), typeof(Clip), new PropertyMetadata(false, OnToBoundsPropertyChanged));

        public static bool GetToBounds(DependencyObject depObj)
        {
            return (bool)depObj.GetValue(ToBoundsProperty);
        }

        public static void SetToBounds(DependencyObject depObj, bool clipToBounds)
        {
            depObj.SetValue(ToBoundsProperty, clipToBounds);
        }

        private static void OnToBoundsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe != null)
            {
                ClipToBounds(fe);

                fe.Loaded += fe_Loaded;
                fe.SizeChanged += fe_SizeChanged;
            }
        }

        private static void ClipToBounds(FrameworkElement fe)
        {
            fe.Clip = GetToBounds(fe)
                ? new RectangleGeometry {Rect = new Rect(0, 0, fe.ActualWidth, fe.ActualHeight)}
                : null;
        }

        static void fe_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClipToBounds(sender as FrameworkElement);
        }

        static void fe_Loaded(object sender, RoutedEventArgs e)
        {
            ClipToBounds(sender as FrameworkElement);
        }
    }
}