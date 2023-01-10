// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateRealTime3DGeoidChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SciChart.Charting3D.Model;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.Charts3D.CreateRealtime3DCharts
{
    /// <summary>
    /// Interaction logic for CreateRealTime3DGeoidChart.xaml
    /// </summary>
    public partial class CreateRealTime3DGeoidChart : UserControl
    {
        private DispatcherTimer _timer;
        private bool _isRunning;
        private readonly object _syncRoot = new object();

        public CreateRealTime3DGeoidChart()
        {
            InitializeComponent();

            Loaded += (s, e) => OnStart();

            Unloaded += (s, e) => OnStop();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            OnStart();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            OnStop();
        }

        private void DataCombo_OnSelectionChanged(object sender, EventArgs e)
        {
            _isRunning = false;

            OnStart();
        }

        private void OnStart()
        {
            if (!IsLoaded || _isRunning) return;

            _isRunning = true;

            int countU, countV;
            switch (DataCombo.SelectedIndex)
            {
                case 0:
                    countU = countV = 10;
                    break;
                case 1:
                    countU = countV = 50;
                    break;
                case 2:
                    countU = countV = 100;
                    break;
                case 3:
                    countU = countV = 500;
                    break;
                case 4:
                    countU = countV = 1000;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            lock (_syncRoot)
            {
                _timer?.Stop();
            }

            BitmapImage bitmapImage = new BitmapImage();

            // Load image from resources
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnDemand;
            bitmapImage.CreateOptions = BitmapCreateOptions.DelayCreation;
            bitmapImage.DecodePixelWidth = countU;
            bitmapImage.DecodePixelHeight = countV;
            bitmapImage.UriSource = new Uri("pack://application:,,,/SciChart.Examples.ExternalDependencies;component/Resources/Images/globe_heightmap.png");
            bitmapImage.EndInit();

            // Creating Geo height (displacement) map
            var geoHeightMap = new double[countU, countV];
            int nStride = (bitmapImage.PixelWidth * bitmapImage.Format.BitsPerPixel + 7) / 8;
            int bytsPerPixel = bitmapImage.Format.BitsPerPixel / 8;
            byte[] pixelByteArray = new byte[bitmapImage.PixelWidth * nStride];
            bitmapImage.CopyPixels(pixelByteArray, nStride, 0);
            for (int v = 0; v < countV; v++)
            {
                for (var u = 0; u < countU; u++)
                {
                    int pixelIndex = v * nStride + u * bytsPerPixel;
                    var offset = pixelByteArray[pixelIndex] / 255.0f;
                    geoHeightMap[v, u] = offset;
                }
            }

            var dataSeries = new EllipsoidDataSeries3D<double>(countU, countV)
            {
                SeriesName = "Geo Mesh",
                A = 6,
                B = 6,
                C = 6
            };

            var frontBuffer = dataSeries.InternalArray;
            var backBuffer = new GridData<double>(countU, countV).InternalArray;

            int frames = 0;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Tick += (s, arg) =>
            {
                lock (_syncRoot)
                {
                    double heightOffsetsScale = sliderHeightOffsetsScale.Value;
                    double freq = (Math.Sin(frames++ * 0.1) + 1.0) / 2.0;

                    // Each set of geoHeightMap[i,j] schedules a redraw when the next Render event fires. Therefore, we suspend updates so that we can update the chart once
                    // We parallelize it by using Parallel.For for the outer loop
                    //  Equivalent of "for (int j = 0; j < countU; j++)"
                    // This will result in more CPU usage, but we wish to demo the performance of the actual rendering, not the slowness of generating test data! :)
                    Parallel.For(0, countV, i =>
                    {
                        var buf = frontBuffer;
                        for (int j = 0; j < countU; j++)
                        {
                            // Rotate (offset) J index
                            int rj = j + frames;
                            if (rj >= countU)
                            {
                                rj -= countU * (rj / countU);
                            }

                            buf[i][j] = geoHeightMap[i, rj] + Math.Pow(geoHeightMap[i, rj], freq * 10.0) * heightOffsetsScale;
                        }
                    });

                    using (dataSeries.SuspendUpdates(false, true))
                    {
                        dataSeries.CopyFrom(frontBuffer);
                        var temp = backBuffer;
                        backBuffer = frontBuffer;
                        frontBuffer = temp;
                    }
                }
            };

            SurfaceMesh.DataSeries = dataSeries;
            _timer.Start();

            StartButton.IsChecked = true;
            PauseButton.IsChecked = false;
        }

        private void OnStop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _timer?.Stop();
                
                StartButton.IsChecked = false;
                PauseButton.IsChecked = true;
            }
        }
    }
}