// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// VitalSignsDataProvider.cs is part of the SCICHART® Examples. Permission is hereby
// granted to modify, create derivative works, distribute and publish any part of this
// source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.VitalSignsMonitor
{
    public class VitalSignsDataProvider
    {
        private readonly TimeSpan _period;
        private readonly IList<VitalSignsData> _vitalSignsData;

        private int _currentIndex;
        private int _totalIndex;
        private bool _isATrace;

        public VitalSignsDataProvider(TimeSpan period)
        {
            _period = period;
            _vitalSignsData = DataManager.Instance.GetVitalSignsData();
        }

        public IObservable<VitalSignsData> Data => Observable.Interval(_period).Select(t => OnNext());

        protected VitalSignsData OnNext()
        {
            if (_currentIndex >= _vitalSignsData.Count)
                _currentIndex = 0;

            var data = _vitalSignsData[_currentIndex];

            data.XValue = _totalIndex * 10 / 800f % 10;
            data.IsATrace = _isATrace;

            _currentIndex += 10;
            _totalIndex++;

            if (_totalIndex % 800 == 0)
                _isATrace = !_isATrace;

            return data;
        }

        public DoubleRange EcgHeartRateRange => GetRange(_vitalSignsData.Select(x => x.ECGHeartRate));
        public DoubleRange BloodPressureRange => GetRange(_vitalSignsData.Select(x => x.BloodPressure));

        public DoubleRange BloodVolumeRange => GetRange(_vitalSignsData.Select(x => x.BloodVolume));
        public DoubleRange BloodOxygenationRange => GetRange(_vitalSignsData.Select(x => x.BloodOxygenation));

        private DoubleRange GetRange(IEnumerable<double> values)
        {
            var min = double.MaxValue;
            var max = double.MinValue;

            foreach (var value in values)
            {
                if (value < min) min = value;
                if (value > max) max = value;
            }

            return new DoubleRange(min, max);
        }
    }
}