using System.Collections.Generic;
using SciChart.Charting.Numerics.TickProviders;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace TicklinesUniformGridExample
{
    public class MillimeterDivisionsTickProvider : TickProvider<double>
    {
        private readonly double _majorTicksMillimetres;
        private readonly double _minorTicksMillimetres;
        public static SciChartSurface ActualVisual = null;

        public MillimeterDivisionsTickProvider(double majorTicksMillimetres, double minorTicksMillimetres)
        {
            _majorTicksMillimetres = majorTicksMillimetres;
            _minorTicksMillimetres = minorTicksMillimetres;
        }
        public override IList<double> GetMinorTicks(IAxisParams axis)
        {
            var parentAxis = axis as AxisBase;
            if (parentAxis == null) return new double[0];

            var axisSize = parentAxis.IsHorizontalAxis ? parentAxis.ActualWidth : parentAxis.ActualHeight;

            var matrixFromDevice = System.Windows.PresentationSource.FromVisual(parentAxis).
                CompositionTarget.
                TransformToDevice;

            double dpiFactor = 1 / matrixFromDevice.M11;
            double oneMM = dpiFactor * 5;

            double size = axisSize / (_minorTicksMillimetres * oneMM);
            return GenerateTicks((DoubleRange)axis.VisibleRange, size);
        }

        private static double[] GenerateTicks(DoubleRange tickRange, double size)
        {
            var ticks = new List<double>();

            var step = tickRange.Diff / size;
            for (int i = 0; i < size; i++)
            {
                ticks.Add(tickRange.Min + step * i);
            }

            return ticks.ToArray();
        }

        public override IList<double> GetMajorTicks(IAxisParams axis)
        {
            var parentAxis = axis as AxisBase;
            if (parentAxis == null) return new double[0];

            var axisSize = parentAxis.IsHorizontalAxis ? parentAxis.ActualWidth : parentAxis.ActualHeight;
            var matrixFromDevice = System.Windows.PresentationSource.FromVisual(parentAxis)
                .CompositionTarget
                .TransformToDevice;
            double dpiFactor = 1 / matrixFromDevice.M11;
            double oneMM = dpiFactor * 5;

            double size = axisSize / (_majorTicksMillimetres * oneMM);

            return GenerateTicks((DoubleRange)axis.VisibleRange, size);
        }
    }
}