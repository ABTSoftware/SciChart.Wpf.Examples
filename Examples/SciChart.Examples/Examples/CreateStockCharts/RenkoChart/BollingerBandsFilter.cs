// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// BollingerBandsFilter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using System.Collections.Generic;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;

namespace SciChart.Examples.Examples.CreateStockCharts.RenkoChart
{
    public sealed class BollingerBandsFilter : FilterBase
    {
        private readonly double _stdScale;
        private readonly int _smaLength;
        private readonly IOhlcDataSeries<DateTime, double> _priceDataSeries;
        private readonly XyyDataSeries<DateTime, double> _filteredDataSeries = new XyyDataSeries<DateTime, double>();

        public BollingerBandsFilter(IOhlcDataSeries<DateTime, double> priceDataSeries, int smaLength, double stdScale) : base(priceDataSeries)
        {
            _stdScale = stdScale;
            _smaLength = smaLength;
            _priceDataSeries = priceDataSeries;

            FilteredDataSeries = _filteredDataSeries;
            FilterAll();
        }

        public override void FilterAll()
        {
            _filteredDataSeries.Clear();

            for (int i = 0; i < _priceDataSeries.Count; ++i)
            {
                CalculateAverageAndStd(_priceDataSeries.CloseValues, i - _smaLength + 1, _smaLength, out double average, out double std);
                _filteredDataSeries.Append(_priceDataSeries.XValues[i], average - _stdScale * std, average + _stdScale * std);
            }
        }

        public static void CalculateAverageAndStd(IList<double> values, int startIndex, int length, out double average, out double std)
        {
            average = std = double.NaN;

            var endIndex = startIndex + length;

            if (length > 0 &&
                values.Count > 0 &&
                startIndex >= 0 &&
                endIndex <= values.Count &&
                startIndex < endIndex)
            {
                // Find average
                var sum = 0d;
                for (int i = startIndex; i < endIndex; i++)
                {
                    sum += values[i];
                }
                average = sum / length;

                // Find STD
                sum = 0d;
                for (int i = startIndex; i < endIndex; i++)
                {
                    sum += Math.Pow(values[i] - average, 2);
                }
                std = Math.Sqrt(sum / length);
            }
        }
    }
}
