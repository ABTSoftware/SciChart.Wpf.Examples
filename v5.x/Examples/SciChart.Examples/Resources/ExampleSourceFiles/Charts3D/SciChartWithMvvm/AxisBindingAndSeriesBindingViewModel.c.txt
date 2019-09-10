using System.Collections.ObjectModel;
using System.Windows.Media;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.Charts3D.SciChartWithMvvm
{
    public  class AxisBindingAndSeriesBindingViewModel : BaseViewModel
    {
        public AxisBindingAndSeriesBindingViewModel()
        {
            YAxis = new TimeSpanAxis3DViewModel { StyleKey = "CustomTimeSpan3DStyle", FontFamily = new FontFamily("Broadway")};
            XAxis = new DateTimeAxis3DViewModel() { StyleKey = "CustomDateTime3DStyle", FontFamily = new FontFamily("Comic sans ms") };
            ZAxis = new NumericAxis3DViewModel { StyleKey = "CustomNumeric3DStyle" };

        }


        public AxisBase3DViewModel XAxis { get; set; }

        public AxisBase3DViewModel YAxis { get; set; }

        public AxisBase3DViewModel ZAxis { get; set; }
    }
}
