﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// PaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Media;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.RenderableSeries;

namespace SciChart.Examples.Examples.Charts3D.TooltipsAndHitTest3DCharts
{
    public class ExamplePointCloudPaletteProvider : IPointMarkerPaletteProvider3D
    {
        public Color? OverridePointMarker(IRenderableSeries3D series, int index, IPointMetadata3D metadata)
        {
            return metadata?.IsSelected == true ? Color.FromArgb(0xFF, 0xDC, 0x79, 0x69) : Color.FromArgb(0xFF, 0x64, 0xBA, 0xE4);
        }

        public void OnAttach(IRenderableSeries3D renderSeries)
        {
        }

        public void OnDetached()
        {
        }
    }
}