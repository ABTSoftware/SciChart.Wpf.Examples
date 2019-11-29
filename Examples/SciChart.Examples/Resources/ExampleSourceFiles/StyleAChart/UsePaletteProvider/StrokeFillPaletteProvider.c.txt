// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// StrokeFillPaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using System.Text;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.StyleAChart.UsePaletteProvider
{
    public class StrokeFillPaletteProvider : SelectedRangePaletteProviderBase, IStrokePaletteProvider, IFillPaletteProvider, IPointMarkerPaletteProvider
    {
        private PointPaletteInfo _pointMarkerColorOverrides = new PointPaletteInfo();

        public Brush FillBrushOverride { get; set; }

        public Brush OverrideFillBrush(IRenderableSeries series, int index, IPointMetadata metadata)
        {
            var isOverridden = IsColorOverridden(index);

            if (isOverridden)
            {
                return FillBrushOverride;
            }

            return null;
        }

        public Color? OverrideStrokeColor(IRenderableSeries series, int index, IPointMetadata metadata)
        {
            var isOverridden = IsColorOverridden(index);

            if (isOverridden)
            {
                return StrokeOverride;
            }

            return null;
        }

        public PointPaletteInfo? OverridePointMarker(IRenderableSeries series, int index, IPointMetadata metadata)
        {
            var isOverridden = IsColorOverridden(index);

            if (isOverridden)
            {
                _pointMarkerColorOverrides.Fill = PointMarkerFillOverride;
                _pointMarkerColorOverrides.Stroke = PointMarkerStrokeOverride;

                return _pointMarkerColorOverrides;
            }

            return null;
        }
    }
}
