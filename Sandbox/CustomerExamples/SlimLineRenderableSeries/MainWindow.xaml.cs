using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SlimLineRenderableSeriesExample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Create a DataSeries of type X=double, Y=double
            var dataSeries = new UniformXyDataSeries<double>(0d, 0.002);

            // Set data to slim renderable series
            slimLineRenderSeries.DataSeries = dataSeries;

            // Append data to series. SciChart automatically redraws
            dataSeries.Append(DataManager.Instance.GetFourierYData(1.0, 0.1));

            sciChart.ZoomExtents();
        }
    }
}