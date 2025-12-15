// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// TrendBandsChartModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Axes.AxisBandProviders;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.CreateStockCharts.RenkoChart
{
    public class TrendBandsChartModifier : ChartModifierBase
    {
        private DateTimeAxisBandsProvider _axisBandsProvider;
        private IOhlcDataSeries<DateTime, double> _priceDataSeries;
        private IXyDataSeries<DateTime, double> _movingAverageDataSeries;
        private int _movingAverageLength;

        public Color UpTrendColor { get; set; } = Color.FromArgb(30, 0, 250, 0);
        public Color DownTrendColor { get; set; } = Color.FromArgb(30, 250, 0, 0);

        public IOhlcDataSeries<DateTime, double> PriceDataSeries
        {
            get => _priceDataSeries;
            set
            {
                _priceDataSeries = value;
                CalculateTrendBands();
            }
        }

        public IXyDataSeries<DateTime, double> MovingAverageDataSeries
        {
            get => _movingAverageDataSeries;
            set
            {
                _movingAverageDataSeries = value;
                CalculateTrendBands();
            }
        }

        public int MovingAverageLength
        {
            get => _movingAverageLength;
            set
            {
                _movingAverageLength = value;
                CalculateTrendBands();
            }
        }

        private void CalculateTrendBands()
        {
            if (PriceDataSeries == null || MovingAverageDataSeries == null || PriceDataSeries.Count <= 1 || MovingAverageLength <= 0) return;

            _axisBandsProvider = new DateTimeAxisBandsProvider();

            var count = MovingAverageDataSeries.Count;
            var maLength = MovingAverageLength;
            var startIndex = maLength - 1;

            var maYValues = MovingAverageDataSeries.YValues;
            var maXValues = MovingAverageDataSeries.XValues;
            var priceCloseValues = PriceDataSeries.CloseValues;

            var maY = maYValues[startIndex];
            var priceY = priceCloseValues[startIndex];

            var halfBarSpan = (maXValues[startIndex] - maXValues[startIndex - 1]).TotalDays / 2d;
            var rangeStart = maXValues[startIndex].AddDays(-halfBarSpan);

            // Look at the MovingAverage data to highlight ranges with up and down trend
            var isUpTrend = priceY > maY;
            for (int i = startIndex + 1; i < count; ++i)
            {
                maY = maYValues[i];
                priceY = priceCloseValues[i];
                var isTrendChange = (priceY <= maY && isUpTrend) || (priceY >= maY && !isUpTrend);

                var isLastBar = i == count - 1;
                if (isTrendChange || isLastBar)
                {
                    // End the previous interval right before the current bar
                    halfBarSpan = (maXValues[i] - maXValues[i - 1]).TotalDays / 2d;
                    var rangeEnd = maXValues[i].AddDays(isLastBar ? halfBarSpan : -halfBarSpan);
                    var trendBand = new AxisBandInfo<DateRange>(new DateRange(rangeStart, rangeEnd), isUpTrend ? UpTrendColor : DownTrendColor);

                    // Add a single highlighted interval
                    _axisBandsProvider.AxisBands.Add(trendBand);

                    rangeStart = rangeEnd;
                    isUpTrend = !isUpTrend;
                }
            }

            // Attach the custom AxisBandsProvider to the X-Axis
            if (ParentSurface?.XAxis is IndexDateTimeAxis xAxis)
            {
                xAxis.DrawMajorBands = false;
                xAxis.AxisBandsProvider = _axisBandsProvider;
            }
        }
    }
}
