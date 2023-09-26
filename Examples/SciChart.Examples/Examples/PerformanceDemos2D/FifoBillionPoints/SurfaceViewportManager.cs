using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals;
using SciChart.Core.Framework;
using SciChart.Drawing.Common;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class SurfaceViewportManager : DefaultViewportManager
    {
        public ISuspendable ParentSurface { get; private set;}

        public IRenderSurface RenderSurface { get; private set; }

        public override void AttachSciChartSurface(ISciChartSurface scs)
        {
            base.AttachSciChartSurface(scs);

            ParentSurface = scs;

            RenderSurface = scs.RenderSurface;
        }
    }
}