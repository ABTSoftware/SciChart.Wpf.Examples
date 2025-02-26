// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateRealTime3DPointCloudChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D.Model;
using SciChart.Data.Extensions;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.CreateRealtime3DCharts
{
    /// <summary>
    /// Interaction logic for CreateRealTime3DPointCloudChart.xaml
    /// </summary>
    public partial class CreateRealTime3DPointCloudChart : UserControl
    {
        private XyzDataSeries3D<double> _xyzData;
        private DispatcherTimer _timer;

        private int _pointCount = 100000;
        private readonly Random _random = new Random();

        private bool _isRunning;
        private bool _isReset;

        public CreateRealTime3DPointCloudChart()
        {
            InitializeComponent();

            OnStart();
        }

        private void OnStart()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _isReset = false;

                StartButton.IsChecked = true;
                PauseButton.IsChecked = false;
                PauseButton.IsEnabled = true;
                ResetButton.IsChecked = false;

                if (ScatterRenderableSeries3D.DataSeries == null)
                {
                    _xyzData = new XyzDataSeries3D<double>();

                    // First load, fill with some random values                    
                    for (int i = 0; i < _pointCount; i++)
                    {
                        double x = DataManager.Instance.GetGaussianRandomNumber(50, 15);
                        double y = DataManager.Instance.GetGaussianRandomNumber(50, 15);
                        double z = DataManager.Instance.GetGaussianRandomNumber(50, 15);

                        _xyzData.Append(x, y, z);
                    }

                    ScatterRenderableSeries3D.DataSeries = _xyzData;
                }

                if (_timer == null)
                {
                    _timer = new DispatcherTimer(DispatcherPriority.Render);
                    _timer.Interval = TimeSpan.FromMilliseconds(1);
                    _timer.Tick += OnTimerTick;
                }

                _timer.Start();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            // Subsequent load, update point positions using a sort of brownian motion by using random
            //
            // Access Raw 'arrays' to the inner data series. This is the fastest way to read and access data, however
            // any operations will not be thread safe, and will not trigger a redraw. This is why we invalidate below
            //
            // See also XyzDataSeries.Append, Update, Remove, Insert which are atomic operations
            // 
            // Note that the count of raw arrays may be greater than _xyzData.Count
            double[] xDataRaw = _xyzData.XValues.ToUncheckedList();
            double[] yDataRaw = _xyzData.YValues.ToUncheckedList();
            double[] zDataRaw = _xyzData.ZValues.ToUncheckedList();

            // Update the data positions simulating 3D random walk / brownian motion 
            for (int i = 0, count = _xyzData.Count; i < count; i++)
            {
                xDataRaw[i] += _random.NextDouble() - 0.5;
                yDataRaw[i] += _random.NextDouble() - 0.5;
                zDataRaw[i] += _random.NextDouble() - 0.5;
            }

            // Raise DataSeriesChanged event and trigger chart updates
            _xyzData.IsDirty = true;
            _xyzData.OnDataSeriesChanged(DataSeriesUpdate.DataChanged, DataSeriesAction.Update);
        }

        private void OnPause()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _isReset = false;

                _timer.Stop();

                StartButton.IsChecked = false;
                PauseButton.IsChecked = true;
                ResetButton.IsChecked = false;
            }
        }

        private void OnReset()
        {
            if (!_isReset)
            {
                _isRunning = false;
                _isReset = true;

                _timer.Stop();

                StartButton.IsChecked = false;
                PauseButton.IsChecked = false;
                PauseButton.IsEnabled = false;
                ResetButton.IsChecked = true;

                using (sciChart.SuspendUpdates())
                {
                    ScatterRenderableSeries3D.DataSeries = null;
                    sciChart.InvalidateElement();

                    ScatterRenderableSeries3D.GetSceneEntity().Update();
                    ScatterRenderableSeries3D.GetSceneEntity().RootSceneEntity.Update();
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            OnStart();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            OnPause();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            OnReset();
        }

        private void CreateRealTime3DPointCloudChart_OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= OnTimerTick;
                _timer = null;
            }
        }
    }
}
