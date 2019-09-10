using System;
using System.Windows;
using SciChart.Charting.Model.DataSeries;
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
            var scatterData = new XyDataSeries<double, double>();
            var lineData = new XyDataSeries<double, double>();

            // NEW CODE HERE           
            // Ensure that DataSeries are named for the legend
            scatterData.SeriesName = "Cos(x)";
            lineData.SeriesName = "Sin(x)";
            // END NEW CODE
            for (int i = 0; i < 1000; i++)
            {
                lineData.Append(i, Math.Sin(i * 0.1));
                scatterData.Append(i, Math.Cos(i * 0.1));
            }
            LineSeries.DataSeries = lineData;
            ScatterSeries.DataSeries = scatterData;
        }
    }
}