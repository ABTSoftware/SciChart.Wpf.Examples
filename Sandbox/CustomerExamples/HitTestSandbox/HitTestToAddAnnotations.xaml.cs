using System.Windows;
using System.Windows.Input;
using SciChart.Charting.Visuals.Annotations;

namespace HitTestSandboxExample
{
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
