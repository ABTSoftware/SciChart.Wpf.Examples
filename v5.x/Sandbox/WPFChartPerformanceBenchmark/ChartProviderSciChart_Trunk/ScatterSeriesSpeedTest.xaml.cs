using System;
using System.Windows;
using System.Windows.Controls;
using ChartProviders.Common;
using ChartProviders.Common.DataProviders;
using ChartProviders.Common.TestRunner;
using SciChart.Charting.Common.AttachedProperties;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Data.Numerics;

namespace ChartProviderSciChart_Trunk
{
    /// <summary>
    /// Interaction logic for SciChartLineSeries.xaml
    /// </summary>
    public partial class SciChartScatterSeries : UserControl, ISpeedTest
    {
        private static IDataDistributionCalculator<double, double> d = new UserDefinedDistributionCalculator<double, double>() { IsEvenlySpaced = false, IsSortedAscending = false, ContainsNaN = false}; 
        private XyDataSeries<double, double> _xyDataSeries;
        private RandomPointsGenerator _generator;
        private ITestRunner _testRunner;

        public SciChartScatterSeries()
        {
            InitializeComponent();
            this.sciChart.Loaded += (s, e) => { this.infoTextAnnotation.Text = string.Format("Renderer: {0}, Enable C++ Resamplers: {1}, Extreme Drawing: {2}", this.sciChart.RenderSurface.GetType().Name, PerformanceHelper.GetEnableExtremeResamplers(this.sciChart), PerformanceHelper.GetEnableExtremeDrawingManager(this.sciChart)); };
        }

        public FrameworkElement Element { get { return this; } }

        public void Execute(TestParameters testParameters, TimeSpan duration, Action<double> fpsResult)
        {
            // Setup
            _xyDataSeries = new XyDataSeries<double, double>() { AcceptsUnsortedData = true, DataDistributionCalculator = d};
            this.scatterSeries.DataSeries = _xyDataSeries;
            this.scatterSeries.AntiAliasing = testParameters.AntiAliasing;
//            if (testParameters.SamplingMode == Resampling.Cluster2DInRenderableSeries)
//                this.scatterSeries.DoClusterResampling = true;
//            else
//            {
//                 ResamplingMode resamplingMode; if (!Enum.TryParse(testParameters.SamplingMode.ToString(), out resamplingMode)) resamplingMode = ResamplingMode.None;
//                this.scatterSeries.ResamplingMode = resamplingMode;
//            }

            

            _generator = testParameters.DataDistribution == DataDistribution.Uniform ? (RandomPointsGenerator)new BrownianMotionPointsGenerator(0, 100, -50, 50) : new CorrelatedDataPointsGenerator(0, 100, -50, 50);
			

            sciChart.XAxis.VisibleRange = new DoubleRange(0, 100);
            sciChart.YAxis.VisibleRange = new DoubleRange(-50, 50);
            var initialData = _generator.GetRandomPoints(testParameters.PointCount);
           _xyDataSeries.Append(initialData.XData, initialData.YData);

            // Execute
           if (testParameters.TestRunner == TestRunnerType.Composition)
               _testRunner = new CompositionTestRunner(duration, () => OnAppendData(testParameters.PointCount), fpsResult);
           else
               _testRunner = new DispatcherTimerRunner(duration, () => OnAppendData(testParameters.PointCount), fpsResult);

            sciChart.Rendered += _testRunner.OnSciChartRendered;
            _testRunner.Run();
        }

        private void OnAppendData(int pointCount)
        {
            var next = _generator.GetRandomPoints(pointCount);
            _xyDataSeries = new XyDataSeries<double, double>(pointCount) { AcceptsUnsortedData = true, DataDistributionCalculator = d};
            _xyDataSeries.Append(next.XData, next.YData);
            scatterSeries.DataSeries = _xyDataSeries;
        }

        public void Dispose()
        {
            this.sciChart.Dispose();
            if (_xyDataSeries != null)
            {
                _xyDataSeries.Clear();
                _xyDataSeries = null;
            }

            _testRunner.Dispose();
        }
    }
}
