using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using SciChart.Charting3D.Model;
using SciChart.Core.Extensions;

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
            AscData result = new AscData()
            {
                XValues = new List<float>(),
                YValues = new List<float>(),
                ZValues = new List<float>(),
                ColorValues = new List<Color>(),
            };

            await Task.Run(() =>
            {
                using (var file = File.OpenText(filename))
                {
                    // Load the ASC file format 
                    result.NumberColumns = ReadInt(file, "ncols");
                    result.NumberRows = ReadInt(file, "nrows");
                    result.XllCorner = ReadInt(file, "xllcorner");
                    result.YllCorner = ReadInt(file, "yllcorner");
                    result.CellSize = ReadInt(file, "cellsize");
                    result.NoDataValue = ReadInt(file, "NODATA_value");

                    // Generate X-values based off cell position 
                    float[] xValuesRow = Enumerable.Range(0, result.NumberColumns).Select(x => (float)x * result.CellSize).ToArray();

                    for (int i = 0; i < result.NumberRows; i++)
                    {
                        // Read heights from the ASC file and generate Z-cell values
                        float[] heightValuesRow = ReadFloats(file, " ", result.NoDataValue);
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
                }
            });

            return result;
        }

        public static async Task<XyzDataSeries3D<float>> ParseToXyzDataSeries(AscData lidarData, string identifier)
        {
            var xyzDataSeries3D = new XyzDataSeries3D<float>() { SeriesName = identifier };

            await Task.Run(() =>
            {
                if (lidarData.ColorValues.IsNullOrEmpty())
                {
                    xyzDataSeries3D.Append(lidarData.XValues, lidarData.YValues, lidarData.ZValues);
                }
                else
                {
                    xyzDataSeries3D.Append(lidarData.XValues, lidarData.YValues, lidarData.ZValues, lidarData.ColorValues.Select(x => new PointMetadata3D(x)));
                }
            });

            return xyzDataSeries3D;
        }

        public static async Task<UniformGridDataSeries3D<float>> ParseToGridDataSeries(AscData lidarData, string identifier)
        {
            UniformGridDataSeries3D<float> uniformGridDataSeries = null;

            await Task.Run(() =>
            {
                uniformGridDataSeries = new UniformGridDataSeries3D<float>(lidarData.NumberColumns, lidarData.NumberRows);
                uniformGridDataSeries.SeriesName = identifier;
                uniformGridDataSeries.StartX = 0;
                uniformGridDataSeries.StepX = lidarData.CellSize;
                uniformGridDataSeries.StartZ = 0;
                uniformGridDataSeries.StepZ = lidarData.CellSize;

                int index = 0;
                for (int z = 0; z < lidarData.NumberRows; z++)
                {
                    for (int x = 0; x < lidarData.NumberColumns; x++)
                    {
                        uniformGridDataSeries.InternalArray[z][x] = lidarData.YValues[index++];
                    }
                }
            });

            return uniformGridDataSeries;
        }

        private static int ReadInt(StreamReader file, string prefix)
        {
            string line = file.ReadLine();
            line = line.Replace(prefix, "").Trim();
            return Int32.Parse(line, CultureInfo.InvariantCulture);
        }

        private static float[] ReadFloats(StreamReader file, string separator, float noDataValue)
        {
            string line = file.ReadLine();
            float[] values = line.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    float rawValue = float.Parse(x);
                    return rawValue == noDataValue ? float.NaN : rawValue;
                } ).ToArray();
            return values;
        }
    }
}