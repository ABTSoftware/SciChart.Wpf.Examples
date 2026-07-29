// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SeismicRegionDem.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    /// <summary>
    /// A digital elevation model of the Banda Sea region (10-2S latitude, 120-134E longitude,
    /// 30 arc-second cells) - the meeting point of the Sunda, Australian and Pacific plates -
    /// derived from the ETOPO 2022 elevation extract. Exposes the elevation grid for the terrain
    /// layer plus the shoreline-distance field and deep-trough cells used to shape the synthetic
    /// seismicity. Build it with <see cref="DataManager.GetSeismicTerrain"/>.
    /// Terrain data: ETOPO 2022 (NOAA), public domain.
    /// </summary>
    public class SeismicRegionDem
    {
        public const int Width = 1680;
        public const int Height = 960;
        public const double LonStart = 120.0;
        public const double LatStart = -10.0;
        public const double CellSizeDeg = 1.0 / 120.0;

        /// <summary>Ocean cells at least this deep count as the deep-trough band.</summary>
        public const double DeepTroughElevation = -5000;

        public static double LonEnd { get { return LonStart + Width * CellSizeDeg; } }
        public static double LatEnd { get { return LatStart + Height * CellSizeDeg; } }

        private readonly double[,] _shoreDistanceKm;
        private readonly List<int> _deepCellIndexes;

        internal SeismicRegionDem(double[,] elevation)
        {
            Elevation = elevation;

            double min = double.MaxValue, max = double.MinValue;
            var deepCells = new List<int>();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var e = elevation[y, x];
                    if (e < min) min = e;
                    if (e > max) max = e;
                    if (e < DeepTroughElevation) deepCells.Add(y * Width + x);
                }
            }

            MinElevation = min;
            MaxElevation = max;
            _deepCellIndexes = deepCells;
            _shoreDistanceKm = ComputeShoreDistance(elevation);
        }

        /// <summary>
        /// Elevation in meters indexed as [latIndex, lonIndex]; row 0 is the southern edge.
        /// Negative values are below sea level.
        /// </summary>
        public double[,] Elevation { get; }

        public double MinElevation { get; }
        public double MaxElevation { get; }

        /// <summary>
        /// Returns the elevation in meters at the given coordinates (nearest cell, clamped to the grid).
        /// </summary>
        public double ElevationAt(double latitude, double longitude)
        {
            return Elevation[LatIndexOf(latitude), LonIndexOf(longitude)];
        }

        /// <summary>
        /// Returns the distance in km from the given point to the nearest land (the shoreline),
        /// zero on land itself. Nearest cell lookup, clamped to the grid.
        /// </summary>
        public double ShoreDistanceKmAt(double latitude, double longitude)
        {
            return _shoreDistanceKm[LatIndexOf(latitude), LonIndexOf(longitude)];
        }

        /// <summary>
        /// Returns the coordinates of a random cell belonging to the deep-trough band
        /// (the deepest bathymetry, where the synthetic seismicity is densest).
        /// </summary>
        public void GetRandomDeepCell(Random rng, out double latitude, out double longitude)
        {
            int index = _deepCellIndexes[rng.Next(_deepCellIndexes.Count)];

            latitude = LatStart + ((index / Width) + 0.5) * CellSizeDeg;
            longitude = LonStart + ((index % Width) + 0.5) * CellSizeDeg;
        }

        private static int LatIndexOf(double latitude)
        {
            int y = (int)Math.Round((latitude - LatStart) / CellSizeDeg - 0.5);
            return y < 0 ? 0 : (y > Height - 1 ? Height - 1 : y);
        }

        private static int LonIndexOf(double longitude)
        {
            int x = (int)Math.Round((longitude - LonStart) / CellSizeDeg - 0.5);
            return x < 0 ? 0 : (x > Width - 1 ? Width - 1 : x);
        }

        /// <summary>
        /// Two-pass chamfer distance transform: distance from every ocean cell to the nearest
        /// land cell, in km, with the horizontal cell size shrinking with latitude.
        /// </summary>
        private static double[,] ComputeShoreDistance(double[,] elevation)
        {
            const double cellKmY = 111.32 * CellSizeDeg;
            const double unreachable = 1e9;

            var dist = new double[Height, Width];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    dist[y, x] = elevation[y, x] > 0 ? 0 : unreachable;
                }
            }

            for (int y = 0; y < Height; y++)
            {
                double cellKmX = cellKmY * Math.Cos((LatStart + (y + 0.5) * CellSizeDeg) * Math.PI / 180);
                double cellKmDiag = Math.Sqrt(cellKmX * cellKmX + cellKmY * cellKmY);

                for (int x = 0; x < Width; x++)
                {
                    double d = dist[y, x];
                    if (x > 0) d = Math.Min(d, dist[y, x - 1] + cellKmX);
                    if (y > 0)
                    {
                        d = Math.Min(d, dist[y - 1, x] + cellKmY);
                        if (x > 0) d = Math.Min(d, dist[y - 1, x - 1] + cellKmDiag);
                        if (x < Width - 1) d = Math.Min(d, dist[y - 1, x + 1] + cellKmDiag);
                    }
                    dist[y, x] = d;
                }
            }

            for (int y = Height - 1; y >= 0; y--)
            {
                double cellKmX = cellKmY * Math.Cos((LatStart + (y + 0.5) * CellSizeDeg) * Math.PI / 180);
                double cellKmDiag = Math.Sqrt(cellKmX * cellKmX + cellKmY * cellKmY);

                for (int x = Width - 1; x >= 0; x--)
                {
                    double d = dist[y, x];
                    if (x < Width - 1) d = Math.Min(d, dist[y, x + 1] + cellKmX);
                    if (y < Height - 1)
                    {
                        d = Math.Min(d, dist[y + 1, x] + cellKmY);
                        if (x < Width - 1) d = Math.Min(d, dist[y + 1, x + 1] + cellKmDiag);
                        if (x > 0) d = Math.Min(d, dist[y + 1, x - 1] + cellKmDiag);
                    }
                    dist[y, x] = d;
                }
            }

            return dist;
        }
    }
}
