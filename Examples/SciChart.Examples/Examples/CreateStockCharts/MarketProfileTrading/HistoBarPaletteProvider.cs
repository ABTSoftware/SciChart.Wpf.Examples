using SciChart.Charting.Model;
using SciChart.Charting.Visuals.RenderableSeries;
using System.Windows;

namespace SciChart.Examples.Examples.CreateStockCharts.MarketProfileTrading
{
    public class HistoBarPaletteProvider : IHistoBarPaletteProvider
    {
        public Style OverriddenAskBarStyle { get; set; }
        public Style OverriddenBidBarStyle { get; set; }

        public double VolumeThreshold { get; set; }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries) { }

        public Style OverrideBarStyle(IRenderableSeries rSeries, double value, BidOrAsk barType, HistogramMode histogramMode)
        {
            Style barStyle = null;

            if (value >= VolumeThreshold)
            {
                barStyle = barType == BidOrAsk.Ask ? OverriddenAskBarStyle : OverriddenBidBarStyle;
            }

            return barStyle;
        }
    }
}
