// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HollowCandlesPaletteProvider.xaml.cs is part of the SCICHART® Examples. Permission is
// hereby granted to modify, create derivative works, distribute and publish any part of
// this source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    public class HollowSolidPaletteProvider : IStrokePaletteProvider, IFillPaletteProvider
    {
        private IOhlcDataSeries<DateTime, double> _dataSeries;

        private Brush _candleBrush;

        public Brush FillUp { get; private set; }
        public Brush FillDown { get; private set; }
        public Brush FillNeutral { get; set; }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
            _dataSeries = rSeries.DataSeries as IOhlcDataSeries<DateTime, double>;

            if (rSeries is FastCandlestickRenderableSeries candlestickSeries)
            {
                FillUp = new SolidColorBrush(candlestickSeries.StrokeUp);

                FillDown = new SolidColorBrush(candlestickSeries.StrokeDown);
            }
        }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            _candleBrush = GetBrush(index);

            return _candleBrush.ExtractColor();
        }

        public Brush OverrideFillBrush(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            var open = _dataSeries.OpenValues[index];
            var close = _dataSeries.CloseValues[index];

            return close >= open ? Brushes.Transparent : _candleBrush;
        }

        private Brush GetBrush(int index)
        {
            var close = _dataSeries.CloseValues[index];
            var prevClose = index > 0 ? _dataSeries.CloseValues[index - 1] : double.MinValue;

            if (close > prevClose)
                return FillUp;

            if (close == prevClose)
                return FillNeutral;

            return FillDown;
        }
    }
}
