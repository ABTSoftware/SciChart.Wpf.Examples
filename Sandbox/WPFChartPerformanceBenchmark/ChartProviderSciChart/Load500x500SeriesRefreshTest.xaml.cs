using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Abt.Controls.SciChart;
using ChartProviderSciChart_v1_7_2.Annotations;
using ChartProviders.Common;
using ChartProviders.Common.DataProviders;
using ChartProviders.Common.TestRunner;

namespace ChartProviderSciChart_v1_7_2
{
    /// <summary>
    /// Interaction logic for Load500x500SeriesRefreshTest.xaml
    /// </summary>
    public partial class Load500x500SeriesRefreshTest : UserControl, ISpeedTest, INotifyPropertyChanged
    {
        private IEnumerable<IDataSeries> _dataSeries;
        private DispatcherTimerRunner _testRunner;

        public Load500x500SeriesRefreshTest()
        {
            InitializeComponent();

            this.DataContext = this;
        }

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
                var dataSeries = new XyDataSeries<double, double>();
                dataSeries.Append(xyData[i].XData, xyData[i].YData);
                allDataSeries[i] = dataSeries;
            }
            DataSeries = allDataSeries;
            
            // Run test (just refresh)
            _testRunner = new DispatcherTimerRunner(duration, () => this.sciChart.InvalidateElement(), fpsResult);
            sciChart.Rendered += _testRunner.OnSciChartRendered;
            _testRunner.Run();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            if (DataSeries != null)
            {
                foreach (var item in DataSeries)
                {
                    ((XyDataSeries<double, double>)item).Clear();
                }
            }
            _testRunner.Dispose();
            DataSeries = null;
        }
    }
}
