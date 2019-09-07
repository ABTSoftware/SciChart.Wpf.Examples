using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Abt.Controls.SciChart.Numerics;
using ChartProviders.Common;
using Abt.Controls.SciChart;
using ChartProviders.Common.DataProviders;
using ChartProviders.Common.TestRunner;

namespace ChartProviderSciChart_v1_7_2
{
    public partial class FifoLineSeriesSpeedTest : UserControl, ISpeedTest
    {
        private XyDataSeries<double, double> _xyDataSeries;
        private RandomLinesGenerator _generator;
        private ITestRunner _testRunner;

        public FifoLineSeriesSpeedTest()
        {
            InitializeComponent();
        }

        public FrameworkElement Element { get { return this; } }

        public void Execute(TestParameters testParameters, TimeSpan duration, Action<double> fpsResult)
        {
            // Setup
            var dataset = new DataSeriesSet<double, double>();
            _xyDataSeries = new XyDataSeries<double, double>() { FifoCapacity = testParameters.PointCount };
            dataset.Add(_xyDataSeries);
            sciChart.DataSet = dataset;

            this.lineSeries.AntiAliasing = testParameters.AntiAliasing;
            ResamplingMode resamplingMode;
            if (!Enum.TryParse(testParameters.SamplingMode.ToString(), out resamplingMode)) resamplingMode = ResamplingMode.MinMax;
            this.lineSeries.ResamplingMode = resamplingMode; 

            _generator = new RandomLinesGenerator();
            var initialData = _generator.GetRandomLinesSeries(testParameters.PointCount);
           _xyDataSeries.Append(initialData.XData, initialData.YData);

            // Execute
           _testRunner = new DispatcherTimerRunner(duration, OnAppendData, fpsResult);
           sciChart.Rendered += _testRunner.OnSciChartRendered;
            _testRunner.Run();
        }

        private void OnAppendData()
        {
            var next = _generator.Next();
            _xyDataSeries.Append(next.X, next.Y);
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
