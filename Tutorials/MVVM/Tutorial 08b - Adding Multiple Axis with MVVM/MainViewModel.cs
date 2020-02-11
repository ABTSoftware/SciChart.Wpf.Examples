using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace SciChart.Mvvm.Tutorial
{
    public class MainViewModel : BindableObject
    {
        private string _chartTitle = "Hello SciChart World!";
        private string _xAxisTitle = "XAxis";
        private string _yAxisTitle = "YAxis";

        private bool _enablePan;
        private bool _enableZoom = true;

        private XyDataSeries<double, double> _lineData;
        private readonly DummyDataProvider _dummyDataProvider = new DummyDataProvider();
        private AutoRangeViewportManager _viewportManager = new AutoRangeViewportManager();

        private ObservableCollection<IRenderableSeriesViewModel> _renderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();
        private ObservableCollection<IAnnotationViewModel> _annotations = new ObservableCollection<IAnnotationViewModel>();

        private readonly ObservableCollection<IAxisViewModel> _yAxes = new ObservableCollection<IAxisViewModel>();
        private readonly ObservableCollection<IAxisViewModel> _xAxes = new ObservableCollection<IAxisViewModel>();

        public MainViewModel()
        {
            CreateChartData();
            CreateChartSeries();
            CreateChartAxis();
            
            // Subscribe to future updates
            int i = 0;

            _dummyDataProvider.SubscribeUpdates(newValues =>
            {
                // Append when new values arrive
                _lineData.Append(newValues.XValues, newValues.YValues);

                // Every 100th datapoint, add an annotation
                if (i % 100 == 0)
                {
                    Annotations.Add(new InfoAnnotationViewModel
                    {
                        X1 = _lineData.XValues.Last(),
                        Y1 = 0.0,
                        YAxisId = "LeftYAxis"
                    });                  
                }
                i++;
            }); 
        }

        private void CreateChartAxis()
        {
            YAxes.Add(new NumericAxisViewModel
            {
                AutoRange = AutoRange.Always,
                AxisTitle = "Left YAxis",
                Id = "LeftYAxis",
                AxisAlignment = AxisAlignment.Left
            });

            YAxes.Add(new NumericAxisViewModel
            {
                AutoRange = AutoRange.Always,
                AxisTitle = "Right YAxis",
                AxisAlignment = AxisAlignment.Right
            });

            XAxes.Add(new NumericAxisViewModel
            {
                AutoRange = AutoRange.Once,
                AxisTitle = "XAxis",
                AxisAlignment = AxisAlignment.Bottom
            });
        }      
        
        public ObservableCollection<IAxisViewModel> YAxes { get { return _yAxes; } }
        
        public ObservableCollection<IAxisViewModel> XAxes { get { return _xAxes; } }

        private void CreateChartData()
        {
            var initialDataValues = _dummyDataProvider.GetHistoricalData();

            // Create a DataSeries. We later apply this to a RenderableSeries
            _lineData = new XyDataSeries<double, double> { SeriesName = "TestingSeries" };

            // Append some data to the chart                                 
            _lineData.Append(initialDataValues.XValues, initialDataValues.YValues);
        }

        private void CreateChartSeries()
        {
            // Create a RenderableSeries. Apply the DataSeries created before 
            _renderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();
            RenderableSeries.Add(new LineRenderableSeriesViewModel
            {
                StrokeThickness = 2,
                Stroke = Colors.SteelBlue,
                DataSeries = _lineData,
                StyleKey = "LineSeriesStyle"
            });
        }

        public ObservableCollection<IAnnotationViewModel> Annotations
        {
            get { return _annotations; }
            set
            {
                _annotations = value;
                OnPropertyChanged("Annotations");
            }
        }

        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries
        {
            get { return _renderableSeries; }
            set
            {
                _renderableSeries = value;
                OnPropertyChanged("RenderableSeries");
            }
        }

        public AutoRangeViewportManager ViewportManager
        {
            get { return _viewportManager; }
            set
            {
                _viewportManager = value;
                OnPropertyChanged("ViewportManager");
            }
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

        public string XAxisTitle
        {
            get { return _xAxisTitle; }
            set
            {
                _xAxisTitle = value;
                OnPropertyChanged("XAxisTitle");
            }
        }

        public string YAxisTitle
        {
            get { return _yAxisTitle; }
            set
            {
                _yAxisTitle = value;
                OnPropertyChanged("YAxisTitle");
            }
        }
        
        public bool EnableZoom
        {
            get { return _enableZoom; }
            set
            {
                if (_enableZoom != value)
                {
                    _enableZoom = value;
                    OnPropertyChanged("EnableZoom");
                    if (_enableZoom) EnablePan = false;
                }
            }
        }
        
        public bool EnablePan
        {
            get { return _enablePan; }
            set
            {
                if (_enablePan != value)
                {
                    _enablePan = value;
                    OnPropertyChanged("EnablePan");
                    if (_enablePan) EnableZoom = false;
                }
            }
        }
    }
}