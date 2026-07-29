// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// DataManager.PopulationMap.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public partial class DataManager
    {
        /// <summary>
        /// Loads and parses geolocation data from an embedded JSON resource file.
        /// </summary>
        /// <returns>
        /// Returns a list of parsed <see cref="EuropeCountryFeature"/> instances.
        /// </returns>
        public List<EuropeCountryFeature> GetEuropeMapData()
        {
            var result = new List<EuropeCountryFeature>();

            var asm = typeof(DataManager).Assembly;
            var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains("europe.json.gz"));

            using (var stream = asm.GetManifestResourceStream(resourceString))
            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gz))
            {
                var root = JsonConvert.DeserializeObject<GeoJsonRoot>(reader.ReadToEnd());

                foreach (var feature in root.Features)
                {
                    var geometry = feature.Geometry;
                    if (geometry == null) continue;

                    var name = feature.Properties != null ? feature.Properties.Name ?? "" : "";
                    var id = feature.Properties?.Id ?? "";
                    long population = feature.Properties != null ? feature.Properties.Population : 0;

                    double bestLon = 0, bestLat = 0, bestArea = 0;
                    var rings = new List<Point[]>();

                    if (geometry.Type == "MultiPolygon")
                    {
                        foreach (var polygon in geometry.Coordinates.ToObject<double[][][][]>())
                        {
                            var outer = polygon[0];
                            rings.Add(RingToPoints(outer));

                            double lon, lat, area;
                            RingCentroid(outer, out lon, out lat, out area);
                            if (area > bestArea) { bestArea = area; bestLon = lon; bestLat = lat; }
                        }
                    }
                    else if (geometry.Type == "Polygon")
                    {
                        var outer = geometry.Coordinates.ToObject<double[][][]>()[0];
                        rings.Add(RingToPoints(outer));
                        RingCentroid(outer, out bestLon, out bestLat, out bestArea);
                    }

                    result.Add(new EuropeCountryFeature
                    {
                        Name = name,
                        Id = id,
                        Population = population,
                        Capital           = feature.Properties?.Capital           ?? string.Empty,
                        CapitalLat        = feature.Properties?.CapitalLat        ?? 0,
                        CapitalLon        = feature.Properties?.CapitalLon        ?? 0,
                        CapitalPopulation = feature.Properties?.CapitalPopulation ?? 0,
                        CentroidLon = bestLon,
                        CentroidLat = bestLat,
                        PolygonRings = rings,
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Loads the list of European cities (including capitals) from an embedded JSON resource file.
        /// </summary>
        /// <returns>
        /// Returns a list of parsed <see cref="City"/> instances.
        /// </returns>
        public List<City> GetEuropeCities()
        {
            var asm = typeof(DataManager).Assembly;
            var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains("europe_cities.json.gz"));

            using (var stream = asm.GetManifestResourceStream(resourceString))
            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gz))
            {
                return JsonConvert.DeserializeObject<List<City>>(reader.ReadToEnd()) ?? new List<City>();
            }
        }

        /// <summary>
        /// Projects a WGS84 longitude/latitude (degrees) to Lambert Azimuthal Equal-Area coordinates
        /// (spherical approximation of EPSG:3035, ETRS89-LAEA Europe; centre 10°E / 52°N), in metres.
        /// This is the projection Eurostat/GISCO uses for NUTS maps; it gives Europe its characteristic
        /// fanned, triangular layout. Both the country polygons and the city points should be projected
        /// through this so they stay aligned.
        /// </summary>
        public static Point Wgs84ToLaea(double longitudeDeg, double latitudeDeg)
        {
            const double R = 6371000.0;                      // mean Earth radius, metres
            const double lon0 = 10.0 * Math.PI / 180.0;      // central meridian
            const double lat1 = 52.0 * Math.PI / 180.0;      // latitude of origin

            var lon = longitudeDeg * Math.PI / 180.0;
            var lat = latitudeDeg * Math.PI / 180.0;
            var dLon = lon - lon0;

            var cosLat = Math.Cos(lat);
            var sinLat = Math.Sin(lat);
            var cosLat1 = Math.Cos(lat1);
            var sinLat1 = Math.Sin(lat1);
            var cosDLon = Math.Cos(dLon);

            var k = Math.Sqrt(2.0 / (1.0 + sinLat1 * sinLat + cosLat1 * cosLat * cosDLon));
            var x = R * k * cosLat * Math.Sin(dLon);
            var y = R * k * (cosLat1 * sinLat - sinLat1 * cosLat * cosDLon);

            return new Point(x, y);
        }

        private static Point[] RingToPoints(double[][] ring)
        {
            var pts = new Point[ring.Length];
            for (int i = 0; i < ring.Length; i++)
                pts[i] = new Point(ring[i][0], ring[i][1]);
            return pts;
        }

        private static void RingCentroid(double[][] ring, out double lon, out double lat, out double area)
        {
            double signedArea = 0, cx = 0, cy = 0;
            int n = ring.Length;
            for (int i = 0; i < n - 1; i++)
            {
                double x0 = ring[i][0], y0 = ring[i][1];
                double x1 = ring[i + 1][0], y1 = ring[i + 1][1];
                double cross = x0 * y1 - x1 * y0;
                signedArea += cross;
                cx += (x0 + x1) * cross;
                cy += (y0 + y1) * cross;
            }
            signedArea *= 0.5;
            double f = 1.0 / (6.0 * signedArea);
            lon  = cx * f;
            lat  = cy * f;
            area = Math.Abs(signedArea);
        }

        // GeoJSON deserialization models — private to this file
        private class GeoJsonRoot
        {
            [JsonProperty("features")] public List<GeoJsonFeature> Features { get; set; }
        }

        private class GeoJsonFeature
        {
            [JsonProperty("geometry")]   public GeoJsonGeometry   Geometry   { get; set; }
            [JsonProperty("properties")] public GeoJsonProperties Properties { get; set; }
        }

        private class GeoJsonGeometry
        {
            [JsonProperty("type")]        public string Type        { get; set; }
            [JsonProperty("coordinates")] public JToken Coordinates { get; set; }
        }

        private class GeoJsonProperties
        {
            [JsonProperty("name")]              public string Name              { get; set; }
            [JsonProperty("id")]                public string Id                { get; set; }
            [JsonProperty("population")]        public long   Population        { get; set; }
            [JsonProperty("capital")]           public string Capital           { get; set; }
            [JsonProperty("capitalLat")]        public double CapitalLat        { get; set; }
            [JsonProperty("capitalLon")]        public double CapitalLon        { get; set; }
            [JsonProperty("capitalPopulation")] public long   CapitalPopulation { get; set; }
        }
    }
}
