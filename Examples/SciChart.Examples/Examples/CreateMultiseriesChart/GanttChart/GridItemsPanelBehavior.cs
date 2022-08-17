using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.GanttChart
{
    public class GridItemsPanelBehavior : Behavior<Grid>
    {
        public double? RowHeight { get; set; }

        public bool IsReverseOrder { get; set;}

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

        private int GetIndex(int index, int count)
        {
            if (IsReverseOrder)
            {
                return count - index - 1;
            }
            return index;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            if (sender is Grid grid && grid.Children.Count > 0)
            {
                grid.RowDefinitions.Clear();

                var count = grid.Children.Count;

                for (int i = 0; i < count; i++)
                {
                    var child = grid.Children[GetIndex(i, count)];

                    grid.RowDefinitions.Add(new RowDefinition
                    {
                        Height = RowHeight.HasValue
                            ? new GridLength(RowHeight.Value)
                            : new GridLength(1.0, GridUnitType.Star)
                    });

                    Grid.SetRow(child, i);
                }

                if (RowHeight.HasValue)
                {
                    grid.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(1.0, GridUnitType.Star)
                    });
                }
            }
        }
    }
}