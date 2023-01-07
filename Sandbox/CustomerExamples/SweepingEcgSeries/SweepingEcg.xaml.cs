using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using SciChart.Charting.Model.DataSeries;
using Timer = System.Timers.Timer;

namespace SweepingEcgExample
{
    public static class DataSeriesExtensions
    {
        public static void OutputCsv(this XyzDataSeries<double, double, double> s)
        {
            for (int i = 0; i < s.Count; i++)
            {
                Console.WriteLine("{0},{1},{2}", s.XValues[i], s.YValues[i], s.ZValues[i]);
            }
        }
    }

    public partial class SweepingEcg : Window
    {
        private const int TimerInterval = 20;
        private const int MaxDataSeriesCount = 4000;

        private int _currentIndex;
        private int _dataSeriesIndex;
        private int _totalIndex;

        private Timer _timer;
        private readonly object _timerLock = new object();
        private readonly SynchronizationContext _syncContext;

        private double[] _sourceData;
        private XyzDataSeries<double, double, double> _dataSeries;

        public SweepingEcg()
        {
            InitializeComponent();

            Loaded += SweepingEcg_Loaded;

            _syncContext = SynchronizationContext.Current;
        }

        void SweepingEcg_Loaded(object sender, RoutedEventArgs e)
        {
            // Create an XyzDataSeries to store the X,Y value and Z-value is used to compute an opacity 
            _dataSeries = new XyzDataSeries<double, double, double>();

            // Simulate waveform
            _sourceData = LoadWaveformData("Waveform.csv");

            _timer = new Timer(TimerInterval) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();

            traceSeries.DataSeries = _dataSeries;
            traceSeries.PaletteProvider = new DimTracePaletteProvider(MaxDataSeriesCount);
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            lock (_timerLock)
            {
                // This constant is just used to calculate the X (time) values from the point index. 
                // The SampleRate, FIFO size are chosen to get exactly '10 seconds' of data in the viewport
                // The XAxis.VisibleRange is also chosen to be 10 seconds. 
                const double sampleRate = 400;

                // As timer cannot tick quicker than ~20ms, we append 10 points
                // per tick to simulate a sampling frequency of 500Hz (e.g. 2ms per sample)
                for (int i = 0; i < 10; i++)
                {
                    AppendPoint(sampleRate);
                }
            }
        }

        private void AppendPoint(double sampleRate)
        {
            if (_currentIndex >= _sourceData.Length)
            {
                _currentIndex = 0;
            }

            // Get the next voltage and time, and append to the chart
            double voltage = _sourceData[_currentIndex];
            double time = _totalIndex / sampleRate % 10;

            // Update the DataSeries.Tag, used by PaletteProvider to dim the trace
            _dataSeries.Tag = _totalIndex;

            if (_dataSeries.Count < MaxDataSeriesCount)
            {
                // For the first N points we append time, voltage, actual time
                // Time must be ascending in X for scichart to perform the best, so we clip this to 0-10s
                _dataSeries.Append(time, voltage, _totalIndex);

            }
            else
            {
                _dataSeriesIndex = _dataSeriesIndex >= MaxDataSeriesCount ? 0 : _dataSeriesIndex;

                // For subsequent points (after reaching the edge of the trace) we wrap traces around
                // We re-use the same data-series just update its Y,Z values then trigger a redraw
                _dataSeries.YValues[_dataSeriesIndex] = voltage;
                _dataSeries.ZValues[_dataSeriesIndex] = _totalIndex;

                _dataSeries.InvalidateParentSurface(RangeMode.None, true);
            }

            // Update the position of the latest Trace annotation
            _syncContext.Post(_ =>
            {
                latestTrace.X1 = time;
                latestTrace.Y1 = voltage;

            }, null);

            _currentIndex++;
            _totalIndex++;
            _dataSeriesIndex++;
        }

        private double[] LoadWaveformData(string filename)
        {
            var values = new List<double>();

            // Load the waveform.csv file for the source data 
            var asm = typeof(SweepingEcg).Assembly;
            var resourceString = asm.GetManifestResourceNames().Single(x => x.Contains(filename));

            using (var stream = asm.GetManifestResourceStream(resourceString))
            using (var streamReader = new StreamReader(stream))
            {
                string line = streamReader.ReadLine();
                while (line != null)
                {
                    values.Add(double.Parse(line, NumberFormatInfo.InvariantInfo));
                    line = streamReader.ReadLine();
                }
            }

            return values.ToArray();
        }
    }
}