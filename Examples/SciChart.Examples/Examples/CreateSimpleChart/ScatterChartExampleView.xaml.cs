﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ScatterChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    /// <summary>
    /// Interaction logic for ScatterChartExampleView.xaml
    /// </summary>
    public partial class ScatterChartExampleView : UserControl
    {
        public ScatterChartExampleView()
        {
            InitializeComponent();
        }

        private void ScatterChartExampleView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Create a data series of type X=double, Y=double
            var dataSeries = new UniformXyDataSeries<double>(0d, 0.05);

            // Append data to series. SciChart automatically redraws
            dataSeries.Append(DataManager.Instance.GetDampedSinewaveYData(1.0, 0.02, 200));

            scatterRenderSeries.DataSeries = dataSeries;

            sciChart.ZoomExtents();
        }
    }
}
