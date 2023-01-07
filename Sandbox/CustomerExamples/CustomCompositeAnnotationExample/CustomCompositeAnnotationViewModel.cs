using System;
using System.Collections.Generic;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace CustomCompositeAnnotationExampleExample
{
    public class CustomCompositeAnnotationViewModel : BaseViewModel
    {
        public CustomCompositeAnnotationViewModel()
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

            var prices = SciChart.Examples.ExternalDependencies.Data.DataManager.Instance.GetPriceData(Instrument.Indu.Value, TimeFrame.Daily);
            stockPrices.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);

            return stockPrices;
        }
    }
}