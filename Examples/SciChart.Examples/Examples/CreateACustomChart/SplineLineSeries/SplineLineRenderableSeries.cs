// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SplineLineRenderableSeries.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SciChart.Charting.Numerics.CoordinateCalculators;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting.Visuals.RenderableSeries.DrawingProviders;
using SciChart.Charting.Visuals.RenderableSeries.HitTesters;
using SciChart.Data.Model;
using SciChart.Drawing.Common;

namespace SciChart.Examples.Examples.CreateACustomChart.SplineLineSeries
{
    /// <summary>
    /// A CustomRenderableSeries example which uses a Cubic Spline algorithm to smooth the points in a FastLineRenderableSeries
    /// </summary>
    public class SplineLineRenderableSeries : CustomRenderableSeries
    {
        public static readonly DependencyProperty IsSplineEnabledProperty =
            DependencyProperty.Register("IsSplineEnabled", typeof (bool), typeof (SplineLineRenderableSeries),
                new PropertyMetadata(default(bool), PropertyChangedCallback));

        public bool IsSplineEnabled
        {
            get { return (bool) GetValue(IsSplineEnabledProperty); }
            set { SetValue(IsSplineEnabledProperty, value); }
        }

        public static readonly DependencyProperty UpSampleFactorProperty = DependencyProperty.Register(
            "UpSampleFactor", typeof (int), typeof (SplineLineRenderableSeries), new PropertyMetadata(10, PropertyChangedCallback));

        public int UpSampleFactor
        {
            get { return (int) GetValue(UpSampleFactorProperty); }
            set { SetValue(UpSampleFactorProperty, value); }
        }

        private IList<Point> _splineSeries;

        #region HitTest
        public class SplineLineHitTestProvider : DefaultHitTestProvider<SplineLineRenderableSeries>
        {
            public SplineLineHitTestProvider(SplineLineRenderableSeries renderSeries) : base(renderSeries)
            {
            }

            public override HitTestInfo HitTest(Point rawPoint, double hitTestRadius, bool interpolate = false)
            {
                var nearestBaseHitResult = base.HitTest(rawPoint, hitTestRadius, interpolate);

                // No spline? Fine - return base implementation
                if (!RenderableSeries.IsSplineEnabled || RenderableSeries._splineSeries == null || RenderableSeries.CurrentRenderPassData == null)
                    return nearestBaseHitResult;

                var nearestHitResult = new HitTestInfo();

                // Get the coordinateCalculators. See 'Converting Pixel Coordinates to Data Coordinates' documentation for coordinate transforms
                var xCalc = RenderableSeries.CurrentRenderPassData.XCoordinateCalculator;

                // Compute the X,Y data value at the mouse location
                var xDataPointAtMouse = xCalc.GetDataValue(RenderableSeries.CurrentRenderPassData.IsVerticalChart ? rawPoint.Y : rawPoint.X);

                // Find the index in the spline interpolated data that is nearest to the X-Data point at mouse
                // NOTE: This assumes the data is sorted in ascending direction and a binary search would be faster ... 
                int foundIndex = RenderableSeries.FindIndex(RenderableSeries._splineSeries, xDataPointAtMouse);

                if (foundIndex != -1)
                {
                    nearestHitResult.IsWithinDataBounds = true;

                    // Find the nearest data point to the mouse 
                    var xDataPointNearest = RenderableSeries._splineSeries[foundIndex].X;
                    var yDataPointNearest = RenderableSeries._splineSeries[foundIndex].Y;
                    nearestHitResult.XValue = xDataPointNearest;
                    nearestHitResult.YValue = yDataPointNearest;

                    // Compute the X,Y coordinates (pixel coords) of the nearest data point to the mouse
                    nearestHitResult.HitTestPoint = nearestHitResult.HitTestPoint = RenderableSeries.GetCoordinatesFor(xDataPointNearest, yDataPointNearest);

                    // Determine if mouse-location is within 7.07 pixels of the nearest data point
                    var distance = Math.Pow(rawPoint.X - nearestHitResult.HitTestPoint.X, 2) +
                                   Math.Pow(rawPoint.Y - nearestHitResult.HitTestPoint.Y, 2);
                    distance = Math.Sqrt(distance);

                    var baseDistance = Math.Pow(rawPoint.X - nearestBaseHitResult.HitTestPoint.X, 2) +
                                       Math.Pow(rawPoint.Y - nearestBaseHitResult.HitTestPoint.Y, 2);
                    baseDistance = Math.Sqrt(baseDistance);

                    nearestHitResult.IsHit = distance <= DefaultHitTestRadius || baseDistance <= DefaultHitTestRadius;
                    nearestHitResult.IsVerticalHit = true;
                    nearestHitResult.DataSeriesIndex = nearestBaseHitResult.DataSeriesIndex;

                    if (RenderableSeries.DataSeries.HasMetadata)
                        nearestHitResult.Metadata = RenderableSeries.DataSeries.Metadata[nearestHitResult.DataSeriesIndex];

                    // Returning a HitTestResult with IsHit = true / IsVerticalHit signifies to the Rollovermodifier & TooltipModifier to show a tooltip at this location
                    return nearestHitResult;
                }
                else
                {
                    // Returning HitTestInfo.Empty signifies to the RolloverModifier & TooltipModifier there is nothing to show here
                    return HitTestInfo.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="T:SciChart.Charting.Visuals.RenderableSeries.HitTesters.IHitTestProvider"/> implementation associated with this series. This class provides methods such as <see cref="M:SciChart.Charting.Visuals.RenderableSeries.HitTesters.IHitTestProvider.HitTest(System.Windows.Point,System.Boolean)"/>
        /// which return a <see cref="T:SciChart.Charting.Visuals.RenderableSeries.HitTestInfo"/> struct containing information about the Hit-Test operation. Use to determine points near the mouse or whether the mouse is over a series.
        /// </summary>
        public override IHitTestProvider HitTestProvider { get; protected set; }
        #endregion

        public SplineLineRenderableSeries()
        {
            HitTestProvider = new SplineLineHitTestProvider(this);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnInvalidateParentSurface(d, e);
        }

        /// <summary>
        /// Draws the series using the <see cref="IRenderContext2D" /> and the <see cref="IRenderPassData" /> passed in
        /// </summary>
        /// <param name="renderContext">The render context. This is a graphics object which has methods to draw lines, quads and polygons to the screen</param>
        /// <param name="renderPassData">The render pass data. Contains a resampled 
        /// <see cref="IPointSeries" />, the 
        /// <see cref="IndexRange" /> of points on the screen
        /// and the current YAxis and XAxis 
        /// <see cref="ICoordinateCalculator{T}" /> to convert data-points to screen points</param>
        protected override void Draw(IRenderContext2D renderContext, IRenderPassData renderPassData)
        {
            base.Draw(renderContext, renderPassData);

            // Get the data from RenderPassData. See CustomRenderableSeries article which describes PointSeries relationship to DataSeries
            if (renderPassData.PointSeries.Count == 0) return;

            // Convert to Spline Series
            _splineSeries = ComputeSplineSeries(renderPassData.PointSeries, IsSplineEnabled, UpSampleFactor);

            // Get the coordinates of the first dataPoint
            var point = GetCoordinatesFor(_splineSeries[0].X, _splineSeries[0].Y);

            // Create a pen to draw the spline line. Make sure you dispose it!             
            using (var linePen = renderContext.CreatePen(this.Stroke, this.AntiAliasing, this.StrokeThickness))
            {
                // Create a line drawing context. Make sure you dispose it!
                // NOTE: You can create mutliple line drawing contexts to draw segments if you want
                //       You can also call renderContext.DrawLine() and renderContext.DrawLines(), but the lineDrawingContext is higher performance
                using (var lineDrawingContext = renderContext.BeginLine(linePen, point.X, point.Y))
                {
                    for (int i = 1; i < _splineSeries.Count; i++)
                    {
                        point = GetCoordinatesFor(_splineSeries[i].X, _splineSeries[i].Y);

                        lineDrawingContext.MoveTo(point.X, point.Y);
                    }
                }
            }

            new LegacyPointMarkerRenderer(this, GetPointMarker(), SelectedPointMarker)
                .Draw(renderContext, renderPassData.PointSeries, renderPassData);
        }

        private Point GetCoordinatesFor(double xValue, double yValue)
        {
            // Get the coordinateCalculators. See 'Converting Pixel Coordinates to Data Coordinates' documentation for coordinate transforms
            var xCoord = CurrentRenderPassData.XCoordinateCalculator.GetCoordinate(xValue);
            var yCoord = CurrentRenderPassData.YCoordinateCalculator.GetCoordinate(yValue);

            if (CurrentRenderPassData.IsVerticalChart)
            {
                Swap(ref xCoord, ref yCoord);
            }
            
            return new Point(xCoord, yCoord);
        }

        private void Swap(ref double arg1, ref double arg2)
        {
            var tmp = arg2;

            arg2 = arg1;
            arg1 = tmp;
        }

        // Cubic Spline interpolation: http://www.codeproject.com/Articles/560163/Csharp-Cubic-Spline-Interpolation
        private IList<Point> ComputeSplineSeries(IPointSeries inputPointSeries, bool isSplineEnabled, int upsampleBy)
        {
            IList<Point> result = null;

            if (!isSplineEnabled)
            {
                // No spline, just return points. Note: for large datasets, even the copy here causes performance problems!                 
                result = new List<Point>(inputPointSeries.Count);
                for (int i = 0; i < inputPointSeries.Count; i++)
                {
                    result.Add(new Point(inputPointSeries.XValues[i], inputPointSeries.YValues[i]));
                }
                return result;
            }

            // Spline enabled
            int n = inputPointSeries.Count * upsampleBy;
            var x = inputPointSeries.XValues.ToArray();
            var y = inputPointSeries.YValues.ToArray();
            double[] xs = new double[n];
            double stepSize = (x[x.Length - 1] - x[0]) / (n - upsampleBy);

            for (int i = 0; i < n; i++)
            {
                xs[i] = x[0] + i * stepSize;
            }

            var cubicSpline = new CubicSpline();
            double[] ys = cubicSpline.FitAndEval(x, y, xs);

            result = new List<Point>(n);
            for (int i = 0; i < xs.Length; i++)
            {
                result.Add(new Point(xs[i], ys[i]));
            }
            return result;
        }

        private int FindIndex(IList<Point> list, double value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].X.CompareTo(value) >= 0)
                    return i;
            }

            return -1;
        }
    }
}
