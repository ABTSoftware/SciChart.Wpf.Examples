﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ModifyCameraProperties.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D;

namespace SciChart.Examples.Examples.Charts3D.ZoomAndPanA3DChart
{
    public partial class ModifyCameraProperties : UserControl
    {
        public ModifyCameraProperties()
        {
            InitializeComponent();
        }

        private void OnAttachOrthogonalChecked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;

            perspAttached.IsChecked = false;
            orthoAttached.IsChecked = true;
 
            // You can also set these on the scichartsurface entirely in code, creating a new instance of 
            // Camera3D rather than declaring in XAML
            sciChart.Camera = TryFindResource("OrthogonalCamera") as Camera3D;
        }

        private void OnAttachPerspectiveChecked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;

            perspAttached.IsChecked = true;
            orthoAttached.IsChecked = false;

            // You can also set these on the scichartsurface entirely in code, creating a new instance of 
            // Camera3D rather than declaring in XAML
            sciChart.Camera = TryFindResource("PerspectiveCamera") as Camera3D;
        }

        private void ModifyCameraProperties_OnLoaded(object sender, RoutedEventArgs e)
        {
            OnAttachPerspectiveChecked(null, null);
        }

        private void OnUseLhsCoordinates(object sender, RoutedEventArgs e)
        {
            if (sciChart != null) sciChart.CoordinateSystem = CoordinateSystem3D.LeftHanded;
        }

        private void OnUseRhsCoordinates(object sender, RoutedEventArgs e)
        {
            if (sciChart != null) sciChart.CoordinateSystem = CoordinateSystem3D.RightHanded;
        }
    }
}
