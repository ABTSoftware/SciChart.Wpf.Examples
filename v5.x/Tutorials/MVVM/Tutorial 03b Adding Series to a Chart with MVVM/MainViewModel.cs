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

        public MainViewModel()
        {
            var lineData = new XyDataSeries<double, double>() { SeriesName = "TestingSeries" };
            lineData.Append(0, 0);
            lineData.Append(1, 1);
            lineData.Append(2, 2);

            _renderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();
            RenderableSeries.Add(new LineRenderableSeriesViewModel()
            {
                StrokeThickness = 2,
                Stroke = Colors.SteelBlue,
                DataSeries = lineData,
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
    }
}