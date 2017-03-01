// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// PolarChartWithMultipleAxes.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    /// <summary>
    /// Interaction logic for PolarChartWithMultipleAxes.xaml
    /// </summary>
    public partial class PolarChartWithMultipleAxes : UserControl
    {
        public PolarChartWithMultipleAxes()
        {
            InitializeComponent();
        }

        private void PolarChartWithMultipleAxes_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataSeries1 = new XyDataSeries<double, double>();
            var dataSeries2 = new XyDataSeries<double, double>();
            var dataSeries3 = new XyDataSeries<double, double>();

            var data1 = DataManager.Instance.GetSquirlyWave();
            var data2 = DataManager.Instance.GetStraightLine(1, 1, 200);
            var data3 = DataManager.Instance.GetRandomDoubleSeries(25);

            dataSeries1.Append(data1.XData, data1.YData);
            dataSeries2.Append(data2.XData, data2.YData);
            dataSeries3.Append(data3.XData, data3.YData);

            
            lineSeries.DataSeries = dataSeries1;
            mountainSeries.DataSeries = dataSeries2;
            scatterSeries.DataSeries = dataSeries3;

            sciChart.ZoomExtents();
        } 
    }
}
