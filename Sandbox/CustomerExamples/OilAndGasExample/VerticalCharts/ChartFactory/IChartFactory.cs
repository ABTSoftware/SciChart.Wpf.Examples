using System.Collections.Generic;
using SciChart.Charting.Model.ChartSeries;

namespace OilAndGasExample.VerticalCharts.ChartFactory
{
    public interface IChartFactory
    {
        string Title { get; }

        IAxisViewModel GetXAxis();

        IAxisViewModel GetYAxis();

        IEnumerable<IRenderableSeriesViewModel> GetSeries();
    }
}