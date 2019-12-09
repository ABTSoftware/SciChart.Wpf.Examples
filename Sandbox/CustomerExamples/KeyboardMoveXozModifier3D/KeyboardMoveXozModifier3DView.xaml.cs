using System;
using System.Windows.Controls;
using SciChart.Charting3D.Model;
using SciChart.Data.Model;

namespace KeyboardMoveXozModifier3DExample
{
    public partial class KeyboardMoveXozModifier3DView : UserControl
    {
        Random rand = new Random();
        BoxAnnotation3D boxAnnotation;

        public KeyboardMoveXozModifier3DView()
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
        }
    }
}
