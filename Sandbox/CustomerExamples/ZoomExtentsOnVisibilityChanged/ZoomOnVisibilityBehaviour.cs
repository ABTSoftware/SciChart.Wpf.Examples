using System;
using System.Windows;
using SciChart.Charting.Visuals;

namespace ZoomExtentsOnVisibilityChangedExample
{
    public class ZoomOnVisibilityBehaviour
    {
        public static readonly DependencyProperty ZoomExtentsOnVisibilityChangedProperty = DependencyProperty.RegisterAttached(
            "ZoomExtentsOnVisibilityChanged", typeof(bool), typeof(ZoomOnVisibilityBehaviour), new PropertyMetadata(default(bool), OnPropertyChanged));        

        public static void SetZoomExtentsOnVisibilityChanged(DependencyObject element, bool value)
        {
            element.SetValue(ZoomExtentsOnVisibilityChangedProperty, value);
        }

        public static bool GetZoomExtentsOnVisibilityChanged(DependencyObject element)
        {
            return (bool) element.GetValue(ZoomExtentsOnVisibilityChangedProperty);
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue) == true)
            {
                var scs = ((SciChartSurface) d);

                scs.Loaded += (sender, args) =>
                {
                    foreach (var rs in scs.RenderableSeries)
                    {
                        rs.IsVisibleChanged += (_, __) =>
                        {
                            // Animate zoom etents when IsVisibleChanged 
                            scs.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
                        };
                    }
                };

                // TODO: If you want to handle all cases, use PropertyNotifier to listen to when scs.RenderableSeries has a new 
                // collection assigned, and then subscribe to scs.RenderableSeries.CollectionChanged in case series are added/removed
            }
        }
    }
}