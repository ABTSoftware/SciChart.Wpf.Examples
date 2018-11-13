// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DragAreaToZoom.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ZoomAndPanAChart
{
    public partial class DragAreaToZoom : UserControl
    {
        public DragAreaToZoom()
        {
            InitializeComponent();
        }

        private void DragAreaToZoom_Loaded(object sender, RoutedEventArgs e)
        {
            // Performing multiple updates in a SuspendUpdates block is efficient as only one redraw is performed
            using (sciChart.SuspendUpdates())
            {
                // Add some data series of type X=double, Y=double
                var dataSeries0 = new XyDataSeries<double, double>();
                var dataSeries1 = new XyDataSeries<double, double>();

                var data2 = DataManager.Instance.GetFourierSeries(1.0, 0.1);
                var data1 = DataManager.Instance.GetDampedSinewave(1500, 3.0, 0.0, 0.005, data2.Count);

                // Append data to series.
                dataSeries0.Append(data1.XData, data1.YData);
                dataSeries1.Append(data2.XData, data2.YData);

                lineSeries.DataSeries = dataSeries1;
                mountainSeries.DataSeries = dataSeries0;

                // Set initial zoom
                sciChart.ZoomExtents();
            }
        }
    }
}
