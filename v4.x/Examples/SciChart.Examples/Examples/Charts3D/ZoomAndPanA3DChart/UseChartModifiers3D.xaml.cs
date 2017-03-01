using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting3D.Model;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.Charts3D.ZoomAndPanA3DChart
{
    /// <summary>
    /// Interaction logic for UseChartModifiers3D.xaml
    /// </summary>
    public partial class UseChartModifiers3D : UserControl
    {
        public UseChartModifiers3D()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            const int count = 25;
            double step = 0.3;
            var random = new Random(0);

            Color color;

            for (int x = 0; x < count; x++)
            {
                // Color is applied to PointMetadata3D and overrides the default ScatterRenderableSeries.Stroke property
                color = Color.FromArgb(0xFF, (byte)random.Next(50, 255), (byte)random.Next(50, 255), (byte)random.Next(50, 255));
                
                for (int z = 1; z < count; z++)
                {
                    var y = (z != 0) ? Math.Pow((double) z, step) : Math.Pow((double) z + 1, 0.3);

                    xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(color, 2));
                }
            }

            ScatterSeries3D.DataSeries = xyzDataSeries3D;
        }

        private void OnClickHorizontalRotate(object sender, RoutedEventArgs e)
        {
            Camera3D.OrbitalYaw = ((int) Camera3D.OrbitalYaw < 360) ? Camera3D.OrbitalYaw + 90 : (Camera3D.OrbitalYaw - 360) * (-1);
        }

        private void OnClickVerticalRotate(object sender, RoutedEventArgs e)
        {
            Camera3D.OrbitalPitch = ((int)Camera3D.OrbitalPitch < 89) ? Camera3D.OrbitalPitch + 90 : -90;
        }
    }
}