using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    public class MyMetadata : IPointMetadata
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsSelected { get; set; }
    }

    public partial class PointMarkersSelectionExampleView : UserControl
    {
        public PointMarkersSelectionExampleView()
        {
            InitializeComponent();
        }

        private void PointMarkersSelectionExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Create a DataSeries of type X=double, Y=double
            var dataSeries1 = new XyDataSeries<double, double> {SeriesName = "Green"};
            var dataSeries2 = new XyDataSeries<double, double> {SeriesName = "Blue"};
            var dataSeries3 = new XyDataSeries<double, double> {SeriesName = "Yellow"};

            // Attach DataSeries to RenderableSeries
            lineRenderSeries1.DataSeries = dataSeries1;
            lineRenderSeries2.DataSeries = dataSeries2;
            lineRenderSeries3.DataSeries = dataSeries3;

            // Generate data
            var count = 200;
            var data1 = DataManager.Instance.GetSinewave(100, 55, count);
            var data3 = DataManager.Instance.GetSinewave(50, 20, count);
            
            // Append data to series
            using (sciChart.SuspendUpdates())
            {
                dataSeries1.Append(data1.XData, data1.YData,
                    data1.XData.Select((d) => new MyMetadata {IsSelected = false}));
                dataSeries3.Append(data3.XData, data3.YData,
                    data3.XData.Select((d) => new MyMetadata {IsSelected = false}));

                for (int i = 0; i < count; i += 4)
                {
                    dataSeries2.Append(data1.XData[i], i++, new MyMetadata {IsSelected = false});
                }
            }

            // Zoom out to the extents of data
            sciChart.ZoomExtents();
        }
    }
}
