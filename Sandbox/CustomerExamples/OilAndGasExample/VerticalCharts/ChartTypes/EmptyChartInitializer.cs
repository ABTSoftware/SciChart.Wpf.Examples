using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SciChart.Charting.Model.ChartSeries;

namespace OilAndGasExample.VerticalCharts.ChartTypes
{
    public class EmptyChartInitializer : IChartInitializer
    {
        public string ChartTitle => "TODO";

        public IAxisViewModel GetXAxis()
        {
            return new NumericAxisViewModel
            {
                Visibility = Visibility.Collapsed
            };
        }

        public IAxisViewModel GetYAxis()
        {
            return new NumericAxisViewModel
            {
                Visibility = Visibility.Collapsed
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            return Enumerable.Empty<IRenderableSeriesViewModel>();
        }
    }
}
