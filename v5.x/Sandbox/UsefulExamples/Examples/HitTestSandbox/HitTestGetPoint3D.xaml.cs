using System;
using System.Windows;
using System.Windows.Input;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.RenderableSeries;

namespace SciChart.Sandbox.Examples.HitTest3DApi
{
    [TestCase("Hit-Test and get Point 3D")]
    public partial class HitTestGetPoint3D : Window
    {
        public HitTestGetPoint3D()
        {
            InitializeComponent();

            int xSize = 25;
            int zSize = 25;
            var meshDataSeries = new UniformGridDataSeries3D<double>(xSize, zSize)
            {
                StepX = 1,
                StepZ = 1,
                SeriesName = "Uniform Surface Mesh",
            };

            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    double xVal = x / (double)xSize * 25.0;
                    double zVal = z / (double)zSize * 25.0;
                    double y = Math.Sin(xVal * 0.2) / ((zVal + 1) * 2);
                    meshDataSeries[z, x] = y;
                }
            }

            renderSeries.DataSeries = meshDataSeries;
        }

        private bool IsHitPointValid(HitTestInfo3D hitTestInfo)
        {
            return !hitTestInfo.IsEmpty() && hitTestInfo.IsHit;
        }

        private void SciChart_OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            // Perform hit test on the series
            HitTestInfo3D hitTestInfo = renderSeries.HitTest(Mouse.GetPosition(sciChart));

            // Check if hit test was successful
            if (IsHitPointValid(hitTestInfo))
            {
                // Convert the hit test result to a series info
                var seriesInfo = renderSeries.ToSeriesInfo(hitTestInfo);
                // Retrieve XYZ of clicked point from series info
                MessageBox.Show($"Clicked point: X [ {seriesInfo.HitVertex.X} ] Y [ {seriesInfo.HitVertex.Y} ] Z [ {seriesInfo.HitVertex.Z} ]");
            }
        }
    }
}
