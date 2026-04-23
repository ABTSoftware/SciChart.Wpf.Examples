// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// PopulationData.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
namespace SciChart.Examples.ExternalDependencies.Data
{
    public enum ContinentsEnum
    {
        Asia,
        Europe,
        Africa,
        Americas,
        Oceania,
    }

    public class PopulationData
    {
        public string Country { get; set; }
        public int Year { get; set; }
        public int Population { get; set; }
        public ContinentsEnum Continent { get; set; }
        public double LifeExpectancy { get; set; }
        public double GDPPerCapita { get; set; }
    }
}
