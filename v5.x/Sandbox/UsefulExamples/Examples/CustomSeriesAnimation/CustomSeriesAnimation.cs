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
                        var addedPointSeriesIndex = pointSeries.Count - 1;
                        var prevPointSeriesIndex = pointSeries.Count - 2;

                        // Find corresponding indexes in DataSeries
                        var addedDataSeriesIndex = pointSeries.Indexes[addedPointSeriesIndex];
                        var prevDataSeriesIndex = pointSeries.Indexes[prevPointSeriesIndex];

                        // Get X,Y data values from DataSeries by indexes
                        var addedXValue = Convert.ToDouble(RenderableSeries.DataSeries.XValues[addedDataSeriesIndex]);
                        var addedYValue = Convert.ToDouble(RenderableSeries.DataSeries.YValues[addedDataSeriesIndex]);

                        var prevXValue = Convert.ToDouble(RenderableSeries.DataSeries.XValues[prevDataSeriesIndex]);
                        var prevYValue = Convert.ToDouble(RenderableSeries.DataSeries.YValues[prevDataSeriesIndex]);

                        // Update last point in PointSeries using linear interpolation
                        // this PointSeries will be drawn in current frame
                        pointSeries.XValues[addedPointSeriesIndex] = prevXValue + currentProgress * (addedXValue - prevXValue);
                        pointSeries.YValues[addedPointSeriesIndex] = prevYValue + currentProgress * (addedYValue - prevYValue);
                    }
                }
            }
        }
    }
}
