// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// InterpolatingRangeSyncTransform.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.ChartSeries;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.PerformanceDemos2D.RacingTelemetryDashboard.ViewModels
{
    /// <summary>
    /// Converts between a local axis domain (e.g. time in seconds) and a canonical
    /// group domain (e.g. distance in meters) using piecewise-linear interpolation
    /// over paired lookup tables.
    /// </summary>
    public class InterpolatingRangeSyncTransform : IRangeSyncTransform
    {
        private readonly double[] _axisValues;
        private readonly double[] _groupValues;

        public InterpolatingRangeSyncTransform(double[] axisValues, double[] groupValues)
        {
            _axisValues = axisValues;
            _groupValues = groupValues;
        }

        public IRange ToGroupRange(IRange axisRange)
        {
            if (axisRange is DoubleRange r)
                return new DoubleRange(Interpolate(_axisValues, _groupValues, r.Min), Interpolate(_axisValues, _groupValues, r.Max));
            return axisRange;
        }

        public IRange FromGroupRange(IRange groupRange)
        {
            if (groupRange is DoubleRange r)
                return new DoubleRange(Interpolate(_groupValues, _axisValues, r.Min), Interpolate(_groupValues, _axisValues, r.Max));
            return groupRange;
        }

        /// <summary>
        /// Piecewise-linear interpolation over paired sorted arrays.
        /// Given two arrays of equal length where <paramref name="xs"/>[i] maps to <paramref name="ys"/>[i],
        /// finds the interval in <paramref name="xs"/> that contains <paramref name="x"/> using binary search,
        /// then linearly interpolates the corresponding value in <paramref name="ys"/>.
        /// Values outside the array bounds are clamped to the first/last element.
        /// </summary>
        /// <param name="xs">Source domain values (must be sorted ascending).</param>
        /// <param name="ys">Target domain values paired with <paramref name="xs"/>.</param>
        /// <param name="x">The value to look up in <paramref name="xs"/>.</param>
        /// <returns>The interpolated value in the <paramref name="ys"/> domain.</returns>
        public static double Interpolate(double[] xs, double[] ys, double x)
        {
            if (x <= xs[0]) return ys[0];
            if (x >= xs[xs.Length - 1]) return ys[ys.Length - 1];

            int lo = 0, hi = xs.Length - 1;
            while (hi - lo > 1)
            {
                int mid = (lo + hi) / 2;
                if (xs[mid] <= x) lo = mid; else hi = mid;
            }

            double t = (x - xs[lo]) / (xs[hi] - xs[lo]);
            return ys[lo] + t * (ys[hi] - ys[lo]);
        }
    }
}
