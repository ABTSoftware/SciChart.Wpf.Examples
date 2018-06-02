using System;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace SciChart.Sandbox.Examples.StripChart
{
    public class RelativeTimeLabelProvider : NumericLabelProvider
    {
        // Update me with current latest time every time you append data!
        public double CurrentTime { get; set; }

        public override string FormatCursorLabel(IComparable dataValue)
        {
            // dataValue is the actual axis label value, e.g. comes from DataSeries.XValues
            double value = (double)dataValue;
            Double relative = (CurrentTime - value);

            return String.Format("t-{0:0.0}", relative);
        }

        public override string FormatLabel(IComparable dataValue)
        {
            // dataValue is the actual axis label value, e.g. comes from DataSeries.XValues
            double value = (double)dataValue;
            Double relative = (CurrentTime - value);

            return String.Format("t-{0:0.0}", relative);
        }
    }
}