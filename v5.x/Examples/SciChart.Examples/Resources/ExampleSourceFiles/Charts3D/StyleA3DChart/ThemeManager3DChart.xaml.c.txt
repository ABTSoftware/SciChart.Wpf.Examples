// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ThemeManager3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using System.Windows.Media;
using SciChart.Charting;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.PointMarkers;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.StyleA3DChart
{
    /// <summary>
    /// Interaction logic for ThemeManager3DChart.xaml
    /// </summary>
    public partial class ThemeManager3DChart : UserControl
    {
        public ThemeManager3DChart()
        {
            InitializeComponent();

            // Fill theme combo box and set default theme
            foreach (string theme in ThemeManager.AllThemes)
            {
                cboTheme.Items.Add(theme);
            }

            cboTheme.SelectedItem = "BlackSteel";
        }

        private void CboTheme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeManager.SetTheme(sciChart, (string)cboTheme.SelectedItem);
        }

        private void ThemeManager3DChart_OnLoaded(object sender, RoutedEventArgs e)
        {
            scatterSeries3D.PointMarker = new PyramidPointMarker3D();
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            int count = 150;

            var random = new Random(0);

            for (int i = 0; i < count; i++)
            {
                double x = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                double y = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                double z = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);

                // Scale is a multiplier used to increase/decrease ScatterRenderableSeries3D.ScatterPointSize
                float scale = (float)((random.NextDouble() + 0.5) * 3.0);

                // Color is applied to PointMetadata3D and overrides the default ScatterRenderableSeries.Stroke property
                Color? randomColor = Color.FromArgb(0xFF, (byte)random.Next(50, 255), (byte)random.Next(50, 255), (byte)random.Next(50, 255));

                // To declare scale and colour, add a VertextData class as the w (fourth) parameter. 
                // The PointMetadata3D class also has other properties defining the behaviour of the XYZ point
                xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(randomColor, scale));
            }

            scatterSeries3D.DataSeries = xyzDataSeries3D;
        }
    }
}
