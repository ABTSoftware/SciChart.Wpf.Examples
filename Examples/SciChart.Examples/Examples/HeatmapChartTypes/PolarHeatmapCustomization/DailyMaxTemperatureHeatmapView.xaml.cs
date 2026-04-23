// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// DailyMaxTemperatureHeatmapView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.HeatmapChartTypes.PolarHeatmapCustomization
{
    /// <summary>
    /// Interaction logic for DailyMaxTemperatureHeatmapView.xaml
    /// Displays daily maximum temperature data in a polar heatmap format with radial and azimuthal grid lines.
    /// </summary>
    public partial class DailyMaxTemperatureHeatmapView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyMaxTemperatureHeatmapView"/> class.
        /// </summary>
        public DailyMaxTemperatureHeatmapView()
        {
            InitializeComponent();

            // Get the data, create a UniformHeatmapDataSeries and
            // assign it to the PolarUniformHeatmapRenderableSeries declared in XAML
            var heatmapDataSeries = GetHeatmapDataSeries();
            HeatmapRenderSeries.DataSeries = heatmapDataSeries;

            // Generate data for chart grid on top of the heatmap
            // This is achieved by drawing additional FastLineRenderableSeries on the chart
            var angleMin = 0;
            var angleMax = heatmapDataSeries.ArrayWidth; // 366 days
            var radiusMin = 0;
            var radiusMax = heatmapDataSeries.ArrayHeight; // years 2011-2024
            AzimuthalGridLinesRenderSeries.DataSeries = GenerateAngularGridLines(angleMin, angleMax, radiusMin, radiusMax);
            AzimuthalGridLinesRenderSeries.DrawNaNAs = LineDrawMode.Gaps;

            RadialGridLinesRenderSeries.DataSeries = GenerateRadialGridLines(radiusMin, radiusMax);
            RadialGridLinesRenderSeries.DrawNaNAs = LineDrawMode.Gaps;
        }

        private IUniformHeatmapDataSeries GetHeatmapDataSeries()
        {
            // Returns daily temperature heatmap for years 2011-2024
            // Every year is considered leap year (366 days)
            var data = DataManager.Instance.GetDailyMaxTemperatureHeatmap();
            var dataSeries = new UniformHeatmapDataSeries<int, int, double>(data, 0, 1, 0, 1)
            {
                SeriesName = "Daily Max Temperature"
            };

            return dataSeries;
        }

        private IDataSeries GenerateAngularGridLines(int angleMin, int angleMax, int radiusMin, int radiusMax)
        {
            var dataSeries = new XyDataSeries<double, double>();
            dataSeries.AcceptsUnsortedData = true;
            
            for (int radius = radiusMin; radius < radiusMax; radius++)
            {
                // Sample a full circle at evenly spaced points to draw a circle
                for (int azimuth = angleMin; azimuth <= angleMax; azimuth++)
                {
                    dataSeries.Append(azimuth, radius);
                }

                // Double.NaN indicates the end of a line segment 
                // Add a NaN to draw disconnected circles
                dataSeries.Append(angleMax, double.NaN);
            }

            return dataSeries;
        }

        private IDataSeries GenerateRadialGridLines(int radiusMin, int radiusMax)
        {
            // Creates data for monthly radial grid lines
            // They are generated for a leap year because the heatmap data has 366 days
            var dataSeries = new XyDataSeries<double, double>();
            var leapYear = 2020;
            for (int month = 0; month <= 12; month++)
            {
                // Draw two lines for January, for the first and the last day
                var monthEndDayOfYear = month == 0 ? 0 : 
                    new DateTime(leapYear, month, DateTime.DaysInMonth(leapYear, month)).DayOfYear;

                // Radial line is defined by 2 points
                dataSeries.Append(monthEndDayOfYear, radiusMin);
                dataSeries.Append(monthEndDayOfYear, radiusMax);

                // Double.NaN indicates the end of a line segment 
                // Add a NaN to draw disconnected lines
                dataSeries.Append(monthEndDayOfYear, double.NaN);
            }
            return dataSeries;
        }
    }
}
