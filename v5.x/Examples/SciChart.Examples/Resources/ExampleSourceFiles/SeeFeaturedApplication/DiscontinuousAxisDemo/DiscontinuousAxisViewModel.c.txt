// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2016. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DiscontinuousAxisViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;
using SciChart.Charting.Numerics.Calendars;
using SciChart.Data.Model;
using SciChart.Examples.Examples.SeeFeaturedApplication.Common;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.DiscontinuousAxisDemo
{
    public class DiscontinuousAxisViewModel : BaseViewModel
    {
        private readonly IOhlcDataSeries<DateTime, double> _priceData = new OhlcDataSeries<DateTime, double>();
        private readonly IEnumerable<ChartType> _allChartTypes = new[] { ChartType.FastLine, ChartType.FastColumn, ChartType.FastMountain, ChartType.FastCandlestick, ChartType.FastOhlc, };
        private readonly IEnumerable<string> _allCalendars = new[] { "Extended", "NYSE", "LSE" };
        
        private ChartType _chartType;
        private IRange _chartDateRange;
        private bool _isShowing200Sma;
        private bool _isShowing50Sma;
        private IOhlcDataSeries<DateTime, double> _priceSeries;
        private IXyDataSeries<DateTime, double> _sma200Series;
        private IXyDataSeries<DateTime, double> _sma50Series;
        private ModifierType _chartModifier = ModifierType.Rollover;
        private ShowTooltipOptions _showToolTipMode;
        private SourceMode _sourceMode;
        private bool _showAxisLabels;
        private IDiscontinuousDateTimeCalendar _calendar;
        private string _selectedCalendar;

        public DiscontinuousAxisViewModel()
        {
            var priceSeries = DataManager.Instance.GetPriceData("EURUSD", TimeFrame.Minute5);
            _priceData.Append(priceSeries.TimeData, priceSeries.OpenData, priceSeries.HighData, priceSeries.LowData, priceSeries.CloseData);
            SetDefaults();
            InitializeChartSurface();
        }
        
        public ActionCommand SetCursorModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.CrosshairsCursor)); } }
        public ActionCommand SetRolloverModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.Rollover)); } }
        public ActionCommand SetZoomPanModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.ZoomPan)); } }
        public ActionCommand SetRubberBandZoomModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.RubberBandZoom)); } }
        public ActionCommand SetNullModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.Null)); } }

        public bool IsZoomPanSelected { get { return ChartModifier == ModifierType.ZoomPan; } }
        public bool IsRubberBandZoomSelected { get { return ChartModifier == ModifierType.RubberBandZoom; } }
        public bool IsRolloverSelected { get { return ChartModifier == ModifierType.Rollover; } }
        public bool IsCursorSelected { get { return ChartModifier == ModifierType.CrosshairsCursor; } }

        public IEnumerable<ChartType> AllChartTypes { get { return _allChartTypes; } }
        public IEnumerable<string> AllCalendars { get { return _allCalendars; } }

        public IOhlcDataSeries<DateTime, double> PriceData
        {
            get { return _priceSeries; }
            set
            {
                _priceSeries = value;
                OnPropertyChanged("PriceData");
            }
        }

        public IXyDataSeries<DateTime, double> Sma200Series
        {
            get { return _sma200Series; }
            set
            {
                _sma200Series = value;
                OnPropertyChanged("Sma200Series");
            }
        }

        public IXyDataSeries<DateTime, double> Sma50Series
        {
            get { return _sma50Series; }
            set
            {
                _sma50Series = value;
                OnPropertyChanged("Sma50Series");
            }
        }

        public ChartType SelectedChartType
        {
            get { return _chartType; }
            set
            {
                _chartType = value;
                OnPropertyChanged("SelectedChartType");
            }
        }

        public IRange ChartVisibleRange
        {
            get { return _chartDateRange; }
            set
            {
                _chartDateRange = value;
                OnPropertyChanged("ChartVisibleRange");
            }
        }

        public bool Show200SMa
        {
            get { return _isShowing200Sma; }
            set
            {
                _isShowing200Sma = value;
                OnPropertyChanged("IsShowing200Sma");
            }
        }


        public bool Show50Sma
        {
            get { return _isShowing50Sma; }
            set
            {
                _isShowing50Sma = value;
                OnPropertyChanged("IsShowing50Sma");
            }
        }
        
        private void InitializeChartSurface()
        {
            UpdatePriceChart();
            SetModifier(ModifierType.Rollover);
        }

        private void SetDefaults()
        {
            _chartType = ChartType.FastCandlestick;
            _calendar = new DefaultDiscontinuousDateTimeCalendar();
            _selectedCalendar = "Extended";
        }

        private void SetModifier(ModifierType modifierType)
        {
            ChartModifier = modifierType;
        }

        public ModifierType ChartModifier
        {
            get
            {
                return _chartModifier;
            }
            set
            {
                _chartModifier = value;
                OnPropertyChanged("ChartModifier");
                OnPropertyChanged("IsRolloverSelected");
                OnPropertyChanged("IsZoomPanSelected");
                OnPropertyChanged("IsRubberBandZoomSelected");
                OnPropertyChanged("IsCursorSelected");
            }
        }

        public ShowTooltipOptions RolloverMode
        {
            get { return _showToolTipMode; }
            set
            {
                _showToolTipMode = value;
                OnPropertyChanged("RolloverMode");
            }
        }


        public SourceMode SourceMode
        {
            get
            {
                return _sourceMode;
            }
            set
            {
                _sourceMode = value;
                OnPropertyChanged("SourceMode");
            }
        }

        public bool ShowAxisLabels
        {
            get { return _showAxisLabels; }
            set
            {
                _showAxisLabels = value;
                OnPropertyChanged("ShowAxisLabels");
            }
        }

        public IDiscontinuousDateTimeCalendar Calendar
        {
            get { return _calendar; }
            set
            {
                _calendar = value;
                OnPropertyChanged("Calendar");
            }
        }

        public string SelectedCalendar
        {
            get { return _selectedCalendar; }
            set
            {
                _selectedCalendar = value;
                UpdateCalendar(_selectedCalendar);
                UpdatePriceChart();
                OnPropertyChanged("SelectedCalendar");
            }
        }

        private void UpdatePriceChart()
        {
            PriceData = (IOhlcDataSeries<DateTime, double>)_priceData.ToDiscontinuousSeries(Calendar);
            PriceData.SeriesName = "PriceData";

            // Create a series for the 200 period SMA which will be plotted as a line chart
            Sma200Series = (IXyDataSeries<DateTime, double>) PriceData.ToMovingAverage(200);
            Sma200Series.SeriesName = "200 SMA";

            // Create a series for the 50 period SMA which will be plotted as a line chart
            Sma50Series = (IXyDataSeries<DateTime, double>)PriceData.ToMovingAverage(50);
            Sma50Series.SeriesName = "50 SMA";

            // Update the chart type and timeframe with current settings
            UpdateChartType(_chartType);

            _priceSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }
        
        private void UpdateChartType(ChartType chartType)
        {
            if (PriceData.Count == 0)
            {
                UpdatePriceChart();
                return;
            }

            SelectedChartType = chartType;
        }

        private void UpdateCalendar(string calendar)
        {
            if (calendar == "Extended")
                Calendar = new DefaultDiscontinuousDateTimeCalendar();
            else if (calendar == "NYSE")
                Calendar = new NYSECalendar();
            else if (calendar == "LSE")
                Calendar = new LSECalendar();
        }
    }
}