﻿using System;
using System.Collections.Generic;
using System.Linq;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;
using SciChart.Charting.Numerics.Calendars;
using SciChart.Charting.Visuals.Axes.LogarithmicAxis;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Sandbox.Examples.StockChartLogAxis
{
    public class StockChartLogAxisViewModel : BaseViewModel
    {
        private readonly IEnumerable<ChartType> _allChartTypes = new[] { ChartType.FastLine, ChartType.FastColumn, ChartType.FastMountain, ChartType.FastCandlestick, ChartType.FastOhlc, };
        private Instrument _instrument;
        private TimeFrame _timeFrame;
        private ChartType _chartType;
        private IRange _chartDateRange;
        private IDataManager _dataManager = DataManager.Instance;
        private bool _isShowing200Sma;
        private bool _isShowing50Sma;
        private IOhlcDataSeries<DateTime, double> _priceSeries;
        private IXyDataSeries<DateTime, double> _sma200Series;
        private IXyDataSeries<DateTime, double> _sma50Series;
        private ModifierType _chartModifier = ModifierType.Rollover;
        private ShowTooltipOptions _showToolTipMode;
        private SourceMode _sourceMode;
        private bool _showAxisLabels;
        private IRange _cdtaVisibleRange;
        private IRange _overviewVisibleRange;
        private IDiscontinuousDateTimeCalendar _calendar;
        private OhlcDataSeries<DateTime, double> _ohlcDataSeries;


        public StockChartLogAxisViewModel()
        {
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

        public IEnumerable<Instrument> AllInstruments { get { return _dataManager.AvailableInstruments; } }

        public IEnumerable<TimeFrame> AllTimeFrames { get { return _dataManager.GetAvailableTimeFrames(SelectedInstrument); } }
        public IEnumerable<ChartType> AllChartTypes { get { return _allChartTypes; } }

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

        public Instrument SelectedInstrument
        {
            get { return _instrument; }
            set
            {
                _instrument = value;

                OnPropertyChanged("AllTimeFrames");
                OnPropertyChanged("SelectedInstrument");
                OnPropertyChanged("PriceChartTextFormatting");

                // Since timeframes depend on the instrument, get a new timeframe before updating the chart and below, raise PropertyChanged events
                SelectedTimeFrame = _dataManager.GetAvailableTimeFrames(_instrument).First();
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

        public TimeFrame SelectedTimeFrame
        {
            get { return _timeFrame; }
            set
            {
                if (value != null)
                {
                    _timeFrame = value;
                    UpdateChartData(_instrument, _timeFrame);
                    OnPropertyChanged("SelectedTimeFrame");
                }
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

        /// <summary>
        /// Generates a formatting string for the Y-Axis, if DecimalPlaces = 1, generates "#", if DecimalPlaces = 3, generates "#.000" etc... 
        /// </summary>
        public string PriceChartTextFormatting
        {
            get
            {
                return _instrument.DecimalPlaces == 0 ? "#" : "0." + new string('0', _instrument.DecimalPlaces);
            }
        }

        private void InitializeChartSurface()
        {
            UpdateChartData(_instrument, _timeFrame);
            SetModifier(ModifierType.Rollover);
        }

        private void SetDefaults()
        {
            _instrument = _dataManager.AvailableInstruments.FirstOrDefault(x => x.Symbol == "INDU");
            _timeFrame = _dataManager.GetAvailableTimeFrames(_instrument).First();
            _chartType = ChartType.FastCandlestick;
            _sourceMode = SourceMode.AllVisibleSeries;
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

        public IRange CDTAVisibleRange
        {
            get { return _cdtaVisibleRange; }
            set
            {
                _cdtaVisibleRange = value;
                OnPropertyChanged("CDTAVisibleRange");
                OnPropertyChanged("OverviewVisibleRange");
            }
        }

        public IRange OverviewVisibleRange
        {
            get { return _overviewVisibleRange; }
            set
            {
                _overviewVisibleRange = value;
                OnPropertyChanged("OverviewVisibleRange");
                OnPropertyChanged("CDTAVisibleRange");
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

        private void UpdateChartData(Instrument instrument, TimeFrame timeFrame)
        {
            // Get the data for the chart from the datasource
            var priceData = _dataManager.GetPriceData(instrument.Symbol, timeFrame);

            UpdatePriceChart(priceData);

            _priceSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }

        private void UpdatePriceChart(PriceSeries priceData)
        {
            // Create a new series and append Open, High, Low, Close data                
            _ohlcDataSeries = new OhlcDataSeries<DateTime, double>();
            _ohlcDataSeries.Append(priceData.TimeData, priceData.OpenData, priceData.HighData, priceData.LowData, priceData.CloseData);
            PriceData = _ohlcDataSeries;
            PriceData.SeriesName = priceData.Symbol;

            // Create a series for the 200 period SMA which will be plotted as a line chart
            Sma200Series = (IXyDataSeries<DateTime, double>)PriceData.ToMovingAverage(200);
            Sma200Series.SeriesName = "200 SMA";

            // Create a series for the 50 period SMA which will be plotted as a line chart
            Sma50Series = (IXyDataSeries<DateTime, double>)PriceData.ToMovingAverage(50);
            Sma50Series.SeriesName = "50 SMA";

            // Update the chart type and timeframe with current settings
            UpdateChartType(_chartType);
        }

        private void UpdateChartType(ChartType chartType)
        {
            if (_ohlcDataSeries.Count == 0)
            {
                UpdateChartData(_instrument, _timeFrame);
                return;
            }

            SelectedChartType = chartType;
        }
    }
}