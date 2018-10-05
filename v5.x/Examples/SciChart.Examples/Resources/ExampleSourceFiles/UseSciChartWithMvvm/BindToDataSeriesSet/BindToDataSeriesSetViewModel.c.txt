// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// BindToDataSeriesSetViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Input;
using System.Windows.Media.Media3D;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.UseSciChartWithMvvm.BindToDataSeriesSet
{
    public class BindToDataSeriesSetViewModel : BaseViewModel
    {
        private IDataSeries<double, double> _dataSeries0;
        private readonly RandomWalkGenerator _dataSource;

        public BindToDataSeriesSetViewModel()
        {
            ViewportManager = new DefaultViewportManager();

            // Create a DataSeriesSet
            _dataSeries0 = new XyDataSeries<double, double>();

            // Create a single data-series
            _dataSource = new RandomWalkGenerator();
            var data = _dataSource.GetRandomWalkSeries(1000);

            // Append data to series.
            _dataSeries0.Append(data.XData, data.YData);
        }

        // Databound to via SciChartSurface.DataSet in the view
        public IDataSeries<double, double> ChartData
        {
            get { return _dataSeries0; }
            set 
            {
                _dataSeries0 = value;
                OnPropertyChanged("ChartData");
            }
        }

        public IViewportManager ViewportManager { get; set; }

        public ICommand AppendDataCommand
        {
            get { return new ActionCommand(AppendData); }
        }

        // Called when the AppendDataCommand is invoked via button click on the view
        private void AppendData()
        {
            var newData = _dataSource.GetRandomWalkSeries(50);

            _dataSeries0.Append(newData.XData, newData.YData);
            ViewportManager.ZoomExtents();
        }
    }
}