using System;
using System.Windows;
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Core.Extensions;
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
                    // Every 100th datapoint, add an annotation
                    if (i % 100 == 0)
                    {
                        sciChartSurface.Annotations.Add(new InfoAnnotation()
                        {
                            X1 = i,
                            Y1 = 0.0,
                            YAxisId = i % 200 == 0 ? AxisBase.DefaultAxisId : "Axis2"
                        });
                        // Optional: Don't forget to remove annotations which are out of range!
                        sciChartSurface.Annotations.RemoveWhere(x => x.X1.ToDouble() < i - 1000);
                    }
                    i++;
                }

            };
            timer.Start();
        }
    }
}