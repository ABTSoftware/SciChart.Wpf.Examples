// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SelectedRangePaletteProviderBase.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.StyleAChart.UsePaletteProvider
{
    public class SelectedRangePaletteProviderBase: DependencyObject, IPaletteProvider
    {
        public static readonly DependencyProperty AreaSelectionProperty =
            DependencyProperty.Register("AreaSelection", typeof (AreaSelection),
                typeof (SelectedRangePaletteProviderBase), new PropertyMetadata(null));

        public Color? PointMarkerStrokeOverride { get; set; }

        public Color? PointMarkerFillOverride { get; set; }

        public Color StrokeOverride { get; set; }

        public AreaSelection AreaSelection
        {
            get { return (AreaSelection)GetValue(AreaSelectionProperty); }
            set { SetValue(AreaSelectionProperty, value); }
        }

        protected bool IsColorOverridden(double xValue)
        {
            return AreaSelection != null && AreaSelection.Contains(xValue);
        }

        public void OnBeginSeriesDraw(IRenderableSeries series) { }
    }
}
