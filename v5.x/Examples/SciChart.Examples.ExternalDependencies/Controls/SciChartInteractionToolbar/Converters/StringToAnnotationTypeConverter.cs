// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// StringToAnnotationTypeConverter.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar.Converters
{
    public enum AnnotationType
    {
        LineAnnotation,
        LineArrowAnnotation,
        TextAnnotation,
        BoxAnnotation,
        HorizontalLineAnnotation,
        VerticalLineAnnotation,
        AxisMarkerAnnotation,
        MyCustomAnnotation
    }

    public class StringToAnnotationTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? value.GetType().Name : typeof(LineAnnotation).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return typeof(LineAnnotation);

            var annotationType = (AnnotationType)Enum.Parse(typeof(AnnotationType), (string)value);

            switch (annotationType)
            {
                case AnnotationType.LineAnnotation:
                    return typeof(LineAnnotation);
                case AnnotationType.AxisMarkerAnnotation:
                    return typeof(AxisMarkerAnnotation);
                case AnnotationType.BoxAnnotation:
                    return typeof(BoxAnnotation);
                case AnnotationType.HorizontalLineAnnotation:
                    return typeof(HorizontalLineAnnotation);
                case AnnotationType.LineArrowAnnotation:
                    return typeof(LineArrowAnnotation);
                case AnnotationType.MyCustomAnnotation:
                    return typeof(MyCustomAnnotation);
                case AnnotationType.TextAnnotation:
                    return typeof(TextAnnotation);
                case AnnotationType.VerticalLineAnnotation:
                    return typeof(VerticalLineAnnotation);
                default:
                    return null;

            }
        }
    }
}
