// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DashboardStylePolarChartsViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.DashboardStylePolarCharts
{
    public class DashboardStylePolarChartsViewModel:BaseViewModel
    {
        private PolarChartViewModel _currentViewModel;
        private readonly IViewportManager _viewportManager = new DefaultViewportManager();
        public ObservableCollection<PolarChartViewModel> PolarChartViewModels { get; set; }

        public IViewportManager ViewportManager
        {
            get { return _viewportManager; }
        }

        public PolarChartViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel == value) return;
                _currentViewModel = value;
                OnPropertyChanged("CurrentViewModel");
                // Defer ZoomExtents execution setting DispatcherPriority lower then DispatcherPriority.Binding.
                // This is required to perform ZoomExtents after DataSeries binding is applied.
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(
                    () => { _viewportManager.AnimateZoomExtents(TimeSpan.FromMilliseconds(500)); }),
                    DispatcherPriority.Background);
            }
        }
        
        public DashboardStylePolarChartsViewModel()
        {
            PolarChartViewModels = new ObservableCollection<PolarChartViewModel>
            {
                PolarChartViewModelFactory.New<LineRenderableSeriesViewModel>(),
                PolarChartViewModelFactory.New<MountainRenderableSeriesViewModel>(),
                PolarChartViewModelFactory.New<ColumnRenderableSeriesViewModel>(),
                PolarChartViewModelFactory.New<XyScatterRenderableSeriesViewModel>(),
                PolarChartViewModelFactory.New<ImpulseRenderableSeriesViewModel>(),
               
                PolarChartViewModelFactory.New<CandlestickRenderableSeriesViewModel>(),
                PolarChartViewModelFactory.New<OhlcRenderableSeriesViewModel>(),
                
                PolarChartViewModelFactory.New<ErrorBarsRenderableSeriesViewModel>(),
                PolarChartViewModelFactory.New<BoxPlotRenderableSeriesViewModel>(),
               
                PolarChartViewModelFactory.New<BandRenderableSeriesViewModel>(),
                PolarChartViewModelFactory.New<BubbleRenderableSeriesViewModel>(),

                PolarChartViewModelFactory.New<StackedMountainRenderableSeriesViewModel>(),
                PolarChartViewModelFactory.New<StackedColumnRenderableSeriesViewModel>(),
            };

            _currentViewModel = PolarChartViewModels.FirstOrDefault();
        }
    }
}
