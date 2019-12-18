using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Wpf.TestSuite.TestCases.StripChart;
using SciChart.Wpf.UI.Reactive;

namespace SciChart.Wpf.TestSuite.ExampleSandbox.StripChart_And_SciChartOverViewMVVM
{
    public class StripChartViewModel : BaseViewModel
    {
        private XyDataSeries<double, double> _dataSeries;

        private Random _random;

        private double _windowSize = 10;
        private double _timeNow = 0;
        private bool _showLatestWindow = true;
        private RelativeTimeLabelProvider _labelProvider;
        private DoubleRange _tDoubleRange;

        public ObservableCollection<IAxisViewModel> XAxes { get; set; }
        public ObservableCollection<IAxisViewModel> YAxes { get; set; }
        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeriesViewModels { get; set; }

        public XyDataSeries<double, double> DataSeries
        {
            get { return _dataSeries; }
            set
            {
                _dataSeries = value;
                OnPropertyChanged("DataSeries");
            }
        }

        public DoubleRange TDoubleRange
        {
            get { return _tDoubleRange; }
            set 
            { 
                _tDoubleRange = value;
                OnPropertyChanged("TDoubleRange");
            }
        }

        public ICommand StartStopCommand { get; set; }

        public StripChartViewModel()
        {
            // (2): Create a DataSeries and assign to FastLineRenderableSeries
            _dataSeries = new XyDataSeries<double, double>();
            _random = new Random();

            XAxes = new ObservableCollection<IAxisViewModel>();
            YAxes = new ObservableCollection<IAxisViewModel>();
            RenderableSeriesViewModels = new ObservableCollection<IRenderableSeriesViewModel>();
            InitializeAxes();
            InitializeRenderableSeries();

            _labelProvider = new RelativeTimeLabelProvider();

            // (3): Create a timer to tick new data 
            var timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerOnTick;
            timer.Start();
            StartStopCommand = new ActionCommand(() =>
            {
                timer.Stop();
            });
        }

        private void InitializeAxes()
        {
            var xNumAxis = new NumericAxisViewModel
            {
                AxisAlignment = AxisAlignment.Bottom,
                AxisTitle = "XAxis",
                DrawMajorBands = false,
                TextFormatting = "0.00#",
                BorderThickness = new Thickness(3),
                BorderBrush = new SolidColorBrush(Colors.CadetBlue),
                LabelProvider = _labelProvider,
            };
            XAxes.Add(xNumAxis);

            var yNumAxis = new NumericAxisViewModel
            {
                AxisTitle = "YAxis",
                DrawMajorBands = false,
                TextFormatting = "0.0#",
                VisibleRange = new DoubleRange(0, 1)
            };

            YAxes.Add(yNumAxis);
        }

        private void InitializeRenderableSeries()
        {
            var lineSeries = new LineRenderableSeriesViewModel
            {
                DataSeries = DataSeries,
                StrokeThickness = 2, 
                Stroke = Colors.Red,
            };
            RenderableSeriesViewModels.Add(lineSeries);
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            _timeNow++;

            // Append next sample
            _labelProvider.CurrentTime = _timeNow;
            //_dataSeries.Append(_timeNow, _random.NextDouble());
            DataSeries.Append(_timeNow, _random.NextDouble());

            // Update visible range if we are in the mode to show the latest window of N seconds
            if (_showLatestWindow && _timeNow >= 10)
            {
                //XAxes[0].VisibleRange = new DoubleRange(_timeNow - _windowSize, _timeNow);
                TDoubleRange = new DoubleRange(0, 1);
                XAxes[0].VisibleRange = new DoubleRange(_timeNow - _windowSize, _timeNow);
            }
        }
    }
}
