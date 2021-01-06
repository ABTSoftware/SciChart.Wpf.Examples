// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// RolloverFeedback.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.InspectDatapoints
{
    public partial class RolloverFeedback : UserControl
    {
        public RolloverFeedback()
        {
            InitializeComponent();

            Loaded += RolloverFeedbackLoaded;
        }

        private void RolloverFeedbackLoaded(object sender, RoutedEventArgs e)
        {
            sciChartSurface.YAxis.GrowBy = new DoubleRange(0.2, 0.2);

            var seriesA = new XyDataSeries<double, double>();
            var seriesB = new XyDataSeries<double, double>();
            var seriesC = new XyDataSeries<double, double>();

            seriesA.SeriesName = "Sinewave A";
            seriesB.SeriesName = "Sinewave B";
            seriesC.SeriesName = "Sinewave C";

            double count = 100.0;
            double k = 2 * Math.PI / 30.0;
            for (int i = 0; i < (int)count; i++)
            {
                var phi = k * i;
                seriesA.Append(i, (1.0 + i / count) * Math.Sin(phi));
                seriesB.Append(i, (0.5 + i / count) * Math.Sin(phi));
                seriesC.Append(i, (i / count) * Math.Sin(phi));
            }

            sciChartSurface.RenderableSeries[0].DataSeries = seriesA;
            sciChartSurface.RenderableSeries[1].DataSeries = seriesB;
            sciChartSurface.RenderableSeries[2].DataSeries = seriesC;
        }
    }
}
