// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// MixedCharts.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    /// <summary>
    /// Interaction logic for MixedCharts.xaml
    /// </summary>
    public partial class MixedCharts : UserControl
    {
        public MixedCharts()
        {
            InitializeComponent();
        }

        private void MixedCharts_OnLoaded(object sender, RoutedEventArgs e)
        {
            var leftDataSeries0 = new UniformXyDataSeries<double>();
            var leftDataSeries1 = new UniformXyDataSeries<double>();

            var rightDataSeries0 = new XyDataSeries<DateTime, double>();
            var rightDataSeries1 = new XyDataSeries<DateTime, double>();

            var random = new Random();

            for (int i = 0; i < 20; i++)
            {
                leftDataSeries0.Append(random.Next(10));
                leftDataSeries1.Append(random.Next(10));

                rightDataSeries0.Append(DateTime.Now.AddHours(i), random.Next(10));
                rightDataSeries1.Append(DateTime.Now.AddHours(i), random.Next(10));
            }

            sciChart.RenderableSeries[0].DataSeries = leftDataSeries0;
            sciChart.RenderableSeries[1].DataSeries = leftDataSeries1;

            sciChart.RenderableSeries[2].DataSeries = rightDataSeries0;
            sciChart.RenderableSeries[3].DataSeries = rightDataSeries1;

            sciChart.ZoomExtents();
        }
    }
}
