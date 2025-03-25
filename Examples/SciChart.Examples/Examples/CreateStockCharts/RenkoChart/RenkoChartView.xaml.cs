// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// RenkoChartView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;
using SciChart.Charting.Visuals.Axes.IndexDataProviders;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateStockCharts.RenkoChart
{
    public partial class RenkoChartView : UserControl
    {
        public RenkoChartView()
        {
            InitializeComponent();
        }

        private void CandlesticksWithLines_Loaded(object sender, RoutedEventArgs e)
        {
            // Prices are in the format Time, Open, High, Low, Close (all IList)
            var prices = DataManager.Instance.GetPriceData(Instrument.EurUsd.Value, TimeFrame.Daily);

            // Create multiple DataSeries to store OHLC candlestick data, XY moving average data and Volume column data
            var priceOhlcData = new OhlcDataSeries<DateTime, double>();
            priceOhlcData.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);

            // Renko series is rendered as a Candlestick Chart so we append OHLC data
            var renkoData = priceOhlcData.AggregateByRenko(0.03, (start, end) =>
            {
                var aggregatedVolume = 0d;

                // Start inclusive, End exclusive
                for (int i = start; i < end; i++)
                {
                    aggregatedVolume += prices.VolumeData[i];
                }
                return new VolumeMetadata(aggregatedVolume, prices.CloseData[end - 1] > prices.OpenData[start]);

            }) as IOhlcDataSeries<DateTime, double>;
            RenkoSeries.DataSeries = renkoData;

            // Volume and Volume SMA series 
            var volumeData = new XyDataSeries<DateTime, double>();
            volumeData.Append(renkoData.XValues, renkoData.Metadata.Select(x => ((VolumeMetadata)x).Volume), renkoData.Metadata);
            VolumeSeries.DataSeries = volumeData;
            var smaLength = 20;
            SmaVolumeSeries.DataSeries = volumeData.ToMovingAverage(smaLength);

            // Price SMA series and Bollinger Bands
            var smaPriceDataSeries = renkoData.ToMovingAverage(smaLength) as IXyDataSeries<DateTime, double>;
            SmaPriceSeries.DataSeries = smaPriceDataSeries;
            BollingerBandsSeries.DataSeries = new BollingerBandsFilter(renkoData, smaLength, 2).FilteredDataSeries as XyyDataSeries<DateTime, double>;

            // Configure X-Axis index base and background shading
            IndexXAxis.IndexDataProvider = new DataSeriesIndexDataProvider(renkoData);
            TrendBandsChartModifier.PriceDataSeries = renkoData;
            TrendBandsChartModifier.MovingAverageDataSeries = smaPriceDataSeries;
            TrendBandsChartModifier.MovingAverageLength = smaLength;

            // Zoom to data extents
            sciChart.ZoomExtents();
        }
    }
}
