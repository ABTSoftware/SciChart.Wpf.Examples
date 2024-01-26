// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateSimpleStockChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.CreateStockCharts
{
    public partial class CreateSimpleStockChart : UserControl
    {
        public CreateSimpleStockChart()
        {
            InitializeComponent();
        }

        private void CreateSciTradeChartLoaded(object sender, RoutedEventArgs e)
        {
            var dataSeries = new OhlcDataSeries<DateTime, double>()
            {
                // Note: SeriesName is needed to display the legend correctly. 
                SeriesName = "Price Data"
            };

            // Append price data
            var prices = DataManager.Instance.GetPriceData(Instrument.Indu.Value, TimeFrame.Daily);
            dataSeries.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);

            // Set BarTimeFrame = 3600 seconds (time of one bar in the input data)
            StockChart.BarTimeFrame = TimeSpan.FromHours(1).TotalSeconds;

            StockChart.RenderableSeries[0].DataSeries = dataSeries;
        }
    }
}
