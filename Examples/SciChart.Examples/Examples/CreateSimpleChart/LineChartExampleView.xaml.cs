// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LineChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
   public partial class LineChartExampleView : UserControl
    {
        public LineChartExampleView()
        {
            InitializeComponent();
        }

        private void LineChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {            
            // Create a DataSeries of type X=double, Y=double
            var dataSeries = new XyDataSeries<double, double>();

            lineRenderSeries.DataSeries = dataSeries;

            var data = DataManager.Instance.GetFourierSeries(1.0, 0.1);

            // Append data to series. SciChart automatically redraws
            dataSeries.Append(data.XData, data.YData);
            
            sciChart.ZoomExtents();
        }
    }
}
