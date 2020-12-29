// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
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
using SciChart.Charting.Visuals.Annotations;
using SciChart.Examples.Examples.AnnotateAChart.OverlayTradeMarkers;
using SciChart.Examples.Examples.SeeFeaturedApplication.Common;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.DiscontinuousAxisDemo
{
    public class DiscontinuousAxisViewModel : BaseViewModel
    {
        private readonly IOhlcDataSeries<DateTime, double> _priceData = new OhlcDataSeries<DateTime, double>();

        private IOhlcDataSeries<DateTime, double> _priceSeries;
        private IXyDataSeries<DateTime, double> _sma200Series;
        private IXyDataSeries<DateTime, double> _sma50Series;
        
        private ModifierType _chartZoomModifier;
        private ModifierType _chartSeriesModifier;

        private ChartType _chartType;
        private ShowTooltipOptions _showTooltipMode;
        private SourceMode _sourceMode;
        
        private bool _showAxisLabels;
        private string _selectedCalendar;
        private string _selectedSeriesToSnap;

        private IDiscontinuousDateTimeCalendar _calendar;
        private AnnotationCollection _annotations;

        public DiscontinuousAxisViewModel()
        {
            var priceSeries = DataManager.Instance.GetPriceData("EURUSD", TimeFrame.Minute5);
            _priceData.Append(priceSeries.TimeData, priceSeries.OpenData, priceSeries.HighData, priceSeries.LowData, priceSeries.CloseData);

            SetZoomPanModifierCommand = new ActionCommand(() => ChartZoomModifier = IsZoomPanSelected ? ModifierType.Null : ModifierType.ZoomPan);
            SetRubberBandModifierCommand = new ActionCommand(() => ChartZoomModifier = IsRubberBandZoomSelected ? ModifierType.Null : ModifierType.RubberBandZoom);

            SetCursorModifierCommand = new ActionCommand(() => ChartSeriesModifier = IsCursorSelected ? ModifierType.Null : ModifierType.CrosshairsCursor);
            SetRolloverModifierCommand = new ActionCommand(() => ChartSeriesModifier = IsRolloverSelected ? ModifierType.Null : ModifierType.Rollover);

            Annotations = CreateAnnotations();
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

        public IEnumerable<string> AllSeriesNames { get; } = new[] { "PriceData", "200 SMA", "50 SMA" };

        public IEnumerable<ChartType> AllChartTypes { get; } = new[]
        {
            ChartType.FastLine,
            ChartType.FastColumn,
            ChartType.FastMountain,
            ChartType.FastCandlestick,
            ChartType.FastOhlc
        };
        
        public IEnumerable<string> AllCalendars { get; } = new[]
        {
            "Extended",
            "NYSE",
            "LSE"
        };

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

        public AnnotationCollection Annotations
        {
            get => _annotations;
            set
            {
                _annotations = value;
                OnPropertyChanged(nameof(Annotations));
            }
        }

        private void InitializeChartSurface()
        {
            UpdatePriceChart();
            ChartZoomModifier = ModifierType.RubberBandZoom;
            ChartSeriesModifier = ModifierType.Rollover;
        }

        private void SetDefaults()
        {
            _chartType = ChartType.FastCandlestick;
            _calendar = new DefaultDiscontinuousDateTimeCalendar();
            _selectedCalendar = "Extended";
            _selectedSeriesToSnap = "PriceData";
        }

        private static AnnotationCollection CreateAnnotations()
        {
            return new AnnotationCollection
            {
                new BuyMarkerAnnotation(),
                new SellMarkerAnnotation(),
                new LineAnnotation(),
                new BoxAnnotation()
            };
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

        public IDiscontinuousDateTimeCalendar Calendar
        {
            get => _calendar;
            set
            {
                _calendar = value;
                OnPropertyChanged(nameof(Calendar));
            }
        }

        public string SelectedCalendar
        {
            get => _selectedCalendar;
            set
            {
                _selectedCalendar = value;
                UpdateCalendar(_selectedCalendar);
                UpdatePriceChart();
                OnPropertyChanged(nameof(SelectedCalendar));
            }
        }

        private void UpdatePriceChart()
        {
            PriceSeries = (IOhlcDataSeries<DateTime, double>)_priceData.ToDiscontinuousSeries(Calendar);

            // Create a series for the 200 period SMA which will be plotted as a line chart
            Sma200Series = (IXyDataSeries<DateTime, double>)PriceSeries.ToMovingAverage(200);

            // Create a series for the 50 period SMA which will be plotted as a line chart
            Sma50Series = (IXyDataSeries<DateTime, double>)PriceSeries.ToMovingAverage(50);

            // Update the chart type and timeframe with current settings
            UpdateChartType(_chartType);
            
            UpdateAnnotations();

            _priceSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }

        private void UpdateAnnotations()
        {
            var minIndex = PriceSeries.CloseValues.IndexOf(PriceSeries.CloseValues.Min());
            var maxIndex = PriceSeries.CloseValues.IndexOf(PriceSeries.CloseValues.Max());

            Annotations[0].X1 = PriceSeries.XValues[minIndex];
            Annotations[0].Y1 = PriceSeries.YValues[minIndex];

            Annotations[1].X1 = PriceSeries.XValues[maxIndex];
            Annotations[1].Y1 = PriceSeries.YValues[maxIndex];

            Annotations[2].X1 = PriceSeries.XValues[minIndex];
            Annotations[2].Y1 = PriceSeries.YValues[minIndex];
            Annotations[2].X2 = PriceSeries.XValues[maxIndex];
            Annotations[2].Y2 = PriceSeries.YValues[maxIndex];

            Annotations[3].X1 = PriceSeries.XValues[minIndex];
            Annotations[3].Y1 = PriceSeries.YValues[minIndex];
            Annotations[3].X2 = PriceSeries.XValues[maxIndex];
            Annotations[3].Y2 = PriceSeries.YValues[maxIndex];
        }

        private void UpdateChartType(ChartType chartType)
        {
            if (PriceSeries.Count == 0)
            {
                UpdatePriceChart();
                return;
            }

            SelectedChartType = chartType;
        }

        private void UpdateCalendar(string calendar)
        {
            if (calendar == "Extended")
            {
                Calendar = new DefaultDiscontinuousDateTimeCalendar();
            }
            else if (calendar == "NYSE")
            {
                Calendar = new NYSECalendar();
            }
            else if (calendar == "LSE")
            {
                Calendar = new LSECalendar();
            }
        }
    }
}