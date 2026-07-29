// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CityLegendItem.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
namespace SciChart.Examples.Examples.SeeFeaturedApplication.PopulationMap
{
    /// <summary>
    /// One row of the graduated-circle city size legend: an ellipse of <see cref="Diameter"/> pixels
    /// (matching the on-map bubble size for a representative population) beside its <see cref="Label"/>.
    /// </summary>
    public class CityLegendItem
    {
        public double Diameter { get; set; }
        public string Label { get; set; }
    }
}
