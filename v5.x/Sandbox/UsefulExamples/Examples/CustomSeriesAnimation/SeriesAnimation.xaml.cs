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
            lineSeries.DataSeries.DataSeriesChanged += DataSeries_DataSeriesChanged;
        }

        private void DataSeries_DataSeriesChanged(object sender, DataSeriesChangedEventArgs e)
        {
            customAnimation?.StartAnimation();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lineSeries.DataSeries is XyDataSeries<double, double> xySeries)
            {
                var n = 100;
                for (int i = 0; i < n; ++i, ++count)
                {
                    tcs = new TaskCompletionSource<bool>();
                    xySeries.Append(count, 2.0 + Math.Sin(count * 0.2));

                    var result = await tcs.Task;
                    if (result == false) throw new Exception("Previous animation haven't been completed");
                }
            }
        }

        private void CustomSeriesAnimation_AnimationEnded(object sender, EventArgs e)
        {
            tcs?.TrySetResult(true);
        }
    }
}
