// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ScatterChartPerformanceTest.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting.Model.DataSeries;
using SciChart.Core.Utility;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.PerformanceDemos2D.ScatterPerf
{
    /// <summary>
    /// Interaction logic for ScatterChartPerformanceTest.xaml
    /// </summary>
    public partial class ScatterChartPerformanceTest : UserControl
    {
        public ScatterChartPerformanceTest()
        {
            InitializeComponent();
            
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // Create our data in another thread to stop the UI from being stalled. 
            // It's not appending to SciChart data series that is the problem, its Calling Rand.Next() two million times which is pretty slow :)
            TimedMethod.Invoke(() =>
            {
                var dataSeries = new XyDataSeries<double, double>() {AcceptsUnsortedData = true};
                var rand = new Random();

                // Allow 1M points in WPF/DirectX. In Silverlight or software rendering, 100k points is enough to stress the renderer
                int count = FeaturesHelper.Instance.SupportsHardwareAcceleration ? (int) 1E6 : (int) 1E5;

                // Append some data 
                for (int i = 0; i < count; i++)
                {
                    dataSeries.Append(rand.NextDouble(), rand.NextDouble());
                }

                // Bind to scichart 
                Action bindData = () => { BindData(dataSeries); };
                Dispatcher.BeginInvoke(bindData);

            }).After(200).OnThread(TimedMethodThread.Background).Go();
        }

        private void BindData(XyDataSeries<double, double> dataSeries)
        {
            // Must be called on UI thread, as DataSeries is a dependency property
            // Note in MVVM you can bind RenderableSeries.DataSeries to a property in VM and update in any thread.
            using (sciChart.SuspendUpdates())
            {
                foreach (var rSeries in sciChart.RenderableSeries)
                {
                    rSeries.DataSeries = dataSeries;
                }                
            }

            sciChart.ZoomExtents();
        }
    }
}