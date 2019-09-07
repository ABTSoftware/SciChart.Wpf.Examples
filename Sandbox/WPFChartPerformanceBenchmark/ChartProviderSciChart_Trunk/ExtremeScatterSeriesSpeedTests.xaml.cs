using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChartProviders.Common;
using ChartProviders.Common.DataProviders;
using ChartProviders.Common.TestRunner;
using SciChart.Charting.Common.AttachedProperties;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Data.Numerics;

namespace ChartProviderSciChart_Trunk
{
    /// <summary>
    /// Interaction logic for SciChartLineSeries.xaml
    /// </summary>
    public partial class SciChartExtremeScatterSeries : UserControl, ISpeedTest
    {
        private static IDataDistributionCalculator<double, double> d = new UserDefinedDistributionCalculator<double, double>() { IsEvenlySpaced = false, IsSortedAscending = false, ContainsNaN = false}; 
        private XyDataSeries<double, double> _xyDataSeries;
        private RandomPointsGenerator _generator;
        private ITestRunner _testRunner;

        public SciChartExtremeScatterSeries()
        {
            InitializeComponent();
            this.sciChart.Loaded += (s, e) => { this.infoTextAnnotation.Text = string.Format("Renderer: {0}, C++ Resampler: {1}, Extreme Drawing: {2}, Extreme Scatter: {3}", this.sciChart.RenderSurface.GetType().Name, PerformanceHelper.GetEnableExtremeResamplers(this.sciChart), PerformanceHelper.GetEnableExtremeDrawingManager(this.sciChart), true); };
        }

        public FrameworkElement Element { get { return this; } }

        public void Execute(TestParameters testParameters, TimeSpan duration, Action<double> fpsResult)
        {
            // Setup
            _xyDataSeries = new XyDataSeries<double, double>() { AcceptsUnsortedData = true, DataDistributionCalculator = d};
            this.scatterSeries.DataSeries = _xyDataSeries;
            this.scatterSeries.AntiAliasing = testParameters.AntiAliasing;
            this.scatterSeries.PaletteProvider = new TestPaletteProvider();

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

    public class TestPaletteProvider : IExtremePointMarkerPaletteProvider
    {
        private readonly Values<Color> _colors = new Values<Color>();

        public Values<Color> Colors { get { return _colors; } }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
            var dataSeriesCount = rSeries.DataSeries.Count;
            _colors.Count = dataSeriesCount;
            var colors = _colors.Items;
            var constantColor = System.Windows.Media.Colors.Red;
            for (int i = 0; i < dataSeriesCount; i++)
            {
                colors[i] = constantColor;
            }
        }
    }
}
