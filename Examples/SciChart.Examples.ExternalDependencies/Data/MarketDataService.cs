// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// MarketDataService.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Collections.Generic;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public interface IMarketDataService
    {
        void SubscribePriceUpdate(Action<PriceBar> callback);
        IEnumerable<PriceBar> GetHistoricalData(int numberBars);
        void ClearSubscriptions();
        PriceBar GetNextBar();
    }

    public delegate void OnNewData(PriceBar data);
    public delegate void OnUpdateData(PriceBar data);

    public class MarketDataService : IMarketDataService
    {
        private readonly DateTime _startDate;
        private readonly int _timeFrameMinutes;
        private readonly int _tickTimerIntervalms;
        private readonly RandomPricesDataSource _generator;

        public MarketDataService(DateTime startDate, int timeFrameMinutes, int tickTimerIntervalms)
        {
            _startDate = startDate;
            _timeFrameMinutes = timeFrameMinutes;
            _tickTimerIntervalms = tickTimerIntervalms;
            _generator = new RandomPricesDataSource(_timeFrameMinutes, true, _tickTimerIntervalms, 25, 367367, 30, _startDate);
        }

        public void SubscribePriceUpdate(Action<PriceBar> callback)
        {
            if (!_generator.IsRunning)
            {
                _generator.NewData += (arg) => callback(arg);
                _generator.UpdateData += (arg) => callback(arg);

                _generator.StartGeneratePriceBars();
            }
        }

        public IEnumerable<PriceBar> GetHistoricalData(int numberBars)
        {
            List<PriceBar> prices = new List<PriceBar>(numberBars);
            for (int i = 0; i < numberBars; i++)
            {
                prices.Add(_generator.GetNextData());
            }

            return prices;
        }

        public void ClearSubscriptions()
        {
            if (_generator.IsRunning)
            {
                _generator.StopGeneratePriceBars();
                _generator.ClearEventHandlers();
            }
        }

        public PriceBar GetNextBar()
        {
            return _generator.Tick();
        }
    }
}