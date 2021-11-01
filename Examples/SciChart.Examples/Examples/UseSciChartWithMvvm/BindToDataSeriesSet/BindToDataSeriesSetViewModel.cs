// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// BindToDataSeriesSetViewModel.cs is part of the SCICHART® Examples. Permission is hereby
// granted to modify, create derivative works, distribute and publish any part of this
// source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Input;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.UseSciChartWithMvvm.BindToDataSeriesSet
{
    public class BindToDataSeriesSetViewModel : BaseViewModel
    {
        private IUniformXyDataSeries<double> _dataSeries0;
        private readonly RandomWalkGenerator _dataSource;

        public BindToDataSeriesSetViewModel()
        {
            ViewportManager = new DefaultViewportManager();

            // Create a DataSeriesSet
            _dataSeries0 = new UniformXyDataSeries<double>();

            // Create a single data-series
            _dataSource = new RandomWalkGenerator();

            // Append data to series
            _dataSeries0.Append(_dataSource.GetRandomWalkYData(1000));
        }

        // Databound to via SciChartSurface.DataSet in the view
        public IUniformXyDataSeries<double> ChartData
        {
            get => _dataSeries0;
            set
            {
                _dataSeries0 = value;
                OnPropertyChanged("ChartData");
            }
        }

        public IViewportManager ViewportManager { get; set; }

        public ICommand AppendDataCommand => new ActionCommand(AppendData);

        // Called when the AppendDataCommand is invoked via button click on the view
        private void AppendData()
        {
            _dataSeries0.Append(_dataSource.GetRandomWalkYData(50));

            ViewportManager.ZoomExtents();
        }
    }
}