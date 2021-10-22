// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// PolarChartViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Linq;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.DashboardStylePolarCharts
{
    public class PolarChartViewModel : BaseViewModel
    {
        public List<IRenderableSeriesViewModel> RenderableSeriesViewModel { get; private set; }

        public string Title { get; private set; }

        public PolarChartViewModel(params IRenderableSeriesViewModel[] series)
        {
            RenderableSeriesViewModel = new List<IRenderableSeriesViewModel>(series);
            
            Title = series.First().GetType().Name;
        }
    }
}