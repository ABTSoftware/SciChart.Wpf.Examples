// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HitTestDatapoints.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using SciChart.Charting.Model.ChartData;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.InspectDatapoints
{
    public partial class HitTestDatapoints : UserControl
    {
        public HitTestDatapoints()
        {
            InitializeComponent();

            // Append some data
            var series0 = new XyDataSeries<double, double> { SeriesName = "Line Series"};
            series0.Append(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new double[] { 0, 0.1, 0.3, 0.5, 0.4, 0.35, 0.3, 0.25, 0.2, 0.1 });
            renderableLineSeries.DataSeries = series0;

            var series1 = new XyDataSeries<double, double> { SeriesName = "Column Series"};
            series1.Append(series0.XValues, series0.YValues.Select(x => x * 0.7));
            renderableColumnSeries.DataSeries = series1;

            var series2 = new OhlcDataSeries<double, double> {SeriesName = "Candlestick Series"};
            series2.Append(series0.XValues, series0.YValues.Select(x => x + 0.5), series0.YValues.Select(x => x + 1.0), series0.YValues.Select(x => x + 0.3), series0.YValues.Select(x => x + 0.7));
            renderableCandlestickSeries.DataSeries = series2;
        }

        private void SciChartSurfaceMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Perform the hit test relative to the GridLinesPanel
            var hitTestPoint = e.GetPosition(sciChartSurface.GridLinesPanel as UIElement);

            // Show info for series, which HitTest operation was successful for only
            foreach(var renderableSeries in sciChartSurface.RenderableSeries)
            {
                // Get hit-test the RenderableSeries using interpolation
                var hitTestInfo = renderableSeries.HitTestProvider.HitTest(hitTestPoint, true);

                if (hitTestInfo.IsHit)
                {
                    // Convert the result of hit-test operation to SeriesInfo
                    var seriesInfo = renderableSeries.GetSeriesInfo(hitTestInfo);

                    // Output result
                    var formattedString = SeriesInfoToFormattedString(seriesInfo, hitTestPoint);

                    // Show result
                    Console.WriteLine(formattedString);
                    AddOnView(formattedString);
                }
            }
        }

        private string SeriesInfoToFormattedString(SeriesInfo seriesInfo, Point hitTestPoint)
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0}\nMouse Coord: {1:N2}, {2:N2}\nNearest Point Coord: {3:N2}, {4:N2} \nData Value: {5:N2}, {6:N2}", 
                    seriesInfo.SeriesName,
                    seriesInfo.XyCoordinate.X, seriesInfo.XyCoordinate.Y,
                    hitTestPoint.X, hitTestPoint.Y,
                    seriesInfo.XValue, seriesInfo.YValue);
        }

        private void AddOnView(string formattedString)
        {
            var newItem = new ListBoxItem {Content = formattedString};
            hitTestListBox.Items.Add(newItem);
            hitTestListBox.ScrollIntoView(newItem);
        }
    }
}