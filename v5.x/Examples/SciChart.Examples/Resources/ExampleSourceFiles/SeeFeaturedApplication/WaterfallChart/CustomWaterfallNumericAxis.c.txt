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
