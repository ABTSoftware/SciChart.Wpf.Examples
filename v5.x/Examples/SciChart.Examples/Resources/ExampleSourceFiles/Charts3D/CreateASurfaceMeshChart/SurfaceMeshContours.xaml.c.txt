using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.RenderableSeries;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;

namespace SciChart.Examples.Examples.Charts3D.CreateASurfaceMeshChart
{
    /// <summary>
    /// Interaction logic for CreateAContouredChart.xaml
    /// </summary>
    public partial class SurfaceMeshContours : UserControl
    {

        public SurfaceMeshContours()
        {
            InitializeComponent();

            var meshDataSeries = FillSeries(0, 200, 200);
            surfaceMeshRenderableSeries.DataSeries = meshDataSeries;
            contourComboBox.ItemsSource = typeof(Colors).GetProperties().Select(x => new ColorModel { ColorName = x.Name, Color = (Color)x.GetValue(null, null) }).ToList();
        }

        private void CheckDrawSkirtChanged(object sender, RoutedEventArgs e)
        {
            if (surfaceMeshRenderableSeries != null)
            {
                surfaceMeshRenderableSeries.DrawSkirt = CheckDrawSkirt.IsChecked == true;
            }
        }

        private IDataSeries3D FillSeries(int index, int width, int height)
        {
            double angle = Math.PI*2*index/30;

            int w = width, h = height;

            var dataSeries = new UniformGridDataSeries3D<double>(w, h)
            {
                StepX = 0.01,
                StepZ = 0.01,
            };

            for (int x = 0; x < 200; x++)
            {
                for (int y = 0; y < 200; y++)
                {
                    var v = (1 + Math.Sin(x*0.04 + angle))*50 + (1 + Math.Sin(y*0.1 + angle))*50*(1 + Math.Sin(angle*2));
                    var cx = w/2;
                    var cy = h/2;
                    var r = Math.Sqrt((x - cx)*(x - cx) + (y - cy)*(y - cy));
                    var exp = Math.Max(0, 1 - r*0.008);
                    var zValue = v*exp;

                    dataSeries[y, x] = zValue;
                }
            }

            return dataSeries;
        }

        private void ContourColorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            surfaceMeshRenderableSeries.ContourColor = ((ColorModel)contourComboBox.SelectedItem).Color;
        }
    }

    public class ColorModel
    {
        public Color Color { get; set; }

        public Brush Brush
        {
            get { return new SolidColorBrush(Color); }
        }

        public string ColorName { get; set; }
    }
}