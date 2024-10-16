using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class Load500x500SeriesSpeedTest : UserControl, ISpeedTest, INotifyPropertyChanged
    {       
        private IEnumerable<IDataSeries> _dataSeries;
        private ITestRunner _testRunner;

        private static IDataDistributionCalculator<double, double> d = new UserDefinedDistributionCalculator<double, double>()
        {
            IsEvenlySpaced = true,
            IsSortedAscending = true,
            ContainsNaN = false
        };

        public Load500x500SeriesSpeedTest()
        {
            InitializeComponent();
            
            sciChart.Loaded += (s, e) =>
            {
                infoTextAnnotation.Text = string.Format("Renderer: {0}, Enable C++ Resamplers: {1}, Extreme Drawing: {2}", sciChart.RenderSurface.GetType().Name, PerformanceHelper.GetEnableExtremeResamplers(sciChart), PerformanceHelper.GetEnableExtremeDrawingManager(sciChart));
            };
        }

        public string RenderSurface { get; set; }

        public FrameworkElement Element => this;

        public IEnumerable<IDataSeries> DataSeries
        {
            get => _dataSeries;
            set
            {
                if (_dataSeries != value)
                {
                    _dataSeries = value;
                    OnPropertyChanged(nameof(DataSeries));
                }
            }
        }

        public void Execute(TestParameters testParameters, TimeSpan duration, Action<double> fpsResult)
        {
            int pointCount = testParameters.PointCount;
            int seriesCount = testParameters.PointCount;

            DataSeries = null;

            using (sciChart.SuspendUpdates())
            {
                // Generate Data and mark time                 
                XyData[] xyData = new XyData[seriesCount];
                var random = new Random();
                for (int i = 0; i < seriesCount; i++)
                {
                    var generator = new RandomWalkGenerator(random.Next(0, int.MaxValue));
                    xyData[i] = generator.GetRandomWalkSeries(pointCount);
                }

                // Append to SciChartSurface 
                var allDataSeries = new IDataSeries[seriesCount];
                for (int i = 0; i < seriesCount; i++)
                {
                    var dataSeries = new XyDataSeries<double, double>() {DataDistributionCalculator = d};
                    dataSeries.Append(xyData[i].XData, xyData[i].YData);
                    allDataSeries[i] = dataSeries;
                }

                DataSeries = allDataSeries;

                LineSeriesSource.SetStrokeThickness(sciChart, testParameters.StrokeThickness);
                LineSeriesSource.SetAntiAliasing(sciChart, testParameters.AntiAliasing);
                LineSeriesSource.SetDataSeries(sciChart, allDataSeries);

                sciChart.ZoomExtents();
            }

            var sw = new Stopwatch();
            sw.Restart();

            if (testParameters.TestRunner == TestRunnerType.Composition)
            {
                _testRunner = new CompositionTestRunner(duration, () =>
                {
                    // Make small changes to the YAxis Visible Range to trigger an update and cause some rescaling
                    // var currentRange = this.sciChart.YAxis.VisibleRange.AsDoubleRange();

                    double dY = sw.ElapsedMilliseconds / 1000.0;
                    sciChart.YAxis.VisibleRange = new DoubleRange(-40 - dY, 40 + dY);

                }, fpsResult);
            }
            else
            {
                _testRunner = new DispatcherTimerRunner(duration, () =>
                {
                    // Make small changes to the YAxis Visible Range to trigger an update and cause some rescaling
                    // var currentRange = this.sciChart.YAxis.VisibleRange.AsDoubleRange();
                    // this.sciChart.YAxis.VisibleRange = new DoubleRange(currentRange.Min - RangeIncrement, currentRange.Max + RangeIncrement);

                    // If not setting large scale enough, SciChart crashes randomly. (-30 ... 30) is not enough 
                    double dY = sw.ElapsedMilliseconds / 1000.0;
                    sciChart.YAxis.VisibleRange = new DoubleRange(-40 - dY, 40 + dY);

                }, fpsResult);
            }

            sciChart.Rendered += _testRunner.OnSciChartRendered;
            _testRunner.Run();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                _testRunner.Dispose();

                if (DataSeries != null)
                {
                    foreach (var item in DataSeries)
                    {
                        ((XyDataSeries<double, double>)item).Clear();
                    }
                }

                sciChart.Dispose();

                DataSeries = null;
            }
        }
    }
}
