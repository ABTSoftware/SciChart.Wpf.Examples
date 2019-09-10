// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ShiftedAxes.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour.ShiftedAxes
{
    public partial class ShiftedAxes
    {
        public ShiftedAxes()
        {
            InitializeComponent();
        }

        private void ShiftedAxes_OnLoaded(object sender, RoutedEventArgs e)
        {
            using (sciChart.SuspendUpdates())
            {
                var dataSeries = new XyDataSeries<double, double> {AcceptsUnsortedData = true};
                lineRenderSeries.DataSeries = dataSeries;

                var data = DataManager.Instance.GetButterflyCurve(20000);
                dataSeries.Append(data.XData, data.YData);
            }
            
            // To provide correct appearance of chart in Silverlight
            // we use Dispatcher to delay ZoomExtents call on chart
            Dispatcher.BeginInvoke(new Action(() => sciChart.ZoomExtents()));
        }
    }
}