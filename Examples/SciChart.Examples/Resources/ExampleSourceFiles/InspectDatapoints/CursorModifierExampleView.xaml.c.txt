// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CursorModifierExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.InspectDatapoints
{
    /// <summary>
    /// Interaction logic for CursorModifierExampleView.xaml
    /// </summary>
    public partial class CursorModifierExampleView : UserControl
    {
        public CursorModifierExampleView()
        {
            InitializeComponent();

            var dataSeries0 = new XyDataSeries<double, double>() { SeriesName = "Green Series" };
            var dataSeries1 = new XyDataSeries<double, double>() { SeriesName = "Red Series" };
            var dataSeries2 = new XyDataSeries<double, double>() { SeriesName = "Gray Series" };
            var dataSeries3 = new XyDataSeries<double, double>() { SeriesName = "Yellow Series" };

            var data1 = DataManager.Instance.GetNoisySinewave(300, 1.0, 300, 0.25);
            var data2 = DataManager.Instance.GetSinewave(100, 2, 300);
            var data3 = DataManager.Instance.GetSinewave(200, 1.5, 300);
            var data4 = DataManager.Instance.GetSinewave(50, 0.1, 300);

            // Append data to series.
            dataSeries0.Append(data1.XData, data1.YData);
            dataSeries1.Append(data2.XData, data2.YData);
            dataSeries2.Append(data3.XData, data3.YData);
            dataSeries3.Append(data4.XData, data4.YData);

            // Assign DataSeries to RenderableSeries
            sciChartSurface.RenderableSeries[0].DataSeries = dataSeries0;
            sciChartSurface.RenderableSeries[1].DataSeries = dataSeries1;
            sciChartSurface.RenderableSeries[2].DataSeries = dataSeries2;
            sciChartSurface.RenderableSeries[3].DataSeries = dataSeries3;

            sciChartSurface.XAxis.VisibleRange = new DoubleRange(3, 6);
        }
    }
}
