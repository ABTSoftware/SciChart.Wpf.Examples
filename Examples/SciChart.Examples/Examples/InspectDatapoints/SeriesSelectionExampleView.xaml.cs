// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SeriesSelectionExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.InspectDatapoints
{
    public partial class SeriesSelectionExampleView : UserControl
    {
        private const int SeriesPointCount = 150;
        private const int SeriesCount = 80;

        public SeriesSelectionExampleView()
        {
            InitializeComponent();
        }

        public void SeriesSelectionExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Create a number of DataSeries of type X=double, Y=double
            var allDataSeries = new IUniformXyDataSeries<double>[SeriesCount];

            var initialColor = Colors.Blue;

            // Suspend visual updates while we add N RenderableSeries
            using (sciChartSurface.SuspendUpdates())
            {
                // Add N data and renderable series
                for (int i = 0; i < SeriesCount; i++)
                {
                    AxisAlignment alignment = i % 2 == 0 ? AxisAlignment.Left : AxisAlignment.Right;

                    allDataSeries[i] = GenerateDataSeries(alignment, i);

                    var renderableSeries = new FastLineRenderableSeries {Stroke = initialColor};

                    renderableSeries.YAxisId = alignment.ToString();

                    // Assign DataSeries to RenderableSeries
                    renderableSeries.DataSeries = allDataSeries[i];

                    // Assign RenderableSeries to SciChartSurface
                    sciChartSurface.RenderableSeries.Add(renderableSeries);

                    // Colors are incremented for visual purposes only
                    int newR = initialColor.R == 255 ? 255 : initialColor.R + 5;
                    int newB = initialColor.B == 0 ? 0 : initialColor.B - 2;

                    initialColor = Color.FromArgb(255, (byte) newR, initialColor.G, (byte) newB);
                }
            }

            sciChartSurface.RenderableSeries[SeriesCount / 2].IsSelected = true;

            sciChartSurface.ZoomExtents();
        }

        private IUniformXyDataSeries<double> GenerateDataSeries(AxisAlignment axisAlignment, int index)
        {
            var dataSeries = new UniformXyDataSeries<double>
            {
                SeriesName = string.Format("Series {0}", index)
            };

            double gradient = axisAlignment == AxisAlignment.Right ? index : -index;
            double start = axisAlignment == AxisAlignment.Right ? 0.0 : 14000;

            dataSeries.Append(DataManager.Instance.GetStraightLineYData(gradient, start, SeriesPointCount));

            return dataSeries;
        }
    }
}
