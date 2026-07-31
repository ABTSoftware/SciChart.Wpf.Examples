// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CountryCentroid.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
namespace SciChart.Examples.Examples.SeeFeaturedApplication.PopulationMap
{
    public class CountryCentroid
    {
        public string Name { get; }
        public double Lon { get; }
        public double Lat { get; }
        public long Population { get; }

        public string PopulationLabel => Population >= 1_000_000 ? $"{Population / 1_000_000.0:F1} M" : Population >= 1_000 ? $"{Population / 1_000.0:F0} k" : Population.ToString();

        public CountryCentroid(string name, double lon, double lat, long population)
        {
            Name = name;
            Lon = lon;
            Lat = lat;
            Population = population;
        }
    }
}
