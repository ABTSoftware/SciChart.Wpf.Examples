// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SelectScatterPoint3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.PointMarkers;
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.TooltipsAndHitTest3DCharts
{
    /// <summary>
    /// Interaction logic for SelectScatterPoint3DChart.xaml
    /// </summary>
    public partial class SelectScatterPoint3DChart : UserControl
    {
        private XyzDataSeries3D<double> _xyzDataSeries3D;

        public SelectScatterPoint3DChart()
        {
            InitializeComponent();

            sciChartSurface.FullScreenAntiAliasing = FullScreenAntiAliasingMode.None;

            XAx.VisibleRange = new DoubleRange(0, 10);
            YAx.VisibleRange = new DoubleRange(-10, 10);
            ZAx.VisibleRange = new DoubleRange(1000, 10000);

            _xyzDataSeries3D = new XyzDataSeries3D<double>();
            renderableSeries3D.PointMarker = new EllipsePointMarker3D();
            _xyzDataSeries3D.DataSeriesChanged += OnScatterDataChanged;

            for (int i = 0; i < 300; i++)
            {
                double x = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                double y = DataManager.Instance.GetGaussianRandomNumber(0.0, 5.0);
                double z = DataManager.Instance.GetGaussianRandomNumber(5500, 1500);

                PointMetadata3D vertex = new PointMetadata3D(Colors.Coral, 10);

                _xyzDataSeries3D.Append(x, y, z, vertex);
            }

            renderableSeries3D.DataSeries = _xyzDataSeries3D;
            sciChartSurface.ZoomExtents();
        }


        private void OnScatterDataChanged(object sender, DataSeriesChangedEventArgs e)
        {
            List<XyzDataPointViewModel<double>> selectedPoints = new List<XyzDataPointViewModel<double>>();
            for (int i = 0; i < _xyzDataSeries3D.Count; i++)
            {
                if (_xyzDataSeries3D.WValues[i] != null)
                {
                    if (_xyzDataSeries3D.WValues[i].IsSelected)
                    {
                        selectedPoints.Add(new XyzDataPointViewModel<double>(i,
                            _xyzDataSeries3D.XValues[i],
                            _xyzDataSeries3D.YValues[i],
                            _xyzDataSeries3D.ZValues[i],
                            _xyzDataSeries3D.WValues[i]));
                    }
                }
            }
            if (!selectedPoints.IsNullOrEmpty())
            {
                slectedItem.Visibility = Visibility.Visible;
                slectedItem.ItemsSource = selectedPoints;
            }
            else slectedItem.Visibility = Visibility.Collapsed;
        }
    }
}
