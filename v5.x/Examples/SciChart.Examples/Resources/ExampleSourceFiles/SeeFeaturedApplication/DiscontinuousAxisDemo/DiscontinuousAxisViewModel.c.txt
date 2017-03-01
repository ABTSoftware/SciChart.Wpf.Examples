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
using SciChart.Charting.Model.DataSeries.Filters;
using SciChart.Charting.Numerics.Calendars;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.Axes.DiscontinuousAxis;
using SciChart.Data.Model;
using SciChart.Examples.Examples.AnnotateAChart.OverlayTradeMarkers;
using SciChart.Examples.Examples.SeeFeaturedApplication.Common;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.DiscontinuousAxisDemo
{
    public class DiscontinuousAxisViewModel : BaseViewModel
    {
        private readonly List<Tick> _ticks;
        private readonly IEnumerable<ChartType> _allChartTypes = new[] { ChartType.FastLine, ChartType.FastColumn, ChartType.FastMountain, ChartType.FastCandlestick, ChartType.FastOhlc, };
        private readonly IEnumerable<string> _allCalendars = new[] { "Extended", "NYSE", "LSE" };
        private readonly IEnumerable<BarStyle> _allBarStyles = new[] { BarStyle.TimeBar, BarStyle.VolumeBar, BarStyle.RangeBar };
        private readonly IEnumerable<int> _allTimeFrames = new[] { 5, 15, 30 };
        private readonly IEnumerable<int> _allVolumes = new[] { 1000, 2000, 3000 };
        private readonly IEnumerable<double> _allRanges = new[] { 0.5, 1.0, 1.5 };
        private readonly double _barTimeFrame = TimeSpan.FromMinutes(1).Ticks;

        private Instrument _instrument;
        private int _selectedTimeFrame;
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
        private IDiscontinuousDateTimeCalendar _calendar;
        private string _selectedCalendar;
        private BarStyle _selectedBarStyle;
        private int _selectedVolume;
        private double _selectedRange;
        private AnnotationCollection _buySellAnnotations;
        private Tuple<DateTime, double> _min;
        private Tuple<DateTime, double> _max;

        public DiscontinuousAxisViewModel()
        {
            _ticks = DataManager.Instance.GetTicks().ToList();
            BuySellAnnotations = CreateAnnotations();
            SetDefaults();
            InitializeChartSurface();
        }

        /// <summary>
        /// Overloaded constructor used for testing
        /// </summary>
        /// <param name="dataManager"></param>
        public DiscontinuousAxisViewModel(IDataManager dataManager)
        {
            _dataManager = dataManager;
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
        public IEnumerable<BarStyle> AllBarStyles { get { return _allBarStyles; } }
        public IEnumerable<int> AllTimeFrames { get { return _allTimeFrames; } }
        public IEnumerable<int> AllVolumes { get { return _allVolumes; } }
        public IEnumerable<double> AllRanges { get { return _allRanges; } }

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

        public AnnotationCollection BuySellAnnotations
        {
            get { return _buySellAnnotations; }
            set
            {
                _buySellAnnotations = value;
                OnPropertyChanged("BuySellAnnotations");
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
            UpdateChartData(_selectedBarStyle);
            SetModifier(ModifierType.Rollover);
        }

        private void SetDefaults()
        {
            _selectedBarStyle = BarStyle.TimeBar;
            _selectedTimeFrame = 5;
            _selectedVolume = 1000;
            _selectedRange = 0.5;
            _chartType = ChartType.FastCandlestick;
            _calendar = new DefaultDiscontinuousDateTimeCalendar();
            _selectedCalendar = "Extended";
        }

        private static AnnotationCollection CreateAnnotations()
        {
            return new AnnotationCollection
            {
                new BuyMarkerAnnotation(), 
                new SellMarkerAnnotation(), 
            };
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
                UpdateChartData(_selectedBarStyle);
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
                OnPropertyChanged("SelectedCalendar");
            }
        }

        public BarStyle SelectedBarStyle
        {
            get { return _selectedBarStyle; }
            set
            {
                _selectedBarStyle = value;
                UpdateChartData(_selectedBarStyle);
                OnPropertyChanged("SelectedBarStyle");
            }
        }

        public int SelectedTimeFrame
        {
            get { return _selectedTimeFrame; }
            set
            {
                _selectedTimeFrame = value;
                UpdateChartData(_selectedBarStyle);
                OnPropertyChanged("SelectedTimeFrame");

            }
        }

        public int SelectedVolume
        {
            get { return _selectedVolume; }
            set
            {
                _selectedVolume = value;
                UpdateChartData(_selectedBarStyle);
                OnPropertyChanged("SelectedVolume");
            }
        }

        public double SelectedRange
        {
            get { return _selectedRange; }
            set
            {
                _selectedRange = value;
                UpdateChartData(_selectedBarStyle);
                OnPropertyChanged("SelectedRange");
            }
        }

        private void UpdateChartData(BarStyle barStyle)
        {
            PriceSeries priceData = null;

            switch (barStyle)
            {
                case BarStyle.TimeBar:
                    priceData = TicksAggregator.GetPriceDataByTimeFrame(_ticks, _selectedTimeFrame, _calendar, out _min, out _max);
                    break;
                case BarStyle.VolumeBar:
                    priceData = TicksAggregator.GetPriceDataByVolume(_ticks, _selectedVolume, _calendar, _barTimeFrame, out _min, out _max);
                    break;
                case BarStyle.RangeBar:
                    priceData = TicksAggregator.GetPriceDataByRange(_ticks, _selectedRange, _calendar, _barTimeFrame, out _min, out _max);
                    break;
            }

            UpdatePriceChart(priceData);

            _priceSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }

        private void UpdatePriceChart(PriceSeries priceData)
        {
            // Create a new series and append Open, High, Low, Close data                
            var ohlcDataSeries = new OhlcDataSeries<DateTime, double>();
            ohlcDataSeries.Append(priceData.TimeData, priceData.OpenData, priceData.HighData, priceData.LowData, priceData.CloseData);
            PriceData = new DiscontinuousSeries<double>(ohlcDataSeries, Calendar);
            PriceData.SeriesName = priceData.Symbol;

            var xyDataSeries = new XyDataSeries<DateTime, double>();
            xyDataSeries.Append(priceData.TimeData, priceData.CloseData);

            // Create a series for the 200 period SMA which will be plotted as a line chart
            Sma200Series = new MovingAverageFilter(xyDataSeries, 200);
            Sma200Series.SeriesName = "200 SMA";

            // Create a series for the 50 period SMA which will be plotted as a line chart
            Sma50Series = new MovingAverageFilter(xyDataSeries, 50);
            Sma50Series.SeriesName = "50 SMA";

            // Update the chart type and timeframe with current settings
            UpdateChartType(_chartType);
            UpdateAnnoations();
        }

        private void UpdateAnnoations()
        {
            BuySellAnnotations[0].X1 = _min.Item1;
            BuySellAnnotations[0].Y1 = _min.Item2;
            BuySellAnnotations[1].X1 = _max.Item1;
            BuySellAnnotations[1].Y1 = _max.Item2;
        }

        private void UpdateChartType(ChartType chartType)
        {
            if (PriceData.Count == 0)
            {
                UpdateChartData(_selectedBarStyle);
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