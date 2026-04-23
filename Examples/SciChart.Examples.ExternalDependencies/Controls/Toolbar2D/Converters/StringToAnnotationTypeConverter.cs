// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// StringToAnnotationTypeConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows.Data;
using SciChart.Charting.Visuals.Annotations;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.Converters
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
            if (value is Type annotationType)
            {
                return annotationType.Name;
            }
            return typeof(LineAnnotation).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            { 
                return typeof(LineAnnotation);
            }

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

                default: return null;
            }
        }
    }
}
