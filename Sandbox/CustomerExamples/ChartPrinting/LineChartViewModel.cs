using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Numerics;

namespace ChartPrintingMultiPaneChartsExample
{
    public class LineChartViewModel : INotifyPropertyChanged
    {
        public LineChartViewModel()
        {
            _chartDataseries.Add(
                new LineRenderableSeriesViewModel()
                {
                    Stroke = Colors.Green,
                    StrokeDashArray = null,
                    DrawNaNAs = LineDrawMode.ClosedLines,
                    ResamplingMode = ResamplingMode.Auto
                }
            );
        }

        private readonly IList<IRenderableSeriesViewModel> _chartDataseries = new List<IRenderableSeriesViewModel>();

        public XyDataSeries<double, double> ChartData
        {
            get => (XyDataSeries<double, double>)_chartDataseries [0].DataSeries;
            set => _chartDataseries[0].DataSeries = value;
        }

        public IList<IRenderableSeriesViewModel> RenderableSeries
        {
            get { return _chartDataseries; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}