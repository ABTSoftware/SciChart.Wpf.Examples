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
using System.Windows.Shapes;
using SciChart.Charting.Visuals.Annotations;

namespace SciChart.Sandbox.Examples.HitTestSandbox
{
    [TestCase("Hit-Test to Add Annotations on a Chart")]
    public partial class HitTestToAddAnnotations : Window
    {
        public HitTestToAddAnnotations()
        {
            InitializeComponent();
        }

        private void Scs_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Get mouse point
            var mousePoint = e.GetPosition(scs);

            // Transform point to inner viewport 
            // https://www.scichart.com/questions/question/how-can-i-convert-xyaxis-value-to-chart-surface-coodinate
            // https://www.scichart.com/documentation/v5.x/Axis%20APIs%20-%20Convert%20Pixel%20to%20Data%20Coordinates.html
            mousePoint = scs.RootGrid.TranslatePoint(mousePoint, scs.ModifierSurface);

            // Convert the mousePoint.X to x DataValue using axis
            var xDataValue = scs.XAxis.GetDataValue(mousePoint.X);

            // Create a vertical line annotation at the mouse point 
            scs.Annotations.Add(new VerticalLineAnnotation() { X1 = xDataValue});
        }
    }
}
