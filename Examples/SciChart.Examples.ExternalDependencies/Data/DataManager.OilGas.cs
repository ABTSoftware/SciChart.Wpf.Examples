// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DataManager.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public partial class DataManager : IDataManager
    {
        private static double ParseOptionalValue(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Equals("-"))
            {
                return double.NaN;
            }
            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Loads and parses grid data from an embedded CSV resource file based on the specified grid index.
        /// </summary>
        /// <param name="gridIndex">
        /// The index of the grid resource to load (clamped between 0 and 8).
        /// Determines which embedded CSV file is accessed for data parsing.
        /// </param>
        /// <returns>
        /// Returns a list of parsed <see cref="OilGasData"/> instances.
        /// </returns>
        public IList<OilGasData> LoadOilGasGridData(int gridIndex)
        {
            var values = new List<OilGasData>();
            var asm = Assembly.GetExecutingAssembly();

            var resourceIndex = Math.Max(Math.Min(gridIndex, 8), 0);
            var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains($"OilAndGas.00-Grid-{gridIndex}.csv.gz"));

            using (var stream = asm.GetManifestResourceStream(resourceString))
            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gz))
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');

                        if (data.Length < 6)
                        {
                            var x = ParseOptionalValue(data[0]);

                            values.Add(new OilGasData
                            {
                                X1 = x,
                                Y1 = ParseOptionalValue(data[1]),

                                X2 = x,
                                Y2 = ParseOptionalValue(data[2]),

                                X3 = x,
                                Y3 = ParseOptionalValue(data[3])
                            });
                        }
                        else
                        {
                            values.Add(new OilGasData
                            {
                                X1 = ParseOptionalValue(data[0]),
                                Y1 = ParseOptionalValue(data[1]),

                                X2 = ParseOptionalValue(data[2]),
                                Y2 = ParseOptionalValue(data[3]),

                                X3 = ParseOptionalValue(data[4]),
                                Y3 = ParseOptionalValue(data[5])
                            });
                        }
                    }

                    line = streamReader.ReadLine();
                }
            }

            return values;
        }

        /// <summary>
        /// Loads and parses Density data from an embedded CSV resource file.
        /// </summary>
        /// <returns>
        /// Returns a list of parsed <see cref="OilGasData"/> instances.
        /// </returns>
        public IList<OilGasData> LoadOilGasDensityData()
        {
            var values = new List<OilGasData>();
            var asm = Assembly.GetExecutingAssembly();
            var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains("OilAndGas.01-Density.csv.gz"));

            using (var stream = asm.GetManifestResourceStream(resourceString))
            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gz))
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');

                        values.Add(new OilGasData
                        {
                            X1 = double.Parse(data[0], CultureInfo.InvariantCulture),
                            Y1 = double.Parse(data[1], CultureInfo.InvariantCulture),
                            Y2 = double.Parse(data[2], CultureInfo.InvariantCulture),
                        });
                    }

                    line = streamReader.ReadLine();
                }
            }

            return values;
        }

        /// <summary>
        /// Loads and parses Shale data from an embedded CSV resource file.
        /// </summary>
        /// <returns>
        /// Returns a list of parsed <see cref="OilGasData"/> instances.
        /// </returns>
        public IList<OilGasData> LoadOilGasShaleData()
        {
            var values = new List<OilGasData>();
            var asm = Assembly.GetExecutingAssembly();
            var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains("OilAndGas.01-Shale.csv.gz"));

            using (var stream = asm.GetManifestResourceStream(resourceString))
            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gz))
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');

                        var x = double.Parse(data[0], CultureInfo.InvariantCulture);

                        values.Add(new OilGasData
                        {
                            X1 = x,
                            Y1 = double.Parse(data[1], CultureInfo.InvariantCulture),

                            X2 = x,
                            Y2 = double.Parse(data[2], CultureInfo.InvariantCulture),

                            X3 = x,
                            Y3 = double.Parse(data[3], CultureInfo.InvariantCulture)
                        });
                    }

                    line = streamReader.ReadLine();
                }
            }

            return values;
        }

        /// <summary>
        /// Loads and parses Resistivity data from an embedded CSV resource file.
        /// </summary>
        /// <returns>
        /// Returns a list of parsed <see cref="OilGasData"/> instances.
        /// </returns>
        public IList<OilGasData> LoadOilGasResistivityData()
        {
            var values = new List<OilGasData>();
            var asm = Assembly.GetExecutingAssembly();
            var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains("OilAndGas.01-Resistivity.csv.gz"));

            using (var stream = asm.GetManifestResourceStream(resourceString))
            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gz))
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');

                        values.Add(new OilGasData
                        {
                            X1 = double.Parse(data[0], CultureInfo.InvariantCulture),
                            Y1 = double.Parse(data[1], CultureInfo.InvariantCulture)
                        });
                    }

                    line = streamReader.ReadLine();
                }
            }

            return values;
        }

        /// <summary>
        /// Loads and parses Pore Space data from an embedded CSV resource file.
        /// </summary>
        /// <returns>
        /// Returns a list of parsed <see cref="OilGasData"/> instances.
        /// </returns>
        public IList<OilGasData> LoadOilGasPoreSpaceData()
        {
            var values = new List<OilGasData>();
            var asm = Assembly.GetExecutingAssembly();
            var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains("OilAndGas.01-PoreSpace.csv.gz"));

            using (var stream = asm.GetManifestResourceStream(resourceString))
            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gz))
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');

                        var x = double.Parse(data[0], CultureInfo.InvariantCulture);

                        values.Add(new OilGasData
                        {
                            X1 = x,
                            Y1 = double.Parse(data[1], CultureInfo.InvariantCulture),

                            X2 = x,
                            Y2 = double.Parse(data[2], CultureInfo.InvariantCulture),

                            X3 = x,
                            Y3 = ParseOptionalValue(data[3])
                        });
                    }

                    line = streamReader.ReadLine();
                }
            }

            return values;
        }

        /// <summary>
        /// Loads Sonic data from an embedded CSV file named and populates the provided 2D array with parsed values.
        /// </summary>
        /// <param name="destArray">
        /// A 2D array of doubles with minimum dimensions [100, 1000]. The parsed sonic data is 
        /// written into this array, where each row corresponds to one line of CSV values.
        /// </param>
        public void LoadOilGasSonicData(double[,] destArray)
        {
            if (destArray?.GetLength(0) >= 100 && destArray?.GetLength(1) >= 1000)
            {
                var asm = Assembly.GetExecutingAssembly();
                var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains("OilAndGas.01-Sonic.csv.gz"));

                using (var stream = asm.GetManifestResourceStream(resourceString))
                using (var gz = new GZipStream(stream, CompressionMode.Decompress))
                using (var streamReader = new StreamReader(gz))
                {
                    var i = 0;
                    var line = streamReader.ReadLine();

                    while (!string.IsNullOrEmpty(line))
                    {
                        if (!line.StartsWith("/"))
                        {
                            var data = line.Split(';');

                            for (int j = 0; j < 1000; j++)
                            {
                                destArray[i, j] = double.Parse(data[j], CultureInfo.InvariantCulture);
                            }

                            i++;
                        }

                        line = streamReader.ReadLine();
                    }
                }
            }
        }

        /// <summary>
        /// Loads and parses Texture data from an embedded CSV resource file.
        /// </summary>
        /// <returns>
        /// Returns a list of parsed <see cref="OilGasData"/> instances.
        /// </returns>
        public IList<OilGasData> LoadOilGasTextureData()
        {
            var values = new List<OilGasData>();
            var asm = Assembly.GetExecutingAssembly();
            var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains("OilAndGas.01-Texture.csv.gz"));

            using (var stream = asm.GetManifestResourceStream(resourceString))
            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gz))
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');

                        values.Add(new OilGasData
                        {
                            X1 = double.Parse(data[0], CultureInfo.InvariantCulture),
                            Y1 = double.Parse(data[1], CultureInfo.InvariantCulture)
                        });
                    }

                    line = streamReader.ReadLine();
                }
            }

            return values;
        }

        /// <summary>
        /// Loads and parses Scatter 3D data from an embedded CSV resource file.
        /// </summary>
        /// <returns>
        /// Returns a list of parsed <see cref="OilGasData"/> instances.
        /// </returns>
        public IList<OilGasData> LoadOilGasScatterXyzData()
        {
            var values = new List<OilGasData>();
            var asm = Assembly.GetExecutingAssembly();
            var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains("OilAndGas.02-Scatter-XYZ.csv.gz"));

            using (var stream = asm.GetManifestResourceStream(resourceString))
            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gz))
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');

                        values.Add(new OilGasData
                        {
                            X1 = double.Parse(data[0], CultureInfo.InvariantCulture),
                            Y1 = double.Parse(data[1], CultureInfo.InvariantCulture),
                            Z1 = double.Parse(data[2], CultureInfo.InvariantCulture),

                            Scale = double.Parse(data[3], CultureInfo.InvariantCulture)
                        });
                    }

                    line = streamReader.ReadLine();
                }
            }

            return values;
        }
    }
}
