using System;
using System.Windows;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting.Visuals.RenderableSeries.HitTesters;
using SciChart.Core.Extensions;

namespace SlimLineRenderableSeriesExample
{
    public class SlimLineHitTestProvider : DefaultHitTestProvider<SlimLineRenderableSeries>
    {
        public SlimLineHitTestProvider(SlimLineRenderableSeries renderSeries) : base(renderSeries)
        {
        }

        protected override HitTestInfo InterpolatePoint(Point rawPoint, HitTestInfo nearestHitResult, double hitTestRadius)
        {
            if (!nearestHitResult.IsEmpty())
            {
                var prevDataPointIndex = nearestHitResult.DataSeriesIndex;
                var nextDataPointIndex = nearestHitResult.DataSeriesIndex + 1;

                if (nextDataPointIndex >= 0 && nextDataPointIndex < RenderableSeries.DataSeries.Count)
                {
                    var prevAndNextYValues = new Tuple<double, double>(
                        ((IComparable)RenderableSeries.DataSeries.YValues[prevDataPointIndex]).ToDouble(),
                        ((IComparable)RenderableSeries.DataSeries.YValues[nextDataPointIndex]).ToDouble());

                    nearestHitResult = InterpolatePoint(rawPoint, nearestHitResult, hitTestRadius, prevAndNextYValues);
                }
            }

            return nearestHitResult;
        }
    }
}