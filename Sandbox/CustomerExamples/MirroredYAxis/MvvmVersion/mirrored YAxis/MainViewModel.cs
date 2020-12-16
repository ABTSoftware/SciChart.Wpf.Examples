using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using mirrored_YAxis.Annotations;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace mirrored_YAxis
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IRange _visibleRange;

        public MainViewModel()
        {
            XAxis.Add(new NumericAxisViewModel());
            YAxis.Add(new NumericAxisViewModel() { AxisAlignment = AxisAlignment.Right, StyleKey = "YAxisStyle"});
            YAxis.Add(new NumericAxisViewModel() { AxisAlignment = AxisAlignment.Left, Id="LeftAxis", StyleKey = "YAxisStyle"});
        }

        public ObservableCollection<IAxisViewModel> XAxis { get; } = new ObservableCollection<IAxisViewModel>();
        public ObservableCollection<IAxisViewModel> YAxis { get; } = new ObservableCollection<IAxisViewModel>();

        public IRange YVisibleRange
        {
            get => _visibleRange;
            set
            {
                _visibleRange = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
