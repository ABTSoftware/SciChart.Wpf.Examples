// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// VerticalValuesPaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Visuals.PaletteProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.WaterfallChart
{
    public class VerticalValuesPaletteProvider : DependencyObject, IStrokePaletteProvider
    {
        public int SelectedIndex { get; set; }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            if (rSeries.IsSelected)
            {
                return Colors.Red;
            }

            if (index == SelectedIndex)
            {
                return Colors.Goldenrod;
            }

            return null;
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }
    }
}
