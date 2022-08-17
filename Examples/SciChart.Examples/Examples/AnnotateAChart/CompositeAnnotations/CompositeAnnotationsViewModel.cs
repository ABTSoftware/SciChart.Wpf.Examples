// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CompositeAnnotationsViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations
{
    public class CompositeAnnotationsViewModel : BaseViewModel
    {
        public CompositeAnnotationsViewModel()
        {
            RenderableSeriesViewModels = new List<IRenderableSeriesViewModel>
            {
                new CandlestickRenderableSeriesViewModel {DataSeries = GetPriceDataSeries()}
            };
        }

        public List<IRenderableSeriesViewModel> RenderableSeriesViewModels { get; set; }

        private IOhlcDataSeries GetPriceDataSeries()
        {
            var stockPrices = new OhlcDataSeries<DateTime, double>();

            var prices = DataManager.Instance.GetPriceData(Instrument.Indu.Value, TimeFrame.Daily);
            stockPrices.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);

            return stockPrices;
        }
    }
}