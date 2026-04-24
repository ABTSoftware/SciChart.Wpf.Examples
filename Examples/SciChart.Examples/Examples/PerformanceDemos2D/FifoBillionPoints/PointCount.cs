// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// PointCount.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class PointCount : BindableObject
    {
        public string DisplayName { get; }
        public int SeriesCount { get; }
        public int PointsCount { get; }

        public PointCount(string displayName, int seriesCount, int pointsCount)
        {
            DisplayName = displayName;
            SeriesCount = seriesCount;
            PointsCount = pointsCount;
        }
    }
}