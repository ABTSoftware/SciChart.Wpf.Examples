using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Sandbox.Shared;

namespace SciChart.Sandbox.Examples.TouchScreenModifiers
{
    /// <summary>
    /// Interaction logic for TouchInputSandbox.xaml
    /// </summary>
    [TestCase("Touch Input Sandbox")]
    public partial class TouchInputSandbox : Window
    {
        public TouchInputSandbox()
        {
            InitializeComponent();

            // Set a DataSeries of type x=DateTime, y0=Double, y1=double on the RenderableSeries declared in XAML
            var series = new XyyDataSeries<double, double>();
            scs2D.RenderableSeries[0].DataSeries = series;

            // Get some data for the upper and lower band
            var data = DataManager.Instance.GetDampedSinewave(1.0, 0.01, 1000);
            var moreData = DataManager.Instance.GetDampedSinewave(1.0, 0.005, 1000, 12);

            // Append data to series. SciChart automatically redraws
            series.Append(
                data.XData,
                data.YData,
                moreData.YData);
        }
    }
}
