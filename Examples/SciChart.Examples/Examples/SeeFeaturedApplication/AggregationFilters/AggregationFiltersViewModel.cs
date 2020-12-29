// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AggregationFiltersViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AggregationFilters
{
    public class AggregationFiltersViewModel : BaseViewModel
    {
        private readonly List<Tick> _ticks;
        private readonly IOhlcDataSeries<DateTime, double> _priceData = new OhlcDataSeries<DateTime, double>();

        private IOhlcDataSeries<DateTime, double> _priceSeries;
        private IXyDataSeries<DateTime, double> _sma200Series;
        private IXyDataSeries<DateTime, double> _sma50Series;

        private AggregationPriceChart _selectedAggregationPriceChart;
        private ModifierType _chartZoomModifier;
        private ModifierType _chartSeriesModifier;
        
        private ShowTooltipOptions _showTooltipMode;
        private SourceMode _sourceMode;
        
        private bool _showAxisLabels;
        private string _selectedSeriesToSnap;
        
        private int _selectedCount;
        private int _selectedTimeFrame;
        private int _selectedVolume;
        
        private double _selectedRange;
        private double _selectedBrickSize;

        public AggregationFiltersViewModel()
        {
            _ticks = DataManager.Instance.GetTicks().ToList();
            _priceData.Append(_ticks.Select(x => x.DateTime), _ticks.Select(x => x.Open), _ticks.Select(x => x.High), _ticks.Select(x => x.Low), _ticks.Select(x => x.Close));
            
            SetZoomPanModifierCommand = new ActionCommand(() => ChartZoomModifier = IsZoomPanSelected ? ModifierType.Null : ModifierType.ZoomPan);
            SetRubberBandModifierCommand = new ActionCommand(() => ChartZoomModifier = IsRubberBandZoomSelected ? ModifierType.Null : ModifierType.RubberBandZoom);

            SetCursorModifierCommand = new ActionCommand(() => ChartSeriesModifier = IsCursorSelected ? ModifierType.Null : ModifierType.CrosshairsCursor);
            SetRolloverModifierCommand = new ActionCommand(() => ChartSeriesModifier = IsRolloverSelected ? ModifierType.Null : ModifierType.Rollover);

            SetDefaults();
            InitializeChartSurface();
        }

        public ActionCommand SetZoomPanModifierCommand { get; }
        public ActionCommand SetRubberBandModifierCommand { get; }
        
        public ActionCommand SetCursorModifierCommand { get; }
        public ActionCommand SetRolloverModifierCommand { get; }

        public bool IsZoomPanSelected => ChartZoomModifier == ModifierType.ZoomPan;
        public bool IsRubberBandZoomSelected => ChartZoomModifier == ModifierType.RubberBandZoom;
        
        public bool IsCursorSelected => ChartSeriesModifier == ModifierType.CrosshairsCursor;
        public bool IsRolloverSelected => ChartSeriesModifier == ModifierType.Rollover;

        public IEnumerable<AggregationPriceChart> AllAggregationPriceCharts { get; } = new[]
        {
            AggregationPriceChart.Count,
            AggregationPriceChart.Time,
            AggregationPriceChart.Volume,
            AggregationPriceChart.Range,
            AggregationPriceChart.Renko
        };
        
        public IEnumerable<int> AllCounts { get; } = new[] { 1000, 2000, 3000 };
        public IEnumerable<int> AllTimeFrames { get; } = new[] { 5, 15, 30 };
        public IEnumerable<int> AllVolumes { get; } = new[] { 1000, 2000, 3000 };
        public IEnumerable<double> AllRanges { get; } = new[] { 0.5, 1.0, 1.5 };
        public IEnumerable<double> AllBrickSizes { get; } = new[] { 0.5, 1.0, 1.5 };
        public IEnumerable<string> AllSeriesNames { get; } = new[] { "PriceData", "200 SMA", "50 SMA" };

        public IOhlcDataSeries<DateTime, double> PriceSeries
        {
            get => _priceSeries;
            set
            {
                _priceSeries = value;
                _priceSeries.SeriesName = "PriceData";
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

        public string SelectedSeriesToSnap
        {
            get => _selectedSeriesToSnap;
            set
            {
                _selectedSeriesToSnap = value;
                OnPropertyChanged(nameof(SelectedSeriesToSnap));
            }
        }

        private void InitializeChartSurface()
        {
            UpdatePriceChart(_selectedAggregationPriceChart);
            ChartZoomModifier = ModifierType.RubberBandZoom;
            ChartSeriesModifier = ModifierType.Rollover;
        }

        private void SetDefaults()
        {
            _selectedSeriesToSnap = "PriceData";
            _selectedAggregationPriceChart = AggregationPriceChart.Count;
            _selectedCount = 1000;
            _selectedTimeFrame = 5;
            _selectedVolume = 1000;
            _selectedRange = 0.5;
            _selectedBrickSize = 0.5;
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

        public AggregationPriceChart SelectedAggregationPriceChart
        {
            get => _selectedAggregationPriceChart;
            set
            {
                _selectedAggregationPriceChart = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged(nameof(SelectedAggregationPriceChart));
            }
        }

        public int SelectedCount
        {
            get => _selectedCount;
            set
            {
                _selectedCount = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged(nameof(SelectedCount));

            }
        }

        public int SelectedTimeFrame
        {
            get => _selectedTimeFrame;
            set
            {
                _selectedTimeFrame = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged(nameof(SelectedTimeFrame));

            }
        }

        public int SelectedVolume
        {
            get => _selectedVolume;
            set
            {
                _selectedVolume = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged(nameof(SelectedVolume));
            }
        }

        public double SelectedRange
        {
            get => _selectedRange;
            set
            {
                _selectedRange = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged(nameof(SelectedRange));
            }
        }

        public double SelectedBrickSize
        {
            get => _selectedBrickSize;
            set
            {
                _selectedBrickSize = value;
                UpdatePriceChart(_selectedAggregationPriceChart);
                OnPropertyChanged(nameof(SelectedBrickSize));
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
                    PriceSeries = (IOhlcDataSeries<DateTime, double>)_priceData.AggregateByVolume(i =>_ticks[i].Volume.ToDouble(), _selectedVolume);
                    break;
                
                case AggregationPriceChart.Range:
                    PriceSeries = (IOhlcDataSeries<DateTime, double>)_priceData.AggregateByRange(_selectedRange);
                    break;
                
                case AggregationPriceChart.Renko:
                    PriceSeries = (IOhlcDataSeries<DateTime, double>)_priceData.AggregateByRenko(_selectedBrickSize);
                    break;
            }

            // Create a series for the 200 period SMA which will be plotted as a line chart
            Sma200Series = (IXyDataSeries<DateTime, double>)PriceSeries.ToMovingAverage(200);

            // Create a series for the 50 period SMA which will be plotted as a line chart
            Sma50Series = (IXyDataSeries<DateTime, double>)((IXyDataSeries<DateTime, double>)PriceSeries.ToMovingAverage(50)).Scale(1.001);

            _priceSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }
    }
}