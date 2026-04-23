// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CustomWaterfallNumericAxis.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.WaterfallChart
{
    public class CustomWaterfallNumericAxis : NumericAxis
    {
        public double offset { get; set; }
        public double min { get; set; }
        public double max { get; set; }

        public CustomWaterfallNumericAxis()
        {
            min = 0.0;
            max = 0.0;
        }

        public override double GetAxisOffset()
        {
            return base.GetAxisOffset() + offset;
        }

        protected override IRange CalculateDataRange()
        {
            return IsXAxis ? new DoubleRange(0.0, 15.0) : new DoubleRange(-50.0, 50.0);
        }

        public override IRange GetMaximumRange()
        {
            return (min.Equals(max)) ? new DoubleRange(min, max) : base.GetMaximumRange();
        }
    }
}
