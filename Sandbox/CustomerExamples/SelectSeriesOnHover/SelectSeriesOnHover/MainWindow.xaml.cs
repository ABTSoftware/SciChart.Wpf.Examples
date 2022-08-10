using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;

namespace SelectSeriesOnHover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            line0.DataSeries = GetData(1);
            line1.DataSeries = GetData(2);
            line2.DataSeries = GetData(3);

            line0.SelectionChanged += (s, e) => Console.WriteLine($"Series 0 IsSelected=${line0.IsSelected}");
            line1.SelectionChanged += (s, e) => Console.WriteLine($"Series 0 IsSelected=${line1.IsSelected}");
            line2.SelectionChanged += (s, e) => Console.WriteLine($"Series 0 IsSelected=${line2.IsSelected}");
        }

        private IDataSeries GetData(int index)
        {
            var xyDataSeries = new XyDataSeries<double>();
            for (int i = 0; i < 100; i++)
            {
                xyDataSeries.Append(i, Math.Sin(i * 0.05 * index) + Math.Cos(i * 0.01 * index + 0.05));
            }

            return xyDataSeries;
        }
    }
}
