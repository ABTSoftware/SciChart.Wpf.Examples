using System.Windows;

namespace MutipleUIThreadExample
{
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();

            Title = $"Second Window | {App.CurrentThreadInfo}";

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataManager = new DataManager();

            using (sciChartSurface.SuspendUpdates())
            {
                sciChartSurface.RenderableSeries[0].DataSeries = dataManager.AggregateByCategory(Category.Meat);
                sciChartSurface.RenderableSeries[1].DataSeries = dataManager.AggregateByCategory(Category.Vegetables);
            }

            sciChartSurface.ZoomExtents();
        }
    }
}