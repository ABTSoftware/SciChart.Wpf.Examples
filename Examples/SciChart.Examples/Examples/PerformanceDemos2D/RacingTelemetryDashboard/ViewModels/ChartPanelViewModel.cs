// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ChartPanelViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.ChartSeries;
using SciChart.Examples.ExternalDependencies.Common;
using System.Collections.ObjectModel;

namespace SciChart.Examples.Examples.PerformanceDemos2D.RacingTelemetryDashboard.ViewModels
{
    public class ChartPanelViewModel : BaseViewModel
    {
        public string ChartTitle { get; set; }

        private double _sliceX;
        public double SliceX
        {
            get => _sliceX;
            set
            {
                if (_sliceX == value) return;
                _sliceX = value;
                OnPropertyChanged(nameof(SliceX));
            }
        }

        public ObservableCollection<IAxisViewModel> XAxes { get; set; } = new ObservableCollection<IAxisViewModel>();

        public ObservableCollection<IAxisViewModel> YAxes { get; set; } = new ObservableCollection<IAxisViewModel>();

        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries { get; set; } = new ObservableCollection<IRenderableSeriesViewModel>();
    }
}
