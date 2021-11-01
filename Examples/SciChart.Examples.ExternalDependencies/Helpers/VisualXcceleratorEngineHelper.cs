using System;
using SciChart.Charting;
using SciChart.Charting.Common.AttachedProperties;
using SciChart.Charting.Visuals;
using SciChart.Drawing.HighSpeedRasterizer;
using SciChart.Drawing.VisualXcceleratorRasterizer;

namespace SciChart.Examples.ExternalDependencies.Helpers
{
    /// <summary>
    /// Helper class that is used to force usage of VisualXcceleratorEngine for rendering.
    /// </summary>
    public static class VisualXcceleratorEngineHelper
    {
        /// <summary>
        /// Creates <see cref="VisualXcceleratorRenderSurface"/> and assigns it to <see cref="SciChartSurfaceBase.RenderSurface"/>.
        /// If hardware acceleration is not supported assigns an instance of <see cref="HighSpeedRenderSurface"/> instead.
        /// </summary>
        /// <param name="scs"></param>
        public static void ForceEnable(SciChartSurface scs)
        {
            VisualXcceleratorEngine.SetEnableImpossibleMode(scs, false);

            if (VisualXcceleratorEngine.GetAvoidBlacklistedGpu(scs) && VisualXcceleratorEngine.IsGpuBlacklisted)
            {
                SetDefaultRenderSurface(scs);
                throw new Exception("Your GPU is blacklisted for use by the Visual Xccelerator Engine");
            }

            if (VisualXcceleratorEngine.SupportsHardwareAcceleration)
            {
                if (scs.IsDisposed) return;

                try
                {
                    PerformanceHelper.SetEnableExtremeResamplers(scs, true);
                    scs.SetCurrentValue(SciChartSurfaceBase.RenderSurfaceProperty, new VisualXcceleratorRenderSurface());
                    return;
                }
                catch (Exception ex)
                {
                    SetDefaultRenderSurface(scs);
                    throw new Exception("The Visual Xccelerator Engine or one of it's classes is not supported on this computer", ex);
                }
            }

            SetDefaultRenderSurface(scs);
        }

        private static void SetDefaultRenderSurface(SciChartSurface scs)
        {
            scs.SetCurrentValue(SciChartSurfaceBase.RenderSurfaceProperty, new HighSpeedRenderSurface());
        }
    }
}
