using System.Windows;
using SciChart.Charting.Model;
using SciChart.Charting.Visuals.RenderableSeries;


namespace SciChart.Sandbox.Examples.MarketProfileTradingExample
{
    public class HistoBarPaletteProvider: IHistoBarPaletteProvider
    {
        public Style OverriddenBarStyle { get; set; }

        public double VolumeThreshold { get; set; }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries){}

        public Style OverrideBarStyle(IRenderableSeries rSeries, double value, BidOrAsk barType, HistogramMode histogramMode)
        {
            Style barStyle = null;

            if (histogramMode == HistogramMode.VolumeLadder)
            {
                if (value >= VolumeThreshold)
                {
                    barStyle = OverriddenBarStyle;
                }
            }

            return barStyle;
        }
    }
}
