using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.ParallelCoordinatePlot
{
    public partial class ParallelCoordinatePlotView : UserControl
    {
        private ParallelCoordinateDataSource<WeatherData> _pcSource;

        public ParallelCoordinatePlotView()
        {
            InitializeComponent();
        }

        private void ParallelCoordinatePlotView_OnLoaded(object sender, RoutedEventArgs e)
        {
            sciChart.ShowLicensingWarnings = false;

            var defaultAxisStyle = (Style)FindResource("DefaultParallelAxisStyle");
            var alternativeAxisStyle = (Style)FindResource("AlternativeParallelAxisStyle");

            _pcSource = new ParallelCoordinateDataSource<WeatherData>(

                new ParallelCoordinateDataItem<WeatherData, DateTime>(p => p.Date)
                {
                    Title = "Time",
                    AxisStyle = defaultAxisStyle
                },
                new ParallelCoordinateDataItem<WeatherData, double>(p => p.MinTemp)
                {
                    Title = "Min Temp",
                    AxisStyle = alternativeAxisStyle
                },
                new ParallelCoordinateDataItem<WeatherData, double>(p => p.MaxTemp)
                {
                    Title = "Max Temp",
                    AxisStyle = defaultAxisStyle
                },
                new ParallelCoordinateDataItemString<WeatherData>(p => p.Forecast)
                {
                    Title = "Forecast",
                    AxisStyle = alternativeAxisStyle
                },
                new ParallelCoordinateDataItem<WeatherData, double>(p => p.Rainfall)
                {
                    Title = "Rainfall",
                    AxisStyle = defaultAxisStyle
                },
                new ParallelCoordinateDataItem<WeatherData, int>(p => p.UVIndex)
                {
                    Title = "UV Index",
                    AxisStyle = alternativeAxisStyle
                },
                new ParallelCoordinateDataItem<WeatherData, double>(p => p.Sunshine)
                {
                    Title = "Sunshine",
                    AxisStyle = defaultAxisStyle
                },
                new ParallelCoordinateDataItemString<WeatherData>(p => p.WindDirection.ToString())
                {
                    Title = "Wind Direction",
                    AxisStyle = alternativeAxisStyle
                },
                new ParallelCoordinateDataItem<WeatherData, int>(p => p.WindSpeed)
                {
                    Title = "Wind Speed",
                    AxisStyle = defaultAxisStyle
                },
                new ParallelCoordinateDataItem<WeatherData, bool>(p => p.LocalStation)
                {
                    Title = "Local Station",
                    AxisStyle = alternativeAxisStyle
                });

            _pcSource.SetValues(DataManager.Instance.LoadWeatherData());

            sciChart.ParallelCoordinateDataSource = _pcSource;
        }

        private void OnAxisReordered(object sender, ParallelAxisReorderArgs args)
        {
            _pcSource?.ReorderItems(args.OldIndex, args.NewIndex);
        }
    }
}