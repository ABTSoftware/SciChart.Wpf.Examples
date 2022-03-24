using System.Windows;
using Microsoft.Xaml.Behaviors;
using OilAndGasExample.Common;
using SciChart.Charting.Visuals;

namespace OilAndGasExample.Behaviors
{
    public class SurfaceToViewModelBehavior : Behavior<SciChartSurfaceBase>
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
                chartViewModel.Suspendable = AssociatedObject;
            }

            if (AssociatedObject.DataContext is Chart3DViewModel chart3DViewModel)
            {
                chart3DViewModel.Suspendable = AssociatedObject;
            }
        }
    }
}