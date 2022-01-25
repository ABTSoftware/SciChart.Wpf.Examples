// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HistogramView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting3D.Extensions;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Lidar3DPointCloudDemo
{
    /// <summary>
    /// Interaction logic for Lidar3DPointCloudDemoView.xaml
    /// </summary>
    public partial class Lidar3DPointCloudDemoView : UserControl
    {
        public Lidar3DPointCloudDemoView()
        {
            InitializeComponent();
        }

        private async void Lidar3DPointCloudDemoView_OnLoaded(object sender, RoutedEventArgs e)
        {
            await ReadLidarData();
        }

        private async Task ReadLidarData()
        {
            // The LinearColorMap type in SciChart allows you to generate a colour map based on a 
            // minimum and maximum value, e.g. min=0, max=50 means the gradient brush below is mapped into that range
            var colorMap = new LinearColorMap
            {
                Minimum = 0,
                Maximum = 50,
                GradientStops = new[]
                {
                    new ColorMapPoint { Color = Colors.DodgerBlue.ToArgb(), Offset = 0.0},
                    new ColorMapPoint { Color = Colors.LimeGreen.ToArgb(), Offset = 0.2},
                    new ColorMapPoint { Color = Colors.Orange.ToArgb(), Offset = 0.5},
                    new ColorMapPoint { Color = Colors.OrangeRed.ToArgb(), Offset = 0.7},
                    new ColorMapPoint { Color = Colors.Purple.ToArgb(), Offset = 1.0},
                }
            };

            // Call .InitializeConstants() once and use ColorUtil.Lerp to interpolate the colormap for each data-value 
            colorMap.InitializeConstants();

            // Read the ASC Lidar data file with optional color map data
            var lidarData = await DataManager.Instance.GetAscDataAsync(heightValue => ColorMapFunction(heightValue, colorMap));

            // Parse into SciChart format
            pointCloud.DataSeries = await ParseToXyzDataSeries(lidarData, "tq3080_DSM_2M Point-Cloud");
            surfaceMesh.DataSeries = await ParseToGridDataSeries(lidarData, "tq3080_DSM_2M Topology Map");
        }

        private Color ColorMapFunction(float heightValue, LinearColorMap colorMap)
        {
            // Linearly interpolate each heightValue into a colour and return to the ASCReader
            // This will be injected into the SciChart XyzDataSeries3D to colour points in the point-cloud
            var argbColor = ColorUtil.Lerp(colorMap, heightValue);
            return ColorUtil.FromUInt(argbColor);
        }

        public static async Task<XyzDataSeries3D<float>> ParseToXyzDataSeries(global::Lidar3DPointCloudDemo.AscData lidarData, string identifier)
        {
            var xyzDataSeries3D = new XyzDataSeries3D<float> { SeriesName = identifier };

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

        public static async Task<UniformGridDataSeries3D<float>> ParseToGridDataSeries(global::Lidar3DPointCloudDemo.AscData lidarData, string identifier)
        {
            UniformGridDataSeries3D<float> uniformGridDataSeries = null;

            await Task.Run(() =>
            {
                uniformGridDataSeries = new UniformGridDataSeries3D<float>(lidarData.NumberColumns, lidarData.NumberRows) 
                {
                    SeriesName = identifier, 
                    StartX = 0, 
                    StepX = lidarData.CellSize, 
                    StartZ = 0, 
                    StepZ = lidarData.CellSize
                };

                for (int index = 0, z = 0; z < lidarData.NumberRows; z++)
                {
                    for (var x = 0; x < lidarData.NumberColumns; x++)
                    {
                        uniformGridDataSeries.InternalArray[z][x] = lidarData.YValues[index++];
                    }
                }
            });

            return uniformGridDataSeries;
        }
    }
}