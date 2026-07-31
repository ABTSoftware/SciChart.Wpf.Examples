// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// DataManager.SeismicEventsData.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public partial class DataManager : IDataManager
    {
        // ~1,000 events per year over 2000-2010
        public const int SeismicEventCount = 10000;

        private static readonly DateTime SeismicCatalogStart = new DateTime(2000, 1, 1);
        private const double SeismicCatalogSpanMinutes = 10 * 365.25 * 24 * 60d;

        /// <summary>
        /// Generates a synthetic earthquake catalog shaped by the real terrain: sparse shallow
        /// crustal events on the islands, and offshore depths growing with both the distance
        /// from the shoreline and the local water depth, so each deep basin reads as a bullseye
        /// deepening toward its center - reaching 500-650 km in the Banda Sea cores. The densest
        /// activity follows the deep troughs. All events are randomly generated, not real records.
        /// </summary>
        public SeismicCatalog GenerateSeismicCatalog(SeismicRegionDem dem, int totalCount = SeismicEventCount)
        {
            var rng = new Random(8214);
            var events = new List<SeismicEvent>(totalCount + 64);

            // Named mainshocks across the depth gradient, each with its own aftershock swarm:
            // deep in the South Banda Basin core, intermediate in the Weber Deep, shallow coastal
            AddMainshock(events, dem, rng, -6.10, 126.20, 7.9,
                new DateTime(2003, 9, 17, 3, 42, 0), "M7.9 Mainshock", 500);
            AddMainshock(events, dem, rng, -5.80, 131.10, 7.3,
                new DateTime(2006, 4, 5, 21, 8, 0), "M7.3 Mainshock", 350);
            AddMainshock(events, dem, rng, -3.40, 122.80, 6.9,
                new DateTime(2008, 11, 23, 12, 30, 0), "M6.9 Mainshock", 200);

            // Background seismicity fills the remaining budget
            while (events.Count < totalCount)
            {
                var ev = NextBackgroundEvent(dem, rng);
                if (ev == null) continue;

                events.Add(ev);

                // Large background events spawn their own tight aftershock clusters
                if (ev.Magnitude >= 6.8 && events.Count < totalCount - 10)
                {
                    AddAftershocks(events, rng, ev, Math.Min(15 + rng.Next(30), totalCount - events.Count));
                }
            }

            double minDepthKm = double.MaxValue, maxDepthKm = double.MinValue, maxMagnitude = 0;
            foreach (var ev in events)
            {
                if (ev.DepthKm < minDepthKm) minDepthKm = ev.DepthKm;
                if (ev.DepthKm > maxDepthKm) maxDepthKm = ev.DepthKm;
                if (ev.Magnitude > maxMagnitude) maxMagnitude = ev.Magnitude;
            }

            return new SeismicCatalog(events, minDepthKm, maxDepthKm, maxMagnitude);
        }

        private static SeismicEvent NextBackgroundEvent(SeismicRegionDem dem, Random rng)
        {
            double latitude, longitude;

            if (rng.NextDouble() < 0.30)
            {
                // The dense band follows the deep troughs (the curved arc has no single trench axis)
                dem.GetRandomDeepCell(rng, out latitude, out longitude);
                latitude += Gaussian(rng) * 0.08;
                longitude += Gaussian(rng) * 0.08;
            }
            else
            {
                latitude = SeismicRegionDem.LatStart + 0.15
                    + rng.NextDouble() * (SeismicRegionDem.LatEnd - SeismicRegionDem.LatStart - 0.3);
                longitude = SeismicRegionDem.LonStart + 0.15
                    + rng.NextDouble() * (SeismicRegionDem.LonEnd - SeismicRegionDem.LonStart - 0.3);
            }

            if (longitude < SeismicRegionDem.LonStart + 0.05 || longitude > SeismicRegionDem.LonEnd - 0.05) return null;
            if (latitude < SeismicRegionDem.LatStart + 0.05 || latitude > SeismicRegionDem.LatEnd - 0.05) return null;

            double elevation = dem.ElevationAt(latitude, longitude);
            double depth;

            if (elevation > 0)
            {
                // On the islands: much fewer events, all shallow crustal (up to 15 km)
                if (rng.NextDouble() > 0.15) return null;
                depth = 3 + rng.NextDouble() * 12;
            }
            else
            {
                depth = BasinDepth(dem.ShoreDistanceKmAt(latitude, longitude), -elevation, rng);
            }

            return new SeismicEvent
            {
                Latitude = latitude,
                Longitude = longitude,
                DepthKm = depth,
                Magnitude = NextMagnitude(rng, 4.0, 8.4),
                Time = SeismicCatalogStart.AddMinutes(rng.NextDouble() * SeismicCatalogSpanMinutes)
            };
        }

        /// <summary>
        /// Focal depth grows with both the distance from the shoreline and the local water
        /// depth, so events deepen toward the centers of the deep basins (where the subducted
        /// slab lies), reaching 500-650 km in the Banda Sea cores, while shallow shelves stay
        /// shallow no matter how far from land they are.
        /// </summary>
        private static double BasinDepth(double shoreDistanceKm, double waterDepthM, Random rng)
        {
            double depth = 8 + 650 * Math.Min(1, shoreDistanceKm / 160) * Math.Min(1, waterDepthM / 5000);

            if (rng != null)
            {
                depth = depth * (1 + Gaussian(rng) * 0.10) + Gaussian(rng) * 4;
            }

            return Math.Max(3, Math.Min(660, depth));
        }

        /// <summary>
        /// Depth a mainshock gets at the given location, following the same zoning as the
        /// background events: crustal on land, basin model offshore.
        /// </summary>
        private static double ModelDepthAt(SeismicRegionDem dem, double latitude, double longitude)
        {
            double elevation = dem.ElevationAt(latitude, longitude);
            if (elevation > 0) return 10;

            return BasinDepth(dem.ShoreDistanceKmAt(latitude, longitude), -elevation, null);
        }

        private static void AddMainshock(List<SeismicEvent> events, SeismicRegionDem dem, Random rng,
            double latitude, double longitude, double magnitude, DateTime time, string name, int aftershockCount)
        {
            var mainshock = new SeismicEvent
            {
                Latitude = latitude,
                Longitude = longitude,
                DepthKm = ModelDepthAt(dem, latitude, longitude),
                Magnitude = magnitude,
                Name = name,
                Time = time
            };

            events.Add(mainshock);
            AddAftershocks(events, rng, mainshock, aftershockCount);
        }

        private static void AddAftershocks(List<SeismicEvent> events, Random rng, SeismicEvent parent, int count)
        {
            // Omori-style decay: gaps between aftershocks grow as the sequence dies out
            double hours = 0;

            for (int i = 0; i < count; i++)
            {
                hours += (i + 1) * rng.NextDouble() * 9;
                events.Add(new SeismicEvent
                {
                    Latitude = parent.Latitude + Gaussian(rng) * 0.09,
                    Longitude = parent.Longitude + Gaussian(rng) * 0.11,
                    // Depth scatter proportional to the parent's depth, so shallow sequences stay
                    // shallow and deep clusters don't blur the basin depth pattern
                    DepthKm = Math.Max(3, parent.DepthKm + Gaussian(rng) * Math.Max(4, parent.DepthKm * 0.1)),
                    Magnitude = NextMagnitude(rng, 4.0, parent.Magnitude - 0.7),
                    Time = parent.Time.AddHours(hours)
                });
            }
        }

        /// <summary>
        /// Exponential magnitude falloff approximating the Gutenberg-Richter law:
        /// small events are common, large ones are rare.
        /// </summary>
        private static double NextMagnitude(Random rng, double min, double max)
        {
            double magnitude = min - Math.Log(1 - rng.NextDouble()) * 0.45;
            return Math.Min(magnitude, Math.Max(min, max));
        }

        private static double Gaussian(Random rng)
        {
            double u1 = 1.0 - rng.NextDouble();
            double u2 = rng.NextDouble();
            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
        }
    }
}
