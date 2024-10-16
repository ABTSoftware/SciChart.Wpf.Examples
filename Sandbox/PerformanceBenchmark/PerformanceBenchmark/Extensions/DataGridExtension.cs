using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PerformanceBenchmark.Extensions
{
    public static class DataGridExtension
    {
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached
            ("Columns", typeof(ObservableCollection<DataGridColumn>), typeof(DataGridExtension),
            new UIPropertyMetadata(new ObservableCollection<DataGridColumn>(), OnDataGridColumnsPropertyChanged));

        public static ObservableCollection<DataGridColumn> GetColumns(DependencyObject obj)
        {
            return (ObservableCollection<DataGridColumn>)obj.GetValue(ColumnsProperty);
        }

        public static void SetColumns(DependencyObject obj, ObservableCollection<DataGridColumn> value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        private static void OnDataGridColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d.GetType() == typeof(DataGrid))
            {
                DataGrid myGrid = d as DataGrid;

                var Columns = (ObservableCollection<DataGridColumn>)e.NewValue;

                if (Columns != null)
                {
                    myGrid.Columns.Clear();

                    if (Columns != null && Columns.Count > 0)
                    {
                        foreach (DataGridColumn dataGridColumn in Columns)
                        {
                            myGrid.Columns.Add(dataGridColumn);
                        }
                    }

                    Columns.CollectionChanged += (s, e) =>
                    {
                        if (e.NewItems != null)
                        {
                            foreach (DataGridColumn column in e.NewItems.Cast<DataGridColumn>())
                            {
                                myGrid.Columns.Add(column);
                            }
                        }

                        if (e.OldItems != null)
                        {
                            foreach (DataGridColumn column in e.OldItems.Cast<DataGridColumn>())
                            {
                                myGrid.Columns.Remove(column);
                            }
                        }
                    };
                }
            }
        }
    }
}
