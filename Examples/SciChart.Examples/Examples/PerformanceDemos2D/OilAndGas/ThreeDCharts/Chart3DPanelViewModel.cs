// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Chart3DPanelViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using SciChart.Data.Model;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.ThreeDCharts.ChartFactory;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.ThreeDCharts
{
    public class Chart3DPanelViewModel : BindableObject
    {
        public Chart3DPanelViewModel()
        {
            Chart = new Chart3DViewModel(new ScatterChart3DFactory());
        }

        public Chart3DViewModel Chart { get; }
    }
}