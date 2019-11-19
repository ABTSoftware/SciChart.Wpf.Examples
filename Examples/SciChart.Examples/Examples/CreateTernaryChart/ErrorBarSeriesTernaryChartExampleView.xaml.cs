using System;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateTernaryChart
{
    /// <summary>
    /// Interaction logic for ErrorBarSeriesTernaryChartExampleView.xaml
    /// </summary>
    public partial class ErrorBarSeriesTernaryChartExampleView : UserControl
    {
        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        private RandomWalkGenerator _random;

        public ErrorBarSeriesTernaryChartExampleView()
        {
            InitializeComponent();

            _random =new RandomWalkGenerator(seed: 0);

            cursorModButton.IsChecked = false;
            tooltipModButton.IsChecked = false;

            // scatters series
            var scatterDataSeries1 = new XyzDataSeries<double> {AcceptsUnsortedData = true, SeriesName = "Residue" };
            var scatterDataSeries2 = new XyzDataSeries<double> {AcceptsUnsortedData = true, SeriesName = "Dolomite" };
            var scatterDataSeries3 = new XyzDataSeries<double> {AcceptsUnsortedData = true, SeriesName = "Calcite" };


            // Residue series
            for (int i = 0; i < 25; i++)
            {
                var x = _random.Next(0, 40);
                var z = _random.Next(40, 60);
                var y = 100 - (x + z);
                scatterDataSeries1.Append(x, y, z);
            }

            // Dolomite series
            for (int i = 0; i < 25; i++)
            {
                var x = _random.Next(0, 40);
                var z = _random.Next(10, 30);
                var y = 100 - (x + z);
                scatterDataSeries2.Append(x, y, z);
            }

            // Calcite series
            for (int i = 0; i < 25; i++)
            {
                var z = _random.Next(70, 90);
                var x = _random.Next(0, 100-z);
                var y = 100 - (x + z);
                scatterDataSeries3.Append(x, y, z);
            }

            scatterSeries1.DataSeries = scatterDataSeries1;
            scatterSeries2.DataSeries = scatterDataSeries2;
            scatterSeries3.DataSeries = scatterDataSeries3;

            // errorbars series
            var errorBarDataSeries1 = (XyzDataSeries<double, double, double>) scatterDataSeries1.Clone();
            var errorBarDataSeries2 = (XyzDataSeries<double, double, double>) scatterDataSeries2.Clone();
            var errorBarDataSeries3 = (XyzDataSeries<double, double, double>) scatterDataSeries3.Clone();

            errorBarDataSeries1.SeriesName = "ResidueError";
            errorBarDataSeries2.SeriesName = "DolomiteError";
            errorBarDataSeries3.SeriesName = "CalciteError";

            // EllipseError series
            errorBarsSeries1.DataSeries = errorBarDataSeries1;

            // TriangleError series
            errorBarsSeries2.DataSeries = errorBarDataSeries2;

            // SquareError series
            errorBarsSeries3.DataSeries = errorBarDataSeries3;

        }
    }
}