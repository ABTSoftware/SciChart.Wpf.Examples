using System.Collections.Generic;
using System.Windows;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public class EuropeCountryFeature
    {
        public string Name { get; set; }

        /// <summary>ISO-2 country code (e.g. "DE"), used to join city records to their country.</summary>
        public string Id { get; set; }

        public long Population { get; set; }
        public double CentroidLon { get; set; }
        public double CentroidLat { get; set; }

        public string Capital { get; set; }
        public double CapitalLat { get; set; }
        public double CapitalLon { get; set; }
        public long CapitalPopulation { get; set; }

        /// <summary>All outer polygon rings (one per part for MultiPolygon countries).</summary>
        public List<Point[]> PolygonRings { get; set; } = new List<Point[]>();
    }
}
