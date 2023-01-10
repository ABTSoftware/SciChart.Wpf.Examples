// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
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
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    public class CategoryVsValueAxisViewModel : BaseViewModel
    {
        private IDataSeries<double, double> _dataSeries;
        private bool _useCategoryNumericAxis;
        
        private readonly double[] _xData = { 1, 1.8, 2.35, 3.4, 4, 12, 12.3, 13.2, 13.5, 14, 20, 20.1, 20.6, 21.5, 22, 23, 24.2, 24.8, 25.15, 25.65, 26 };
        private readonly double[] _yData = { 1, 4, 3.0, 5.2, 2, 2, 1.3, 7, 5.5, 6.3, 6.3, 5.8, 4.1, 5.5, 3, 3, 4.8, 4.1, 6, 5.1, 5.8 };

        public CategoryVsValueAxisViewModel()
        {
            DataSeries = new XyDataSeries<double> {AcceptsUnsortedData = true};
            DataSeries.Append(_xData, _yData);
        }
        
        public IDataSeries<double, double> DataSeries
        {
            get => _dataSeries;
            set
            {
                if (_dataSeries != value)
                {
                    _dataSeries = value;
                    OnPropertyChanged("DataSeries");
                }
            }
        }

        public bool UseCategoryNumericAxis
        {
            get => _useCategoryNumericAxis;
            set
            {
                _useCategoryNumericAxis = value;
                OnPropertyChanged("UseCategoryNumericAxis");
            }
        }
    }
}