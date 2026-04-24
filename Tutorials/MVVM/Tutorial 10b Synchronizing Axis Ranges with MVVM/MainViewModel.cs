using System;
using System.Windows.Media;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace SciChart.Mvvm.Tutorial
{
    public class MainViewModel : BindableObject
    {
        public ChartViewModel CelsiusChart { get; }
        public ChartViewModel FahrenheitChart { get; }

        public MainViewModel()
        {
            CelsiusChart = BuildCelsiusChart();
            FahrenheitChart = BuildFahrenheitChart();

            CelsiusChart.RenderableSeries.ZoomExtentsWhenReady();
            FahrenheitChart.RenderableSeries.ZoomExtentsWhenReady();
        }

        private ChartViewModel BuildCelsiusChart()
        {
            var chart = new ChartViewModel { ChartTitle = "Temperature (°C)" };

            // X axis — synced with the Fahrenheit chart's X axis.
            // Both share "TimeGroup" with no transform (same time unit).
            chart.XAxes.Add(new NumericAxisViewModel
            {
                AxisTitle = "Time (s)",
                AutoRange = AutoRange.Once,
                RangeSyncGroupId = "TimeGroup"
            });

            // Y axis — synced with the Fahrenheit chart's Y axis.
            // No transform: this axis's range IS the canonical group range (Celsius).
            //
            // RangeSyncOnZoomExtents = true makes this axis the authoritative source
            // for the "TempGroup" during ZoomExtents. When ZoomExtents fires on either
            // chart, this axis computes its own extents and pushes them to the group;
            // the Fahrenheit axis then converts via FromGroupRange.
            chart.YAxes.Add(new NumericAxisViewModel
            {
                AxisTitle = "°C",
                AutoRange = AutoRange.Once,
                RangeSyncGroupId = "TempGroup"
            });

            // Enable RangeSyncOnZoomExtents on all axes in this chart.
            // This makes ZoomExtents propagate the computed extents to the sync group.
            chart.RangeSyncOnZoomExtents = true;

            // Sample data: a temperature signal oscillating around 20 °C
            var ds = new XyDataSeries<double, double> { SeriesName = "Sensor (°C)" };
            for (int i = 0; i < 500; i++)
            {
                double x = i * 0.1;
                ds.Append(x, 20 + 10 * Math.Sin(x * 0.5) + 5 * Math.Sin(x * 1.3));
            }

            chart.RenderableSeries.Add(new LineRenderableSeriesViewModel
            {
                DataSeries = ds,
                Stroke = Color.FromRgb(0x50, 0xC7, 0xE0),
                StrokeThickness = 2
            });

            return chart;
        }

        private ChartViewModel BuildFahrenheitChart()
        {
            var chart = new ChartViewModel { ChartTitle = "Temperature (°F)" };

            // X axis — same sync group, no transform
            chart.XAxes.Add(new NumericAxisViewModel
            {
                AxisTitle = "Time (s)",
                AutoRange = AutoRange.Once,
                RangeSyncGroupId = "TimeGroup"
            });

            // Y axis — same sync group as the Celsius chart, but with a transform.
            // This axis displays Fahrenheit; the group range is Celsius.
            // CelsiusFahrenheitTransform converts between the two:
            //   ToGroupRange:   F → C  (when this axis changes, broadcast Celsius to the group)
            //   FromGroupRange: C → F  (when the group changes, update this axis to Fahrenheit)
            chart.YAxes.Add(new NumericAxisViewModel
            {
                AxisTitle = "°F",
                AutoRange = AutoRange.Once,
                RangeSyncGroupId = "TempGroup",
                RangeSyncTransform = new CelsiusFahrenheitTransform()
            });

            // Same signal converted to Fahrenheit: F = C × 9/5 + 32
            var ds = new XyDataSeries<double, double> { SeriesName = "Sensor (°F)" };
            for (int i = 0; i < 500; i++)
            {
                double x = i * 0.1;
                double celsius = 20 + 10 * Math.Sin(x * 0.5) + 5 * Math.Sin(x * 1.3);
                ds.Append(x, celsius * 9.0 / 5.0 + 32);
            }

            chart.RenderableSeries.Add(new LineRenderableSeriesViewModel
            {
                DataSeries = ds,
                Stroke = Color.FromRgb(0xF4, 0x84, 0x20),
                StrokeThickness = 2
            });

            return chart;
        }
    }
}
