﻿using System;
using SciChart.Charting.Model.ChartSeries;


namespace SciChart.Examples.Examples.UseSciChartWithMvvm.SciChartMVVMBinding
{
    public class ArrowAnnotationViewModel : BaseAnnotationViewModel
    {
        public override Type ViewType
        {
            get { return typeof(ArrowAnnotation); }
        }
    }
}
