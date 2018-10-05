// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UseHighQualityRenderingViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.StyleAChart
{
    public class UseHighQualityRenderingViewModel : BaseViewModel
    {
        private IXyDataSeries<double, double> _dataSeries0;

        public UseHighQualityRenderingViewModel()
        {
            _dataSeries0 = CreateDataSeries();
        }

        public IXyDataSeries<double, double> DataSeries0
        {
            get { return _dataSeries0; }
            set
            {
                if (_dataSeries0 == value) return;
                _dataSeries0 = value;
                OnPropertyChanged("DataSeries0");
            }
        }

        private IXyDataSeries<double, double> CreateDataSeries()
        {
            var ds0 = new XyDataSeries<double, double>();

            const int count = 1000;
            for (int i = 0; i < count; i++)
            {
                ds0.Append(i, count * Math.Sin(i * Math.PI * 0.1) / i);
            }

            return ds0;
        }
    }
}
