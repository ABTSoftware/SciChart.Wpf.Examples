// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UsePaletteProviderViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.StyleAChart.UsePaletteProvider
{
    public class UsePaletteProviderViewModel : BaseViewModel
    {
        private readonly List<IRenderableSeriesViewModel> _chartSeriesViewModels;

        public UsePaletteProviderViewModel()
        {
            // In the MvvmSciChartSurface, we don't have to create a dataset. Just create DataSeries and append as usual
            var dataSeries0 = new XyDataSeries<DateTime, double>();
            var dataSeries1 = new XyDataSeries<DateTime, double>();
            var dataSeries2 = new OhlcDataSeries<DateTime, double>();
            var dataSeries3 = new OhlcDataSeries<DateTime, double>();
            var dataSeries4 = new XyDataSeries<DateTime, double>();
            var dataSeries5 = new XyDataSeries<DateTime, double>();

            var dataSource = DataManager.Instance;

            // Prices are in the format Time, Open, High, Low, Close (all IList)            
            var prices = dataSource.GetPriceData(Instrument.Indu.Value, TimeFrame.Daily);

            var dataOffset = -1000;

            // Append data to series             
            dataSeries0.Append(prices.TimeData, dataSource.Offset(prices.OpenData, dataOffset * 2));
            dataSeries1.Append(prices.TimeData, dataSource.Offset(prices.OpenData, -dataOffset));
            dataSeries2.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);
            dataSeries3.Append(prices.TimeData, dataSource.Offset(prices.OpenData, dataOffset),
                               dataSource.Offset(prices.HighData, dataOffset),
                               dataSource.Offset(prices.LowData, dataOffset),
                               dataSource.Offset(prices.CloseData, dataOffset));
            dataSeries4.Append(prices.TimeData, dataSource.Offset(prices.CloseData, dataOffset*3));
            dataSeries5.Append(prices.TimeData, dataSource.Offset(prices.OpenData, dataOffset * 2.5));

            // Add the DataSeries / RenderableSeries to the ChartSeriesViewModel collection
            // These are paired so if you want to remove the series, just remove the viewmodel (it will remove the data and renderable series)
            // Or, if you want to change the RenderableSeries properties, just update the RenderSeries on the ChartSeriesViewModel

            var mountainSeriesVm = new MountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries0,
                StyleKey = "MountainSeriesStyle"
            };
            var lineSeriesVm = new LineRenderableSeriesViewModel
            {
                DataSeries = dataSeries1,
                StyleKey = "LineSeriesStyle"
            };

            var ohlcSeriesVm = new OhlcRenderableSeriesViewModel
            {
                DataSeries = dataSeries2,
                StyleKey = "OhlcSeriesStyle"
            };

            var candlestickSeriesVm = new CandlestickRenderableSeriesViewModel
            {
                DataSeries = dataSeries3,
                StyleKey = "CandlestickSeriesStyle"
            };

            var columnSeriesVm = new ColumnRenderableSeriesViewModel
            {
                DataSeries = dataSeries4,
                StyleKey = "ColumnSeriesStyle"
            };

            var scatterSeriesVm = new XyScatterRenderableSeriesViewModel
            {
                DataSeries = dataSeries5,
                StyleKey = "ScatterSeriesStyle"
            };

            _chartSeriesViewModels = new List<IRenderableSeriesViewModel>
            {
                mountainSeriesVm,
                lineSeriesVm,
                ohlcSeriesVm,
                candlestickSeriesVm,
                columnSeriesVm,
                scatterSeriesVm
            };
        }

        public List<IRenderableSeriesViewModel> ChartSeriesViewModels
        {
            get { return _chartSeriesViewModels; }
        }
    }
}
