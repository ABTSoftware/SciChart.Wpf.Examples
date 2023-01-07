using System.Collections.ObjectModel;
using OilAndGasExample.Common;
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

        public ObservableCollection<ChartViewModel> VerticalCharts { get; }

        public VerticalPanelViewModel()
        {
            VerticalCharts = new ObservableCollection<ChartViewModel>
            {
                new ChartViewModel(new ShaleChartFactory()),
                new ChartViewModel(new DensityChartFactory()),
                new ChartViewModel(new ResistivityChartFactory()),
                new ChartViewModel(new PoreSpaceChartFactory()),
                new ChartViewModel(new SonicChartFactory()),
                new ChartViewModel(new TextureChartFactory())
            };
        }
    }
}