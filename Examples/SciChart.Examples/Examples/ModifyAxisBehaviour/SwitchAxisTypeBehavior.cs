﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SwitchAxisTypeBehavior.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    // This is an example class showing how attached properties can be created simply to 
    // change behaviour on a SciChartSurface. This is not part of SciChart API and is not supported
    // but serves as an example only
    public class SwitchAxisTypeBehavior
    {
        // Define the Default XAxis -- this can be done in XAML
        public static readonly DependencyProperty DefaultXAxisProperty = DependencyProperty.RegisterAttached
            ("DefaultXAxis", typeof(AxisBase), typeof(SwitchAxisTypeBehavior), new PropertyMetadata(default(AxisBase), OnXPropertyChanged));

        // Define the Alternative XAxis -- this can be done in XAML
        public static readonly DependencyProperty AlternativeXAxisProperty = DependencyProperty.RegisterAttached
            ("AlternativeXAxis", typeof(AxisBase), typeof(SwitchAxisTypeBehavior), new PropertyMetadata(default(AxisBase), OnXPropertyChanged));

        // Define a boolean attached property which switches between default and alternative XAxis
        public static readonly DependencyProperty UseAlternateXAxisProperty = DependencyProperty.RegisterAttached
            ("UseAlternateXAxis", typeof(bool), typeof(SwitchAxisTypeBehavior), new PropertyMetadata(default(bool), OnXPropertyChanged));

        // Define the Default YAxis -- this can be done in XAML
        public static readonly DependencyProperty DefaultYAxisProperty = DependencyProperty.RegisterAttached
            ("DefaultYAxis", typeof(AxisBase), typeof(SwitchAxisTypeBehavior), new PropertyMetadata(default(AxisBase), OnYPropertyChanged));

        // Define the Alternative YAxis -- this can be done in XAML
        public static readonly DependencyProperty AlternativeYAxisProperty = DependencyProperty.RegisterAttached
            ("AlternativeYAxis", typeof(AxisBase), typeof(SwitchAxisTypeBehavior), new PropertyMetadata(default(AxisBase), OnYPropertyChanged));

        // Define a boolean attached property which switches between default and alternative YAxis
        public static readonly DependencyProperty UseAlternateYAxisProperty = DependencyProperty.RegisterAttached
            ("UseAlternateYAxis", typeof(bool), typeof(SwitchAxisTypeBehavior), new PropertyMetadata(default(bool), OnYPropertyChanged));

        public static void SetDefaultXAxis(DependencyObject element, AxisBase value)
        {
            element.SetValue(DefaultXAxisProperty, value);
        }

        public static AxisBase GetDefaultXAxis(DependencyObject element)
        {
            return (AxisBase) element.GetValue(DefaultXAxisProperty);
        }

        public static void SetAlternativeXAxis(DependencyObject element, AxisBase value)
        {
            element.SetValue(AlternativeXAxisProperty, value);
        }

        public static AxisBase GetAlternativeXAxis(DependencyObject element)
        {
            return (AxisBase) element.GetValue(AlternativeXAxisProperty);
        }

        public static void SetUseAlternateXAxis(DependencyObject element, bool value)
        {
            element.SetValue(UseAlternateXAxisProperty, value);
        }

        public static bool GetUseAlternateXAxis(DependencyObject element)
        {
            return (bool) element.GetValue(UseAlternateXAxisProperty);
        }

        public static void SetDefaultYAxis(DependencyObject element, AxisBase value)
        {
            element.SetValue(DefaultYAxisProperty, value);
        }

        public static AxisBase GetDefaultYAxis(DependencyObject element)
        {
            return (AxisBase) element.GetValue(DefaultYAxisProperty);
        }

        public static void SetAlternativeYAxis(DependencyObject element, AxisBase value)
        {
            element.SetValue(AlternativeYAxisProperty, value);
        }

        public static AxisBase GetAlternativeYAxis(DependencyObject element)
        {
            return (AxisBase) element.GetValue(AlternativeYAxisProperty);
        }

        public static void SetUseAlternateYAxis(DependencyObject element, bool value)
        {
            element.SetValue(UseAlternateYAxisProperty, value);
        }

        public static bool GetUseAlternateYAxis(DependencyObject element)
        {
            return (bool) element.GetValue(UseAlternateYAxisProperty);
        }

        private static void OnXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // This is where we switch the axis type depending on boolean
            if (d is SciChartSurface scs)
            {
                bool useAlternateXAxis = GetUseAlternateXAxis(scs);
                scs.XAxis = useAlternateXAxis ? GetAlternativeXAxis(scs) : GetDefaultXAxis(scs);
                scs.RenderableSeries.ForEachDo(s => s.XAxisId = scs.XAxis.Id);
            }
        }

        private static void OnYPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // This is where we switch the axis type depending on boolean
            if (d is SciChartSurface scs)
            {
                bool useAlternateYAxis = GetUseAlternateYAxis(scs);
                scs.YAxis = useAlternateYAxis ? GetAlternativeYAxis(scs) : GetDefaultYAxis(scs);
                scs.RenderableSeries.ForEachDo(s => s.XAxisId = scs.XAxis.Id);
            }
        }
    }
}