using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D;
using Viewport3D = SciChart.Charting3D.Viewport3D;

namespace SciChart.Examples.Examples.Charts3D.ZoomAndPanA3DChart.ZAxisUp3D
{
    /// <summary>
    /// Interaction logic for ZAxisUp3DChart.xaml
    /// </summary>
    public partial class ZAxisUp3DChart : UserControl
    {
        public ZAxisUp3DChart()
        {
            InitializeComponent();

            // Clicking this button changes Viewport orientation
            zUpAxisToggleButton.IsChecked = true;

            Unloaded += OnUnLoaded;
        }

        private void ZUpAxisToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            // Change Viewport orientation
            Viewport3D.SetViewportOrientation(Viewport3DOrientation.ZAxisUp);
        }

        private void ZUpAxisToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            // Change Viewport orientation
            Viewport3D.SetViewportOrientation(Viewport3DOrientation.YAxisUp);
        }
        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            if (zUpAxisToggleButton.IsChecked == true)
            {
                Viewport3D.SetViewportOrientation(Viewport3DOrientation.YAxisUp);
            }
        }
    }
}
