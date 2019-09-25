

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using Fifo100MillionPointsDemo.HelperClasses;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting2D.Interop;
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using SciChart.UI.Reactive;
using Colors = Fifo100MillionPointsDemo.HelperClasses.Colors;

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

            // Add the point count options 
            AllPointCounts.AddRange(new []
            {
                new PointCountViewModel("1 Million", 5, 200_000),
                new PointCountViewModel("5 Million", 5, 1_000_000),
                new PointCountViewModel("10 Million", 5, 2_000_000),
                new PointCountViewModel("50 Million", 5, 10_000_000),
            });

            // If you have 8GB of RAM or more you can render 100M (will require just 1GB but to be safe...)
            if (SysInfo.GetRamGb() >= 8)
                AllPointCounts.Add(new PointCountViewModel("100 Million", 5, 20_000_000));

            SelectedPointCount = AllPointCounts.Last();

            // Add further test cases depending on system RAM and 64/32bit status and how much RAM
            // 1 Billion points requires 8GB of free RAM or it will hit swap drive 
            if (Environment.Is64BitProcess && SysInfo.GetRamGb() >= 16)
            {
                // Note: these point counts require the experimental VisualXccelerator.EnableImpossibleMode flag set to true on the chart 
                AllPointCounts.Add(new PointCountViewModel("500 Million", 5, 100_000_000));
                AllPointCounts.Add(new PointCountViewModel("1 Bazillion", 5, 200_000_000));
            }

            // Setup some warnings
            PerformanceWarnings = GetPerformanceWarnings();
        }

        public string PerformanceWarnings { get; }

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
            LoadingMessage = $"Generating {SelectedPointCount.DisplayName} Points...";
            IsStopped = false;

            // Load the points
            var series = await CreateSeries(seriesCount, pointCount);
            using (ViewportManager.SuspendUpdates())
            {
                Series.AddRange(series);
            }

            _timer.Start();

            LoadingMessage = null;
        }

        private static async Task<List<IRenderableSeriesViewModel>> CreateSeries(int seriesCount, int pointCount)
        {
            return await Task.Run(() =>
            {
                // Create N series of M points async. Return to calling code to set on the chart 
                IRenderableSeriesViewModel[] series = new IRenderableSeriesViewModel[seriesCount];

                // We generate data in parallel as just generating 1,000,000,000 points takes a long time no matter how fast your chart is! 
                Parallel.For(0, seriesCount, i =>
                {
                    // Temporary buffer for fast filling of DataSeries
                    var xBuffer = new float[AppendCount];
                    var yBuffer = new float[AppendCount];

                    int randomSeed = i * short.MaxValue;
                    var randomWalkGenerator = new Rand(randomSeed);
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
                        },

                        // Just associate a random walk generator with the series for more consistent random generation
                        Tag = randomWalkGenerator,
                    };

                    int yOffset = i + i;
                    for (int j = 0; j < pointCount; j += AppendCount)
                    {
                        for (int k = 0; k < AppendCount; k++)
                        {
                            xBuffer[k] = j + k;
                            yBuffer[k] = randomWalkGenerator.NextWalk() + yOffset;
                        }

                        // Append blocks of 10k points for performance
                        // see https://www.scichart.com/documentation/v5.x/webframe.html#Performance_Tips_&_Tricks.html for why
                        xyDataSeries.Append(xBuffer, yBuffer);
                    }

                    // Store the series 
                    series[i] = new LineRenderableSeriesViewModel()
                    {
                        DataSeries = xyDataSeries,
                        Stroke = Colors.RandomColor()
                    };
                });

                // Force a GC Collect before we begin
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

                return series.ToList();
            });
        }

        private string GetPerformanceWarnings()
        {
            List<string> warnings = new List<string>();
#if DEBUG
            // Debug mode is the cause of all performance woes. Try release mode?
            warnings.Add("Debug mode is slow, try Release");
#endif
            if (Debugger.IsAttached)
            {
                // Its considerably slower to run the code when debugger is attached. Warn the user
                warnings.Add("Debugger is attached, try without");
            }

            if (SysInfo.GetRamGb() <= 8)
            {
                // Hmm, time to upgrade? https://www.amazon.co.uk/s?k=16GB+DDR4+RAM :) 
                warnings.Add("Low system RAM, try on 16GB machine?");
            }

            return warnings.Any() ? "Perf warnings! " + string.Join(". ", warnings) : null;
        }

        private void OnStop()
        {
            lock (Series)
            {
                _timer.Stop();
                IsStopped = true;
                Series.ForEachDo(x => x.DataSeries.FifoCapacity = 1);
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
                        var randomWalkGenerator = (Rand)dataSeries.Tag;
                        int startIndex = (int) dataSeries.XValues.Last() + 1;

                        // Append new points in blocks of AppendCount 
                        // see https://www.scichart.com/documentation/v5.x/webframe.html#Performance_Tips_&_Tricks.html for why
                        int yOffset = seriesIndex + seriesIndex;
                        for (int i = 0, j = startIndex; i < AppendCount; i++, j++)
                        {
                            _xBuffer[i] = j;
                            _yBuffer[i] = randomWalkGenerator.NextWalk() + yOffset;
                        }
                        dataSeries.Append(_xBuffer, _yBuffer);

                        seriesIndex++;
                    }
                }
            }
        }
    }
}
