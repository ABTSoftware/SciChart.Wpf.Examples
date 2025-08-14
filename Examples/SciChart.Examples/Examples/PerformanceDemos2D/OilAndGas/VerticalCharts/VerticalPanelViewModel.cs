// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// VerticalPanelViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.VerticalCharts.ChartFactory;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.VerticalCharts
{
    public class VerticalPanelViewModel : BindableObject
    {
        private DoubleRange _sharedXRange;

        public VerticalPanelViewModel()
        {
            VerticalCharts = new List<ChartViewModel>
            {
                new ChartViewModel(new ShaleChartFactory()),
                new ChartViewModel(new DensityChartFactory()),
                new ChartViewModel(new ResistivityChartFactory()),
                new ChartViewModel(new PoreSpaceChartFactory()),
                new ChartViewModel(new SonicChartFactory()),
                new ChartViewModel(new TextureChartFactory())
            };
        }

        public DoubleRange SharedXRange
        {
            get => _sharedXRange;
            set
            {
                if (_sharedXRange != value)
                {
                    _sharedXRange = value;
                    OnPropertyChanged(nameof(SharedXRange));
                }
            }
        }

        public int RowsCount => 1;

        public int ColumnsCount => VerticalCharts.Count;

        public IList<ChartViewModel> VerticalCharts { get; }
    }
}