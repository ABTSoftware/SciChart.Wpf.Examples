using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Lidar3DPointCloudDemo
{
    public struct AscData
    {
        public List<float> XValues { get; set; }
        public List<float> YValues { get; set; }
        public List<float> ZValues { get; set; }
        public List<Color> ColorValues { get; set; }
        public int CellSize { get; set; }
        public int XllCorner { get; set; }
        public int YllCorner { get; set; }
        public int NumberColumns { get; set; }
        public int NumberRows { get; set; }
        public int NoDataValue { get; set; }
    }

    public class AscReader
    {
        public static async Task<AscData> ReadFileToAscData(
            string filename, Func<float, Color> colorMapFunction, Action<int> reportProgress = null)
        {
            var result = await Task.Run(() =>
            {
                using (var file = File.OpenText(filename))
                {
                    return ReadFromStream(file, colorMapFunction, reportProgress);
                }
            });

            return result;
        }

        public static async Task<AscData> ReadResourceToAscData(
            string resourceName, Func<float, Color> colorMapFunction, Action<int> reportProgress = null)
        {
            var result = await Task.Run(() =>
            {
                var asm = Assembly.GetExecutingAssembly();
                var resource = asm.GetManifestResourceNames()
                    .Single(x => x.Contains(resourceName));

                using (var stream = asm.GetManifestResourceStream(resource))
                using (var gz = new GZipStream(stream, CompressionMode.Decompress))
                using (var streamReader = new StreamReader(gz))
                {
                    return ReadFromStream(streamReader, colorMapFunction, reportProgress);
                }
            });

            return result;
        }

        private static AscData ReadFromStream(StreamReader stream, Func<float, Color> colorMapFunction, Action<int> reportProgress = null)
        {
            var result = new AscData
            {
                XValues = new List<float>(),
                YValues = new List<float>(),
                ZValues = new List<float>(),
                ColorValues = new List<Color>(),
                NumberColumns = ReadInt(stream, "ncols"),
                NumberRows = ReadInt(stream, "nrows"),
                XllCorner = ReadInt(stream, "xllcorner"),
                YllCorner = ReadInt(stream, "yllcorner"),
                CellSize = ReadInt(stream, "cellsize"),
                NoDataValue = ReadInt(stream, "NODATA_value"),
            };

            // Load the ASC file format 

            // Generate X-values based off cell position 
            float[] xValuesRow = Enumerable.Range(0, result.NumberColumns).Select(x => (float)x * result.CellSize).ToArray();

            for (int i = 0; i < result.NumberRows; i++)
            {
                // Read heights from the ASC file and generate Z-cell values
                float[] heightValuesRow = ReadFloats(stream, " ", result.NoDataValue);
                float[] zValuesRow = Enumerable.Repeat(0 + i * result.CellSize, result.NumberRows).Select(x => (float)x).ToArray();

                result.XValues.AddRange(xValuesRow);
                result.YValues.AddRange(heightValuesRow);
                result.ZValues.AddRange(zValuesRow);

                if (colorMapFunction != null)
                {
                    // Optional color-mapping of points based on height 
                    Color[] colorValuesRow = heightValuesRow
                        .Select(colorMapFunction)
                        .ToArray();
                    result.ColorValues.AddRange(colorValuesRow);
                }

                // Optional report loading progress 0-100%
                reportProgress?.Invoke((int)(100.0f * i / result.NumberRows));
            }

            return result;
        }

        private static int ReadInt(StreamReader file, string prefix)
        {
            string line = file.ReadLine();
            line = line.Replace(prefix, string.Empty).Trim();
            return int.Parse(line, NumberFormatInfo.InvariantInfo);
        }

        private static float[] ReadFloats(StreamReader file, string separator, float noDataValue)
        {
            string line = file.ReadLine();
            float[] values = line.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    float rawValue = float.Parse(x, NumberFormatInfo.InvariantInfo);
                    return rawValue.CompareTo(noDataValue) == 0 ? float.NaN : rawValue;
                } ).ToArray();
            return values;
        }
    }
}