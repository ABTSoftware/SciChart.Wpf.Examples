using System.Windows;
using Microsoft.Xaml.Behaviors;
using SciChart.Charting.Visuals;

namespace OilAndGasExample.VerticalCharts
{
    public class SurfaceToViewModelBehavior : Behavior<SciChartSurface>
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
            if (AssociatedObject.DataContext is VerticalChartViewModel chartViewModel)
            {
                chartViewModel.Suspendable = AssociatedObject;
            }
        }
    }
}