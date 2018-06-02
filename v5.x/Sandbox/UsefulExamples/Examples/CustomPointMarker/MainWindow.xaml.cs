using System.Linq;
using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Sandbox;

namespace CustomPointMarker
{
    [TestCase("Custom pointmarker and legend")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                var data0 = new XyDataSeries<double, double>();
                data0.Append(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new double[] { 0, 1, 2, 3, 2, 1, 0, -1, -2, -1, 0 });
                data0.SeriesName = "TrianglePointMarker";

                var data1 = new XyDataSeries<double, double>();
                data1.Append(data0.XValues, data0.YValues.Select(y => y * -2));
                data1.SeriesName = "SpritePointMarker";

                var data2 = new XyDataSeries<double, double>();
                data2.Append(data0.XValues.Select(x => x * 2), data0.YValues.Select(y => y * -2));
                data2.SeriesName = "Custom PointMarker";

                series0.DataSeries = data0;
                series1.DataSeries = data1;
                series2.DataSeries = data2;
            };
        }
    }
}
