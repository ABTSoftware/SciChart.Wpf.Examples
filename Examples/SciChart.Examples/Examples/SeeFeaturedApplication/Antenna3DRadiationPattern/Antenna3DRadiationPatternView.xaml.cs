// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// Antenna3DRadiationPatternView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Common.MarkupExtensions;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting3D.Model;
using SciChart.Data.Model;
using SciChart.Examples.Examples.Charts3D.Customize3DChart.AddGeometry3D;
using Viewport3D = SciChart.Charting3D.Viewport3D;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Antenna3DRadiationPattern
{
    /// <summary>
    /// Main view for the Antenna 3D Radiation Pattern example.
    ///
    /// Layout: toolbar | gain coverage charts (left) | 3D chart (centre) | polar charts (right).
    ///
    /// The 3D chart renders a closed triangulated mesh of the antenna gain pattern
    /// (AntennaGainMeshEntity) with interactive θ/φ coordinate rings and angle
    /// indicators.  The side panels show synchronised 2D cross-sections:
    ///   - GainCoverageChartsView: E/H-plane gain cut lines + φ×θ heatmap
    ///   - PolarChartsView: E-plane and H-plane polar plots
    /// </summary>
    public partial class Antenna3DRadiationPatternView : UserControl
    {
        private const int ThetaCount = 180;
        private const int PhiCount = 360;
        private const double Scale = 5.0;
        private const int MeshLodStep = 2;
        private const int RingSteps = 72;
        private const double InitialPhiDeg = 0.0;
        private const double InitialThetaDeg = 22.0;

        private Viewport3DOrientation _defaultOrientation;

        private double[,] _gain;
        private double _ringRadius;
        private double _ringCenterY;

        private TextSceneEntity _thetaLabel;
        private TextSceneEntity _phiLabel;
        private Color _thetaLabelColor;
        private Color _phiLabelColor;

        public Antenna3DRadiationPatternView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Viewport3D.SetViewportOrientation(_defaultOrientation);
        }

        private void OnLoaded(object _, RoutedEventArgs __)
        {
            // Disable shadow effects for tooltips
            EffectManager.EnableDropShadows = false;

            // Save the used Viewport orientation before the Example is initialized
            _defaultOrientation = Viewport3D.ViewportOrientation;
            Viewport3D.SetViewportOrientation(Viewport3DOrientation.ZAxisUp);

            // Set up the example
            double thetaStepDeg = 180.0 / (ThetaCount - 1);
            double phiStepDeg = 360.0 / PhiCount;
            double thetaStepRad = AntennaMetrics.ToRadians(thetaStepDeg);
            double phiStepRad = AntennaMetrics.ToRadians(phiStepDeg);

            // 1. Generate gain data — normalised [0,1] pattern for each (θ, φ) sample
            var gain = new double[ThetaCount, PhiCount];
            for (int ti = 0; ti < ThetaCount; ti++)
                for (int pi = 0; pi < PhiCount; pi++)
                    gain[ti, pi] = AntennaGain(ti * thetaStepRad, pi * phiStepRad);

            _gain = gain;

            // Subsample for the 3D mesh to reduce triangle count (~32K vs ~129K)
            int meshThetaCount = (ThetaCount - 1) / MeshLodStep + 1;
            int meshPhiCount = PhiCount / MeshLodStep;
            var meshGain = new double[meshThetaCount, meshPhiCount];
            for (int ti = 0; ti < meshThetaCount; ti++)
                for (int pi = 0; pi < meshPhiCount; pi++)
                    meshGain[ti, pi] = gain[ti * MeshLodStep, pi * MeshLodStep];

            // Use the shared HeatmapColorPalette from XAML for consistent coloring
            var palette = (HeatmapColorPalette)FindResource("GainColorPalette");
            palette.InitializeColorMap(typeof(object), 1.0);
            var meshEntity = new AntennaGainMeshEntity(meshGain, meshThetaCount, meshPhiCount, Scale, palette);

            // Ring geometry is derived from the mesh bounding box
            _ringRadius = (meshEntity.MaxY - meshEntity.MinY) / 2.0;
            _ringCenterY = (meshEntity.MinY + meshEntity.MaxY) / 2.0;

            // 2. Populate chart views
            sciChart.Viewport3D.RootEntity.Children.Add(meshEntity);
            gainCoverageView.Populate(gain, thetaStepDeg, phiStepDeg, InitialThetaDeg, InitialPhiDeg);
            gainCoverageView.BindSliders(ThetaCutSlider, PhiCutSlider);
            polarView.Populate(gain, thetaStepDeg, phiStepDeg, InitialThetaDeg, InitialPhiDeg);

            // 3. Build rings, angle indicators and metrics
            BuildPhiRing(InitialPhiDeg, _ringRadius, _ringCenterY);
            BuildThetaRing(InitialThetaDeg, gain, _ringRadius);
            BuildAngleIndicators(_ringRadius, _ringCenterY);
            var metrics = (AntennaMetrics)Resources["AntennaMetrics"];
            PopulateMetrics(metrics, gain, thetaStepDeg, phiStepDeg);

            // 4. Set visible ranges to tightly fit the mesh with a small padding
            double rangePadding = 1.05;
            sciChart.YAxis.VisibleRange = new DoubleRange(meshEntity.MinY, meshEntity.MaxY);
            var xzRange = new DoubleRange(-_ringRadius * rangePadding, _ringRadius * rangePadding);
            sciChart.XAxis.VisibleRange = xzRange;
            sciChart.ZAxis.VisibleRange = xzRange;
            UpdateAngleIndicators(IndicatorRadiusSlider.Value, _ringRadius, _ringCenterY);
        }

        /// <summary>
        /// Builds a great-circle ring on the 3D mesh at a fixed azimuth angle φ.
        /// The ring traces a full circle in the plane defined by φ, tilted from
        /// the +Y axis (θ=0) down to -Y (θ=180).
        /// </summary>
        private void BuildPhiRing(double phiDeg, double ringRadius, double ringCenterY)
        {
            double phi = AntennaMetrics.ToRadians(phiDeg);
            var ds = new XyzDataSeries3D<double, double, double>();
            using (ds.SuspendUpdates())
                for (int i = 0; i <= RingSteps; i++)
                {
                    double alpha = i * 2.0 * Math.PI / RingSteps;
                    ds.Append(ringRadius * Math.Sin(alpha) * Math.Cos(phi),
                              ringCenterY + ringRadius * Math.Cos(alpha),
                              ringRadius * Math.Sin(alpha) * Math.Sin(phi));
                }
            phiRing.DataSeries = ds;
        }

        /// <summary>
        /// Builds a horizontal ring on the 3D mesh at a fixed elevation angle θ.
        /// The ring Y position follows the average mesh surface at that θ so it
        /// sits on (not through) the gain pattern.
        /// </summary>
        private void BuildThetaRing(double thetaDeg, double[,] gain, double ringRadius)
        {
            double theta = AntennaMetrics.ToRadians(thetaDeg);

            // Ring Y = average mesh vertex Y at this theta: avgGain(θ) * Scale * cos(θ)
            int thetaIdx = (int)Math.Round(thetaDeg / 180.0 * (ThetaCount - 1));
            thetaIdx = Math.Max(0, Math.Min(thetaIdx, ThetaCount - 1));
            double sum = 0;
            for (int pi = 0; pi < PhiCount; pi++)
                sum += gain[thetaIdx, pi];
            double ringY = (sum / PhiCount) * Scale * Math.Cos(theta);

            var ds = new XyzDataSeries3D<double, double, double>();
            using (ds.SuspendUpdates())
                for (int i = 0; i <= RingSteps; i++)
                {
                    double beta = i * 2.0 * Math.PI / RingSteps;
                    ds.Append(ringRadius * Math.Cos(beta),
                              ringY,
                              ringRadius * Math.Sin(beta));
                }
            thetaRing.DataSeries = ds;
        }

        /// <summary>
        /// Creates the θ and φ angle indicator labels (TextSceneEntity) and
        /// triggers the initial arc/arrow layout.
        /// </summary>
        private void BuildAngleIndicators(double ringRadius, double ringCenterY)
        {
            _thetaLabelColor = Color.FromRgb(0xFF, 0x80, 0x40);
            _phiLabelColor = Color.FromRgb(0x40, 0x80, 0xFF);

            _thetaLabel = new TextSceneEntity("Theta", _thetaLabelColor, new Vector3(), 5, "Segoe UI");
            sciChart.Viewport3D.RootEntity.Children.Add(_thetaLabel);

            _phiLabel = new TextSceneEntity("Phi", _phiLabelColor, new Vector3(), 5, "Segoe UI");
            sciChart.Viewport3D.RootEntity.Children.Add(_phiLabel);

            UpdateAngleIndicators(IndicatorRadiusSlider.Value, ringRadius, ringCenterY);
        }

        /// <summary>
        /// Redraws the θ and φ angle indicator arcs, arrowheads, and labels.
        /// The θ arc is a semicircle on the XY plane (back wall), while the
        /// φ arc is a semicircle on the XZ plane (top).
        /// </summary>
        private void UpdateAngleIndicators(double r, double ringRadius, double ringCenterY)
        {
            const int arcSteps = 72;
            double arrowLength = r * 0.25;
            double arrowWingSpread = 0.3;
            double rangePadding = 1.05;
            double labelOffsetFactor = 1.08;

            double zPlane = -ringRadius * rangePadding;
            double yPlane = ringCenterY + ringRadius;

            // Theta arc: semicircle on XY plane (Z = zPlane), centered at ringCenterY
            var thetaArcDs = new XyzDataSeries3D<double, double, double>();
            using (thetaArcDs.SuspendUpdates())
                for (int i = 0; i <= arcSteps; i++)
                {
                    double alpha = i * Math.PI / arcSteps;
                    thetaArcDs.Append(r * Math.Sin(alpha), ringCenterY + r * Math.Cos(alpha), zPlane);
                }
            thetaArc.DataSeries = thetaArcDs;

            // Theta arrowhead at bottom of arc, wings spread in X
            double thetaTipY = ringCenterY - r;
            var thetaArrowDs = new XyzDataSeries3D<double, double, double>();
            using (thetaArrowDs.SuspendUpdates())
            {
                thetaArrowDs.Append(arrowLength, thetaTipY - arrowLength * arrowWingSpread, zPlane);
                thetaArrowDs.Append(0, thetaTipY, zPlane);
                thetaArrowDs.Append(arrowLength, thetaTipY + arrowLength * arrowWingSpread, zPlane);
            }
            thetaArrow.DataSeries = thetaArrowDs;

            // Phi arc: semicircle on XZ plane (Y = yPlane)
            var phiArcDs = new XyzDataSeries3D<double, double, double>();
            using (phiArcDs.SuspendUpdates())
                for (int i = 0; i <= arcSteps; i++)
                {
                    double beta = i * Math.PI / arcSteps;
                    phiArcDs.Append(r * Math.Cos(beta), yPlane, r * Math.Sin(beta));
                }
            phiArc.DataSeries = phiArcDs;

            // Phi arrowhead at left end of arc, wings spread in Z
            var phiArrowDs = new XyzDataSeries3D<double, double, double>();
            using (phiArrowDs.SuspendUpdates())
            {
                phiArrowDs.Append(-r - arrowLength * arrowWingSpread, yPlane, arrowLength);
                phiArrowDs.Append(-r, yPlane, 0);
                phiArrowDs.Append(-r + arrowLength * arrowWingSpread, yPlane, arrowLength);
            }
            phiArrow.DataSeries = phiArrowDs;

            // Position labels near arc midpoints
            double labelR = r * labelOffsetFactor;
            _thetaLabel.Location = TransformToWorldCoordinates(labelR, ringCenterY, zPlane);
            _phiLabel.Location = TransformToWorldCoordinates(0, yPlane, labelR);
            sciChart.InvalidateElement();
        }

        private void OnIndicatorRadiusChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_gain == null) return;
            UpdateAngleIndicators(e.NewValue, _ringRadius, _ringCenterY);
        }

        /// <summary>
        /// Slider changed: update the φ ring on the 3D mesh and both 2D E-plane cuts.
        /// </summary>
        private void OnPhiCutChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_gain == null) return;
            BuildPhiRing(e.NewValue, _ringRadius, _ringCenterY);
            polarView.UpdateElevationCut(e.NewValue);
            gainCoverageView.UpdatePhiCut(e.NewValue);
        }

        /// <summary>
        /// Slider changed: update the θ ring on the 3D mesh and both 2D H-plane cuts.
        /// </summary>
        private void OnThetaCutChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_gain == null) return;
            BuildThetaRing(e.NewValue, _gain, _ringRadius);
            polarView.UpdateAzimuthCut(e.NewValue);
            gainCoverageView.UpdateThetaCut(e.NewValue);
        }

        private void OnIndicatorOpacityChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_thetaLabel == null) return;
            byte alpha = (byte)(e.NewValue * 255);
            _thetaLabel.TextColor = Color.FromArgb(alpha, _thetaLabelColor.R, _thetaLabelColor.G, _thetaLabelColor.B);
            _phiLabel.TextColor = Color.FromArgb(alpha, _phiLabelColor.R, _phiLabelColor.G, _phiLabelColor.B);
            sciChart.InvalidateElement();
        }

        /// <summary>
        /// Computes the normalised gain [0,1] for an 8×8 microstrip patch array
        /// with λ/2 spacing and broadside steering.
        /// Pattern = element_factor(θ) × array_factor(θ,φ)^afExponent.
        /// θ ∈ [0,π] from +Y (main beam); φ ∈ [0,2π], φ=0 → +X axis.
        /// </summary>
        private static double AntennaGain(double theta, double phi)
        {
            int elementsPerAxis = 8;
            int totalElements = elementsPerAxis * elementsPerAxis;
            double afExponent = 0.75;
            double backLobeAttenuation = 0.22;

            double sinT = Math.Sin(theta);
            double cosT = Math.Cos(theta);
            double psix = Math.PI * sinT * Math.Cos(phi);
            double psiy = Math.PI * sinT * Math.Sin(phi);
            double af = Math.Pow((SincArrayFactor(psix, elementsPerAxis) * SincArrayFactor(psiy, elementsPerAxis)) / totalElements, afExponent);
            double ef = cosT >= 0 ? cosT * cosT : backLobeAttenuation * cosT * cosT;
            return ef * af;
        }

        /// <summary>
        /// sin(N·ψ/2) / sin(ψ/2) — the standard uniform linear array factor.
        /// Returns N at ψ = 0 (L'Hôpital limit).
        /// </summary>
        private static double SincArrayFactor(double psi, int n)
        {
            double halfPsi = psi / 2.0;
            double denom = Math.Sin(halfPsi);
            if (Math.Abs(denom) < 1e-9) return n;
            return Math.Abs(Math.Sin(n * halfPsi) / denom);
        }

        /// <summary>
        /// Computes scalar antenna metrics from the gain grid and writes them
        /// into the AntennaMetrics model (bound to the legend panel).
        /// </summary>
        private static void PopulateMetrics(AntennaMetrics m, double[,] gain,
                                            double thetaStepDeg, double phiStepDeg)
        {
            int thetaCount = gain.GetLength(0);
            int phiCount = gain.GetLength(1);

            // Half-power beamwidth in the E-plane (φ = 0° cut)
            double hpbwE = ComputeHpbw(gain, thetaCount, phiIdx: 0, thetaStepDeg);

            // Half-power beamwidth in the H-plane (φ = 90° cut)
            int phiIdx90 = (int)Math.Round(90.0 / phiStepDeg) % phiCount;
            double hpbwH = ComputeHpbw(gain, thetaCount, phiIdx: phiIdx90, thetaStepDeg);

            // First sidelobe level relative to peak in dBi
            double sllDbi = ComputeFirstSllDbi(gain, thetaCount, phiIdx: 0);

            // Front-to-back ratio: peak gain (0 dBi) minus gain at θ = 180°
            double fbr = 0.0 - AntennaMetrics.ToDbi(gain[thetaCount - 1, 0]);

            // Peak gain is 0 dBi by definition (normalised pattern)
            m.PeakGain = "0.0 dBi";
            m.HpbwEPlane = double.IsNaN(hpbwE) ? "—" : $"{hpbwE:F1}°";
            m.HpbwHPlane = double.IsNaN(hpbwH) ? "—" : $"{hpbwH:F1}°";
            m.FirstSll = $"{sllDbi:F1} dBi";
            m.FrontToBack = $"{fbr:F1} dB";
        }

        /// <summary>
        /// Finds the half-power beamwidth (HPBW) by scanning θ from broadside
        /// until the gain drops below half-power, then interpolating.
        /// Returns the full (2×) beamwidth in degrees.
        /// </summary>
        private static double ComputeHpbw(double[,] gain, int thetaCount, int phiIdx, double thetaStepDeg)
        {
            double peak = gain[0, phiIdx];
            double halfPower = peak * 0.5;
            for (int ti = 1; ti < thetaCount; ti++)
            {
                if (gain[ti, phiIdx] < halfPower)
                {
                    double g0 = gain[ti - 1, phiIdx], g1 = gain[ti, phiIdx];
                    double frac = (halfPower - g0) / (g1 - g0);
                    return 2.0 * (ti - 1 + frac) * thetaStepDeg;
                }
            }
            return double.NaN;
        }

        /// <summary>
        /// Finds the first sidelobe peak by scanning θ past the main lobe.
        /// Returns the sidelobe level in dBi.
        /// </summary>
        private static double ComputeFirstSllDbi(double[,] gain, int thetaCount, int phiIdx)
        {
            double mainLobeEndThreshold = 0.1;
            double sidelobePeakMinGain = 0.001;

            bool mainLobeEnded = false;
            double prevG = gain[0, phiIdx];
            for (int ti = 1; ti < thetaCount - 1; ti++)
            {
                double g = gain[ti, phiIdx], gNext = gain[ti + 1, phiIdx];
                if (!mainLobeEnded) { if (g < prevG * mainLobeEndThreshold && g < mainLobeEndThreshold) mainLobeEnded = true; }
                else if (g > prevG && g > gNext && g > sidelobePeakMinGain) return AntennaMetrics.ToDbi(g);
                prevG = g;
            }
            return -AntennaMetrics.DbiRange;
        }

        /// <summary>
        /// Converts data-space (X,Y,Z) coordinates into 3D world coordinates,
        /// accounting for the axis coordinate calculators and world-center offsets.
        /// </summary>
        private Vector3 TransformToWorldCoordinates(double dataX, double dataY, double dataZ)
        {
            return new Vector3
            {
                X = (float)sciChart.XAxis.GetCurrentCoordinateCalculator().GetCoordinate(dataX) - sciChart.WorldDimensions.X / 2.0f,
                Y = (float)sciChart.YAxis.GetCurrentCoordinateCalculator().GetCoordinate(dataY),
                Z = (float)sciChart.ZAxis.GetCurrentCoordinateCalculator().GetCoordinate(dataZ) - sciChart.WorldDimensions.Z / 2.0f
            };
        }
    }
}
