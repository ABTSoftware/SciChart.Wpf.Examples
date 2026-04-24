// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// GainCoverageChartsView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.RenderableSeries;
using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Antenna3DRadiationPattern
{
    /// <summary>
    /// Displays two vertically stacked charts:
    ///   - Top: E-plane and H-plane gain cut lines (fan chart)
    ///   - Bottom: full (φ × θ) gain coverage heatmap with draggable cross-section lines
    /// Both charts share a synchronized mouse event group.
    /// </summary>
    public partial class GainCoverageChartsView : UserControl
    {
        private double[,] _gain;
        private double _thetaStepDeg;
        private double _phiStepDeg;

        private const double AnnotationThicknessNormal = 1.5;
        private const double AnnotationThicknessSelected = 3.0;

        public GainCoverageChartsView()
        {
            InitializeComponent();
            HookAnnotationSelection(thetaLine, thetaCutSeries);
            HookAnnotationSelection(phiLine, phiCutSeries);
        }

        public void Populate(double[,] gain, double thetaStepDeg, double phiStepDeg,
                             double initialThetaDeg, double initialPhiDeg)
        {
            _gain = gain;
            _thetaStepDeg = thetaStepDeg;
            _phiStepDeg = phiStepDeg;

            PopulateGainHeatmap(gain, thetaStepDeg, phiStepDeg);
            UpdatePhiCut(initialPhiDeg);
            UpdateThetaCut(initialThetaDeg);
        }

        /// <summary>
        /// Updates the E-plane (elevation) cut line: gain vs θ at a fixed φ.
        /// </summary>
        public void UpdatePhiCut(double phiDeg)
        {
            if (_gain == null) return;

            int thetaCount = _gain.GetLength(0);
            int phiIdx = (int)Math.Round(phiDeg / _phiStepDeg) % _gain.GetLength(1);

            var series = new XyDataSeries<double, double> { SeriesName = $"E-plane (φ = {phiDeg:F0}°)" };
            for (int ti = 0; ti < thetaCount; ti++)
                series.Append(ti * _thetaStepDeg, AntennaMetrics.ToDbi(_gain[ti, phiIdx]));

            phiCutSeries.DataSeries = series;
        }

        /// <summary>
        /// Updates the H-plane (azimuth) cut line: gain vs φ at a fixed θ.
        /// </summary>
        public void UpdateThetaCut(double thetaDeg)
        {
            if (_gain == null) return;

            int thetaCount = _gain.GetLength(0);
            int phiCount = _gain.GetLength(1);
            int thetaIdx = (int)Math.Round(thetaDeg / _thetaStepDeg);
            thetaIdx = Math.Max(0, Math.Min(thetaIdx, thetaCount - 1));

            var series = new XyDataSeries<double, double> { SeriesName = $"H-plane (θ = {thetaDeg:F0}°)" };
            for (int pi = 0; pi < phiCount; pi++)
                series.Append(pi * _phiStepDeg, AntennaMetrics.ToDbi(_gain[thetaIdx, pi]));

            thetaCutSeries.DataSeries = series;
        }

        /// <summary>
        /// Builds the gain heatmap: converts normalised gain to dBi and creates
        /// a UniformHeatmapDataSeries with φ on X and θ on Y.
        /// </summary>
        private void PopulateGainHeatmap(double[,] gain, double thetaStepDeg, double phiStepDeg)
        {
            int thetaCount = gain.GetLength(0);
            int phiCount = gain.GetLength(1);

            var dbiValues = new double[thetaCount, phiCount];
            for (int ti = 0; ti < thetaCount; ti++)
                for (int pi = 0; pi < phiCount; pi++)
                    dbiValues[ti, pi] = AntennaMetrics.ToDbi(gain[ti, pi]);

            gainHeatmapSeries.DataSeries =
                new UniformHeatmapDataSeries<double, double, double>(dbiValues, 0.0, phiStepDeg, 0.0, thetaStepDeg)
                {
                    SeriesName = "Gain"
                };
        }

        /// <summary>
        /// Links an annotation's selection state to a fan chart series:
        /// selecting the annotation thickens it and marks the series as selected.
        /// </summary>
        private void HookAnnotationSelection(LineAnnotationWithLabelsBase annotation, BaseRenderableSeries series)
        {
            annotation.Selected += (s, e) =>
            {
                annotation.StrokeThickness = AnnotationThicknessSelected;
                series.IsSelected = true;
            };
            annotation.Unselected += (s, e) =>
            {
                annotation.StrokeThickness = AnnotationThicknessNormal;
                series.IsSelected = false;
            };
        }

        /// <summary>
        /// Binds the heatmap cross-section annotations to the toolbar sliders
        /// so dragging either updates the corresponding slider value.
        /// </summary>
        public void BindSliders(RangeBase thetaSlider, RangeBase phiSlider)
        {
            thetaLine.SetBinding(AnnotationBase.Y1Property,
                new Binding("Value") { Source = thetaSlider, Mode = BindingMode.TwoWay });
            phiLine.SetBinding(AnnotationBase.X1Property,
                new Binding("Value") { Source = phiSlider, Mode = BindingMode.TwoWay });
        }
    }
}
