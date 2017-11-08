using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace SciChart.Mvvm.Tutorial
{
    public class ChartViewModel : BindableObject
    {
        private readonly IDataProvider _dataProvider;
        private string _chartTitle;
        private readonly ObservableCollection<IRenderableSeriesViewModel> _renderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();
        private readonly ObservableCollection<IAxisViewModel> _yAxes = new ObservableCollection<IAxisViewModel>();
        private readonly ObservableCollection<IAxisViewModel> _xAxes = new ObservableCollection<IAxisViewModel>();
        private readonly ObservableCollection<IAnnotationViewModel> _annotations = new ObservableCollection<IAnnotationViewModel>();

        private XyDataSeries<double, double> _lineData;        

        public ChartViewModel(IDataProvider dataProvider, string chartTitle, string mouseEventGroupId)
        {
            _dataProvider = dataProvider;
            _chartTitle = chartTitle;
            MouseEventGroup = mouseEventGroupId;

            CreateChartData();
            CreateChartSeries();
            CreateChartAxis();

            // Subscribe to future updates
            int i = 0;
            _dataProvider.SubscribeUpdates((newValues) =>
            {
                // Append when new values arrive
                _lineData.Append(newValues.XValues, newValues.YValues);
                // Zoom the chart to fit
                _lineData.InvalidateParentSurface(RangeMode.ZoomToFit);

                // Every 100th datapoint, add an annotation
                if (i % 100 == 0)
                {
                    Annotations.Add(new InfoAnnotationViewModel() { X1 = _lineData.XValues.Last(), Y1 = 0.0 });
                }
                i++;
            });
        }

        public string MouseEventGroup { get; set; }

        public ObservableCollection<IAxisViewModel> YAxes { get { return _yAxes; } }

        public ObservableCollection<IAxisViewModel> XAxes { get { return _xAxes; } }

        public ObservableCollection<IAnnotationViewModel> Annotations { get { return _annotations; } }

        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries { get { return _renderableSeries; } }

        private void CreateChartAxis()
        {
            YAxes.Add(new NumericAxisViewModel()
            {
                AutoRange = AutoRange.Always,
                AxisTitle = "Left YAxis",
                Id = "LeftYAxis",
                AxisAlignment = AxisAlignment.Left,
            });

            YAxes.Add(new NumericAxisViewModel()
            {
                AutoRange = AutoRange.Always,
                AxisTitle = "Right YAxis",
                AxisAlignment = AxisAlignment.Right,
            });

            XAxes.Add(new NumericAxisViewModel()
            {
                AutoRange = AutoRange.Always,
                AxisTitle = "XAxis",
                AxisAlignment = AxisAlignment.Bottom,
            });
        }

        private void CreateChartData()
        {
            var initialDataValues = _dataProvider.GetHistoricalData();

            // Create a DataSeries. We later apply this to a RenderableSeries
            _lineData = new XyDataSeries<double, double>() { SeriesName = _chartTitle + " Line Series" };

            // Append some data to the chart                                 
            _lineData.Append(initialDataValues.XValues, initialDataValues.YValues);

        }

        private void CreateChartSeries()
        {
            // Create a RenderableSeries. Apply the DataSeries created before 
            RenderableSeries.Add(new LineRenderableSeriesViewModel()
            {
                StrokeThickness = 2,
                Stroke = Colors.SteelBlue,
                DataSeries = _lineData,
                StyleKey = "LineSeriesStyle"
            });
        }                

        public string ChartTitle
        {
            get { return _chartTitle; }
            set
            {
                _chartTitle = value;
                OnPropertyChanged("ChartTitle");
            }
        }
    }
}