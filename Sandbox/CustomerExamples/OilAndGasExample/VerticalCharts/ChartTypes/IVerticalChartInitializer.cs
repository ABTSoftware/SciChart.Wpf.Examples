using System.Collections.Generic;
using SciChart.Charting.Model.ChartSeries;

namespace OilAndGasExample.VerticalCharts.ChartTypes
{
    public interface IVerticalChartInitializer
    {
        string ChartTitle { get; }

        IAxisViewModel GetXAxis();

        IAxisViewModel GetYAxis();

        IEnumerable<IRenderableSeriesViewModel> GetSeries();
    }
}