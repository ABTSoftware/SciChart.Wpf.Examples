// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// VerticallyStackedAxes.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    /// <summary>
    /// Interaction logic for VerticallyStackedAxes.xaml
    /// </summary>
    public partial class VerticallyStackedAxes : UserControl
    {
        public VerticallyStackedAxes()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                // Creates 8 dataseries with data on a background thread
                var dataSeries = new List<IDataSeries>();
                for (int i = 0; i < 8; i++)
                {
                    var ds = new XyDataSeries<double, double>();
                    dataSeries.Add(ds);

                    var someData = DataManager.Instance.GetAcousticChannel(i);
                    ds.Append(someData.XData, someData.YData);
                }

                // Creates 8 renderable series on the UI thread
                Dispatcher.BeginInvoke(new Action(() => CreateRenderableSeries(dataSeries)));
            });
        }

        private void CreateRenderableSeries(List<IDataSeries> result)
        {
            // Batch updates with one redraw
            using (sciChart.SuspendUpdates())
            {
                for (int i = 0; i < 8; i++)
                {
                    sciChart.RenderableSeries[i].DataSeries = result[i];
                }
            }
        }
    }
}