using System.Windows;

namespace CustomPointMarker
{
    public class SeriesExtensions
    {
        /// <summary>
        /// When true, draws the line in a FastLineRenderableSeries, else, hides it
        /// </summary>
        public static readonly DependencyProperty DrawLineProperty = DependencyProperty.RegisterAttached(
            "DrawLine", typeof (bool), typeof (SeriesExtensions), new PropertyMetadata(true));

        public static void SetDrawLine(DependencyObject element, bool value)
        {
            element.SetValue(DrawLineProperty, value);
        }

        public static bool GetDrawLine(DependencyObject element)
        {
            return (bool) element.GetValue(DrawLineProperty);
        }

        /// <summary>
        /// When True, draws a marker in a FastLineRenderableSeries, else, hides it
        /// </summary>
        public static readonly DependencyProperty DrawMarkerProperty = DependencyProperty.RegisterAttached(
            "DrawMarker", typeof (bool), typeof (SeriesExtensions), new PropertyMetadata(true));

        public static void SetDrawMarker(DependencyObject element, bool value)
        {
            element.SetValue(DrawMarkerProperty, value);
        }

        public static bool GetDrawMarker(DependencyObject element)
        {
            return (bool) element.GetValue(DrawMarkerProperty);
        }
    }
}
