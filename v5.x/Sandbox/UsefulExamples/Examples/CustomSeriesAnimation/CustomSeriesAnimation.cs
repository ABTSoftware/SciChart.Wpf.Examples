using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting.Visuals.RenderableSeries.Animations;
using SciChart.Data.Model;
using System;

namespace SciChart.Sandbox.Examples.CustomSeriesAnimation
{
    public class CustomSeriesAnimation : PointSeriesAnimationBase
    {
        /// <inheritdoc/>
        protected override IRenderPassData Animate(IRenderPassData rpd, double currentProgress)
        {
            if (rpd?.PointSeries != null)
            {
                CustomTransform(rpd.PointSeries, currentProgress);
            }

            return rpd;
        }

        private void CustomTransform(IPointSeries pointSeries, double currentProgress)
        {
            if (pointSeries.Count > 1)
            {
                lock (RenderableSeries.DataSeries.SyncRoot)
                {
                    if (pointSeries.Count > 1)
                    {
                        var nextPointIndex = pointSeries.Count - 1;
                        var lastPointIndex = pointSeries.Count - 2;

                        var nextIndex = pointSeries.Indexes[nextPointIndex];
                        var lastIndex = pointSeries.Indexes[lastPointIndex];

                        var nextXValue = Convert.ToDouble(RenderableSeries.DataSeries.XValues[nextIndex]);
                        var lastXValue = Convert.ToDouble(RenderableSeries.DataSeries.XValues[lastIndex]);

                        var nextYValue = Convert.ToDouble(RenderableSeries.DataSeries.YValues[nextIndex]);
                        var lastYValue = Convert.ToDouble(RenderableSeries.DataSeries.YValues[lastIndex]);

                        pointSeries.XValues[nextPointIndex] = lastXValue + currentProgress * (nextXValue - lastXValue);
                        pointSeries.YValues[nextPointIndex] = lastYValue + currentProgress * (nextYValue - lastYValue);
                    }
                }
            }
        }
    }
}
