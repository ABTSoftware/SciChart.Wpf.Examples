// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ImpulseChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    public partial class ImpulseChartExampleView : UserControl
    {
        public ImpulseChartExampleView()
        {
            InitializeComponent();
        }

        private void ImpulseChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataSeries = new XyDataSeries<double, double>();

            // Substitute for your own data
            var dummyData = DataManager.Instance.GetDampedSinewave(1.0, 0.05, 100);
            dataSeries.Append(dummyData.XData, dummyData.YData);

            impulseRenderSeries.DataSeries = dataSeries;

            // Explicit call to ZoomExtents()
            sciChart.ZoomExtents();
        }
    }
}
