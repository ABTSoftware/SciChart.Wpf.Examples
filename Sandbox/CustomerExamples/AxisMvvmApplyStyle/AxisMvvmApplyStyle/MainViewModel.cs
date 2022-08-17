using System.Collections.ObjectModel;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Data.Model;

namespace AxisMvvmApplyStyle
{
    public class MainViewModel : BindableObject // replace this for your BaseViewModel of choice
    {
        public MainViewModel()
        {
            YAxisViewModels.Add(new NumericAxisViewModel()
            {
                // Apply a style key here to apply a named style to this axis
                StyleKey = "YAxisStyle",
                AxisTitle = "Y Axis"
            });

            XAxisViewModels.Add(new NumericAxisViewModel()
            {
                StyleKey = "XAxisStyle",
                AxisTitle = "X Axis"
            });
        }

        public ObservableCollection<IAxisViewModel> YAxisViewModels { get; } =
            new ObservableCollection<IAxisViewModel>();

        public ObservableCollection<IAxisViewModel> XAxisViewModels { get; } =
            new ObservableCollection<IAxisViewModel>();
    }
}
