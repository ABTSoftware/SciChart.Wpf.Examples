using System;
using System.Windows;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting3D.Model;

namespace _3DChartScatterSeriesWithDraggablePoints
{
    public partial class MainWindow : Window
    {
        private readonly NonUniformGridDataSeries3D<double> meshDataSeries;
        private readonly XyzDataSeries3D<double> xyzDataSeries;
        private readonly double[] xSteppings, zSteppings;

        public MainWindow()
        {
            InitializeComponent();
            
            int xSize = 15;
            int zSize = 25;

            xSteppings = new double[xSize];
            zSteppings = new double[zSize];

            for (int i = 0; i < xSize; i++)
            {
                xSteppings[i] = Math.Log((i + 1) * 0.01);
            }
            for (int i = 0; i < zSize; i++)
            {
                zSteppings[i] = Math.Log((i + 1) * 0.01);
            }

            meshDataSeries = new NonUniformGridDataSeries3D<double>(xSize, zSize, xIndex => xSteppings[xIndex], zIndex => zSteppings[zIndex]);
            xyzDataSeries = new XyzDataSeries3D<double>();

            for (int xIndex = 0; xIndex < xSize; xIndex++)
            {
                for (int zIndex = 0; zIndex < zSize; zIndex++)
                {
                    var y = Math.Sin(xIndex / 3.0) / ((zIndex + 1.0) * 2.0);
                    meshDataSeries[zIndex, xIndex] = y;
                    xyzDataSeries.Append(xSteppings[xIndex], y, zSteppings[zIndex], new PointMetadata3D());
                }
            }

            surfaceMeshRenderableSeries.DataSeries = meshDataSeries;
            scatterRenderableSeries.DataSeries = xyzDataSeries;
        }

        private void UpdatePointInfo(int index, double x, double y, double z)
        {
            pointInfo.Text = $"Point Index: {index} (X: {x:N2} Y: {y:N2} Z: {z:N2})";
        }

        private void ClearPointInfo()
        {
            pointInfo.Text = string.Empty;
        }

        private void OnPointDragStart(object sender, DragPointEventArgs e)
        {
            if (e.PointIndex < xyzDataSeries.Count)
            {
                var x = xyzDataSeries.XValues[e.PointIndex];
                var y = xyzDataSeries.YValues[e.PointIndex];
                var z = xyzDataSeries.ZValues[e.PointIndex];

                UpdatePointInfo(e.PointIndex, x, y, z);
            }
        }

        private void OnPointDragDelta(object sender, DragPointEventArgs e)
        {
            if (e.PointIndex < xyzDataSeries.Count)
            {
                var y = (double)e.YValue;

                var x = xyzDataSeries.XValues[e.PointIndex];
                var z = xyzDataSeries.ZValues[e.PointIndex];
                var w = xyzDataSeries.WValues[e.PointIndex];

                var xIndex = xSteppings.FindIndex<double>(true, x, SearchMode.Exact);
                var zIndex = zSteppings.FindIndex<double>(true, z, SearchMode.Exact);

                meshDataSeries[zIndex, xIndex] = y;
                xyzDataSeries.Update(e.PointIndex, x, y, z, w);

                UpdatePointInfo(e.PointIndex, x, y, z);
            }
        }

        private void OnPointDragEnd(object sender, DragPointEventArgs e)
        {
            ClearPointInfo();
        }
    }
}