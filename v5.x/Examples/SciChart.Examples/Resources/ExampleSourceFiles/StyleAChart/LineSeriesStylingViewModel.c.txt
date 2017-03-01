// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LineSeriesStylingViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Linq;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.StyleAChart
{
    public class LineSeriesStylingViewModel : BaseViewModel
    {
        public LineSeriesStylingViewModel()
        {
            // Generate some data to display 
            var data = DataManager.Instance.GetFourierSeriesZoomed(1.0, 0.1, 5.0, 5.15);

            // Append data to series. SciChart automatically redraws
            Series0 = new XyDataSeries<double, double>();
            Series1 = new XyDataSeries<double, double>();
            Series2 = new XyDataSeries<double, double>();

            Series0.Append(data.XData, data.YData);
            Series1.Append(data.XData, data.YData.Select(x => x * 1.1));
            Series2.Append(data.XData, data.YData.Select(x => x * 1.5));
        }

        // Note: No need to implement INotifyPropertyChanged here since we're not updating the series instances
        public IXyDataSeries<double, double> Series0 { get; private set; }
        public IXyDataSeries<double, double> Series1 { get; private set; }
        public IXyDataSeries<double, double> Series2 { get; private set; }
    }
}
