// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
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
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.CreateRealtime3DCharts
{
    /// <summary>
    /// Interaction logic for CreateRealTime3DPointCloudChart.xaml
    /// </summary>
    public partial class CreateRealTime3DPointCloudChart : UserControl
    {
        private XyzDataSeries3D<double> _xyzData;
        private Timer _timer;
        private int _pointCount = 25000;

        public CreateRealTime3DPointCloudChart()
        {
            InitializeComponent();

            OnStart();
        }

        private void OnStart()
        {
            StartButton.IsChecked = true;
            PauseButton.IsChecked = false;
            ResetButton.IsChecked = false;

            if (ScatterRenderableSeries3D.DataSeries == null)
            {
                _xyzData = new XyzDataSeries3D<double>();
                ScatterRenderableSeries3D.DataSeries = _xyzData;
            }

            if (_timer == null)
            {
                _timer = new Timer(16);
                _timer.Elapsed += OnTimerTick;
            }

            _timer.Start();
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            lock (_timer)
            {
                Random r = new Random();

                // First load, fill with some random values                    
                if (_xyzData.Count == 0)
                {
                    for (int i = 0; i < _pointCount; i++)
                    {
                        double x = DataManager.Instance.GetGaussianRandomNumber(50, 15);
                        double y = DataManager.Instance.GetGaussianRandomNumber(50, 15);
                        double z = DataManager.Instance.GetGaussianRandomNumber(50, 15);

                        _xyzData.Append(x, y, z);
                    }

                    return;
                }

                // Subsequent load, update point positions using a sort of brownian motion by using random
                // numbers between -0.5, +0.5
                using (_xyzData.SuspendUpdates())
                {
                    for (int i = 0; i < _xyzData.Count; i++)
                    {
                        double currentX = _xyzData.XValues[i];
                        double currentY = _xyzData.YValues[i];
                        double currentZ = _xyzData.ZValues[i];

                        currentX += r.NextDouble() - 0.5;
                        currentY += r.NextDouble() - 0.5;
                        currentZ += r.NextDouble() - 0.5;

                        _xyzData.Update(i, currentX, currentY, currentZ);
                    }
                }
            }
        }

        private void OnPause()
        {
            _timer.Stop();

            StartButton.IsChecked = false;
            PauseButton.IsChecked = true;
            ResetButton.IsChecked = false;
        }

        private void OnReset()
        {
            _timer.Stop();

            StartButton.IsChecked = false;
            PauseButton.IsChecked = false;
            ResetButton.IsChecked = true;

            using (sciChart.SuspendUpdates())
            {
                ScatterRenderableSeries3D.DataSeries = null;
                sciChart.InvalidateElement();

                ScatterRenderableSeries3D.GetSceneEntity().Update();
                ScatterRenderableSeries3D.GetSceneEntity().RootSceneEntity.Update();
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
                _timer.Elapsed -= OnTimerTick;
                _timer = null;
            }
        }
    }
}
