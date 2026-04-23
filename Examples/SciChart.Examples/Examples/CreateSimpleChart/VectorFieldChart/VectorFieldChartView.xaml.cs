// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// VectorFieldChartView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    /// <summary>
    /// Demonstrates a static <see cref="VectorFieldRenderableSeries"/> with two selectable field types:
    /// <list type="bullet">
    ///   <item><description>
    ///     <b>Sinusoidal Vortices (Uniform)</b> — a <see cref="UniformVectorFieldDataSeries"/> on a regular
    ///     n×n grid, where the displacement is derived from a sinusoidal stream function producing
    ///     two counter-rotating vortices.
    ///   </description></item>
    ///   <item><description>
    ///     <b>Scattered Vortex Flow (Non-Uniform)</b> — a <see cref="NonUniformVectorFieldDataSeries"/>
    ///     with randomly placed sample points, driven by two point-vortex velocity fields.
    ///     Points inside the vortex cores are excluded to avoid singularities.
    ///   </description></item>
    /// </list>
    /// The toolbar lets users explore colour modes, length modes, arrow geometry, and LOD settings.
    /// </summary>
    public partial class VectorFieldChartView : UserControl
    {
        // Half-side of the square data domain; axes are set to [-DomainSize, DomainSize]
        private const double DomainSize = 10.0;

        // Sinusoidal field: half-wavelength of the stream function (controls vortex spacing)
        private const double HalfWavelength = 5.0;

        // Non-uniform field: scales the raw 1/r vortex velocity to a displayable magnitude
        private const double VortexFieldScale = 3.0;

        // Two vortex centres for the non-uniform point-vortex field.
        // CoreRadius defines the exclusion zone around each singularity.
        private static readonly (double X, double Y, double CoreRadius)[] VortexCenters =
        {
            (-4.0, -4.0, 2.0 / 1.5),
            ( 4.0,  4.0, 2.0 / 3.0),
        };

        // Display names for the two field types; index matches _currentFieldIndex
        private static readonly string[] FieldSourceNames =
        {
            "Sinusoidal Vortices (Uniform)",
            "Scattered Vortex Flow (Non-Uniform)",
        };

        private int _currentFieldIndex;

        /// <summary>
        /// Initializes a new instance of <see cref="VectorFieldChartView"/>.
        /// </summary>
        public VectorFieldChartView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Populate combo boxes; selecting the first item triggers data generation via OnFieldTypeChanged
            foreach (string name in FieldSourceNames)
                FieldSourceCombo.Items.Add(name);
            FieldSourceCombo.SelectedIndex = 0;

            foreach (VectorColorMode mode in Enum.GetValues(typeof(VectorColorMode)))
                ColorModeCombo.Items.Add(mode);
            ColorModeCombo.SelectedItem = VectorColorMode.ByMagnitude;

            foreach (VectorLengthMode mode in Enum.GetValues(typeof(VectorLengthMode)))
                LengthModeCombo.Items.Add(mode);
            LengthModeCombo.SelectedItem = VectorLengthMode.MagnitudePixels;

        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            // Clear combo boxes to prevent item duplication when the view is reloaded
            FieldSourceCombo.Items.Clear();
            ColorModeCombo.Items.Clear();
            LengthModeCombo.Items.Clear();

            VectorSeries.DataSeries.Clear();
        }

        /// <summary>
        /// Generates data for the currently selected field type, updates the visible axis range,
        /// and refreshes the vector count annotation.
        /// </summary>
        private void RegenerateData()
        {
            if (VectorSeries == null) return;

            int count = _currentFieldIndex == 0
                ? GenerateUniformData()
                : GenerateNonUniformData();

            var axisRange = DomainSize + DomainSize * 0.2;
            XAxis.VisibleRange = new DoubleRange(-axisRange, axisRange);
            YAxis.VisibleRange = new DoubleRange(-axisRange, axisRange);
            VectorCountAnnotation.Text = $"Count: {count:N0} vectors";
        }

        /// <summary>
        /// Generates a <see cref="UniformVectorFieldDataSeries"/> on an n×n grid using a
        /// sinusoidal stream function: DX = -sin(π·y/λ), DY = sin(π·x/λ).
        /// This produces two counter-rotating vortices centred at the origin.
        /// </summary>
        private int GenerateUniformData()
        {
            int n = (int)GridSizeSlider.Value;
            double xMin = -DomainSize, xMax = DomainSize;
            double yMin = -DomainSize, yMax = DomainSize;
            double xStep = (xMax - xMin) / (n - 1);
            double yStep = (yMax - yMin) / (n - 1);

            var dxs = new double[n, n];
            var dys = new double[n, n];
            var metadata = new IPointMetadata[n, n];

            for (int xi = 0; xi < n; xi++)
            {
                double x = xMin + xi * xStep;
                for (int yi = 0; yi < n; yi++)
                {
                    double y = yMin + yi * yStep;
                    dxs[xi, yi] = -Math.Sin(Math.PI * y / HalfWavelength);
                    dys[xi, yi] = Math.Sin(Math.PI * x / HalfWavelength);
                    metadata[xi, yi] = new VectorPointMetadata();
                }
            }

            VectorSeries.DataSeries = new UniformVectorFieldDataSeries(xMin, xStep, yMin, yStep, dxs, dys, metadata);
            return n * n;
        }

        /// <summary>
        /// Generates a <see cref="NonUniformVectorFieldDataSeries"/> with randomly scattered points
        /// driven by two point-vortex (Biot-Savart) velocity fields. Points that fall inside a
        /// vortex core are rejected to avoid near-singular velocity values.
        /// </summary>
        private int GenerateNonUniformData()
        {
            int targetCount = (int)VectorsCountSlider.Value;
            var rng = new Random((int)DateTime.UtcNow.Ticks);

            var xs = new List<double>(targetCount);
            var ys = new List<double>(targetCount);
            var dxs = new List<double>(targetCount);
            var dys = new List<double>(targetCount);
            var metadata = new List<IPointMetadata>(targetCount);

            int attempts = 0;
            while (xs.Count < targetCount && attempts < targetCount * 20)
            {
                attempts++;
                double x = rng.NextDouble() * 2 * DomainSize - DomainSize;
                double y = rng.NextDouble() * 2 * DomainSize - DomainSize;

                // Reject points inside any vortex core to avoid singularities
                bool inCore = false;
                foreach (var (cx, cy, coreR) in VortexCenters)
                {
                    double ex = x - cx, ey = y - cy;
                    if (ex * ex + ey * ey < coreR * coreR) { inCore = true; break; }
                }
                if (inCore) continue;

                // Sum contributions from both point vortices (Biot-Savart: v = Γ/2π × r⊥/r²)
                double u = 0, v = 0;
                foreach (var (cx, cy, _) in VortexCenters)
                {
                    double rx = x - cx, ry = y - cy;
                    double r2 = rx * rx + ry * ry;
                    u += VortexFieldScale * (-ry / r2);
                    v += VortexFieldScale * (+rx / r2);
                }

                xs.Add(x);
                ys.Add(y);
                dxs.Add(u);
                dys.Add(v);
                metadata.Add(new VectorPointMetadata());
            }

            VectorSeries.DataSeries = new NonUniformVectorFieldDataSeries(
                xs.ToArray(), ys.ToArray(), dxs.ToArray(), dys.ToArray(), metadata);
            return xs.Count;
        }

        /// <summary>
        /// Sets the colour map range to match the active colour mode and field type.
        /// Direction mode spans [0, 2π]; magnitude mode is clamped to the expected field peak.
        /// </summary>
        private void UpdateColorMapRange()
        {
            if (VectorSeries?.ColorMap == null || ColorModeCombo.SelectedItem == null) return;

            VectorSeries.ColorMap.Minimum = 0;
            if ((VectorColorMode)ColorModeCombo.SelectedItem == VectorColorMode.ByDirection)
            {
                VectorSeries.ColorMap.Maximum = 2 * Math.PI;
            }
            else
            {
                // Sinusoidal field magnitude ∈ [0, √2]; vortex field saturates at the larger core boundary
                double pointVortexMax = VortexFieldScale / Math.Max(VortexCenters[0].CoreRadius, VortexCenters[1].CoreRadius);
                VectorSeries.ColorMap.Maximum = _currentFieldIndex == 0 ? Math.Sqrt(2) : pointVortexMax;
            }
        }

        private void OnFieldTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FieldSourceCombo.SelectedIndex < 0) return;
            _currentFieldIndex = FieldSourceCombo.SelectedIndex;

            // Show the grid-size slider for the uniform field and the count slider for non-uniform
            bool isUniform = _currentFieldIndex == 0;
            GridSizeHeader.Visibility = isUniform ? Visibility.Visible : Visibility.Collapsed;
            GridSizePanel.Visibility = isUniform ? Visibility.Visible : Visibility.Collapsed;
            VectorsCountHeader.Visibility = isUniform ? Visibility.Collapsed : Visibility.Visible;
            VectorsCountPanel.Visibility = isUniform ? Visibility.Collapsed : Visibility.Visible;

            SciChartSurface.ChartTitle = FieldSourceNames[_currentFieldIndex];
            UpdateColorMapRange();
            RegenerateData();
        }

        private void OnGridSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RegenerateData();
        }

        private void OnVectorsCountChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RegenerateData();
        }

        private void OnColorModeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VectorSeries == null || ColorModeCombo.SelectedItem == null) return;
            var mode = (VectorColorMode)ColorModeCombo.SelectedItem;
            VectorSeries.ColorMode = mode;
            VectorSeries.ColorMap = mode == VectorColorMode.ByDirection
                ? (HeatmapColorPalette)Resources["CoolWarmCircularPalette"]
                : (HeatmapColorPalette)Resources["CoolWarmLinearPalette"];
            UpdateColorMapRange();
        }

        private void OnLengthModeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VectorSeries == null || LengthModeCombo.SelectedItem == null) return;
            var mode = (VectorLengthMode)LengthModeCombo.SelectedItem;
            VectorSeries.LengthMode = mode;
            MagnitudeScaleHeader.Visibility = mode == VectorLengthMode.MagnitudePixels ? Visibility.Visible : Visibility.Collapsed;
            MagnitudeScalePanel.Visibility = mode == VectorLengthMode.MagnitudePixels ? Visibility.Visible : Visibility.Collapsed;
            PixelLengthHeader.Visibility = mode == VectorLengthMode.FixedPixels ? Visibility.Visible : Visibility.Collapsed;
            PixelLengthPanel.Visibility = mode == VectorLengthMode.FixedPixels ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
