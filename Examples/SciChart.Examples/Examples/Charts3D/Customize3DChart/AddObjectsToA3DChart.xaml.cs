// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AddObjectsToA3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Controls;
using SciChart.Charting3D.Model;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart
{
    public partial class AddObjectsToA3DChart : UserControl
    {
        public AddObjectsToA3DChart()
        {
            InitializeComponent();

            var dataSeries = new UniformGridDataSeries3D<double>(9, 9)
            {
                StartX = 1,
                StepX = 1,

                StartZ = 100,
                StepZ = 1
            };
            
            for (int x = 0; x < 9; x++)
            {
                for (int z = 0; z < 9; z++)
                {
                    if (z % 2 == 0)
                    {
                        dataSeries[z, x] = (x % 2 == 0 ? 1 : 4);
                    }
                    else
                    {
                        dataSeries[z, x] = (x % 2 == 0 ? 4 : 1);
                    }
                }
            }

            surfaceMeshRenderableSeries.DataSeries = dataSeries;
        }
    }
}