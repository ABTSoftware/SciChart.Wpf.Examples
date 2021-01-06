// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateRealTimeTickingStockChartViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using System.Windows.Input;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateStockCharts.RealtimeMvvm
{
    public class CreateRealTimeTickingStockChartViewModel : BaseViewModel
    {
        private readonly IMarketDataService _marketDataService;        
        private readonly MovingAverage _sma50 = new MovingAverage(50);

        private PriceBar _lastPrice;
        private IndexRange _xVisibleRange;

        private int _selectedStrokeThickness;
        private string _selectedSeriesStyle;

        private ObservableCollection<IRenderableSeriesViewModel> _seriesViewModels;

        public CreateRealTimeTickingStockChartViewModel()
        {
            _seriesViewModels = new ObservableCollection<IRenderableSeriesViewModel>();

            // Market data service simulates live ticks. We want to load the chart with 150 historical bars
            // then later do real-time ticking as new data comes in
            _marketDataService = new MarketDataService(new DateTime(2000, 08, 01, 12, 00, 00), 5, 20);

            // Add ChartSeriesViewModels for the candlestick and SMA series
            var ds0 = new OhlcDataSeries<DateTime, double> {SeriesName = "Price Series"};
            _seriesViewModels.Add(new OhlcRenderableSeriesViewModel {DataSeries = ds0, StyleKey = "BaseRenderableSeriesStyle"});

            var ds1 = new XyDataSeries<DateTime, double> {SeriesName = "50-Period SMA"};
            _seriesViewModels.Add(new LineRenderableSeriesViewModel {DataSeries = ds1, StyleKey = "LineStyle"});

            // Append 150 historical bars to data series
            var prices = _marketDataService.GetHistoricalData(100);

            ds0.Append(prices.Select(x => x.DateTime),
                prices.Select(x => x.Open),
                prices.Select(x => x.High),
                prices.Select(x => x.Low),
                prices.Select(x => x.Close));

            ds1.Append(prices.Select(x => x.DateTime), prices.Select(y => _sma50.Push(y.Close).Current));

            StrokeThicknesses = new[] {1, 2, 3, 4, 5};
            SeriesStyles = new[] {"OHLC", "Candlestick", "Line", "Mountain"};

            SelectedStrokeThickness = 2;
            SelectedSeriesStyle = "OHLC";
        }

        public ObservableCollection<IRenderableSeriesViewModel> SeriesViewModels
        {
            get => _seriesViewModels;
            set
            {
                _seriesViewModels = value;
                OnPropertyChanged("SeriesViewModels");
            }
        }

        public double BarTimeFrame { get; } = TimeSpan.FromMinutes(5).TotalSeconds;

        public ICommand TickCommand => new ActionCommand(() => OnNewPrice(_marketDataService.GetNextBar()));
        
        public ICommand StartUpdatesCommand => new ActionCommand(() => _marketDataService.SubscribePriceUpdate(OnNewPrice)); 

        public ICommand StopUpdatesCommand => new ActionCommand(() => _marketDataService.ClearSubscriptions());

        public IEnumerable<string> SeriesStyles { get; }

        public IEnumerable<int> StrokeThicknesses { get; }

        public int SelectedStrokeThickness
        {
            get => _selectedStrokeThickness;
            set
            {
                _selectedStrokeThickness = value;
                OnPropertyChanged("SelectedStrokeThickness");
            }
        }

        public string SelectedSeriesStyle
        {
            get => _selectedSeriesStyle;
            set
            {
                _selectedSeriesStyle = value;
                OnPropertyChanged("SelectedSeriesStyle");

                if (_selectedSeriesStyle == "OHLC")
                {
                    SeriesViewModels[0] = new OhlcRenderableSeriesViewModel
                    {
                        DataSeries = SeriesViewModels[0].DataSeries,
                        StyleKey = "BaseRenderableSeriesStyle"
                    };
                }                   
                else if (_selectedSeriesStyle == "Candlestick")
                {
                    SeriesViewModels[0] = new CandlestickRenderableSeriesViewModel
                    {
                        DataSeries = SeriesViewModels[0].DataSeries,
                        StyleKey = "BaseRenderableSeriesStyle"
                    };
                }
                else if (_selectedSeriesStyle == "Line")
                {
                    SeriesViewModels[0] = new LineRenderableSeriesViewModel
                    {
                        DataSeries = SeriesViewModels[0].DataSeries,
                        StyleKey = "BaseRenderableSeriesStyle"
                    };
                }
                else if (_selectedSeriesStyle == "Mountain")
                {
                    SeriesViewModels[0] = new MountainRenderableSeriesViewModel
                    {
                        DataSeries = SeriesViewModels[0].DataSeries,
                        StyleKey = "BaseRenderableSeriesStyle"
                    };
                }

                OnPropertyChanged("SeriesViewModels");
            }
        }      

        public IndexRange XVisibleRange
        {
            get => _xVisibleRange;
            set
            {
                if (!Equals(_xVisibleRange, value))
                {
                    _xVisibleRange = value;
                    OnPropertyChanged("XVisibleRange");
                }
            }
        }

        private void OnNewPrice(PriceBar price)
        {
            // Ensure only one update processed at a time from multi-threaded timer
            lock (this)
            {
                // Update the last price, or append? 
                var ds0 = (IOhlcDataSeries<DateTime, double>) _seriesViewModels[0].DataSeries;
                var ds1 = (IXyDataSeries<DateTime, double>) _seriesViewModels[1].DataSeries;

                if (_lastPrice != null && _lastPrice.DateTime == price.DateTime)
                {
                    ds0.Update(price.DateTime, price.Open, price.High, price.Low, price.Close);
                    ds1.Update(price.DateTime, _sma50.Update(price.Close).Current);
                }
                else
                {
                    ds0.Append(price.DateTime, price.Open, price.High, price.Low, price.Close);
                    ds1.Append(price.DateTime, _sma50.Push(price.Close).Current);

                    // If the latest appending point is inside the viewport (i.e. not off the edge of the screen)
                    // then scroll the viewport 1 bar, to keep the latest bar at the same place
                    if (XVisibleRange.Max > ds0.Count)
                    {
                        var existingRange = _xVisibleRange;
                        var newRange = new IndexRange(existingRange.Min + 1, existingRange.Max + 1);
                        XVisibleRange = newRange;
                    }
                }

                _lastPrice = price;
            }
        }      
    }
}