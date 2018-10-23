using System.Windows.Controls;
using SciChart.Charting3D.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D.RenderableSeries;

namespace SciChart.Examples.Examples.Charts3D.CreateASurfaceMeshChart
{
    using System;
    using System.IO;
    using System.Windows;

    using SciChart.Charting.Common.MarkupExtensions;

    /// <summary>
    /// Interaction logic for SurfaceMeshFloorAndCeiling.xaml
    /// </summary>
    public partial class Bottom3D : UserControl
    {
        private UniformGridDataSeries3D<double> ds = new UniformGridDataSeries3D<double>(135, 61);
        private UniformGridDataSeries3D<double> dsEmpty = new UniformGridDataSeries3D<double>(135, 61);

        public Bottom3D()
        {
            InitializeComponent();

            this.LoadData();

            EffectManager.EnableDropShadows = false;

            ds.StartX = 0;
            ds.StepX = 1;
            ds.StartZ = 0;
            ds.StepZ = 1;

            // Initialize some nice looking data
            //ds[0, 0] = -1.43; ds[0, 1] = -2.95; ds[0, 2] = -2.97; ds[0, 3] = -1.81; ds[0, 4] = -1.33; ds[0, 5] = -1.53; ds[0, 6] = -2.04; ds[0, 7] = 2.08; ds[0, 8] = 1.94; ds[0, 9] = 1.42; ds[0, 10] = 1.58; ds[1, 0] = 1.77;
            //ds[1, 1] = 1.76; ds[1, 2] = -1.1; ds[1, 3] = -0.26; ds[1, 4] = 0.72; ds[1, 5] = 0.64; ds[1, 6] = 3.26; ds[1, 7] = 3.2; ds[1, 8] = 3.1; ds[1, 9] = 1.94; ds[1, 10] = 1.54; 
            //ds[2, 0] = 0; ds[2, 1] = 0; ds[2, 2] = 0; ds[2, 3] = 0; ds[2, 4] = 0; ds[2, 5] = 3.7; ds[2, 6] = 3.7; ds[2, 7] = 3.7; ds[2, 8] = 3.7; ds[2, 9] = -0.48; ds[2, 10] = -0.48;
            //ds[3, 0] = 0; ds[3, 1] = 0; ds[3, 2] = 0; ds[3, 3] = 0; ds[3, 4] = 0; ds[3, 5] = 0; ds[3, 6] = 0; ds[3, 7] = 0; ds[3, 8] = 0; ds[3, 9] = 0; ds[3, 10] = 0;



            // We assign the same data to all three SurfaceMeshRenderableSeries3D
            // One for floor, one for ceiling, one for the actual mesh in the center 
            scs.RenderableSeries[0].DataSeries = ds;
            //scs.RenderableSeries[1].DataSeries = ds;
            //scs.RenderableSeries[2].DataSeries = ds;
        }

        private void LoadData()
        {
            double dNullMaskingValue = 0.30562356772;
            SurfaceMeshPaletteProvider3D.NullMaskingValue = dNullMaskingValue;
            using (var reader = new StreamReader(@"..\..\Examples\ShadowsOn3DChart\Bottom3D.csv"))
            {
                var i = 0;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    for (int j = 0; j < values.Length; j++)
                    {
                        if (values[j].Equals("NaN"))
                            this.ds[j, i] = dNullMaskingValue;
                        else
                        this.ds[j, i] = Double.Parse(values[j]) / 1000.0;
                    }

                    i++;
                }
            }
            SurfaceMeshPaletteProvider3D.s_ds = ds;
        }
    }

    public class SurfaceMeshPaletteProvider3D : ISurfaceMeshPaletteProvider3D
    {
        private List<Color> _colors;
        private Random _random;

        public static UniformGridDataSeries3D<double> s_ds;
        public static double NullMaskingValue = 0.0;

        public void OnAttach(IRenderableSeries3D renderSeries)
        {
            _colors = GetCashedColorsCollection();
            _random = new Random();
        }

        public void OnDetached()
        {
            _colors = null;
        }

        /// <summary>
        /// Returns a collection of cached brushes
        /// </summary>
        /// <returns></returns>
        private List<Color> GetCashedColorsCollection()
        {
            return typeof(Colors).GetProperties().Select(x => (Color)x.GetValue(null, null)).ToList();
        }

        public Color? OverrideCellColor(IRenderableSeries3D series, int xIndex, int zIndex)
        {
            /// override with transparent here where cells are hidden
            if (  s_ds[zIndex, xIndex] == NullMaskingValue)
            {
                return (Color?)Colors.Transparent;
            }
            else
            {
                /// returning null here mean no override, 
                /// returning white or any other color, will override what comes from gradient
                //return (Color?)Colors.White;
                return null;
            }
        }
    }
}
