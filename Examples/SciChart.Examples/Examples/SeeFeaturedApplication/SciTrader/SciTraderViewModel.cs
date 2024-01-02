// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SciTraderViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.SeeFeaturedApplication.SciTrader
{
    public class SciTraderViewModel : BaseViewModel
    {
        private readonly IDataManager _dataManager = DataManager.Instance;

        private Instrument _instrument;
        private TimeFrame _timeFrame;
        
        private IOhlcDataSeries<DateTime, double> _priceSeries;
        private IXyDataSeries<DateTime, double> _sma200Series;
        private IXyDataSeries<DateTime, double> _sma50Series;
        private IXyDataSeries<DateTime, long> _volumeSeries;

        private ModifierType _chartZoomModifier;
        private ModifierType _chartSeriesModifier;

        private ChartType _chartType;
        private ShowTooltipOptions _showTooltipMode;
        private SourceMode _sourceMode;

        private string _selectedSeriesToSnap;
        private bool _showAxisLabels;
        private bool _useDiscontinuousDateTimeAxis;

        private IRange _cdtaVisibleRange;
        private IRange _ddtaVisibleRange;
        private IRange _overviewVisibleRange;
        
        private IDiscontinuousDateTimeCalendar _calendar;
        private OhlcDataSeries<DateTime, double> _ohlcDataSeries;

        public SciTraderViewModel()
        {
            SetDefaults();
            InitializeChartSurface();
        }

        /// <summary>
        /// Overloaded constructor used for testing
        /// </summary>
        /// <param name="dataManager"></param>
        public SciTraderViewModel(IDataManager dataManager)
        {
            _dataManager = dataManager;

            SetDefaults();
            InitializeChartSurface();
        }

        public ActionCommand SetZoomPanModifierCommand { get; private set; }
        public ActionCommand SetRubberBandModifierCommand { get; private set; }

        public ActionCommand SetCursorModifierCommand { get; private set; }
        public ActionCommand SetRolloverModifierCommand { get; private set; }

        public bool IsZoomPanSelected => ChartZoomModifier == ModifierType.ZoomPan;
        public bool IsRubberBandZoomSelected => ChartZoomModifier == ModifierType.RubberBandZoom;

        public bool IsCursorSelected => ChartSeriesModifier == ModifierType.CrosshairsCursor;
        public bool IsRolloverSelected => ChartSeriesModifier == ModifierType.Rollover;

        public ObservableCollection<string> AllSeriesNames { get; } = new ObservableCollection<string> {"200 SMA", "50 SMA"};

        public IEnumerable<Instrument> AllInstruments => _dataManager.AvailableInstruments;
        public IEnumerable<TimeFrame> AllTimeFrames => _dataManager.GetAvailableTimeFrames(SelectedInstrument);

        public IEnumerable<ChartType> AllChartTypes { get; } = new[]
        {
            ChartType.FastLine,
            ChartType.FastColumn,
            ChartType.FastMountain,
            ChartType.FastCandlestick,
            ChartType.FastOhlc
        };

        public IOhlcDataSeries<DateTime, double> PriceSeries
        {
            get => _priceSeries;
            set
            {
                _priceSeries = value;
               // _priceSeries.SeriesName = "PriceData";
                OnPropertyChanged(nameof(PriceSeries));
            }
        }

        public IXyDataSeries<DateTime, double> Sma200Series
        {
            get => _sma200Series;
            set
            {
                _sma200Series = value;
                _sma200Series.SeriesName = "200 SMA";
                OnPropertyChanged(nameof(Sma200Series));
            }
        }

        public IXyDataSeries<DateTime, double> Sma50Series
        {
            get => _sma50Series;
            set
            {
                _sma50Series = value;
                _sma50Series.SeriesName = "50 SMA";
                OnPropertyChanged(nameof(Sma50Series));
            }
        }

        public IXyDataSeries<DateTime, long> VolumeData
        {
            get => _volumeSeries;
            set
            {
                _volumeSeries = value;
                OnPropertyChanged(nameof(VolumeData));
            }
        }

        public Instrument SelectedInstrument
        {
            get => _instrument;
            set
            {
                _instrument = value;

                OnPropertyChanged(nameof(AllTimeFrames));
                OnPropertyChanged(nameof(SelectedInstrument));
                OnPropertyChanged(nameof(PriceChartTextFormatting));

                // Since timeframes depend on the instrument, get a new timeframe before updating the chart and below, raise PropertyChanged events
                SelectedTimeFrame = _dataManager.GetAvailableTimeFrames(_instrument).First();
            }
        }

        public ChartType SelectedChartType
        {
            get => _chartType;
            set
            {
                _chartType = value;
                OnPropertyChanged(nameof(SelectedChartType));
            }
        }

        public string SelectedSeriesToSnap
        {
            get => _selectedSeriesToSnap;
            set
            {
                _selectedSeriesToSnap = value;
                OnPropertyChanged(nameof(SelectedSeriesToSnap));
            }
        }

        public TimeFrame SelectedTimeFrame
        {
            get => _timeFrame;
            set
            {
                if (value != null)
                {
                    _timeFrame = value;
                    UpdateChartData(_instrument, _timeFrame);
                    OnPropertyChanged(nameof(SelectedTimeFrame));
                }
            }
        }
        
        /// <summary>
        /// Generates a formatting string for the Y-Axis, if DecimalPlaces = 1, generates "#", if DecimalPlaces = 3, generates "#.000" etc... 
        /// </summary>
        public string PriceChartTextFormatting => _instrument.DecimalPlaces == 0 ? "#" : "0." + new string('0', _instrument.DecimalPlaces);

        private void InitializeChartSurface()
        {
            SetZoomPanModifierCommand = new ActionCommand(() => ChartZoomModifier = IsZoomPanSelected ? ModifierType.Null : ModifierType.ZoomPan);
            SetRubberBandModifierCommand = new ActionCommand(() => ChartZoomModifier = IsRubberBandZoomSelected ? ModifierType.Null : ModifierType.RubberBandZoom);

            SetCursorModifierCommand = new ActionCommand(() => ChartSeriesModifier = IsCursorSelected ? ModifierType.Null : ModifierType.CrosshairsCursor);
            SetRolloverModifierCommand = new ActionCommand(() => ChartSeriesModifier = IsRolloverSelected ? ModifierType.Null : ModifierType.Rollover);

            UpdateChartData(_instrument, _timeFrame);
            ChartZoomModifier = ModifierType.RubberBandZoom;
            ChartSeriesModifier = ModifierType.Rollover;
        }

        private void SetDefaults()
        {
            _instrument = _dataManager.AvailableInstruments.FirstOrDefault(x => x.Symbol == "INDU");
            _timeFrame = _dataManager.GetAvailableTimeFrames(_instrument).First();
            _chartType = ChartType.FastCandlestick;
            _sourceMode = SourceMode.AllVisibleSeries;
        }

        public ModifierType ChartSeriesModifier
        {
            get => _chartSeriesModifier;
            set
            {
                _chartSeriesModifier = value;
                OnPropertyChanged(nameof(ChartSeriesModifier));
                OnPropertyChanged(nameof(IsRolloverSelected));
                OnPropertyChanged(nameof(IsCursorSelected));
            }
        }

        public ModifierType ChartZoomModifier
        {
            get => _chartZoomModifier;
            set
            {
                _chartZoomModifier = value;
                OnPropertyChanged(nameof(ChartZoomModifier));
                OnPropertyChanged(nameof(IsZoomPanSelected));
                OnPropertyChanged(nameof(IsRubberBandZoomSelected));
            }
        }

        public ShowTooltipOptions ShowTooltipMode
        {
            get => _showTooltipMode;
            set
            {
                _showTooltipMode = value;
                OnPropertyChanged(nameof(ShowTooltipMode));
            }
        }

        public SourceMode SourceMode
        {
            get => _sourceMode;
            set
            {
                _sourceMode = value;
                OnPropertyChanged(nameof(SourceMode));
            }
        }

        public bool ShowAxisLabels
        {
            get => _showAxisLabels;
            set
            {
                _showAxisLabels = value;
                OnPropertyChanged(nameof(ShowAxisLabels));
            }
        }

        public bool UseDiscontinuousDateTimeAxis
        {
            get => _useDiscontinuousDateTimeAxis;
            set
            {
                _useDiscontinuousDateTimeAxis = value;
                
                OnPropertyChanged(nameof(UseDiscontinuousDateTimeAxis));
                UpdateChartData(_instrument, _timeFrame);
                
                _priceSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
                _volumeSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
            }
        }

        public IRange CDTAVisibleRange
        {
            get => _cdtaVisibleRange;
            set
            {
                _cdtaVisibleRange = value;
                OnPropertyChanged(nameof(CDTAVisibleRange));
                OnPropertyChanged(nameof(OverviewVisibleRange));
            }
        }

        public IRange DDTAVisibleRange
        {
            get => _ddtaVisibleRange;
            set
            {
                _ddtaVisibleRange = value;
                OnPropertyChanged(nameof(DDTAVisibleRange));
                OnPropertyChanged(nameof(OverviewVisibleRange));
            }
        }

        public IRange OverviewVisibleRange
        {
            get => _overviewVisibleRange;
            set
            {
                _overviewVisibleRange = value;
                OnPropertyChanged(nameof(OverviewVisibleRange));
                OnPropertyChanged(!_useDiscontinuousDateTimeAxis
                    ? nameof(CDTAVisibleRange)
                    : nameof(DDTAVisibleRange));
            }
        }

        public IDiscontinuousDateTimeCalendar Calendar
        {
            get => _calendar;
            set
            {
                _calendar = value;
                OnPropertyChanged(nameof(Calendar));
            }
        }

        private void UpdateChartData(Instrument instrument, TimeFrame timeFrame)
        {
            // Get the data for the chart from the datasource
            var priceData = _dataManager.GetPriceData(instrument.Symbol, timeFrame);

            UpdatePriceChart(priceData);
            UpdateVolumeChart(priceData);
            UpdateCalendar();

            _priceSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
            _volumeSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }

        private void UpdatePriceChart(PriceSeries priceData)
        {
            // Create a new series and append Open, High, Low, Close data                
            _ohlcDataSeries = new OhlcDataSeries<DateTime, double>();
            _ohlcDataSeries.Append(priceData.TimeData, priceData.OpenData, priceData.HighData, priceData.LowData, priceData.CloseData);
            
            PriceSeries = (IOhlcDataSeries<DateTime, double>) (UseDiscontinuousDateTimeAxis ? _ohlcDataSeries.ToDiscontinuousSeries(Calendar) : _ohlcDataSeries);
            PriceSeries.SeriesName = priceData.Symbol;
            
            // Create a series for the 200 period SMA which will be plotted as a line chart
            Sma200Series = (IXyDataSeries<DateTime, double>)PriceSeries.ToMovingAverage(200);

            // Create a series for the 50 period SMA which will be plotted as a line chart
            Sma50Series = (IXyDataSeries<DateTime, double>)PriceSeries.ToMovingAverage(50);

            // Update the chart type, series names and timeframe with current settings
            UpdateChartType(_chartType);
            UpdateSeriesNames(priceData.Symbol);
        }

        private void UpdateSeriesNames(string priceName)
        {
            if (!AllSeriesNames.First().Contains("SMA"))
                AllSeriesNames.RemoveAt(0);
            
            AllSeriesNames.Insert(0, priceName);
            SelectedSeriesToSnap = priceName;
        }

        private void UpdateVolumeChart(PriceSeries prices)
        {
            // Create a new series and append Open, High, Low, Close data                
            VolumeData = new XyDataSeries<DateTime, long>
            {
                SeriesName = $"{prices.Symbol} Volume"
            };
            VolumeData.Append(prices.TimeData, prices.VolumeData);
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

        private void UpdateCalendar()
        {
            Calendar = new NYSECalendar();
        }
    }
}