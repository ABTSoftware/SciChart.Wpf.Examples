// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
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
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.InspectDatapoints.CustomSeriesValueMarkers
{
    public class CustomSeriesValueMarkersViewModel : BaseViewModel
    {
        private PriceBar _lastPrice;
        private DateRange _xVisibleRange;

        private readonly IMarketDataService _marketDataService;
        private readonly OhlcDataSeries<DateTime, double> _dataSeries;

        private readonly object _tickLocker = new object();

        public CustomSeriesValueMarkersViewModel()
        {
            // Set up ViewportManager to observe updates of the SciChartSurface
            ViewportManager = new OnRenderedActionViewportManager(UpdateLineAnnotations);

            // Configure the data source
            var startDate = new DateTime(2000, 8, 01, 12, 0, 0);
            _marketDataService = new MarketDataService(startDate, 5, 20);
            var prices = _marketDataService.GetHistoricalData(100);

            // Create the DataSeries
            _dataSeries = new OhlcDataSeries<DateTime, double> { SeriesName = "Price Series" };
            _dataSeries.Append(prices.Select(x => x.DateTime),
                prices.Select(x => x.Open),
                prices.Select(x => x.High),
                prices.Select(x => x.Low),
                prices.Select(x => x.Close));

            Series.Add(new CandlestickRenderableSeriesViewModel
            {
                DataSeries = _dataSeries,
                StrokeUp = Color.FromArgb(0xFF, 0x64, 0xBA, 0xE4),
                StrokeDown = Color.FromArgb(0xFF, 0xDC, 0x79, 0x69),
                SeriesInfoProvider = new CustomSeriesInfoProvider()
            });

            // Add two LineAnnotations
            // They will highlight the Open and Close prices of the last data point in the viewport.
            Annotations.Add(new HorizontalLineAnnotationViewModel
            {
                StrokeThickness = 1,
                Stroke = Colors.Gray,
                StrokeDashArray = new DoubleCollection(new[] { 8d, 8d }),
                HorizontalAlignment = HorizontalAlignment.Right
            });

            Annotations.Add(new HorizontalLineAnnotationViewModel
            {
                StrokeThickness = 1,
                Stroke = Colors.Gray,
                StrokeDashArray = new DoubleCollection(new[] { 8d, 8d }),
                HorizontalAlignment = HorizontalAlignment.Right
            });

            // Configure the initial VisibleRange of the X-Axis
            if (_dataSeries.Count >= 100)
            {
                XVisibleRange = new DateRange(startDate, startDate.AddDays(1));
            }
        }

        public IViewportManager ViewportManager { get; }

        public ObservableCollection<IRenderableSeriesViewModel> Series { get; } = new ObservableCollection<IRenderableSeriesViewModel>();

        public ObservableCollection<IAnnotationViewModel> Annotations { get; } = new ObservableCollection<IAnnotationViewModel>();

        public ICommand StartUpdatesCommand => new ActionCommand(OnStartUpdates);

        public ICommand StopUpdatesCommand => new ActionCommand(OnStopUpdates);

        public DateRange XVisibleRange
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
            // Zoom extents to the data bounds
            ViewportManager?.ZoomExtentsX();

            // Begin receiving new data from the data source
            _marketDataService.SubscribePriceUpdate(AppendUpdatePrice);
        }

        private void OnStopUpdates()
        {
            // Stop receiving data from the data source
            _marketDataService.ClearSubscriptions();
        }

        private void AppendUpdatePrice(PriceBar price)
        {
            lock (_tickLocker)
            {
                if (_lastPrice != null && _lastPrice.DateTime == price.DateTime)
                {
                    // Update the last data point
                    _dataSeries.Update(price.DateTime, price.Open, price.High, price.Low, price.Close);
                }
                else
                {
                    // Append a new data point
                    _dataSeries.Append(price.DateTime, price.Open, price.High, price.Low, price.Close);

                    // Update the VisibleRange on X-Axis
                    if (XVisibleRange.Max > _dataSeries.XMax.ToDateTime())
                    {
                        var existingRange = _xVisibleRange;
                        var newRange = new DateRange(existingRange.Min.AddMinutes(5), existingRange.Max.AddMinutes(5));

                        XVisibleRange = newRange;
                    }
                }

                _lastPrice = price;
            }
        }

        private void UpdateLineAnnotations(ISciChartSurface surface)
        {
            // IndexDateTimeAxis is expected
            if (surface.XAxis.GetCurrentCoordinateCalculator() is not IIndexCoordinateCalculator categoryCalc)
                return;

            // Find new positions and update the LineAnnotations 
            var lineOpen = (LineAnnotationViewModel)Annotations.First();
            var lineClose = (LineAnnotationViewModel)Annotations.Last();

            // Hide LineAnnotations when Series leaves the Viewport
            var isSeriesInViewport = XVisibleRange.Min <= _dataSeries.XMax.ToDateTime();
            lineOpen.IsHidden = lineClose.IsHidden = !isSeriesInViewport;
            if (!isSeriesInViewport) return;

            // Calculate and Update positions
            IComparable x1Value;
            IComparable x2Value = XVisibleRange.Max;
            if (XVisibleRange.Max >= _dataSeries.XMax.ToDateTime())
            {
                // Find new position when the last data point is inside the Viewport
                x1Value = _dataSeries.XValues.Last();
                lineOpen.Y1 = _dataSeries.OpenValues.Last();
                lineClose.Y1 = _dataSeries.CloseValues.Last();
            }
            else
            {
                // Find index of the the most right data point in the DataSeries
                var maxIndexInterpolated = categoryCalc.TransformDataToIndex(XVisibleRange.Max.ToDouble());
                var maxIndex = (int)Math.Round(maxIndexInterpolated, MidpointRounding.AwayFromZero);
                maxIndex = Math.Max(maxIndex, 0);

                // Find new position
                x1Value = _dataSeries.XValues[maxIndex];
                lineOpen.Y1 = _dataSeries.OpenValues[maxIndex];
                lineClose.Y1 = _dataSeries.CloseValues[maxIndex];
            }

            // Update positions of the LineAnnotations
            lineOpen.X1 = x1Value;
            lineClose.X1 = x1Value;

            lineOpen.X2 = x2Value;
            lineClose.X2 = x2Value;

            // Update the Stroke color of the LineAnnotations
            var candleSeries = (CandlestickRenderableSeriesViewModel)Series.First();
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