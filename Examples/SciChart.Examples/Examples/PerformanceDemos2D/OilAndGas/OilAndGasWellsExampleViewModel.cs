// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// OilAndGasWellsExampleViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.GridCharts;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.ThreeDCharts;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.VerticalCharts;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas
{
    public class OilAndGasWellsExampleViewModel : BaseViewModel
    {
        public OilAndGasWellsExampleViewModel()
        {
            GridPanelViewModel = new GridPanelViewModel();
            Chart3DPanelViewModel = new Chart3DPanelViewModel();
            VerticalPanelViewModel = new VerticalPanelViewModel();
        }

        public GridPanelViewModel GridPanelViewModel { get; }
        public Chart3DPanelViewModel Chart3DPanelViewModel { get; }
        public VerticalPanelViewModel VerticalPanelViewModel { get; }
    }
}