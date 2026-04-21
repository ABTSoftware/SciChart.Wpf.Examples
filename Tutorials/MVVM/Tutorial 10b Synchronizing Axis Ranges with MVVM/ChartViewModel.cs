using System.Collections.ObjectModel;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace SciChart.Mvvm.Tutorial
{
    public class ChartViewModel : BindableObject
    {
        private string _chartTitle;
        private bool _rangeSyncOnZoomExtents;

        public ObservableCollection<IAxisViewModel> XAxes { get; } = new ObservableCollection<IAxisViewModel>();
        public ObservableCollection<IAxisViewModel> YAxes { get; } = new ObservableCollection<IAxisViewModel>();
        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries { get; } = new ObservableCollection<IRenderableSeriesViewModel>();

        public string ChartTitle
        {
            get => _chartTitle;
            set
            {
                _chartTitle = value;
                OnPropertyChanged("ChartTitle");
            }
        }

        /// <summary>
        /// Toggles RangeSyncOnZoomExtents on every axis in this chart.
        /// When true, ZoomExtents propagates this chart's computed extents
        /// to all other axes in the same sync group. When false, ZoomExtents
        /// fits to local data only and sync is temporarily bypassed.
        /// </summary>
        public bool RangeSyncOnZoomExtents
        {
            get => _rangeSyncOnZoomExtents;
            set
            {
                if (_rangeSyncOnZoomExtents != value)
                {
                    _rangeSyncOnZoomExtents = value;
                    OnPropertyChanged(nameof(RangeSyncOnZoomExtents));

                    foreach (var axis in XAxes)
                        if (axis is AxisBaseViewModel vm)
                            vm.RangeSyncSourceOnZoomExtents = value;

                    foreach (var axis in YAxes)
                        if (axis is AxisBaseViewModel vm)
                            vm.RangeSyncSourceOnZoomExtents = value;
                }
            }
        }
    }
}
