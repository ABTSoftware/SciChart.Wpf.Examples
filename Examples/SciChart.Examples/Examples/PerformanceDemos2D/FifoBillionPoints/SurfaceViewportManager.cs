// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SurfaceViewportManager.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals;
using SciChart.Core.Framework;
using SciChart.Drawing.Common;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class SurfaceViewportManager : DefaultViewportManager
    {
        private ISciChartSurface _parentSurface;
        private Action _renderSurfaceChangedAction;

        public SurfaceViewportManager(Action renderSurfaceChangedAction = null)
        {
            _renderSurfaceChangedAction = renderSurfaceChangedAction;
        }

        public ISuspendable ParentSurface => _parentSurface;

        public IRenderSurface RenderSurface { get; private set; }

        public override void AttachSciChartSurface(ISciChartSurface scs)
        {
            base.AttachSciChartSurface(scs);

            _parentSurface = scs;
            RenderSurface = scs.RenderSurface;

            _parentSurface.Rendered += OnSurfaceRendered;
        }

        public override void DetachSciChartSurface()
        {
            base.DetachSciChartSurface();

            _parentSurface.Rendered -= OnSurfaceRendered;
        }

        private void OnSurfaceRendered(object sender, EventArgs e)
        {
            if (!ReferenceEquals(RenderSurface, _parentSurface.RenderSurface))
            {
                RenderSurface = _parentSurface.RenderSurface;
                _renderSurfaceChangedAction();
            }
        }
    }
}