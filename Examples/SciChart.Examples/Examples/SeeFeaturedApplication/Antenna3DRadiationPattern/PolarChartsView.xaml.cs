// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// PolarChartsView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using System;
using System.Windows.Controls;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Antenna3DRadiationPattern
{
    /// <summary>
    /// Displays E-plane (elevation) and H-plane (azimuth) polar gain cuts.
    /// Each polar chart shows gain in dBi around a full 360° circle for a
    /// single slice through the 3-D radiation pattern.
    /// </summary>
    public partial class PolarChartsView : UserControl
    {
        private double[,] _gain;
        private double _thetaStepDeg;
        private double _phiStepDeg;

        public PolarChartsView()
        {
            InitializeComponent();
        }

        public void Populate(double[,] gain, double thetaStepDeg, double phiStepDeg,
                             double initialThetaDeg, double initialPhiDeg)
        {
            _gain = gain;
            _thetaStepDeg = thetaStepDeg;
            _phiStepDeg = phiStepDeg;

            UpdateElevationCut(initialPhiDeg);
            UpdateAzimuthCut(initialThetaDeg);
        }

        /// <summary>
        /// Builds a full 360° elevation (E-plane) polar trace at a fixed φ.
        /// The front half (0°–180°) uses the gain at φ, and the back half
        /// (180°–360°) mirrors the gain at the opposite azimuth (φ + 180°),
        /// producing the classic "front + back lobe" polar pattern.
        /// </summary>
        public void UpdateElevationCut(double phiDeg)
        {
            if (_gain == null) return;

            int thetaCount = _gain.GetLength(0);
            int phiCount = _gain.GetLength(1);
            double thetaMaxDeg = (thetaCount - 1) * _thetaStepDeg;
            double phiMaxDeg = phiCount * _phiStepDeg;

            // Forward and backward azimuth indices for the selected φ plane
            int phiIdx = (int)Math.Round(phiDeg / _phiStepDeg) % phiCount;
            int phiIdxBack = (int)Math.Round((phiDeg + thetaMaxDeg) / _phiStepDeg) % phiCount;

            var series = new XyDataSeries<double, double> { SeriesName = "E-plane" };

            // Front half: θ sweeps 0° → 180° at φ
            for (int ti = 0; ti < thetaCount; ti++)
                series.Append(ti * _thetaStepDeg, AntennaMetrics.ToDbi(_gain[ti, phiIdx]));

            // Back half: θ sweeps 180° → 360° using the opposite azimuth (φ + 180°)
            for (int ti = thetaCount - 1; ti >= 0; ti--)
                series.Append(phiMaxDeg - ti * _thetaStepDeg, AntennaMetrics.ToDbi(_gain[ti, phiIdxBack]));

            elevationSeries.DataSeries = series;
            elevationTitleAxis.AxisTitle = $"E-plane (φ = {phiDeg:F0}°)";
        }

        /// <summary>
        /// Builds a 360° azimuth (H-plane) polar trace at a fixed θ.
        /// Sweeps φ from 0° to 360° at the given elevation angle, with a
        /// closing point to complete the circle.
        /// </summary>
        public void UpdateAzimuthCut(double thetaDeg)
        {
            if (_gain == null) return;

            int thetaCount = _gain.GetLength(0);
            int phiCount = _gain.GetLength(1);
            double phiMaxDeg = phiCount * _phiStepDeg;

            int thetaIdx = (int)Math.Round(thetaDeg / _thetaStepDeg);
            thetaIdx = Math.Max(0, Math.Min(thetaIdx, thetaCount - 1));

            var series = new XyDataSeries<double, double> { SeriesName = "H-plane" };
            for (int pi = 0; pi < phiCount; pi++)
                series.Append(pi * _phiStepDeg, AntennaMetrics.ToDbi(_gain[thetaIdx, pi]));

            // Close the circle by repeating the first point at φ = 360°
            series.Append(phiMaxDeg, AntennaMetrics.ToDbi(_gain[thetaIdx, 0]));

            azimuthSeries.DataSeries = series;
            azimuthTitleAxis.AxisTitle = $"H-plane (θ = {thetaDeg:F0}°)";
        }

    }
}
