﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SynchronizeMouseAcrossCharts.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.ZoomAndPanAChart
{
    public partial class SynchronizeMouseAcrossCharts : UserControl
    {
        public SynchronizeMouseAcrossCharts()
        {
            InitializeComponent();

            this.Loaded += MultiChartMouseEvents_Loaded;
        }

        void MultiChartMouseEvents_Loaded(object sender, RoutedEventArgs e)
        {
            chart0.AnimateZoomExtents(TimeSpan.FromMilliseconds(1000));
            chart1.AnimateZoomExtents(TimeSpan.FromMilliseconds(1000));
        }

        private void ZoomExtentsClick(object sender, EventArgs e)
        {
            chart0.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
            chart1.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
        }
    }
}
