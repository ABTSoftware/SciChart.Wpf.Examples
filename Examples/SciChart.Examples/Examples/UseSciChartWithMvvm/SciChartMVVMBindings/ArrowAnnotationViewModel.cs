using System;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Visuals.Annotations;


namespace SciChart.Examples.Examples.UseSciChartWithMvvm.SciChartMVVMBinding
{
    public class ArrowAnnotationViewModel : BaseAnnotationViewModel
    {
        private VerticalAnchorPoint _verticalAnchorPoint;
        private HorizontalAnchorPoint _horizontalAnchorPoint;

        public VerticalAnchorPoint VerticalAnchorPoint
        {
            get => _verticalAnchorPoint;
            set => SetValue(ref _verticalAnchorPoint, value, nameof(VerticalAnchorPoint));
        }

        public HorizontalAnchorPoint HorizontalAnchorPoint
        {
            get => _horizontalAnchorPoint;
            set => SetValue(ref _horizontalAnchorPoint, value, nameof(HorizontalAnchorPoint));
        }

        public override Type ViewType => typeof(ArrowAnnotation);
    }
}
