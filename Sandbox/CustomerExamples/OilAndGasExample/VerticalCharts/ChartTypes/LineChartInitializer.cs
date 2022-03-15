using System.Collections.Generic;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Visuals.Axes;

namespace OilAndGasExample.VerticalCharts.ChartTypes
{
    public class LineChartInitializer : IVerticalChartInitializer
    {
        public string ChartTitle => "TODO";
        public IAxisViewModel GetXAxis()
        {
            return new NumericAxisViewModel
            {
                AxisAlignment = AxisAlignment.Left,
                StyleKey = "VerticalChartAxisStyle"
            };
        }

        public IAxisViewModel GetYAxis()
        {
            return new NumericAxisViewModel
            {
                FlipCoordinates = true,
                AxisAlignment = AxisAlignment.Bottom,
                StyleKey = "VerticalChartAxisStyle"
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            return new List<IRenderableSeriesViewModel>();
        }
    }
}