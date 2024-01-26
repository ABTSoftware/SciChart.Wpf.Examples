﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// VerticalCharts.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    /// Interaction logic for VerticalCharts.xaml
    /// </summary>
    public partial class VerticalCharts : UserControl
    {
        public VerticalCharts()
        {
            InitializeComponent();
        }

        private void VerticalXAxis_OnLoaded(object sender, RoutedEventArgs e)
        {
            var leftDataSeries0 = new UniformXyDataSeries<double>();
            var leftDataSeries1 = new UniformXyDataSeries<double>();

            var rightDataSeries0 = new UniformXyDataSeries<double>();
            var rightDataSeries1 = new UniformXyDataSeries<double>();

            var random = new Random(0);

            for (int i = 0; i < 20; i++)
            {
                leftDataSeries0.Append(random.Next(10));
                leftDataSeries1.Append(random.Next(10));

                rightDataSeries0.Append(random.Next(10));
                rightDataSeries1.Append(random.Next(10));
            }

            sciChartLeft.RenderableSeries[0].DataSeries = leftDataSeries0;
            sciChartLeft.RenderableSeries[1].DataSeries = leftDataSeries1;

            sciChartRight.RenderableSeries[0].DataSeries = rightDataSeries0;
            sciChartRight.RenderableSeries[1].DataSeries = rightDataSeries1;
        }
    }
}