using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SciChart.Charting3D.Model;

namespace Lidar3DPointCloudDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ReadLidarData();
        }

        private async void ReadLidarData()
        {
            const string filename = "LIDAR-DSM-2M-TQ38sw\\tq3080_DSM_2M.asc";
            var xyzDataSeries3D = await AscReader.ReadFile(filename, (progress) =>
                {
                    Console.WriteLine($"Progress {progress}%");
                });

            pointCloud.DataSeries = xyzDataSeries3D;
        }
    }

    public class AscReader
    {
        public static async Task<XyzDataSeries3D<float>> ReadFile(string filename, Action<int> reportProgress)
        {
            var xyzDataSeries3D = new XyzDataSeries3D<float>();

            await Task.Run(() =>
            {
                using (var file = File.OpenText(filename))
                {
                    int ncols = ReadInt(file, "ncols");
                    int nrows = ReadInt(file, "nrows");
                    int xllcorner = ReadInt(file, "xllcorner");
                    int yllcorner = ReadInt(file, "yllcorner");
                    int cellsize = ReadInt(file, "cellsize");
                    int NODATA_value = ReadInt(file, "NODATA_value");

                    float[] xValues = Enumerable.Range(0, ncols).Select(x => (float)x).ToArray();

                    for (int i = 0; i < nrows; i++)
                    {
                        float[] heightValues = ReadFloats(file, " ");
                        float[] zValues = Enumerable.Repeat(0 + i * cellsize, nrows).Select(x => (float)x).ToArray();
                        xyzDataSeries3D.Append(xValues, heightValues, zValues);
                        reportProgress((int)(100.0f * i / nrows));
                    }
                }
            });

            return xyzDataSeries3D;
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
