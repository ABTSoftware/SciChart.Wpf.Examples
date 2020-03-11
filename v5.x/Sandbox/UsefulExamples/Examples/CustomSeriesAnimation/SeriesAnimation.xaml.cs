using SciChart.Charting.Model.DataSeries;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SciChart.Sandbox.Examples.CustomSeriesAnimation
{
    [TestCase("Custom SeriesAnimation")]
    public partial class SeriesAnimation : Window
    {
        private int count = 120;
        private TaskCompletionSource<bool> tcs = null;

        public SeriesAnimation()
        {
            InitializeComponent();

            // Initialize a DataSeries with some data
            var data = new XyDataSeries<double, double>();
            for (int i = 0; i < count; i++)
            {
                if (i < 31)
                    data.Append(i, Math.Sin(i * 0.1));
                else if (i < 95)
                    data.Append(i, 1.5 * (1.0 + Math.Cos(i * 0.1)));
                else
                    data.Append(i, Math.Min(1.0, Math.Sqrt(i) - Math.Sqrt(95)));
            }

            lineSeries.DataSeries = data;

            // Subscribe to changes in the DataSeries
            lineSeries.DataSeries.DataSeriesChanged += (sender, args) => customAnimation?.StartAnimation();
        }

        private async void OnAppendButtonClick(object sender, RoutedEventArgs e)
        {
            if (lineSeries.DataSeries is XyDataSeries<double, double> xySeries)
            {
                var n = 100;
                for (int i = 0; i < n; ++i, ++count)
                {
                    tcs = new TaskCompletionSource<bool>();
                    xySeries.Append(count, 2.0 + Math.Sin(count * 0.2));

                    var result = await tcs.Task;
                    if (!result) throw new Exception("Animation is still running.");
                }
            }
        }

        private void CustomSeriesAnimation_AnimationEnded(object sender, EventArgs e)
        {
            tcs?.TrySetResult(true);
        }
    }
}
