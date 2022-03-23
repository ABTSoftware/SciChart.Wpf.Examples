using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OilAndGasExample.Common;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Data.Model;

namespace OilAndGasExample.GridCharts
{
    public class GridPanelViewModel : BindableObject
    {
        public ObservableCollection<ChartViewModel> GridCharts { get; }

        public GridPanelViewModel()
        {
            GridCharts = new ObservableCollection<ChartViewModel>
            {
                new ChartViewModel(new EmptyChartFactory()),
                new ChartViewModel(new EmptyChartFactory()),
                new ChartViewModel(new EmptyChartFactory()),

                new ChartViewModel(new EmptyChartFactory()),
                new ChartViewModel(new EmptyChartFactory()),
                new ChartViewModel(new EmptyChartFactory()),
                
                new ChartViewModel(new EmptyChartFactory()),
                new ChartViewModel(new EmptyChartFactory()),
                new ChartViewModel(new EmptyChartFactory())
            };
        }
    }

    public class EmptyChartFactory : IChartFactory
    {
        public string Title => "Empty";

        public string StyleKey => null;

        public IAxisViewModel GetXAxis()
        {
            return new NumericAxisViewModel
            {
                Visibility = System.Windows.Visibility.Collapsed
            };
        }

        public IAxisViewModel GetYAxis()
        {
            return new NumericAxisViewModel
            {
                Visibility = System.Windows.Visibility.Collapsed
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {           
            return Enumerable.Empty<IRenderableSeriesViewModel>();
        }
    }
}