using System;
using System.Windows;
using System.Windows.Controls;
using ChartProviders.Common.DataProviders;
using ChartProviders.Common.Interfaces;
using ChartProviders.Common.Models;
using ChartProviders.Common.TestRunners;
using SciChart.Charting.Common.AttachedProperties;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace ChartProviders.SciChartTests
{
    public partial class ScatterSeriesSpeedTest : UserControl, ISpeedTest
    {
        private XyDataSeries<double, double> _xyDataSeries;
        private RandomPointsGenerator _generator;
        private ITestRunner _testRunner;

        private static IDataDistributionCalculator<double, double> d = new UserDefinedDistributionCalculator<double, double>()
        {
            IsEvenlySpaced = false,
            IsSortedAscending = false,
            ContainsNaN = false
        };

        public ScatterSeriesSpeedTest()
        {
            InitializeComponent();
            sciChart.Loaded += (s, e) =>
            {
                infoTextAnnotation.Text = string.Format("Renderer: {0}, Enable C++ Resamplers: {1}, Extreme Drawing: {2}", sciChart.RenderSurface.GetType().Name, PerformanceHelper.GetEnableExtremeResamplers(sciChart), PerformanceHelper.GetEnableExtremeDrawingManager(sciChart));
            };
        }

        public FrameworkElement Element => this;

        public void Execute(TestParameters testParameters, TimeSpan duration, Action<double> fpsResult)
        {
            // Setup
            _xyDataSeries = new XyDataSeries<double, double>() { AcceptsUnsortedData = true, DataDistributionCalculator = d };

            scatterSeries.DataSeries = _xyDataSeries;
            scatterSeries.AntiAliasing = testParameters.AntiAliasing;

            _generator = testParameters.DataDistribution == DataDistribution.Uniform ? (RandomPointsGenerator)new BrownianMotionPointsGenerator(0, 100, -50, 50) : new CorrelatedDataPointsGenerator(0, 100, -50, 50);

            sciChart.XAxis.VisibleRange = new DoubleRange(0, 100);
            sciChart.YAxis.VisibleRange = new DoubleRange(-50, 50);

            var initialData = _generator.GetRandomPoints(testParameters.PointCount);
            _xyDataSeries.Append(initialData.XData, initialData.YData);

            // Execute
            if (testParameters.TestRunner == TestRunnerType.Composition)
            {
                _testRunner = new CompositionTestRunner(duration, () => OnAppendData(testParameters.PointCount), fpsResult);
            }
            else
            {
                _testRunner = new DispatcherTimerRunner(duration, () => OnAppendData(testParameters.PointCount), fpsResult);
            }

            sciChart.Rendered += _testRunner.OnSciChartRendered;
            _testRunner.Run();
        }

        private void OnAppendData(int pointCount)
        {
            var next = _generator.GetRandomPoints(pointCount);

            _xyDataSeries = new XyDataSeries<double, double>(pointCount) { AcceptsUnsortedData = true, DataDistributionCalculator = d };
            _xyDataSeries.Append(next.XData, next.YData);

            scatterSeries.DataSeries = _xyDataSeries;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                sciChart.Dispose();
                
                _xyDataSeries?.Clear();
                _xyDataSeries = null;
                
                _testRunner.Dispose();
            }
        }
    }
}
