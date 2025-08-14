using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D;
using Viewport3D = SciChart.Charting3D.Viewport3D;

namespace SciChart.Examples.Examples.Charts3D.ZoomAndPanA3DChart.ZAxisUp3D
{
    public partial class ZAxisUp3DChart : UserControl
    {
        private readonly Viewport3DOrientation _defaultOrientation;

        public ZAxisUp3DChart()
        {
            // Save the used Viewport orientation before the Example is initialized
            _defaultOrientation = Viewport3D.ViewportOrientation;

            InitializeComponent();

            Unloaded += OnUnLoaded;
        }

        private void OnZUpAxisChecked(object sender, RoutedEventArgs e)
        {
            // Change Viewport orientation
            Viewport3D.SetViewportOrientation(Viewport3DOrientation.ZAxisUp);
        }

        private void OnZUpAxisUnchecked(object sender, RoutedEventArgs e)
        {
            // Change Viewport orientation
            Viewport3D.SetViewportOrientation(Viewport3DOrientation.YAxisUp);
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            // Restore the original Viewport orientation
            Viewport3D.SetViewportOrientation(_defaultOrientation);            
        }
    }
}