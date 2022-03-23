using System.Collections.Generic;
using SciChart.Charting.Model.ChartSeries;

namespace OilAndGasExample.Common
{
    public interface IChartFactory
    {
        string Title { get; }

        string StyleKey { get; }

        IAxisViewModel GetXAxis();

        IAxisViewModel GetYAxis();

        IEnumerable<IRenderableSeriesViewModel> GetSeries();
    }
}