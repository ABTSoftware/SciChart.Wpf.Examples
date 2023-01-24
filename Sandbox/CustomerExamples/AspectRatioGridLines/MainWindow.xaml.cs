using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace AspectRatioGridLines
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;

            sciChartSurface.SizeChanged += (s, e) => SetXAxisVisibleRange();

            yAxis.VisibleRangeChanged += (s, e) => SetXAxisVisibleRange();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Create a DataSeries of type X=double, Y=double
            var dataSeries = new UniformXyDataSeries<double>(-2500d, 1d);

            lineSeries.DataSeries = dataSeries;

            // Append data to series. SciChart automatically redraws
            dataSeries.Append(DataManager.Instance.GetFourierYData(100d, 0.1));

            yAxis.VisibleRange = new DoubleRange { Min = -1350, Max = 1350 };
        }

        private void SetXAxisVisibleRange()
        {
            var surface = sciChartSurface.RenderSurface;

            if (surface != null)
            {
                var xVisibleRange = VisibleRangeHelper.GetXRangeByYRange
                    (surface.ActualHeight, surface.ActualWidth, yAxis.VisibleRange);

                if (xVisibleRange?.IsDefined == true)
                {
                    xAxis.VisibleRange = xVisibleRange;
                }
            }
        }
    }
}