// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// RealTimeStaticAxis.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Core.Helpers;
using SciChart.Examples.ExternalDependencies.Helpers;

namespace SciChart.Examples.Examples.CreateRealtimeChart.RealTimeStaticAxis
{
    /// <summary>
    /// Interaction logic for RealTimeStaticAxis.xaml
    /// </summary>
    public partial class RealTimeStaticAxis : UserControl
    {
        private DispatcherTimer _timer;

        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        private readonly Random _random = new Random();

        private int _annotationCoord;
        private int _dataPoint1, _dataPoint2;

        private const int FifoCapacity = 100;
        private const int Interval = 100;

        public RealTimeStaticAxis()
        {
            InitializeComponent();

            mountainRenderableSeries1.DataSeries = new XyDataSeries<double, double>
            {
                FifoCapacity = FifoCapacity,
                SeriesName = "Mountain Series (1)"
            };

            mountainRenderableSeries2.DataSeries = new XyDataSeries<double, double>
            {
                FifoCapacity = FifoCapacity,
                SeriesName = "Mountain Series (2)"
            };
        }

        private int GenerateNextDataPoint(int preveiousValue)
        {
            var newValue = _random.Next(preveiousValue - 2, preveiousValue + 3);

            newValue = Math.Min(20, newValue);
            newValue = Math.Max(-20, newValue);

            return newValue;
        }

        private void TimerOnElapsed(object sender, EventArgs e)
        {
            _dataPoint1 = GenerateNextDataPoint(_dataPoint1);
            _dataPoint2 = GenerateNextDataPoint(_dataPoint2);

            ((XyDataSeries<double, double>)mountainRenderableSeries1.DataSeries).Append(_annotationCoord, _dataPoint1);

            ((XyDataSeries<double, double>)mountainRenderableSeries2.DataSeries).Append(_annotationCoord, _dataPoint2);

            UpdateLineAnnotations();

            _annotationCoord ++;
        }

        private void UpdateLineAnnotations()
        {
            var updatePos = _annotationCoord != 0 && _annotationCoord % 20 == 0;
            foreach (var annotation in sciChart.Annotations.OfType<VerticalLineAnnotation>())
            {
                // update position of annotation to show it again if annotation is ouside of view port
                if (xAxis.VisibleRange != null && updatePos && annotation.X1.CompareTo(xAxis.VisibleRange.Min) < 0)
                {
                    annotation.X1 = (double)_annotationCoord;
                    updatePos = false;
                }
                UpdateLabel(annotation);
            }
        }

        private void UpdateLabel(VerticalLineAnnotation annotation)
        {
            var expr = annotation.GetBindingExpression(LineAnnotationWithLabelsBase.LabelValueProperty);

            if (expr != null)
            {
                var binding = expr.ParentBinding;
                annotation.SetBinding(LineAnnotationWithLabelsBase.LabelValueProperty, binding);
            }
        }

        private void OnExampleLoaded(object sender, RoutedEventArgs e)
        {
            // Preload with data
            for (int i = 0; i < FifoCapacity; i++)
            {
                TimerOnElapsed(null, EventArgs.Empty);
            }

            if (_timer == null)
            {
                _timer = new DispatcherTimer(DispatcherPriority.Background);
                _timer.Interval = TimeSpan.FromMilliseconds(Interval);
                _timer.Tick += TimerOnElapsed;
                _timer.Start();
            }
        }

        private void OnExampleUnloaded(object sender, RoutedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= TimerOnElapsed;
                _timer = null;
            }
        }
    }
}
