using System.Linq;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Core.Utility.Mouse;

namespace SelectSeriesOnHover
{
    public class HoverSelectionModifier : ChartModifierBase
    {
        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            // Translate mouse point to modifier surface (required for left/top axis scenarios)
            var mousePoint = GetPointRelativeTo(e.MousePoint, ModifierSurface);

            // Call Series.HitTestProvider.HitTest to find which series is under the mouse point 
            var selectedSeries = this.ParentSurface.RenderableSeries.FirstOrDefault(x => x.HitTestProvider.HitTest(e.MousePoint).IsHit);

            // Apply the selection
            DeselectAllBut(selectedSeries);
        }
        
        private void DeselectAllBut(IRenderableSeries series)
        {
            DeselectAll();
            if (series != null)
            {
                series.IsSelected = true;
            }
        }
        
        private void DeselectAll()
        {
            ParentSurface.RenderableSeries.ForEachDo(rs => rs.IsSelected = false);
        }
    }
}