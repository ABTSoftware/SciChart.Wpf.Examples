using System.Windows;

namespace MutipleUIThreadExample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Title = $"Main Window | {App.CurrentThreadInfo}";

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataManager = new DataManager();

            using (sciChartSurface.SuspendUpdates())
            {
                sciChartSurface.RenderableSeries[0].DataSeries = dataManager.DataSeriesPork;
                sciChartSurface.RenderableSeries[1].DataSeries = dataManager.DataSeriesVeal;
                sciChartSurface.RenderableSeries[2].DataSeries = dataManager.DataSeriesTomato;
                sciChartSurface.RenderableSeries[3].DataSeries = dataManager.DataSeriesCucumber;
                sciChartSurface.RenderableSeries[4].DataSeries = dataManager.DataSeriesPepper;
            }

            sciChartSurface.ZoomExtents();
        }
    }
}