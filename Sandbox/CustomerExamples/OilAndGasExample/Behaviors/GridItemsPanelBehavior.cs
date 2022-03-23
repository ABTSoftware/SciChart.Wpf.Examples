using System;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace OilAndGasExample.Behaviors
{
    public enum ItemPlacement
    {
        Row,
        Column,
        RowAndColumn
    }

    public class GridItemsPanelBehavior : Behavior<Grid>
    {
        public ItemPlacement ItemPlacement { get; set; }

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
            if (sender is Grid grid && grid.Children.Count > 0)
            {
                grid.RowDefinitions.Clear();
                grid.ColumnDefinitions.Clear();

                if (ItemPlacement == ItemPlacement.RowAndColumn)
                {
                    var index = 0;
                    var count = grid.Children.Count;

                    var rows = (int)Math.Sqrt(grid.Children.Count);
                    var columns = grid.Children.Count / rows;

                    for (int i = 0; i < rows; i++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition());

                        for (int j = 0; j < columns; j++)
                        {
                            if (i == 0)
                            {
                                grid.ColumnDefinitions.Add(new ColumnDefinition());
                            }

                            if (index < count)
                            {
                                var child = grid.Children[index];

                                Grid.SetRow(child, i);
                                Grid.SetColumn(child, j);

                                index++;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < grid.Children.Count; i++)
                    {
                        var child = grid.Children[i];

                        if (ItemPlacement == ItemPlacement.Row)
                        {
                            grid.RowDefinitions.Add(new RowDefinition());
                            Grid.SetRow(child, i);
                        }
                        else if (ItemPlacement == ItemPlacement.Column)
                        {
                            grid.ColumnDefinitions.Add(new ColumnDefinition());
                            Grid.SetColumn(child, i);
                        }
                    }
                }
            }
        }
    }
}