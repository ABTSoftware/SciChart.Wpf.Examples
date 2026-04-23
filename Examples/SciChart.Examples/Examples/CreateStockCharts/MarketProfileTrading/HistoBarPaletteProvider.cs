// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// HistoBarPaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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
