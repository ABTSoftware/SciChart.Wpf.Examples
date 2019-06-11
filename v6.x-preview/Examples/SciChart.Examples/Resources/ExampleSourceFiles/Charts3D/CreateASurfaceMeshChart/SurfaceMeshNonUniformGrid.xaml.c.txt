using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SciChart.Charting3D.Model;

namespace SciChart.Examples.Examples.Charts3D.CreateASurfaceMeshChart
{
    /// <summary>
    /// Interaction logic for SurfaceMeshNonUniformGrid.xaml
    /// </summary>
    public partial class SurfaceMeshNonUniformGrid : UserControl
    {
        public SurfaceMeshNonUniformGrid()
        {
            InitializeComponent();

            int xSize = 25;
            int zSize = 25;
            var xSteppings = new double[25];
            var zSteppings = new double[25];

            // Create the stepping information for the X,Z cells 
            // The X-Z plane exists on the floor of the 3D Chart. 
            for (int i = 0; i < 25; i++)
            {
                xSteppings[i] = Math.Log((i + 1) * 0.01);
                zSteppings[i] = xSteppings[i];
            }

            // Create the Nonuniform data series to hold the data 
            var meshDataSeries = new NonUniformGridDataSeries3D<double>(xSize, zSize, xIndex => xSteppings[xIndex], zIndex => zSteppings[zIndex])
            {                
                SeriesName = "Nonuniform Surface Mesh",
            };

            // Now we populate our heights (The Y-values are up)
            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    double xVal = (double)x / (double)xSize * 25.0;
                    double zVal = (double)z / (double)zSize * 25.0;
                    double y = Math.Sin(xVal * 0.2) / ((zVal + 1) * 2);
                    meshDataSeries[z, x] = y;
                }
            }

            // Assign the DataSeries to the chart 
            surfaceMeshRenderableSeries.DataSeries = meshDataSeries;
        }

        private void CheckDrawSkirtChanged(object sender, RoutedEventArgs e)
        {
            if (surfaceMeshRenderableSeries != null)
            {
                surfaceMeshRenderableSeries.DrawSkirt = CheckDrawSkirt.IsChecked == true;
            }
        }
    }
}
