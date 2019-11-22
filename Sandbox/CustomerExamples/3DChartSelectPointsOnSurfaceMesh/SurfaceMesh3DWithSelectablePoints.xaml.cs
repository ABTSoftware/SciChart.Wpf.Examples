using System;
using System.Windows;
using SciChart.Charting3D.Model;

namespace _3DChartChangePropertiesDynamically
{
    public partial class SurfaceMesh3DWithSelectablePoints : Window
    {
        public SurfaceMesh3DWithSelectablePoints()
        {
            InitializeComponent();

            int xSize = 15;
            int zSize = 25;
            var xSteppings = new double[xSize];
            var zSteppings = new double[zSize];
                
            // Create the stepping information for the X, Z cells
            // The X-Z plane exists on the floor of the 3D Chart. 
            for (int i = 0; i < xSize; i++)
            {
                xSteppings[i] = Math.Log((i + 1) * 0.01);
            }
            for (int i = 0; i < zSize; i++)
            { 
                zSteppings[i] = 1 - Math.Log((i + 1) * 0.01);
            }

            // Create the Nonuniform data series to hold the data 
            // NOTE: A bug which we are investigating is why NonUniformGridDataSeries3D requires -zSteppings to have the same XYZ positions as Scatter chart
            var meshDataSeries = new NonUniformGridDataSeries3D<double>(xSize, zSize, xIndex => xSteppings[xIndex], zIndex => -zSteppings[zIndex])
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

            // Mirror the same data on the Scatter chart
            var xyzDataSeries = meshDataSeries.ToXyzDataSeries3D();
            ScatterRenderableSeries3D.DataSeries = xyzDataSeries;

            // Select some random points 

            // Select at X = 5, 6, 7, 8, 9
            //       and Z = 10,11,12,13,14

            // NOTE: Corresponding xyzDataSeries indices to SurfaceMeshRenderableSeries cells as follows
            // because the method ToXyzDataSeries3D() iterates through X, then Z, we can calculate the index 
            // of a point in the XyzDataSeries from the grid cell as follows: 
            //
            // index = zIndex + xIndex*xSize
            
            xyzDataSeries.PerformSelection(true, new int[]
            {
                10 + 5 * zSize, 11 + 5 * zSize, 12 + 5 * zSize, 13 + 5 * zSize, 14 + 5 * zSize,
                10 + 6 * zSize, 11 + 6 * zSize, 12 + 6 * zSize, 13 + 6 * zSize, 14 + 6 * zSize,
                10 + 7 * zSize, 11 + 7 * zSize, 12 + 7 * zSize, 13 + 7 * zSize, 14 + 7 * zSize,
                10 + 8 * zSize, 11 + 8 * zSize, 12 + 8 * zSize, 13 + 8 * zSize, 14 + 8 * zSize,
                10 + 9 * zSize, 11 + 9 * zSize, 12 + 9 * zSize, 13 + 9 * zSize, 14 + 9 * zSize,
            });
        }
    }

    public static class SciChartExtensions
    {
        public static XyzDataSeries3D<TX,TY,TZ> ToXyzDataSeries3D<TX,TY,TZ>(this BaseGridDataSeries3D<TX,TY,TZ> gridDataSeries)
            where TX:IComparable
            where TY:IComparable
            where TZ:IComparable
        {
            var xyzDataSeries = new XyzDataSeries3D<TX,TY,TZ>();

            for (int x = 0; x < gridDataSeries.XSize; x++)
            {
                for (int z = 0; z < gridDataSeries.ZSize; z++)
                {
                    xyzDataSeries.Append(gridDataSeries.GetX(x), gridDataSeries[z,x], gridDataSeries.GetZ(z), new PointMetadata3D(null, 1F, false));
                }
            }

            return xyzDataSeries;
        }
    }
}
