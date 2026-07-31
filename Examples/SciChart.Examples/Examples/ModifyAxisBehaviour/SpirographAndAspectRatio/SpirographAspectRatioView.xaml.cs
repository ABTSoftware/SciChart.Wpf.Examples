// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SpirographAspectRatioView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour.SpirographAndAspectRatio
{
    public partial class SpirographAspectRatioView : UserControl
    {
        // Visual density vs frame rate: fewer passes / coarser curves spin more smoothly.
        private const int TrianglePassesPerSpike = 12;
        private const int MainPasses = 12;
        private const double SpinRadiansPerSecond = 0.4;

        private readonly List<Pass> _passes = new List<Pass>();
        private TimeSpan? _spinStart;

        // Used when the aspect-ratio lock is toggled off: a plain manager draws the axes without aspect correction.
        private readonly DefaultViewportManager _defaultViewportManager = new DefaultViewportManager();

        // Rotation-invariant data bounds accumulated in AddPass, used to pin each axis's ZoomExtentsRange so
        // ZoomExtents always frames the whole figure regardless of the current spin angle.
        private double _dataXMin, _dataXMax, _dataYMin, _dataYMax;

        public SpirographAspectRatioView()
        {
            InitializeComponent();
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Rebuild on every load so a re-attached instance (or one whose series were cleared while
            // detached) always shows the figures, and restart the spin clock for a clean re-entry.
            BuildSpirographs();
            _spinStart = null;

            // Drive the endless rotation off the WPF render clock. Re-subscribe defensively so a repeated
            // Loaded never stacks handlers.
            CompositionTarget.Rendering -= OnRendering;
            CompositionTarget.Rendering += OnRendering;
        }

        private void OnUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= OnRendering;
        }

        // Toggles the aspect-ratio lock.
        // Checked: the surface uses the AspectRatioViewportManager (declared as a resource) so the figures stay round.
        // Unchecked: falls back to a plain manager, so the axes are drawn without aspect correction.
        private void OnAspectRatioEnabledChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            var isEnabled = aspectRatioToggle.IsChecked == true;

            if (aspectRatioMenu != null)
                aspectRatioMenu.IsEnabled = isEnabled;

            if (sciChartSurface != null)
            {
                sciChartSurface.ViewportManager = isEnabled
                    ? (IViewportManager)Resources["AspectRatioVm"]
                    : _defaultViewportManager;
            }
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (!IsLoaded)
            {
                CompositionTarget.Rendering -= OnRendering;
                return;
            }

            var now = ((RenderingEventArgs)e).RenderingTime;
            if (_spinStart == null) _spinStart = now;

            var spin = (now - _spinStart.Value).TotalSeconds * SpinRadiansPerSecond;
            Spin(spin);
        }

        // Builds every pass once: each pass is one line series filled from a precomputed base curve. The
        // render loop then only rotates these in place (see Spin), so no geometry is rebuilt per frame.
        private void BuildSpirographs()
        {
            sciChartSurface.RenderableSeries.Clear();
            _passes.Clear();

            _dataXMin = _dataYMin = double.MaxValue;
            _dataXMax = _dataYMax = double.MinValue;

            var (triX, triY) = ComputeBaseCurve(outerRadius: 3, innerRadius: 1, distanceFromCenter: 2.1);
            var triStroke = Color.FromRgb(0x64, 0xBA, 0xE4);
            double spread = (30.0 * Math.PI / 180.0) / (TrianglePassesPerSpike - 1);
            for (int s = 0; s < 3; s++)
            {
                double spikeCenter = s * 2.0 * Math.PI / 3.0;
                for (int p = 0; p < TrianglePassesPerSpike; p++)
                {
                    double rotation = spikeCenter + (p - (TrianglePassesPerSpike - 1) / 2.0) * spread;
                    AddPass(triX, triY, rotation, xOffset: -12.0, triStroke);
                }
            }

            var (mainX, mainY) = ComputeBaseCurve(outerRadius: 10, innerRadius: 8, distanceFromCenter: 2.5);
            double angleStep = 2.0 * Math.PI / MainPasses;
            for (int p = 0; p < MainPasses; p++)
            {
                AddPass(mainX, mainY, p * angleStep, xOffset: 0.0, ColorFromHsv(p * 360.0 / MainPasses, 1.0, 0.9));
            }

            // ZoomExtents should always frame the whole figure whatever the spin angle
            sciChartSurface.XAxis.ZoomExtentsRange = new DoubleRange(_dataXMin, _dataXMax);
            sciChartSurface.YAxis.ZoomExtentsRange = new DoubleRange(_dataYMin, _dataYMax);
        }

        private void AddPass(double[] baseX, double[] baseY, double rotation, double xOffset, Color stroke)
        {
            var series = new XyDataSeries<double, double> { AcceptsUnsortedData = true };
            var pass = new Pass(series, baseX, baseY, rotation, xOffset);

            // Accumulate the rotation-invariant bounds: this pass spins about (xOffset, 0), so across all spin
            // angles it stays within a disc of radius = its base curve's farthest point from the centre.
            double radius = MaxRadius(baseX, baseY);
            _dataXMin = Math.Min(_dataXMin, xOffset - radius);
            _dataXMax = Math.Max(_dataXMax, xOffset + radius);
            _dataYMin = Math.Min(_dataYMin, -radius);
            _dataYMax = Math.Max(_dataYMax, radius);

            double cosA = Math.Cos(rotation);
            double sinA = Math.Sin(rotation);
            for (int i = 0; i < baseX.Length; i++)
            {
                double x = baseX[i];
                double y = baseY[i];
                series.Append(x * cosA - y * sinA + xOffset, x * sinA + y * cosA);
            }

            _passes.Add(pass);
            sciChartSurface.RenderableSeries.Add(new FastLineRenderableSeries
            {
                DataSeries = series,
                Stroke = stroke,
                StrokeThickness = 1
            });
        }

        // The farthest distance any point of a base curve sits from its centre; because the curve spins about
        // that centre, this is the radius of the disc that contains the pass at every spin angle.
        private static double MaxRadius(double[] x, double[] y)
        {
            double maxSquared = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                double distanceSquared = x[i] * x[i] + y[i] * y[i];
                if (distanceSquared > maxSquared) maxSquared = distanceSquared;
            }

            return Math.Sqrt(maxSquared);
        }

        // Rewrites every pass's points in place for the current spin angle (no allocation, no trig in the
        // inner loop). Each spirograph rotates about its own centre; the AspectRatioViewportManager keeps
        // the spinning figures undistorted.
        private void Spin(double angle)
        {
            foreach (var pass in _passes)
            {
                double cosA = Math.Cos(pass.Rotation + angle);
                double sinA = Math.Sin(pass.Rotation + angle);

                using (pass.Series.SuspendUpdates())
                {
                    var xs = pass.Series.XValues;
                    var ys = pass.Series.YValues;
                    for (int i = 0; i < pass.BaseX.Length; i++)
                    {
                        double x = pass.BaseX[i];
                        double y = pass.BaseY[i];
                        xs[i] = x * cosA - y * sinA + pass.XOffset;
                        ys[i] = x * sinA + y * cosA;
                    }
                }
            }
        }

        // The un-rotated hypotrochoid, sampled once per spirograph and reused for every rotated pass.
        private static (double[] X, double[] Y) ComputeBaseCurve(int outerRadius, int innerRadius, double distanceFromCenter)
        {
            double period = 2.0 * Math.PI * innerRadius / GreatestCommonDivisor(outerRadius, innerRadius);
            int steps = Math.Max(300, (int)(period / (2 * Math.PI) * 600));
            double dt = period / steps;

            var x = new double[steps + 1];
            var y = new double[steps + 1];
            for (int i = 0; i <= steps; i++)
            {
                double t = i * dt;
                x[i] = (outerRadius - innerRadius) * Math.Cos(t) + distanceFromCenter * Math.Cos((outerRadius - innerRadius) * t / innerRadius);
                y[i] = (outerRadius - innerRadius) * Math.Sin(t) - distanceFromCenter * Math.Sin((outerRadius - innerRadius) * t / innerRadius);
            }

            return (x, y);
        }

        private static Color ColorFromHsv(double hue, double saturation, double value)
        {
            int sector = (int)(hue / 60.0) % 6;
            double sectorFraction = hue / 60.0 - Math.Floor(hue / 60.0);
            double colorFloor = value * (1 - saturation);
            double colorFalling = value * (1 - sectorFraction * saturation);
            double colorRising = value * (1 - (1 - sectorFraction) * saturation);

            double r, g, b;
            switch (sector)
            {
                case 0: r = value; g = colorRising; b = colorFloor; break;
                case 1: r = colorFalling; g = value; b = colorFloor; break;
                case 2: r = colorFloor; g = value; b = colorRising; break;
                case 3: r = colorFloor; g = colorFalling; b = value; break;
                case 4: r = colorRising; g = colorFloor; b = value; break;
                default: r = value; g = colorFloor; b = colorFalling; break;
            }

            return Color.FromRgb((byte)Math.Round(r * 255), (byte)Math.Round(g * 255), (byte)Math.Round(b * 255));
        }

        private static int GreatestCommonDivisor(int x, int y)
        {
            while (y != 0)
            {
                int temp = y;
                y = x % y;
                x = temp;
            }
            return x;
        }

        private sealed class Pass
        {
            public Pass(XyDataSeries<double, double> series, double[] baseX, double[] baseY, double rotation, double xOffset)
            {
                Series = series;
                BaseX = baseX;
                BaseY = baseY;
                Rotation = rotation;
                XOffset = xOffset;
            }

            public XyDataSeries<double, double> Series { get; }
            public double[] BaseX { get; }
            public double[] BaseY { get; }
            public double Rotation { get; }
            public double XOffset { get; }
        }
    }
}
