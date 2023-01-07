﻿using System;
using System.Windows;
using SciChart.Charting.Visuals.Annotations;

namespace CustomCompositeAnnotationExampleExample
{
    public partial class CustomCompositeAnnotationExample : Window
    {
        public CustomCompositeAnnotationExample()
        {
            InitializeComponent();

            DataContext = new CustomCompositeAnnotationViewModel();
            AnnotationCreation.IsEnabled = false;
        }

        private void OnAnnotationCreated(object sender, EventArgs e)
        {
            var newAnnotation = (AnnotationCreation.Annotation as AnnotationBase);
            if (newAnnotation != null)
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

            LineSquare.IsChecked = false;
        }

        private void AnnotationTypeUnchecked(object sender, RoutedEventArgs e)
        {
            AnnotationCreation.IsEnabled = false;
        }

        private void LineSquare_OnChecked(object sender, RoutedEventArgs e)
        {
            AnnotationCreation.IsEnabled = true;

            AnnotationCreation.AnnotationType = typeof(CompositeLineSquareAnnotation);
        }
    }
}