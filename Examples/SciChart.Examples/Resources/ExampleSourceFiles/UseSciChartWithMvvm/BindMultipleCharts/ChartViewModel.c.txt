// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ChartViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.UseSciChartWithMvvm.BindMultipleCharts
{
    public class ChartViewModel : BaseViewModel
    {
        private IDataSeries _chartData;
        private AutoRange _AutoRange;

        public IDataSeries ChartData
        {
            get { return _chartData; }
            set
            {
                _chartData = value;
                OnPropertyChanged("ChartData");
            }
        }

        public AutoRange AutoRange
        {
            get { return _AutoRange; }
            set
            {
                _AutoRange = value;
                OnPropertyChanged("AutoRange");
            }
        } 
    }
}
