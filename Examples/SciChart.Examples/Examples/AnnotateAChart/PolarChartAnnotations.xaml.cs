// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// PolarChartAnnotations.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.AnnotateAChart
{
    /// <summary>
    /// Interaction logic for PolarChartAnnotations.xaml
    /// </summary>
    public partial class PolarChartAnnotations : UserControl
    {
        public PolarChartAnnotations()
        {
            InitializeComponent();
        }

        private void PolarChartAnnotations_OnLoaded(object sender, RoutedEventArgs e)
        {
            var data = DataManager.Instance.GetStraightLine(0.5, -100, 360);
           
            var dataSeries = new XyDataSeries<double, double>();

            dataSeries.Append(data.XData, data.YData);

            lineSeries1.DataSeries = dataSeries;
        }
    }
}
