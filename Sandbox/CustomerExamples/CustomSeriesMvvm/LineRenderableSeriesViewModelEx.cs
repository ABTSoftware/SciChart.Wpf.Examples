using System;
using SciChart.Charting.Model.ChartSeries;

namespace CustomSeriesMvvmExample
{
    /// <summary>
    /// The ViewModel type for your custom series, which inherits from one of our series, or CustomRenderableSeriesViewModel
    /// </summary>
    public class LineRenderableSeriesViewModelEx : LineRenderableSeriesViewModel
    {
        // Specify what Series type to create for this viewmodel
        public override Type ViewType => typeof(FastLineRenderableSeriesEx);
    }
}