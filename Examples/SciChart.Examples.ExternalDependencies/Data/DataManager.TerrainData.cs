// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// DataManager.TerrainData.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public partial class DataManager : IDataManager
    {
        /// <summary>
        /// Loads the Banda Sea digital elevation model from the gzipped little-endian Int16 grid
        /// embedded in this assembly, returning a <see cref="SeismicRegionDem"/> ready for the
        /// terrain layer and the synthetic-catalog generator.
        /// </summary>
        public SeismicRegionDem GetSeismicTerrain()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .Single(n => n.EndsWith("SeismicRegionElevation.gz"));

            var raw = new byte[SeismicRegionDem.Width * SeismicRegionDem.Height * sizeof(short)];
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            {
                int offset = 0;
                while (offset < raw.Length)
                {
                    int read = gz.Read(raw, offset, raw.Length - offset);
                    if (read <= 0) throw new EndOfStreamException("Unexpected end of elevation data");
                    offset += read;
                }
            }

            var elevation = new double[SeismicRegionDem.Height, SeismicRegionDem.Width];
            for (int y = 0; y < SeismicRegionDem.Height; y++)
            {
                for (int x = 0; x < SeismicRegionDem.Width; x++)
                {
                    elevation[y, x] = BitConverter.ToInt16(raw, (y * SeismicRegionDem.Width + x) * sizeof(short));
                }
            }

            return new SeismicRegionDem(elevation);
        }
    }
}
