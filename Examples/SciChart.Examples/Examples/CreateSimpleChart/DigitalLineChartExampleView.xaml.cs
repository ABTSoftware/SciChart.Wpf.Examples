// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DigitalLineChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    public partial class DigitalLineChartExampleView : UserControl
    {
        public DigitalLineChartExampleView()
        {
            InitializeComponent();
        }

        private void DigitalLineChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Create a dataseries of type X=double, Y=double
            var dataSeries = new UniformXyDataSeries<double>(0d, 0.002);

            // Append data to series. SciChart automatically redraws
            dataSeries.Append(DataManager.Instance.GetFourierYData(1.0, 0.1));

            lineRenderSeries.DataSeries = dataSeries;

            // We set visible ranges only to zoom in to series to show Digital Line
            sciChart.XAxis.VisibleRange = new DoubleRange(1, 1.25);
            sciChart.YAxis.VisibleRange = new DoubleRange(2.3, 3.3);
        }
    }
}