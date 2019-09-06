using System;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.CreateTernaryChart
{
    /// <summary>
    /// Interaction logic for ScatterSeriesTernaryChartExampleView.xaml
    /// </summary>
    public partial class ScatterSeriesTernaryChartExampleView : UserControl
    {
        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        private Random _random;
        public ScatterSeriesTernaryChartExampleView()
        {
            InitializeComponent();

            _random = new Random();

            // scatters series
            var scatterDataSeries1 = new XyzDataSeries<double> { AcceptsUnsortedData = true, SeriesName = "Substance A" };
            var scatterDataSeries2 = new XyzDataSeries<double> { AcceptsUnsortedData = true, SeriesName = "Substance B" };
            var scatterDataSeries3 = new XyzDataSeries<double> { AcceptsUnsortedData = true, SeriesName = "Substance C" };

            // Air series
            for (int i = 0; i < 75; i++)
            {
                var x = _random.Next(0, 50);
                var z = _random.Next(20, 40);
                var y = 100 - (x + z);
                scatterDataSeries1.Append(x, y, z);
            }

            // Earth series
            for (int i = 0; i < 75; i++)
            {
                var x = _random.Next(0, 40);
                var z = _random.Next(0, 20);
                var y = 100 - (x + z);
                scatterDataSeries2.Append(x, y, z);
            }

            // Fire series
            for (int i = 0; i < 75; i++)
            {
                var x = _random.Next(20, 40);
                var y = _random.Next(0, 25);
                var z = 100 - (x + y);
                scatterDataSeries3.Append(x, y, z);
            }

            scatterSeries1.DataSeries = scatterDataSeries1;
            scatterSeries2.DataSeries = scatterDataSeries2;
            scatterSeries3.DataSeries = scatterDataSeries3;
        }
    }
}
