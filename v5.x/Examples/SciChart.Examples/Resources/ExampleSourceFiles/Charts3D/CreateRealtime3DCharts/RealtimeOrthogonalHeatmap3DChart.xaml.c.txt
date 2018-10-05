// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// RealtimeOrthogonalHeatmap3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D.Model;
using SciChart.Core.Helpers;

namespace SciChart.Examples.Examples.Charts3D.CreateRealtime3DCharts
{
    /// <summary>
    /// Interaction logic for RealtimeOrthogonalHeatmap3DChart.xaml
    /// </summary>
    public partial class RealtimeOrthogonalHeatmap3DChart : UserControl
    {
        // Data Sample Rate (sec) 
        private double dt = 0.02;

        // FIFO Size is 1200 samples, meaning after 1200 samples have been appended, each new sample appended
        // results in one sample being discarded
        private int FifoSize = 1200;

        // Timer to process updates
        private readonly DispatcherTimer _timerNewDataUpdate;

        // The current time
        private double t;

        private int _tick;
        private int _step = 3;

        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        FasterRandom _random = new FasterRandom();

        // The dataseries to fill
        private IXyDataSeries<double, double> _series0;
        private UniformGridDataSeries3D<double> _series1;
        private UniformGridDataSeries3D<double> _series2;

        private readonly double[] _tValues = new double[100];
        private readonly double[] _yValues = new double[100];

        public RealtimeOrthogonalHeatmap3DChart()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            _timerNewDataUpdate = new DispatcherTimer(DispatcherPriority.Render);
            _timerNewDataUpdate.Tick += OnNewData;
            _timerNewDataUpdate.Interval = TimeSpan.FromMilliseconds(25);

            CreateDataSetAndSeries();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _timerNewDataUpdate.Stop();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _timerNewDataUpdate.Start();
            
        }

        private void CreateDataSetAndSeries()
        {
            _series0 = new XyDataSeries<double, double>();
            _series1 = new UniformGridDataSeries3D<double>(100, 100) { StartX = 0, StepX = 0.1, StartZ = 0 };
            _series2 = new UniformGridDataSeries3D<double>(100, 100) { StartX = 0, StepX = 0.1, StartZ = 0 };

            _series0.FifoCapacity = FifoSize;

            for (int a = 0; a < FifoSize; a++)
            {
                OnNewData(this, null);
            }

            RenderableSeries0.DataSeries = _series0;
            surfaceMesh.Maximum = 22;
            surfaceMesh.Minimum = 0;
            surfaceMesh.DataSeries = _series1;

            RenderableSeries1.DataSeries = _series0;
            surfaceMesh1.Maximum = 22;
            surfaceMesh1.Minimum = 0;
            surfaceMesh1.DataSeries = _series2;
        }

        // Add 100 new data to DataSeries's        
        private void OnNewData(object sender, EventArgs e)
        {
            _tick++;
            if (_tick == 2)
            {
                _tick = 0;
                _step = _random.Next(0, 11);
            }

            var massVal = new double[100];

            for (int i = 0; i < 100; i++)
            {
                double y = _step * Math.Sin(((2 * Math.PI) * 0.4) * t) + _random.NextDouble() * 2;
                _yValues[i] = y;
                _tValues[i] = t;
                massVal[i] = y + 10;                

                t += dt;
            }

            var sortData = massVal.OrderByDescending(x => x);

            using (_series0.SuspendUpdates())
            using (_series1.SuspendUpdates())
            using (_series2.SuspendUpdates())
            {
                _series0.Append(_tValues, _yValues);
                _series1.PushRow(sortData.ToArray());
                _series2.PushRow(sortData.ToArray());
            }
        }
    }
}