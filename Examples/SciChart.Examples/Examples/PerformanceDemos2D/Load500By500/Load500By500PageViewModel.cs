// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Load500By500PageViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.PerformanceDemos2D.Load500By500
{
    public class Load500By500PageViewModel : BaseViewModel
    {
        private DispatcherObservableCollection<string> _messages;
        private int _seriesCount;
        private int _pointCount;
        private bool _isBusy;
        private IEnumerable<IDataSeries> _dataSeries;

        public Load500By500PageViewModel()
        {
            SeriesCount = 500;
            PointCount = 500;

            Messages = new DispatcherObservableCollection<string>();
            RunExampleCommand = new ActionCommand(OnRunExample);
        }

        private void UpdateExampleTitle()
        {
            OnPropertyChanged("ExampleTitle");
        }

        public ActionCommand RunExampleCommand { get; private set; }

        public IViewportManager ViewportManager { get; } = new DefaultViewportManager();

        public int SeriesCount
        {
            get => _seriesCount;
            set
            {
                if (_seriesCount != value)
                {
                    _seriesCount = value;
                    UpdateExampleTitle();
                    OnPropertyChanged("SeriesCount");
                }
            }
        }

        public int PointCount
        {
            get => _pointCount;
            set
            {
                if (_pointCount != value)
                {
                    _pointCount = value;
                    UpdateExampleTitle();
                    OnPropertyChanged("PointCount");
                }
            }
        }

        public DispatcherObservableCollection<string> Messages
        {
            get => _messages;
            set
            {
                if (_messages != value)
                {
                    _messages = value;
                    OnPropertyChanged("Messages");
                }
            }
        }

        public IEnumerable<IDataSeries> DataSeries
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

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
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
                IsBusy = true;
                var stopwatch = Stopwatch.StartNew();

                // Generate Data and mark time                 
                var yData = new double[SeriesCount][];
                var generator = new RandomWalkGenerator(0d);
                for (int i = 0; i < SeriesCount; i++)
                {
                    yData[i] = generator.GetRandomWalkYData(PointCount);
                    generator.Reset();
                }

                stopwatch.Stop();
                IsBusy = false;

                // Append to SciChartSurface and mark time
                stopwatch = Stopwatch.StartNew();
                var allDataSeries = new IDataSeries[SeriesCount];
                for (int i = 0; i < SeriesCount; i++)
                {
                    var dataSeries = new UniformXyDataSeries<double>();
                    dataSeries.Append(yData[i]);
                    allDataSeries[i] = dataSeries;
                }

                DataSeries = allDataSeries;
                stopwatch.Stop();

                ViewportManager.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
            });
        }
    }
}