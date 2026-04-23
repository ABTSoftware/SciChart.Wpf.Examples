// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// HistogramBarViewportManager.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;
using System;

namespace SciChart.Examples.Examples.CreateStockCharts.MarketProfileTrading
{
    class HistogramBarViewportManager : DefaultViewportManager
    {
        private IAxis _priceYAxis;

        public override void AttachSciChartSurface(ISciChartSurface scs)
        {
            base.AttachSciChartSurface(scs);
            ParentSurface = scs;

            // Set Autorange behavior for the Price Y Axis
            _priceYAxis = scs.YAxes.GetAxisById("PriceYAxis");
            _priceYAxis.AutoRange = AutoRange.Always;
        }

        public override void DetachSciChartSurface()
        {
            // Reset Autorange behavior for the Price Y Axis
            _priceYAxis.AutoRange = AutoRange.Once;
            _priceYAxis = null;
            ParentSurface = null;

            base.DetachSciChartSurface();
        }

        public ISciChartSurface ParentSurface { get; private set; }

        protected override IRange OnCalculateNewXRange(IAxis xAxis)
        {
            var currentVisibleRange = xAxis.VisibleRange;
            // Getting X-Range of sorted data is quick,
            // so it can be done every frame
            var dataXRange = xAxis.DataRange;

            return new DateRange((DateTime)currentVisibleRange.Min, (DateTime)dataXRange.Max);
        }
    }
}