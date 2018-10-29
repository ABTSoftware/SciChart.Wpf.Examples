using System.Diagnostics;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Drawing.Common;

namespace SciChart.Sandbox.Examples.CustomSeriesMvvm
{
    /// <summary>
    /// The series type to create. See <see cref="LineRenderableSeriesViewModelEx"/> ViewType
    /// </summary>
    public class FastLineRenderableSeriesEx : FastLineRenderableSeriesForMvvm
    {
        protected override void InternalDraw(IRenderContext2D renderContext, IRenderPassData renderPassData)
        {
            base.InternalDraw(renderContext, renderPassData);
            Debug.WriteLine("Drawing! Custom series...");
        }
    }
}