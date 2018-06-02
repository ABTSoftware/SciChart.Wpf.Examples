using System;
using System.Windows;
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace SciChart.Sandbox.Examples.StripChart
{
    [TestCase("Scrolling Strip Chart")]
    public partial class ScrollingStripChart : Window
    {
        private XyDataSeries<double, double> _dataSeries;
        private Random _random;
        private DateTime? _startTime;

        private double _windowSize = 10;
        private double _timeNow = 0;
        private bool _showLatestWindow = true;
        private bool _thatWasADoubleClick;
        private RelativeTimeLabelProvider _labelProvider;

        public ScrollingStripChart()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // (2): Create a DataSeries and assign to FastLineRenderableSeries
            _dataSeries = new XyDataSeries<double, double>();
            _random = new Random();
            lineSeries.DataSeries = _dataSeries;

            _labelProvider = new RelativeTimeLabelProvider();
            xAxis.LabelProvider = _labelProvider;
            sciChartSurface.InvalidateElement();

            // (6): We subscribe to PreviewMouseDown to set a flag to prevent scrolling calculation in (5)
            sciChartSurface.PreviewMouseDown += (s, arg) =>
            {
                // On mouse down (but not double click), freeze our last N seconds window 
                if (!_thatWasADoubleClick) _showLatestWindow = false;

                _thatWasADoubleClick = false;
            };

            // (7): Subscribe to PreviewMouseDoubleClick to re-enable the auto scrolling window
            sciChartSurface.PreviewMouseDoubleClick += (s, arg) =>
            {
                _showLatestWindow = true;
                _thatWasADoubleClick = true; // (8): Prevent contention between double click and single click event

                // Restore our last N seconds window on double click
                yAxis.AnimateVisibleRangeTo(new DoubleRange(0, 1), TimeSpan.FromMilliseconds(200));
                xAxis.AnimateVisibleRangeTo(new DoubleRange(_timeNow - _windowSize, _timeNow), TimeSpan.FromMilliseconds(200));
            };

            // (3): Create a timer to tick new data 
            var timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerOnTick;
            timer.Start();
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            _timeNow++;

            // (4): Append next sample            
            _dataSeries.Append(_timeNow, _random.NextDouble());

            // Update currentTime in LabelProvider to calculate relative labels
            _labelProvider.CurrentTime = _timeNow;

            // (5): Update visible range if we are in the mode to show the latest window of N seconds
            if (_showLatestWindow)
            {
                xAxis.AnimateVisibleRangeTo(new DoubleRange(_timeNow - _windowSize, _timeNow), TimeSpan.FromMilliseconds(280));
            }
        }
    }
}
