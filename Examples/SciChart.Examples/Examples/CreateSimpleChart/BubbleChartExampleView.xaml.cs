// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// BubbleChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries.Animations;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    /// <summary>
    /// Interaction logic for BubbleChartExampleView.xaml
    /// </summary>
    public partial class BubbleChartExampleView : UserControl
    {
        public BubbleChartExampleView()
        {
            InitializeComponent();
            sciChart.Loaded += OnSciChartLoaded;
        }
        private void OnSciChartLoaded(object sender, RoutedEventArgs e)
        {
            // Add a data series to contain Xyy data. We want to use X,Y = position and Y1 = trade size
            var dataSeries = new XyzDataSeries<DateTime, double, double>();

            // Load the TradeTicks.csv file
            var tradeDataSource = DataManager.Instance.GetTradeticks().ToArray();

            // Append data to series. SciChart automatically redraws
            dataSeries.Append(tradeDataSource.Select(x => x.TradeDate),
                          tradeDataSource.Select(x => x.TradePrice),
                          tradeDataSource.Select(x => x.TradeSize));

            // XyzDataSeries is shared across two RenderableSeries
            //  - FastLineRenderableSeries chooses X,Y value to draw
            //  - FastBubbleRenderableSeries chooses X,Y value for position, Z for size
            lineSeries.DataSeries = dataSeries as IXyzDataSeries;
            bubbleSeries.DataSeries = dataSeries;

            sciChart.ZoomExtents();
        }
    }
}
