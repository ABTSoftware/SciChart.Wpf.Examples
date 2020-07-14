using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Drawing.Common;

namespace State_Series_Example
{
    public class StateRenderableSeries : CustomRenderableSeries
    {
        protected override void Draw(IRenderContext2D renderContext, IRenderPassData renderPassData)
        {
            base.Draw(renderContext, renderPassData);

            double marginTop = this.Margin.Top;
            double marginBottom = this.Margin.Bottom;

            var pointMarker = base.GetPointMarker();
            if (pointMarker != null)
            {
                // Begin a batched PointMarker draw operation
                pointMarker.BeginBatch(renderContext, pointMarker.Stroke, pointMarker.Fill);

                for (int i = 0; i < renderPassData.PointSeries.Count; i++)
                {
                    double x = renderPassData.PointSeries.XValues[i];

                    double xCoord = renderPassData.XCoordinateCalculator.GetCoordinate(x);
                    double yCoord = VerticalAlignment == VerticalAlignment.Top ? marginTop : renderContext.ViewportSize.Height - marginBottom;

                    pointMarker.MoveTo(renderContext, xCoord, yCoord, i);
                }

                // End the batch
                // Note: To change point color, start a new batch
                pointMarker.EndBatch(renderContext);
            }
        }

        public override IRange GetYRange(IRange xRange, bool getPositiveRange)
        {
            return new DoubleRange(double.NaN, double.NaN);
        }
    }
}