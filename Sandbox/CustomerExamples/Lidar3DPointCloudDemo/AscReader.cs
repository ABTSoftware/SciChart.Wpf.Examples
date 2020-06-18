using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.RenderableSeries;

namespace Lidar3DPointCloudDemo
{
    public class AscReader
    {
        public static async Task<XyzDataSeries3D<float>> ReadFile(string filename, Func<float, Color> colorMapFunction, Action<int> reportProgress = null)
        {
            var xyzDataSeries3D = new XyzDataSeries3D<float>();

            await Task.Run(() =>
            {
                using (var file = File.OpenText(filename))
                {
                    // Load the ASC file format 
                    int ncols = ReadInt(file, "ncols");
                    int nrows = ReadInt(file, "nrows");
                    int xllcorner = ReadInt(file, "xllcorner");
                    int yllcorner = ReadInt(file, "yllcorner");
                    int cellsize = ReadInt(file, "cellsize");
                    int NODATA_value = ReadInt(file, "NODATA_value");

                    // Generate X-values based off cell position 
                    float[] xValues = Enumerable.Range(0, ncols).Select(x => (float)x).ToArray();

                    for (int i = 0; i < nrows; i++)
                    {
                        // Read heights from the ASC file and generate Z-cell values
                        float[] heightValues = ReadFloats(file, " ");
                        float[] zValues = Enumerable.Repeat(0 + i * cellsize, nrows).Select(x => (float)x).ToArray();

                        if (colorMapFunction != null)
                        {
                            // Optional color-mapping of points based on height 
                            PointMetadata3D[] metadata = heightValues
                                .Select(height => new PointMetadata3D(colorMapFunction(height)))
                                .ToArray();
                            xyzDataSeries3D.Append(xValues, heightValues, zValues, metadata);
                        }
                        else
                        {
                            // No color-map, just append x,y,z values 
                            xyzDataSeries3D.Append(xValues, heightValues, zValues);
                        }
                        
                        // Optional report loading progress 0-100%
                        reportProgress?.Invoke((int)(100.0f * i / nrows));
                    }
                }
            });

            return xyzDataSeries3D;
        }

        private static PointMetadata3D[] GetPointColors(float yMin, float yMax, Color minColor, Color maxColor, float[] heightValues)
        {
            return heightValues.Select(h =>
            {
                float ratio = (h - yMin) / (yMax - yMin);
                ratio = Math.Max(ratio, 0);
                var pointColor = ColorUtil.Lerp(minColor, maxColor, ratio);
                return new PointMetadata3D(pointColor);
            }).ToArray();
        }

        private static int ReadInt(StreamReader file, string prefix)
        {
            string line = file.ReadLine();
            line = line.Replace(prefix, "").Trim();
            return Int32.Parse(line, CultureInfo.InvariantCulture);
        }

        private static float[] ReadFloats(StreamReader file, string separator)
        {
            string line = file.ReadLine();
            float[] values = line.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => float.Parse(x)).ToArray();
            return values;
        }
    }
}