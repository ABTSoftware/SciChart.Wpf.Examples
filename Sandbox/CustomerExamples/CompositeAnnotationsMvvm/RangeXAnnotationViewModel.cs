using System;
using SciChart.Charting.Model.ChartSeries;

namespace CompositeAnnotationsMvvmExample
{
    public class RangeXAnnotationViewModel : CompositeAnnotationViewModel
    {
        public override Type ViewType { get { return typeof(RangeXAnnotation); } }
    }
}