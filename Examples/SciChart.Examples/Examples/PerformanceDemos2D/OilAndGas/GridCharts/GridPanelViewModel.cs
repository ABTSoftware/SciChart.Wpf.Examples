// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// GridPanelViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using SciChart.Data.Model;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.GridCharts.ChartFactory;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.GridCharts
{
    public class GridPanelViewModel : BindableObject
    {
        public GridPanelViewModel()
        {
            GridCharts = new List<ChartViewModel>
            {
                new ChartViewModel(new MountainChartFactory("Grid-0")),
                new ChartViewModel(new ScatterChartFactory("Grid-1")),
                new ChartViewModel(new ScatterChartFactory("Grid-2")),

                new ChartViewModel(new ScatterChartFactory("Grid-3")),
                new ChartViewModel(new MountainChartFactory("Grid-4")),
                new ChartViewModel(new ScatterChartFactory("Grid-5")),

                new ChartViewModel(new ScatterChartFactory("Grid-6")),
                new ChartViewModel(new ScatterChartFactory("Grid-7")),
                new ChartViewModel(new MountainChartFactory("Grid-8"))
            };
        }

        public int RowsCount => GridCharts.Count / 3;

        public int ColumnsCount => GridCharts.Count / 3;

        public IList<ChartViewModel> GridCharts { get; }
    }
}