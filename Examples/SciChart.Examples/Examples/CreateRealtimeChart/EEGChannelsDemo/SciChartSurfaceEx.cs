using SciChart.Charting.Visuals;
using SciChart.Examples.ExternalDependencies.Helpers;

namespace SciChart.Examples.Examples.CreateRealtimeChart.EEGChannelsDemo
{
    /// <summary>
    /// Uses hardware acceleration for rendering by default, if supported.
    /// </summary>
    public class SciChartSurfaceEx : SciChartSurface
    {
        public SciChartSurfaceEx()
        {
            // Try enabling VisualXccelerator renderer by default, if supported
            VisualXcceleratorEngineHelper.ForceEnable(this);
        }
    }
}
