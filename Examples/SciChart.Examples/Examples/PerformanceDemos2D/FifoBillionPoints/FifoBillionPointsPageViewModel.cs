using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class FifoBillionPointsPageViewModel : BaseViewModel
    {
        private bool _isRunning;
        private bool _isStopped;
        private string _loadingMessage;

        private RenderSyncedTimer _timer;
        private PointCount _selectedPointCount;

        private const int AppendCount = 10_000; // The number of points to append per timer tick
        private const int TimerIntervalMs = 10; // Interval of timer tick 

        private readonly float[] _xBuffer = new float[AppendCount];
        private readonly float[] _yBuffer = new float[AppendCount];

        public FifoBillionPointsPageViewModel()
        {
            _isRunning = false;
            _isStopped = true;

            RunCommand = new ActionCommand(OnRun);
            PauseCommand = new ActionCommand(OnPause);
            StopCommand = new ActionCommand(OnStop);

            // Add the point count options 
            AllPointCounts.Add(new PointCount("1 Million", 5, 200_000));
            AllPointCounts.Add(new PointCount("5 Million", 5, 1_000_000));
            AllPointCounts.Add(new PointCount("10 Million", 5, 2_000_000));
            AllPointCounts.Add(new PointCount("50 Million", 5, 10_000_000));

            // If you have 8GB of RAM or more you can render 100M (will require just 1GB but to be safe...)
            if (SystemMemoryInfo.GetRamGb() >= 8)
            {
                AllPointCounts.Add(new PointCount("100 Million", 5, 20_000_000));
            }

            // Add further test cases depending on system RAM and 64/32bit status and how much RAM
            // 1 Billion points requires 8GB of free RAM or it will hit swap drive 
            if (Environment.Is64BitProcess && SystemMemoryInfo.GetRamGb() >= 16)
            {
                // Note: these point counts require the experimental VisualXccelerator.EnableImpossibleMode flag set to true on the chart 
                AllPointCounts.Add(new PointCount("500 Million", 5, 100_000_000));
                AllPointCounts.Add(new PointCount("1 Billion", 5, 200_000_000));
            }

            // Setup some warnings
            PerformanceWarnings = GetPerformanceWarnings();

            // Get ready to rock & roll 
            SelectedPointCount = AllPointCounts.Last();
        }

        public string PerformanceWarnings { get; }

        public bool HasWarnings => !string.IsNullOrEmpty(PerformanceWarnings);

        public ObservableCollection<IRenderableSeriesViewModel> Series { get; } = new ObservableCollection<IRenderableSeriesViewModel>();

        public ObservableCollection<PointCount> AllPointCounts { get; } = new ObservableCollection<PointCount>();

        public SurfaceViewportManager ViewportManager { get; } = new SurfaceViewportManager();

        public PointCount SelectedPointCount
        {
            get => _selectedPointCount;
            set
            {
                _selectedPointCount = value;
                OnPropertyChanged(nameof(SelectedPointCount));
            }
        }

        public ActionCommand RunCommand { get; }
        public ActionCommand PauseCommand { get; }
        public ActionCommand StopCommand { get; }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }

        public bool IsStopped
        {
            get => _isStopped;
            set
            {
                _isStopped = value;
                OnPropertyChanged(nameof(IsStopped));
            }
        }

        public bool IsLoading => !string.IsNullOrEmpty(LoadingMessage);

        public string LoadingMessage
        {
            get => _loadingMessage;
            set
            {
                _loadingMessage = value;

                OnPropertyChanged(nameof(LoadingMessage));
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private async void OnRun()
        {
            if (!IsRunning)
            {
                _timer ??= new RenderSyncedTimer(TimeSpan.FromMilliseconds(TimerIntervalMs),
                    ViewportManager.RenderSurface,
                    OnTimerTick);

                IsRunning = true;

                if (IsStopped)
                {
                    var seriesCount = SelectedPointCount.SeriesCount;
                    var pointCount = SelectedPointCount.PointsCount;

                    LoadingMessage = $"Generating {SelectedPointCount.DisplayName} Points...";

                    var series = await CreateSeriesAsync(seriesCount, pointCount);
                    using (ViewportManager.ParentSurface.SuspendUpdates())
                    {
                        series.ForEachDo(x => Series.Add(x));
                    }

                    LoadingMessage = null;
                }

                IsStopped = false;

                _timer.Start();
            }
        }

        private async Task<List<IRenderableSeriesViewModel>> CreateSeriesAsync(int seriesCount, int pointCount)
        {
            var seriesColors = new Color[]
            {
                ColorUtil.FromUInt(0xFF50C7E0),
                ColorUtil.FromUInt(0xFFF48420),
                ColorUtil.FromUInt(0xFF882B91),
                ColorUtil.FromUInt(0xFF30BC9A),
                ColorUtil.FromUInt(0xFFEC0F6C),
                ColorUtil.FromUInt(0xFF364BA0),
            };

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

                    var randomSeed = i * short.MaxValue;
                    var randomWalkGenerator = new Rand(randomSeed);

                    var xyDataSeries = new XyDataSeries<float, float>
                    {
                        // Required for scrolling / streaming 'first in first out' charts
                        FifoCapacity = pointCount,

                        Capacity = pointCount,

                        // Optional to improve performance when you know in advance whether 
                        // data is sorted ascending and contains float.NaN or not 
                        DataDistributionCalculator = new UserDefinedDistributionCalculator<float, float>
                        {
                            ContainsNaN = false,
                            IsEvenlySpaced = true,
                            IsSortedAscending = true,
                        },

                        // Just associate a random walk generator with the series for more consistent random generation
                        Tag = randomWalkGenerator
                    };

                    int yOffset = i * 2;
                    for (int j = 0; j < pointCount; j += AppendCount)
                    {
                        for (int k = 0; k < AppendCount; k++)
                        {
                            xBuffer[k] = j + k;
                            yBuffer[k] = randomWalkGenerator.NextWalk() + yOffset;
                        }

                        xyDataSeries.Append(xBuffer, yBuffer);
                    }

                    // Store the series 
                    series[i] = new LineRenderableSeriesViewModel
                    {
                        DataSeries = xyDataSeries,
                        Stroke = i >= seriesColors.Length ? GetRandomColor() : seriesColors[i]
                    };
                });

                return series.ToList();
            });
        }

        private static Color GetRandomColor()
        {
            return Color.FromRgb(Rand.NextByte(55), Rand.NextByte(55), Rand.NextByte(55));
        }

        private string GetPerformanceWarnings()
        {
#if DEBUG
            // Debug mode is the cause of all performance woes. Try release mode?
            var warnings = new List<string> { "Debug mode is slow, try Release." };
#else
            var warnings = new List<string>();
#endif
            if (Debugger.IsAttached)
            {
                // Its considerably slower to run the code when debugger is attached. Warn the user
                warnings.Add("Debugger is attached, try without.");
            }

            if (SystemMemoryInfo.GetRamGb() <= 8)
            {
                // Hmm, time to upgrade? :) 
                warnings.Add("Low system RAM, try on 16GB machine.");
            }

            return warnings.Any() ? "Performance warnings! " + string.Join(" ", warnings) : null;
        }

        private void OnPause()
        {
            IsRunning = false;
            IsStopped = false;

            _timer.Stop();
            ViewportManager.ZoomExtentsX();
        }

        private void OnStop()
        {
            if (!IsStopped)
            {
                _timer.Stop();

                IsRunning = false;
                IsStopped = true;

                Series.ForEachDo(x => x.DataSeries.Clear(true));
                Series.Clear();
            }

            // For example purposes, we're including GC.Collect. We don't recommend you do this in a production app
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        /// <summary>
        /// Free memory when the example is unloaded 
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnStop();
            }

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }

            _timer = null;
        }

        private void OnTimerTick()
        {
            using (ViewportManager.ParentSurface.SuspendUpdates())
            {
                int seriesIndex = 0;
                foreach (var series in Series)
                {
                    var dataSeries = (XyDataSeries<float, float>)series.DataSeries;
                    var randomWalkGenerator = (Rand)dataSeries.Tag;
                    var startIndex = (int)dataSeries.XValues.Last() + 1;

                    int yOffset = seriesIndex * 2;
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