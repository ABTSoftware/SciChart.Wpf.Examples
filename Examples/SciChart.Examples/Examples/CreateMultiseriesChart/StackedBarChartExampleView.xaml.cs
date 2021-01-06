// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// StackedBarChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    /// <summary>
    /// Interaction logic for StackedBarChartExampleView.xaml
    /// </summary>
    public partial class StackedBarChartExampleView
    {
        private bool _firstTimeRendered;

        public StackedBarChartExampleView()
        {
            InitializeComponent();

            sciChart.Rendered += (o, args) =>
            {
                if (!_firstTimeRendered)
                {
                    _firstTimeRendered = true;
                    sciChart.ZoomExtents();
                }
            };

            var yValues1 = new[] { 0.0, 0.1, 0.2, 0.4, 0.8, 1.1, 1.5, 2.4, 4.6, 8.1, 11.7, 14.4, 16.0, 13.7, 10.1, 6.4, 3.5, 2.5, 5.4, 6.4, 7.1, 8.0, 9.0 };
            var yValues2 = new[] { 2.0, 10.1, 10.2, 10.4, 10.8, 1.1, 11.5, 3.4, 4.6, 0.1, 1.7, 14.4, 16.0, 13.7, 10.1, 6.4, 3.5, 2.5, 1.4, 0.4, 10.1, 0.0, 0.0 };
            var yValues3 = new[] { 20.0, 4.1, 4.2, 10.4, 10.8, 1.1, 11.5, 3.4, 4.6, 5.1, 5.7, 14.4, 16.0, 13.7, 10.1, 6.4, 3.5, 2.5, 1.4, 10.4, 8.1, 10.0, 15.0 };

            var dataSeries1 = new XyDataSeries<double, double> { SeriesName = "data1" };
            var dataSeries2 = new XyDataSeries<double, double> { SeriesName = "data2" };
            var dataSeries3 = new XyDataSeries<double, double> { SeriesName = "data3" };

            for (int i = 0; i < yValues1.Length; i++) dataSeries1.Append(i, yValues1[i]);
            for (int i = 0; i < yValues2.Length; i++) dataSeries2.Append(i, yValues2[i]);
            for (int i = 0; i < yValues3.Length; i++) dataSeries3.Append(i, yValues3[i]);

            using (sciChart.SuspendUpdates())
            {
                columnSeries1.DataSeries = dataSeries1;
                columnSeries2.DataSeries = dataSeries2;
                columnSeries3.DataSeries = dataSeries3;
            }
        }

        private void CboXAxisAlignment_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (numericXAxis != null)
            {
                numericXAxis.AxisAlignment = (AxisAlignment)cboXAxisAlignment.SelectedValue;
            }
        }

        private void CboYAxisAlignment_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (numericXAxis != null)
            {
                numericYAxis.AxisAlignment = (AxisAlignment)cboYAxisAlignment.SelectedValue;
            }
        }
    }
}
