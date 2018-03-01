using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ChartProviders.Common;
using ChartProviders.Common.DataProviders;
using ChartProviders.Common.TestRunner;
using SciChart.Charting.Common.AttachedProperties;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Data.Model;

namespace ChartProviderSciChart_Trunk
{
    /// <summary>
    /// Interaction logic for Load500x500SeriesRefreshTest.xaml
    /// </summary>
    public partial class Load500x500SeriesRefreshTest : UserControl, ISpeedTest, INotifyPropertyChanged
    {
        private static IDataDistributionCalculator<double, double> d = new UserDefinedDistributionCalculator<double, double>() { IsEvenlySpaced = true, IsSortedAscending = true, ContainsNaN = false}; 
        private IEnumerable<IDataSeries> _dataSeries;
        private ITestRunner _testRunner;

        public Load500x500SeriesRefreshTest()
        {
            InitializeComponent();
     
            this.sciChart.Loaded += (s, e) => { this.infoTextAnnotation.Text = string.Format("Renderer: {0}, Enable C++ Resamplers: {1}, Extreme Drawing: {2}", this.sciChart.RenderSurface.GetType().Name, PerformanceHelper.GetEnableExtremeResamplers(this.sciChart), PerformanceHelper.GetEnableExtremeDrawingManager(this.sciChart)); };
        }

        public string RenderSurface { get; set; }

        public FrameworkElement Element { get { return this; } }

        public IEnumerable<IDataSeries> DataSeries
        {
            get { return _dataSeries; }
            set
            {
                if (_dataSeries == value) return;
                _dataSeries = value;
                OnPropertyChanged("DataSeries");
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

                // Run test (just refresh)
                //int inc = 1;
            }

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Restart();
      
            if (testParameters.TestRunner == TestRunnerType.Composition)
                _testRunner = new CompositionTestRunner(duration, () =>
                {

                    // Make small changes to the YAxis Visible Range to trigger an update and cause some rescaling
                    //var currentRange = this.sciChart.YAxis.VisibleRange.AsDoubleRange();
                    
                    double dY = (double)sw.ElapsedMilliseconds/1000.0;
                    this.sciChart.YAxis.VisibleRange = new DoubleRange(-40 - dY, 40 + dY); 
                    //this.sciChart.YAxis.VisibleRange.SetMinMax(-1 - dY, 1 + dY); 
                    //inc = -inc;
                }, fpsResult);
            else
                _testRunner = new DispatcherTimerRunner(duration, () =>
                {

                    // Make small changes to the YAxis Visible Range to trigger an update and cause some rescaling
                    //var currentRange = this.sciChart.YAxis.VisibleRange.AsDoubleRange();
                    //this.sciChart.YAxis.VisibleRange = new DoubleRange(currentRange.Min - RangeIncrement, currentRange.Max + RangeIncrement);
                    
                    //If not setting large scale enough, SciChart crashes randomly. (-30 ... 30) is not enough 
                    double dY = (double)sw.ElapsedMilliseconds / 1000.0;
                    this.sciChart.YAxis.VisibleRange = new DoubleRange(-40 - dY, 40 + dY); 
                    //inc = -inc;
                }, fpsResult);


            sciChart.Rendered += _testRunner.OnSciChartRendered;
            _testRunner.Run();
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _testRunner.Dispose();

            if (DataSeries != null)
            {
                foreach (var item in DataSeries)
                {
                    ((XyDataSeries<double, double>)item).Clear();
                }
            }

            this.sciChart.Dispose();

            DataSeries = null;
        }
    }
}
