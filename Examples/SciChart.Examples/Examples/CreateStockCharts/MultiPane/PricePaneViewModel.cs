// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// PricePaneViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateStockCharts.MultiPane
{
    public class PricePaneViewModel : BaseChartPaneViewModel
    {
        public PricePaneViewModel(CreateMultiPaneStockChartsViewModel parentViewModel, PriceSeries prices)
            : base(parentViewModel)
        {
            // We can add Series via the SeriesSource API, where SciStockChart or SciChartSurface bind to IEnumerable<IChartSeriesViewModel>
            // Alternatively, you can delcare your RenderableSeries in the SciStockChart and just bind to DataSeries
            // A third method (which we don't have an example for yet, but you can try out) is to create an Attached Behaviour to transform a collection of IDataSeries into IRenderableSeries

            // Add the main OHLC chart
            var stockPrices = new OhlcDataSeries<DateTime, double>() { SeriesName = "OHLC" };
            stockPrices.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);
            ChartSeriesViewModels.Add(new CandlestickRenderableSeriesViewModel
            {
                
                DataSeries = stockPrices,
                AntiAliasing = false,
                
            });

            // Add a moving average
            var maLow = new XyDataSeries<DateTime, double>() { SeriesName = "Low Line" };
            maLow.Append(prices.TimeData, prices.CloseData.MovingAverage(50));
            ChartSeriesViewModels.Add(new LineRenderableSeriesViewModel
            {
                DataSeries = maLow,
                StyleKey = "LowLineStyle",
            });

            // Add a moving average
            var maHigh = new XyDataSeries<DateTime, double>() { SeriesName = "High Line" };
            maHigh.Append(prices.TimeData, prices.CloseData.MovingAverage(200));
            ChartSeriesViewModels.Add(new LineRenderableSeriesViewModel
            {
                DataSeries = maHigh,
                StyleKey = "HighLineStyle",
            });

            YAxisTextFormatting = "$0.0000";
        }
    }
}
