// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ScatterChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using System.Linq;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace DpiAware_SciChartSurface
{
    /// <summary>
    /// Interaction logic for ScatterChartExampleView.xaml
    /// </summary>
    public partial class ScatterChartExampleView : UserControl
    {
        public ScatterChartExampleView()
        {
            InitializeComponent();
        }

        private void ScatterChartExampleView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Create some xValues, yValues arrays
            var random = new Random(0);
            
            var xValues = DataManager.Instance.GetRangeD(0, 250);
            var yValues = xValues.Select(x => 3 * x + x * random.NextDouble()).ToArray();
            var y1Values = xValues.Select(x => x + (x * random.NextDouble())).ToArray();

            // Create a DataSeries with the data for the chart

            scatterRenderSeries0.DataSeries = new XyDataSeries<double>(xValues, yValues);
            scatterRenderSeries1.DataSeries = new XyDataSeries<double>(xValues, y1Values);

            sciChart.ZoomExtents();
        }
    }
}
