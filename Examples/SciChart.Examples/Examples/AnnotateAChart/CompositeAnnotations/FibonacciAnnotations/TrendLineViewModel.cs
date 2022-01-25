// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// TrendLineViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Media;

namespace SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.FibonacciAnnotations
{
    public class TrendLineViewModel
    {
        public TrendLineViewModel(DoubleCollection strokeDashArray, double strokeThickness, Brush stroke)
        {
            StrokeDashArray = strokeDashArray;
            StrokeThickness = strokeThickness;
            Stroke = stroke;
        }

        public DoubleCollection StrokeDashArray { get; set; }

        public double StrokeThickness{ get; set; }

        public Brush Stroke { get; set; }
    }
}