// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AnnotatedPointMarker.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Core.Extensions;
using SciChart.Drawing.Common;

namespace SciChart.Examples.Examples.InspectDatapoints.SeriesWithMetadata
{
    public class AnnotatedPointMarker : BasePointMarker
    {
        private const float TextSize = 12f;
        private const double TextIndent = 3f;
        private Color _checkedPointMarkerTextColor = Color.FromArgb(0xFF, 0x6B, 0xC4, 0xA9);

        private IList<IPointMetadata> _dataPointMetadata;
        private IList<int> _dataPointIndexes;

        private IPen2D _strokePen;

        private IBrush2D _gainFillBrush;
        private IBrush2D _lossFillBrush;

        private readonly TextBlock _textBlock;

        public Color GainMarkerFill { get; set; }
        public Color LossMarkerFill { get; set; }

        public AnnotatedPointMarker()
        {
            _dataPointIndexes = new List<int>();

            _textBlock = new TextBlock { FontSize = TextSize, Margin = new Thickness(TextIndent) };

            SetCurrentValue(PointMarkerBatchStrategyProperty, new DefaultPointMarkerBatchStrategy());
        }

        public override void BeginBatch(IRenderContext2D context, Color? strokeColor, Color? fillColor)
        {
            _dataPointMetadata ??= RenderableSeries.DataSeries.Metadata;

            _dataPointIndexes = new List<int>();

            base.BeginBatch(context, strokeColor, fillColor);
        }

        public override void MoveTo(IRenderContext2D context, double x, double y, int index)
        {
            if (IsInBounds(x, y))
            {
                _dataPointIndexes.Add(index);
            }

            base.MoveTo(context, x, y, index);
        }

        public override void Draw(IRenderContext2D context, IEnumerable<Point> centers)
        {
            TryCacheResources(context);

            var markerLocations = centers.ToArray();
            var locationIndex = 0;
            var prevValue = 0d;

            for (int i = 0; i < _dataPointMetadata.Count; ++i)
            {
                if (_dataPointMetadata[i] is BudgetPointMetadata metadata)
                {
                    var isGain = metadata.GainLossValue >= prevValue;
                    prevValue = metadata.GainLossValue;

                    if (_dataPointIndexes.Contains(i))
                    {
                        var center = markerLocations[locationIndex];
                        var gainLossValue = metadata.GainLossValue + "$";

                        DrawDiamond(context, center, Width, Height, _strokePen, isGain ? _gainFillBrush : _lossFillBrush);

                        _textBlock.Text = gainLossValue;
                        _textBlock.MeasureArrange();

                        var xPos = center.X - _textBlock.DesiredSize.Width / 2;
                        xPos = xPos < 0 ? TextIndent : xPos;

                        var marginalRightPos = context.ViewportSize.Width - _textBlock.DesiredSize.Width - TextIndent;
                        xPos = xPos > marginalRightPos ? marginalRightPos : xPos;

                        var yPos = center.Y;
                        var yOffset = isGain ? -_textBlock.DesiredSize.Height - TextIndent : TextIndent;
                        yPos += yOffset;

                        var textRect = new Rect(xPos, yPos, _textBlock.DesiredSize.Width, _textBlock.DesiredSize.Height);
                        context.DrawText(textRect,
                                        !metadata.IsCheckPoint ? isGain ? GainMarkerFill : LossMarkerFill : _checkedPointMarkerTextColor,
                                        TextSize,
                                        gainLossValue,
                                        FontFamily,
                                        FontWeight,
                                        FontStyle);

                        locationIndex++;
                    }
                }
            }
        }

        private void TryCacheResources(IRenderContext2D context)
        {
            if (!context.IsCompatibleType(_strokePen))
                Dispose();

            _strokePen ??= context.CreatePen(Stroke, AntiAliasing, (float)StrokeThickness, Opacity);
            _gainFillBrush ??= context.CreateBrush(GainMarkerFill);
            _lossFillBrush ??= context.CreateBrush(LossMarkerFill);
        }

        private void DrawDiamond(IRenderContext2D context, Point center, double width, double height, IPen2D stroke, IBrush2D fill)
        {
            double top = center.Y - height;
            double bottom = center.Y + height;
            double left = center.X - width;
            double right = center.X + width;

            var diamondPoints = new[]
            {
                // Points drawn like this:
                // 
                //      x0      (x4 in same location as x0)
                // 
                // x3        x1
                //
                //      x2
                new Point(center.X, top),
                new Point(right, center.Y),
                new Point(center.X, bottom),
                new Point(left, center.Y),
                new Point(center.X, top),
            };

            context.FillPolygon(fill, diamondPoints);
            context.DrawLines(stroke, diamondPoints);
        }

        public override void Dispose()
        {
            base.Dispose();

            _strokePen.SafeDispose();
            _strokePen = null;

            _gainFillBrush.SafeDispose();
            _gainFillBrush = null;

            _lossFillBrush.SafeDispose();
            _lossFillBrush = null;
        }
    }
}