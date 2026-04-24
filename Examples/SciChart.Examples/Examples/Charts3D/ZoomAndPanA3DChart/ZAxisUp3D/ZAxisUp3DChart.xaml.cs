// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ZAxisUp3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D;
using Viewport3D = SciChart.Charting3D.Viewport3D;

namespace SciChart.Examples.Examples.Charts3D.ZoomAndPanA3DChart.ZAxisUp3D
{
    public partial class ZAxisUp3DChart : UserControl
    {
        private Viewport3DOrientation _defaultOrientation;

        public ZAxisUp3DChart()
        {
            // Save the used Viewport orientation before the Example is initialized
            _defaultOrientation = Viewport3D.ViewportOrientation;

            InitializeComponent();

            Unloaded += OnUnLoaded;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Change Viewport orientation
            Viewport3D.SetViewportOrientation(Viewport3DOrientation.ZAxisUp);
        }

        private void OnZUpAxisChecked(object sender, RoutedEventArgs e)
        {
            // Change Viewport orientation
            Viewport3D.SetViewportOrientation(Viewport3DOrientation.ZAxisUp);
        }

        private void OnZUpAxisUnchecked(object sender, RoutedEventArgs e)
        {
            // Change Viewport orientation
            Viewport3D.SetViewportOrientation(Viewport3DOrientation.YAxisUp);
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            // Restore the original Viewport orientation
            Viewport3D.SetViewportOrientation(_defaultOrientation);            
        }
    }
}