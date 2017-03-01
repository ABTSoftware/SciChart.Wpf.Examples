using System;
using System.Timers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.PerformanceDemos2D.AppendMillions
{
    public class AppendMillionsPageViewModel : BaseViewModel
    {
        private readonly string _exampleTitle;

        private MovingAverage _maLow;
        private MovingAverage _maHigh;

        private readonly Random _random = new Random();

        private const int MaxCount = 10000000; // Max number of points to draw before demo stops
        private const int BufferSize = 1000; // Number of points to append to each channel each tick
        private const int TimerInterval = 20; // Interval of the timer to generate data in ms

        // X, Y buffers used to buffer data into the Scichart in blocks of BufferSize
        // This gives an increase in rendering throughput over one off appends of X, Y points
        private readonly int[] _xBuffer = new int[BufferSize];
        private readonly double[] _yBuffer = new double[BufferSize];
        private readonly double[] _maLowBuffer = new double[BufferSize];
        private readonly double[] _maHighBuffer = new double[BufferSize];

        private Timer _timer;
        private IXyDataSeries<int, double> _mainSeries;
        private IXyDataSeries<int, double> _maLowSeries;
        private IXyDataSeries<int, double> _maHighSeries;
        private AutoRange _boundRange;
        private string _exampleSubtitle;

        private object _syncRoot = new object();

        public AppendMillionsPageViewModel()
        {
            RunExampleCommand = new ActionCommand(OnRunExample);
        }

        public ActionCommand RunExampleCommand { get; private set; }

        public IXyDataSeries<int, double> MainSeries
        {
            get { return _mainSeries; }
            set
            {
                if (_mainSeries == value) return;
                _mainSeries = value;
                OnPropertyChanged("MainSeries");
            }
        }

        public IXyDataSeries<int, double> MaLowSeries
        {
            get { return _maLowSeries; }
            set
            {
                if (_maLowSeries == value) return;
                _maLowSeries = value;
                OnPropertyChanged("MaLowSeries");
            }
        }

        public IXyDataSeries<int, double> MaHighSeries
        {
            get { return _maHighSeries; }
            set
            {
                if (_maHighSeries == value) return;
                _maHighSeries = value;
                OnPropertyChanged("MaHighSeries");
            }
        }

        public AutoRange BoundRange
        {
            get { return _boundRange; }
            set
            {
                if (_boundRange == value) return;
                _boundRange = value;
                OnPropertyChanged("BoundRange");
            }
        }

        public void OnPageExit()
        {
            if (_timer != null)
            {
                lock (_syncRoot)
                {
                    _timer.Stop();
                    _timer = null;
                }

                // Reset all variables
                MaHighSeries = null;
                MaLowSeries = null;
                MainSeries = null;
            }
        }

        private void OnRunExample()
        {
            // Reset all variables
            _reached = 0;
            MaHighSeries = new XyDataSeries<int, double>();
            MaLowSeries = new XyDataSeries<int, double>();
            MainSeries = new XyDataSeries<int, double>();

            _maLow = new MovingAverage(200);
            _maHigh = new MovingAverage(1000);

            BoundRange = AutoRange.Always;

            _timer = new Timer(TimerInterval);
            _timer.Elapsed += OnTick;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Ensure only one timer Tick processed at a time
            lock (_syncRoot)
            {
                DataAppendLoop();
            }
        }

        private int _reached;

        private void DataAppendLoop()
        {
            // if timer was released in another thread - skip all further processing
            if (_timer == null) return;

            // By nesting multiple updates inside a SuspendUpdates using block, you get one redraw at the end
            using (_mainSeries.SuspendUpdates())
            {
                // Preload previous value with k-1 sample, or 0.0 if the count is zero
                int xValue = _mainSeries.Count > 0 ? _mainSeries.XValues[_mainSeries.Count - 1] : 0;
                double yValue = _mainSeries.Count > 0 ? _mainSeries.YValues[_mainSeries.Count - 1] : 10.0f;

                // Add N points at a time. We want to get to the higher point counts 
                // quickly to demonstrate performance. 
                // Also, it is more efficient to buffer and block update the chart
                // even if you use SuspendUpdates due to the overhead of calculating min, max
                // for a series
                for (int i = 0; i < BufferSize; i++)
                {
                    // Generate a new X,Y value in the random walk and buffer
                    xValue = xValue + 1;
                    yValue = (double) (yValue + (_random.NextDouble() - 0.5));

                    _xBuffer[i] = xValue;
                    _yBuffer[i] = yValue;

                    // Update moving averages
                    _maLowBuffer[i] = (double) _maLow.Push(yValue).Current;
                    _maHighBuffer[i] = (double) _maHigh.Push(yValue).Current;
                }

                // Append block of values to all three series
                _mainSeries.Append(_xBuffer, _yBuffer);
                _maLowSeries.Append(_xBuffer, _maLowBuffer);
                _maHighSeries.Append(_xBuffer, _maHighBuffer);
            }

            // Render the total point count (all series) to the screen
            int numPoints = 3*_mainSeries.Count;

            if (numPoints > 10000 && _reached < 10000)
            {
                _reached = 10000;
            }

            if (numPoints > 100000 && _reached < 100000)
            {
                _reached = 100000;
            }

            if (numPoints > 300000 && _reached < 300000)
            {
                _reached = 300000;
            }

            if (numPoints > 500000 && _reached < 500000)
            {
                _reached = 500000;
            }

            if (numPoints > 1000000 && _reached < 1000000)
            {
                _reached = 1000000;
            }

            if (numPoints > 3000000 && _reached < 3000000)
            {
                Messages.Add("Reached 3,000,000 Points!");
                _reached = 3000000;
            }

            if (numPoints > 5000000 && _reached < 5000000)
            {
                Messages.Add("Reached 5,000,000 Points!");
                _reached = 5000000;
            }

            if (numPoints > MaxCount)
            {
                Messages.Add("Reached 10M points!");
                Messages.Add(".. and I'm still going!");
                _timer.Stop();
                _timer = null;
                BoundRange = AutoRange.Never;

                this.ShowInstructionsToUser();
            }
        }
    }
}