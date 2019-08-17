// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// RandomPricesDataSource.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************
using System;
using System.Timers;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public class RandomPricesDataSource
    {
        private sealed class PriceBarInfo
        {
            public DateTime DateTime;
            public double Close;
        }

        private const double Frequency = 1.1574074074074073E-05;
        private readonly Random _random;
        private readonly int _candleIntervalMinutes;
        private readonly bool _simulateDateGap;
        private readonly Timer _timer;
        private PriceBar _lastPriceBar;
        private readonly PriceBarInfo _initialPriceBar;
        private double _currentTime;
        private readonly int _updatesPerPrice;
        private int _currentUpdateCount;
        private readonly TimeSpan _openMarketTime = new TimeSpan(0, 08, 0, 0);
        private readonly TimeSpan _closeMarketTime = new TimeSpan(0, 16, 30, 0);
        public event OnNewData NewData;
        public event OnUpdateData UpdateData;

        public RandomPricesDataSource(int candleIntervalMinutes, bool simulateDateGap, double timerInterval, int updatesPerPrice, int randomSeed, double startingPrice, DateTime startDate)
        {
            _candleIntervalMinutes = candleIntervalMinutes;
            _simulateDateGap = simulateDateGap;
            _updatesPerPrice = updatesPerPrice;
            
            _timer = new Timer(timerInterval)
                             {
                                 Enabled = false,
                                 AutoReset = true,
                             };
            _timer.Elapsed += TimerElapsed;
            _initialPriceBar = new PriceBarInfo
                                       {
                                           Close = startingPrice,
                                           DateTime = startDate
                                       };
            _lastPriceBar = new PriceBar(_initialPriceBar.DateTime, _initialPriceBar.Close, _initialPriceBar.Close, _initialPriceBar.Close, _initialPriceBar.Close, 0L);
            _random = new Random(randomSeed);
        }

        public bool IsRunning       
        {
            get { return _timer.Enabled; }
        }

        public void StartGeneratePriceBars()
        {
            _timer.Enabled = true;            
        }

        public void StopGeneratePriceBars()
        {
            _timer.Enabled = false;
        }

        public PriceBar GetNextData()
        {
            PriceBar nextRandomPriceBar = GetNextRandomPriceBar();
            return nextRandomPriceBar;
        }

        private PriceBar GetNextRandomPriceBar()
        {
            double close = _lastPriceBar.Close;
            double num = (_random.NextDouble() - 0.9) * _initialPriceBar.Close / 30.0;
            double num2 = _random.NextDouble();
            double num3 = _initialPriceBar.Close + _initialPriceBar.Close / 2.0 * Math.Sin(7.27220521664304E-06 * _currentTime) + _initialPriceBar.Close / 16.0 * Math.Cos(7.27220521664304E-05 * _currentTime) + _initialPriceBar.Close / 32.0 * Math.Sin(7.27220521664304E-05 * (10.0 + num2) * _currentTime) + _initialPriceBar.Close / 64.0 * Math.Cos(7.27220521664304E-05 * (20.0 + num2) * _currentTime) + num;
            double num4 = Math.Max(close, num3);
            double num5 = _random.NextDouble() * _initialPriceBar.Close / 100.0;
            double high = num4 + num5;
            double num6 = Math.Min(close, num3);
            double num7 = _random.NextDouble() * _initialPriceBar.Close / 100.0;
            double low = num6 - num7;
            long volume = (long) (_random.NextDouble()*30000 + 20000);
            DateTime openTime = _simulateDateGap ? EmulateDateGap(_lastPriceBar.DateTime) : _lastPriceBar.DateTime;
            DateTime closeTime = openTime.AddMinutes(_candleIntervalMinutes);
            PriceBar candle = new PriceBar(closeTime, close, high, low, num3, volume);
            _lastPriceBar = new PriceBar(candle.DateTime, candle.Open, candle.High, candle.Low, candle.Close, volume);
            _currentTime += _candleIntervalMinutes * 60;
            return candle;
        }

        private DateTime EmulateDateGap(DateTime candleOpenTime)
        {
            DateTime result = candleOpenTime;
            TimeSpan timeOfDay = candleOpenTime.TimeOfDay;
            if (timeOfDay > _closeMarketTime)
            {
                DateTime dateTime = candleOpenTime.Date;
                dateTime = dateTime.AddDays(1.0);
                result = dateTime.Add(_openMarketTime);
            }
            while (result.DayOfWeek == DayOfWeek.Saturday || result.DayOfWeek == DayOfWeek.Sunday)
            {
                result = result.AddDays(1.0);
            }
            return result;
        }

#if SILVERLIGHT

        private void TimerElapsed(object sender, EventArgs e)
        {
            OnTimerElapsed();
        }

#else
        
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            OnTimerElapsed();
        }

#endif

        private void OnTimerElapsed()
        {
            if (_currentUpdateCount < _updatesPerPrice)
            {
                _currentUpdateCount++;
                PriceBar updatedData = GetUpdatedData();
                if (UpdateData != null)
                {
                    UpdateData(updatedData);
                }
            }
            else
            {
                _currentUpdateCount = 0;
                PriceBar nextData = GetNextData();
                if (NewData != null)
                {
                    NewData(nextData);
                }
            }
        }

        private PriceBar GetUpdatedData()
        {
            double num = _lastPriceBar.Close + ((_random.NextDouble() - 0.48) * (_lastPriceBar.Close / 100.0));
            double high = (num > _lastPriceBar.High) ? num : _lastPriceBar.High;
            double low = (num < _lastPriceBar.Low) ? num : _lastPriceBar.Low;
            long volumeInc = (long)((_random.NextDouble() * 30000 + 20000) * 0.05);
            _lastPriceBar = new PriceBar(_lastPriceBar.DateTime, _lastPriceBar.Open, high, low, num, _lastPriceBar.Volume + volumeInc);

            return _lastPriceBar;
        }

        public void ClearEventHandlers()
        {
            NewData = null;
            UpdateData = null;
        }

        public PriceBar Tick()
        {
            if (_currentUpdateCount < _updatesPerPrice)
            {
                _currentUpdateCount++;
                return GetUpdatedData();                
            }
            else
            {
                _currentUpdateCount = 0;
                return GetNextData();
            }
        }
    }
}