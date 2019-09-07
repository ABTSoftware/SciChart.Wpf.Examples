using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting.Visuals;

namespace CustomPointMarker
{
    /// <summary>
    /// A behaviour that allows zoom extents on a SciChartSurface when a Visibility Checbox is checked in the Legend
    /// </summary>
    public static class CheckedChangeZoomExtentsBehaviour
    {
        public static readonly DependencyProperty EnableZoomExtentsOnCheckedProperty = DependencyProperty.RegisterAttached(
            "EnableZoomExtentsOnChecked", typeof (bool), typeof (CheckedChangeZoomExtentsBehaviour), new PropertyMetadata(default(bool), OnEnableZoomExtentsOnChecked));        

        public static void SetEnableZoomExtentsOnChecked(DependencyObject element, bool value)
        {
            element.SetValue(EnableZoomExtentsOnCheckedProperty, value);
        }

        public static bool GetEnableZoomExtentsOnChecked(DependencyObject element)
        {
            return (bool) element.GetValue(EnableZoomExtentsOnCheckedProperty);
        }

        private static void OnEnableZoomExtentsOnChecked(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var checkbox = d as CheckBox;
            bool enabled = (bool)e.NewValue;

            checkbox.Checked -= CheckboxChecked;
            checkbox.Unchecked -= CheckboxChecked;
            if (enabled)
            {
                checkbox.Checked += CheckboxChecked;
                checkbox.Unchecked += CheckboxChecked;    
            }
        }

        private static void CheckboxChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            var scs = checkbox.FindVisualParent<SciChartSurface>();
            if (scs != null) scs.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
        }

        private static T FindVisualParent<T>(this DependencyObject dependencyObject)
            where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
 
            if (parent == null) return null;
 
            var parentT = parent as T;
            return parentT ?? FindVisualParent<T>(parent);
        }
    }
}
