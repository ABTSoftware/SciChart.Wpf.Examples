using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
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
using SciChart.Charting3D.Visuals.Object;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart
{
    /// <summary>
    /// Interaction logic for AddObjectsToA3DChart.xaml
    /// </summary>
    public partial class AddObjectsToA3DChart : UserControl
    {
        public AddObjectsToA3DChart()
        {
            InitializeComponent();
            var dataSeries = new UniformGridDataSeries3D<double>(9, 9) { StartX = 1, StepX = 1, StartZ = 100, StepZ = 1 };
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