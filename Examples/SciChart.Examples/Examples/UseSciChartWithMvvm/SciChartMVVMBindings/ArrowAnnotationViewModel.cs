// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ArrowAnnotationViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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
