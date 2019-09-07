

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.UI.Reactive;

namespace Fifo100MillionPointsDemo
{
    public class MainViewModel : BindableObject
    {
        private bool _isStopped;
        private string _loadingMessage;

        public MainViewModel()
        {
            _isStopped = true;
            RunCommand = new ActionCommand(OnRun, () => this.IsStopped);
            StopCommand = new ActionCommand(OnStop, () => this.IsStopped == false);
        }


        public ObservableCollection<IRenderableSeriesViewModel> Series { get; } = new ObservableCollection<IRenderableSeriesViewModel>();

        public ActionCommand RunCommand { get; }
        public ActionCommand StopCommand { get; }

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
            LoadingMessage = "Loading 50 Million Points...";
            IsStopped = false;

            // Load the points
            const int seriesCount = 5;
            const int pointCount = 10_000_000;
            var series = await CreateSeries(seriesCount, pointCount);
            Series.AddRange(series);

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

                        // Optional to improve performance when you know in advance whether 
                        // data is sorted ascending and contains float.NaN or not 
                        DataDistributionCalculator = new UserDefinedDistributionCalculator<float, float>()
                        {
                            ContainsNaN = false, 
                            IsEvenlySpaced = true, 
                            IsSortedAscending = true,
                        }
                    };

                    int yOffset = i + i;
                    for (int j = 0; j < pointCount; j++)
                    {
                        xyDataSeries.Append(j, Rand.Next() + yOffset);
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
            IsStopped = true;
            Series.Clear();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
    }
}
