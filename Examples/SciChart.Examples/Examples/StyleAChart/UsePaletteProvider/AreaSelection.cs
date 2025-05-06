// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AreaSelection.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using SciChart.Charting.Visuals;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.StyleAChart.UsePaletteProvider
{
    public class AreaSelection
    {
        private double _x1, _x2;

        public ISciChartSurface Surface { get; set; }

        public void SetSelectionRect(double x1, double width)
        {
            var xCalc = Surface.XAxis?.GetCurrentCoordinateCalculator();

            if (xCalc != null)
            {
                _x1 = xCalc.GetDataValue(x1);
                _x2 = xCalc.GetDataValue(x1 + width);
            }
        }

        /// <summary>
        /// Checks whether current <see cref="AreaSelection"/> contains a point with x coordinate
        /// </summary>
        public bool Contains(IComparable xValue)
        {
            var x = xValue.ToDouble();

            return x.CompareTo(_x1) >= 0 && x.CompareTo(_x2) <= 0;
        }
    }
}