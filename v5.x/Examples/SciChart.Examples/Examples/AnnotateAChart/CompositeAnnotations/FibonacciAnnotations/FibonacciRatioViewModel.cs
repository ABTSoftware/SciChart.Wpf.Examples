// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// FibonacciRatioViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows.Media;

namespace SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.FibonacciAnnotations
{
    public class FibonacciRatioViewModel
    {
        public FibonacciRatioViewModel(double y1, Brush stroke)
        {
            Y1 = 1 - y1;
            Stroke = stroke;
        }

        /// <summary>
        /// Y1 defines the Y Data-Value for positioning the annotation
        /// </summary>
        public double Y1 { get; set; }

        /// <summary>
        /// LabelText will allow us to bind to TextAnnotation.Text
        /// </summary>
        public string LabelValue { get { return string.Format("{0:#0.##%}", (Y1 >= 0 && Y1 <= 1 ? 1 - Y1 : Math.Abs(Y1 - 1))); } }

        public Brush Stroke{ get; set; }
    }
}