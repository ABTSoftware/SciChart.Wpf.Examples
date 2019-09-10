// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AddRemoveDataSeries3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart
{
    public partial class AddGeometryTo3DChart : UserControl
    {
        public AddGeometryTo3DChart()
        {
            InitializeComponent();

            CreateCubes();
            CreateLabels();
        }

        private void CreateCubes()
        {
            // Create a cubes in 3D World space with TopLeft and BottomRight coordinates
            // We set some cubes to transparent colors to demonstrate Order Independent Transparency 
            // NOTE: Coordinates are in world coordinates. If you want data coordinates, take a look at Axis.GetCoordinate() method to convert 
            
            var cubeA = new CubeGeometry(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(50.0f, 50.0f, 50.0f), Color.FromArgb(128, 255,0,0));            
            var cubeB = new CubeGeometry(new Vector3(50.0f, 0.0f, 50.0f), new Vector3(100.0f, 100.0f, 100.0f), Color.FromArgb(128, 0, 255, 0));            
            var cubeC = new CubeGeometry(new Vector3(100.0f, 0.0f, 100.0f), new Vector3(150.0f, 150.0f, 150.0f), Color.FromArgb(255, 0, 0, 255));            
            var cubeD = new CubeGeometry(new Vector3(-100.0f, 0.0f, -100.0f), new Vector3(0.0f, 150.0f, 0.0f), Color.FromArgb(255, 0, 255, 255));            
            var cubeE = new CubeGeometry(new Vector3(-150.0f, 0.0f, -150.0f), new Vector3(50.0f, 200.0f, 50.0f), Color.FromArgb(128, 255, 255, 255));
            
            // force a far position on this cube ( user can do this in cases where a box inside another )
            var cubeF = new CubeGeometry(new Vector3(-150.0f, 0.0f, -150.0f), new Vector3(-100.0f, 50.0f, -100.0f), Color.FromArgb(128, 255, 0, 0));
            TSRVector3 farPosition = new TSRVector3(20000.0f, 20000.0f, 2000.0f);
            cubeF.SetPosition(farPosition);

            // Add the cubes to the 3D Scene 
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeA);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeB);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeC);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeD);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeE);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(cubeF);
        }

        private void CreateLabels()
        {
            // Add some text labels to the scene 
            var textA = new TextSceneEntity("Teeny Block", Color.FromArgb(255, 0, 225, 0), new Vector3(75f, 100f, 75f), TextDisplayMode.FacingCameraAlways, 9, "Segoe UI");
            var textB = new TextSceneEntity("Big Block", Color.FromArgb(255, 255, 255, 255), new Vector3(-50f,200f,-50f), TextDisplayMode.FacingCameraAlways, 10, "Tahoma");

            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(textA);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(textB);
        }
    }
}
