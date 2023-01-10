// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateMultiPaneStockChartsViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SciChart.Charting;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.TradeChart;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateStockCharts.MultiPane
{
    public class CreateMultiPaneStockChartsViewModel : BaseViewModel
    {
        private IndexRange _xAxisVisibleRange;
        private ObservableCollection<BaseChartPaneViewModel> _chartPaneViewModels = new ObservableCollection<BaseChartPaneViewModel>();
        private readonly ICommand _closePaneCommand;
        private bool _isPanEnabled;
        private bool _isZoomEnabled;
        private string _verticalChartGroupId;
        private IViewportManager _viewportManager;

        public CreateMultiPaneStockChartsViewModel()
        {
            ZoomModeCommand = new ActionCommand(SetZoomMode);
            PanModeCommand = new ActionCommand(SetPanMode);
            ZoomExtentsCommand = new ActionCommand(ZoomExtends);

            // Get prices and append for the main price chart (Candlestick)
            var instrumentPriceData = DataManager.Instance.GetPriceData(Instrument.EurUsd.Value, TimeFrame.Daily);

            // ChartGroup is an ID which is used to synchronize chart pane heights and mouse events. it must be unique per SciChartGroup, but differ if you have many top level SciChartGroups
            _verticalChartGroupId = Guid.NewGuid().ToString();

            _viewportManager = new DefaultViewportManager();
            var closePaneCommand = new ActionCommand<IChildPane>(pane => ChartPaneViewModels.Remove((BaseChartPaneViewModel)pane));

            _chartPaneViewModels.Add(new PricePaneViewModel(this, instrumentPriceData) { IsFirstChartPane = true, ViewportManager = _viewportManager });
            _chartPaneViewModels.Add(new MacdPaneViewModel(this, instrumentPriceData) { Title = "MACD", ClosePaneCommand = closePaneCommand });
            _chartPaneViewModels.Add(new RsiPaneViewModel(this, instrumentPriceData) { Title = "RSI", ClosePaneCommand = closePaneCommand });
            _chartPaneViewModels.Add(new VolumePaneViewModel(this, instrumentPriceData) { Title = "Volume", ClosePaneCommand = closePaneCommand, IsLastChartPane = true});

            SetZoomMode();
        }

        private void ZoomExtends()
        {
            _viewportManager.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
        }

        public IEnumerable<string> AllThemes { get { return ThemeManager.AllThemes; } }

        public ICommand ZoomModeCommand { get; private set; }

        public ICommand PanModeCommand { get; private set; }

        public ICommand ZoomExtentsCommand { get; private set; }

        public string VerticalChartGroupId
        {
            get { return _verticalChartGroupId; }
            set
            {
                if (_verticalChartGroupId == value) return;
                _verticalChartGroupId = value;
                OnPropertyChanged("VerticalChartGroupId");
            }
        }    

        /// <summary>
        /// Shared XAxis VisibleRange for all charts
        /// </summary>
        public IndexRange XVisibleRange
        {
            get { return _xAxisVisibleRange; }
            set
            {
                if (Equals(_xAxisVisibleRange, value)) return;

                _xAxisVisibleRange = value;
                OnPropertyChanged("XVisibleRange");
            }
        }        

        public ObservableCollection<BaseChartPaneViewModel> ChartPaneViewModels
        {
            get { return _chartPaneViewModels; }
            set
            {
                if (_chartPaneViewModels == value) return;

                _chartPaneViewModels = value;
                OnPropertyChanged("ChartPaneViewModels");
            }
        }

        public bool IsPanEnabled
        {
            get { return _isPanEnabled; }
            set
            {
                _isPanEnabled = value;
                OnPropertyChanged("IsPanEnabled");
            }
        }

        public bool IsZoomEnabled
        {
            get { return _isZoomEnabled; }
            set
            {
                _isZoomEnabled = value;
                OnPropertyChanged("IsZoomEnabled");
            }
        }  

        private void SetPanMode()
        {
            IsPanEnabled = true;
            IsZoomEnabled = false;
        }

        private void SetZoomMode()
        {
            IsPanEnabled = false;
            IsZoomEnabled = true;
        }     
    }
}
