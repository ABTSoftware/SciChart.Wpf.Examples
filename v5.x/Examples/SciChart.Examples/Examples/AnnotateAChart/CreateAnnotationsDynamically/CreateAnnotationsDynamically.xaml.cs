// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateAnnotationsDynamically.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.AnnotateAChart.CreateAnnotationsDynamically
{
    public partial class CreateAnnotationsDynamically : UserControl
    {
        private readonly IDictionary<string, Type> _annotationTypes = new Dictionary<string, Type>()
        {
            { "LineAnnotation", typeof(LineAnnotation)},
            { "LineArrowAnnotation", typeof(LineArrowAnnotation)},
            { "TextAnnotation", typeof(TextAnnotation)},
            { "BoxAnnotation", typeof(BoxAnnotation)},
            { "HorizontalLineAnnotation", typeof(HorizontalLineAnnotation)},
            { "VerticalLineAnnotation", typeof(VerticalLineAnnotation)},
            { "AxisMarkerAnnotation", typeof(AxisMarkerAnnotation)},
            { "MyCustomAnnotation", typeof(MyCustomAnnotation)}
        };

        public CreateAnnotationsDynamically()
        {
            InitializeComponent();
        }

        void CreateAnnotationsDynamicallyLoaded(object sender, RoutedEventArgs e)
        {
            var dataSeries = new OhlcDataSeries<DateTime, double>();

            var marketDataService = new MarketDataService(DateTime.Now, 5, 5);
            var data = marketDataService.GetHistoricalData(200);
            dataSeries.Append(data.Select(x => x.DateTime), data.Select(x => x.Open), data.Select(x => x.High), data.Select(x => x.Low), data.Select(x => x.Close));

            sciChart.RenderableSeries[0].DataSeries = dataSeries;
            pointerButton.IsChecked = true;
            AnnotationTypes.ItemsSource = _annotationTypes.Keys;
        }

        private void OnDeleteSelectedAnnotationsClick(object sender, RoutedEventArgs e)
        {
            var selectedAnnotations = sciChart.Annotations.Where(annotation => annotation.IsSelected).ToList();

            foreach (var selectedAnnotation in selectedAnnotations)
            {
                sciChart.Annotations.Remove(selectedAnnotation);
            }
        }

       
        private void OnAnnotationCreated(object sender, EventArgs e)
        {
            var newAnnotation = (annotationCreation.Annotation as AnnotationBase);
            if (newAnnotation != null)
            {
                newAnnotation.IsEditable = true;
                newAnnotation.CanEditText = true;
            }
            pointerButton.IsChecked = true;
        }

        private void OnEditingEnabled(object sender, RoutedEventArgs e)
        {
            if (annotationCreation != null)
                annotationCreation.IsEnabled = false;

            foreach (var annotation in sciChart.Annotations)
            {
                annotation.IsEditable = true;
            }
        }

        private void OnEditDisabled(object sender, RoutedEventArgs e)
        {
            annotationCreation.IsEnabled = true;
        }

        private void AnnotationTypes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pointerButton.IsChecked = false;

            var type = _annotationTypes[(string)AnnotationTypes.SelectedValue];

            var resourceName = String.Format("{0}Style", type.Name);
            if (Resources.Contains(resourceName))
            {
                annotationCreation.AnnotationStyle = (Style)Resources[resourceName];
            }
            annotationCreation.AnnotationType = type;
        }

        private void AddAnnotation_OnClick(object sender, RoutedEventArgs e)
        {
            pointerButton.IsChecked = false;
        }
    }
}
