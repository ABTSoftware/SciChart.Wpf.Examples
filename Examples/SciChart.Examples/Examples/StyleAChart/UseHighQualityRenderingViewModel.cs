// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UseHighQualityRenderingViewModel.cs is part of the SCICHART® Examples. Permission is
// hereby granted to modify, create derivative works, distribute and publish any part of
// this source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.StyleAChart
{
    public class UseHighQualityRenderingViewModel : BaseViewModel
    {
        private IRange _sharedXVisibleRange;

        public IRange SharedXVisibleRange
        {
            get => _sharedXVisibleRange;
            set
            {
                _sharedXVisibleRange = value;
                OnPropertyChanged(nameof(SharedXVisibleRange));
            }
        }

        public IXyDataSeries<double, double> DataSeries { get; }

        public UseHighQualityRenderingViewModel()
        {
            DataSeries = CreateDataSeries();
        }

        private IXyDataSeries<double, double> CreateDataSeries()
        {
            const int count = 1000;

            var dataSeries = new XyDataSeries<double>();

            for (int i = 0; i < count; i++)
            {
                dataSeries.Append(i, count * Math.Sin(i * Math.PI * 0.1) / i);
            }
            return dataSeries;
        }
    }
}