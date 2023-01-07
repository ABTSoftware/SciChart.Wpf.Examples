// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// OnRenderedActionViewportManager.cs is part of the SCICHART® Examples. Permission
// is hereby granted to modify, create derivative works, distribute and publish any part
// of this source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals;

namespace SciChart.Examples.Examples.InspectDatapoints.CustomSeriesValueMarkers
{
    public class OnRenderedActionViewportManager : DefaultViewportManager
    {
        public Action<ISciChartSurface> OnRenderedAction { get; }

        public OnRenderedActionViewportManager(Action<ISciChartSurface> onRenderedAction)
        {
            OnRenderedAction = onRenderedAction;
        }

        public override void OnParentSurfaceRendered(ISciChartSurface sciChartSurface)
        {
            base.OnParentSurfaceRendered(sciChartSurface);

            OnRenderedAction?.Invoke(sciChartSurface);
        }
    }
}