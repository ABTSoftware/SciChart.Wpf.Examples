using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.DashedLinesChart
{
    public partial class DashedLinesChartExampleView : UserControl
    {
        public DashedLinesChartExampleView()
        {
            InitializeComponent();
        }

        private void DigitalLineChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Create line data series
            var lineDataSeries = new XyDataSeries<double, double>();
            lineDataSeries.Append(new[] { 0.0, 1.0}, new[] { 0.0, 0.0 });
            lineSeries.DataSeries = lineDataSeries;

            // Create band data series
            var bandDataSeries = new XyyDataSeries<double, double>();
            bandDataSeries.Append(new [] {0.0, 1.0}, new []{1.0, 2.0}, new []{2.0, 1.0});
            bandSeries.DataSeries = bandDataSeries;

            sciChart.ZoomExtents();
        }
    }
}
