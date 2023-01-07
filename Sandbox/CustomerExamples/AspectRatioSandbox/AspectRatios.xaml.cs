using System;
using System.Linq;
using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace SciChart.Sandbox.Examples.AspectRatios
{
    public partial class AspectRatios : Window
    {
        public AspectRatios()
        {
            InitializeComponent();

            // Draw a circle on the chart
            double radius = 3;
            var dataSeries = new XyDataSeries<double>() { AcceptsUnsortedData = true };
            for (int i = 0; i <= 100; i++)
            {
                double x = radius * Math.Cos(2 * Math.PI * i / 100) + 5;
                double y = radius * Math.Sin(2 * Math.PI * i / 100) + 5;
                dataSeries.Append(x,y);
            }

            this.lineSeries.DataSeries = dataSeries;

            this.SizeChanged += Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = new Size(this.sciChart.GridLinesPanel.Width, this.sciChart.GridLinesPanel.Height);

            var yRange = yAxis.VisibleRange.AsDoubleRange();
            var xRange = xAxis.VisibleRange.AsDoubleRange();

            // We want to normalize xAxis.visiblerange.max so that 
            // (yRange.Max - yRange.Min) = (xRange.Max - xRange.Min)
            // ------------------------    -------------------------
            //         size.Height               size.Width

            var newXMax = (yRange.Max - yRange.Min) * size.Width / size.Height + xRange.Min;

            xAxis.VisibleRange = new DoubleRange(xRange.Min, newXMax);
        }
    }
}
