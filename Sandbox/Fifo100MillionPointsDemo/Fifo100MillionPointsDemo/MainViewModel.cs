

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting2D.Interop;
using SciChart.Data.Model;
using SciChart.UI.Reactive;

namespace Fifo100MillionPointsDemo
{
    public class MainViewModel : BindableObject
    {
        private bool _isStopped;
        private string _loadingMessage;
        private readonly NoLockTimer _timer;
        const int AppendCount = 10_000;
        const int TimerIntervalMs = 10;

        // Temporary buffers for improving the performance of loading and appending data
        // see https://www.scichart.com/documentation/v5.x/webframe.html#Performance_Tips_&_Tricks.html for why
        readonly float[] _xBuffer = new float[AppendCount];
        readonly float[] _yBuffer = new float[AppendCount];
        private PointCountViewModel _selectedPointCount;

        public MainViewModel()
        {
            _isStopped = true;
            _timer = new NoLockTimer(TimeSpan.FromMilliseconds(TimerIntervalMs), OnTimerTick);
            RunCommand = new ActionCommand(OnRun, () => this.IsStopped);
            StopCommand = new ActionCommand(OnStop, () => this.IsStopped == false);
            AllPointCounts.AddRange(new []
            {
                new PointCountViewModel("1 Million", 5, 200_000),
                new PointCountViewModel("5 Million", 5, 1_000_000),
                new PointCountViewModel("10 Million", 5, 2_000_000),
                new PointCountViewModel("50 Million", 5, 10_000_000),
                new PointCountViewModel("100 Million", 5, 20_000_000),
            });
            SelectedPointCount = AllPointCounts.Last();
        }


        public ObservableCollection<IRenderableSeriesViewModel> Series { get; } = new ObservableCollection<IRenderableSeriesViewModel>();

        public ObservableCollection<PointCountViewModel> AllPointCounts { get; } = new ObservableCollection<PointCountViewModel>();

        public PointCountViewModel SelectedPointCount
        {
            get => _selectedPointCount;
            set
            {
                _selectedPointCount = value;
                OnPropertyChanged("SelectedPointCount");
            }
        }
        
        public ActionCommand RunCommand { get; }
        public ActionCommand StopCommand { get; }

        public DefaultViewportManager ViewportManager { get; } = new DefaultViewportManager();

        public bool IsStopped
        {
            get => _isStopped;
            set
            {
                _isStopped = value;
                OnPropertyChanged("IsStopped");
                RunCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
            }
        }

        public string LoadingMessage
        {
            get => _loadingMessage;
            set
            {
                _loadingMessage = value;
                OnPropertyChanged("LoadingMessage");
            }
        }

        private async void OnRun()
        {
            int seriesCount = SelectedPointCount.SeriesCount;
            int pointCount = SelectedPointCount.PointCount;
            LoadingMessage = $"Loading {SelectedPointCount.DisplayName} Points...";
            IsStopped = false;

            // Load the points
            var series = await CreateSeries(seriesCount, pointCount);
            Series.AddRange(series);

            _timer.Start();

            LoadingMessage = null;
        }

        private async Task<List<IRenderableSeriesViewModel>> CreateSeries(int seriesCount, int pointCount)
        {
            return await Task.Run(() =>
            {
                // Create N series of M points async. Return to calling code to set on the chart 
                List<IRenderableSeriesViewModel> series = new List<IRenderableSeriesViewModel>();
                for (int i = 0; i < seriesCount; i++)
                {
                    var xyDataSeries = new XyDataSeries<float, float>()
                    {
                        // Required for scrolling / streaming 'first in first out' charts
                        FifoCapacity = pointCount,

                        Capacity = pointCount,

                        // Optional to improve performance when you know in advance whether 
                        // data is sorted ascending and contains float.NaN or not 
                        // see https://www.scichart.com/documentation/v5.x/webframe.html#Performance_Tips_&_Tricks.html for why
                        DataDistributionCalculator = new UserDefinedDistributionCalculator<float, float>()
                        {
                            ContainsNaN = false, 
                            IsEvenlySpaced = true, 
                            IsSortedAscending = true,
                        }
                    };

                    int yOffset = i + i;
                    for (int j = 0; j < pointCount; j += AppendCount)
                    {
                        for (int k = 0; k < AppendCount; k++)
                        {
                            _xBuffer[k] = j+k;
                            _yBuffer[k] = Rand.Next() + yOffset;
                        }
                        // Append blocks of 10k points for performance
                        // see https://www.scichart.com/documentation/v5.x/webframe.html#Performance_Tips_&_Tricks.html for why
                        xyDataSeries.Append(_xBuffer, _yBuffer);
                    }
                    
                    series.Add(new LineRenderableSeriesViewModel()
                    {
                        DataSeries = xyDataSeries,
                        Stroke = Colors.RandomColor()
                    });
                }

                return series;
            });
        }

        private void OnStop()
        {
            lock (Series)
            {
                _timer.Stop();
                IsStopped = true;
                Series.Clear();
            }
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        private void OnTimerTick()
        {
            lock (Series)
            {
                // Freeze updates on scichart UI 
                using (ViewportManager.SuspendUpdates())
                {
                    int seriesIndex = 0;
                    foreach (var series in Series)
                    {
                        var dataSeries = (XyDataSeries<float, float>) series.DataSeries;
                        int startIndex = (int) dataSeries.XValues.Last() + 1;

                        // Append new points in blocks of AppendCount 
                        // see https://www.scichart.com/documentation/v5.x/webframe.html#Performance_Tips_&_Tricks.html for why
                        int yOffset = seriesIndex + seriesIndex;
                        for (int i = 0, j = startIndex; i < AppendCount; i++, j++)
                        {
                            _xBuffer[i] = j;
                            _yBuffer[i] = Rand.Next() + yOffset;
                        }
                        dataSeries.Append(_xBuffer, _yBuffer);

                        seriesIndex++;
                    }
                }
            }
        }
    }
}
