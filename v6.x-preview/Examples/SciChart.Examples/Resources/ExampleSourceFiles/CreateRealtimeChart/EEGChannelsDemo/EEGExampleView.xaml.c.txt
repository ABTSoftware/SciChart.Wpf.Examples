// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
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
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateRealtimeChart.EEGChannelsDemo
{
    /// <summary>
    /// Interaction logic for EEGExampleView.xaml
    /// </summary>
    public partial class EEGExampleView : UserControl
    {
        private Stopwatch _stopWatch;
        private double _lastFrameTime;
        private MovingAverage _fpsAverage;

        public EEGExampleView()
        {
            InitializeComponent();

            _stopWatch = Stopwatch.StartNew();
            _fpsAverage = new MovingAverage(5);         
   
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;            
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;         
            CompositionTarget.Rendering += CompositionTarget_Rendering;            
        }


        /// <summary>
        /// Purely for stats reporting (FPS). Not needed for SciChart rendering
        /// </summary>
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (StopButton.IsChecked != true && ResetButton.IsChecked != true)
            {
                // Compute the render time
                double frameTime = _stopWatch.ElapsedMilliseconds;
                double delta = frameTime - _lastFrameTime;
                double fps = 1000.0 / delta;

                // Push the fps to the movingaverage, we want to average the FPS to get a more reliable reading
                _fpsAverage.Push(fps);

                // Render the fps to the screen
                fpsCounter.Text = double.IsNaN(_fpsAverage.Current) ? "-" : string.Format("{0:0}", _fpsAverage.Current);

                // Render the total point count (all series) to the screen
                var eegExampleViewModel = (DataContext as EEGExampleViewModel);
                pointCount.Text = eegExampleViewModel != null ? eegExampleViewModel.PointCount.ToString() : "Na";

                _lastFrameTime = frameTime;
            }
        }
    }
}
