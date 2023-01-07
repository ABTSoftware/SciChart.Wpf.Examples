using System;
using System.Windows;
using System.Windows.Controls;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Numerics;
using ChartProviders.Common;
using ChartProviders.Common.DataProviders;
using ChartProviders.Common.TestRunner;

namespace ChartProviderSciChart_v1_7_2
{
    /// <summary>
    /// Interaction logic for LineSeriesAppendingSpeedTest.xaml
    /// </summary>
    public partial class LineSeriesAppendingSpeedTest : UserControl, ISpeedTest
    {
        private ITestRunner _testRunner;
        private XyDataSeries<double, double> _mainSeries = new XyDataSeries<double, double>();
        private XyDataSeries<double, double> _maLowSeries = new XyDataSeries<double, double>();
        private XyDataSeries<double, double> _maHighSeries = new XyDataSeries<double, double>();

        private MovingAverage _maLow = new MovingAverage(20);
        private MovingAverage _maHigh = new MovingAverage(100);

        private RandomWalkGenerator _generator = new RandomWalkGenerator(0);
        private Random _rand = new Random(0);

        private double[] xBuffer;
        private double[] yBuffer;
        private double[] maLowBuffer;
        private double[] maHighBuffer;

        public LineSeriesAppendingSpeedTest()
        {
            InitializeComponent();
        }

        public FrameworkElement Element { get { return this; } }

        public void Execute(TestParameters testParameters, TimeSpan duration, Action<double> fpsResult)
        {
            var testParams = testParameters as LineAppendTestParameters;

            ResamplingMode rMode;
            if (!Enum.TryParse(testParams.SamplingMode.ToString(), out rMode)) rMode = ResamplingMode.MinMax;

            line0.AntiAliasing = testParameters.AntiAliasing;
            line0.ResamplingMode = rMode;

            line1.AntiAliasing = testParameters.AntiAliasing;
            line1.ResamplingMode = rMode;

            line2.AntiAliasing = testParameters.AntiAliasing;
            line2.ResamplingMode = rMode;

            var dataset = new DataSeriesSet<double, double>();
            dataset.Add(_mainSeries);
            dataset.Add(_maLowSeries);
            dataset.Add(_maHighSeries);
            sciChart.DataSet = dataset;

            xBuffer = new double[testParams.IncrementPoints];
            yBuffer = new double[testParams.IncrementPoints];
            maLowBuffer = new double[testParams.IncrementPoints];
            maHighBuffer = new double[testParams.IncrementPoints];

            // Execute
            //

            // Prime the chart with initial points
            using (sciChart.SuspendUpdates())
                AppendData(testParams.PointCount, testParams.Noisyness);

            // Start the test runner 
            _testRunner = new CompositionTestRunner(duration, () => OnAppendData(testParams.Noisyness), fpsResult);
            sciChart.Rendered += _testRunner.OnSciChartRendered;
            _testRunner.Run();
        }

        private void OnAppendData(double noisyness)
        {
            // By nesting multiple updates inside a SuspendUpdates using block, you get one redraw at the end
            using (sciChart.SuspendUpdates())
            {
                AppendData(xBuffer.Length, noisyness);
            }
        }

        private void AppendData(int length, double noisyness)
        {
            int blocks = length / xBuffer.Length;

            for (int j = 0; j < blocks; j++)
            {
                for (int i = 0; i < xBuffer.Length; i++)
                {
                    // Generate a new X,Y value in the random walk and buffer
                    var next = _generator.Next();

                    xBuffer[i] = next.X;
                    yBuffer[i] = next.Y + _rand.NextDouble() * noisyness;

                    // Update moving averages
                    maLowBuffer[i] = _maLow.Push(yBuffer[i]).Current;
                    maHighBuffer[i] = _maHigh.Push(yBuffer[i]).Current;
                }

                // Append block of values to all three series
                _mainSeries.Append(xBuffer, yBuffer);
                _maLowSeries.Append(xBuffer, maLowBuffer);
                _maHighSeries.Append(xBuffer, maHighBuffer);
            }
        }

        public void Dispose()
        {
            if (_mainSeries != null)
            {
                _mainSeries.Clear();
                _mainSeries = null;
            }

            if (_maLowSeries != null)
            {
                _maLowSeries.Clear();
                _maLowSeries = null;
            }

            if (_maHighSeries != null)
            {
                _maHighSeries.Clear();
                _maHighSeries = null;
            }

            _testRunner.Dispose();
        }
    }
}
