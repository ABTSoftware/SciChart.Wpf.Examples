// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomSeriesValueMarkersViewModel.cs is part of the SCICHART® Examples. Permission
// is hereby granted to modify, create derivative works, distribute and publish any part
// of this source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Numerics.CoordinateCalculators;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.InspectDatapoints.CustomSeriesValueMarkers
{
    public class CustomSeriesValueMarkersViewModel : BaseViewModel
    {
        private PriceBar _lastPrice;
        private IndexRange _xVisibleRange;

        private readonly IMarketDataService _marketDataService;
        private readonly OhlcDataSeries<DateTime, double> _dataSeries;

        private readonly object _tickLocker = new object();

        public CustomSeriesValueMarkersViewModel()
        {
            ViewportManager = new OnRenderedActionViewportManager(UpdateLineAnnotations);

            _marketDataService = new MarketDataService(new DateTime(2000, 08, 01, 12, 00, 00), 5, 20);
            _dataSeries = new OhlcDataSeries<DateTime, double> {SeriesName = "Price Series"};

            var prices = _marketDataService.GetHistoricalData(100);

            _dataSeries.Append(prices.Select(x => x.DateTime),
                prices.Select(x => x.Open),
                prices.Select(x => x.High),
                prices.Select(x => x.Low),
                prices.Select(x => x.Close));

            if (_dataSeries.Count >= 100)
            {
                XVisibleRange = new IndexRange(23, 83);
            }

            Series.Add(new CandlestickRenderableSeriesViewModel
            {
                DataSeries = _dataSeries,
                StrokeUp = Color.FromArgb(0xFF, 0x64, 0xBA, 0xE4),
                StrokeDown = Color.FromArgb(0xFF, 0xDC, 0x79, 0x69),
                SeriesInfoProvider = new CustomSeriesInfoProvider()
            });

            Annotations.Add(new HorizontalLineAnnotationViewModel
            {
                StrokeThickness = 1,
                Stroke = Colors.Gray,
                StrokeDashArray = new DoubleCollection(new[] {8d, 8d}),
                HorizontalAlignment = HorizontalAlignment.Right
            });

            Annotations.Add(new HorizontalLineAnnotationViewModel
            {
                StrokeThickness = 1,
                Stroke = Colors.Gray,
                StrokeDashArray = new DoubleCollection(new[] {8d, 8d}),
                HorizontalAlignment = HorizontalAlignment.Right
            });
        }

        public IViewportManager ViewportManager { get; }

        public double BarTimeFrame { get; } = TimeSpan.FromMinutes(5).TotalSeconds;

        public ObservableCollection<IRenderableSeriesViewModel> Series { get; } = new ObservableCollection<IRenderableSeriesViewModel>();

        public ObservableCollection<IAnnotationViewModel> Annotations { get; } = new ObservableCollection<IAnnotationViewModel>();

        public ICommand StartUpdatesCommand => new ActionCommand(OnStartUpdates);

        public ICommand StopUpdatesCommand => new ActionCommand(OnStopUpdates);

        public IndexRange XVisibleRange
        {
            get => _xVisibleRange;
            set
            {
                if (!Equals(_xVisibleRange, value))
                {
                    _xVisibleRange = value;
                    OnPropertyChanged(nameof(XVisibleRange));
                }
            }
        }

        private void OnStartUpdates()
        {
            ViewportManager?.ZoomExtentsX();

            _marketDataService.SubscribePriceUpdate(AppendUpdatePrice);
        }

        private void OnStopUpdates()
        {
            _marketDataService.ClearSubscriptions();
        }

        private void AppendUpdatePrice(PriceBar price)
        {
            lock (_tickLocker)
            {
                if (_lastPrice != null && _lastPrice.DateTime == price.DateTime)
                {
                    _dataSeries.Update(price.DateTime, price.Open, price.High, price.Low, price.Close);
                }
                else
                {
                    _dataSeries.Append(price.DateTime, price.Open, price.High, price.Low, price.Close);

                    if (XVisibleRange.Max > _dataSeries.Count)
                    {
                        var existingRange = _xVisibleRange;
                        var newRange = new IndexRange(existingRange.Min + 1, existingRange.Max + 1);

                        XVisibleRange = newRange;
                    }
                }

                _lastPrice = price;
            }
        }

        private void UpdateLineAnnotations(ISciChartSurface surface)
        {
            if (surface.XAxis.GetCurrentCoordinateCalculator() is ICategoryCoordinateCalculator categoryCalc)
            {
                var lineOpen = (LineAnnotationViewModel) Annotations.First();
                var lineClose = (LineAnnotationViewModel) Annotations.Last();

                IComparable x1Value;
                IComparable x2Value = categoryCalc.TransformIndexToData(XVisibleRange.Max);

                if (XVisibleRange.Max >= _dataSeries.Count)
                {
                    x1Value = _dataSeries.XValues.Last();

                    lineOpen.Y1 = _dataSeries.OpenValues.Last();
                    lineClose.Y1 = _dataSeries.CloseValues.Last();
                }
                else
                {
                    x1Value = _dataSeries.XValues[XVisibleRange.Max];

                    lineOpen.Y1 = _dataSeries.OpenValues[XVisibleRange.Max];
                    lineClose.Y1 = _dataSeries.CloseValues[XVisibleRange.Max];
                }

                lineOpen.X1 = x1Value;
                lineClose.X1 = x1Value;

                lineOpen.X2 = x2Value;
                lineClose.X2 = x2Value;

                var candleSeries = (CandlestickRenderableSeriesViewModel) Series.First();

                if (lineOpen.Y1.CompareTo(lineClose.Y1) <= 0)
                {
                    lineOpen.Stroke = candleSeries.StrokeUp;
                    lineClose.Stroke = candleSeries.StrokeUp;
                }
                else
                {
                    lineOpen.Stroke = candleSeries.StrokeDown;
                    lineClose.Stroke = candleSeries.StrokeDown;
                }
            }
        }
    }
}