using System.Collections.Generic;
using SciChart.Charting.Model.ChartSeries;

namespace OilAndGasExample.VerticalCharts.ChartTypes
{
    public interface IChartInitializer
    {
        string ChartTitle { get; }

        IAxisViewModel GetXAxis();

        IAxisViewModel GetYAxis();

        IEnumerable<IRenderableSeriesViewModel> GetSeries();
    }
}