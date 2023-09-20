using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Visuals.RenderableSeries;

namespace MutipleAppDomainsExample
{
    public partial class ChartWindow : Window
    {
        public string CurrentDomainName => AppDomain.CurrentDomain.FriendlyName;

        public string CurrentThreadName => $"Thread #{Thread.CurrentThread.ManagedThreadId} [{Thread.CurrentThread.GetApartmentState()}]";

        public ChartWindow()
        {
            InitializeComponent();
            InitializeWindow();

            Loaded += OnLoaded;
            Closed += OnClosed;  
        }

        private void InitializeWindow()
        {
            var positionOffset = CurrentDomainName.EndsWith("1") ? 0d : 200;

            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2) + positionOffset;
            Top = (SystemParameters.PrimaryScreenHeight / 2) - (Height / 2) + positionOffset;

            Title = $"{CurrentDomainName} | {CurrentThreadName}";
        }

        private void CreateGroupedChart()
        {
            var dataManager = new DataManager();

            sciChartSurface.ChartTitle = "Meat vs. Vegetables (Grouped)";

            using (sciChartSurface.SuspendUpdates())
            {
                sciChartSurface.RenderableSeries.Add(new StackedColumnRenderableSeries
                {
                    DataSeries = dataManager.DataSeriesPork,
                    Fill = new SolidColorBrush(Color.FromRgb(34, 111, 183)), //#FF226FB7
                    StackedGroupId = "Meat"
                });

                sciChartSurface.RenderableSeries.Add(new StackedColumnRenderableSeries
                {
                    DataSeries = dataManager.DataSeriesVeal,
                    Fill = new SolidColorBrush(Color.FromRgb(255, 154, 46)), //#FFFF9A2E
                    StackedGroupId = "Meat"
                });

                sciChartSurface.RenderableSeries.Add(new StackedColumnRenderableSeries
                {
                    DataSeries = dataManager.DataSeriesTomato,
                    Fill = new SolidColorBrush(Color.FromRgb(220, 68, 63)), //#FFDC443F
                    StackedGroupId = "Vegetables"
                });

                sciChartSurface.RenderableSeries.Add(new StackedColumnRenderableSeries
                {
                    DataSeries = dataManager.DataSeriesCucumber,
                    Fill = new SolidColorBrush(Color.FromRgb(170, 211, 79)), //#FFAAD34F
                    StackedGroupId = "Vegetables"
                });

                sciChartSurface.RenderableSeries.Add(new StackedColumnRenderableSeries
                {
                    DataSeries = dataManager.DataSeriesPepper,
                    Fill = new SolidColorBrush(Color.FromRgb(133, 98, 180)), //#FF8562B4
                    StackedGroupId = "Vegetables"
                });
            }

            sciChartSurface.ZoomExtents();
        }

        private void CreateAggregatedChart()
        {
            var dataManager = new DataManager();

            sciChartSurface.ChartTitle = "Meat vs. Vegetables (Aggregated)";

            using (sciChartSurface.SuspendUpdates())
            {
                sciChartSurface.RenderableSeries.Add(new StackedMountainRenderableSeries
                {
                    DataSeries = dataManager.AggregateByCategory(Category.Meat),
                    Fill = new SolidColorBrush(Color.FromRgb(34, 111, 183)), //#FF226FB7
                    Stroke = Color.FromRgb(34, 111, 183)
                });

                sciChartSurface.RenderableSeries.Add(new StackedMountainRenderableSeries
                {
                    DataSeries = dataManager.AggregateByCategory(Category.Vegetables),
                    Fill = new SolidColorBrush(Color.FromRgb(170, 211, 79)), //#FFAAD34F
                    Stroke = Color.FromRgb(170, 211, 79)
                });
            }

            sciChartSurface.ZoomExtents();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (CurrentDomainName.EndsWith("1"))
            {
                CreateGroupedChart();
            }
            else
            {
                CreateAggregatedChart();
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Dispatcher.InvokeShutdown();
        }
    }
}