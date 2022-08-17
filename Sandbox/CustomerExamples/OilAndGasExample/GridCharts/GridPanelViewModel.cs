using System.Collections.ObjectModel;
using OilAndGasExample.Common;
using OilAndGasExample.GridCharts.ChartFactory;
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
                new ChartViewModel(new MountainChartFactory("Grid-1.csv.gz")),
                new ChartViewModel(new ScatterChartFactory("Grid-2.csv.gz")),
                new ChartViewModel(new ScatterChartFactory("Grid-3.csv.gz")),

                new ChartViewModel(new ScatterChartFactory("Grid-4.csv.gz")),
                new ChartViewModel(new MountainChartFactory("Grid-5.csv.gz")),
                new ChartViewModel(new ScatterChartFactory("Grid-6.csv.gz")),
                
                new ChartViewModel(new ScatterChartFactory("Grid-7.csv.gz")),
                new ChartViewModel(new ScatterChartFactory("Grid-8.csv.gz")),
                new ChartViewModel(new MountainChartFactory("Grid-9.csv.gz"))
            };
        }
    }    
}