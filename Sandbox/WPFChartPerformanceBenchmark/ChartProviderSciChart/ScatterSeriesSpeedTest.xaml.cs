using System;
using System.Windows;
using System.Windows.Controls;
using Abt.Controls.SciChart.Numerics;
using ChartProviders.Common;
using ChartProviders.Common.DataProviders;
using ChartProviders.Common.TestRunner;
using Abt.Controls.SciChart;

namespace ChartProviderSciChart_v1_7_2
{
    /// <summary>
    /// Interaction logic for SciChartLineSeries.xaml
    /// </summary>
    public partial class SciChartScatterSeries : UserControl, ISpeedTest
    {
        private XyDataSeries<double, double> _xyDataSeries;
        private RandomPointsGenerator _generator;
        private ITestRunner _testRunner;

        public SciChartScatterSeries()
        {
            InitializeComponent();
        }

        public FrameworkElement Element { get { return this; } }

        public void Execute(TestParameters testParameters, TimeSpan duration, Action<double> fpsResult)
        {
            // Setup
            var dataset = new DataSeriesSet<double, double>();
            _xyDataSeries = new XyDataSeries<double, double>();
            dataset.Add(_xyDataSeries);

            sciChart.DataSet = dataset;

			this.scatterSeries.AntiAliasing = testParameters.AntiAliasing;
			ResamplingMode resamplingMode; if (!Enum.TryParse(testParameters.SamplingMode.ToString(), out resamplingMode)) resamplingMode = ResamplingMode.None;
			this.scatterSeries.ResamplingMode = resamplingMode;

			_generator = testParameters.DataDistribution == DataDistribution.Uniform ? (RandomPointsGenerator)new BrownianMotionPointsGenerator(0, 100, -50, 50) : new CorrelatedDataPointsGenerator(0, 100, -50, 50);
			
            sciChart.XAxis.VisibleRange = new DoubleRange(0, 100);
            sciChart.YAxis.VisibleRange = new DoubleRange(-50, 50);
            var initialData = _generator.GetRandomPoints(testParameters.PointCount);
            _xyDataSeries.Append(initialData.XData, initialData.YData);

            // Execute
            _testRunner = new DispatcherTimerRunner(duration, () => OnAppendData(testParameters.PointCount), fpsResult);
            sciChart.Rendered += _testRunner.OnSciChartRendered;
            _testRunner.Run();
        }

        private void OnAppendData(int pointCount)
        {
            using (sciChart.SuspendUpdates())
            {
                var next = _generator.GetRandomPoints(pointCount);
                _xyDataSeries.Clear();
                _xyDataSeries.Append(next.XData, next.YData);
            }
        }

        public void Dispose()
        {
            if (_xyDataSeries != null)
            {
                _xyDataSeries.Clear();
                _xyDataSeries = null;
            }

            _testRunner.Dispose();
        }
    }
}
