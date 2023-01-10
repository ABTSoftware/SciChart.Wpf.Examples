// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CandlestickWithLinesView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    public partial class CandlestickWithLinesView : UserControl
    {
        public CandlestickWithLinesView()
        {
            InitializeComponent();
        }

        private void CandlesticksWithLines_Loaded(object sender, RoutedEventArgs e)
        {
            // Create multiple DataSeries to store OHLC candlestick data, and Xy moving average data
            var dataSeries0 = new OhlcDataSeries<DateTime, double>();
            var dataSeries1 = new XyDataSeries<DateTime, double>();
            var dataSeries2 = new XyDataSeries<DateTime, double>();
            var dataSeries3 = new XyDataSeries<DateTime, double>();

            // Prices are in the format Time, Open, High, Low, Close (all IList)
            var prices = DataManager.Instance.GetPriceData(Instrument.Indu.Value, TimeFrame.Daily);            

            // Append data to series. 
            // First series is rendered as a Candlestick Chart so we append OHLC data
            dataSeries0.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);

            // Subsequent series append moving average of the close prices
            dataSeries1.Append(prices.TimeData, DataManager.Instance.ComputeMovingAverage(prices.CloseData, 100));
            dataSeries2.Append(prices.TimeData, DataManager.Instance.ComputeMovingAverage(prices.CloseData, 50));
            dataSeries3.Append(prices.TimeData, DataManager.Instance.ComputeMovingAverage(prices.CloseData, 20));

            sciChart.RenderableSeries[0].DataSeries = dataSeries0;
            sciChart.RenderableSeries[1].DataSeries = dataSeries1;
            sciChart.RenderableSeries[2].DataSeries = dataSeries2;
            sciChart.RenderableSeries[3].DataSeries = dataSeries3;
            
            sciChart.ZoomExtents();
        }
    }
}
