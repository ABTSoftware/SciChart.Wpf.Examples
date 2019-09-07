using System;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace SciChart.Sandbox.Examples.SweepingEcgSeries
{
    public class ModuloLabelProvider : LabelProviderBase
    {
        private string _stringFormat;

        public ModuloLabelProvider()
        {
            // Default value
            Divisor = 1;
        }

        public double Divisor { get; set; }

        public override void Init(IAxisCore parentAxis)
        {
            base.Init(parentAxis);

            _stringFormat = parentAxis.TextFormatting;
        }

        public override string FormatLabel(IComparable dataValue)
        {
            double seconds = (double)Convert.ChangeType(dataValue, typeof (double));

            double modulus = seconds%Divisor;

            return string.Format(_stringFormat, modulus);
        }

        public override string FormatCursorLabel(IComparable dataValue)
        {
            // TODO: Cursor specific formatting if required
            return FormatLabel(dataValue);
        }
    }
}
