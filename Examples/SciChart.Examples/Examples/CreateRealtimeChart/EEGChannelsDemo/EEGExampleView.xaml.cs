// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// EEGExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using SciChart.Drawing.Common;
using SciChart.Examples.ExternalDependencies.Data;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SciChart.Examples.Examples.CreateRealtimeChart.EEGChannelsDemo
{
    /// <summary>
    /// Interaction logic for EEGExampleView.xaml
    /// </summary>
    public partial class EEGExampleView : UserControl
    {
        private Stopwatch _stopWatch;
        private MovingAverage _fpsAverage;
        private int _frameCount;

        public EEGExampleView()
        {
            InitializeComponent();

            _fpsAverage = new MovingAverage(5);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            RenderSurfaceBase.UseThreadedRenderTimer = false;

            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // Synchronizes rendering across all SciChartSurfaces
            RenderSurfaceBase.UseThreadedRenderTimer = true;

            _stopWatch = Stopwatch.StartNew();
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        /// <summary>
        /// Purely for stats reporting (FPS). Not needed for SciChart rendering
        /// </summary>
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (StartButton.IsChecked != true)
            {
                fpsCounter.Text = "-";
                return;
            }

            if (_stopWatch.ElapsedMilliseconds >= 1000)
            {
                // Push the fps to the moving average, we want to average the FPS to get a more reliable reading
                _fpsAverage.Push(_frameCount);

                // Render the fps to the screen
                var text = double.IsNaN(_fpsAverage.Current) ? "-" : $"{_fpsAverage.Current:0}";
                Dispatcher.BeginInvoke(() => fpsCounter.Text = text, DispatcherPriority.Render);

                _frameCount = 0;
                _stopWatch.Restart();
            }

            _frameCount++;
        }
    }
}
