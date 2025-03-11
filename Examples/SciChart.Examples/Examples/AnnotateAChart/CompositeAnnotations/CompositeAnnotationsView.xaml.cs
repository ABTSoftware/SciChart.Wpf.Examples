﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CompositeAnnotationsView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.FibonacciAnnotations;
using SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.MeasureAnnotations;

namespace SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations
{
    public partial class CompositeAnnotationsView : UserControl
    {
        public CompositeAnnotationsView()
        {
            InitializeComponent();
            AnnotationCreation.IsEnabled = false;
        }

        private void UncheckAnnotationToggles(string exceptName = null)
        {
            foreach (var annotationToggle in new[] { FibRetr, FibExt, MeasureX, MeasureY, MeasureXy })
            {
                if (annotationToggle.Name != exceptName)
                {
                    annotationToggle.IsChecked = false;
                }
            }
        }

        private void OnAnnotationCreated(object sender, EventArgs e)
        {
            if (AnnotationCreation.Annotation is AnnotationBase newAnnotation)
            {
                newAnnotation.IsEditable = true;
                newAnnotation.CanEditText = true;
            }

            if (AnnotationCreation != null)
            {
                AnnotationCreation.IsEnabled = false;
            }

            foreach (var annotation in SciChart.Annotations)
            {
                annotation.IsEditable = true;
            }

            UncheckAnnotationToggles();
        }

        private void FibonacciRetracementChecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggle)
            {
                UncheckAnnotationToggles(toggle.Name);
                AnnotationCreation.IsEnabled = true;
                AnnotationCreation.AnnotationType = typeof(FibonacciRetracementAnnotation);
            }
        }

        private void FibonacciExtensionChecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggle)
            {
                UncheckAnnotationToggles(toggle.Name);
                AnnotationCreation.IsEnabled = true;
                AnnotationCreation.AnnotationType = typeof(FibonacciExtensionAnnotation);
            }
        }

        private void MeasureXChecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggle)
            {
                UncheckAnnotationToggles(toggle.Name);
                AnnotationCreation.IsEnabled = true;
                AnnotationCreation.AnnotationType = typeof(MeasureXAnnotation);
            }
        }

        private void MeasureYChecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggle)
            {
                UncheckAnnotationToggles(toggle.Name);
                AnnotationCreation.IsEnabled = true;
                AnnotationCreation.AnnotationType = typeof(MeasureYAnnotation);
            }
        }

        private void MeasureXyChecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggle)
            {
                UncheckAnnotationToggles(toggle.Name);
                AnnotationCreation.IsEnabled = true;
                AnnotationCreation.AnnotationType = typeof(MeasureXyAnnotation);
            }
        }

        private void AnnotationTypeUnchecked(object sender, RoutedEventArgs e)
        {
            AnnotationCreation.IsEnabled = false;
            AnnotationCreation.AnnotationType = null;           
        }
    }
}