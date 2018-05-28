using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Sandbox.Examples.SweepingEcgSeries
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

    /// <summary>
    /// Interaction logic for SweepingEcg.xaml
    /// </summary>
    [TestCase("Sweeping ECG Trace")]
    public partial class SweepingEcg : Window
    {
        private double[] _sourceData;
        private DispatcherTimer _timer;
        private const int TimerInterval = 20;
        private int _currentIndex;
        private int _totalIndex;
        private TraceAOrB _whichTrace = TraceAOrB.TraceA;
        private XyzDataSeries<double, double, double> _dataSeriesA;
        private XyzDataSeries<double, double, double> _dataSeriesB;
        private int _dataSeriesIndex;

        private enum TraceAOrB
        {
            TraceA, 
            TraceB,
        }

        public SweepingEcg()
        {
            InitializeComponent();  

            this.Loaded += SweepingEcg_Loaded;
        }

        void SweepingEcg_Loaded(object sender, RoutedEventArgs e)
        {
            // Create an XyzDataSeries to store the X,Y value and Z-value is used to compute an opacity 
            _dataSeriesA = new XyzDataSeries<double, double, double>();

            // Simulate waveform
            _sourceData = LoadWaveformData("Waveform.csv");

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(TimerInterval);
            _timer.Tick += TimerElapsed;
            _timer.Start();

            traceSeriesA.DataSeries = _dataSeriesA;
            traceSeriesB.DataSeries = _dataSeriesB;
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            // This constant is just used to calculate the X (time) values from the point index. 
            // The SampleRate, FIFO size are chosen to get exactly '10 seconds' of data in the viewport
            // The XAxis.VisibleRange is also chosen to be 10 seconds. 
            const double sampleRate = 400;

            // As timer cannot tick quicker than ~20ms, we append 10 points
            // per tick to simulate a sampling frequency of 500Hz (e.g. 2ms per sample)
            for (int i = 0; i < 10; i++)
                AppendPoint(sampleRate);
        }

        private void AppendPoint(double sampleRate)
        {
            if (_currentIndex >= _sourceData.Length)
            {
                _currentIndex = 0;
            }

            // Get the next voltage and time, and append to the chart
            double voltage = _sourceData[_currentIndex];
            double actualTime = (_totalIndex / sampleRate);
            double time = actualTime%10;

            const int MaxDataSeriesCount = 4000;

            if (_dataSeriesA.Count < MaxDataSeriesCount)
            {
                // For the first N points we append time, voltage, actual time
                // Time must be ascending in X for scichart to perform the best, so we clip this to 0-10s
                _dataSeriesA.Append(time, voltage, actualTime);                
            }
            else
            {
                _dataSeriesIndex = _dataSeriesIndex >= MaxDataSeriesCount ? 0 : _dataSeriesIndex;

                // For subsequent points (after reaching the edge of the trace) we wrap traces around
                // We re-use the same data-series just update its Y,Z values then trigger a redraw
                _dataSeriesA.YValues[_dataSeriesIndex] = voltage;
                _dataSeriesA.ZValues[_dataSeriesIndex] = actualTime;
                _dataSeriesA.InvalidateParentSurface(RangeMode.None, hasDataChanged:true);

                //_dataSeriesA.OutputCsv();
            }

            // Update the position of the latest Trace annotation
            latestTrace.X1 = time;
            latestTrace.Y1 = voltage;

            // Update the DataSeries.Tag, used by PaletteProvider to dim the trace as time passes
            _dataSeriesA.Tag = time;

            _currentIndex++;
            _totalIndex++;
            _dataSeriesIndex++;
        }

        private double[] LoadWaveformData(string filename)
        {
            var values = new List<double>();

            // Load the waveform.csv file for the source data 
            var asm = typeof (SweepingEcg).Assembly; 
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
