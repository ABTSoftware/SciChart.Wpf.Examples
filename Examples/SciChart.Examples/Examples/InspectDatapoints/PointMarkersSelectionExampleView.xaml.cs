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
            var dataSeries1 = new UniformXyDataSeries<double>(0d, 0.05) { SeriesName = "Green" };
            var dataSeries2 = new UniformXyDataSeries<double>(0d, 0.05) { SeriesName = "Blue" };
            var dataSeries3 = new UniformXyDataSeries<double>(0d, 0.05) { SeriesName = "Yellow" };

            // Attach DataSeries to RenderableSeries
            lineRenderSeries1.DataSeries = dataSeries1;
            lineRenderSeries2.DataSeries = dataSeries2;
            lineRenderSeries3.DataSeries = dataSeries3;

            // Generate data
            var count = 200;
            var data1 = DataManager.Instance.GetSinewaveYData(100, 55, count);
            var data3 = DataManager.Instance.GetSinewaveYData(50, 20, count);
            
            // Append data to series
            using (sciChart.SuspendUpdates())
            {
                dataSeries1.Append(data1, Enumerable.Range(0, count).Select(i => new MyMetadata()));
                dataSeries3.Append(data3, Enumerable.Range(0, count).Select(i => new MyMetadata()));

                for (int i = 0; i < count; i += 4)
                {
                    dataSeries2.Append(i++, new MyMetadata());
                }
            }

            // Zoom out to the extents of data
            sciChart.ZoomExtents();
        }
    }
}