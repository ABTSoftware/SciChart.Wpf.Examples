using System.Collections.Generic;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.Visuals.RenderableSeries;

namespace OilAndGasExample.Common
{
    public interface IChart3DFactory
    {
        string Title { get; }

        string StyleKey { get; }

        IAxis3DViewModel GetXAxis();

        IAxis3DViewModel GetYAxis();

        IAxis3DViewModel GetZAxis();

        IEnumerable<IRenderableSeries3DViewModel> GetSeries();
    }
}