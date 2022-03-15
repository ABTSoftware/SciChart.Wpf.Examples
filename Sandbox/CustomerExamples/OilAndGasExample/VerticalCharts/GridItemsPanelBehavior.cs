using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace OilAndGasExample.VerticalCharts
{
    public class GridItemsPanelBehavior : Behavior<Grid>
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

        private void OnLoaded(object sender, EventArgs e)
        {
            if (sender is Grid grid)
            { 
                if (grid.ColumnDefinitions?.Count > 0)
                    grid.ColumnDefinitions.Clear();
                
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    if (grid.Children[i] is FrameworkElement child)
                        Grid.SetColumn(child, i);
                }
            }
        }
    }
}