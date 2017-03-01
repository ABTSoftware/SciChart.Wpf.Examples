// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CategoryVsValueAxisViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Core.Helpers;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    public class CategoryVsValueAxisViewModel : BaseViewModel
    {
        private IDataSeries<double, double> _dataSeries;
        private bool _useCategoryNumericAxis;

        public CategoryVsValueAxisViewModel()
        {
            DataSeries = new XyDataSeries<double, double>();
            DataSeries.AcceptsUnsortedData = true;
            DataSeries.Append(GenerateXData(100), GenerateYData(100));
        }

        public IList<double> GenerateXData(int pointCount)
        {
            var retval = new List<double>();
            var random = new FasterRandom();

            for (var index = 0; index < pointCount; index++)
            {
                retval.Add(random.Next(1000));
            }

            retval.Sort();

            return retval;
        }

        public IList<double> GenerateYData(int pointCount)
        {
            var retval = new List<double>();
            var random = new FasterRandom();

            for (var index = 0; index < pointCount; index++)
            {
                retval.Add(random.Next(10));
            }

            return retval;
        }

        public IDataSeries<double, double> DataSeries
        {
            get { return _dataSeries; }
            set
            {
                if (_dataSeries == value) return;
                _dataSeries = value;
                OnPropertyChanged("DataSeries");
            }
        }

        public bool UseCategoryNumericAxis
        {
            get { return _useCategoryNumericAxis; }
            set
            {
                _useCategoryNumericAxis = value;
                OnPropertyChanged("UseCategoryNumericAxis");
            }
        }
    }
}