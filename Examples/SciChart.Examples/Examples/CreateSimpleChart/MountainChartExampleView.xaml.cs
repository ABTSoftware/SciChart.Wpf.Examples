// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// MountainChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    /// <summary>
    /// Interaction logic for MountainChartExample.xaml
    /// </summary>
    public partial class MountainChartExampleView : UserControl
    {
        public MountainChartExampleView()
        {
            InitializeComponent();
        }

        private void MountainChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Add a data series of type X=DateTime, Y=Double
            var series = new XyDataSeries<DateTime, double>();

            var prices = DataManager.Instance.GetPriceData(Instrument.Indu.Value, TimeFrame.Daily);

            // Append data to series. SciChart automatically redraws
            series.Append(prices.TimeData, prices.CloseData);

            mountainRenderSeries.DataSeries = series;

            sciChart.ZoomExtents();
        }
    }
}
