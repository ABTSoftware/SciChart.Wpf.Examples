// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CustomAnnotationCreationModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals.Annotations;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers
{
    public class CustomAnnotationCreationModifier : AnnotationCreationModifier
    {
        public Type SelectedType
        {
            get => AnnotationType;
            set
            {
                IsEnabled = true;
                AnnotationType = value;
                ApplyAnnotationStyle();
            }
        }

        public ICommand DeleteAnnotation { get; }

        public CustomAnnotationCreationModifier()
        {
            SelectedType = typeof(LineAnnotation);
            DeleteAnnotation = new ActionCommand(OnDeleteAnnotation);
        }
        private void ApplyAnnotationStyle()
        {
            var resourceAnnotatinStyles = new ResourceDictionary
            {
                Source = new Uri("/SciChart.Examples.ExternalDependencies;component/Resources/Styles/Annotations.xaml",
                    UriKind.RelativeOrAbsolute)
            };

            var resourceName = string.Format("{0}Style", AnnotationType.Name);
            var annotationStyle = (Style)resourceAnnotatinStyles[resourceName];

            if (annotationStyle != null)
                AnnotationStyle = annotationStyle;
        }

        private void OnDeleteAnnotation()
        {
            if (ParentSurface != null)
            {
                var selectedAnnotations = ParentSurface.Annotations.Where(annotation => annotation.IsSelected).ToList();
                
                foreach (var selectedAnnotation in selectedAnnotations)
                {
                    ParentSurface.Annotations.Remove(selectedAnnotation);
                }
            }
        }
    }
}
