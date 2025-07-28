// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// IChartFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Threading.Tasks;
using SciChart.Charting.Model.ChartSeries;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common
{
    public interface IChartFactory
    {
        string Title { get; }

        IAxisViewModel GetXAxis();

        IAxisViewModel GetYAxis();

        Task<IEnumerable<IRenderableSeriesViewModel>> GetSeriesAsync();
    }
}