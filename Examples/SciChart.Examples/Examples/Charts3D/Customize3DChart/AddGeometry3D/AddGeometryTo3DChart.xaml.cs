// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AddGeometryTo3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting3D;
using SciChart.Charting3D.Interop;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart.AddGeometry3D
{
    public partial class AddGeometryTo3DChart : UserControl
    {
        public AddGeometryTo3DChart()
        {
            InitializeComponent();

            Loaded += OnExampleLoaded;
        }

        private void OnExampleLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Create some SceneInstances and add to the 3D scene

            // Requires Coordinate transformation from Data Coordinates to World Coordinates,
            // which requires Axes3D to be created
            CreateCubes();
            CreateLabels();
        }

        private void CreateLabels()
        {
            // Add text labels to the scene 
            var textA = new TextSceneEntity("Teeny Block", Color.FromArgb(255, 0xDC, 0x79, 0x69), TransformToWorldCoordinates(new Vector3(6.5f, 2.0f, 6.5f)), 9, "Segoe UI");
            var textB = new TextSceneEntity("Big Block", Color.FromArgb(255, 0xF4, 0x84, 0x0B), TransformToWorldCoordinates(new Vector3(4.0f, 4.0f, 4.0f)), 10, "Tahoma");

            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(textA);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(textB);
        }

        private Vector3 TransformToWorldCoordinates(Vector3 chartPoint3D)
        {
            var worldPoint3D = new Vector3
            {
                X = (float)sciChart3DSurface.XAxis.GetCurrentCoordinateCalculator().GetCoordinate(chartPoint3D.X) - sciChart3DSurface.WorldDimensions.X / 2.0f,
                Y = (float)sciChart3DSurface.YAxis.GetCurrentCoordinateCalculator().GetCoordinate(chartPoint3D.Y),
                Z = (float)sciChart3DSurface.ZAxis.GetCurrentCoordinateCalculator().GetCoordinate(chartPoint3D.Z) - sciChart3DSurface.WorldDimensions.Z / 2.0f
            };

            return worldPoint3D;
        }

        private void CreateCubes()
        {
            // Create a cubes in 3D World space with TopLeft and BottomRight coordinates
            // We set some cubes to transparent colors to demonstrate Order Independent Transparency 

            // Create CubeGeometries defined by chart coordinates
            var cubeA = CreateCubeGeometry(new Vector3(4.5f, 0.0f, 4.5f), new Vector3(6.0f, 1.5f, 6.0f), Color.FromArgb(128, 0x64, 0xBA, 0xE4));
            var cubeB = CreateCubeGeometry(new Vector3(6.0f, 0.0f, 6.0f), new Vector3(7.0f, 2.5f, 7.0f), Color.FromArgb(128, 0xDC, 0x79, 0x69));
            var cubeC = CreateCubeGeometry(new Vector3(7.0f, 0.0f, 7.0f), new Vector3(8.0f, 3.5f, 8.0f), Color.FromArgb(255, 0x6B, 0xC4, 0xA9));
            var cubeD = CreateCubeGeometry(new Vector3(2.5f, 0.0f, 2.5f), new Vector3(4.5f, 3.5f, 4.5f), Color.FromArgb(255, 0x5C, 0x43, 0x9B));
            var cubeE = CreateCubeGeometry(new Vector3(1.0f, 0.0f, 1.0f), new Vector3(6.0f, 5.0f, 6.0f), Color.FromArgb(128, 0xF4, 0x84, 0x0B));
            var cubeF = CreateCubeGeometry(new Vector3(1.0f, 0.0f, 1.0f), new Vector3(2.5f, 1.5f, 2.5f), Color.FromArgb(128, 0xF6, 0x08, 0x6C));

            // force a far position on this cube, in world coordinates ( user can do this in cases where a box inside another )
            var farPosition = new TSRVector3(20000.0f, 20000.0f, 2000.0f);
            cubeF.SetPosition(farPosition);

            // Add the cubes to the 3D Scene 
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeA);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeB);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeC);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeD);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeE);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeF);
        }

        private CubeGeometry CreateCubeGeometry(Vector3 topLeft, Vector3 bottomRight, Color color)
        {
            // Transform the chart coordinates to world coordinates
            var topLeftWorld = TransformToWorldCoordinates(topLeft);
            var bottomRightWorld = TransformToWorldCoordinates(bottomRight);

            // Create a CubeGeometry from the world coordinates
            var cubeGeometry = new CubeGeometry(topLeftWorld, bottomRightWorld, color);

            return cubeGeometry;
        }
    }
}
