// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// BindMultipleChartsGroupViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.ObjectModel;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.UseSciChartWithMvvm.BindMultipleCharts
{
    public class BindMultipleChartsGroupViewModel : BaseViewModel
    {
        private readonly RandomWalkGenerator _dataSource = new RandomWalkGenerator();

        // Expose N ChartViewModels for each ChartView to bind to
        public ObservableCollection<ChartViewModel> ChartViewModels { get; } = new ObservableCollection<ChartViewModel>();

        public BindMultipleChartsGroupViewModel()
        {
            // Instantiate 9 ChartViewModels
            for (int i = 0; i < 9; i++)
            {
                // Create a ChartViewModel          
                var chartViewModel = new ChartViewModel();             

                // Create the data-series
                var dataSeries = new UniformXyDataSeries<double>(i * 1000, 1d);

                // Create data
                var dummyYData = _dataSource.GetRandomWalkYData(1000);

                // Append X,Y data
                dataSeries.Append(dummyYData);

                // Set the DataSeries on the ChartViewModel
                chartViewModel.ChartData = dataSeries;

                ChartViewModels.Add(chartViewModel);
            }
        }
    }
}
