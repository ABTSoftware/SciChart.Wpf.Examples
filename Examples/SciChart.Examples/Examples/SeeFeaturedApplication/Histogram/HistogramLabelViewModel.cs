// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HistogramLabelViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Histogram
{
    public class HistogramLabelViewModel
    {
        public HistogramLabelViewModel(IComparable x1, IComparable y1, string text)
        {
            X1 = x1;
            Y1 = y1;
            LabelText = text;
        }

        /// <summary>
        /// LabelText will allow us to bind to TextAnnotation.Text
        /// </summary>
        public string LabelText { get; set; }

        /// <summary>
        /// X1 defines the X Data-Value for positioning the annotation
        /// </summary>
        public IComparable X1 { get; set; }

        /// <summary>
        /// Y1 defines the Y Data-Value for positioning the annotation
        /// </summary>
        public IComparable Y1 { get; set; }
    }
}