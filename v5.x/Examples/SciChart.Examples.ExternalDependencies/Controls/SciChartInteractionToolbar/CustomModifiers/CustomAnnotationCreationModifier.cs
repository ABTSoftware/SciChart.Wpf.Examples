// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomAnnotationCreationModifier.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals.Annotations;

namespace SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar.CustomModifiers
{
    public class CustomAnnotationCreationModifier : AnnotationCreationModifier
    {
        public ICommand AddAnnotation
        {
            get
            {
                return new ActionCommand(() =>
                {
                    IsEnabled = true;
                    if (AnnotationType == null)
                    {
                        AnnotationType = typeof(LineAnnotation);
                    }

                    var resourceAnnotatinStyles = new ResourceDictionary();
                    resourceAnnotatinStyles.Source =
                        new Uri(
                            "/SciChart.Examples.ExternalDependencies;component/Resources/Styles/AnnotationStylesResourceDictionary.xaml",
                            UriKind.RelativeOrAbsolute);

                    var resourceName = string.Format("{0}Style", AnnotationType.Name);
                    var annotationStyle = (Style)resourceAnnotatinStyles[resourceName];

                    if (annotationStyle != null)
                        AnnotationStyle = annotationStyle;
                });
            }
        }

        public ICommand DeleteAnnotation
        {
            get
            {
                return new ActionCommand(() =>
                {
                    if (ParentSurface != null)
                    {
                        var selectedAnnotations =
                            ParentSurface.Annotations.Where(annotation => annotation.IsSelected).ToList();
                        foreach (var selectedAnnotation in selectedAnnotations)
                        {
                            ParentSurface.Annotations.Remove(selectedAnnotation);
                        }
                    }
                });
            }
        }
    }
}
