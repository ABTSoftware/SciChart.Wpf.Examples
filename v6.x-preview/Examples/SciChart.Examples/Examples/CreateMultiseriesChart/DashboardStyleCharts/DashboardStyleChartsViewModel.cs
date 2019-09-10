// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DashboardStyleChartsViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.DashboardStyleCharts
{
    public class DashboardStyleChartsViewModel: BaseViewModel
    {
        private readonly IViewportManager _viewportManager = new DefaultViewportManager();
        private readonly List<ChartTypeViewModel> _chartTypesSource;

        private ChartTypeViewModel _currentChartType;
        private bool _isZoomExtendsAnimated = true;
        private int _selectedChartIndex;

        public DashboardStyleChartsViewModel()
        {
            _chartTypesSource = new List<ChartTypeViewModel>
            {
                ChartTypeViewModelFactory.New(typeof(IStackedColumnRenderableSeries), false, true),
                ChartTypeViewModelFactory.New(typeof(IStackedColumnRenderableSeries), false, false),
                ChartTypeViewModelFactory.New(typeof(IStackedColumnRenderableSeries), true, false),
                ChartTypeViewModelFactory.New(typeof(IStackedMountainRenderableSeries), false, false),
                ChartTypeViewModelFactory.New(typeof(IStackedMountainRenderableSeries), true, false),
            };
            
            SpacingModes = new List<SpacingMode> {SpacingMode.Absolute, SpacingMode.Relative};
            SelectedChartIndex = 0;
        }

        public IViewportManager ViewportManager
        {
            get { return _viewportManager; }
        }

        public List<SpacingMode> SpacingModes { get; set; }

        public List<ChartTypeViewModel> ChartTypesSource
        {
            get { return _chartTypesSource; }
        }

        public ChartTypeViewModel CurrentChartType
        {
            get { return _currentChartType; }
            set
            {
                _currentChartType = value;
                OnPropertyChanged("CurrentChartType");             
   
                // Invoke AnimateZoomExtents after binding engine has stabilised 
                _viewportManager.BeginInvoke(() =>
                {
                    var duration = _isZoomExtendsAnimated ? TimeSpan.FromMilliseconds(250) : TimeSpan.FromMilliseconds(0);
                    _viewportManager.AnimateZoomExtents(duration);                    
                });
            }
        }

        public int SelectedChartIndex
        {
            get { return _selectedChartIndex; }
            set
            {
                _selectedChartIndex = value;
                CurrentChartType = ChartTypesSource[value];
                OnPropertyChanged("SelectedChartIndex");
            }
        }

        public bool IsZoomExtendsAnimated
        {
            get { return _isZoomExtendsAnimated; }
            set
            {
                _isZoomExtendsAnimated = value;
                OnPropertyChanged("IsZoomExtendsAnimated");
            }
        }
    }
}