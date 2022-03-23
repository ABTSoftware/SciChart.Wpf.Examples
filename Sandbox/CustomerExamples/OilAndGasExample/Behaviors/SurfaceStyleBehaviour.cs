using System.Windows;
using Microsoft.Xaml.Behaviors;
using OilAndGasExample.Common;
using SciChart.Charting.Visuals;

namespace OilAndGasExample.Behaviors
{
    public class SurfaceStyleBehaviour : Behavior<SciChartSurface>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= OnLoaded;
        }
        
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.DataContext is ChartViewModel chartViewModel)
            {
                var styleKey = chartViewModel.ChartFactory?.StyleKey;

                if (!string.IsNullOrEmpty(styleKey))
                {
                    var style = AssociatedObject.TryFindResource(styleKey);

                    if (style is Style surfaceStyle)
                    {
                        AssociatedObject.Style = surfaceStyle;
                    }
                }
            }
        }
    }
}