using System;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting3D.Model;

namespace Tutorial_03_Adding_Series_to_a_3D_Chart
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // Create XyzDataSeries3D to host data for our chart
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            var color = Color.FromRgb(64, 131, 183); //#FF4083B7

            for (var i = 0; i < 100; i++)
            {
                var x = 5 * Math.Sin(i);
                var y = i;
                var z = 5 * Math.Cos(i);

                xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(color));
            }

            // Assign DataSeries to RenderableSeries
            pointLineSeries3D.DataSeries = xyzDataSeries3D;
        }
    }
}