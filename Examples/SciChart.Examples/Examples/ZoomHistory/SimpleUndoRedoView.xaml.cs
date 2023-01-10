﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SimpleUndoRedoView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.HistoryManagers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ZoomHistory
{
    public partial class SimpleUndoRedoView : UserControl
    {
        public SimpleUndoRedoView()
        {
            InitializeComponent();
        }

        private void LineChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            SciChart.ZoomHistoryManager = new ZoomHistoryManager();

            // Create a DataSeries of type X=double, Y=double
            var dataSeries = new UniformXyDataSeries<double>(0d, 0.002);
            LineRenderSeries.DataSeries = dataSeries;

            // Append data to series. SciChart automatically redraws
            var data = DataManager.Instance.GetFourierYData(1.0, 0.1);
            dataSeries.Append(data);

            SciChart.ZoomExtents();
        }
    }
}