using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic; 
using ChartProviders.Common;
using ChartProviders.Common.DataProviders;
using ChartProviders.Common.TestRunner;
using SciChart.Charting.Common.AttachedProperties;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Numerics;

namespace ChartProviderSciChart_Trunk
{
    /// <summary>
    /// Interaction logic for FifoLineSeriesSpeedTest.xaml
    /// </summary>
    public partial class FifoLineSeriesSpeedTest : UserControl, ISpeedTest
    {
        private static IDataDistributionCalculator<double, double> d = new UserDefinedDistributionCalculator<double, double>() { IsEvenlySpaced = true, IsSortedAscending = true, ContainsNaN = false}; 
        private XyDataSeries<double, double> _xyDataSeries;
        private RandomLinesGenerator _generator;
        private ITestRunner _testRunner;

        private const int SeriesCount = 4; //Added by Arction 
        private const int PointAddCount = 10000;

        //private const int PointAddCount = 1;
        private List<XyDataSeries<double, double>> _listSeries = new List<XyDataSeries<double, double>>(SeriesCount); 

		public FifoLineSeriesSpeedTest()
		{
            InitializeComponent();
		    this.sciChart.Loaded += (s, e) => { this.infoTextAnnotation.Text = string.Format("Renderer: {0}, Enable C++ Resamplers: {1}, Extreme Drawing: {2}", this.sciChart.RenderSurface.GetType().Name, PerformanceHelper.GetEnableExtremeResamplers(this.sciChart), PerformanceHelper.GetEnableExtremeDrawingManager(this.sciChart)); };
        }

        public FrameworkElement Element { get { return this; } }

        public void Execute(TestParameters testParameters, TimeSpan duration, Action<double> fpsResult)
        {
            _generator = new RandomLinesGenerator();
            var initialData = _generator.GetRandomLinesSeries(testParameters.PointCount);
            Random random = new Random(); 
            for (int i = 0; i < SeriesCount; i++)
            {
                // Setup
                _xyDataSeries = new XyDataSeries<double, double>() { FifoCapacity = testParameters.PointCount, DataDistributionCalculator = d };
                
                _listSeries.Add(_xyDataSeries);

                var rgb = new byte[3];
                random.NextBytes(rgb);
                _xyDataSeries.Append(initialData.XData, ScaleAndOffset(initialData.YData, 0.1, (double)i*0.2));
                var tmp = new SciChart.Charting.Visuals.RenderableSeries.FastLineRenderableSeries();
                tmp.DataSeries = _xyDataSeries;
                tmp.AntiAliasing = testParameters.AntiAliasing;
                tmp.Stroke  = System.Windows.Media.Color.FromArgb(255, rgb[0], rgb[1], rgb[2]);
                tmp.ResamplingMode = (ResamplingMode)Enum.Parse(typeof(ResamplingMode), testParameters.SamplingMode.ToString());
                sciChart.RenderableSeries.Add(tmp);
            }

            // Execute
           if (testParameters.TestRunner == TestRunnerType.Composition)
               _testRunner = new CompositionTestRunner(duration, OnAppendData, fpsResult);
           else
               _testRunner = new DispatcherTimerRunner(duration, OnAppendData, fpsResult);



           sciChart.Rendered += _testRunner.OnSciChartRendered;
            _testRunner.Run();
        }

        double[] ScaleAndOffset(IList<double>data,double factor, double offset)
        {
            int iLen = data.Count;
            double[] yArrayWithOffset = new double[iLen];
            for (int i = 0; i < iLen; i++)
            {
                yArrayWithOffset[i] = data[i] * factor + offset;
            }
            return yArrayWithOffset; 
        } 

        private void OnAppendData()
        {
            
            double[] xArray = new double[PointAddCount];
            double[] yArray = new double[PointAddCount];

            for (int i = 0; i < PointAddCount; i++)
            {
                var next = _generator.Next();
                xArray[i] = next.X;
                yArray[i] = next.Y;

            }

            for (int iSeries = 0; iSeries < SeriesCount; iSeries++)
            {
                _listSeries[iSeries].Append(xArray, ScaleAndOffset(yArray, 0.1, (double) iSeries * 0.2));
            }
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
