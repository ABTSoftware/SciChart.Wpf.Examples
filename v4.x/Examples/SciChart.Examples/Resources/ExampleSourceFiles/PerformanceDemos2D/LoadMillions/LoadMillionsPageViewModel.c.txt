// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LoadMillionsPageViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.PerformanceDemos2D.LoadMillions
{
    public class LoadMillionsPageViewModel : BaseViewModel
    {
        private IXyDataSeries<double, double> _dataSeries;
        private readonly IViewportManager _viewportManager = new DefaultViewportManager();

        private const int Count = 1000000;

        public LoadMillionsPageViewModel()
        {
            RunExampleCommand = new ActionCommand(OnRunExample);
        }

        public ActionCommand RunExampleCommand { get; private set; }

        public IViewportManager ViewportManager
        {
            get { return _viewportManager; }
        }

        public IXyDataSeries<double, double> DataSeries
        {
            get { return _dataSeries; }
            set
            {
                if (_dataSeries == value) return;
                _dataSeries = value;
                OnPropertyChanged("DataSeries");
            }
        }

        public void OnPageExit()
        {
            DataSeries = null;
        }

        private void OnRunExample()
        {
            Task.Factory.StartNew(() =>
                {
                    DataSeries = null;

                    // Generate Data and mark time 
                    var dataSeries = new XyDataSeries<double, double>();
                    var stopwatch = Stopwatch.StartNew();
                    var xyData = new RandomWalkGenerator(0.0d).GetRandomWalkSeries(Count);
                    stopwatch.Stop();

                    // Append to SciChartSurface and mark time
                    stopwatch = Stopwatch.StartNew();
                    dataSeries.Append(xyData.XData, xyData.YData);
                    DataSeries = dataSeries;
                    stopwatch.Stop();

                    // Zoom viewport to extents
                    ViewportManager.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
                });
        }
    }
}