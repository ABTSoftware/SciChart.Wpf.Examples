using System.Collections.ObjectModel;
using OilAndGasExample.VerticalCharts.ChartTypes;
using SciChart.Data.Model;

namespace OilAndGasExample.VerticalCharts
{
    public class VerticalPanelViewModel : BindableObject
    {
        private DoubleRange _sharedXRange;

        public DoubleRange SharedXRange
        {
            get => _sharedXRange;
            set
            {
                if (_sharedXRange != value)
                {
                    _sharedXRange = value;
                    OnPropertyChanged(nameof(SharedXRange));
                }
            }
        }

        public ObservableCollection<VerticalChartViewModel> VerticalCharts { get; }

        public VerticalPanelViewModel()
        {
            VerticalCharts = new ObservableCollection<VerticalChartViewModel>
            {
                new VerticalChartViewModel(new ShaleChartInitializer()),
                new VerticalChartViewModel(new DensityChartInitializer()),
                new VerticalChartViewModel(new ResistivityChartInitializer()),
                new VerticalChartViewModel(new PermChartInitializer()),
                new VerticalChartViewModel(new EmptyChartInitializer()),
                new VerticalChartViewModel(new EmptyChartInitializer())
            };
        }
    }
}