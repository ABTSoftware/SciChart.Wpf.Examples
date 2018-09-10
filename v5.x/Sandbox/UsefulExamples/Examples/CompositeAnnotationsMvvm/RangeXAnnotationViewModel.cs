using System;
using SciChart.Charting.Model.ChartSeries;

namespace SciChart.Sandbox.Examples.CompositeAnnotationsMvvm
{
    public class RangeXAnnotationViewModel : CompositeAnnotationViewModel
    {
        public override Type ViewType { get { return typeof(RangeXAnnotation); } }
    }
}