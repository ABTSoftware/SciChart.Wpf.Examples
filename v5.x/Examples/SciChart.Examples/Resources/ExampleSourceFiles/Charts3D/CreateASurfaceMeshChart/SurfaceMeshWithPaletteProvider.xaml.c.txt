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
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.RenderableSeries;

namespace SciChart.Examples.Examples.Charts3D.CreateASurfaceMeshChart
{
    /// <summary>
    /// Interaction logic for SurfaceMeshWithTextureMask.xaml
    /// </summary>
    public partial class SurfaceMeshWithTextureMask : UserControl
    {
        private DispatcherTimer _timer;
        private UniformGridDataSeries3D<double> _meshDataSeries0;
        private UniformGridDataSeries3D<double> _meshDataSeries1;

        public SurfaceMeshWithTextureMask()
        {
            InitializeComponent();

            int xSize = 49;
            int zSize = 49;
            _meshDataSeries0 = new UniformGridDataSeries3D<double>(xSize, zSize);
            _meshDataSeries1 = new UniformGridDataSeries3D<double>(xSize, zSize);

            for (int x = 48; x >= 24; x--)
            {
                var y = Math.Pow((double)x - 23 - 0.7, 0.3);
                _meshDataSeries0[24, x] = y;

                var y2 = Math.Pow((double)(x - 49) * (-1) + 0.5, 0.3);
                _meshDataSeries1[24, x] = y2 + 1.505;
            }

            for (int x = 24; x >= 0; x--)
            {
                for (int z = 49; z > 25; z--)
                {
                    var y = Math.Pow((double)z - 23 - 0.7, 0.3);
                    var y2 = Math.Pow((double)(z - 50) * (-1) + 0.5, 0.3);

                    _meshDataSeries0[x + 24, (z - 49) * (-1)] = y;
                    _meshDataSeries0[z - 1, (x - 24) * (-1)] = y;

                    _meshDataSeries1[x + 24, (z - 49) * (-1)] = y2 + 1.505;
                    _meshDataSeries1[z - 1, (x - 24) * (-1)] = y2 + 1.505;

                    _meshDataSeries0[(x - 24) * (-1), (z - 49) * (-1)] = y;
                    _meshDataSeries0[(z - 49) * (-1), (x - 24) * (-1)] = y;

                    _meshDataSeries1[(x - 24) * (-1), (z - 49) * (-1)] = y2 + 1.505;
                    _meshDataSeries1[(z - 49) * (-1), (x - 24) * (-1)] = y2 + 1.505;

                    _meshDataSeries0[x + 24, z - 1] = y;
                    _meshDataSeries0[z - 1, x + 24] = y;

                    _meshDataSeries1[x + 24, z - 1] = y2 + 1.505;
                    _meshDataSeries1[z - 1, x + 24] = y2 + 1.505;

                    _meshDataSeries0[(x - 24) * (-1), z - 1] = y;
                    _meshDataSeries0[(z - 49) * (-1), x + 24] = y;

                    _meshDataSeries1[(x - 24) * (-1), z - 1] = y2 + 1.505;
                    _meshDataSeries1[(z - 49) * (-1), x + 24] = y2 + 1.505;

                }
            }


            if (_timer == null)
            {
                _timer = new DispatcherTimer(DispatcherPriority.Render);
                _timer.Interval = TimeSpan.FromMilliseconds(1);
                _timer.Tick += OnTimerTick;
            }

            _timer.Start();
            surfaceMeshRenderableSeries.Maximum = (double)_meshDataSeries1.YRange.Max;
            surfaceMeshRenderableSeries.Minimum = (double)_meshDataSeries0.YRange.Min;
            surfaceMeshRenderableSeries.DataSeries = _meshDataSeries0;

            surfaceMeshRenderableSeries2.Maximum = (double)_meshDataSeries1.YRange.Max;
            surfaceMeshRenderableSeries2.Minimum = (double)_meshDataSeries0.YRange.Min;
            surfaceMeshRenderableSeries2.DataSeries = _meshDataSeries1;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            // Raise DataSeriesChanged event and trigger chart updates
            _meshDataSeries0.IsDirty = true;
            _meshDataSeries0.OnDataSeriesChanged(DataSeriesUpdate.DataChanged, DataSeriesAction.Update);
            _meshDataSeries1.IsDirty = true;
            _meshDataSeries1.OnDataSeriesChanged(DataSeriesUpdate.DataChanged, DataSeriesAction.Update);
        }
    }

    public class SurfaceMeshPaletteProvider3D : ISurfaceMeshPaletteProvider3D
    {
        private List<Color> _colors;
        private Random _random;

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
            if (zIndex == 0 || xIndex == 0 || 
                zIndex == 47 || xIndex == 47)
            {
                return _colors[_random.Next(1, _colors.Count())];
            }
            else if ((zIndex >= 20 && zIndex <= 26) || (xIndex >= 20 && xIndex <= 26))
            {
                return (Color?)Colors.Transparent;
            }

            return _colors[_random.Next(1, _colors.Count())];
        }
    }
}