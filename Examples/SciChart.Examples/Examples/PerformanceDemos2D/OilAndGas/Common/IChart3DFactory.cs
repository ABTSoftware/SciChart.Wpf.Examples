// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// IChart3DFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Threading.Tasks;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common
{
    public interface IChart3DFactory
    {
        string Title { get; }

        IAxis3DViewModel GetXAxis();

        IAxis3DViewModel GetYAxis();

        IAxis3DViewModel GetZAxis();

        Task<IEnumerable<IRenderableSeries3DViewModel>> GetSeriesAsync();
    }
}