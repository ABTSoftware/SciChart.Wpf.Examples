using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting3D.Extensions;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.RenderableSeries;

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

            this.Loaded += async (s, e) => { await ReadLidarData(); };

        }

        private async Task ReadLidarData()
        {
            // The LinearColorMap type in SciChart allows you to generate a colour map based on a 
            // minimum and maximum value, e.g. min=0, max=50 means the gradient brush below is mapped into that range
            // 
            // call .InitializeConstants() once and use ColorUtil.Lerp to interpolate the colormap for each data-value 
            LinearColorMap colorMap = new LinearColorMap()
            {
                Minimum = 0,
                Maximum = 50,
                GradientStops = new []
                {
                    new ColorMapPoint() { Color = Colors.DodgerBlue.ToArgb(), Offset = 0},
                    new ColorMapPoint() { Color = Colors.LimeGreen.ToArgb(), Offset = 0.2},
                    new ColorMapPoint() { Color = Colors.Orange.ToArgb(), Offset = 0.5},
                    new ColorMapPoint() { Color = Colors.OrangeRed.ToArgb(), Offset = 0.7},
                    new ColorMapPoint() { Color = Colors.Purple.ToArgb(), Offset = 1},
                }
            };
            colorMap.InitializeConstants();

            // Read the ASC Lidar data file with optional color map data
            const string filename = "LIDAR-DSM-2M-TQ38sw\\tq3080_DSM_2M.asc";
            var xyzDataSeries3D = await AscReader.ReadFile(filename, heightValue => this.ColorMapFunction(heightValue, colorMap));

            pointCloud.DataSeries = xyzDataSeries3D;
        }

        private Color ColorMapFunction(float heightValue, LinearColorMap colorMap)
        {
            // Linearly interpolate each heightValue into a colour and return to the ASCReader
            // This will be injected into the SciChart XyzDataSeries3D to colour points in the point-cloud
            uint argbColor = ColorUtil.Lerp(colorMap, heightValue);
            return ColorUtil.FromUInt(argbColor);
        }
    }
}
