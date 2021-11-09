using SciChart.Charting.Visuals;
using SciChart.Drawing.Common;
using SciChart.Examples.ExternalDependencies.Helpers;

namespace SciChart_DigitalAnalyzerPerformanceDemo
{
    /// <summary>
    /// Uses hardware acceleration for rendering by default, if supported.
    /// </summary>
    public class SciChartSurfaceEx : SciChartSurface
    {
        public SciChartSurfaceEx()
        {
            // Allow sync rendering across multiple surfaces
            RenderSurfaceBase.UseThreadedRenderTimer = true;

            // Try enabling VisualXccelerator renderer by default, if supported
            VisualXcceleratorEngineHelper.ForceEnable(this);
        }
    }
}
