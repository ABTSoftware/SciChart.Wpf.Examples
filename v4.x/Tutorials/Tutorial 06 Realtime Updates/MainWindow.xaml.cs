using System;
using System.Windows;
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace SciChart.Tutorial
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            //Method1_UpdatingDataValues();
            //Method2_AppendingDataValues();
            //Method3_FifoScrollingUpdates();
            Method4_ScrollingAndAllowingZooming();
        }        

        // The First method of updating a real-time chart: Updating Data Values
        private void Method1_UpdatingDataValues()
        {
            var scatterData = new XyDataSeries<double, double>();
            var lineData = new XyDataSeries<double, double>();

            // Ensure that DataSeries are named for the legend
            scatterData.SeriesName = "Cos(x)";
            lineData.SeriesName = "Sin(x)";
            for (int i = 0; i < 1000; i++)
            {
                lineData.Append(i, Math.Sin(i * 0.1));
                scatterData.Append(i, Math.Cos(i * 0.1));
            }
            LineSeries.DataSeries = lineData;
            ScatterSeries.DataSeries = scatterData;
            // Start a timer to update our data
            double phase = 0.0;
            var timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += (s, e) =>
            {
                // SuspendUpdates() ensures the chart is frozen
                // while you do updates. This ensures best performance
                using (lineData.SuspendUpdates())
                using (scatterData.SuspendUpdates())
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        // Updates the Y value at index i
                        lineData.Update(i, Math.Sin(i * 0.1 + phase));
                        scatterData.Update(i, Math.Cos(i * 0.1 + phase));
                    }
                }
                phase += 0.01;
            };
            timer.Start();
        }

        // The second method of updating a realtime chart: Appending Data values
        private void Method2_AppendingDataValues()
        {
            var scatterData = new XyDataSeries<double, double>();
            var lineData = new XyDataSeries<double, double>();
            // Ensure that DataSeries are named for the legend
            scatterData.SeriesName = "Cos(x)";
            lineData.SeriesName = "Sin(x)";
            LineSeries.DataSeries = lineData;
            ScatterSeries.DataSeries = scatterData;
            // Start a timer to update our data
            var timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += (s, e) =>
            {
                // This time we will append, not update.
                using (lineData.SuspendUpdates())
                using (scatterData.SuspendUpdates())
                {
                    int i = lineData.Count;
                    // Append a new data point;
                    lineData.Append(i, Math.Sin(i * 0.1));
                    scatterData.Append(i, Math.Cos(i * 0.1));
                    // ZoomExtents after appending data.
                    // Also see XAxis.AutoRange, and XAxis.VisibleRange for more options
                    sciChartSurface.ZoomExtents();
                }
            };
            timer.Start();
        }

        // The third method of updating a realtime chart: Using FIFO (First in first out) series
        private void Method3_FifoScrollingUpdates()
        {
            // Create DataSeries with FifoCapacity
            var scatterData = new XyDataSeries<double, double>() { SeriesName = "Cos(x)", FifoCapacity = 1000 };
            var lineData = new XyDataSeries<double, double>() { SeriesName = "Sin(x)", FifoCapacity = 1000 };
            // Assign DataSeries to RenderableSeries
            LineSeries.DataSeries = lineData;
            ScatterSeries.DataSeries = scatterData;
            int i = 0;
            // Start a timer to update our data
            var timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += (s, e) =>
            {
                // This time we will append, not update.
                using (lineData.SuspendUpdates())
                using (scatterData.SuspendUpdates())
                {
                    // Append a new data point;
                    lineData.Append(i, Math.Sin(i * 0.1));
                    scatterData.Append(i, Math.Cos(i * 0.1));
                    // Set VisibleRange to last 1,000 points
                    sciChartSurface.XAxis.VisibleRange = new DoubleRange(i - 1000, i);
                    i++;
                }

            };
            timer.Start();
        }

        // The final method of updating a realtime chart. Using a ViewportManager to allow zooming + scrolling at the same time
        private void Method4_ScrollingAndAllowingZooming()
        {
            // Instantiate the ViewportManager here
            double windowSize = 1000.0;
            sciChartSurface.ViewportManager = new ScrollingViewportManager(windowSize);

            // Create DataSeries with FifoCapacity
            var scatterData = new XyDataSeries<double, double>() { SeriesName = "Cos(x)",  };
            var lineData = new XyDataSeries<double, double>() { SeriesName = "Sin(x)",  };
            // Assign DataSeries to RenderableSeries
            LineSeries.DataSeries = lineData;
            ScatterSeries.DataSeries = scatterData;
            int i = 0;
            // Start a timer to update our data
            var timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += (s, e) =>
            {
                // This time we will append, not update.
                using (lineData.SuspendUpdates())
                using (scatterData.SuspendUpdates())
                {
                    // Append a new data point;
                    lineData.Append(i, Math.Sin(i * 0.1));
                    scatterData.Append(i, Math.Cos(i * 0.1));
                    i++;
                }

            };
            timer.Start();
        }
    }
}