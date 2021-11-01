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
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
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
            var dataSeries = new UniformXyDataSeries<double>(0d, 0.002);

            lineRenderSeries.DataSeries = dataSeries;

            // Append data to series. SciChart automatically redraws
            dataSeries.Append(DataManager.Instance.GetFourierYData(1.0, 0.1));
            
            sciChart.ZoomExtents();
        }
    }
}
