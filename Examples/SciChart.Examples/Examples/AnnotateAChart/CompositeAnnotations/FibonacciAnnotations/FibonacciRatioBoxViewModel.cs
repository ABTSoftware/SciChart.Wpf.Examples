// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// FibonacciRatioBoxViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    public class FibonacciRatioBoxViewModel
    {
        public FibonacciRatioBoxViewModel(double y1, double y2, Brush background)
        {
            Y1 = 1 - y1;
            Y2 = 1 - y2;
            Background = background;
        }

        /// <summary>
        /// Y1 defines the Y1 Data-Value for positioning the annotation
        /// </summary>
        public double Y1 { get; set; }

        /// <summary>
        /// Y2 defines the Y2 Data-Value for positioning the annotation
        /// </summary>
        public double Y2 { get; set; }

        public Brush Background { get; set; }
    }
}