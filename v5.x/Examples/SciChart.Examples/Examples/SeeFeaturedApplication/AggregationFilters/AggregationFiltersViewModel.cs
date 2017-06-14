using System;
using System.Collections.Generic;
using System.Linq;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AggregationFilters
{
    public class AggregationFiltersViewModel : BaseViewModel
    {
        private readonly List<Tick> _ticks;
        private readonly IOhlcDataSeries<DateTime, double> _priceData = new OhlcDataSeries<DateTime, double>();
        private readonly IEnumerable<AggregationPriceChart> _allAggregationPriceCharts = new[] { AggregationPriceChart.Count, AggregationPriceChart.Time, AggregationPriceChart.Volume, AggregationPriceChart.Range, AggregationPriceChart.Renko };
        private readonly IEnumerable<int> _allCounts = new[] { 1000, 2000, 3000 };
        private readonly IEnumerable<int> _allTimeFrames = new[] { 5, 15, 30 };
        private readonly IEnumerable<int> _allVolumes = new[] { 1000, 2000, 3000 };
        private readonly IEnumerable<double> _allRanges = new[] { 0.5, 1.0, 1.5 };
        private readonly IEnumerable<double> _allBrickSizes = new[] { 0.5, 1.0, 1.5 };
        
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
        private AggregationPriceChart _selectedAggregationPriceChart;
        private int _selectedCount;
        private int _selectedTimeFrame;
        private int _selectedVolume;
        private double _selectedRange;
        private double _selectedBrickSize;

        public AggregationFiltersViewModel()
        {
            _ticks = DataManager.Instance.GetTicks().ToList();
            _priceData.Append(_ticks.Select(x=>x.DateTime), _ticks.Select(x => x.Open), _ticks.Select(x => x.High), _ticks.Select(x => x.Low), _ticks.Select(x => x.Close));
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

        public IEnumerable<AggregationPriceChart> AllAggregationPriceCharts { get { return _allAggregationPriceCharts; } }
        public IEnumerable<int> AllCounts { get { return _allCounts; } }
        public IEnumerable<int> AllTimeFrames { get { return _allTimeFrames; } }
        public IEnumerable<int> AllVolumes { get { return _allVolumes; } }
        public IEnumerable<double> AllRanges { get { return _allRanges; } }
        public IEnumerable<double> AllBrickSizes { get { return _allBrickSizes; } }

        public IOhlcDataSeries<DateTime, double> PriceSeries
        {
            get { return _priceSeries; }
            set
            {
                _priceSeries = value;
                OnPropertyChanged("PriceSeries");
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
            UpdatePriceChart(_selectedAggregationPriceChart);
            SetModifier(ModifierType.Rollover);
        }

        private void SetDefaults()
        {
            _selectedAggregationPriceChart = AggregationPriceChart.Count;
            _selectedCount = 1000;
            _selectedTimeFrame = 5;
            _selectedVolume = 1000;
            _selectedRange = 0.5;
            _selectedBrickSize = 0.5;
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

        public AggregationPriceChart SelectedAggregationPriceChart
        {
            get { return _selectedAggregationPriceChart; }
            set
            {
                _selectedAggregationPriceChart = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged("SelectedAggregationPriceChart");
            }
        }

        public int SelectedCount
        {
            get { return _selectedCount; }
            set
            {
                _selectedCount = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged("SelectedCount");

            }
        }

        public int SelectedTimeFrame
        {
            get { return _selectedTimeFrame; }
            set
            {
                _selectedTimeFrame = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged("SelectedTimeFrame");

            }
        }

        public int SelectedVolume
        {
            get { return _selectedVolume; }
            set
            {
                _selectedVolume = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged("SelectedVolume");
            }
        }

        public double SelectedRange
        {
            get { return _selectedRange; }
            set
            {
                _selectedRange = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged("SelectedRange");
            }
        }

        public double SelectedBrickSize
        {
            get { return _selectedBrickSize; }
            set
            {
                _selectedBrickSize = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged("SelectedBrickSize");
            }
        }

        private void UpdatePriceChart(AggregationPriceChart barStyle)
        {
            switch (barStyle)
            {
                case AggregationPriceChart.Count:
                    PriceSeries = (IOhlcDataSeries<DateTime, double>)_priceData.AggregateByCount(_selectedCount);
                    break;
                case AggregationPriceChart.Time:
                    PriceSeries = (IOhlcDataSeries<DateTime, double>)_priceData.AggregateByTime(TimeSpan.FromMinutes(_selectedTimeFrame));
                    break;
                case AggregationPriceChart.Volume:
                    PriceSeries = (IOhlcDataSeries<DateTime, double>)_priceData.AggregateByVolume(_ticks.Select(x => x.Volume.ToDouble()).ToList(), _selectedVolume);
                    break;
                case AggregationPriceChart.Range:
                    PriceSeries = (IOhlcDataSeries<DateTime, double>)_priceData.AggregateByRange(_selectedRange);
                    break;
                case AggregationPriceChart.Renko:
                    PriceSeries = (IOhlcDataSeries<DateTime, double>)_priceData.AggregateByRenko(_selectedBrickSize);
                    break;
            }

            PriceSeries.SeriesName = "PriceData";

            // Create a series for the 200 period SMA which will be plotted as a line chart
            Sma200Series = (IXyDataSeries<DateTime, double>)PriceSeries.ToMovingAverage(200);
            Sma200Series.SeriesName = "200 SMA";

            // Create a series for the 50 period SMA which will be plotted as a line chart
            Sma50Series = (IXyDataSeries<DateTime, double>)PriceSeries.ToMovingAverage(50);
            Sma50Series.SeriesName = "50 SMA";

            _priceSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }
    }
}