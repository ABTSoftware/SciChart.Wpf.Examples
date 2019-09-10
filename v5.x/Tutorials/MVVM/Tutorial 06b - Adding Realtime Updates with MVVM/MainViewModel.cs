using System.Collections.ObjectModel;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace SciChart.Mvvm.Tutorial
{
    public class MainViewModel : BindableObject
    {
        private string _chartTitle = "Hello SciChart World!";
        private string _xAxisTitle = "XAxis";
        private string _yAxisTitle = "YAxis";
        private ObservableCollection<IRenderableSeriesViewModel> _renderableSeries;
        private bool _enablePan;
        private bool _enableZoom = true;

        public MainViewModel()
        {
            var dummyDataProvider = new DummyDataProvider();
            var lineData = new XyDataSeries<double, double>() { SeriesName = "TestingSeries" };

            _renderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();
            RenderableSeries.Add(new LineRenderableSeriesViewModel()
            {
                StrokeThickness = 2,
                Stroke = Colors.SteelBlue,
                DataSeries = lineData,
                StyleKey = "LineSeriesStyle"
            });

            // Append the initial values to the chart
            var initialDataValues = dummyDataProvider.GetHistoricalData();
            lineData.Append(initialDataValues.XValues, initialDataValues.YValues);

            // Subscribe to future updates
            dummyDataProvider.SubscribeUpdates((newValues) =>
            {
                // Append when new values arrive
                lineData.Append(newValues.XValues, newValues.YValues);
                // Zoom the chart to fit
                lineData.InvalidateParentSurface(RangeMode.ZoomToFit);
            });

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