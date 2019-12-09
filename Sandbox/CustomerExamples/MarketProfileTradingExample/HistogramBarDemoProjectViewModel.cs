// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
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
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

namespace MarketProfileTradingChartExample
{
    public class CandleTick : ITick
    {
        public double Price { get; set; }
        public DateTime TimeStamp { get; set; }
        public BidOrAsk BidOrAsk { get; set; }
        public long Volume { get; set; }
    }

    public class HistogramBarDemoProjectViewModel : INotifyPropertyChanged
    {
        private readonly Timer _timerNewDataUpdate;
        private Random _random;
        public MovingAverage _movingAverage;

        private OhlcDataSeries<DateTime, double> _dataSeries0;
        private OhlcDataSeries<DateTime, double> _dataSeries1;
        private XyDataSeries<DateTime, double> _filterDataSeries;

        private IViewportManager _viewportManager;

        private int _index;
        private int _candleCount;
        private double[] _data;

        private bool _alowToChangeVisibleRangeToMax;
        private bool _yAutoRange;
        private IRange _xVisibleRange;
        private IRange _yVisibleRange;

        private double _verticalBarSpacing;
        private double _horizontalBarSpacing;
        private Brush _askStroke = new SolidColorBrush(Colors.White) { Opacity = 0.8 };
        private Brush _askFill = new SolidColorBrush(Colors.Gray) { Opacity = 0.6 };
        private Brush _bidStroke = new SolidColorBrush(Colors.White) { Opacity = 0.8 };
        private Brush _bidFill = new SolidColorBrush(Colors.Gray) { Opacity = 0.6 };

        public HistogramMode _histogramBarMode;
        private double _tickSize;
        public int _maxHistoBarCandles;
        private double _cumulativeVolume;

        private int _ticksPerCandle;

        private DateTime _baseTime = new DateTime(2017, 1, 1);

        //time\tprice\tvolume\tbidorask
        public HistogramBarDemoProjectViewModel()
        {
            _random = new Random();

            _timerNewDataUpdate = new Timer(1);
            _timerNewDataUpdate.AutoReset = true;
            _timerNewDataUpdate.Elapsed += OnNewData;

            _viewportManager = new HistogramBarViewportManager();

            _dataSeries0 = new OhlcDataSeries<DateTime, double>();
            _dataSeries1 = new OhlcDataSeries<DateTime, double>();

            _filterDataSeries = new XyDataSeries<DateTime, double>();
            _movingAverage = new MovingAverage(5);

            YAutoRange = true;
            AlowToChangeVisibleRangeToMax = true;

            HorizontalBarSpacing = 5;
            HistogramBarMode = HistogramMode.MarketProfile;
            MaxHistoBarCandles = 40;
            TickSize = 0.5;

              LoadRandomData(30, 250);
            _viewportManager.ZoomExtents();
        }

        public IViewportManager ViewportManager
        {
            get { return _viewportManager; }
        }

        public IDataSeries MovingAverageLine
        {
            get
            {
                return _filterDataSeries;
            }
        }

        public Brush AskStroke
        {
            get { return _askStroke; }
            set
            {
                _askStroke = value;
                OnPropertyChanged("AskStroke");
            }
        }

        public int MaxHistoBarCandles
        {
            get { return _maxHistoBarCandles; }
            set
            {
                _maxHistoBarCandles = value;
                OnPropertyChanged("MaxHistoBarCandles");
            }
        }

        public Brush AskFill
        {
            get { return _askFill; }
            set
            {
                _askFill = value;
                OnPropertyChanged("AskFill");
            }
        }

        public Brush BidStroke
        {
            get { return _bidStroke; }
            set
            {
                _bidStroke = value;
                OnPropertyChanged("BidStroke");
            }
        }

        public Brush BidFill
        {
            get { return _bidFill; }
            set
            {
                _bidFill = value;
                OnPropertyChanged("BidFill");
            }
        }


        public ICommand StopUpdatesCommand { get { return new ActionCommand(() => _timerNewDataUpdate.Stop()); } }
        public ICommand StartUpdatesCommand { get { return new ActionCommand(() => _timerNewDataUpdate.Start()); } }

        public double TickSize
        {
            get { return _tickSize; }
            set
            {
                _tickSize = value;
                OnPropertyChanged("TickSize");
            }
        }

        public double HorizontalBarSpacing
        {
            get { return _horizontalBarSpacing; }
            set
            {
                _horizontalBarSpacing = value;
                OnPropertyChanged("HorizontalBarSpacing");
            }
        }

        public double VerticalBarSpacing
        {
            get { return _verticalBarSpacing; }
            set
            {
                _verticalBarSpacing = value;
                OnPropertyChanged("VerticalBarSpacing");
            }
        }

        public IRange XVisibleRange
        {
            get { return _xVisibleRange; }
            set
            {
                if (Equals(_xVisibleRange, value))
                    return;
                _xVisibleRange = value;
                OnPropertyChanged("XVisibleRange");
            }
        }

        public IRange YVisibleRange
        {
            get { return _yVisibleRange; }
            set
            {
                if (Equals(_yVisibleRange, value))
                    return;
                _yVisibleRange = value;
                OnPropertyChanged("YVisibleRange");
            }
        }

        public OhlcDataSeries<DateTime, double> DataSeries
        {
            get { return _dataSeries0; }
            set
            {
                _dataSeries0 = value;
                OnPropertyChanged("DataSeries");
            }
        }

        public OhlcDataSeries<DateTime, double> BottomChartDataSeries
        {
            get { return _dataSeries1; }
            set
            {
                _dataSeries1 = value;
                OnPropertyChanged("BottomChartDataSeries");
            }
        }

        public bool YAutoRange
        {
            get { return _yAutoRange; }
            set
            {
                _yAutoRange = value;
                OnPropertyChanged("YAutoRange");
            }
        }

        public HistogramMode HistogramBarMode
        {
            get { return _histogramBarMode; }
            set
            {
                _histogramBarMode = value;

                ChangeBarStyling(_histogramBarMode);
                OnPropertyChanged("HistogramBarMode");
            }
        }

        public bool AlowToChangeVisibleRangeToMax
        {
            get { return _alowToChangeVisibleRangeToMax; }
            set
            {
                _alowToChangeVisibleRangeToMax = value;
                OnPropertyChanged("AlowToChangeVisibleRangeToMax");
            }
        }

        public void LoadRandomData(int candlesCount, int ticksPerCandle)
        {
            _ticksPerCandle = ticksPerCandle;

            _data = null;

            var dataSource = new RandomWalkGenerator();

            var ticksCount = (candlesCount + 1) * ticksPerCandle;
            _data = dataSource.GetRandomWalkSeries(ticksCount).YData.ToArray();

            _index = 0;
           //var baseDate = DateTime.Now;
            for (int j = 0; j < candlesCount; j++)
            {
                var date = _baseTime .AddMinutes(j*30);
                var volume = _random.Next(100);
                var bidOrAsk = _random.Next(2) == 0 ? BidOrAsk.Bid : BidOrAsk.Ask;
                var cumulativeVolume = default(double);

                var metaData = new CandlestickMetaData();
                metaData.AddTick(new CandleTick
                {
                    BidOrAsk = bidOrAsk,
                    Price = _data[_index],
                    Volume = volume,
                    TimeStamp = date
                });

                cumulativeVolume += bidOrAsk.Equals(BidOrAsk.Ask) ? volume : -volume;

                var high = cumulativeVolume > 0 ? cumulativeVolume : 0;
                var low = cumulativeVolume < 0 ? cumulativeVolume : 0;

                _dataSeries1.Append(date, cumulativeVolume, high, low, cumulativeVolume);
                _dataSeries0.Append(date, _data[_index], _data[_index], _data[_index], _data[_index], metaData);
                _candleCount = _dataSeries0.Count;

                for (int i = 0; i < ticksPerCandle; i++)
                {
                    _index++;

                    volume = _random.Next(100);
                    bidOrAsk = _random.Next(2) == 0 ? BidOrAsk.Bid : BidOrAsk.Ask;

                    //date = date;
                    var newTick = _data[_index];
                    var open = _dataSeries0.OpenValues[_candleCount - 1];
                    high = _dataSeries0.HighValues[_candleCount - 1];
                    high = high > newTick ? high : newTick;

                    low = _dataSeries0.LowValues[_candleCount - 1];
                    low = low < newTick ? low : newTick;

                    var meta = (CandlestickMetaData)_dataSeries0.Metadata[_candleCount - 1];

                    meta.AddTick(new CandleTick
                    {
                        BidOrAsk = bidOrAsk,
                        Price = newTick,
                        Volume = volume,
                        TimeStamp = date
                    });

                    _dataSeries0.Update(_candleCount - 1, open, high, low, newTick);

                    cumulativeVolume += bidOrAsk.Equals(BidOrAsk.Ask) ? volume : -volume;

                    open = _dataSeries1.OpenValues[_candleCount - 1];
                    high = _dataSeries1.HighValues[_candleCount - 1];
                    high = high > cumulativeVolume ? high : cumulativeVolume;

                    low = _dataSeries1.LowValues[_candleCount - 1];
                    low = low < cumulativeVolume ? low : cumulativeVolume;

                    _dataSeries1.Update(_candleCount - 1, open, high, low, cumulativeVolume);
                }

                _filterDataSeries.Append(date, _movingAverage.Push(cumulativeVolume).Current);
            }
        }

        private void OnNewData(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (XVisibleRange != null)
            {
                lock (elapsedEventArgs)
                {
                    if(_index >= _data.Length) _timerNewDataUpdate.Stop();

                    var newTick = _data[_index];
                    var date = _dataSeries0.XValues[_dataSeries0.Count - 1].AddMinutes(30);

                    var volume = _random.Next(100);
                    var bidOrAsk = _random.Next(2) == 0 ? BidOrAsk.Bid : BidOrAsk.Ask;

                    if (_index % _ticksPerCandle == 0)
                    {
                        _filterDataSeries.Append(date, _movingAverage.Push(_cumulativeVolume).Current);
                        _cumulativeVolume = default(double);

                        var metaNew = new CandlestickMetaData();
                        metaNew.AddTick(new CandleTick
                        {
                            BidOrAsk = bidOrAsk,
                            Price = newTick,
                            Volume = volume,
                            TimeStamp = date
                        });

                        _cumulativeVolume += bidOrAsk.Equals(BidOrAsk.Ask) ? volume : -volume;

                        var high = _cumulativeVolume > 0 ? _cumulativeVolume : 0;
                        var low = _cumulativeVolume < 0 ? _cumulativeVolume : 0;

                        _dataSeries1.Append(date, _cumulativeVolume, high, low, _cumulativeVolume);
                        _dataSeries0.Append(date, newTick, newTick, newTick, newTick, metaNew);
                        _candleCount = _dataSeries0.Count;

                        var visibleRange = (IndexRange)XVisibleRange;
                        if (visibleRange.Max + 2 >= _dataSeries0.Count)
                        {
                            var newRange = new IndexRange(visibleRange.Min + 1, visibleRange.Max + 1);
                            XVisibleRange = newRange;
                        }
                    }
                    else
                    {
                        var open = _dataSeries0.OpenValues[_candleCount - 1];
                        var high = _dataSeries0.HighValues[_candleCount - 1];
                        high = high > newTick ? high : newTick;

                        var low = _dataSeries0.LowValues[_candleCount - 1];
                        low = low < newTick ? low : newTick;

                        var metaData = (CandlestickMetaData)_dataSeries0.Metadata[_candleCount - 1];

                        metaData.AddTick(new CandleTick
                        {
                            BidOrAsk = bidOrAsk,
                            Price = newTick,
                            Volume = volume,
                            TimeStamp = date
                        });


                        _dataSeries0.Update(_candleCount - 1, open, high, low, newTick);

                        _cumulativeVolume += bidOrAsk.Equals(BidOrAsk.Ask) ? volume : -volume;

                        open = _dataSeries1.OpenValues[_candleCount - 1];
                        high = _dataSeries1.HighValues[_candleCount - 1];
                        high = high > _cumulativeVolume ? high : _cumulativeVolume;

                        low = _dataSeries1.LowValues[_candleCount - 1];
                        low = low < _cumulativeVolume ? low : _cumulativeVolume;

                        _dataSeries1.Update(_candleCount - 1, open, high, low, _cumulativeVolume);
                    }

                    ++_index;
                }
            }
        }

        private void ChangeBarStyling(HistogramMode histogramBarMode)
        {
            var brushConverter = new BrushConverter();
            var askBrush = ((SolidColorBrush)brushConverter.ConvertFrom("#7052CC54"));
            var bidBrush = ((SolidColorBrush)brushConverter.ConvertFrom("#D0E26565"));

            switch (histogramBarMode)
            {
                case HistogramMode.VolumeLadder:

                    AskStroke = new SolidColorBrush(Colors.White) { Opacity = 0.8 };
                    AskFill = askBrush;

                    BidStroke = new SolidColorBrush(Colors.White) { Opacity = 0.8 };
                    BidFill = bidBrush;

                    break;

                case HistogramMode.CumulativeVolume:

                    AskStroke = new SolidColorBrush(Colors.Crimson) { Opacity = 0.8 };
                    AskFill = new SolidColorBrush(Colors.Crimson) { Opacity = 0.6 };

                    BidStroke = new SolidColorBrush(Colors.LimeGreen) { Opacity = 0.8 };
                    BidFill = new SolidColorBrush(Colors.LimeGreen) { Opacity = 0.6 };

                    break;

                case HistogramMode.MarketProfile:
                  
                    AskStroke = askBrush;
                    AskFill = askBrush;
                   
                    BidStroke = bidBrush;
                    BidFill = bidBrush;

                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}