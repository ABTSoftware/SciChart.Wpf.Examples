using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Annotations;
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

        private ObservableCollection<IAnnotationViewModel> _annotations = new ObservableCollection<IAnnotationViewModel>();
        private ObservableCollection<IRenderableSeriesViewModel> _renderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();

        public MainViewModel()
        {
            CreateChartData();
            CreateChartSeries();
            
            // Subscribe to future updates
            int i = 0;
            _dummyDataProvider.SubscribeUpdates(newValues =>
            {
                // Append when new values arrive
                _lineData.Append(newValues.XValues, newValues.YValues);

                // Every 100th data point, add an annotation
                if (i % 100 == 0)
                {
                    Annotations.Add(new InfoAnnotationViewModel
                    {
                        X1 = _lineData.XValues.Last(),
                        Y1 = 0.0,

                        // Specify that the point (X1, Y1) is located at the bottom-center of the annotation
                        VerticalAnchorPoint=VerticalAnchorPoint.Bottom,
                        HorizontalAnchorPoint=HorizontalAnchorPoint.Center,
                    });
                }
                i++;
            });
        }

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