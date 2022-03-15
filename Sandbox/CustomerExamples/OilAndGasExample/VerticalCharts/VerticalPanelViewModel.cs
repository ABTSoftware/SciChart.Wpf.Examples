using System.Collections.ObjectModel;
using OilAndGasExample.VerticalCharts.ChartTypes;
using SciChart.Data.Model;

namespace OilAndGasExample.VerticalCharts
{
    public class VerticalPanelViewModel : BindableObject
    {
        public ObservableCollection<VerticalChartViewModel> VerticalCharts { get; }

        public VerticalPanelViewModel()
        {
            VerticalCharts = new ObservableCollection<VerticalChartViewModel>
            {
                new VerticalChartViewModel(new StackedMountainChartInitializer()),
                new VerticalChartViewModel(new LineChartInitializer()),
                new VerticalChartViewModel(new LineChartInitializer()),
                new VerticalChartViewModel(new LineChartInitializer()),
                new VerticalChartViewModel(new LineChartInitializer()),
            };
        }
    }
}