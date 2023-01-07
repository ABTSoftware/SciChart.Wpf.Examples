﻿using System.Windows;
using System.Windows.Media;

namespace Plane3DAnnotationExample
{
    public partial class Plane3DAnnotation : Window
    {
        public Plane3DAnnotation()
        {
            InitializeComponent();

            // Create a plane
            var color = Colors.DarkRed;
            color.A = 127;
            var plane = new VerticalPlaneGeometry(5.0, 5.0, 0.0, 5.0, 5.0, 10.0, 10.0, color, true);

            // Add the cubes to the 3D Scene 
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(plane);
        }
    }
}
