// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
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
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.PointMarkers;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Core.Helpers;
using SciChart.Examples.ExternalDependencies.Data;
using SciChart.Examples.ExternalDependencies.Helpers;

namespace SciChart.Examples.Examples.Charts3D.ManipulateSeries
{
    /// <summary>
    /// Interaction logic for AddRemoveDataSeries3DChart.xaml
    /// </summary>
    public partial class AddRemoveDataSeries3DChart : UserControl, INotifyPropertyChanged
    {
        private const int MaxSeriesAmount = 15;
        private int _currentSeries = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public AddRemoveDataSeries3DChart()
        {
            InitializeComponent();
        }

        public bool CanAddSeries 
        {
            get { return sciChart.RenderableSeries.Count < MaxSeriesAmount; }
        }

        public bool CanRemoveSeries
        {
            get { return sciChart.RenderableSeries.Count > 0; }
        }

        private void AddSeriesButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sciChart.RenderableSeries.Count >= MaxSeriesAmount)
            {
                return;
            }

            var renderSerias = new ScatterRenderableSeries3D();
            var xyzDataSeries3D = new XyzDataSeries3D<double>() {SeriesName = "Series " + ++_currentSeries};

            int dataPointsCount = 15;
            var random = new Random(0);

            for (int i = 0; i < dataPointsCount; i++)
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

            var randomPicker = new FasterRandom();
            int randValue = randomPicker.Next(0, 6);

            switch (randValue)
            {
                case 0:
                    renderSerias.PointMarker = new CubePointMarker3D();
                    break;
                case 1:
                    renderSerias.PointMarker = new EllipsePointMarker3D();
                    break;
                case 2:
                    renderSerias.PointMarker = new PyramidPointMarker3D();
                    break;
                case 3:
                    renderSerias.PointMarker = new QuadPointMarker3D();
                    break;
                case 4:
                    renderSerias.PointMarker = new SpherePointMarker3D();
                    break;
                case 5:
                    renderSerias.PointMarker = new TrianglePointMarker3D();
                    break;
            }

            renderSerias.DataSeries = xyzDataSeries3D;
            sciChart.RenderableSeries.Add(renderSerias);

            var index = sciChart.RenderableSeries.IndexOf(renderSerias);
            xyzDataSeries3D.SeriesName = String.Format("Series #{0}", index);

            OnPropertyChanged("CanAddSeries");
            OnPropertyChanged("CanRemoveSeries");

            sciChart.ZoomExtents();
        }

        private void DeleteSeriesButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sciChart.RenderableSeries.Any())
            {
                var rSeries = sciChart.RenderableSeries.LastOrDefault();
                if (rSeries == null || rSeries.DataSeries == null)
                    return;

                sciChart.RenderableSeries.Remove(rSeries);

                OnPropertyChanged("CanAddSeries");
                OnPropertyChanged("CanRemoveSeries");

                sciChart.ZoomExtents();
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
