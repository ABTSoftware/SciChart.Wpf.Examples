using SciChart.Charting3D.Model;
using SciChart.Data.Model;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace SciChart.Sandbox.Examples.MouseDragModifier3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [TestCase("Annotation Drag Modifier 3D")]
    public partial class AnnotationDragModifier3D : UserControl
    {
        Random rand = new Random();
        BoxAnnotation3D boxAnnotation;

        public AnnotationDragModifier3D()
        {
            InitializeComponent();

            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            const int count = 100;

            for (int i = 0; i < count; i++)
            {
                var x = rand.Next(10, 90);
                var y = rand.Next(10, 90);
                var z = rand.Next(10, 90);

                xyzDataSeries3D.Append(x, y, z);
            }

            ScatterSeries3D.DataSeries = xyzDataSeries3D;

            SciChart.XAxis.VisibleRange = new DoubleRange(0, 100);
            SciChart.YAxis.VisibleRange = new DoubleRange(0, 100);
            SciChart.ZAxis.VisibleRange = new DoubleRange(0, 100);


            //addAnnotation();
            AddAnnotation();
        }

        void AddAnnotation()
        {
            boxAnnotation = new BoxAnnotation3D();

            boxAnnotation.RangeX = new DoubleRange(rand.Next(20, 30), rand.Next(70, 90));
            boxAnnotation.RangeY = new DoubleRange(rand.Next(20, 30), rand.Next(70, 90));
            boxAnnotation.RangeZ = new DoubleRange(rand.Next(20, 30), rand.Next(70, 90));

            boxAnnotation.Color = Color.FromArgb(150, 255, 255, 255);
            boxAnnotation.DragColor = Color.FromArgb(220, 240, 147, 43);
            boxAnnotation.Stroke = Color.FromRgb(52, 152, 219);
            boxAnnotation.StrokeWidth = 2;
            
            SciChart.Viewport3D.RootEntity.Children.Add(boxAnnotation);
        }
    }
}
