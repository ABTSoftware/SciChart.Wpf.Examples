using System.Collections.ObjectModel;
using OilAndGasExample.VerticalCharts.ChartFactory;
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
                new VerticalChartViewModel(new ShaleChartFactory()),
                new VerticalChartViewModel(new DensityChartFactory()),
                new VerticalChartViewModel(new ResistivityChartFactory()),
                new VerticalChartViewModel(new PoreSpaceChartFactory()),
                new VerticalChartViewModel(new SonicChartFactory()),
                new VerticalChartViewModel(new TextureChartFactory())
            };
        }
    }
}