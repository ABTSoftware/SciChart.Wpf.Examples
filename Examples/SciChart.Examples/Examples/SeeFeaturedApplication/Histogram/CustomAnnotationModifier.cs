// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomAnnotationModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections;
using System.Windows;
using SciChart.Charting.ChartModifiers;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Histogram
{
    public class CustomAnnotationModifier : ChartModifierBase
    {
        public static readonly DependencyProperty LabelsSourceProperty = DependencyProperty.Register("LabelsSource", typeof(IEnumerable), typeof(CustomAnnotationModifier), new PropertyMetadata(null, OnLabelsSourceChanged));

        public IEnumerable LabelsSource
        {
            get { return (IEnumerable)GetValue(LabelsSourceProperty); }
            set { SetValue(LabelsSourceProperty, value); }
        }

        /// <summary>
        /// Called when the Chart Modifier is attached to the Chart Surface
        /// </summary>
        public override void OnAttached()
        {
            base.OnAttached();

            // Catch the condition where LabelsSource binds before chart is shown. Rebuild annotations
            if (LabelsSource != null && base.ParentSurface.Annotations.Count == 0)
            {
                RebuildAnnotations();
            }
        }

        private static void OnLabelsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var modifier = (CustomAnnotationModifier) d;
            var newValue = e.NewValue as IEnumerable;
            if (newValue != null)
            {
                modifier.RebuildAnnotations();
            }
        }

        private void RebuildAnnotations()
        {
            if (base.ParentSurface != null && LabelsSource != null)
            {
                // Take a look at the base class, ChartModifierBase for a wealth of API methods and properties to manipulate the SciChartSurface
                var annotationCollection = base.ParentSurface.Annotations;
                annotationCollection.Clear();

                foreach (var item in LabelsSource)
                {
                    annotationCollection.Add(new HistogramLabelAnnotation { DataContext = item });
                }
            }
        }
    }
}
