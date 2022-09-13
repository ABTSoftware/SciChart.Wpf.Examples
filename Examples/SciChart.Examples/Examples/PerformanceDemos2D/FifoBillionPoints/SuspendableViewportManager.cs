using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals;
using SciChart.Core.Framework;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class SuspendableViewportManager : DefaultViewportManager
    {
        public ISuspendable ParentSurface { get; private set;}

        public override void AttachSciChartSurface(ISciChartSurface scs)
        {
            base.AttachSciChartSurface(scs);

            ParentSurface = scs;
        }
    }
}