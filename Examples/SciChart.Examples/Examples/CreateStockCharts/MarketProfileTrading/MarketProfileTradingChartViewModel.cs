// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HistogramBarDemoProjectViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;
using Brush = System.Windows.Media.Brush;

namespace SciChart.Examples.Examples.CreateStockCharts.MarketProfileTrading
{
    /// <summary>
    /// Represents a single data tick
    /// </summary>
    public class CandleTick : ITick
    {
        public double Price { get; set; }
        public DateTime TimeStamp { get; set; }
        public BidOrAsk BidOrAsk { get; set; }
        public long Volume { get; set; }
    }

    public class MarketProfileTradingChartViewModel : BaseViewModel
    {
        // Ranges
        private IViewportManager _viewportManager;
        private IRange _xTimeRange;
        private IRange _yPriceRange;

        // Price level
        private double _priceLevel = 0.5;
        // Bar Time Frame in Minutes
        private int _barTimeFrame = 30;
        private DateTime _startDate = new DateTime(2017, 1, 1);
        // Number of ticks 
        private int _ticksInBarTimeFrame = 250;
        private PriceBar _lastPriceTick = new PriceBar();

        // Data and Timer
        private MovingAverage _movingAverage;

        private OhlcDataSeries<DateTime, double> _priceDataSeries;
        private OhlcDataSeries<DateTime, double> _volumeDataSeries;
        private XyDataSeries<DateTime, double> _smaVolumeDataSeries;

        // Auxiliary
        private Random _random = new Random();
        private double _cumulativeVolume;
        private MarketDataService _marketDataService;

        // Appearance & Styling
        private HistogramMode _histogramBarMode = HistogramMode.MarketProfile;
        private double _verticalBarSpacing = 2d;
        private double _horizontalBarSpacing = 5d;

        private Brush _askStroke = new SolidColorBrush(Colors.White) { Opacity = 0.8 };
        private Brush _askFill = new SolidColorBrush(Colors.Gray) { Opacity = 0.6 };
        private Brush _bidStroke = new SolidColorBrush(Colors.White) { Opacity = 0.8 };
        private Brush _bidFill = new SolidColorBrush(Colors.Gray) { Opacity = 0.6 };

        private readonly object _tickLocker = new();

        //time\tprice\tvolume\tbidorask
        public MarketProfileTradingChartViewModel()
        {
            _viewportManager = new DefaultViewportManager();

            _priceDataSeries = new OhlcDataSeries<DateTime, double>();
            _volumeDataSeries = new OhlcDataSeries<DateTime, double>();
            _smaVolumeDataSeries = new XyDataSeries<DateTime, double>();
            _movingAverage = new MovingAverage(20);

            // Generate market data and fill the data series
            var initialBarNumber = 60;
            FillWithMarketData(initialBarNumber, _ticksInBarTimeFrame);

            // Setup X TimeRange to show the last 25 bars
            var lastIndex = _priceDataSeries.Count - 1;
            var timeRangeMax = _priceDataSeries.XValues[lastIndex];
            var timeRangeMin = _priceDataSeries.XValues[lastIndex - 10];
            XVisibleRange = new DateRange(timeRangeMin, timeRangeMax);

            //
            _viewportManager.ZoomExtentsY();

            // Init brushes
            ChangeBarStyling(_histogramBarMode);
        }

        public IViewportManager ViewportManager
        {
            get => _viewportManager;
            set
            {
                _viewportManager = value;
                OnPropertyChanged(nameof(ViewportManager));
            }
        }

        public IDataSeries MovingAverageVolumeDataSeries => _smaVolumeDataSeries;

        public Brush AskStroke
        {
            get => _askStroke;
            set
            {
                _askStroke = value;
                OnPropertyChanged("AskStroke");
            }
        }

        public Brush AskFill
        {
            get => _askFill;
            set
            {
                _askFill = value;
                OnPropertyChanged("AskFill");
            }
        }

        public Brush BidStroke
        {
            get => _bidStroke;
            set
            {
                _bidStroke = value;
                OnPropertyChanged("BidStroke");
            }
        }

        public Brush BidFill
        {
            get => _bidFill;
            set
            {
                _bidFill = value;
                OnPropertyChanged("BidFill");
            }
        }


        public ICommand StopUpdatesCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    _marketDataService.ClearSubscriptions();
                    ViewportManager = new DefaultViewportManager();
                });
            }
        }
        public ICommand StartUpdatesCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    ViewportManager = new HistogramBarViewportManager();
                    _marketDataService.SubscribePriceUpdate(OnNewPriceTick);
                });
            }
        }

        public double PriceLevel
        {
            get => _priceLevel;
            set
            {
                _priceLevel = value;
                OnPropertyChanged(nameof(PriceLevel));
            }
        }

        public double HorizontalBarSpacing
        {
            get => _horizontalBarSpacing;
            set
            {
                _horizontalBarSpacing = value;
                OnPropertyChanged(nameof(HorizontalBarSpacing));
            }
        }

        public double VerticalBarSpacing
        {
            get => _verticalBarSpacing;
            set
            {
                _verticalBarSpacing = value;
                OnPropertyChanged(nameof(VerticalBarSpacing));
            }
        }

        public IRange XVisibleRange
        {
            get => _xTimeRange;
            set
            {
                if (Equals(_xTimeRange, value))
                    return;
                _xTimeRange = value;
                OnPropertyChanged(nameof(XVisibleRange));
            }
        }

        public IRange YVisibleRange
        {
            get => _yPriceRange;
            set
            {
                if (Equals(_yPriceRange, value))
                    return;
                _yPriceRange = value;
                OnPropertyChanged(nameof(YVisibleRange));
            }
        }

        public OhlcDataSeries<DateTime, double> PriceDataSeries
        {
            get => _priceDataSeries;
            set
            {
                _priceDataSeries = value;
                OnPropertyChanged(nameof(PriceDataSeries));
            }
        }

        public OhlcDataSeries<DateTime, double> VolumeDataSeries
        {
            get => _volumeDataSeries;
            set
            {
                _volumeDataSeries = value;
                OnPropertyChanged(nameof(VolumeDataSeries));
            }
        }

        public HistogramMode HistogramBarMode
        {
            get => _histogramBarMode;
            set
            {
                _histogramBarMode = value;

                ChangeBarStyling(_histogramBarMode);
                OnPropertyChanged(nameof(HistogramBarMode));
            }
        }

        public void FillWithMarketData(int candlesCount, int ticksPerBar)
        {
            _marketDataService = new MarketDataService(_startDate, _barTimeFrame, 100, ticksPerBar);
            var ticks = candlesCount * ticksPerBar;
            Enumerable.Range(0, ticks).ForEachDo(_ => OnNewPriceTick(_marketDataService.GetNextBar()));
        }

        private void OnNewPriceTick(PriceBar newPriceTick)
        {
            // Ensure only one update processed at a time from multi-threaded timer
            lock (_tickLocker)
            {
                //  Round generated price to match price levels
                newPriceTick.Close = Math.Round(newPriceTick.Close, 1, MidpointRounding.AwayFromZero);
                // Determine tick type randomly
                var tickType = _random.Next(2) % 2 == 0 ? BidOrAsk.Ask : BidOrAsk.Bid;

                // Update the last price, or append? 
                if (_lastPriceTick.DateTime == newPriceTick.DateTime)
                {
                    UpdateLastBar(newPriceTick, tickType);
                }
                else
                {
                    AddNewBar(newPriceTick, tickType);

                    // X-range is updated in HistogramBarViewportManager
                }

                _lastPriceTick = newPriceTick;
            }
        }

        private void UpdateLastBar(PriceBar newTick, BidOrAsk tickType)
        {
            var lastIndex = _priceDataSeries.Count - 1;

            // Update Price DataSeries
            var newPrice = newTick.Close;
            var high = _priceDataSeries.HighValues[lastIndex];
            high = high > newPrice ? high : newPrice;

            var low = _priceDataSeries.LowValues[lastIndex];
            low = low < newPrice ? low : newPrice;
            _priceDataSeries.Update(lastIndex, _priceDataSeries.OpenValues[lastIndex], high, low, newPrice);
            var metaData = (CandlestickMetaData)_priceDataSeries.Metadata[lastIndex];

            metaData.AddTick(new CandleTick
            {
                BidOrAsk = tickType,
                Price = newTick.Close,
                Volume = newTick.Volume,
                TimeStamp = newTick.DateTime
            });

            // Update Volume DataSeries
            _cumulativeVolume += tickType.Equals(BidOrAsk.Ask) ? newTick.Volume : -newTick.Volume;

            high = _volumeDataSeries.HighValues[lastIndex];
            high = high > _cumulativeVolume ? high : _cumulativeVolume;
            low = _volumeDataSeries.LowValues[lastIndex];
            low = low < _cumulativeVolume ? low : _cumulativeVolume;
            _volumeDataSeries.Update(lastIndex, _volumeDataSeries.OpenValues[lastIndex], high, low, _cumulativeVolume);
        }

        private void AddNewBar(PriceBar newTick, BidOrAsk tickType)
        {
            // Update Price DataSeries
            var barMetadata = new CandlestickMetaData();
            barMetadata.AddTick(new CandleTick
            {
                BidOrAsk = tickType,
                Price = newTick.Close,
                Volume = newTick.Volume,
                TimeStamp = newTick.DateTime
            });
            _priceDataSeries.Append(newTick.DateTime, newTick.Close, newTick.Close, newTick.Close, newTick.Close, barMetadata);

            // Update Volume DataSeries
            var volume = tickType.Equals(BidOrAsk.Ask) ? newTick.Volume : -newTick.Volume;

            var high = volume > 0 ? volume : 0;
            var low = volume < 0 ? volume : 0;
            _volumeDataSeries.Append(newTick.DateTime, volume, high, low, volume);

            // Update Volume SMA
            var lastTickTime = _lastPriceTick.DateTime;
            var dateTime = lastTickTime.IsDefined() ? lastTickTime : newTick.DateTime;
            _smaVolumeDataSeries.Append(dateTime, _movingAverage.Push(_cumulativeVolume).Current);

            // Reset the Cumulative Volume for a new bar
            _cumulativeVolume = volume;
        }

        private void ChangeBarStyling(HistogramMode histogramBarMode)
        {
            var brushConverter = new BrushConverter();
            var askBrush = ((SolidColorBrush)brushConverter.ConvertFrom("#7052CC54"));
            var bidBrush = ((SolidColorBrush)brushConverter.ConvertFrom("#D0E26565"));

            switch (histogramBarMode)
            {
                case HistogramMode.CumulativeVolume:

                    BidStroke = new SolidColorBrush(Colors.Crimson) { Opacity = 0.8 };
                    BidFill = new SolidColorBrush(Colors.Crimson) { Opacity = 0.6 };

                    AskStroke = new SolidColorBrush(Colors.LimeGreen) { Opacity = 0.8 };
                    AskFill = new SolidColorBrush(Colors.LimeGreen) { Opacity = 0.6 };

                    break;

                case HistogramMode.VolumeLadder:
                case HistogramMode.MarketProfile:

                    AskStroke = askBrush;
                    AskFill = askBrush;

                    BidStroke = bidBrush;
                    BidFill = bidBrush;

                    break;
            }
        }
    }

}